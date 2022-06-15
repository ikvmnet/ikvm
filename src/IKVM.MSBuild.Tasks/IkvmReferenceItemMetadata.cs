namespace IKVM.MSBuild.Tasks
{

    public static class IkvmReferenceItemMetadata
    {

        public const char PropertySeperatorChar = ';';
        public static readonly string PropertySeperatorString = PropertySeperatorChar.ToString();
        public static readonly char[] PropertySeperatorCharArray = new[] { PropertySeperatorChar };
        public static readonly string AssemblyName = "AssemblyName";
        public static readonly string AssemblyVersion = "AssemblyVersion";
        public static readonly string Debug = "Debug";
        public static readonly string Compile = "Compile";
        public static readonly string Sources = "Sources";
        public static readonly string References = "References";
        public static readonly string IkvmIdentity = "IkvmIdentity";

    }

}
