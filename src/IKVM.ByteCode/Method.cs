using System.Collections;
using System.Collections.Generic;

namespace IKVM.ByteCode
{

    public class Method
    {

        /// <summary>
        /// Gets or sets the access flags of the method.
        /// </summary>
        public AccessFlag AccessFlags { get; set; }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the descriptor of the method.
        /// </summary>
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets the list of attributes applied to the method.
        /// </summary>
        public IList<Attribute> Attributes { get; } = new List<Attribute>();

    }

}
