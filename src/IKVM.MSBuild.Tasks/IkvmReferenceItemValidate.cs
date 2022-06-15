using System;
using System.IO;
using System.Reflection;

using IKVM.MSBuild.Tasks.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// For each <see cref="IkvmReferenceItem"/> passed in, validates the metdata.
    /// </summary>
    public class IkvmReferenceItemValidate : Task
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceItemValidate() :
            base(SR.ResourceManager, "IKVM")
        {

        }

        /// <summary>
        /// <see cref="IkvmReferenceItem"/> items to validate.
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
            var items = IkvmReferenceItemUtil.Import(Items);

            // assign other metadata
            foreach (var item in items)
                if (Validate(item) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Validates the item.
        /// </summary>
        /// <param name="item"></param>
        bool Validate(IkvmReferenceItem item)
        {
            var valid = true;

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyName", item.ItemSpec, item.AssemblyName);
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                valid = false;
            }
            else
            {
                if (Version.TryParse(item.AssemblyVersion, out _) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                    valid = false;
                }
            }

            if (item.Compile.Count == 0)
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmRequiresCompile", item.ItemSpec);
                valid = false;
            }
            else
            {
                foreach (var compile in item.Compile)
                {
                    if (Path.GetExtension(compile) is not ".jar" and not ".class")
                    {
                        Log.LogErrorWithCodeFromResources("Error.IkvmInvalidCompile", item.ItemSpec, compile);
                        valid = false;
                    }

                    if (File.Exists(compile) == false)
                    {
                        Log.LogErrorWithCodeFromResources("Error.IkvmMissingCompile", item.ItemSpec, compile);
                        valid = false;
                    }
                }
            }

            foreach (var source in item.Sources)
            {
                if (Path.GetExtension(source) is not ".java")
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmInvalidSources", item.ItemSpec, source);
                    valid = false;
                }
                else if (File.Exists(source) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmMissingSources", item.ItemSpec, source);
                    valid = false;
                }
            }

            // bail out early, no need to add more if these are already broken
            if (valid == false)
                return false;

            try
            {
                new AssemblyName($"{item.AssemblyName}, Version={item.AssemblyVersion}");
            }
            catch (Exception)
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyInfo", item.ItemSpec, item.AssemblyName, item.AssemblyVersion);
                valid = false;
            }

            return valid;
        }

    }

}
