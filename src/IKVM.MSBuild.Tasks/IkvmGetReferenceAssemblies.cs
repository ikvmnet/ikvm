using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using IKVM.Tool;
using IKVM.Tool.Compiler;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Gets the set of reference assemblies for the IKVM tasks, given the specified target framework.
    /// </summary>
    public class IkvmGetReferenceAssemblies : Task
    {

        /// <summary>
        /// Root of the tools directory.
        /// </summary>
        [Required]
        public string ToolPath { get; set; }

        /// <summary>
        /// Whether we are generating a NetFramework or NetCore assembly.
        /// </summary>
        [Required]
        public string ToolFramework { get; set; } = "NetCore";

        /// <summary>
        /// Set of input references.
        /// </summary>
        [Output]
        public ITaskItem[] ResolvedFrameworkReferences { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            ResolvedFrameworkReferences = GetReferenceAssemblies().Select(i => new TaskItem(i)).ToArray();
            return true;
        }

        /// <summary>
        /// Gets the assemblies that should be referenced by the configured tool.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetReferenceAssemblies()
        {
            var l = new IkvmCompilerLauncher(ToolPath);
            var f = ParseToolFramework(ToolFramework);

            // gets the reference assemblies
            foreach (var path in Directory.GetFiles(l.GetReferenceAssemblyDirectory(f), "*.dll"))
                yield return path;
        }

        /// <summary>
        /// Converts a target framework value into an enum.
        /// </summary>
        /// <param name="targetFramework"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IkvmToolFramework ParseToolFramework(string targetFramework) => targetFramework switch
        {
            "NetCore" => IkvmToolFramework.NetCore,
            "NetFramework" => IkvmToolFramework.NetFramework,
            _ => throw new NotImplementedException(),
        };

    }

}
