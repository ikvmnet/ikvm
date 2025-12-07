using System.Collections.Generic;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Tfm"></param>
    /// <param name="TargetFrameworkIdentifier"></param>
    /// <param name="TargetFrameworkVersion"></param>
    public record struct FrameworkSpec(string Tfm, string TargetFrameworkIdentifier, string TargetFrameworkVersion)
    {

        /// <summary>
        /// Individual Frameworks to test.
        /// </summary>
        public static IEnumerable<object[]> GetFrameworkTestData()
        {
            yield return new object[] { new FrameworkSpec("net472", ".NETFramework", "4.7.2") };
            yield return new object[] { new FrameworkSpec("net481", ".NETFramework", "4.8.1") };
            yield return new object[] { new FrameworkSpec("net6.0", ".NET", "6.0") };
            yield return new object[] { new FrameworkSpec("net8.0", ".NET", "8.0") };
        }

    }

}
