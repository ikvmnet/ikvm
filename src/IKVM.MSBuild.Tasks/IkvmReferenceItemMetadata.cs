namespace IKVM.MSBuild.Tasks
{

    static class IkvmReferenceItemMetadata
    {

        public const char PropertySeperatorChar = ';';
        public static readonly string PropertySeperatorString = PropertySeperatorChar.ToString();
        public static readonly char[] PropertySeperatorCharArray = new[] { PropertySeperatorChar };
        public static readonly string AssemblyName = "AssemblyName";
        public static readonly string AssemblyVersion = "AssemblyVersion";
        public static readonly string DisableAutoAssemblyName = "DisableAutoAssemblyName";
        public static readonly string DisableAutoAssemblyVersion = "DisableAutoAssemblyVersion";
        public static readonly string FallbackAssemblyName = "FallbackAssemblyName";
        public static readonly string FallbackAssemblyVersion = "FallbackAssemblyVersion";
        public static readonly string Compile = "Compile";
        public static readonly string Sources = "Sources";
        public static readonly string References = "References";
        public static readonly string IkvmIdentity = "IkvmIdentity";
        public static readonly string CachePath = "CachePath";
        public static readonly string StagePath = "StagePath";
        public static readonly string ResolvedReferences = "ResolvedReferences";
        public static readonly string Debug = "Debug";
        public static readonly string KeyFile = "KeyFile";
        public static readonly string DelaySign = "DelaySign";
        public static readonly string Aliases = "Aliases";
        public static readonly string Private = "Private";
        public static readonly string ReferenceOutputAssembly = "ReferenceOutputAssembly";

    }

}
