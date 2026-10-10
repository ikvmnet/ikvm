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

        const BindingFlags DeclaredOnly = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        readonly Assembly source;
        readonly Universe target;
        readonly AssemblyBuilder assembly;
        readonly ModuleBuilder module;
        readonly Dictionary<Type, TypeBuilder> types = new();
        readonly Dictionary<Type, GenericTypeParameterBuilder> genericParameters = new();
        readonly Dictionary<MethodBase, MethodBuilder> methods = new();
        readonly Dictionary<FieldInfo, FieldBuilder> fields = new();
        readonly Dictionary<PropertyInfo, PropertyBuilder> properties = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source">The assembly to copy.</param>
        /// <param name="target">The universe to define the copy in.</param>
        /// <param name="directory">The directory to save the copy to.</param>
        public FixtureCopier(Assembly source, Universe target, string directory)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.target = target ?? throw new ArgumentNullException(nameof(target));

            var name = source.GetName();
            assembly = target.DefineDynamicAssembly(new AssemblyName(name.Name) { Version = name.Version }, AssemblyBuilderAccess.Save, directory);
            module = assembly.DefineDynamicModule(source.ManifestModule.ScopeName, source.ManifestModule.ScopeName, false);
        }

        /// <summary>
        /// Copies the assembly and saves it, returning the file name.
        /// </summary>
        /// <returns></returns>
        public string Copy()
        {
            var all = source.GetTypes();

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

            foreach (var a in source.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    assembly.SetCustomAttribute(cab);

            foreach (var a in source.ManifestModule.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    module.SetCustomAttribute(cab);

            foreach (var t in source.ManifestModule.__GetExportedTypes())
                assembly.__AddTypeForwarder(Map(t));

            // enclosing types must be created before their nested types, and base types before derived types
            var created = new HashSet<Type>();
            foreach (var t in all)
                Create(t, created);

            var fileName = source.ManifestModule.ScopeName;
            assembly.Save(fileName);
            return fileName;
        }

        /// <summary>
        /// The builders cannot emit function pointer types; the importer never does.
        /// </summary>
        static bool UsesFunctionPointers(MethodInfo method) => method.ReturnType.IsFunctionPointer || method.GetParameters().Any(i => i.ParameterType.IsFunctionPointer);

        static int Depth(Type type) => type.DeclaringType is { } d ? Depth(d) + 1 : 0;

        void DefineType(Type type)
        {
            var attributes = type.Attributes;
            var b = type.DeclaringType is { } declaring ? types[declaring].DefineNestedType(type.Name, attributes) : module.DefineType(type.FullName, attributes);
            types.Add(type, b);

            if (type.IsGenericTypeDefinition)
            {
                var parameters = type.GetGenericArguments();
                var builders = b.DefineGenericParameters(parameters.Select(i => i.Name).ToArray());
                for (int i = 0; i < parameters.Length; i++)
                    genericParameters.Add(parameters[i], builders[i]);
            }
        }

        void DefineHierarchy(Type type)
        {
            var b = types[type];
            if (type.BaseType != null)
                b.SetParent(Map(type.BaseType));

            foreach (var i in type.__GetDeclaredInterfaces())
                b.AddInterfaceImplementation(Map(i));

            if (type.IsGenericTypeDefinition)
                foreach (var p in type.GetGenericArguments())
                    DefineConstraints(p);
        }

        void DefineConstraints(Type parameter)
        {
            var b = genericParameters[parameter];
            b.SetGenericParameterAttributes(parameter.GenericParameterAttributes);
            foreach (var c in parameter.GetGenericParameterConstraints())
                if (c.IsInterface == false)
                    b.SetBaseTypeConstraint(Map(c));
        }

        void DefineMembers(Type type)
        {
            var b = types[type];

            foreach (var f in type.GetFields(DeclaredOnly))
            {
                var fb = b.DefineField(f.Name, Map(f.FieldType), Map(f.GetRequiredCustomModifiers()), Map(f.GetOptionalCustomModifiers()), f.Attributes);
                if (f.IsLiteral)
                    fb.SetConstant(f.GetRawConstantValue());
                if (f.__TryGetFieldOffset(out var offset))
                    fb.SetOffset(offset);
                fields.Add(f, fb);
            }

            foreach (var c in type.GetConstructors(DeclaredOnly))
                methods.Add(c, DefineMethod(b, c));

            foreach (var m in type.GetMethods(DeclaredOnly))
                if (UsesFunctionPointers(m) == false)
                    methods.Add(m, DefineMethod(b, m));

            foreach (var p in type.GetProperties(DeclaredOnly))
            {
                var parameters = p.GetIndexParameters();
                var pb = b.DefineProperty(p.Name, p.Attributes, Map(p.PropertyType), Map(p.GetRequiredCustomModifiers()), Map(p.GetOptionalCustomModifiers()), parameters.Select(i => Map(i.ParameterType)).ToArray(), parameters.Select(i => Map(i.GetRequiredCustomModifiers())).ToArray(), parameters.Select(i => Map(i.GetOptionalCustomModifiers())).ToArray());
                if (p.GetGetMethod(true) is { } get)
                    pb.SetGetMethod(methods[get]);
                if (p.GetSetMethod(true) is { } set)
                    pb.SetSetMethod(methods[set]);
                properties.Add(p, pb);
            }
        }

        /// <summary>
        /// Defines a method or constructor the way the importer does: constructors are methods named .ctor or .cctor.
        /// </summary>
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
                    genericParameters.Add(gps[i], builders[i]);
                foreach (var gp in gps)
                    DefineConstraints(gp);
            }

            var returnParameter = method is MethodInfo m ? m.ReturnParameter : null;
            mb.SetSignature(
                Map(returnParameter?.ParameterType ?? target.Import(typeof(void))),
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

        static void EmitBody(MethodBase method, MethodBuilder mb)
        {
            if (method.IsAbstract || (method.Attributes & MethodAttributes.PinvokeImpl) != 0 || (method.GetMethodImplementationFlags() & (MethodImplAttributes.Runtime | MethodImplAttributes.InternalCall)) != 0)
                return;

            var g = mb.GetILGenerator();
            g.Emit(OpCodes.Ldnull);
            g.Emit(OpCodes.Throw);
        }

        void DefineOverrides(Type type)
        {
            var map = type.__GetMethodImplMap();
            for (int i = 0; i < map.MethodDeclarations.Length; i++)
                foreach (var declaration in map.MethodDeclarations[i])
                    types[type].DefineMethodOverride((MethodInfo)MapMethod(declaration), methods[map.MethodBodies[i]]);
        }

        void SetCustomAttributes(Type type)
        {
            var b = types[type];
            foreach (var a in type.GetCustomAttributesData())
                if (Attribute(a) is { } cab)
                    b.SetCustomAttribute(cab);

            foreach (var f in type.GetFields(DeclaredOnly))
                foreach (var a in f.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        fields[f].SetCustomAttribute(cab);

            foreach (var p in type.GetProperties(DeclaredOnly))
                foreach (var a in p.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        properties[p].SetCustomAttribute(cab);

            foreach (var m in type.GetConstructors(DeclaredOnly).Cast<MethodBase>().Concat(type.GetMethods(DeclaredOnly).Where(i => UsesFunctionPointers(i) == false)))
            {
                foreach (var a in m.GetCustomAttributesData())
                    if (Attribute(a) is { } cab)
                        methods[m].SetCustomAttribute(cab);

                var parameters = m is MethodInfo mi ? m.GetParameters().Prepend(mi.ReturnParameter) : m.GetParameters();
                foreach (var p in parameters)
                {
                    var attributes = p.GetCustomAttributesData().Select(Attribute).Where(i => i != null).ToList();
                    if (attributes.Count == 0)
                        continue;

                    var pb = methods[m].DefineParameter(p.Position + 1, p.Attributes & ~ParameterAttributes.HasDefault, p.Name);
                    foreach (var cab in attributes)
                        pb.SetCustomAttribute(cab);
                }
            }
        }

        void Create(Type type, HashSet<Type> created)
        {
            if (created.Add(type) == false)
                return;

            if (type.DeclaringType is { } declaring)
                Create(declaring, created);
            if (type.BaseType is { } baseType && baseType.Assembly == source)
                Create(baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType, created);

            types[type].CreateType();
        }

        /// <summary>
        /// Rebuilds a custom attribute for the copy, or returns <c>null</c> for attributes the copy carries as metadata
        /// flags instead.
        /// </summary>
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
        object Value(CustomAttributeTypedArgument argument, Type declaredType)
        {
            var actualType = Map(argument.ArgumentType);
            if (declaredType == target.Import(typeof(object)) && actualType != declaredType)
                return CustomAttributeBuilder.__MakeTypedArgument(actualType, Value(argument, actualType));

            switch (argument.Value)
            {
                case IList<CustomAttributeTypedArgument> items:
                    var elementType = declaredType.GetElementType();
                    var values = items.Select(i => Value(i, elementType)).ToArray();
                    if (elementType == target.Import(typeof(object)))
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

        static System.Type ClrType(Type type)
        {
            if (type.IsEnum)
                type = type.GetEnumUnderlyingType();

            return type.FullName == "System.Type" ? typeof(Type) : System.Type.GetType(type.FullName, true);
        }

        Type[] Map(Type[] types) => types.Select(Map).ToArray();

        Type[][] Map(Type[][] types) => types.Select(Map).ToArray();

        /// <summary>
        /// Maps a type of the source universe to the corresponding type of the target universe.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type Map(Type type)
        {
            if (type == null)
                return null;
            if (type.IsGenericParameter)
                return genericParameters[type];
            if (type.IsByRef)
                return Map(type.GetElementType()).MakeByRefType();
            if (type.IsPointer)
                return Map(type.GetElementType()).MakePointerType();
            if (type.IsArray)
                return type.Name.EndsWith("[]", StringComparison.Ordinal) ? Map(type.GetElementType()).MakeArrayType() : Map(type.GetElementType()).MakeArrayType(type.GetArrayRank());
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
                return Map(type.GetGenericTypeDefinition()).MakeGenericType(Map(type.GetGenericArguments()));
            if (type.Assembly == source)
                return types[type];
            if (type.DeclaringType is { } declaring)
                return Map(declaring).GetNestedType(type.Name, BindingFlags.Public | BindingFlags.NonPublic);

            return target.Load(type.Assembly.FullName).GetType(type.FullName, true);
        }

        PropertyInfo MapProperty(PropertyInfo property)
        {
            if (properties.TryGetValue(property, out var pb))
                return pb;

            return Map(property.DeclaringType).GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        FieldInfo MapField(FieldInfo field)
        {
            if (fields.TryGetValue(field, out var fb))
                return fb;

            return Map(field.DeclaringType).GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Maps a method of the source universe to the corresponding method of the target universe.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        MethodBase MapMethod(MethodBase method)
        {
            if (methods.TryGetValue(method, out var mapped))
                return method is ConstructorInfo ? mapped.__AsConstructorInfo() : mapped;

            var declaring = Map(method.DeclaringType);

            // members of generic instances of copied types are bound through the builder
            if (method.DeclaringType.IsGenericType && method.DeclaringType.IsGenericTypeDefinition == false && method.DeclaringType.GetGenericTypeDefinition().Assembly == source)
            {
                var definition = method.Module.ResolveMethod(method.MetadataToken);
                return method is ConstructorInfo ? TypeBuilder.GetConstructor(declaring, methods[definition].__AsConstructorInfo()) : TypeBuilder.GetMethod(declaring, methods[definition]);
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
