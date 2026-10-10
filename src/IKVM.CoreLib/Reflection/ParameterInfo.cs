/*
  Copyright (C) 2009 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    abstract class ParameterInfo : ICustomAttributeProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ParameterInfo()
        {
        }

        public sealed override bool Equals(object obj)
        {
            var other = obj as ParameterInfo;
            return other != null && other.Member == Member && other.Position == Position;
        }

        public sealed override int GetHashCode() => Member.GetHashCode() * 1777 + Position;

        public static bool operator ==(ParameterInfo p1, ParameterInfo p2) =>
            ReferenceEquals(p1, p2) || (!ReferenceEquals(p1, null) && p1.Equals(p2));

        public static bool operator !=(ParameterInfo p1, ParameterInfo p2) => !(p1 == p2);

        public abstract string Name { get; }

        public abstract Type ParameterType { get; }

        public abstract ParameterAttributes Attributes { get; }

        public abstract int Position { get; }

        public abstract object RawDefaultValue { get; }

        public abstract CustomModifiers __GetCustomModifiers();

        public abstract bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal);

        public abstract MemberInfo Member { get; }

        public abstract int MetadataToken { get; }

        public abstract Module Module { get; }

        public Type[] GetOptionalCustomModifiers() => __GetCustomModifiers().GetOptional();

        public Type[] GetRequiredCustomModifiers() => __GetCustomModifiers().GetRequired();

        public bool IsIn => (Attributes & ParameterAttributes.In) != 0;

        public bool IsOut => (Attributes & ParameterAttributes.Out) != 0;

        public bool IsLcid => (Attributes & ParameterAttributes.Lcid) != 0;

        public bool IsRetval => (Attributes & ParameterAttributes.Retval) != 0;

        public bool IsOptional => (Attributes & ParameterAttributes.Optional) != 0;

        /// <summary>
        /// Gets whether the parameter has a default value. Like System.Reflection, a <see cref="decimal"/> or
        /// <see cref="System.DateTime"/> default recorded as a custom constant attribute counts.
        /// </summary>
        public bool HasDefaultValue => (Attributes & ParameterAttributes.HasDefault) != 0 || TryGetCustomConstant(out _);

        /// <summary>
        /// Attempts to read a default value that the compiler recorded as a DecimalConstantAttribute or
        /// DateTimeConstantAttribute, because metadata constants cannot hold those types.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryGetCustomConstant(out object value)
        {
            var universe = Module.Universe;
            var type = ParameterType;

            if (type == universe.System_Decimal && universe.System_Runtime_CompilerServices_DecimalConstantAttribute is { } decimalConstant)
            {
                foreach (var cad in CustomAttributeData.__GetCustomAttributes(this, decimalConstant, false))
                {
                    var args = cad.ConstructorArguments;
                    if (args.Count != 5 || args[0].ArgumentType != universe.System_Byte || args[1].ArgumentType != universe.System_Byte)
                        continue;

                    // the constructor takes the 96 bit integer either as signed or as unsigned ints
                    if (args[2].ArgumentType == universe.System_Int32 && args[3].ArgumentType == universe.System_Int32 && args[4].ArgumentType == universe.System_Int32)
                    {
                        value = new decimal((int)args[4].Value, (int)args[3].Value, (int)args[2].Value, (byte)args[1].Value != 0, (byte)args[0].Value);
                        return true;
                    }

                    if (args[2].ArgumentType == universe.System_UInt32 && args[3].ArgumentType == universe.System_UInt32 && args[4].ArgumentType == universe.System_UInt32)
                    {
                        value = new decimal(unchecked((int)(uint)args[4].Value), unchecked((int)(uint)args[3].Value), unchecked((int)(uint)args[2].Value), (byte)args[1].Value != 0, (byte)args[0].Value);
                        return true;
                    }
                }
            }
            else if (type == universe.System_DateTime && universe.System_Runtime_CompilerServices_DateTimeConstantAttribute is { } dateTimeConstant)
            {
                foreach (var cad in CustomAttributeData.__GetCustomAttributes(this, dateTimeConstant, false))
                {
                    var args = cad.ConstructorArguments;
                    if (args.Count == 1 && args[0].ArgumentType == universe.System_Int64)
                    {
                        value = new System.DateTime((long)args[0].Value);
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }

        public bool IsDefined(Type attributeType, bool inherit) => CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;

        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit) =>
            CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);

        public IList<CustomAttributeData> GetCustomAttributesData() => CustomAttributeData.GetCustomAttributes(this);

    }

    sealed class ParameterInfoWrapper : ParameterInfo
    {

        readonly MemberInfo _member;
        readonly ParameterInfo _forward;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="forward"></param>
        internal ParameterInfoWrapper(MemberInfo member, ParameterInfo forward)
        {
            _member = member;
            _forward = forward;
        }

        public override string Name => _forward.Name;

        public override Type ParameterType => _forward.ParameterType;

        public override ParameterAttributes Attributes => _forward.Attributes;

        public override int Position => _forward.Position;

        public override object RawDefaultValue => _forward.RawDefaultValue;

        public override CustomModifiers __GetCustomModifiers() => _forward.__GetCustomModifiers();

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => _forward.__TryGetFieldMarshal(out fieldMarshal);

        public override MemberInfo Member => _member;

        public override int MetadataToken => _forward.MetadataToken;

        public override Module Module => _member.Module;

    }

}
