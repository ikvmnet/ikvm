namespace IKVM.MSBuild.Tasks
{

    static class IkvmReferenceExportItemMetadata
    {

        public const char PropertySeperatorChar = ';';
        public static readonly string PropertySeperatorString = PropertySeperatorChar.ToString();
        public static readonly char[] PropertySeperatorCharArray = new[] { PropertySeperatorChar };
        public static readonly string References = "References";
        public static readonly string Namespaces = "Namespaces";
        public static readonly string Shared = "Shared";
        public static readonly string NoStdLib = "NoStdLib";
        public static readonly string Libraries = "Libraries";
        public static readonly string Forwarders = "Forwarders";
        public static readonly string IncludeNonPublicTypes = "IncludeNonPublicTypes";
        public static readonly string IncludeNonPublicInterfaces = "IncludeNonPublicInterfaces";
        public static readonly string IncludeNonPublicMembers = "IncludeNonPublicMembers";
        public static readonly string IncludeParameterNames = "IncludeParameterNames";
        public static readonly string Bootstrap = "Bootstrap";
        public static readonly string IkvmIdentity = "IkvmIdentity";
        public static readonly string RandomIndex = "RandomIndex";

    }

}
