using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Tests.Emit
{

    /// <summary>
    /// Re-emits an assembly read by IKVM.Reflection through the IKVM.Reflection builders, using only the builder APIs IKVM
    /// uses. Method bodies are replaced by <c>ldnull; throw</c>, since only metadata is compared. The source and target
    /// live in different universes, so the copy can keep the identity of the source; types are mapped by name.
    /// </summary>
    sealed class FixtureCopier
    {

        /// <summary>
        /// Binds every member declared by a type itself, whatever its visibility and whether it is static.
        /// </summary>
        const BindingFlags DeclaredOnly = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        readonly Assembly _source;
        readonly Universe _target;
        readonly AssemblyBuilder _assembly;
        readonly ModuleBuilder _module;
        readonly Dictionary<Type, TypeBuilder> _types = new();
        readonly Dictionary<Type, GenericTypeParameterBuilder> _genericParameters = new();
        readonly Dictionary<MethodBase, MethodBuilder> _methods = new();
        readonly Dictionary<FieldInfo, FieldBuilder> _fields = new();
        readonly Dictionary<PropertyInfo, PropertyBuilder> _properties = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source">The assembly to copy.</param>
        /// <param name="target">The universe to define the copy in.</param>
        /// <param name="directory">The directory to save the copy to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
        public FixtureCopier(Assembly source, Universe target, string directory)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));

            var name = source.GetName();
            _assembly = target.DefineDynamicAssembly(new AssemblyName(name.Name) { Version = name.Version }, AssemblyBuilderAccess.Save, directory);
            _module = _assembly.DefineDynamicModule(source.ManifestModule.ScopeName, source.ManifestModule.ScopeName, false);
        }

        /// <summary>
        /// Copies the assembly and saves it, returning the file name.
        /// </summary>
        /// <returns>The file name of the saved copy, relative to the directory given to the constructor.</returns>
        public string Copy()
        {
            var all = _source.GetTypes();

            // define every type first, outermost first, so references between them can be mapped
            foreach (var t in all.OrderBy(Depth))
                DefineType(t);

            foreach (var t in all)
                DefineHierarchy(t);

            foreach (var t in all)
                DefineMembers(t);

            foreach (var t in all)
                DefineOverrides(t);

            foreach (var t in all)
                SetCustomAttributes(t);

            foreach (var a in _source.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    _assembly.SetCustomAttribute(cab);

            foreach (var a in _source.ManifestModule.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    _module.SetCustomAttribute(cab);

            foreach (var t in _source.ManifestModule.__GetExportedTypes())
                _assembly.__AddTypeForwarder(Map(t));

            // enclosing types must be created before their nested types, and base types before derived types
            var created = new HashSet<Type>();
            foreach (var t in all)
                Create(t, created);

            var fileName = _source.ManifestModule.ScopeName;
            _assembly.Save(fileName);
            return fileName;
        }

        /// <summary>
        /// Gets whether the signature of a method uses function pointer types. Such methods are left out of the copy,
        /// because the builders cannot emit function pointer types; the importer never does.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns><see langword="true"/> if the return type or a parameter type is a function pointer.</returns>
        static bool UsesFunctionPointers(MethodInfo method) => method.ReturnType.IsFunctionPointer || method.GetParameters().Any(i => i.ParameterType.IsFunctionPointer);

        /// <summary>
        /// Gets the nesting depth of a type, zero for a top-level type.
        /// </summary>
        /// <param name="type">The type to measure.</param>
        /// <returns>The number of types that enclose <paramref name="type"/>.</returns>
        static int Depth(Type type) => type.DeclaringType is { } d ? Depth(d) + 1 : 0;

        /// <summary>
        /// Defines the builder for a type, nested in the builder of its enclosing type, along with its generic parameters.
        /// </summary>
        /// <param name="type">The source type, whose enclosing type must already be defined.</param>
        void DefineType(Type type)
        {
            var attributes = type.Attributes;
            var b = type.DeclaringType is { } declaring ? _types[declaring].DefineNestedType(type.Name, attributes) : _module.DefineType(type.FullName, attributes);
            _types.Add(type, b);

            if (type.IsGenericTypeDefinition)
            {
                var parameters = type.GetGenericArguments();
                var builders = b.DefineGenericParameters(parameters.Select(i => i.Name).ToArray());
                for (int i = 0; i < parameters.Length; i++)
                    _genericParameters.Add(parameters[i], builders[i]);
            }
        }

        /// <summary>
        /// Sets the base type, the directly declared interfaces and the generic parameter constraints of the copy of a type.
        /// </summary>
        /// <param name="type">The source type.</param>
        void DefineHierarchy(Type type)
        {
            var b = _types[type];
            if (type.BaseType != null)
                b.SetParent(Map(type.BaseType));

            foreach (var i in type.__GetDeclaredInterfaces())
                b.AddInterfaceImplementation(Map(i));

            if (type.IsGenericTypeDefinition)
                foreach (var p in type.GetGenericArguments())
                    DefineConstraints(p);
        }

        /// <summary>
        /// Sets the attributes and the base type constraint of the copy of a generic parameter. Interface constraints are
        /// not copied.
        /// </summary>
        /// <param name="parameter">The source generic parameter, whose builder must already be defined.</param>
        void DefineConstraints(Type parameter)
        {
            var b = _genericParameters[parameter];
            b.SetGenericParameterAttributes(parameter.GenericParameterAttributes);
            foreach (var c in parameter.GetGenericParameterConstraints())
                if (c.IsInterface == false)
                    b.SetBaseTypeConstraint(Map(c));
        }

        /// <summary>
        /// Defines the fields, constructors, methods and properties a type declares, recording each builder so references
        /// to the member can be mapped.
        /// </summary>
        /// <param name="type">The source type.</param>
        void DefineMembers(Type type)
        {
            var b = _types[type];

            foreach (var f in type.GetFields(DeclaredOnly))
            {
                var fb = b.DefineField(f.Name, Map(f.FieldType), Map(f.GetRequiredCustomModifiers()), Map(f.GetOptionalCustomModifiers()), f.Attributes);
                if (f.IsLiteral)
                    fb.SetConstant(f.GetRawConstantValue());
                if (f.__TryGetFieldOffset(out var offset))
                    fb.SetOffset(offset);
                _fields.Add(f, fb);
            }

            foreach (var c in type.GetConstructors(DeclaredOnly))
                _methods.Add(c, DefineMethod(b, c));

            foreach (var m in type.GetMethods(DeclaredOnly))
                if (UsesFunctionPointers(m) == false)
                    _methods.Add(m, DefineMethod(b, m));

            foreach (var p in type.GetProperties(DeclaredOnly))
            {
                var parameters = p.GetIndexParameters();
                var pb = b.DefineProperty(p.Name, p.Attributes, Map(p.PropertyType), Map(p.GetRequiredCustomModifiers()), Map(p.GetOptionalCustomModifiers()), parameters.Select(i => Map(i.ParameterType)).ToArray(), parameters.Select(i => Map(i.GetRequiredCustomModifiers())).ToArray(), parameters.Select(i => Map(i.GetOptionalCustomModifiers())).ToArray());
                if (p.GetGetMethod(true) is { } get)
                    pb.SetGetMethod(_methods[get]);
                if (p.GetSetMethod(true) is { } set)
                    pb.SetSetMethod(_methods[set]);
                _properties.Add(p, pb);
            }
        }

        /// <summary>
        /// Defines a method or constructor the way the importer does: constructors are methods named .ctor or .cctor.
        /// </summary>
        /// <param name="type">The builder of the declaring type.</param>
        /// <param name="method">The source method or constructor.</param>
        /// <returns>The new method builder.</returns>
        MethodBuilder DefineMethod(TypeBuilder type, MethodBase method)
        {
            var parameters = method.GetParameters();
            var mb = type.DefineMethod(method.Name, method.Attributes, method.CallingConvention, null, null);

            // generic parameters must exist before the signature that uses them is set
            if (method.IsGenericMethodDefinition)
            {
                var gps = method.GetGenericArguments();
                var builders = mb.DefineGenericParameters(gps.Select(i => i.Name).ToArray());
                for (int i = 0; i < gps.Length; i++)
                    _genericParameters.Add(gps[i], builders[i]);
                foreach (var gp in gps)
                    DefineConstraints(gp);
            }

            var returnParameter = method is MethodInfo m ? m.ReturnParameter : null;
            mb.SetSignature(
                Map(returnParameter?.ParameterType ?? _target.Import(typeof(void))),
                Map(returnParameter?.GetRequiredCustomModifiers() ?? []),
                Map(returnParameter?.GetOptionalCustomModifiers() ?? []),
                Map(parameters.Select(i => i.ParameterType).ToArray()),
                Map(parameters.Select(i => i.GetRequiredCustomModifiers()).ToArray()),
                Map(parameters.Select(i => i.GetOptionalCustomModifiers()).ToArray()));
            mb.SetImplementationFlags(method.GetMethodImplementationFlags());

            foreach (var p in parameters)
                if (p.Name != null || p.Attributes != ParameterAttributes.None)
                    mb.DefineParameter(p.Position + 1, p.Attributes & ~ParameterAttributes.HasDefault, p.Name);

            EmitBody(method, mb);
            return mb;
        }

        /// <summary>
        /// Emits a <c>ldnull; throw</c> body for a method that has a body in the source, and nothing for abstract,
        /// P/Invoke, runtime-implemented and internal call methods.
        /// </summary>
        /// <param name="method">The source method.</param>
        /// <param name="mb">The builder of its copy.</param>
        static void EmitBody(MethodBase method, MethodBuilder mb)
        {
            if (method.IsAbstract || (method.Attributes & MethodAttributes.PinvokeImpl) != 0 || (method.GetMethodImplementationFlags() & (MethodImplAttributes.Runtime | MethodImplAttributes.InternalCall)) != 0)
                return;

            var g = mb.GetILGenerator();
            g.Emit(OpCodes.Ldnull);
            g.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Copies the explicit method overrides (MethodImpl entries) of a type.
        /// </summary>
        /// <param name="type">The source type.</param>
        void DefineOverrides(Type type)
        {
            var map = type.__GetMethodImplMap();
            for (int i = 0; i < map.MethodDeclarations.Length; i++)
                foreach (var declaration in map.MethodDeclarations[i])
                    _types[type].DefineMethodOverride(_methods[map.MethodBodies[i]], (MethodInfo)MapMethod(declaration));
        }

        /// <summary>
        /// Copies the custom attributes of a type and of its fields, properties, methods, constructors and parameters,
        /// including the return parameter.
        /// </summary>
        /// <param name="type">The source type.</param>
        void SetCustomAttributes(Type type)
        {
            var b = _types[type];
            foreach (var a in type.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    b.SetCustomAttribute(cab);

            foreach (var f in type.GetFields(DeclaredOnly))
                foreach (var a in f.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        _fields[f].SetCustomAttribute(cab);

            foreach (var p in type.GetProperties(DeclaredOnly))
                foreach (var a in p.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        _properties[p].SetCustomAttribute(cab);

            foreach (var m in type.GetConstructors(DeclaredOnly).Cast<MethodBase>().Concat(type.GetMethods(DeclaredOnly).Where(i => UsesFunctionPointers(i) == false)))
            {
                foreach (var a in m.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        _methods[m].SetCustomAttribute(cab);

                var parameters = m is MethodInfo mi ? m.GetParameters().Prepend(mi.ReturnParameter) : m.GetParameters();
                foreach (var p in parameters)
                {
                    var attributes = p.GetCustomAttributesData().Select(Attribute).Where(i => i != null).ToList();
                    if (attributes.Count == 0)
                        continue;

                    var pb = _methods[m].DefineParameter(p.Position + 1, p.Attributes & ~ParameterAttributes.HasDefault, p.Name);
                    foreach (var cab in attributes)
                        pb.SetCustomAttribute(cab);
                }
            }
        }

        /// <summary>
        /// Creates the copy of a type, first creating its enclosing type and its base type when that is copied too.
        /// </summary>
        /// <param name="type">The source type.</param>
        /// <param name="created">The source types already created, or being created.</param>
        void Create(Type type, HashSet<Type> created)
        {
            if (created.Add(type) == false)
                return;

            if (type.DeclaringType is { } declaring)
                Create(declaring, created);
            if (type.BaseType is { } baseType && baseType.Assembly == _source)
                Create(baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType, created);

            _types[type].CreateType();
        }

        /// <summary>
        /// Rebuilds a custom attribute for the copy, or returns <see langword="null"/> for attributes the copy carries as
        /// metadata flags instead.
        /// </summary>
        /// <param name="data">The custom attribute read from the source.</param>
        /// <returns>The builder for the attribute, or <see langword="null"/> for a pseudo-custom attribute.</returns>
        CustomAttributeBuilder Attribute(CustomAttributeData data)
        {
            var type = data.Constructor.DeclaringType;
            if (type.FullName is "System.SerializableAttribute" or "System.NonSerializedAttribute" or "System.Runtime.InteropServices.ComImportAttribute" or "System.Runtime.InteropServices.PreserveSigAttribute" or "System.Runtime.InteropServices.FieldOffsetAttribute" or "System.Runtime.InteropServices.StructLayoutAttribute" or "System.Runtime.InteropServices.InAttribute" or "System.Runtime.InteropServices.OutAttribute" or "System.Runtime.InteropServices.OptionalAttribute" or "System.Runtime.CompilerServices.MethodImplAttribute" or "System.Runtime.CompilerServices.SpecialNameAttribute")
                return null;

            var constructor = (ConstructorInfo)MapMethod(data.Constructor);
            var parameterTypes = constructor.GetParameters().Select(i => i.ParameterType).ToArray();
            var args = data.ConstructorArguments.Select((a, i) => Value(a, parameterTypes[i])).ToArray();
            var namedProperties = data.NamedArguments.Where(i => i.IsField == false).Select(i => (Member: MapProperty((PropertyInfo)i.MemberInfo), i.TypedValue)).ToList();
            var namedFields = data.NamedArguments.Where(i => i.IsField).Select(i => (Member: MapField((FieldInfo)i.MemberInfo), i.TypedValue)).ToList();

            return new CustomAttributeBuilder(
                constructor,
                args,
                namedProperties.Select(i => i.Member).ToArray(),
                namedProperties.Select(i => Value(i.TypedValue, i.Member.PropertyType)).ToArray(),
                namedFields.Select(i => i.Member).ToArray(),
                namedFields.Select(i => Value(i.TypedValue, i.Member.FieldType)).ToArray());
        }

        /// <summary>
        /// Converts a custom attribute value read from the source to the form <see cref="CustomAttributeBuilder"/> takes
        /// for a value of the given declared type: a typed argument where the declared type is <see cref="object"/>,
        /// otherwise the raw value, with arrays as CLR arrays and types mapped to the target universe.
        /// </summary>
        /// <param name="argument">The value read from the source.</param>
        /// <param name="declaredType">The declared type of the parameter, property or field the value is for.</param>
        /// <returns>The value in the form <see cref="CustomAttributeBuilder"/> takes.</returns>
        object Value(CustomAttributeTypedArgument argument, Type declaredType)
        {
            var actualType = Map(argument.ArgumentType);
            if (declaredType == _target.Import(typeof(object)) && actualType != declaredType)
                return CustomAttributeBuilder.__MakeTypedArgument(actualType, Value(argument, actualType));

            switch (argument.Value)
            {
                case IList<CustomAttributeTypedArgument> items:
                    var elementType = declaredType.GetElementType();
                    var values = items.Select(i => Value(i, elementType)).ToArray();
                    if (elementType == _target.Import(typeof(object)))
                        return values;

                    var array = Array.CreateInstance(ClrType(elementType), values.Length);
                    for (int i = 0; i < values.Length; i++)
                        array.SetValue(values[i], i);
                    return array;
                case Type t:
                    return Map(t);
                default:
                    return argument.Value;
            }
        }

        /// <summary>
        /// Gets the runtime type that holds values of the given attribute value type, used to create arrays of the right
        /// element type. Enums are held as their underlying type, and <c>System.Type</c> as IKVM.Reflection's
        /// <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The element type, in the target universe.</param>
        /// <returns>The runtime type for array elements.</returns>
        static System.Type ClrType(Type type)
        {
            if (type.IsEnum)
                type = type.GetEnumUnderlyingType();

            return type.FullName == "System.Type" ? typeof(Type) : System.Type.GetType(type.FullName, true);
        }

        /// <summary>
        /// Maps each type of the source universe to the corresponding type of the target universe.
        /// </summary>
        /// <param name="types">The types to map.</param>
        /// <returns>The mapped types, in the same order.</returns>
        Type[] Map(Type[] types) => types.Select(Map).ToArray();

        /// <summary>
        /// Maps each array of types of the source universe, such as the custom modifiers of each parameter, to the target
        /// universe.
        /// </summary>
        /// <param name="types">The arrays of types to map.</param>
        /// <returns>The mapped arrays, in the same order.</returns>
        Type[][] Map(Type[][] types) => types.Select(Map).ToArray();

        /// <summary>
        /// Maps a type of the source universe to the corresponding type of the target universe.
        /// </summary>
        /// <param name="type">The source type, or <see langword="null"/>.</param>
        /// <returns>The target type, or <see langword="null"/> if <paramref name="type"/> is <see langword="null"/>.</returns>
        Type Map(Type type)
        {
            if (type == null)
                return null;
            if (type.IsGenericParameter)
                return _genericParameters[type];
            if (type.IsByRef)
                return Map(type.GetElementType()).MakeByRefType();
            if (type.IsPointer)
                return Map(type.GetElementType()).MakePointerType();
            if (type.IsArray)
                return type.Name.EndsWith("[]", StringComparison.Ordinal) ? Map(type.GetElementType()).MakeArrayType() : Map(type.GetElementType()).MakeArrayType(type.GetArrayRank());
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
                return Map(type.GetGenericTypeDefinition()).MakeGenericType(Map(type.GetGenericArguments()));
            if (type.Assembly == _source)
                return _types[type];
            if (type.DeclaringType is { } declaring)
                return Map(declaring).GetNestedType(type.Name, BindingFlags.Public | BindingFlags.NonPublic);

            return _target.Load(type.Assembly.FullName).GetType(type.FullName, true);
        }

        /// <summary>
        /// Maps a property of the source universe, such as one named in a custom attribute, to the target universe.
        /// </summary>
        /// <param name="property">The source property.</param>
        /// <returns>The builder of the copied property, or the matching instance property of the mapped declaring type.</returns>
        PropertyInfo MapProperty(PropertyInfo property)
        {
            if (_properties.TryGetValue(property, out var pb))
                return pb;

            return Map(property.DeclaringType).GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Maps a field of the source universe, such as one named in a custom attribute, to the target universe.
        /// </summary>
        /// <param name="field">The source field.</param>
        /// <returns>The builder of the copied field, or the matching instance field of the mapped declaring type.</returns>
        FieldInfo MapField(FieldInfo field)
        {
            if (_fields.TryGetValue(field, out var fb))
                return fb;

            return Map(field.DeclaringType).GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Maps a method of the source universe to the corresponding method of the target universe.
        /// </summary>
        /// <param name="method">The source method or constructor.</param>
        /// <returns>The target method or constructor, matched by name and parameter types when it is not copied.</returns>
        MethodBase MapMethod(MethodBase method)
        {
            if (_methods.TryGetValue(method, out var mapped))
                return method is ConstructorInfo ? mapped.__AsConstructorInfo() : mapped;

            var declaring = Map(method.DeclaringType);

            // members of generic instances of copied types are bound through the builder
            if (method.DeclaringType.IsGenericType && method.DeclaringType.IsGenericTypeDefinition == false && method.DeclaringType.GetGenericTypeDefinition().Assembly == _source)
            {
                var definition = method.Module.ResolveMethod(method.MetadataToken);
                return method is ConstructorInfo ? TypeBuilder.GetConstructor(declaring, _methods[definition].__AsConstructorInfo()) : TypeBuilder.GetMethod(declaring, _methods[definition]);
            }

            var parameterTypes = method.GetParameters().Select(i => i.ParameterType).ToArray();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            var candidates = method is ConstructorInfo
                ? declaring.GetConstructors(flags).Cast<MethodBase>()
                : declaring.GetMethods(flags).Where(i => i.Name == method.Name);

            return candidates.Single(i => i.GetParameters().Length == parameterTypes.Length && i.IsGenericMethodDefinition == method.IsGenericMethodDefinition && i.GetParameters().Select(p => p.ParameterType.ToString()).SequenceEqual(parameterTypes.Select(p => p.ToString())));
        }

    }

}
