using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionCustomAttributeBuilder : ICustomAttributeBuilder
    {

        readonly CustomAttributeBuilder _builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReflectionCustomAttributeBuilder(CustomAttributeBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the underlying reflection <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        internal CustomAttributeBuilder UnderlyingBuilder => _builder;

    }

}
