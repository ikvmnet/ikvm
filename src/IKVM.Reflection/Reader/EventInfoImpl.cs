/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class EventInfoImpl : EventInfo
    {

        readonly ModuleReader module;
        readonly Type declaringType;
        readonly int index;
        bool isPublic;
        bool isNonPrivate;
        bool isStatic;
        bool flagsCached;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="index"></param>
        internal EventInfoImpl(ModuleReader module, Type declaringType, int index)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.index = index;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EventInfoImpl;
            return other != null && other.declaringType == declaringType && other.index == index;
        }

        public override int GetHashCode()
        {
            return declaringType.GetHashCode() * 123 + index;
        }

        public override EventAttributes Attributes
        {
            get { return (EventAttributes)module.EventTable.records[index].EventFlags; }
        }

        public override MethodInfo GetAddMethod(bool nonPublic)
        {
            return module.MethodSemanticsTable.GetMethod(module, this.MetadataToken, nonPublic, MethodSemanticsTable.AddOn);
        }

        public override MethodInfo GetRaiseMethod(bool nonPublic)
        {
            return module.MethodSemanticsTable.GetMethod(module, this.MetadataToken, nonPublic, MethodSemanticsTable.Fire);
        }

        public override MethodInfo GetRemoveMethod(bool nonPublic)
        {
            return module.MethodSemanticsTable.GetMethod(module, this.MetadataToken, nonPublic, MethodSemanticsTable.RemoveOn);
        }

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            return module.MethodSemanticsTable.GetMethods(module, this.MetadataToken, nonPublic, MethodSemanticsTable.Other);
        }

        public override MethodInfo[] __GetMethods()
        {
            return module.MethodSemanticsTable.GetMethods(module, this.MetadataToken, true, -1);
        }

        public override Type EventHandlerType
        {
            get { return module.ResolveType(module.EventTable.records[index].EventType, declaringType); }
        }

        public override string Name
        {
            get { return module.GetString(module.EventTable.records[index].Name); }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override Module Module
        {
            get { return module; }
        }

        public override int MetadataToken
        {
            get { return (EventTable.Index << 24) + index + 1; }
        }

        internal override bool IsPublic
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isPublic;
            }
        }

        internal override bool IsNonPrivate
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isNonPrivate;
            }
        }

        internal override bool IsStatic
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isStatic;
            }
        }

        void ComputeFlags()
        {
            module.MethodSemanticsTable.ComputeFlags(module, this.MetadataToken, out isPublic, out isNonPrivate, out isStatic);
            flagsCached = true;
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

        internal override int GetCurrentToken()
        {
            return this.MetadataToken;
        }

    }

}
