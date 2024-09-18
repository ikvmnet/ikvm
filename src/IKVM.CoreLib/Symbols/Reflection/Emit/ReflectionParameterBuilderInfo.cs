using System;
using System.Reflection;
using System.Reflection.Emit;
using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Fake <see cref="ParameterInfo"/> implementation that wraps a <see cref="ParameterBuilder"/>, which does not extend <see cref="ParameterInfo"/>.
    /// </summary>
    class ReflectionParameterBuilderInfo : ParameterInfo
    {

        readonly ParameterBuilder _builder;
        readonly Func<object?> _getDefaultValue;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="getDefaultValue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionParameterBuilderInfo(ParameterBuilder builder, Func<object?> getDefaultValue)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _getDefaultValue = getDefaultValue ?? throw new ArgumentNullException(nameof(getDefaultValue));
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => (ParameterAttributes)_builder.Attributes;

        /// <inheritdoc />
        public override MemberInfo Member => _builder.GetMethodBuilder();

        /// <inheritdoc />
        public override string? Name => _builder.Name;

        /// <inheritdoc />
        public override int Position => _builder.Position;

        /// <inheritdoc />
        public override int MetadataToken => _builder.GetMetadataToken();

        /// <inheritdoc />
        public override object? DefaultValue => _getDefaultValue();

    }

}
