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

        readonly Module module;
        readonly Type arrayClass;
        readonly string methodName;
        readonly CallingConventions callingConvention;
        readonly Type returnType;
        protected readonly Type[] parameterTypes;
        MethodSignature methodSignature;

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
            this.module = module;
            this.arrayClass = arrayClass;
            this.methodName = methodName;
            this.callingConvention = callingConvention;
            this.returnType = returnType ?? module.universe.System_Void;
            this.parameterTypes = Util.Copy(parameterTypes);
        }

        public override MethodBody GetMethodBody()
        {
            throw new InvalidOperationException();
        }

        public override int __MethodRVA
        {
            get { throw new InvalidOperationException(); }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotSupportedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            throw new NotSupportedException();
        }

        internal override Type[] GetParameterTypes()
        {
            return parameterTypes;
        }

        internal override int ImportTo(ModuleBuilder module)
        {
            return module.ImportMethodOrField(arrayClass, methodName, MethodSignature);
        }

        public override MethodAttributes Attributes
        {
            get { throw new NotSupportedException(); }
        }

        public override CallingConventions CallingConvention
        {
            get { return callingConvention; }
        }

        public override Type DeclaringType
        {
            get { return arrayClass; }
        }

        internal override MethodSignature MethodSignature
        {
            get
            {
                if (methodSignature == null)
                {
                    methodSignature = MethodSignature.MakeFromBuilder(returnType, parameterTypes, new PackedCustomModifiers(), callingConvention, 0);
                }
                return methodSignature;
            }
        }

        public override Module Module
        {
            // FXBUG like .NET, we return the module that GetArrayMethod was called on, not the module associated with the array type
            get { return module; }
        }

        public override string Name
        {
            get { return methodName; }
        }

        internal override int ParameterCount
        {
            get { return parameterTypes.Length; }
        }

        public override ParameterInfo ReturnParameter
        {
            // FXBUG like .NET, we throw NotImplementedException
            get { throw new NotImplementedException(); }
        }

        public override Type ReturnType
        {
            get { return returnType; }
        }

        internal override bool HasThis
        {
            get { return (callingConvention & (CallingConventions.HasThis | CallingConventions.ExplicitThis)) == CallingConventions.HasThis; }
        }

        internal override int GetCurrentToken()
        {
            return this.MetadataToken;
        }

        internal override bool IsBaked
        {
            get { return arrayClass.IsBaked; }
        }

    }

}
