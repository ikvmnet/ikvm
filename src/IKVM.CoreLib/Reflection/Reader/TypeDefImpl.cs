/*
  Copyright (C) 2009-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a type defined in the TypeDef table of a module that was read from a file.
    /// </summary>
    sealed class TypeDefImpl : TypeInfo
    {

        readonly ModuleReader _module;
        readonly TypeDefinitionHandle _handle;
        readonly TypeDefinition _definition;
        readonly string _typeName;
        readonly string _typeNamespace;

        Type[] _typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module that defines the type.</param>
        /// <param name="handle">The handle of the type's TypeDef row.</param>
        internal TypeDefImpl(ModuleReader module, TypeDefinitionHandle handle)
        {
            _module = module;
            _handle = handle;
            _definition = module.Metadata.GetTypeDefinition(handle);
            _typeName = module.GetString(_definition.Name);
            _typeNamespace = module.GetString(_definition.Namespace);
            MarkKnownType(_typeNamespace, _typeName);
        }

        /// <summary>
        /// Gets the type from which the current type directly inherits, or <see langword="null"/> if the type has no
        /// base type, as is the case for <see cref="object"/>, interfaces and the &lt;Module&gt; pseudo type.
        /// </summary>
        public override Type BaseType => _definition.BaseType.IsNil ? null : _module.ResolveType(MetadataTokens.GetToken(_definition.BaseType), this);

        /// <inheritdoc />
        public override TypeAttributes Attributes => (TypeAttributes)_definition.Attributes;

        /// <inheritdoc />
        public override EventInfo[] __GetDeclaredEvents()
        {
            var handles = _definition.GetEvents();
            var events = new EventInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                events[i++] = new EventInfoImpl(_module, this, h);

            return events;
        }

        /// <inheritdoc />
        public override FieldInfo[] __GetDeclaredFields()
        {
            var handles = _definition.GetFields();
            var fields = new FieldInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                fields[i++] = _module.GetFieldAt(this, h);

            return fields;
        }

        /// <inheritdoc />
        public override Type[] __GetDeclaredInterfaces()
        {
            var handles = _definition.GetInterfaceImplementations();
            if (handles.Count == 0)
                return Type.EmptyTypes;

            var interfaces = new Type[handles.Count];
            var i = 0;
            foreach (var h in handles)
                interfaces[i++] = _module.ResolveType(MetadataTokens.GetToken(_module.Metadata.GetInterfaceImplementation(h).Interface), this);

            return interfaces;
        }

        /// <inheritdoc />
        public override MethodBase[] __GetDeclaredMethods()
        {
            var handles = _definition.GetMethods();
            var methods = new MethodBase[handles.Count];
            var i = 0;
            foreach (var h in handles)
                methods[i++] = _module.GetMethodAt(this, h);

            return methods;
        }

        /// <inheritdoc />
        public override __MethodImplMap __GetMethodImplMap()
        {
            PopulateGenericArguments();

            var bodies = new List<MethodInfo>();
            var declarations = new List<List<MethodInfo>>();
            var index = new Dictionary<MethodInfo, int>();
            foreach (var h in _definition.GetMethodImplementations())
            {
                var impl = _module.Metadata.GetMethodImplementation(h);
                var body = (MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(impl.MethodBody), _typeArgs, null);
                if (index.TryGetValue(body, out var i) == false)
                {
                    index.Add(body, i = bodies.Count);
                    bodies.Add(body);
                    declarations.Add([]);
                }

                declarations[i].Add((MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(impl.MethodDeclaration), _typeArgs, null));
            }

            var map = new __MethodImplMap
            {
                TargetType = this,
                MethodBodies = bodies.ToArray(),
                MethodDeclarations = new MethodInfo[declarations.Count][]
            };
            for (var i = 0; i < map.MethodDeclarations.Length; i++)
                map.MethodDeclarations[i] = declarations[i].ToArray();

            return map;
        }

        /// <inheritdoc />
        public override Type[] __GetDeclaredTypes()
        {
            var handles = _definition.GetNestedTypes();
            if (handles.Length == 0)
                return Type.EmptyTypes;

            var types = new Type[handles.Length];
            for (var i = 0; i < types.Length; i++)
                types[i] = _module.ResolveType(MetadataTokens.GetToken(handles[i]));

            return types;
        }

        /// <inheritdoc />
        public override PropertyInfo[] __GetDeclaredProperties()
        {
            var handles = _definition.GetProperties();
            var properties = new PropertyInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                properties[i++] = new PropertyInfoImpl(_module, this, h);

            return properties;
        }

        /// <inheritdoc />
        internal override TypeName TypeName => new(_typeNamespace, _typeName);

        /// <summary>
        /// Gets the name of the type, escaped as in a type name string.
        /// </summary>
        public override string Name => TypeNameParser.Escape(_typeName);

        /// <inheritdoc />
        public override string FullName => GetFullName();

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <summary>
        /// Returns the generic type parameters of the type definition.
        /// </summary>
        /// <returns>A copy of the array of generic type parameters, or an empty array if the type is not generic.</returns>
        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(_typeArgs);
        }

        /// <summary>
        /// Creates the generic type parameters of the type definition on first use.
        /// </summary>
        void PopulateGenericArguments()
        {
            if (_typeArgs == null)
            {
                var handles = _definition.GetGenericParameters();
                var args = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
                for (var i = 0; i < args.Length; i++)
                    args[i] = new GenericTypeParameter(_module, handles[i], Signature.ELEMENT_TYPE_VAR);

                _typeArgs = args;
            }
        }

        /// <inheritdoc />
        internal override Type GetGenericTypeArgument(int index)
        {
            PopulateGenericArguments();
            return _typeArgs[index];
        }

        /// <summary>
        /// Returns the custom modifiers of the generic arguments. The generic parameters of a type definition have no
        /// custom modifiers, so every element is empty.
        /// </summary>
        /// <returns>An array of empty custom modifiers, one for each generic type parameter.</returns>
        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            PopulateGenericArguments();
            return new CustomModifiers[_typeArgs.Length];
        }

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type. A type definition is generic exactly
        /// when it is a generic type definition.
        /// </summary>
        public override bool IsGenericType => IsGenericTypeDefinition;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => _definition.GetGenericParameters().Count > 0;

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a generic type definition from which the current generic
        /// type can be constructed.
        /// </summary>
        /// <returns>The current type, which is its own generic type definition.</returns>
        /// <exception cref="InvalidOperationException">The current type is not a generic type.</exception>
        public override Type GetGenericTypeDefinition() => IsGenericTypeDefinition ? (Type)this : throw new InvalidOperationException();

        /// <summary>
        /// Returns the full name of the type followed by its generic type parameters in brackets, if it has any.
        /// </summary>
        /// <returns>A string that represents the current type.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(FullName);
            var sep = "[";

            foreach (var arg in GetGenericArguments())
            {
                sb.Append(sep);
                sb.Append(arg);
                sep = ",";
            }

            if (sep != "[")
                sb.Append(']');

            return sb.ToString();
        }

        /// <summary>
        /// Gets a value indicating whether the type is nested, determined from its visibility flags alone, so that it
        /// can be evaluated without resolving the declaring type.
        /// </summary>
        internal bool IsNestedByFlags => (Attributes & TypeAttributes.VisibilityMask & ~TypeAttributes.Public) != 0;

        /// <summary>
        /// Gets the type that declares the current nested type, or <see langword="null"/> if the type is not nested.
        /// </summary>
        /// <exception cref="InvalidOperationException">The type has nested visibility but no NestedClass row.</exception>
        public override Type DeclaringType
        {
            get
            {
                // we cannot use Type.IsNested for this, because that calls DeclaringType
                if (!IsNestedByFlags)
                    return null;

                var declaring = _definition.GetDeclaringType();
                if (declaring.IsNil)
                    throw new InvalidOperationException();

                return _module.ResolveType(MetadataTokens.GetToken(declaring), null, null);
            }
        }

        /// <inheritdoc />
        public override Module Module => _module;

        /// <summary>
        /// Gets a value indicating whether this is the &lt;Module&gt; pseudo type, which is always the first row of the
        /// TypeDef table.
        /// </summary>
        internal override bool IsModulePseudoType => MetadataTokens.GetRowNumber(_handle) == 1;

        /// <summary>
        /// Gets a value indicating whether the type is complete. A type read from a file is always complete.
        /// </summary>
        internal override bool IsBaked => true;

        /// <summary>
        /// Gets a value indicating whether the type is a value type: it derives from <see cref="ValueType"/> or
        /// <see cref="Enum"/> and is not one of those two types itself. The result is cached in the type flags.
        /// </summary>
        protected override bool IsValueTypeImpl
        {
            get
            {
                var baseType = BaseType;
                if (baseType != null && baseType.IsEnumOrValueType && !IsEnumOrValueType)
                {
                    typeFlags |= TypeFlags.ValueType;
                    return true;
                }
                else
                {
                    typeFlags |= TypeFlags.NotValueType;
                    return false;
                }
            }
        }

    }

}
