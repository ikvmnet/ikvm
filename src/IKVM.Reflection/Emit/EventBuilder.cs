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
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    public sealed class EventBuilder : EventInfo
    {

        struct Accessor
        {

            internal short Semantics;
            internal MethodBuilder Method;

        }

        readonly TypeBuilder typeBuilder;
        readonly string name;
        EventAttributes attributes;
        readonly int eventType;
        MethodBuilder addOnMethod;
        MethodBuilder removeOnMethod;
        MethodBuilder fireMethod;
        readonly List<Accessor> accessors = new List<Accessor>();
        int lazyPseudoToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventtype"></param>
        internal EventBuilder(TypeBuilder typeBuilder, string name, EventAttributes attributes, Type eventtype)
        {
            this.typeBuilder = typeBuilder;
            this.name = name;
            this.attributes = attributes;
            this.eventType = typeBuilder.ModuleBuilder.GetTypeTokenForMemberRef(eventtype);
        }

        public void SetAddOnMethod(MethodBuilder mdBuilder)
        {
            addOnMethod = mdBuilder;
            var acc = new Accessor();
            acc.Semantics = MethodSemanticsTable.AddOn;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void SetRemoveOnMethod(MethodBuilder mdBuilder)
        {
            removeOnMethod = mdBuilder;
            var acc = new Accessor();
            acc.Semantics = MethodSemanticsTable.RemoveOn;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void SetRaiseMethod(MethodBuilder mdBuilder)
        {
            fireMethod = mdBuilder;
            var acc = new Accessor();
            acc.Semantics = MethodSemanticsTable.Fire;
            acc.Method = mdBuilder;
            accessors.Add(acc);
        }

        public void AddOtherMethod(MethodBuilder mdBuilder)
        {
            var acc = new Accessor();
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
                attributes |= EventAttributes.SpecialName;
            }
            else
            {
                if (lazyPseudoToken == 0)
                    lazyPseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();

                typeBuilder.ModuleBuilder.SetCustomAttribute(lazyPseudoToken, customBuilder);
            }
        }

        public override EventAttributes Attributes
        {
            get { return attributes; }
        }

        public override MethodInfo GetAddMethod(bool nonPublic)
        {
            return nonPublic || (addOnMethod != null && addOnMethod.IsPublic) ? addOnMethod : null;
        }

        public override MethodInfo GetRemoveMethod(bool nonPublic)
        {
            return nonPublic || (removeOnMethod != null && removeOnMethod.IsPublic) ? removeOnMethod : null;
        }

        public override MethodInfo GetRaiseMethod(bool nonPublic)
        {
            return nonPublic || (fireMethod != null && fireMethod.IsPublic) ? fireMethod : null;
        }

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var acc in accessors)
                if (acc.Semantics == MethodSemanticsTable.Other && (nonPublic || acc.Method.IsPublic))
                    list.Add(acc.Method);

            return list.ToArray();
        }

        public override MethodInfo[] __GetMethods()
        {
            List<MethodInfo> list = new List<MethodInfo>();
            foreach (Accessor acc in accessors)
            {
                list.Add(acc.Method);
            }
            return list.ToArray();
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
            get { return typeBuilder.ModuleBuilder; }
        }

        public EventToken GetEventToken()
        {
            if (lazyPseudoToken == 0)
                lazyPseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();

            return new EventToken(lazyPseudoToken);
        }

        public override Type EventHandlerType
        {
            get { return typeBuilder.ModuleBuilder.ResolveType(eventType); }
        }

        internal void Bake()
        {
            var rec = new EventTable.Record();
            rec.EventFlags = (short)attributes;
            rec.Name = typeBuilder.ModuleBuilder.GetOrAddString(name);
            rec.EventType = eventType;

            var token = MetadataTokens.GetToken(MetadataTokens.EventDefinitionHandle(typeBuilder.ModuleBuilder.EventTable.AddRecord(rec)));
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
                foreach (Accessor acc in accessors)
                {
                    if (acc.Method.IsPublic)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        internal override bool IsNonPrivate
        {
            get
            {
                foreach (Accessor acc in accessors)
                {
                    if ((acc.Method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        internal override bool IsStatic
        {
            get
            {
                foreach (Accessor acc in accessors)
                {
                    if (acc.Method.IsStatic)
                    {
                        return true;
                    }
                }
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
            {
                return typeBuilder.ModuleBuilder.ResolvePseudoToken(lazyPseudoToken);
            }
            else
            {
                return lazyPseudoToken;
            }
        }

    }

}
