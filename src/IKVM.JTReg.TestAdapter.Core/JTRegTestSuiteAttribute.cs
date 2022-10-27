using System;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Specifies the relative paths to test suites discoverable by this test assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class JTRegTestSuiteAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JTRegTestSuiteAttribute(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Gets or sets the relative path to the test suite directory from the test assembly.
        /// </summary>
        public string Path { get; set; }

    }

}
