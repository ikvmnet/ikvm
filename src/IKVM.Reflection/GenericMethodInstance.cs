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
using System;

namespace IKVM.Reflection
{

    // this represents both generic method instantiations and non-generic methods on generic type instantations
    // (this means that it can be a generic method declaration as well as a generic method instance)
    sealed class GenericMethodInstance : MethodInfo
    {

        readonly Type declaringType;
        readonly MethodInfo method;
        readonly Type[] methodArgs;
        MethodSignature lazyMethodSignature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        internal GenericMethodInstance(Type declaringType, MethodInfo method, Type[] methodArgs)
        {
            System.Diagnostics.Debug.Assert(method is not GenericMethodInstance);
            this.declaringType = declaringType;
            this.method = method;
            this.methodArgs = methodArgs;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericMethodInstance;
            return other != null
                && other.method.Equals(method)
                && other.declaringType.Equals(declaringType)
                && Util.ArrayEquals(other.methodArgs, methodArgs);
        }

        public override int GetHashCode()
        {
            return declaringType.GetHashCode() * 33 ^ method.GetHashCode() ^ Util.GetHashCode(methodArgs);
        }

        public override Type ReturnType
        {
            get { return method.ReturnType.BindTypeParameters(this); }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new GenericParameterInfoImpl(this, method.ReturnParameter); }
        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new GenericParameterInfoImpl(this, parameters[i]);

            return parameters;
        }

        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = method.GetParameterTypes();
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = parameterTypes[i].BindTypeParameters(method);

            return parameterTypes;
        }

        internal override int ParameterCount
        {
            get { return method.ParameterCount; }
        }

        public override CallingConventions CallingConvention
        {
            get { return method.CallingConvention; }
        }

        public override MethodAttributes Attributes
        {
            get { return method.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return method.GetMethodImplementationFlags();
        }

        public override string Name
        {
            get { return method.Name; }
        }

        public override Type DeclaringType
        {
            get { return declaringType.IsModulePseudoType ? null : declaringType; }
        }

        public override Module Module
        {
            get { return method.Module; }
        }

        public override int MetadataToken
        {
            get { return method.MetadataToken; }
        }

        public override MethodBody GetMethodBody()
        {
            var md = method as IKVM.Reflection.Reader.MethodDefImpl;
            if (md != null)
                return md.GetMethodBody(this);

            throw new NotSupportedException();
        }

        public override int __MethodRVA
        {
            get { return method.__MethodRVA; }
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return new GenericMethodInstance(declaringType, method, typeArguments);
        }

        public override bool IsGenericMethod
        {
            get { return method.IsGenericMethod; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return method.IsGenericMethodDefinition && methodArgs == null; }
        }

        public override bool ContainsGenericParameters
        {
            get
            {
                if (declaringType.ContainsGenericParameters)
                    return true;

                if (methodArgs != null)
                    foreach (var type in methodArgs)
                        if (type.ContainsGenericParameters)
                            return true;

                return false;
            }
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            if (IsGenericMethod)
            {
                if (IsGenericMethodDefinition)
                    return this;
                else if (declaringType.IsConstructedGenericType)
                    return new GenericMethodInstance(declaringType, method, null);
                else
                    return method;
            }

            throw new InvalidOperationException();
        }

        public override MethodBase __GetMethodOnTypeDefinition()
        {
            return method;
        }

        public override Type[] GetGenericArguments()
        {
            if (methodArgs == null)
                return method.GetGenericArguments();
            else
                return (Type[])methodArgs.Clone();
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            if (methodArgs == null)
                return method.GetGenericMethodArgument(index);
            else
                return methodArgs[index];
        }

        internal override int GetGenericMethodArgumentCount()
        {
            return method.GetGenericMethodArgumentCount();
        }

        internal override MethodInfo GetMethodOnTypeDefinition()
        {
            return method.GetMethodOnTypeDefinition();
        }

        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            if (methodArgs == null)
                return module.ImportMethodOrField(declaringType, method.Name, method.MethodSignature);
            else
                return module.ImportMethodSpec(declaringType, method, methodArgs);
        }

        internal override MethodSignature MethodSignature
        {
            get { return lazyMethodSignature ?? (lazyMethodSignature = method.MethodSignature.Bind(declaringType, methodArgs)); }
        }

        internal override MethodBase BindTypeParameters(Type type)
        {
            System.Diagnostics.Debug.Assert(methodArgs == null);
            return new GenericMethodInstance(declaringType.BindTypeParameters(type), method, null);
        }

        internal override bool HasThis
        {
            get { return method.HasThis; }
        }

        public override MethodInfo[] __GetMethodImpls()
        {
            var methods = method.__GetMethodImpls();
            for (int i = 0; i < methods.Length; i++)
                methods[i] = (MethodInfo)methods[i].BindTypeParameters(declaringType);

            return methods;
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
