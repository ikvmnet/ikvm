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

using IKVM.Reflection;

namespace IKVM.Internal
{

    sealed class AssemblyResolver
    {

        readonly List<string> libpath = new List<string>();
        Universe universe;

        internal enum WarningId
        {
            HigherVersion = 1,
            LocationIgnored = 2,
            InvalidLibDirectoryOption = 3,
            InvalidLibDirectoryEnvironment = 4,
            LegacySearchRule = 5,
        }

        internal delegate void WarningEvent(WarningId warning, string message, string[] parameters);
        internal event WarningEvent Warning;

        /// <summary>
        /// Emits a warning event.
        /// </summary>
        /// <param name="warning"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        void EmitWarning(WarningId warning, string message, params string[] parameters)
        {
            if (Warning != null)
                Warning(warning, message, parameters);
            else
                Console.Error.WriteLine("Warning: " + message, parameters);
        }

        internal void Init(Universe universe, IList<string> references, IList<string> userLibPaths)
        {
            this.universe = universe;

            // search in the local directory first
            libpath.Add(Environment.CurrentDirectory);

            // then search in directories specified by the user
            foreach (string str in userLibPaths)
                AddLibraryPaths(str, true);

            // then search in the LIB environment variable
            AddLibraryPaths(Environment.GetEnvironmentVariable("LIB") ?? "", false);

            // attach to the universe to handle resolution
            universe.AssemblyResolve += AssemblyResolve;
        }

        /// <summary>
        /// Attempts to load the assembly from the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal Assembly LoadFile(string path)
        {
            string msg = null;
            try
            {
                using var module = universe.OpenRawModule(path);
                var assembly = universe.LoadAssembly(module);
                if (assembly.Location != module.Location && CanonicalizePath(assembly.Location) != CanonicalizePath(module.Location))
                    EmitWarning(WarningId.LocationIgnored, "assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identity \"{2}\"", path, assembly.Location, assembly.FullName);

                return assembly;
            }
            catch (IOException x)
            {
                msg = x.Message;
            }
            catch (UnauthorizedAccessException x)
            {
                msg = x.Message;
            }
            catch (IKVM.Reflection.BadImageFormatException x)
            {
                msg = x.Message;
            }

            Console.Error.WriteLine("Error: unable to load assembly '{0}'" + Environment.NewLine + "    ({1})", path, msg);
            Environment.Exit(1);
            return null;
        }

        /// <summary>
        /// Returns the canonical path for the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string CanonicalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));

