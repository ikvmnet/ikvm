using System;

namespace IKVM.Tools.Importer.MapXml
{

    /// <summary>
    /// Describes a map XML instruction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class InstructionAttribute : System.Attribute
    {

        readonly string elementName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InstructionAttribute(string elementName)
        {
            this.elementName = elementName ?? throw new ArgumentNullException(nameof(elementName));
        }

        /// <summary>
        /// Gets the name of the XML element corresponding to the instruction.
        /// </summary>
        public string ElementName => elementName;

    }

}
