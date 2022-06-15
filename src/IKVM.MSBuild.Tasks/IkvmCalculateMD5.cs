using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Calculates a MD5 sum for each of the files refered to by the source collection.
    /// </summary>
    public class IkvmCalculateMD5 : Task
    {

        /// <summary>
        /// Items for which a hash is calculated.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Name of the metadata to record the hash.
        /// </summary>
        public string MetadataName { get; set; } = "FileHashMD5";

        public override bool Execute()
        {
            using var md5 = MD5.Create();
            for (int i = 0; i < Items.Length; i++)
            {
                var jarFile = Items[i].ItemSpec;
                if (File.Exists(jarFile))
                {
                    using var stm = File.OpenRead(jarFile);
                    var hsh = md5.ComputeHash(stm);
                    var bld = new StringBuilder(hsh.Length * 2);
                    foreach (var b in hsh)
                        bld.Append(b.ToString("x2"));

                    Items[i].SetMetadata(MetadataName, bld.ToString());
                }
            }

            return true;
        }

    }

}
