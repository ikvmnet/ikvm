using System;

using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    /// <summary>
    /// Fake <see cref="ParameterInfo"/> implementation that wraps a <see cref="ParameterBuilder"/>, which does not extend <see cref="ParameterInfo"/>.
    /// </summary>
    class IkvmReflectionParameterBuilderInfo : ParameterInfo
    {

        readonly MemberInfo _member;
        readonly ParameterBuilder _parameter;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionParameterBuilderInfo(MemberInfo member, ParameterBuilder parameter)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => (ParameterAttributes)_parameter.Attributes;

        /// <inheritdoc />
        public override Module Module => _member.Module;

        /// <inheritdoc />
        public override MemberInfo Member => _member;

        /// <inheritdoc />
        public override string? Name => _parameter.Name;

        /// <inheritdoc />
        public override int Position => _parameter.Position;

        /// <inheritdoc />
        public override int MetadataToken => throw new NotImplementedException();

        /// <inheritdoc />
        public override Type ParameterType => throw new NotImplementedException();

        /// <inheritdoc />
        public override object RawDefaultValue => throw new NotImplementedException();

        public override CustomModifiers __GetCustomModifiers()
        {
            throw new NotSupportedException();
        }

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
        {
            throw new NotSupportedException();
        }

    }

}
