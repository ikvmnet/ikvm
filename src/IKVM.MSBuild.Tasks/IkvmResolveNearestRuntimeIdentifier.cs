namespace IKVM.MSBuild.Tasks
{

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Resolves the nearest IKVM RID from the specified RID.
    /// </summary>
    public class IkvmResolveNearestRuntimeIdentifier : Task
    {

        class RuntimesElement
        {

            [DataMember(Name = "runtimes")]
            public Dictionary<string, RuntimeElement> Runtimes { get; set; }

        }

        class RuntimeElement
        {

            [DataMember(Name = "#import")]
            public string[] Imports { get; set; }

        }

        static Dictionary<string, RuntimeElement> runtimeJsonElement;

        /// <summary>
        /// Gets the 'runtime.json' file.
        /// </summary>
        static Dictionary<string, RuntimeElement> RuntimeJson => runtimeJsonElement ??= GetRuntimeJson();

        /// <summary>
        /// Loads the 'runtime.json' file.
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, RuntimeElement> GetRuntimeJson()
        {
            using var stream = typeof(IkvmResolveNearestRuntimeIdentifier).Assembly.GetManifestResourceStream("runtime.json");
            using var reader = new StreamReader(stream);
            var g = IkvmJsonParser.FromJson<RuntimesElement>(reader.ReadToEnd());
            return g.Runtimes;
        }

        /// <summary>
        /// Recurses into the runtimes graph and attempts to resolve the RID and all RIDs it imports.
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        static IEnumerable<string> GetRuntimeIdentifierIterator(string rid)
        {
            if (RuntimeJson.TryGetValue(rid, out var node) == false)
                yield break;

            // initiate a breadth first search across runtime.json from specified RID
            var v = new HashSet<string>();
            var q = new Queue<(string, RuntimeElement)>();
            v.Add(rid);
            q.Enqueue((rid, node));

            // continue until end is reached
            while (q.Count > 0)
            {
                // dequeue next item and yield
                var (thisRid, thisNode) = q.Dequeue();
                yield return thisRid;

                // enqueue referenced rids
                if (thisNode.Imports is string[] imports)
                    foreach (var import in imports)
                        if (import is string nextRid && string.IsNullOrWhiteSpace(nextRid) == false)
                            if (v.Add(nextRid))
                                if (RuntimeJson.TryGetValue(nextRid, out var nextNode))
                                    q.Enqueue((nextRid, nextNode));
            }
        }

        /// <summary>
        /// Gets the RID from the <paramref name="availableRuntimeIdentifiers"/> which is nearest to <paramref name="target"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="availableRuntimeIdentifiers"></param>
        /// <returns></returns>
        static string GetNearestRuntimeIdentififer(string target, string[] availableRuntimeIdentifiers)
        {
            // target is already available (notask)
            if (availableRuntimeIdentifiers.Contains(target))
                return target;

            // otherwise, loop through available
            foreach (var rid in GetRuntimeIdentifierIterator(target))
                if (availableRuntimeIdentifiers.Contains(rid))
                    return rid;

            return null;
        }

        /// <summary>
        /// Semicolon seperated list of available runtime identifiers.
        /// </summary>
        [Required]
        public string AvailableRuntimeIdentifiers { get; set; }

        /// <summary>
        /// Specified target runtime identifier.
        /// </summary>
        public string TargetRuntimeIdentifier { get; set; }

        /// <summary>
        /// Resulting runtime identifier.
        /// </summary>
        [Output]
        public string NearestRuntimeIdentifier { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(TargetRuntimeIdentifier) == false)
                NearestRuntimeIdentifier = GetNearestRuntimeIdentififer(TargetRuntimeIdentifier, AvailableRuntimeIdentifiers.Split(';'));

            return true;
        }

    }

}
