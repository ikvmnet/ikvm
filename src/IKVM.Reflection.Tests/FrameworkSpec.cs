using System;
using System.Collections.Generic;
using System.Linq;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Describes a target framework whose reference assemblies a test loads into a <see cref="Universe"/>.
    /// </summary>
    /// <param name="Tfm"></param>
    /// <param name="TargetFrameworkIdentifier"></param>
    /// <param name="TargetFrameworkVersion"></param>
    public record struct FrameworkSpec(string Tfm, string TargetFrameworkIdentifier, string TargetFrameworkVersion)
    {

        static readonly FrameworkSpec[] all =
        [
            new FrameworkSpec("net472", ".NETFramework", "4.7.2"),
            new FrameworkSpec("net481", ".NETFramework", "4.8.1"),
            new FrameworkSpec("net6.0", ".NET", "6.0"),
            new FrameworkSpec("net8.0", ".NET", "8.0"),
        ];

        /// <summary>
        /// Gets the frameworks to test, as dynamic data rows of TFM strings.
        /// </summary>
        public static IEnumerable<object[]> GetFrameworkTestData() => all.Select(i => new object[] { i.Tfm });

        /// <summary>
        /// Gets the <see cref="FrameworkSpec"/> for the given TFM.
        /// </summary>
        /// <param name="tfm"></param>
        /// <returns></returns>
        public static FrameworkSpec Get(string tfm) => all.FirstOrDefault(i => i.Tfm == tfm) is { Tfm: not null } s ? s : throw new ArgumentException($"Unknown framework '{tfm}'.", nameof(tfm));

    }

}
