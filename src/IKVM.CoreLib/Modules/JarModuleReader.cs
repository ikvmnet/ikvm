using System;
using System.Collections.Generic;
using System.IO;

using IKVM.CoreLib.Jar;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A <see cref="ModuleReader"/> for a modular JAR file.
    /// </summary>
    class JarModuleReader : ModuleReader
    {

        readonly string _file;
        readonly JarFile _jar;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="runtimeVersion"></param>
        public JarModuleReader(string file, RuntimeVersion runtimeVersion)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _file = Path.GetFullPath(_file);
            _jar = new JarFile(_file, runtimeVersion);
        }

        /// <inheritdoc />
        public override IEnumerable<string> List()
        {
            foreach (var i in _jar.GetEntries())
                yield return i.Name;
        }

        /// <inheritdoc />
        public override Stream? Open(string name)
        {
            var entry = _jar.GetEntry(name);
            if (entry is not null)
                return entry.Value.Open();

            return null;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _jar.Dispose();
        }

    }

}
