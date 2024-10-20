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
namespace IKVM.Reflection
{

    sealed class GenericParameterInfoImpl : ParameterInfo
    {

        readonly GenericMethodInstance method;
        readonly ParameterInfo parameterInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameterInfo"></param>
        internal GenericParameterInfoImpl(GenericMethodInstance method, ParameterInfo parameterInfo)
        {
            this.method = method;
            this.parameterInfo = parameterInfo;
        }

        public override string Name
        {
            get { return parameterInfo.Name; }
        }

        public override Type ParameterType
        {
            get { return parameterInfo.ParameterType.BindTypeParameters(method); }
        }

        public override ParameterAttributes Attributes
        {
            get { return parameterInfo.Attributes; }
        }

        public override int Position
        {
            get { return parameterInfo.Position; }
        }

        public override object RawDefaultValue
        {
            get { return parameterInfo.RawDefaultValue; }
        }

        public override CustomModifiers __GetCustomModifiers()
        {
            return parameterInfo.__GetCustomModifiers().Bind(method);
        }

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
        {
            return parameterInfo.__TryGetFieldMarshal(out fieldMarshal);
        }

        public override MemberInfo Member
        {
            get { return method; }
        }

        public override int MetadataToken
        {
            get { return parameterInfo.MetadataToken; }
        }

        public override Module Module
        {
            get { return method.Module; }
        }

    }

}
