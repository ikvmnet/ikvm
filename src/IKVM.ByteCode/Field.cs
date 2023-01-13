using System.Collections.Generic;

namespace IKVM.ByteCode
{

    public class Field
    {

        /// <summary>
        /// Gets or sets the access flags of the field.
        /// </summary>
        public AccessFlag AccessFlags { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the descriptor of the field.
        /// </summary>
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets the list of attributes applied to the field.
        /// </summary>
        public IList<Attribute> Attributes { get; } = new List<Attribute>();

    }

}
