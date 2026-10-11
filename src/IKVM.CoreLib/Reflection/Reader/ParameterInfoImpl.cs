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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    sealed class ParameterInfoImpl : ParameterInfo
    {

        readonly MethodDefImpl method;
        readonly int position;
        readonly ParameterHandle handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="position"></param>
        /// <param name="handle">The parameter row, or a nil handle for a parameter without one.</param>
        internal ParameterInfoImpl(MethodDefImpl method, int position, ParameterHandle handle)
        {
            this.method = method;
            this.position = position;
            this.handle = handle;
        }

        ModuleReader ModuleReader => (ModuleReader)method.Module;

        public override string Name => handle.IsNil ? null : ModuleReader.GetString(ModuleReader.Metadata.GetParameter(handle).Name);

        public override Type ParameterType => position == -1 ? method.MethodSignature.GetReturnType(method) : method.MethodSignature.GetParameterType(method, position);

        public override ParameterAttributes Attributes => handle.IsNil ? ParameterAttributes.None : (ParameterAttributes)ModuleReader.Metadata.GetParameter(handle).Attributes;

        public override int Position => position;

        public override object RawDefaultValue
        {
            get
            {
                if ((Attributes & ParameterAttributes.HasDefault) != 0)
                    return ModuleReader.GetConstantValue(ModuleReader.Metadata.GetParameter(handle).GetDefaultValue());

                if (TryGetCustomConstant(out var value))
                    return value;

                if ((Attributes & ParameterAttributes.Optional) != 0)
                    return Missing.Value;

                return null;
            }
        }

        public override CustomModifiers __GetCustomModifiers()
        {
            return position == -1 ? method.MethodSignature.GetReturnTypeCustomModifiers(method) : method.MethodSignature.GetParameterCustomModifiers(method, position);
        }

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => FieldMarshal.ReadFieldMarshal(Module, MetadataToken, out fieldMarshal);

        // return the right ConstructorInfo wrapper
        public override MemberInfo Member => method.Module.ResolveMethod(method.MetadataToken);

        // like .NET, a parameter without a row in the Param table has token 0x08000000
        public override int MetadataToken => handle.IsNil ? 0x08000000 : MetadataTokens.GetToken(handle);

        public override Module Module => method.Module;

    }

}
