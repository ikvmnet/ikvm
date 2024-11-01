/*
 Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Text.RegularExpressions;
using System.Threading;

using IKVM.CoreLib.Symbols;
using IKVM.Reflection;
using IKVM.Runtime;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Holds some state for an instance of <see cref="ImportContextFactory"/>.
    /// </summary>
    sealed class ImportContext
    {

        internal string manifestMainClass;
        internal string defaultAssemblyName;
        internal List<Jar> jars = new List<Jar>();
        private Dictionary<string, int> jarMap = new Dictionary<string, int>();
        internal int classesJar = -1;
        internal int resourcesJar = -1;
        internal bool nojarstubs;
        internal FileInfo path;
        internal FileInfo keyfile;
        internal string keycontainer;
        internal bool delaysign;
        internal byte[] publicKey;
        internal StrongNameKeyPair keyPair;
        internal Version version;
        internal string fileversion;
        internal FileInfo iconfile;
        internal FileInfo manifestFile;
        internal bool targetIsModule;
        internal string assembly;
        internal string mainClass;
        internal ApartmentState apartment;
        internal IKVM.CoreLib.Symbols.Emit.PEFileKinds target;
        internal bool guessFileKind;
        internal List<string> unresolvedReferences = new();
        internal Dictionary<string, string> legacyStubReferences = new();    // only used during command line parsing
        internal List<IAssemblySymbol> references = new();
        internal string[] peerReferences;
        internal bool crossReferenceAllPeers = true;
        internal string[] classesToExclude;     // only used during command line parsing
        internal FileInfo remapfile;
        internal Dictionary<string, string> props;
        internal bool noglobbing;
        internal CodeGenOptions codegenoptions;
        internal DebugMode debugMode = DebugMode.Portable;
        internal string debugFileName;
        internal bool compressedResources;
        internal string[] privatePackages;
        internal string[] publicPackages;
        internal string sourcepath;
        internal Dictionary<string, string> externalResources;
        internal string classLoader;
        internal System.Reflection.PortableExecutableKinds pekind = System.Reflection.PortableExecutableKinds.ILOnly;
        internal IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine = IKVM.CoreLib.Symbols.ImageFileMachine.I386;
        internal ulong baseAddress;
        internal uint fileAlignment;
        internal bool highentropyva;
        internal List<ImportClassLoader> sharedclassloader; // should *not* be deep copied in Copy(), because we want the list of all compilers that share a class loader
        internal List<string> proxies = new();
        internal List<object> assemblyAttributeAnnotations = new();
        internal bool warningLevelHigh;
        internal bool noParameterReflection;
        internal bool bootstrap;

        /// <summary>
        /// Creates a copy of the current <see cref="ImportContext"/>.
        /// </summary>
        /// <returns></returns>
        internal ImportContext Copy()
        {
            var copy = (ImportContext)MemberwiseClone();
            copy.jars = Copy(jars);
            copy.jarMap = new Dictionary<string, int>(jarMap);

            if (props != null)
                copy.props = new Dictionary<string, string>(props);

            if (externalResources != null)
                copy.externalResources = new Dictionary<string, string>(externalResources);

            return copy;
        }

        private static List<Jar> Copy(List<Jar> jars)
        {
            List<Jar> newJars = new List<Jar>();
            foreach (Jar jar in jars)
            {
                newJars.Add(jar.Copy());
            }
            return newJars;
        }

        internal Jar GetJar(string file)
        {
            if (jarMap.TryGetValue(file, out var existingJar))
                return jars[existingJar];

            jarMap.Add(file, jars.Count);
            return CreateJar(Path.GetFileName(file));
        }

        Jar CreateJar(string jarName)
        {
            int count = 0;
            var name = jarName;
        retry:
            foreach (var jar in jars)
            {
                if (jar.Name == name)
                {
                    name = Path.GetFileNameWithoutExtension(jarName) + "-" + (++count) + Path.GetExtension(jarName);
                    goto retry;
                }
            }

            var newJar = new Jar(name);
            jars.Add(newJar);
            return newJar;
        }

        internal Jar GetClassesJar()
        {
            if (classesJar == -1)
            {
                classesJar = jars.Count;
                CreateJar("classes.jar");
            }

            return jars[classesJar];
        }

        internal bool IsClassesJar(Jar jar)
        {
            return classesJar != -1 && jars[classesJar] == jar;
        }

        internal Jar GetResourcesJar()
        {
            if (resourcesJar == -1)
            {
                resourcesJar = jars.Count;
                CreateJar("resources.jar");
            }

            return jars[resourcesJar];
        }

        internal bool IsResourcesJar(Jar jar)
        {
            return resourcesJar != -1 && jars[resourcesJar] == jar;
        }

        internal bool IsExcludedClass(string className)
        {
            if (classesToExclude != null)
                for (int i = 0; i < classesToExclude.Length; i++)
                    if (Regex.IsMatch(className, classesToExclude[i]))
                        return true;

            return false;
        }

    }

}