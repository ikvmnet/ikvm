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

        readonly ModuleReader module;
        readonly TypeDefinitionHandle handle;
        readonly TypeDefinition definition;
        readonly string typeName;
        readonly string typeNamespace;

        Type[] typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="handle"></param>
        internal TypeDefImpl(ModuleReader module, TypeDefinitionHandle handle)
        {
            this.module = module;
            this.handle = handle;
            this.definition = module.Metadata.GetTypeDefinition(handle);
            this.typeName = module.GetString(definition.Name);
            this.typeNamespace = module.GetString(definition.Namespace);
            MarkKnownType(typeNamespace, typeName);
        }

        public override Type BaseType => definition.BaseType.IsNil ? null : module.ResolveType(MetadataTokens.GetToken(definition.BaseType), this);

        public override TypeAttributes Attributes => (TypeAttributes)definition.Attributes;

        public override EventInfo[] __GetDeclaredEvents()
        {
            var handles = definition.GetEvents();
            var events = new EventInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                events[i++] = new EventInfoImpl(module, this, h);

            return events;
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            var handles = definition.GetFields();
            var fields = new FieldInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                fields[i++] = module.GetFieldAt(this, h);

            return fields;
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            var handles = definition.GetInterfaceImplementations();
            if (handles.Count == 0)
                return Type.EmptyTypes;

            var interfaces = new Type[handles.Count];
            var i = 0;
            foreach (var h in handles)
                interfaces[i++] = module.ResolveType(MetadataTokens.GetToken(module.Metadata.GetInterfaceImplementation(h).Interface), this);

            return interfaces;
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var handles = definition.GetMethods();
            var methods = new MethodBase[handles.Count];
            var i = 0;
            foreach (var h in handles)
                methods[i++] = module.GetMethodAt(this, h);

            return methods;
        }

        public override __MethodImplMap __GetMethodImplMap()
        {
            PopulateGenericArguments();

            var bodies = new List<MethodInfo>();
            var declarations = new List<List<MethodInfo>>();
            var index = new Dictionary<MethodInfo, int>();
            foreach (var h in definition.GetMethodImplementations())
            {
                var impl = module.Metadata.GetMethodImplementation(h);
                var body = (MethodInfo)module.ResolveMethod(MetadataTokens.GetToken(impl.MethodBody), typeArgs, null);
                if (index.TryGetValue(body, out var i) == false)
                {
                    index.Add(body, i = bodies.Count);
                    bodies.Add(body);
                    declarations.Add(new List<MethodInfo>());
                }

                declarations[i].Add((MethodInfo)module.ResolveMethod(MetadataTokens.GetToken(impl.MethodDeclaration), typeArgs, null));
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
            var handles = definition.GetNestedTypes();
            if (handles.Length == 0)
                return Type.EmptyTypes;

            var types = new Type[handles.Length];
            for (int i = 0; i < types.Length; i++)
                types[i] = module.ResolveType(MetadataTokens.GetToken(handles[i]));

            return types;
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            var handles = definition.GetProperties();
            var properties = new PropertyInfo[handles.Count];
            var i = 0;
            foreach (var h in handles)
                properties[i++] = new PropertyInfoImpl(module, this, h);

            return properties;
        }

        internal override TypeName TypeName => new TypeName(typeNamespace, typeName);

        public override string Name => TypeNameParser.Escape(typeName);

        public override string FullName => GetFullName();

        public override int MetadataToken => MetadataTokens.GetToken(handle);

        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(typeArgs);
        }

        void PopulateGenericArguments()
        {
            if (typeArgs == null)
            {
                var handles = definition.GetGenericParameters();
                var args = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
                for (int i = 0; i < args.Length; i++)
                    args[i] = new GenericTypeParameter(module, handles[i], Signature.ELEMENT_TYPE_VAR);

                typeArgs = args;
            }
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            PopulateGenericArguments();
            return typeArgs[index];
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            PopulateGenericArguments();
            return new CustomModifiers[typeArgs.Length];
        }

        public override bool IsGenericType => IsGenericTypeDefinition;

        public override bool IsGenericTypeDefinition => definition.GetGenericParameters().Count > 0;

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

                var declaring = definition.GetDeclaringType();
                if (declaring.IsNil)
                    throw new InvalidOperationException();

                return module.ResolveType(MetadataTokens.GetToken(declaring), null, null);
            }
        }

        public override Module Module => module;

        internal override bool IsModulePseudoType => MetadataTokens.GetRowNumber(handle) == 1;

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
