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

        readonly Type _reflectedType;
        readonly MethodInfo _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="method"></param>
        internal MethodInfoWithReflectedType(Type reflectedType, MethodInfo method)
        {
            Debug.Assert(reflectedType != method.DeclaringType);
            _reflectedType = reflectedType;
            _method = method;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MethodInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._method == _method;
        }

        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _method.GetHashCode();

        internal override MethodSignature MethodSignature => _method.MethodSignature;

        internal override int ParameterCount => _method.ParameterCount;

        public override ParameterInfo[] GetParameters()
        {
            var parameters = _method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        internal override Type[] GetParameterTypes() => _method.GetParameterTypes();

        public override MethodAttributes Attributes => _method.Attributes;

        public override MethodImplAttributes GetMethodImplementationFlags() => _method.GetMethodImplementationFlags();

        public override CallingConventions CallingConvention => _method.CallingConvention;

        public override Type ReturnType => _method.ReturnType;

        public override ParameterInfo ReturnParameter => new ParameterInfoWrapper(this, _method.ReturnParameter);

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) =>
            SetReflectedType(_method.MakeGenericMethod(typeArguments), _reflectedType);

        public override MethodInfo GetGenericMethodDefinition() => _method.GetGenericMethodDefinition();

        public override string ToString() => _method.ToString();

        internal override Type GetGenericMethodArgument(int index) => _method.GetGenericMethodArgument(index);

        public override Module Module => _method.Module;

        public override Type DeclaringType => _method.DeclaringType;

        public override string Name => _method.Name;

        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module) => _method.ImportTo(module);

        public override bool __IsMissing => _method.__IsMissing;

        internal override MethodBase BindTypeParameters(Type type) => _method.BindTypeParameters(type);

        public override bool ContainsGenericParameters => _method.ContainsGenericParameters;

        public override Type[] GetGenericArguments() => _method.GetGenericArguments();

        public override bool IsGenericMethod => _method.IsGenericMethod;

        public override bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

        public override int MetadataToken => _method.MetadataToken;

        internal override int GetCurrentToken() => _method.GetCurrentToken();

        internal override bool IsBaked => _method.IsBaked;

    }

}
