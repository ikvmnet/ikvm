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

    /// <summary>
    /// Discovers the attributes of a parameter and provides access to parameter metadata.
    /// </summary>
    abstract class ParameterInfo : ICustomAttributeProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ParameterInfo()
        {

        }

        /// <summary>
        /// Determines whether the specified object is a parameter of the same member at the same position.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> represents the same parameter; otherwise, <see langword="false"/>.</returns>
        public sealed override bool Equals(object obj)
        {
            var other = obj as ParameterInfo;
            return other != null && other.Member == Member && other.Position == Position;
        }

        /// <inheritdoc />
        public sealed override int GetHashCode() => Member.GetHashCode() * 1777 + Position;

        /// <summary>
        /// Determines whether two parameters are equal.
        /// </summary>
        /// <param name="p1">The first parameter to compare.</param>
        /// <param name="p2">The second parameter to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="p1"/> equals <paramref name="p2"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ParameterInfo p1, ParameterInfo p2) =>
            ReferenceEquals(p1, p2) || (!ReferenceEquals(p1, null) && p1.Equals(p2));

        /// <summary>
        /// Determines whether two parameters are not equal.
        /// </summary>
        /// <param name="p1">The first parameter to compare.</param>
        /// <param name="p2">The second parameter to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="p1"/> does not equal <paramref name="p2"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(ParameterInfo p1, ParameterInfo p2) => !(p1 == p2);

        /// <summary>
        /// Gets the name of the parameter, or <see langword="null"/> if the parameter has no name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of this parameter.
        /// </summary>
        public abstract Type ParameterType { get; }

        /// <summary>
        /// Gets the attributes for this parameter.
        /// </summary>
        public abstract ParameterAttributes Attributes { get; }

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list, or -1 for the return parameter.
        /// </summary>
        public abstract int Position { get; }

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value. This is
        /// <see cref="Missing.Value"/> for an optional parameter without a default value, and <see langword="null"/>
        /// when there is no default value.
        /// </summary>
        public abstract object RawDefaultValue { get; }

        /// <summary>
        /// Gets the optional and required custom modifiers of the parameter, in signature order.
        /// </summary>
        /// <returns>The custom modifiers of the parameter.</returns>
        public abstract CustomModifiers __GetCustomModifiers();

        /// <summary>
        /// Attempts to read the marshalling information recorded for the parameter in the FieldMarshal table.
        /// </summary>
        /// <param name="fieldMarshal">When this method returns, contains the marshalling information if there is any; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the parameter has marshalling information; otherwise, <see langword="false"/>.</returns>
        public abstract bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal);

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        public abstract MemberInfo Member { get; }

        /// <summary>
        /// Gets a value that identifies this parameter in metadata.
        /// </summary>
        public abstract int MetadataToken { get; }

        /// <summary>
        /// Gets the module in which the parameter is defined.
        /// </summary>
        public abstract Module Module { get; }

        /// <summary>
        /// Gets the optional custom modifiers of the parameter.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the optional custom modifiers of the current parameter.</returns>
        public Type[] GetOptionalCustomModifiers() => __GetCustomModifiers().GetOptional();

        /// <summary>
        /// Gets the required custom modifiers of the parameter.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that identify the required custom modifiers of the current parameter.</returns>
        public Type[] GetRequiredCustomModifiers() => __GetCustomModifiers().GetRequired();

        /// <summary>
        /// Gets a value indicating whether this is an input parameter.
        /// </summary>
        public bool IsIn => (Attributes & ParameterAttributes.In) != 0;

        /// <summary>
        /// Gets a value indicating whether this is an output parameter.
        /// </summary>
        public bool IsOut => (Attributes & ParameterAttributes.Out) != 0;

        /// <summary>
        /// Gets a value indicating whether this parameter is a locale identifier (lcid).
        /// </summary>
        public bool IsLcid => (Attributes & ParameterAttributes.Lcid) != 0;

        /// <summary>
        /// Gets a value indicating whether this is a <c>Retval</c> parameter.
        /// </summary>
        public bool IsRetval => (Attributes & ParameterAttributes.Retval) != 0;

        /// <summary>
        /// Gets a value indicating whether this parameter is optional.
        /// </summary>
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
        /// <param name="value">When this method returns, contains the <see cref="decimal"/> or <see cref="System.DateTime"/> default value if one was found; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if a custom constant default value was found; otherwise, <see langword="false"/>.</returns>
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

        /// <summary>
        /// Determines whether the custom attribute of the specified type or its derived types is applied to this
        /// parameter.
        /// </summary>
        /// <param name="attributeType">The type of the custom attribute to search for.</param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns><see langword="true"/> if one or more instances of <paramref name="attributeType"/> or its derived types are applied to this parameter; otherwise, <see langword="false"/>.</returns>
        public bool IsDefined(Type attributeType, bool inherit) => CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;

        /// <summary>
        /// Returns the custom attributes of the specified type or its derived types that are applied to this parameter.
        /// </summary>
        /// <param name="attributeType">The type of the custom attributes to return.</param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>The matching custom attributes.</returns>
        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit) =>
            CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects for the current parameter.
        /// </summary>
        /// <returns>A list of objects that represent data about the attributes that have been applied to the current parameter.</returns>
        public IList<CustomAttributeData> GetCustomAttributesData() => CustomAttributeData.GetCustomAttributes(this);

    }

    /// <summary>
    /// Wraps a <see cref="ParameterInfo"/> so that it reports a different <see cref="ParameterInfo.Member"/>, such as a
    /// member wrapper that records a reflected type. All other members forward to the wrapped parameter, except
    /// <see cref="ParameterInfo.Module"/>, which comes from the reported member.
    /// </summary>
    sealed class ParameterInfoWrapper : ParameterInfo
    {

        readonly MemberInfo _member;
        readonly ParameterInfo _forward;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member">The member to report as the member the parameter belongs to.</param>
        /// <param name="forward">The parameter to forward to.</param>
        internal ParameterInfoWrapper(MemberInfo member, ParameterInfo forward)
        {
            _member = member;
            _forward = forward;
        }

        /// <inheritdoc />
        public override string Name => _forward.Name;

        /// <inheritdoc />
        public override Type ParameterType => _forward.ParameterType;

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _forward.Attributes;

        /// <inheritdoc />
        public override int Position => _forward.Position;

        /// <inheritdoc />
        public override object RawDefaultValue => _forward.RawDefaultValue;

        /// <inheritdoc />
        public override CustomModifiers __GetCustomModifiers() => _forward.__GetCustomModifiers();

        /// <inheritdoc />
        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => _forward.__TryGetFieldMarshal(out fieldMarshal);

        /// <inheritdoc />
        public override MemberInfo Member => _member;

        /// <inheritdoc />
        public override int MetadataToken => _forward.MetadataToken;

        /// <inheritdoc />
        public override Module Module => _member.Module;

    }

}
