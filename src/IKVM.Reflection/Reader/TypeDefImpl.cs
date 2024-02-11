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
using System.Text;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class TypeDefImpl : TypeInfo
    {

        readonly ModuleReader module;
        readonly int index;
        readonly string typeName;
        readonly string typeNamespace;

        Type[] typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="index"></param>
        internal TypeDefImpl(ModuleReader module, int index)
        {
            this.module = module;
            this.index = index;
            this.typeName = module.GetString(module.TypeDefTable.records[index].TypeName);
            this.typeNamespace = module.GetString(module.TypeDefTable.records[index].TypeNamespace);
            MarkKnownType(typeNamespace, typeName);
        }

        public override Type BaseType
        {
            get
            {
                var extends = module.TypeDefTable.records[index].Extends;
                if ((extends & 0xFFFFFF) == 0)
                    return null;

                return module.ResolveType(extends, this);
            }
        }

        public override TypeAttributes Attributes
        {
            get { return (TypeAttributes)module.TypeDefTable.records[index].Flags; }
        }

        public override EventInfo[] __GetDeclaredEvents()
        {
            foreach (var i in module.EventMapTable.Filter(this.MetadataToken))
            {
                var evt = module.EventMapTable.records[i].EventList - 1;
                var end = module.EventMapTable.records.Length > i + 1 ? module.EventMapTable.records[i + 1].EventList - 1 : module.EventTable.records.Length;
                var events = new EventInfo[end - evt];
                if (module.EventPtrTable.RowCount == 0)
                    for (int j = 0; evt < end; evt++, j++)
                        events[j] = new EventInfoImpl(module, this, evt);
                else
                    for (int j = 0; evt < end; evt++, j++)
                        events[j] = new EventInfoImpl(module, this, module.EventPtrTable.records[evt] - 1);

                return events;
            }

            return Array.Empty<EventInfo>();
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            var field = module.TypeDefTable.records[index].FieldList - 1;
            var end = module.TypeDefTable.records.Length > index + 1 ? module.TypeDefTable.records[index + 1].FieldList - 1 : module.FieldTable.records.Length;
            var fields = new FieldInfo[end - field];
            if (module.FieldPtrTable.RowCount == 0)
                for (int i = 0; field < end; i++, field++)
                    fields[i] = module.GetFieldAt(this, field);
            else
                for (int i = 0; field < end; i++, field++)
                    fields[i] = module.GetFieldAt(this, module.FieldPtrTable.records[field] - 1);

            return fields;
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            List<Type> list = null;
            foreach (int i in module.InterfaceImplTable.Filter(MetadataToken))
            {
                list ??= new List<Type>();
                list.Add(module.ResolveType(module.InterfaceImplTable.records[i].Interface, this));
            }

            return Util.ToArray(list, Type.EmptyTypes);
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            var method = module.TypeDefTable.records[index].MethodList - 1;
            var end = module.TypeDefTable.records.Length > index + 1 ? module.TypeDefTable.records[index + 1].MethodList - 1 : module.MethodDefTable.records.Length;
            var methods = new MethodBase[end - method];
            if (module.MethodPtrTable.RowCount == 0)
                for (int i = 0; method < end; method++, i++)
                    methods[i] = module.GetMethodAt(this, method);
            else
                for (int i = 0; method < end; method++, i++)
                    methods[i] = module.GetMethodAt(this, module.MethodPtrTable.records[method] - 1);

            return methods;
        }

        public override __MethodImplMap __GetMethodImplMap()
        {
            PopulateGenericArguments();

            var bodies = new List<MethodInfo>();
            var declarations = new List<List<MethodInfo>>();
            foreach (int i in module.MethodImplTable.Filter(this.MetadataToken))
            {
                var body = (MethodInfo)module.ResolveMethod(module.MethodImplTable.records[i].MethodBody, typeArgs, null);
                int index = bodies.IndexOf(body);
                if (index == -1)
                {
                    index = bodies.Count;
                    bodies.Add(body);
                    declarations.Add(new List<MethodInfo>());
                }

                var declaration = (MethodInfo)module.ResolveMethod(module.MethodImplTable.records[i].MethodDeclaration, typeArgs, null);
                declarations[index].Add(declaration);
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
            var token = this.MetadataToken;
            var list = new List<Type>();

            // note that the NestedClass table is sorted on NestedClass, so we can't use binary search
            for (int i = 0; i < module.NestedClassTable.records.Length; i++)
                if (module.NestedClassTable.records[i].EnclosingClass == token)
                    list.Add(module.ResolveType(module.NestedClassTable.records[i].NestedClass));

            return list.ToArray();
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            foreach (var i in module.PropertyMapTable.Filter(this.MetadataToken))
            {
                var property = module.PropertyMapTable.records[i].PropertyList - 1;
                var end = module.PropertyMapTable.records.Length > i + 1 ? module.PropertyMapTable.records[i + 1].PropertyList - 1 : module.PropertyTable.records.Length;
                var properties = new PropertyInfo[end - property];
                if (module.PropertyPtrTable.RowCount == 0)
                    for (int j = 0; property < end; property++, j++)
                        properties[j] = new PropertyInfoImpl(module, this, property);
                else
                    for (int j = 0; property < end; property++, j++)
                        properties[j] = new PropertyInfoImpl(module, this, module.PropertyPtrTable.records[property] - 1);

                return properties;
            }

            return Array.Empty<PropertyInfo>();
        }

        internal override TypeName TypeName
        {
            get { return new TypeName(typeNamespace, typeName); }
        }

        public override string Name
        {
            get { return TypeNameParser.Escape(typeName); }
        }

        public override string FullName
        {
            get { return GetFullName(); }
        }

        public override int MetadataToken
        {
            get { return (TypeDefTable.Index << 24) + index + 1; }
        }

        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(typeArgs);
        }

        private void PopulateGenericArguments()
        {
            if (typeArgs == null)
            {
                var token = this.MetadataToken;
                var first = module.GenericParamTable.FindFirstByOwner(token);
                if (first == -1)
                {
                    typeArgs = Type.EmptyTypes;
                }
                else
                {
                    var list = new List<Type>();
                    var len = module.GenericParamTable.records.Length;
                    for (int i = first; i < len && module.GenericParamTable.records[i].Owner == token; i++)
                        list.Add(new GenericTypeParameter(module, i, Signature.ELEMENT_TYPE_VAR));

                    typeArgs = list.ToArray();
                }
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

        public override bool IsGenericType
        {
            get { return IsGenericTypeDefinition; }
        }

        public override bool IsGenericTypeDefinition
        {
            get
            {
                if ((typeFlags & (TypeFlags.IsGenericTypeDefinition | TypeFlags.NotGenericTypeDefinition)) == 0)
                    typeFlags |= module.GenericParamTable.FindFirstByOwner(MetadataToken) == -1 ? TypeFlags.NotGenericTypeDefinition : TypeFlags.IsGenericTypeDefinition;

                return (typeFlags & TypeFlags.IsGenericTypeDefinition) != 0;
            }
        }

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

        internal bool IsNestedByFlags
        {
            get { return (Attributes & TypeAttributes.VisibilityMask & ~TypeAttributes.Public) != 0; }
        }

        public override Type DeclaringType
        {
            get
            {
                // note that we cannot use Type.IsNested for this, because that calls DeclaringType
                if (!IsNestedByFlags)
                    return null;

                foreach (int i in module.NestedClassTable.Filter(MetadataToken))
                    return module.ResolveType(module.NestedClassTable.records[i].EnclosingClass, null, null);

                throw new InvalidOperationException();
            }
        }

        public override bool __GetLayout(out int packingSize, out int typeSize)
        {
            foreach (int i in module.ClassLayoutTable.Filter(MetadataToken))
            {
                packingSize = module.ClassLayoutTable.records[i].PackingSize;
                typeSize = module.ClassLayoutTable.records[i].ClassSize;
                return true;
            }

            packingSize = 0;
            typeSize = 0;
            return false;
        }

        public override Module Module
        {
            get { return module; }
        }

        internal override bool IsModulePseudoType
        {
            get { return index == 0; }
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

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
