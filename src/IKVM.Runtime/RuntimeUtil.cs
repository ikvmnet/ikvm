using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains various information about the current runtime environment.
    /// </summary>
    public static class RuntimeUtil
    {

        static JsonElement? runtimeJsonElement;
        static string runtimeIdentifier;

        /// <summary>
        /// Returns <c>true</c> if the current runtime is Mono.
        /// </summary>
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Returns <c>true</c> if the current platform is Windows.
        /// </summary>
        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Linux.
        /// </summary>
        public static bool IsLinux { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Mac OS X.
        /// </summary>
        public static bool IsOSX { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Gets the current runtime identifier.
        /// </summary>
        public static string RuntimeIdentifier => runtimeIdentifier ??= GetRuntimeIdentifier();

        /// <summary>
        /// Discovers the current RID, and ensures it matches a known identifier, or generates a fallback.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifier()
        {
            // allow overrides with environment
            if (Environment.GetEnvironmentVariable("DOTNET_RUNTIME_ID") is string rid1 && string.IsNullOrWhiteSpace(rid1) == false)
                if (RuntimeJson.TryGetProperty(rid1, out _))
                    return rid1;

#if NETCOREAPP
            if (RuntimeInformation.RuntimeIdentifier is string rid2 && string.IsNullOrWhiteSpace(rid2) == false)
                if (RuntimeJson.TryGetProperty(rid2, out _))
                    return rid2;
#endif

            return GetFallbackRuntimeIdentifier();
        }

        /// <summary>
        /// Discovers the current RID.
        /// </summary>
        /// <returns></returns>
        static string GetFallbackRuntimeIdentifier()
        {
            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => null,
            };

            if (IsWindows)
                return $"win-{arch}";

            if (IsLinux)
                return $"linux-{arch}";

            if (IsOSX)
                return $"osx-{arch}";

            return null;
        }

        /// <summary>
        /// Gets the 'runtime.json' file.
        /// </summary>
        static JsonElement RuntimeJson => runtimeJsonElement ??= GetRuntimeJson();

        /// <summary>
        /// Loads the 'runtime.json' file.
        /// </summary>
        /// <returns></returns>
        static JsonElement GetRuntimeJson()
        {
            using var stream = typeof(RuntimeUtil).Assembly.GetManifestResourceStream("runtime.json");
            var g = JsonDocument.Parse(stream);
            return g.RootElement.GetProperty("runtimes");
        }

        /// <summary>
        /// Gets the supported RIDs, returned in most specific order.
        /// </summary>
        public static IEnumerable<string> SupportedRuntimeIdentifiers => GetSupportedRuntimeIdentifiers();

        /// <summary>
        /// Discovers the supported RIDs, returned in most specific order.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetSupportedRuntimeIdentifiers()
        {
            return GetRuntimeIdentifierIterator(RuntimeIdentifier).Distinct();
        }

        /// <summary>
        /// Recurses into the runtimes graph and attempts to resolve the RID and all RIDs it imports.
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        static IEnumerable<string> GetRuntimeIdentifierIterator(string rid)
        {
            if (RuntimeJson.TryGetProperty(rid, out var node) == false)
                yield break;

            // initiate a breadth first search across runtime.json from specified RID
            var v = new HashSet<string>();
            var q = new Queue<(string, JsonElement)>();
            v.Add(rid);
            q.Enqueue((rid, node));

            // continue until end is reached
            while (q.Count > 0)
            {
                // dequeue next item and yield
                var (thisRid, thisNode) = q.Dequeue();
                yield return thisRid;

                // enqueue referenced rids
                if (thisNode.TryGetProperty("#import", out var import))
                    foreach (var imports in import.EnumerateArray())
                        if (imports.GetString() is string nextRid && string.IsNullOrWhiteSpace(nextRid) == false)
                            if (v.Add(nextRid))
                                if (RuntimeJson.TryGetProperty(nextRid, out var nextNode))
                                    q.Enqueue((nextRid, nextNode));
            }
        }

    }

}
