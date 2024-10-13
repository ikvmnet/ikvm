using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionLabel : ILabel
    {

        readonly Label _label;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="label"></param>
        public IkvmReflectionLabel(Label label)
        {
            _label = label;
        }

        /// <summary>
        /// Gets the underlying <see cref="Label"/>.
        /// </summary>
        public Label UnderlyingLabel => _label;

    }

}