using System.Collections.Generic;

namespace IKVM.Tools.Runner.Compiler
{

    /// <summary>
    /// Options available to the IKVM compiler tool.
    /// </summary>
    public class IkvmCompilerOptions
    {

        /// <summary>
        /// Number of milliseconds to wait for the command to execute.
        /// </summary>
        public int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Optional path to the response file to generate. If specified, the response file is not cleaned up.
        /// </summary>
        public string ResponseFile { get; set; }

        /// <summary>
        /// Input files to be compiled.
        /// </summary>
        public IList<string> Input { get; } = new List<string>();

        /// <summary>
        /// Path of the output assembly.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Name of the output assembly.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Version of the output assembly.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Target to build.
        /// </summary>
        public IkvmCompilerTarget? Target { get; set; }

        /// <summary>
        /// Platform to build.
        /// </summary>
        public IkvmCompilerPlatform? Platform { get; set; }

        /// <summary>
        /// Path to the key to use for strong name signing.
        /// </summary>
        public string KeyFile { get; set; }

        /// <summary>
        /// Identity of the key to use for signing.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Whether the assembly should be delay signed.
        /// </summary>
        public bool DelaySign { get; set; }

        /// <summary>
        /// Set of paths to assemblies to add as references.
        /// </summary>
        public IList<string> References { get; } = new List<string>();

        public IList<string> Recurse { get;  } = new List<string>();

        public string Exclude { get; set; }

        /// <summary>
        /// File version to configure on the assembly.
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// Win32 icon to configure on the assembly.
        /// </summary>
        public string Win32Icon { get; set; }

        /// <summary>
        /// Win32 manifest to embed within the assembly.
        /// </summary>
        public string Win32Manifest { get; set; }

        /// <summary>
        /// Set of resources to embed within the assembly.
        /// </summary>
        public List<IkvmCompilerResourceItem> Resources { get; } = new List<IkvmCompilerResourceItem>();

        public List<IkvmCompilerExternalResourceItem> ExternalResources { get; } = new List<IkvmCompilerExternalResourceItem>();

        /// <summary>
        /// Whether resources should be compressed.
        /// </summary>
        public bool CompressResources { get; set; }

        /// <summary>
        /// Whether debug symbols should be produced along with the assembly.
        /// </summary>
        public bool Debug { get; set; }

        public bool NoAutoSerialization { get; set; }

        public bool NoGlobbing { get; set; }

        public bool NoJNI { get; set; }

        public bool OptFields { get; set; }

        public bool RemoveAssertions { get; set; }

        public bool StrictFinalFieldSemantics { get; set; }

        public IList<string> NoWarn { get; } = new List<string>();

        public bool WarnAsError { get; set; }

        public IList<string> WarnAsErrorWarnings { get; } = new List<string>();

        public string WriteSuppressWarningsFile { get; set; }

        public string Main { get; set; }

        public string SrcPath { get; set; }

        public string Apartment { get; set; }

        public IDictionary<string, string> SetProperties { get; } = new Dictionary<string, string>();

        public bool NoStackTraceInfo { get; set; }

        public IList<string> XTrace { get; } = new List<string>();

        public IList<string> XMethodTrace { get; } = new List<string>();

        public IList<string> PrivatePackages { get; } = new List<string>();

        public string ClassLoader { get; set; }

        public bool SharedClassLoader { get; set; }

        public string BaseAddress { get; set; }

        public string FileAlign { get; set; }

        public bool NoPeerCrossReference { get; set; }

        public bool NoStdLib { get; set; }

        public IList<string> Lib { get; set; } = new List<string>();

        public bool HighEntropyVA { get; set; }

        public bool Static { get; set; }

        public IList<string> AssemblyAttributes { get; set; } = new List<string>();

        public string Runtime { get; set; }

        public int? WarningLevel { get; set; }

        public bool NoParameterReflection { get; set; }

        public string Remap { get; set; }

        public bool NoLogo { get; set; }

    }

}
