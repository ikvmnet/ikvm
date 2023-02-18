/*
  Copyright (C) 2007-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a directory containing all of the known assembies.
    /// </summary>
    sealed class VfsAssemblyDirectory : VfsDirectory
    {

        readonly ConcurrentDictionary<Assembly, VfsDirectory> directories = new ConcurrentDictionary<Assembly, VfsDirectory>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public VfsAssemblyDirectory(VfsContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Attempt to parse a directory name into the associated assembly name.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        AssemblyName ParseDirectoryName(string directoryName)
        {
            try
            {
                var assemblyName = new AssemblyName();
                var sb = new StringBuilder();
                var part = 0;
                for (var i = 0; i <= directoryName.Length; i++)
                {
                    if (i == directoryName.Length || directoryName[i] == '_')
                    {
                        if (i < directoryName.Length - 1 && directoryName[i + 1] == '!')
                        {
                            sb.Append('_');
                            i++;
                        }
                        else if (i == directoryName.Length || directoryName[i + 1] == '_')
                        {
                            switch (part++)
                            {
                                case 0:
                                    assemblyName.Name = sb.ToString();
                                    break;
                                case 1:
                                    assemblyName.Version = Version.Parse(sb.ToString());
                                    break;
                                case 2:
                                    assemblyName.SetPublicKeyToken(StringToByteArray(sb.ToString()));
                                    break;
                                case 3:
                                    assemblyName.CultureName = sb.ToString();
                                    break;
                                case 4:
                                    return null;
                            }

                            sb.Length = 0;
                            i++;
                        }
                        else
                        {
                            int start = i + 1;
                            int end = start;
                            while ('0' <= directoryName[end] && directoryName[end] <= '9')
                                end++;

                            if (directoryName[end] != '_' || !int.TryParse(directoryName.Substring(start, end - start), out int repeatCount))
                                return null;

                            sb.Append('_', repeatCount);
                            i = end;
                        }
                    }
                    else
                    {
                        sb.Append(directoryName[i]);
                    }
                }

                return assemblyName;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the directory for the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            // search for a directory by MVID
            if (Guid.TryParse(name, out var guid))
            {
                foreach (var assemblyName in Context.GetAssemblyNames())
                    if (Context.GetAssembly(assemblyName) is Assembly assembly)
                        if (assembly.ManifestModule.ModuleVersionId == guid && assembly.IsDynamic == false)
                            return directories.GetOrAdd(assembly, CreateAssemblyDirectory);

                return null;
            }
            else
            {
                // search for a directory for the given full name
                var assemblyName = ParseDirectoryName(name);
                if (assemblyName != null)
                {
                    var assembly = Context.GetAssembly(assemblyName);
                    if (assembly != null && assembly.IsDynamic == false && name == VfsTable.Default.GetAssemblyDirectoryName(assembly))
                        return directories.GetOrAdd(assembly, CreateAssemblyDirectory);
                }

                return null;
            }
        }

        /// <summary>
        /// Creates a directory entry for the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        VfsDirectory CreateAssemblyDirectory(Assembly assembly)
        {
            var d = new VfsEntryDirectory(Context);
            d.AddEntry("resource", new VfsAssemblyResourceDirectory(Context, assembly));
            d.AddEntry("classes", new VfsAssemblyClassDirectory(Context, assembly, ""));
            return d;
        }

        /// <summary>
        /// Returns an empty list. Assemblies are not discoverable by name.
        /// </summary>
        /// <returns></returns>
        public override string[] List()
        {
            return Array.Empty<string>();
        }

        /// <summary>
        /// Parses a hex string into a byte array.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var arr = new byte[hex.Length >> 1];
            for (int i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

            return arr;

            static int GetHexVal(char hex) => (int)hex - ((int)hex < 58 ? 48 : ((int)hex < 97 ? 55 : 87));
        }

    }

}
