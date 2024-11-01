using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionLabel : ILabel
    {

        readonly Label _label;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="label"></param>
        public ReflectionLabel(Label label)
        {
            _label = label;
        }

        /// <summary>
        /// Gets the underlying <see cref="Label"/>.
        /// </summary>
        public Label UnderlyingLabel => _label;

    }

}