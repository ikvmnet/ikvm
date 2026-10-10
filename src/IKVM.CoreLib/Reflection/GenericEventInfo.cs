/*
  Copyright (C) 2009, 2010 Jeroen Frijters

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
namespace IKVM.Reflection
{

    sealed class GenericEventInfo : EventInfo
    {

        readonly Type _typeInstance;
        readonly EventInfo _eventInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeInstance"></param>
        /// <param name="eventInfo"></param>
        internal GenericEventInfo(Type typeInstance, EventInfo eventInfo)
        {
            this._typeInstance = typeInstance;
            this._eventInfo = eventInfo;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericEventInfo;
            return other != null && other._typeInstance == _typeInstance && other._eventInfo == _eventInfo;
        }

        public override int GetHashCode()
        {
            return _typeInstance.GetHashCode() * 777 + _eventInfo.GetHashCode();
        }

        public override EventAttributes Attributes
        {
            get { return _eventInfo.Attributes; }
        }

        private MethodInfo Wrap(MethodInfo method)
        {
            if (method == null)
                return null;

            return new GenericMethodInstance(_typeInstance, method, null);
        }

        public override MethodInfo GetAddMethod(bool nonPublic)
        {
            return Wrap(_eventInfo.GetAddMethod(nonPublic));
        }

        public override MethodInfo GetRaiseMethod(bool nonPublic)
        {
            return Wrap(_eventInfo.GetRaiseMethod(nonPublic));
        }

        public override MethodInfo GetRemoveMethod(bool nonPublic)
        {
            return Wrap(_eventInfo.GetRemoveMethod(nonPublic));
        }

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var others = _eventInfo.GetOtherMethods(nonPublic);
            for (int i = 0; i < others.Length; i++)
                others[i] = Wrap(others[i]);

            return others;
        }

        public override Type EventHandlerType
        {
            get { return _eventInfo.EventHandlerType.BindTypeParameters(_typeInstance); }
        }

        public override string Name
        {
            get { return _eventInfo.Name; }
        }

        public override Type DeclaringType
        {
            get { return _typeInstance; }
        }

        public override Module Module
        {
            get { return _eventInfo.Module; }
        }

        public override int MetadataToken
        {
            get { return _eventInfo.MetadataToken; }
        }

        internal override EventInfo BindTypeParameters(Type type)
        {
            return new GenericEventInfo(_typeInstance.BindTypeParameters(type), _eventInfo);
        }

        internal override bool IsPublic
        {
            get { return _eventInfo.IsPublic; }
        }

        internal override bool IsNonPrivate
        {
            get { return _eventInfo.IsNonPrivate; }
        }

        internal override bool IsStatic
        {
            get { return _eventInfo.IsStatic; }
        }

        internal override bool IsBaked
        {
            get { return _eventInfo.IsBaked; }
        }

        internal override int GetCurrentToken()
        {
            return _eventInfo.GetCurrentToken();
        }

    }

}
