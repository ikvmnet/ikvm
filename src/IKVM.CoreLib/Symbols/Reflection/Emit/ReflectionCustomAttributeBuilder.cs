using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionCustomAttributeBuilder : ICustomAttributeBuilder
    {

        readonly CustomAttributeBuilder _builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionCustomAttributeBuilder(CustomAttributeBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the underlying reflection <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        internal CustomAttributeBuilder UnderlyingBuilder => _builder;

    }

}
