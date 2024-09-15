using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Fake <see cref="ParameterInfo"/> implementation that wraps a <see cref="ParameterBuilder"/>, which does not extend <see cref="ParameterInfo"/>.
    /// </summary>
    class ReflectionParameterBuilderInfo : ParameterInfo
    {

        readonly ParameterBuilder _builder;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionParameterBuilderInfo(ParameterBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
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

    }

}
