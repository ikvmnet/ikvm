using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Fake <see cref="EventInfo"/> implementation that wraps a <see cref="EventBuilder"/>, which does not extend <see cref="EventInfo"/>.
    /// </summary>
    class ReflectionEventBuilderInfo : EventInfo
    {

        readonly EventBuilder _builder;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionEventBuilderInfo(EventBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override Module Module => _builder.GetModuleBuilder();

        /// <inheritdoc />
        public override Type? DeclaringType => _builder.GetTypeBuilder();

        /// <inheritdoc />
        public override EventAttributes Attributes => _builder.GetEventAttributes();

        /// <inheritdoc />
        public override string Name => _builder.GetEventName();

        /// <inheritdoc />
        public override Type? ReflectedType => DeclaringType;

        /// <inheritdoc />
        public override int MetadataToken => _builder.GetMetadataToken();

        /// <inheritdoc />
        public override MethodInfo? GetAddMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodInfo? GetRemoveMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodInfo? GetRaiseMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

    }

}
