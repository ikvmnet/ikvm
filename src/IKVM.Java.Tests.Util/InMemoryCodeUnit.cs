using System;

namespace IKVM.Java.Tests.Util
{
    /// <summary>
    /// Represents a unit of code to compile.
    /// </summary>
    public class InMemoryCodeUnit
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InMemoryCodeUnit(string name, string code)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        /// <summary>
        /// Represents the class name to be compiled.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the contents of the class to be compiled.
        /// </summary>
        public string Code { get; set; }

    }

}
