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

    sealed class MethodInfoWithReflectedType : MethodInfo
    {

        readonly Type reflectedType;
        readonly MethodInfo method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="method"></param>
        internal MethodInfoWithReflectedType(Type reflectedType, MethodInfo method)
        {
            Debug.Assert(reflectedType != method.DeclaringType);
            this.reflectedType = reflectedType;
            this.method = method;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MethodInfoWithReflectedType;
            return other != null
                && other.reflectedType == reflectedType
                && other.method == method;
        }

        public override int GetHashCode()
        {
            return reflectedType.GetHashCode() ^ method.GetHashCode();
        }

        internal override MethodSignature MethodSignature
        {
            get { return method.MethodSignature; }
        }

        internal override int ParameterCount
        {
            get { return method.ParameterCount; }
        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        internal override Type[] GetParameterTypes()
        {
            return method.GetParameterTypes();
        }

        public override MethodAttributes Attributes
        {
            get { return method.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return method.GetMethodImplementationFlags();
        }

        public override MethodBody GetMethodBody()
        {
            return method.GetMethodBody();
        }

        public override CallingConventions CallingConvention
        {
            get { return method.CallingConvention; }
        }

        public override int __MethodRVA
        {
            get { return method.__MethodRVA; }
        }

        public override Type ReturnType
        {
            get { return method.ReturnType; }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new ParameterInfoWrapper(this, method.ReturnParameter); }
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return SetReflectedType(method.MakeGenericMethod(typeArguments), reflectedType);
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return method.GetGenericMethodDefinition();
        }

        public override string ToString()
        {
            return method.ToString();
        }

        public override MethodInfo[] __GetMethodImpls()
        {
            return method.__GetMethodImpls();
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            return method.GetGenericMethodArgument(index);
        }

        internal override int GetGenericMethodArgumentCount()
        {
            return method.GetGenericMethodArgumentCount();
        }

        internal override MethodInfo GetMethodOnTypeDefinition()
        {
            return method.GetMethodOnTypeDefinition();
        }

        internal override bool HasThis
        {
            get { return method.HasThis; }
        }

        public override Module Module
        {
            get { return method.Module; }
        }

        public override Type DeclaringType
        {
            get { return method.DeclaringType; }
        }

        public override Type ReflectedType
        {
            get { return reflectedType; }
        }

        public override string Name
        {
            get { return method.Name; }
        }

        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            return method.ImportTo(module);
        }

        public override MethodBase __GetMethodOnTypeDefinition()
        {
            return method.__GetMethodOnTypeDefinition();
        }

        public override bool __IsMissing
        {
            get { return method.__IsMissing; }
        }

        internal override MethodBase BindTypeParameters(Type type)
        {
            return method.BindTypeParameters(type);
        }

        public override bool ContainsGenericParameters
        {
            get { return method.ContainsGenericParameters; }
        }

        public override Type[] GetGenericArguments()
        {
            return method.GetGenericArguments();
        }

        public override bool IsGenericMethod
        {
            get { return method.IsGenericMethod; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return method.IsGenericMethodDefinition; }
        }

        public override int MetadataToken
        {
            get { return method.MetadataToken; }
        }

        internal override int GetCurrentToken()
        {
            return method.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return method.IsBaked; }
        }

    }

}
