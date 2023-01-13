using System.Collections.Generic;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Mutable Java class instance.
    /// </summary>
    public class Class
    {

        /// <summary>
        /// Gets or sets the major version of the class.
        /// </summary>
        public int MajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the minor version of the class.
        /// </summary>
        public int MinorVersion { get; set; }

        /// <summary>
        /// Gets or sets the access flags of the class.
        /// </summary>
        public AccessFlag AccessFlags { get; set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the super-class of the class.
        /// </summary>
        public string SuperName { get; set; }

        /// <summary>
        /// Gets the set of interfaces implemented by this class.
        /// </summary>
        public IList<Interface> Interfaces { get;  } = new List<Interface>();

        /// <summary>
        /// Gets the set of fields implemented by this class.
        /// </summary>
        public IList<Field> Fields { get;  } = new List<Field>();

        /// <summary>
        /// Gets the set of methods implemented by this class.
        /// </summary>
        public IList<Method> Methods { get; } = new List<Method>();

        /// <summary>
        /// Gets the set of attributes applied to the class.
        /// </summary>
        public IList<Attribute> Attributes { get; } = new List<Attribute>();

    }

}
