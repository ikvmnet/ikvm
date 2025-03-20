using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;

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

        /// <inheritdoc />
        public override IEnumerable<string> List()
        {
            foreach (var i in Directory.EnumerateFiles(_dir, "*", SearchOption.AllDirectories))
                if (Resources.ToResourceName(_dir, i) is { } path and { Length: > 0 })
                    yield return path;
        }

        /// <inheritdoc />
        public override Stream? Open(string name)
        {
            var path = Resources.ToFilePath(_dir, name);
            if (path is null)
                return null;

            return File.OpenRead(path);
        }

        /// <inheritdoc />
        public override void Dispose()
        {

        }

    }

}
