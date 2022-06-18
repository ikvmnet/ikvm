using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;

using IKVM.Tool.Compiler;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Augments the set of given reference items by replacing assemblies with their reference assembly version from
    /// the tool.
    /// </summary>
    public class IkvmCompilerMergeReferenceAssemblies : Task
    {

        /// <summary>
        /// Root of the tools directory.
        /// </summary>
        [Required]
        public string ToolsPath { get; set; }

        /// <summary>
        /// Whether we are generating a NetFramework or NetCore assembly.
        /// </summary>
        [Required]
        public string TargetFramework { get; set; } = "NetCore";

        /// <summary>
        /// Set of input references.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            // extract the original items and reference assembly names
            var list = Items.Select(i => new { Item = i, Name = TryGetAssemblyName(i.ItemSpec) }).Where(i => i.Name != null).ToList();
            var refs = GetReferenceAssemblies().Select(i => new { Path = i, Name = TryGetAssemblyName(i) }).Where(i => i.Name != null).ToList();
            var sort = new List<ITaskItem>();

            // fill sort with new reference items, and remove old matching item
            foreach (var r in refs)
            {
                var item = list.FirstOrDefault(i => i.Name == r.Name);
                if (item != null)
                    list.Remove(item);

                sort.Add(new TaskItem(r.Path));
            }

            // remaining items remain
            foreach (var i in list)
                sort.Add(i.Item);

            // replace output
            Items = sort.ToArray();
            return true;
        }

        /// <summary>
        /// Gets the assemblies that should be referenced by the configured tool.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetReferenceAssemblies()
        {
            var l = new IkvmCompilerLauncher(ToolsPath);
            var f = ParseTargetFramework(TargetFramework);

            // gets the reference assemblies
            foreach (var path in Directory.GetFiles(l.GetReferenceAssemblyDirectory(f), "*.dll"))
                yield return path;

            // adds the IKVM.Runtime assembly to the end
            yield return l.GetRuntimeAssemblyFile(f);
            yield return l.GetJavaBaseAssemblyFile(f);
        }

        /// <summary>
        /// Converts a target framework value into an enum.
        /// </summary>
        /// <param name="targetFramework"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IkvmCompilerTargetFramework ParseTargetFramework(string targetFramework) => targetFramework switch
        {
            "NetCore" => IkvmCompilerTargetFramework.NetCore,
            "NetFramework" => IkvmCompilerTargetFramework.NetFramework,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Gets the assembly name from the file pointed to by <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        AssemblyName TryGetAssemblyName(string path)
        {
            if (File.Exists(path) == false)
                return null;

            try
            {
                using var file = File.OpenRead(path);
                var pr = new PEReader(file);
                var mr = pr.GetMetadataReader();
                var an = mr.GetAssemblyDefinition().GetAssemblyName();
                return an;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

}
