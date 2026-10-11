using System;
using System.Collections.Generic;
using System.Linq;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Describes a target framework whose reference assemblies a test loads into a <see cref="Universe"/>.
    /// </summary>
    /// <param name="Tfm">The target framework moniker, such as <c>net8.0</c>.</param>
    /// <param name="TargetFrameworkIdentifier">The target framework identifier, such as <c>.NETFramework</c> or <c>.NET</c>.</param>
    /// <param name="TargetFrameworkVersion">The target framework version, such as <c>4.7.2</c>.</param>
    public record struct FrameworkSpec(string Tfm, string TargetFrameworkIdentifier, string TargetFrameworkVersion)
    {

        /// <summary>
        /// The frameworks tests run against.
        /// </summary>
        static readonly FrameworkSpec[] s_all =
        [
            new FrameworkSpec("net472", ".NETFramework", "4.7.2"),
            new FrameworkSpec("net481", ".NETFramework", "4.8.1"),
            new FrameworkSpec("net6.0", ".NET", "6.0"),
            new FrameworkSpec("net8.0", ".NET", "8.0"),
        ];

        /// <summary>
        /// Gets the frameworks to test, as dynamic data rows of TFM strings.
        /// </summary>
        /// <returns>One row per known framework, each holding its TFM.</returns>
        public static IEnumerable<object[]> GetFrameworkTestData() => s_all.Select(i => new object[] { i.Tfm });

        /// <summary>
        /// Gets the <see cref="FrameworkSpec"/> for the given TFM.
        /// </summary>
        /// <param name="tfm">The target framework moniker to look up.</param>
        /// <returns>The matching <see cref="FrameworkSpec"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="tfm"/> is not a known framework.</exception>
        public static FrameworkSpec Get(string tfm) => s_all.FirstOrDefault(i => i.Tfm == tfm) is { Tfm: not null } s ? s : throw new ArgumentException($"Unknown framework '{tfm}'.", nameof(tfm));

    }

}
