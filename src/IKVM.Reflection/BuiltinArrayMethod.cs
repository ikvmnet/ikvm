/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{
    sealed class BuiltinArrayMethod : ArrayMethod
    {

        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MethodInfo method;
            readonly Type type;
            readonly int pos;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="type"></param>
            /// <param name="pos"></param>
            internal ParameterInfoImpl(MethodInfo method, Type type, int pos)
            {
                this.method = method;
                this.type = type;
                this.pos = pos;
            }

            public override Type ParameterType
            {
                get { return type; }
            }

            public override string Name
            {
                get { return null; }
            }

            public override ParameterAttributes Attributes
            {
                get { return ParameterAttributes.None; }
            }

            public override int Position
            {
                get { return pos; }
            }

            public override object RawDefaultValue
            {
                get { return null; }
            }

            public override CustomModifiers __GetCustomModifiers()
            {
                return new CustomModifiers();
            }

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                fieldMarshal = new FieldMarshal();
                return false;
            }

            public override MemberInfo Member
            {
                get { return method.IsConstructor ? (MethodBase)new ConstructorInfoImpl(method) : method; }
            }

            public override int MetadataToken
            {
                get { return 0x08000000; }
            }

            public override Module Module
            {
                get { return method.Module; }
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="arrayClass"></param>
        /// <param name="methodName"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        internal BuiltinArrayMethod(Module module, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) :
            base(module, arrayClass, methodName, callingConvention, returnType, parameterTypes)
        {

        }

        public override MethodAttributes Attributes
        {
            get { return this.Name == ".ctor" ? MethodAttributes.RTSpecialName | MethodAttributes.Public : MethodAttributes.Public; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return MethodImplAttributes.IL;
        }

        public override int MetadataToken
        {
            get { return 0x06000000; }
        }

        public override MethodBody GetMethodBody()
        {
            return null;
        }

        public override ParameterInfo[] GetParameters()
        {
            var parameterInfos = new ParameterInfo[parameterTypes.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
                parameterInfos[i] = new ParameterInfoImpl(this, parameterTypes[i], i);

            return parameterInfos;
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new ParameterInfoImpl(this, this.ReturnType, -1); }
        }

    }

}
