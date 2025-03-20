using System;
using System.Collections.Generic;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A <see cref="ModuleReader "/> for an exploded module.
    /// </summary>
    class ExplodedModuleReader : ModuleReader
    {

        readonly string _dir;
        readonly RuntimeVersion _runtimeVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="runtimeVersion"></param>
        public ExplodedModuleReader(string dir, RuntimeVersion runtimeVersion)
        {
            _dir = dir ?? throw new ArgumentNullException(nameof(dir));
            _dir = Path.GetFullPath(_dir);
            _runtimeVersion = runtimeVersion;
        }

        /// <summary>
        /// Translates a file within 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetResourceName(string path)
        {
            if (Path.DirectorySeparatorChar != '/')
                return path[_dir.Length..].Replace(Path.DirectorySeparatorChar, '/');
            else
                return path[_dir.Length..];
        }

        /// <inheritdoc />
        public override IEnumerable<string> List()
        {
            foreach (var i in Directory.EnumerateFiles(_dir, "*", SearchOption.AllDirectories))
                yield return GetResourceName(i);
        }

        /// <inheritdoc />
        public override Stream? Open(string name)
        {
            var path = name;
            if (Path.DirectorySeparatorChar != '/')
                path = name.Replace('/', Path.DirectorySeparatorChar);

            path = Path.GetFullPath(Path.Combine(_dir, path));
            if (File.Exists(path))
                return File.OpenRead(path);

            return null;
        }

        /// <inheritdoc />
        public override void Dispose()
        {

        }

    }

}