            try
            {
                var fi = new FileInfo(Path.GetFullPath(path));
                if (fi.DirectoryName == null)
                    return path.Length > 1 && path[1] == ':' ? path.ToUpper() : path;

                var dir = CanonicalizePath(fi.DirectoryName);
                var name = fi.Name;
                try
                {
                    var arr = Directory.GetFileSystemEntries(dir, name);
                    if (arr.Length == 1)
                        name = arr[0];
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (IOException)
                {

                }

                return Path.Combine(dir, name);
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (System.Security.SecurityException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return path;
        }

        /// <summary>
        /// Attempts to load an assembly with a partial name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Assembly LoadWithPartialName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            foreach (var path in FindAssemblyPath(name + ".dll"))
                return LoadFile(path);

            return null;
        }

        /// <summary>
        /// Attempts to resolve the given reference, appending the discovered assembly to the references list.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="references"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        internal bool ResolveReference(Dictionary<string, Assembly> cache, List<Assembly> references, string reference)
        {
            if (cache is null)
                throw new ArgumentNullException(nameof(cache));
            if (references is null)
                throw new ArgumentNullException(nameof(references));
            if (string.IsNullOrEmpty(reference))
                throw new ArgumentException($"'{nameof(reference)}' cannot be null or empty.", nameof(reference));

            var files = new List<string>(0);

            // attempt to find the reference in the current directory if it exists
            var fullPath = Path.GetFullPath(reference);
            if (File.Exists(fullPath))
                files.Add(fullPath);

            // we haven't yet found the reference
            if (files.Count == 0)
            {
                if (cache.TryGetValue(reference, out Assembly assembly) == false)
                {
                    foreach (string found in FindAssemblyPath(reference))
                    {
                        assembly = LoadFile(found);
                        cache.Add(reference, assembly);
                        break;
                    }
                }

                if (assembly == null)
                    return false;

                references.Add(assembly);
            }
            else
            {
                foreach (var file in files)
                {
                    if (cache.TryGetValue(file, out Assembly assembly) == false)
                        assembly = LoadFile(file);

                    references.Add(assembly);
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to resolve a required assembly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly AssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            AssemblyName previousMatch = null;
            int previousMatchLevel = 0;

            // first attempt to match the requested assembly against one that is already laoe
            foreach (var asm in universe.GetAssemblies())
                if (Match(asm.GetName(), name, ref previousMatch, ref previousMatchLevel))
                    return asm;

            // attempt to locate teh assembly in the search path
            foreach (var file in FindAssemblyPath(name.Name + ".dll"))
                if (Match(AssemblyName.GetAssemblyName(file), name, ref previousMatch, ref previousMatchLevel))
                    return LoadFile(file);

            // we did not find an exact match, but we did find one that was close
            if (previousMatch != null)
            {
                // match level was okay to proceed
                if (previousMatchLevel == 2)
                {
#if NETFRAMEWORK
                    EmitWarning(WarningId.HigherVersion, "assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy", previousMatch.FullName, name.FullName);
#endif
                    return universe.Load(previousMatch.FullName);
                }

                if (args.RequestingAssembly != null)
                {
                    // we found an assembly match, but it was of a lower version than that which was requested
                    Console.Error.WriteLine("Error: Assembly '{0}' uses '{1}' which has a higher version than referenced assembly '{2}'", args.RequestingAssembly.FullName, name.FullName, previousMatch.FullName);
                    Environment.Exit(1);
                    return null;
                }
                else
                {
                    // an assembly was found at a lower version, but wasn't specifically requested by a dependent
                    Console.Error.WriteLine("Error: Assembly '{0}' was requested which is a higher version than referenced assembly '{1}'", name.FullName, previousMatch.FullName);
                    Environment.Exit(1);
                    return null;
                }
            }

            // generate a missing assembly reference, only if we had a requester
            if (args.RequestingAssembly != null)
                return universe.CreateMissingAssembly(args.Name);

            return null;
        }

        bool Match(AssemblyName assemblyDef, AssemblyName assemblyRef, ref AssemblyName bestMatch, ref int bestMatchLevel)
        {
            // Match levels:
            //   0 = no match
            //   1 = lower version match (i.e. not a suitable match, but used in error reporting: something was found but the version was too low)
            //   2 = higher version potential match (i.e. we can use this version, but if it is available the exact match will be preferred)
            //
            // If we find a perfect match, bestMatch is not changed but we return true to signal that the search can end right now.
            AssemblyComparisonResult result;
            universe.CompareAssemblyIdentity(assemblyRef.FullName, false, assemblyDef.FullName, true, out result);
            switch (result)
            {
                case AssemblyComparisonResult.EquivalentFullMatch:
                case AssemblyComparisonResult.EquivalentPartialMatch:
                case AssemblyComparisonResult.EquivalentFXUnified:
                case AssemblyComparisonResult.EquivalentPartialFXUnified:
                case AssemblyComparisonResult.EquivalentPartialWeakNamed:
                case AssemblyComparisonResult.EquivalentWeakNamed:
                    return true;
                case AssemblyComparisonResult.NonEquivalentPartialVersion:
                case AssemblyComparisonResult.NonEquivalentVersion:
                    if (bestMatchLevel < 1)
                    {
                        bestMatchLevel = 1;
                        bestMatch = assemblyDef;
                    }
                    return false;
                case AssemblyComparisonResult.EquivalentUnified:
                case AssemblyComparisonResult.EquivalentPartialUnified:
                    if (bestMatchLevel < 2)
                    {
                        bestMatchLevel = 2;
                        bestMatch = assemblyDef;
                    }
                    return false;
                case AssemblyComparisonResult.NonEquivalent:
                case AssemblyComparisonResult.Unknown:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }

        void AddLibraryPaths(string str, bool option)
        {
            foreach (string dir in str.Split(Path.PathSeparator))
            {
                if (Directory.Exists(dir))
                {
                    libpath.Add(dir);
                }
                else if (dir != "")
                {
                    if (option)
                    {
                        EmitWarning(WarningId.InvalidLibDirectoryOption, "directory \"{0}\" specified in -lib option is not valid", dir);
                    }
                    else
                    {
                        EmitWarning(WarningId.InvalidLibDirectoryEnvironment, "directory \"{0}\" specified in LIB environment is not valid", dir);
                    }
                }
            }
        }

        IEnumerable<string> FindAssemblyPath(string file)
        {
            if (Path.IsPathRooted(file))
            {
                if (File.Exists(file))
                    yield return file;
            }
            else
            {
                foreach (var dir in libpath)
                {
                    // if the assembly file is found in the lib path, yield it up
                    var path = Path.Combine(dir, file);
                    if (File.Exists(path))
                        yield return path;

                    // it's possible the reference did not specify a dll name
                    path = Path.Combine(dir, file + ".dll");
                    if (File.Exists(path))
                    {
                        EmitWarning(WarningId.LegacySearchRule, "found assembly \"{0}\" using legacy search rule, please append '.dll' to the reference", file);
                        yield return path;
                    }
                }
            }
        }

    }

}
