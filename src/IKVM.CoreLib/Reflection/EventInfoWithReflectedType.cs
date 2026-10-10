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

        readonly Type _reflectedType;
        readonly EventInfo _eventInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="eventInfo"></param>
        internal EventInfoWithReflectedType(Type reflectedType, EventInfo eventInfo)
        {
            Debug.Assert(reflectedType != eventInfo.DeclaringType);
            _reflectedType = reflectedType;
            _eventInfo = eventInfo;
        }

        public override EventAttributes Attributes => _eventInfo.Attributes;

        public override MethodInfo GetAddMethod(bool nonPublic) => SetReflectedType(_eventInfo.GetAddMethod(nonPublic), _reflectedType);

        public override MethodInfo GetRaiseMethod(bool nonPublic) => SetReflectedType(_eventInfo.GetRaiseMethod(nonPublic), _reflectedType);

        public override MethodInfo GetRemoveMethod(bool nonPublic) => SetReflectedType(_eventInfo.GetRemoveMethod(nonPublic), _reflectedType);

        public override MethodInfo[] GetOtherMethods(bool nonPublic) => SetReflectedType(_eventInfo.GetOtherMethods(nonPublic), _reflectedType);

        public override Type EventHandlerType => _eventInfo.EventHandlerType;

        internal override bool IsPublic => _eventInfo.IsPublic;

        internal override bool IsNonPrivate => _eventInfo.IsNonPrivate;

        internal override bool IsStatic => _eventInfo.IsStatic;

        internal override EventInfo BindTypeParameters(Type type) => _eventInfo.BindTypeParameters(type);

        public override string ToString() => _eventInfo.ToString();

        public override bool __IsMissing => _eventInfo.__IsMissing;

        public override Type DeclaringType => _eventInfo.DeclaringType;

        public override bool Equals(object obj)
        {
            var other = obj as EventInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._eventInfo == _eventInfo;
        }

        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _eventInfo.GetHashCode();

        public override int MetadataToken => _eventInfo.MetadataToken;

        public override Module Module => _eventInfo.Module;

        public override string Name => _eventInfo.Name;

        internal override bool IsBaked => _eventInfo.IsBaked;

        internal override int GetCurrentToken() => _eventInfo.GetCurrentToken();

    }

}
