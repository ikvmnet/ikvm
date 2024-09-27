/*
 Copyright (C) 2010-2013 Jeroen Frijters

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
using System.Collections.Generic;
using System.IO;

using IKVM.CoreLib.Diagnostics;
using IKVM.Reflection;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Resolves assemblies from the universe of types.
    /// </summary>
    class ImportAssemblyResolver
    {

        readonly Universe universe;
        readonly ImportOptions options;
        readonly IDiagnosticHandler diagnostics;
        readonly List<string> libpaths = new List<string>();
        readonly Version coreLibVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="options"></param>
        public ImportAssemblyResolver(Universe universe, ImportOptions options, IDiagnosticHandler diagnostics)
        {
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));

            // like the C# compiler, the references are loaded from:
            // current directory, CLR directory, -lib: option, %LIB% environment
            // (note that, unlike the C# compiler, we don't add the CLR directory if -nostdlib has been specified)
            libpaths.Add(Environment.CurrentDirectory);

            // add the runtime directory
            if (options.NoStdLib == false)
                libpaths.Add(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());

            // add user libraries
            foreach (var directory in options.Libraries)
                AddLibraryPaths(directory.FullName, true);

            // add LIB environmental variable
            AddLibraryPaths(Environment.GetEnvironmentVariable("LIB") ?? "", false);

            // either load the corelib from the references, or from what already exists
            if (options.NoStdLib)
                coreLibVersion = LoadCoreLib(options.References).GetName().Version;
            else
                coreLibVersion = universe.Load(universe.CoreLibName).GetName().Version;
        }

        /// <summary>
        /// Gets the universe of types.
        /// </summary>
        public Universe Universe => universe;

        /// <summary>
        /// Loads the assembly at the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DiagnosticEventException"></exception>
        internal Assembly LoadFile(string path)
        {
            string ex = null;

            try
            {
                using (var module = universe.OpenRawModule(path))
                {
                    if (coreLibVersion != null)
                    {
                        // to avoid problems (i.e. weird exceptions), we don't allow assemblies to load that reference a newer version of the corelib
                        foreach (var asmref in module.GetReferencedAssemblies())
                            if (asmref.Name == universe.CoreLibName && asmref.Version > coreLibVersion)
                                throw new DiagnosticEventException(DiagnosticEvent.CoreAssemblyVersionMismatch(asmref.Name, universe.CoreLibName));
                    }

                    var asm = universe.LoadAssembly(module);
                    if (asm.Location != module.Location && CanonicalizePath(asm.Location) != CanonicalizePath(module.Location))
                        diagnostics.AssemblyLocationIgnored(path, asm.Location, asm.FullName);

                    return asm;
                }
            }
            catch (IOException x)
            {
                ex = x.Message;
            }
            catch (UnauthorizedAccessException x)
            {
                ex = x.Message;
            }
            catch (IKVM.Reflection.BadImageFormatException x)
            {
                ex = x.Message;
            }

            // TODO
            Console.Error.WriteLine("Error: unable to load assembly '{0}'" + Environment.NewLine + "    ({1})", path, ex);
            Environment.Exit(1);
            return null;
        }

        static string CanonicalizePath(string path)
        {
            try
            {
                var fi = new System.IO.FileInfo(path);
                if (fi.DirectoryName == null)
                    return path.Length > 1 && path[1] == ':' ? path.ToUpper() : path;

                var dir = CanonicalizePath(fi.DirectoryName);
                var name = fi.Name;
                try
                {
                    var arr = System.IO.Directory.GetFileSystemEntries(dir, name);
                    if (arr.Length == 1)
                        name = arr[0];
                }
                catch (System.UnauthorizedAccessException)
                {

                }
                catch (System.IO.IOException)
                {

                }

                return System.IO.Path.Combine(dir, name);
            }
            catch (System.UnauthorizedAccessException)
            {

            }
            catch (System.IO.IOException)
            {

            }
            catch (System.Security.SecurityException)
            {

            }
            catch (System.NotSupportedException)
            {

            }

            return path;
        }

        internal Assembly LoadWithPartialName(string name)
        {
            foreach (string path in FindAssemblyPath(name + ".dll"))
                return LoadFile(path);

            return null;
        }

        internal bool ResolveReference(Dictionary<string, Assembly> cache, List<Assembly> references, string reference)
        {
            var files = Array.Empty<string>();

            try
            {
                var path = Path.GetDirectoryName(reference);
                files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(reference));
            }
            catch (ArgumentException)
            {

            }
            catch (IOException)
            {

            }

            if (files.Length == 0)
            {
                cache.TryGetValue(reference, out var asm);

                if (asm == null)
                {
                    foreach (var found in FindAssemblyPath(reference))
                    {
                        asm = LoadFile(found);
                        cache.Add(reference, asm);
                        break;
                    }
                }

                if (asm == null)
                    return false;

                references.Add(asm);
            }
            else
            {
                foreach (string file in files)
                {
                    if (!cache.TryGetValue(file, out var asm))
                        asm = LoadFile(file);

                    references.Add(asm);
                }
            }

            return true;
        }

        /// <summary>
        /// Handles the <see cref="Universe.AssemblyResolve"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly AssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            foreach (var asm in universe.GetAssemblies())
                if (Match(asm.GetName(), name))
                    return asm;

            if (args.RequestingAssembly != null)
                return universe.CreateMissingAssembly(args.Name);

            return null;
        }

        /// <summary>
        /// Returs <c>true</c> if the two assembly names match.
        /// </summary>
        /// <param name="assemblyDef"></param>
        /// <param name="assemblyRef"></param>
        /// <returns></returns>
        bool Match(AssemblyName assemblyDef, AssemblyName assemblyRef)
        {
            return assemblyRef.Name == assemblyDef.Name;
        }

        /// <summary>
        /// Adds the library paths from the path separated string formatted string.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="option"></param>
        void AddLibraryPaths(string env, bool option)
        {
            foreach (var directory in env.Split(Path.PathSeparator))
            {
                if (Directory.Exists(directory))
                {
                    libpaths.Add(directory);
                }
                else if (directory != "")
                {
                    if (option)
                        diagnostics.InvalidDirectoryInLibOptionPath(directory);
                    else
                        diagnostics.InvalidDirectoryInLibEnvironmentPath(directory);
                }
            }
        }

        /// <summary>
        /// Search for the CoreLib from the reference assembly set.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        Assembly LoadCoreLib(IList<string> references)
        {
            // try to locate core lib from direct references
            if (references != null)
            {
                foreach (var reference in references)
                {
                    try
                    {
                        if (AssemblyName.GetAssemblyName(reference).Name == universe.CoreLibName)
                            return LoadFile(reference);
                    }
                    catch
                    {

                    }
                }
            }

            // try to locate corelib from library paths
            foreach (var coreLib in FindAssemblyPath(universe.CoreLibName + ".dll"))
                return LoadFile(coreLib);

            Console.Error.WriteLine($"Error: unable to find '{universe.CoreLibName}.dll'.");
            Environment.Exit(1);
            return null;
        }

        /// <summary>
        /// Returns the assembly at the path, or the assemblies within the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IEnumerable<string> FindAssemblyPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                if (File.Exists(path))
                    yield return path;
            }
            else
            {
                foreach (var libpath in libpaths)
                {
                    var p = Path.Combine(libpath, path);
                    if (File.Exists(p))
                        yield return p;

                    // for legacy compat, we try again after appending .dll
                    p = Path.Combine(libpath, p + ".dll");
                    if (File.Exists(p))
                    {
                        diagnostics.LegacySearchRule(p);
                        yield return p;
                    }
                }
            }
        }

    }

}
