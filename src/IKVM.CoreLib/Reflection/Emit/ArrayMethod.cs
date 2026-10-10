/*
  Copyright (C) 2008-2015 Jeroen Frijters

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
using System;

namespace IKVM.Reflection.Emit
{

    class ArrayMethod : MethodInfo
    {

        readonly Module _module;
        readonly Type _arrayClass;
        readonly string _methodName;
        readonly CallingConventions _callingConvention;
        readonly Type _returnType;
        protected readonly Type[] parameterTypes;
        MethodSignature _methodSignature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="arrayClass"></param>
        /// <param name="methodName"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        internal ArrayMethod(Module module, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            _module = module;
            _arrayClass = arrayClass;
            _methodName = methodName;
            _callingConvention = callingConvention;
            _returnType = returnType ?? module.Universe.System_Void;
            this.parameterTypes = Util.Copy(parameterTypes);
        }

        public override MethodImplAttributes GetMethodImplementationFlags() => throw new NotSupportedException();

        public override ParameterInfo[] GetParameters() => throw new NotSupportedException();

        internal override Type[] GetParameterTypes() => parameterTypes;

        internal override int ImportTo(ModuleBuilder module) => module.ImportMethodOrField(_arrayClass, _methodName, MethodSignature);

        public override MethodAttributes Attributes => throw new NotSupportedException();

        public override CallingConventions CallingConvention => _callingConvention;

        public override Type DeclaringType => _arrayClass;

        internal override MethodSignature MethodSignature
        {
            get
            {
                _methodSignature ??= MethodSignature.MakeFromBuilder(_returnType, parameterTypes, new PackedCustomModifiers(), _callingConvention, 0);
                return _methodSignature;
            }
        }

        public override Module Module
            // FXBUG like .NET, we return the module that GetArrayMethod was called on, not the module associated with the array type
            => _module;

        public override string Name => _methodName;

        internal override int ParameterCount => parameterTypes.Length;

        public override ParameterInfo ReturnParameter
            // FXBUG like .NET, we throw NotImplementedException
            => throw new NotImplementedException();

        public override Type ReturnType => _returnType;

        internal override int GetCurrentToken() => MetadataToken;

        internal override bool IsBaked => _arrayClass.IsBaked;

    }

}
