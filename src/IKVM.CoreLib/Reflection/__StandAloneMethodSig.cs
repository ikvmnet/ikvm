/*
  Copyright (C) 2010 Jeroen Frijters

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
using System.Runtime.InteropServices;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a stand-alone method signature, such as the signature referenced by a <c>calli</c> instruction. This is
    /// an IKVM extension; System.Reflection has no equivalent type.
    /// </summary>
    sealed class __StandAloneMethodSig
    {

        readonly bool _unmanaged;
        readonly CallingConvention _unmanagedCallingConvention;
        readonly CallingConventions _callingConvention;
        readonly Type _returnType;
        readonly Type[] _parameterTypes;
        readonly Type[] _optionalParameterTypes;
        readonly PackedCustomModifiers _customModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="unmanaged"><see langword="true"/> if the signature uses an unmanaged calling convention.</param>
        /// <param name="unmanagedCallingConvention">The unmanaged calling convention; meaningful only when <paramref name="unmanaged"/> is <see langword="true"/>.</param>
        /// <param name="callingConvention">The managed calling convention; meaningful only when <paramref name="unmanaged"/> is <see langword="false"/>.</param>
        /// <param name="returnType">The return type.</param>
        /// <param name="parameterTypes">The types of the fixed parameters.</param>
        /// <param name="optionalParameterTypes">The types of the variable arguments that follow the sentinel, for a vararg call site.</param>
        /// <param name="customModifiers">The custom modifiers of the return type and parameters.</param>
        internal __StandAloneMethodSig(bool unmanaged, CallingConvention unmanagedCallingConvention, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, PackedCustomModifiers customModifiers)
        {
            _unmanaged = unmanaged;
            _unmanagedCallingConvention = unmanagedCallingConvention;
            _callingConvention = callingConvention;
            _returnType = returnType;
            _parameterTypes = parameterTypes;
            _optionalParameterTypes = optionalParameterTypes;
            _customModifiers = customModifiers;
        }

        /// <summary>
        /// Determines whether this signature is identical to another.
        /// </summary>
        /// <param name="other">The signature to compare with, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the calling conventions, return type, parameter types and custom modifiers are all equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(__StandAloneMethodSig other)
        {
            return other != null
                && other._unmanaged == _unmanaged
                && other._unmanagedCallingConvention == _unmanagedCallingConvention
                && other._callingConvention == _callingConvention
                && other._returnType == _returnType
                && Util.ArrayEquals(other._parameterTypes, _parameterTypes)
                && Util.ArrayEquals(other._optionalParameterTypes, _optionalParameterTypes)
                && other._customModifiers.Equals(_customModifiers);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as __StandAloneMethodSig);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _returnType.GetHashCode()
                ^ Util.GetHashCode(_parameterTypes);
        }

        /// <summary>
        /// Gets a value indicating whether the signature uses an unmanaged calling convention.
        /// </summary>
        public bool IsUnmanaged => _unmanaged;

        /// <summary>
        /// Gets the managed calling convention of the signature.
        /// </summary>
        public CallingConventions CallingConvention => _callingConvention;

        /// <summary>
        /// Gets the unmanaged calling convention of the signature.
        /// </summary>
        public CallingConvention UnmanagedCallingConvention => _unmanagedCallingConvention;

        /// <summary>
        /// Gets the return type of the signature.
        /// </summary>
        public Type ReturnType => _returnType;

        /// <summary>
        /// Gets the custom modifiers of the return type.
        /// </summary>
        /// <returns>The custom modifiers of the return type.</returns>
        public CustomModifiers GetReturnTypeCustomModifiers() => _customModifiers.GetReturnTypeCustomModifiers();

        /// <summary>
        /// Gets a copy of the types of the fixed parameters.
        /// </summary>
        public Type[] ParameterTypes => Util.Copy(_parameterTypes);

        /// <summary>
        /// Gets a copy of the types of the variable arguments that follow the sentinel.
        /// </summary>
        public Type[] OptionalParameterTypes => Util.Copy(_optionalParameterTypes);

        /// <summary>
        /// Gets the custom modifiers of a parameter.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>The custom modifiers of the parameter.</returns>
        public CustomModifiers GetParameterCustomModifiers(int index) => _customModifiers.GetParameterCustomModifiers(index);

        /// <summary>
        /// Gets a value indicating whether the signature refers to a type that could not be resolved.
        /// </summary>
        public bool ContainsMissingType
        {
            get
            {
                return _returnType.__ContainsMissingType
                    || Type.ContainsMissingType(_parameterTypes)
                    || Type.ContainsMissingType(_optionalParameterTypes)
                    || _customModifiers.ContainsMissingType;
            }
        }

    }

}
