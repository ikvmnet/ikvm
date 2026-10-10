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

    internal sealed class __StandAloneMethodSig
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
        /// <param name="unmanaged"></param>
        /// <param name="unmanagedCallingConvention"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="optionalParameterTypes"></param>
        /// <param name="customModifiers"></param>
        internal __StandAloneMethodSig(bool unmanaged, CallingConvention unmanagedCallingConvention, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, PackedCustomModifiers customModifiers)
        {
            this._unmanaged = unmanaged;
            this._unmanagedCallingConvention = unmanagedCallingConvention;
            this._callingConvention = callingConvention;
            this._returnType = returnType;
            this._parameterTypes = parameterTypes;
            this._optionalParameterTypes = optionalParameterTypes;
            this._customModifiers = customModifiers;
        }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as __StandAloneMethodSig);
        }

        public override int GetHashCode()
        {
            return _returnType.GetHashCode()
                ^ Util.GetHashCode(_parameterTypes);
        }

        public bool IsUnmanaged
        {
            get { return _unmanaged; }
        }

        public CallingConventions CallingConvention
        {
            get { return _callingConvention; }
        }

        public CallingConvention UnmanagedCallingConvention
        {
            get { return _unmanagedCallingConvention; }
        }

        public Type ReturnType
        {
            get { return _returnType; }
        }

        public CustomModifiers GetReturnTypeCustomModifiers()
        {
            return _customModifiers.GetReturnTypeCustomModifiers();
        }

        public Type[] ParameterTypes
        {
            get { return Util.Copy(_parameterTypes); }
        }

        public Type[] OptionalParameterTypes
        {
            get { return Util.Copy(_optionalParameterTypes); }
        }

        public CustomModifiers GetParameterCustomModifiers(int index)
        {
            return _customModifiers.GetParameterCustomModifiers(index);
        }

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
