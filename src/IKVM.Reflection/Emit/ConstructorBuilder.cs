/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.Reflection.Emit
{

    public sealed class ConstructorBuilder : ConstructorInfo
    {

        readonly MethodBuilder methodBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mb"></param>
        internal ConstructorBuilder(MethodBuilder mb)
        {
            this.methodBuilder = mb;
        }

        public override bool Equals(object obj)
        {
            ConstructorBuilder other = obj as ConstructorBuilder;
            return other != null && other.methodBuilder.Equals(methodBuilder);
        }

        public override int GetHashCode()
        {
            return methodBuilder.GetHashCode();
        }

        public void __SetSignature(Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers)
        {
            methodBuilder.__SetSignature(returnType, returnTypeCustomModifiers, parameterTypes, parameterTypeCustomModifiers);
        }

        [Obsolete("Please use __SetSignature(Type, CustomModifiers, Type[], CustomModifiers[]) instead.")]
        public void __SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            methodBuilder.SetSignature(returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
        {
            return methodBuilder.DefineParameter(position, attributes, strParamName);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            methodBuilder.SetCustomAttribute(customBuilder);
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            methodBuilder.SetCustomAttribute(con, binaryAttribute);
        }

        public void __AddDeclarativeSecurity(CustomAttributeBuilder customBuilder)
        {
            methodBuilder.__AddDeclarativeSecurity(customBuilder);
        }

        public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
        {
            methodBuilder.AddDeclarativeSecurity(securityAction, permissionSet);
        }

        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            methodBuilder.SetImplementationFlags(attributes);
        }

        public ILGenerator GetILGenerator()
        {
            return methodBuilder.GetILGenerator();
        }

        public ILGenerator GetILGenerator(int streamSize)
        {
            return methodBuilder.GetILGenerator(streamSize);
        }

        public void __ReleaseILGenerator()
        {
            methodBuilder.__ReleaseILGenerator();
        }

        public Type ReturnType
        {
            get { return methodBuilder.ReturnType; }
        }

        public Module GetModule()
        {
            return methodBuilder.GetModule();
        }

        public MethodToken GetToken()
        {
            return methodBuilder.GetToken();
        }

        public bool InitLocals
        {
            get { return methodBuilder.InitLocals; }
            set { methodBuilder.InitLocals = value; }
        }

        public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
        {
            methodBuilder.SetMethodBody(il, maxStack, localSignature, exceptionHandlers, tokenFixups);
        }

        internal override MethodInfo GetMethodInfo()
        {
            return methodBuilder;
        }

        internal override MethodInfo GetMethodOnTypeDefinition()
        {
            return methodBuilder;
        }

    }

}
