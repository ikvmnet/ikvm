/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    public sealed class PropertyBuilder : PropertyInfo
    {

        readonly TypeBuilder typeBuilder;
        readonly string name;
        PropertyAttributes attributes;
        PropertySignature sig;
        MethodBuilder getter;
        MethodBuilder setter;
        readonly List<Accessor> accessors = new List<Accessor>();
        int lazyPseudoToken;
        bool patchCallingConvention;

        struct Accessor
        {

            internal short Semantics;
            internal MethodBuilder Method;

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="sig"></param>
        /// <param name="patchCallingConvention"></param>
        internal PropertyBuilder(TypeBuilder typeBuilder, string name, PropertyAttributes attributes, PropertySignature sig, bool patchCallingConvention)
        {
            this.typeBuilder = typeBuilder;
            this.name = name;
            this.attributes = attributes;
            this.sig = sig;
            this.patchCallingConvention = patchCallingConvention;
        }

        internal override PropertySignature PropertySignature
        {
            get { return sig; }
        }

        public void SetGetMethod(MethodBuilder mdBuilder)
        {
            getter = mdBuilder;
            Accessor acc;
            acc.Semantics = MethodSemanticsTable.Getter;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void SetSetMethod(MethodBuilder mdBuilder)
        {
            setter = mdBuilder;
            Accessor acc;
            acc.Semantics = MethodSemanticsTable.Setter;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void AddOtherMethod(MethodBuilder mdBuilder)
        {
            Accessor acc;
            acc.Semantics = MethodSemanticsTable.Other;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (customBuilder.KnownCA == KnownCA.SpecialNameAttribute)
            {
                attributes |= PropertyAttributes.SpecialName;
            }
            else
            {
                if (lazyPseudoToken == 0)
                    lazyPseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();

                typeBuilder.ModuleBuilder.SetCustomAttribute(lazyPseudoToken, customBuilder);
            }
        }

        public override object GetRawConstantValue()
        {
            if (lazyPseudoToken != 0)
                return typeBuilder.ModuleBuilder.ConstantTable.GetRawConstantValue(typeBuilder.ModuleBuilder, lazyPseudoToken);

            throw new InvalidOperationException();
        }

        public override PropertyAttributes Attributes
        {
            get { return attributes; }
        }

        public override bool CanRead
        {
            get { return getter != null; }
        }

        public override bool CanWrite
        {
            get { return setter != null; }
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return nonPublic || (getter != null && getter.IsPublic) ? getter : null;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return nonPublic || (setter != null && setter.IsPublic) ? setter : null;
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var acc in accessors)
                AddAccessor(list, nonPublic, acc.Method);

            return list.ToArray();
        }

        static void AddAccessor(List<MethodInfo> list, bool nonPublic, MethodInfo method)
        {
            if (method != null && (nonPublic || method.IsPublic))
                list.Add(method);
        }

        public override Type DeclaringType
        {
            get { return typeBuilder; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override Module Module
        {
            get { return typeBuilder.Module; }
        }

        public void SetConstant(object defaultValue)
        {
            if (lazyPseudoToken == 0)
                lazyPseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();

            attributes |= PropertyAttributes.HasDefault;
            typeBuilder.ModuleBuilder.AddConstant(lazyPseudoToken, defaultValue);
        }

        internal void Bake()
        {
            if (patchCallingConvention)
                sig.HasThis = !this.IsStatic;

            var rec = new PropertyTable.Record();
            rec.Flags = (short)attributes;
            rec.Name = typeBuilder.ModuleBuilder.GetOrAddString(name);
            rec.Type = typeBuilder.ModuleBuilder.GetSignatureBlobIndex(sig);
            int token = MetadataTokens.GetToken(MetadataTokens.PropertyDefinitionHandle(typeBuilder.ModuleBuilder.PropertyTable.AddRecord(rec)));

            if (lazyPseudoToken == 0)
                lazyPseudoToken = token;
            else
                typeBuilder.ModuleBuilder.RegisterTokenFixup(lazyPseudoToken, token);

            foreach (var acc in accessors)
                AddMethodSemantics(acc.Semantics, acc.Method.MetadataToken, token);
        }

        void AddMethodSemantics(short semantics, int methodToken, int propertyToken)
        {
            var rec = new MethodSemanticsTable.Record();
            rec.Semantics = semantics;
            rec.Method = methodToken;
            rec.Association = propertyToken;
            typeBuilder.ModuleBuilder.MethodSemanticsTable.AddRecord(rec);
        }

        internal override bool IsPublic
        {
            get
            {
                foreach (var acc in accessors)
                    if (acc.Method.IsPublic)
                        return true;

                return false;
            }
        }

        internal override bool IsNonPrivate
        {
            get
            {
                foreach (var acc in accessors)
                    if ((acc.Method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private)
                        return true;

                return false;
            }
        }

        internal override bool IsStatic
        {
            get
            {
                foreach (var acc in accessors)
                    if (acc.Method.IsStatic)
                        return true;

                return false;
            }
        }

        internal override bool IsBaked
        {
            get { return typeBuilder.IsBaked; }
        }

        internal override int GetCurrentToken()
        {
            if (typeBuilder.ModuleBuilder.IsSaved && ModuleBuilder.IsPseudoToken(lazyPseudoToken))
                return typeBuilder.ModuleBuilder.ResolvePseudoToken(lazyPseudoToken);
            else
                return lazyPseudoToken;
        }

    }

}
