﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

using IKVM.Tools.Runner.Compiler;

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

            // gets the reference assemblies
            foreach (var path in Directory.GetFiles(l.GetReferenceAssemblyDirectory(), "*.dll"))
                yield return path;
        }

    }

}
