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
using System.Diagnostics;

namespace IKVM.Reflection
{

    sealed class EventInfoWithReflectedType : EventInfo
    {

        readonly Type reflectedType;
        readonly EventInfo eventInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="eventInfo"></param>
        internal EventInfoWithReflectedType(Type reflectedType, EventInfo eventInfo)
        {
            Debug.Assert(reflectedType != eventInfo.DeclaringType);
            this.reflectedType = reflectedType;
            this.eventInfo = eventInfo;
        }

        public override EventAttributes Attributes
        {
            get { return eventInfo.Attributes; }
        }

        public override MethodInfo GetAddMethod(bool nonPublic)
        {
            return SetReflectedType(eventInfo.GetAddMethod(nonPublic), reflectedType);
        }

        public override MethodInfo GetRaiseMethod(bool nonPublic)
        {
            return SetReflectedType(eventInfo.GetRaiseMethod(nonPublic), reflectedType);
        }

        public override MethodInfo GetRemoveMethod(bool nonPublic)
        {
            return SetReflectedType(eventInfo.GetRemoveMethod(nonPublic), reflectedType);
        }

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            return SetReflectedType(eventInfo.GetOtherMethods(nonPublic), reflectedType);
        }

        public override MethodInfo[] __GetMethods()
        {
            return SetReflectedType(eventInfo.__GetMethods(), reflectedType);
        }

        public override Type EventHandlerType
        {
            get { return eventInfo.EventHandlerType; }
        }

        internal override bool IsPublic
        {
            get { return eventInfo.IsPublic; }
        }

        internal override bool IsNonPrivate
        {
            get { return eventInfo.IsNonPrivate; }
        }

        internal override bool IsStatic
        {
            get { return eventInfo.IsStatic; }
        }

        internal override EventInfo BindTypeParameters(Type type)
        {
            return eventInfo.BindTypeParameters(type);
        }

        public override string ToString()
        {
            return eventInfo.ToString();
        }

        public override bool __IsMissing
        {
            get { return eventInfo.__IsMissing; }
        }

        public override Type DeclaringType
        {
            get { return eventInfo.DeclaringType; }
        }

        public override Type ReflectedType
        {
            get { return reflectedType; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as EventInfoWithReflectedType;
            return other != null
                && other.reflectedType == reflectedType
                && other.eventInfo == eventInfo;
        }

        public override int GetHashCode()
        {
            return reflectedType.GetHashCode() ^ eventInfo.GetHashCode();
        }

        public override int MetadataToken
        {
            get { return eventInfo.MetadataToken; }
        }

        public override Module Module
        {
            get { return eventInfo.Module; }
        }

        public override string Name
        {
            get { return eventInfo.Name; }
        }

        internal override bool IsBaked
        {
            get { return eventInfo.IsBaked; }
        }

        internal override int GetCurrentToken()
        {
            return eventInfo.GetCurrentToken();
        }

    }

}
