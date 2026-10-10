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

        readonly Type _declaringType;
        readonly MethodInfo _method;
        readonly Type[] _methodArgs;
        MethodSignature _lazyMethodSignature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        internal GenericMethodInstance(Type declaringType, MethodInfo method, Type[] methodArgs)
        {
            System.Diagnostics.Debug.Assert(method is not GenericMethodInstance);
            this._declaringType = declaringType;
            this._method = method;
            this._methodArgs = methodArgs;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericMethodInstance;
            return other != null
                && other._method.Equals(_method)
                && other._declaringType.Equals(_declaringType)
                && Util.ArrayEquals(other._methodArgs, _methodArgs);
        }

        public override int GetHashCode()
        {
            return _declaringType.GetHashCode() * 33 ^ _method.GetHashCode() ^ Util.GetHashCode(_methodArgs);
        }

        public override Type ReturnType
        {
            get { return _method.ReturnType.BindTypeParameters(this); }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new GenericParameterInfoImpl(this, _method.ReturnParameter); }
        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = _method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new GenericParameterInfoImpl(this, parameters[i]);

            return parameters;
        }

        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = _method.GetParameterTypes();
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = parameterTypes[i].BindTypeParameters(_method);

            return parameterTypes;
        }

        internal override int ParameterCount
        {
            get { return _method.ParameterCount; }
        }

        public override CallingConventions CallingConvention
        {
            get { return _method.CallingConvention; }
        }

        public override MethodAttributes Attributes
        {
            get { return _method.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _method.GetMethodImplementationFlags();
        }

        public override string Name
        {
            get { return _method.Name; }
        }

        public override Type DeclaringType
        {
            get { return _declaringType.IsModulePseudoType ? null : _declaringType; }
        }

        public override Module Module
        {
            get { return _method.Module; }
        }

        public override int MetadataToken
        {
            get { return _method.MetadataToken; }
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return new GenericMethodInstance(_declaringType, _method, typeArguments);
        }

        public override bool IsGenericMethod
        {
            get { return _method.IsGenericMethod; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return _method.IsGenericMethodDefinition && _methodArgs == null; }
        }

        public override bool ContainsGenericParameters
        {
            get
            {
                if (_declaringType.ContainsGenericParameters)
                    return true;

                if (_methodArgs != null)
                    foreach (var type in _methodArgs)
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
                else if (_declaringType.IsConstructedGenericType)
                    return new GenericMethodInstance(_declaringType, _method, null);
                else
                    return _method;
            }

            throw new InvalidOperationException();
        }

        public override Type[] GetGenericArguments()
        {
            if (_methodArgs == null)
                return _method.GetGenericArguments();
            else
                return (Type[])_methodArgs.Clone();
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            if (_methodArgs == null)
                return _method.GetGenericMethodArgument(index);
            else
                return _methodArgs[index];
        }


        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            if (_methodArgs == null)
                return module.ImportMethodOrField(_declaringType, _method.Name, _method.MethodSignature);
            else
                return module.ImportMethodSpec(_declaringType, _method, _methodArgs);
        }

        internal override MethodSignature MethodSignature
        {
            get { return _lazyMethodSignature ?? (_lazyMethodSignature = _method.MethodSignature.Bind(_declaringType, _methodArgs)); }
        }

        internal override MethodBase BindTypeParameters(Type type)
        {
            System.Diagnostics.Debug.Assert(_methodArgs == null);
            return new GenericMethodInstance(_declaringType.BindTypeParameters(type), _method, null);
        }

        internal override int GetCurrentToken()
        {
            return _method.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return _method.IsBaked; }
        }

    }

}
