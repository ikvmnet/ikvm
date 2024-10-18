using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Fake <see cref="ParameterInfo"/> implementation that is used for instantiated method parameters.
    /// </summary>
    class ReflectionParameterSpecInfo : ParameterInfo
    {

        readonly MemberInfo _member;
        readonly ParameterInfo _parameter;
        readonly Type[] _genericTypeArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="parameter"></param>
        /// <param name="genericTypeArguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionParameterSpecInfo(MemberInfo member, ParameterInfo parameter, Type[] genericTypeArguments)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
            _genericTypeArguments = genericTypeArguments ?? throw new ArgumentNullException(nameof(genericTypeArguments));
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => (ParameterAttributes)_parameter.Attributes;

        /// <inheritdoc />
        public override MemberInfo Member => _member;

        /// <inheritdoc />
        public override string? Name => _parameter.Name;

        /// <inheritdoc />
        public override int Position => _parameter.Position;

        /// <inheritdoc />
        public override int MetadataToken => throw new InvalidOperationException();

        /// <inheritdoc />
        public override Type ParameterType => _parameter.ParameterType.SubstituteGenericTypes(_genericTypeArguments);

    }

}
