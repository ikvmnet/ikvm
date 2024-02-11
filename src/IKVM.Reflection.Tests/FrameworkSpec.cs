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
            yield return new object[] { new FrameworkSpec("net48", ".NETFramework", "4.8") };
            yield return new object[] { new FrameworkSpec("net6.0", ".NETCore", "6.0") };
            yield return new object[] { new FrameworkSpec("net7.0", ".NETCore", "7.0") };
        }

    }

}
