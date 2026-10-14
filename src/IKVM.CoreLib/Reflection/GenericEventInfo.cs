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

    /// <summary>
    /// Represents an event of a generic type instance. Wraps the event of the generic type definition and binds its
    /// accessor methods and handler type to the instance's type arguments.
    /// </summary>
    sealed class GenericEventInfo : EventInfo
    {

        readonly Type _typeInstance;
        readonly EventInfo _eventInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeInstance">The generic type instance that declares the event.</param>
        /// <param name="eventInfo">The event as declared on the generic type definition.</param>
        internal GenericEventInfo(Type typeInstance, EventInfo eventInfo)
        {
            _typeInstance = typeInstance;
            _eventInfo = eventInfo;
        }

        /// <summary>
        /// Returns whether the specified object represents the same event of the same generic type instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> represents the same event; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as GenericEventInfo;
            return other != null && other._typeInstance == _typeInstance && other._eventInfo == _eventInfo;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _typeInstance.GetHashCode() * 777 + _eventInfo.GetHashCode();

        /// <inheritdoc />
        public override EventAttributes Attributes => _eventInfo.Attributes;

        /// <summary>
        /// Binds an accessor method of the generic type definition to the generic type instance.
        /// </summary>
        /// <param name="method">The accessor method, or <see langword="null"/>.</param>
        /// <returns>The method on the generic type instance, or <see langword="null"/> if <paramref name="method"/> is <see langword="null"/>.</returns>
        MethodInfo Wrap(MethodInfo method)
        {
            if (method == null)
                return null;

            return new GenericMethodInstance(_typeInstance, method, null);
        }

        /// <inheritdoc />
        public override MethodInfo GetAddMethod(bool nonPublic) => Wrap(_eventInfo.GetAddMethod(nonPublic));

        /// <inheritdoc />
        public override MethodInfo GetRaiseMethod(bool nonPublic) => Wrap(_eventInfo.GetRaiseMethod(nonPublic));

        /// <inheritdoc />
        public override MethodInfo GetRemoveMethod(bool nonPublic) => Wrap(_eventInfo.GetRemoveMethod(nonPublic));

        /// <inheritdoc />
        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var others = _eventInfo.GetOtherMethods(nonPublic);
            for (var i = 0; i < others.Length; i++)
                others[i] = Wrap(others[i]);

            return others;
        }

        /// <summary>
        /// Gets the handler type of the event, with the type parameters of the generic type definition replaced by
        /// the instance's type arguments.
        /// </summary>
        public override Type EventHandlerType => _eventInfo.EventHandlerType.BindTypeParameters(_typeInstance);

        /// <inheritdoc />
        public override string Name => _eventInfo.Name;

        /// <summary>
        /// Gets the generic type instance that declares the event.
        /// </summary>
        public override Type DeclaringType => _typeInstance;

        /// <inheritdoc />
        public override Module Module => _eventInfo.Module;

        /// <inheritdoc />
        public override int MetadataToken => _eventInfo.MetadataToken;

        /// <inheritdoc />
        internal override EventInfo BindTypeParameters(Type type) => new GenericEventInfo(_typeInstance.BindTypeParameters(type), _eventInfo);

        /// <inheritdoc />
        internal override bool IsPublic => _eventInfo.IsPublic;

        /// <inheritdoc />
        internal override bool IsNonPrivate => _eventInfo.IsNonPrivate;

        /// <inheritdoc />
        internal override bool IsStatic => _eventInfo.IsStatic;

        /// <inheritdoc />
        internal override bool IsBaked => _eventInfo.IsBaked;

        /// <inheritdoc />
        internal override int GetCurrentToken() => _eventInfo.GetCurrentToken();

    }

}
