using System;
using System.IO;
using System.Reflection;

using IKVM.Sdk.Tasks.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// For each JavaReferenceItem passed in, validates the metdata.
    /// </summary>
    public class IkvmValidateJavaReferenceItem : Task
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmValidateJavaReferenceItem() :
            base(SR.ResourceManager, "MSBuild")
        {

        }

        /// <summary>
        /// JavaReferenceItem items to validate.
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
            var items = IkvmJavaReferenceItemUtil.Import(Items);

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
        bool Validate(JavaReferenceItem item)
        {
            var valid = true;

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
            {
                Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidAssemblyName", item.ItemSpec, item.AssemblyName);
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
            {
                Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                valid = false;
            }
            else
            {
                if (Version.TryParse(item.AssemblyVersion, out _) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                    valid = false;
                }
            }

            if (item.Compile.Count == 0)
            {
                Log.LogErrorWithCodeFromResources("Error.JavaReferenceRequiresCompile", item.ItemSpec);
                valid = false;
            }
            else
            {
                foreach (var compile in item.Compile)
                {
                    if (Path.GetExtension(compile) is not ".jar" and not ".class")
                    {
                        Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidCompile", item.ItemSpec, compile);
                        valid = false;
                    }

                    if (File.Exists(compile) == false)
                    {
                        Log.LogErrorWithCodeFromResources("Error.JavaReferenceMissingCompile", item.ItemSpec, compile);
                        valid = false;
                    }
                }
            }

            foreach (var source in item.Sources)
            {
                if (Path.GetExtension(source) is not ".java")
                {
                    Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidSources", item.ItemSpec, source);
                    valid = false;
                }
                else if (File.Exists(source) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.JavaReferenceMissingSources", item.ItemSpec, source);
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
                Log.LogErrorWithCodeFromResources("Error.JavaReferenceInvalidAssemblyInfo", item.ItemSpec, item.AssemblyName, item.AssemblyVersion);
                valid = false;
            }

            return valid;
        }

    }

}
