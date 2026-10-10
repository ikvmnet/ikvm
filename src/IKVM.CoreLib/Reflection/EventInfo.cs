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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Discovers the attributes of an event and provides access to event metadata.
    /// </summary>
    abstract class EventInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected EventInfo()
        {
        }

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is an event.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Event;

        /// <summary>
        /// Gets the attributes for this event.
        /// </summary>
        public abstract EventAttributes Attributes { get; }

        /// <summary>
        /// When overridden in a derived class, retrieves the method used to add an event handler delegate to the event
        /// source, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"><see langword="true"/> if non-public methods can be returned; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the method used to add an event handler delegate to the event source.</returns>
        public abstract MethodInfo GetAddMethod(bool nonPublic);

        /// <summary>
        /// When overridden in a derived class, returns the method that is called when the event is raised, specifying
        /// whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"><see langword="true"/> if non-public methods can be returned; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="MethodInfo"/> object that was called when the event was raised.</returns>
        public abstract MethodInfo GetRaiseMethod(bool nonPublic);

        /// <summary>
        /// When overridden in a derived class, retrieves the method used to remove an event handler delegate from the
        /// event source, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"><see langword="true"/> if non-public methods can be returned; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the method used to remove an event handler delegate from the event source.</returns>
        public abstract MethodInfo GetRemoveMethod(bool nonPublic);

        /// <summary>
        /// Returns the methods that have been associated with the event in metadata using the <c>.other</c> directive,
        /// specifying whether to include non-public methods.
        /// </summary>
        /// <param name="nonPublic"><see langword="true"/> to include non-public methods; otherwise, <see langword="false"/>.</param>
        /// <returns>An array of <see cref="MethodInfo"/> objects representing methods that have been associated with an event in metadata by using the <c>.other</c> directive.</returns>
        public abstract MethodInfo[] GetOtherMethods(bool nonPublic);

        /// <summary>
        /// Gets the <see cref="Type"/> object of the underlying event-handler delegate associated with this event.
        /// </summary>
        public abstract Type EventHandlerType { get; }

        /// <summary>
        /// Gets a value indicating whether any accessor of the event is public, for binding flag matching.
        /// </summary>
        internal abstract bool IsPublic { get; }

        /// <summary>
        /// Gets a value indicating whether any accessor of the event is not private, so the event is visible to derived
        /// types.
        /// </summary>
        internal abstract bool IsNonPrivate { get; }

        /// <summary>
        /// Gets a value indicating whether the accessors of the event are static.
        /// </summary>
        internal abstract bool IsStatic { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="EventInfo"/> has a name with a special meaning.
        /// </summary>
        public bool IsSpecialName => (Attributes & EventAttributes.SpecialName) != 0;

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source.
        /// </summary>
        /// <returns>A <see cref="MethodInfo"/> object representing the method used to add an event handler delegate to the event source.</returns>
        public MethodInfo GetAddMethod() => GetAddMethod(false);

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        /// <returns>The method that is called when the event is raised.</returns>
        public MethodInfo GetRaiseMethod() => GetRaiseMethod(false);

        /// <summary>
        /// Returns the method used to remove an event handler delegate from the event source.
        /// </summary>
        /// <returns>A <see cref="MethodInfo"/> object representing the method used to remove an event handler delegate from the event source.</returns>
        public MethodInfo GetRemoveMethod() => GetRemoveMethod(false);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the <c>.other</c>
        /// directive.
        /// </summary>
        /// <returns>An array of <see cref="MethodInfo"/> objects representing the public methods that have been associated with the event in metadata by using the <c>.other</c> directive.</returns>
        public MethodInfo[] GetOtherMethods() => GetOtherMethods(false);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object for the <see cref="GetAddMethod(bool)"/> method of the event,
        /// including non-public methods.
        /// </summary>
        public MethodInfo AddMethod => GetAddMethod(true);

        /// <summary>
        /// Gets the method that is called when the event is raised, including non-public methods.
        /// </summary>
        public MethodInfo RaiseMethod => GetRaiseMethod(true);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object for removing a method of the event, including non-public methods.
        /// </summary>
        public MethodInfo RemoveMethod => GetRemoveMethod(true);

        /// <summary>
        /// Returns this event as a member of the constructed generic type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose generic arguments are substituted into the declaring type.</param>
        /// <returns>The event on the constructed declaring type.</returns>
        internal virtual EventInfo BindTypeParameters(Type type) => new GenericEventInfo(DeclaringType.BindTypeParameters(type), this);

        /// <summary>
        /// Returns a string made of the declaring type and the name of the event.
        /// </summary>
        /// <returns>A string that represents the current event.</returns>
        public override string ToString() => DeclaringType.ToString() + " " + Name;

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return IsNonPrivate
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        /// <inheritdoc />
        internal sealed override MemberInfo SetReflectedType(Type type) => new EventInfoWithReflectedType(type, this);

        /// <summary>
        /// Gets the pseudo custom attributes of the event. Events have none.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter by, or <see langword="null"/> for all.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // events don't have pseudo custom attributes
            null;

    }

}
