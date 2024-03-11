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
using System;
using System.Collections.Generic;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class ParameterInfoImpl : ParameterInfo
    {

        readonly MethodDefImpl method;
        readonly int position;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="position"></param>
        /// <param name="index"></param>
        internal ParameterInfoImpl(MethodDefImpl method, int position, int index)
        {
            this.method = method;
            this.position = position;
            this.index = index;
        }

        public override string Name
        {
            get { return index == -1 ? null : ((ModuleReader)this.Module).GetString(this.Module.ParamTable.records[index].Name); }
        }

        public override Type ParameterType
        {
            get { return position == -1 ? method.MethodSignature.GetReturnType(method) : method.MethodSignature.GetParameterType(method, position); }
        }

        public override ParameterAttributes Attributes
        {
            get { return index == -1 ? ParameterAttributes.None : (ParameterAttributes)this.Module.ParamTable.records[index].Flags; }
        }

        public override int Position
        {
            get { return position; }
        }

        public override object RawDefaultValue
        {
            get
            {
                if ((Attributes & ParameterAttributes.HasDefault) != 0)
                    return Module.ConstantTable.GetRawConstantValue(Module, MetadataToken);

                var universe = Module.Universe;
                if (ParameterType == universe.System_Decimal)
                {
                    var attr = universe.System_Runtime_CompilerServices_DecimalConstantAttribute;
                    if (attr != null)
                    {
                        foreach (var cad in CustomAttributeData.__GetCustomAttributes(this, attr, false))
                        {
                            var args = cad.ConstructorArguments;
                            if (args.Count == 5)
                            {
                                if (args[0].ArgumentType == universe.System_Byte &&
                                    args[1].ArgumentType == universe.System_Byte &&
                                    args[2].ArgumentType == universe.System_Int32 &&
                                    args[3].ArgumentType == universe.System_Int32 &&
                                    args[4].ArgumentType == universe.System_Int32)
                                {
                                    return new decimal((int)args[4].Value, (int)args[3].Value, (int)args[2].Value, (byte)args[1].Value != 0, (byte)args[0].Value);
                                }
                                else if (args[0].ArgumentType == universe.System_Byte &&
                                    args[1].ArgumentType == universe.System_Byte &&
                                    args[2].ArgumentType == universe.System_UInt32 &&
                                    args[3].ArgumentType == universe.System_UInt32 &&
                                    args[4].ArgumentType == universe.System_UInt32)
                                {
                                    return new Decimal(unchecked((int)(uint)args[4].Value), unchecked((int)(uint)args[3].Value), unchecked((int)(uint)args[2].Value), (byte)args[1].Value != 0, (byte)args[0].Value);
                                }
                            }
                        }
                    }
                }

                if ((Attributes & ParameterAttributes.Optional) != 0)
                    return Missing.Value;

                return null;
            }
        }

        public override CustomModifiers __GetCustomModifiers()
        {
            return position == -1 ? method.MethodSignature.GetReturnTypeCustomModifiers(method) : method.MethodSignature.GetParameterCustomModifiers(method, position);
        }

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
        {
            return FieldMarshal.ReadFieldMarshal(this.Module, this.MetadataToken, out fieldMarshal);
        }

        public override MemberInfo Member
        {
            get
            {
                // return the right ConstructorInfo wrapper
                return method.Module.ResolveMethod(method.MetadataToken);
            }
        }

        public override int MetadataToken
        {
            get
            {
                // for parameters that don't have a row in the Param table, we return 0x08000000 (because index is -1 in that case),
                // just like .NET
                return (ParamTable.Index << 24) + index + 1;
            }
        }

        internal override Module Module
        {
            get { return method.Module; }
        }

    }

}
