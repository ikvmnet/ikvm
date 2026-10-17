/*
  Copyright (C) 2011-2012 Jeroen Frijters

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
    /// Represents a property that could not be found on its declaring type, or whose declaring type is itself missing.
    /// It carries only the name and signature it was referred to by; members that would need the actual property
    /// definition throw <see cref="MissingMemberException"/>.
    /// </summary>
    /// <remarks>
    /// This is currently only used by <see cref="CustomAttributeData"/>, because there is no other way to refer to a
    /// property.
    /// </remarks>
    sealed class MissingProperty : PropertyInfo
    {

        readonly Type _declaringType;
        readonly string _name;
        readonly PropertySignature _signature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType">The type the property was looked up on.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="signature">The signature of the property.</param>
        internal MissingProperty(Type declaringType, string name, PropertySignature signature)
        {
            _declaringType = declaringType;
            _name = name;
            _signature = signature;
        }

        /// <summary>
        /// Gets the attributes for this property. Not supported on a missing property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override PropertyAttributes Attributes => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a value indicating whether the property can be read. Not supported on a missing property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override bool CanRead => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a value indicating whether the property can be written to. Not supported on a missing property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override bool CanWrite => throw new MissingMemberException(this);

        /// <summary>
        /// Returns the get accessor for this property. Not supported on a missing property.
        /// </summary>
        /// <param name="nonPublic">Ignored.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override MethodInfo GetGetMethod(bool nonPublic) => throw new MissingMemberException(this);

        /// <summary>
        /// Returns the set accessor for this property. Not supported on a missing property.
        /// </summary>
        /// <param name="nonPublic">Ignored.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override MethodInfo GetSetMethod(bool nonPublic) => throw new MissingMemberException(this);

        /// <summary>
        /// Returns the accessors for this property. Not supported on a missing property.
        /// </summary>
        /// <param name="nonPublic">Ignored.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override MethodInfo[] GetAccessors(bool nonPublic) => throw new MissingMemberException(this);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler. Not supported on a missing property.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        public override object GetRawConstantValue() => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a value indicating whether the property has a public accessor. Not supported on a missing property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        internal override bool IsPublic => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a value indicating whether the property has a non-private accessor. Not supported on a missing
        /// property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        internal override bool IsNonPrivate => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a value indicating whether the property is static. Not supported on a missing property.
        /// </summary>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        internal override bool IsStatic => throw new MissingMemberException(this);

        /// <inheritdoc />
        internal override PropertySignature PropertySignature => _signature;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override Type DeclaringType => _declaringType;

        /// <inheritdoc />
        public override Module Module => _declaringType.Module;

        /// <inheritdoc />
        internal override bool IsBaked => _declaringType.IsBaked;

        /// <summary>
        /// Gets the metadata token of the property. Not supported on a missing property.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always thrown.</exception>
        internal override int GetCurrentToken() => throw new MissingMemberException(this);

    }

}
