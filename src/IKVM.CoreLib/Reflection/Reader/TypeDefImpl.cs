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
        /// <param name="module"></param>
        /// <param name="handle"></param>
        internal TypeDefImpl(ModuleReader module, TypeDefinitionHandle handle)
        {
            this._module = module;
            this._handle = handle;
            this._definition = module.Metadata.GetTypeDefinition(handle);
            this._typeName = module.GetString(_definition.Name);
            this._typeNamespace = module.GetString(_definition.Namespace);
            MarkKnownType(_typeNamespace, _typeName);
        }

        public override Type BaseType => _definition.BaseType.IsNil ? null : _module.ResolveType(MetadataTokens.GetToken(_definition.BaseType), this);

        public override TypeAttributes Attributes => (TypeAttributes)_definition.Attributes;

        public override EventInfo[] __GetDeclaredEvents()
        {
            var handles = _definition.GetEvents();
            var events = new EventInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                events[i++] = new EventInfoImpl(_module, this, h);

            return events;
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            var handles = _definition.GetFields();
            var fields = new FieldInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                fields[i++] = _module.GetFieldAt(this, h);

            return fields;
        }

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

        public override MethodBase[] __GetDeclaredMethods()
        {
            var handles = _definition.GetMethods();
            var methods = new MethodBase[handles.Count];
            var i = 0;
            foreach (var h in handles)
                methods[i++] = _module.GetMethodAt(this, h);

            return methods;
        }

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
                    declarations.Add(new List<MethodInfo>());
                }

                declarations[i].Add((MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(impl.MethodDeclaration), _typeArgs, null));
            }

            var map = new __MethodImplMap();
            map.TargetType = this;
            map.MethodBodies = bodies.ToArray();
            map.MethodDeclarations = new MethodInfo[declarations.Count][];
            for (var i = 0; i < map.MethodDeclarations.Length; i++)
                map.MethodDeclarations[i] = declarations[i].ToArray();

            return map;
        }

        public override Type[] __GetDeclaredTypes()
        {
            var handles = _definition.GetNestedTypes();
            if (handles.Length == 0)
                return Type.EmptyTypes;

            var types = new Type[handles.Length];
            for (int i = 0; i < types.Length; i++)
                types[i] = _module.ResolveType(MetadataTokens.GetToken(handles[i]));

            return types;
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            var handles = _definition.GetProperties();
            var properties = new PropertyInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                properties[i++] = new PropertyInfoImpl(_module, this, h);

            return properties;
        }

        internal override TypeName TypeName => new TypeName(_typeNamespace, _typeName);

        public override string Name => TypeNameParser.Escape(_typeName);

        public override string FullName => GetFullName();

        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(_typeArgs);
        }

        void PopulateGenericArguments()
        {
            if (_typeArgs == null)
            {
                var handles = _definition.GetGenericParameters();
                var args = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
                for (int i = 0; i < args.Length; i++)
                    args[i] = new GenericTypeParameter(_module, handles[i], Signature.ELEMENT_TYPE_VAR);

                _typeArgs = args;
            }
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            PopulateGenericArguments();
            return _typeArgs[index];
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            PopulateGenericArguments();
            return new CustomModifiers[_typeArgs.Length];
        }

        public override bool IsGenericType => IsGenericTypeDefinition;

        public override bool IsGenericTypeDefinition => _definition.GetGenericParameters().Count > 0;

        public override Type GetGenericTypeDefinition()
        {
            return IsGenericTypeDefinition ? (Type)this : throw new InvalidOperationException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder(this.FullName);
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

        internal bool IsNestedByFlags => (Attributes & TypeAttributes.VisibilityMask & ~TypeAttributes.Public) != 0;

        public override Type DeclaringType
        {
            get
            {
                // note that we cannot use Type.IsNested for this, because that calls DeclaringType
                if (!IsNestedByFlags)
                    return null;

                var declaring = _definition.GetDeclaringType();
                if (declaring.IsNil)
                    throw new InvalidOperationException();

                return _module.ResolveType(MetadataTokens.GetToken(declaring), null, null);
            }
        }

        public override Module Module => _module;

        internal override bool IsModulePseudoType => MetadataTokens.GetRowNumber(_handle) == 1;

        internal override bool IsBaked => true;

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
