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

        readonly ParameterBuilder _builder;
        readonly Func<object?> _getDefaultValue;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="getParameterType"></param>
        /// <param name="getDefaultValue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionParameterBuilderInfo(ParameterBuilder builder, Func<object?> getDefaultValue)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _getDefaultValue = getDefaultValue ?? throw new ArgumentNullException(nameof(getDefaultValue));
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => (ParameterAttributes)_builder.Attributes;

        /// <inheritdoc />
        public override MemberInfo Member => throw new NotImplementedException();

        /// <inheritdoc />
        public override Type ParameterType => ((MethodBase)Member).GetParameters()[Position].ParameterType;

        /// <inheritdoc />
        public override Module Module => _builder.Module;

        /// <inheritdoc />
        public override string? Name => _builder.Name;

        /// <inheritdoc />
        public override int Position => _builder.Position;

        /// <inheritdoc />
        public override int MetadataToken => throw new NotImplementedException();

        /// <inheritdoc />
        public override object? RawDefaultValue => _getDefaultValue();

        /// <inheritdoc />
        public override CustomModifiers __GetCustomModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
        {
            throw new NotImplementedException();
        }

    }

}
