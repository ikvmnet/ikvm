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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using IKVM.CoreLib.Diagnostics;
using IKVM.Reflection;
using IKVM.Reflection.Diagnostics;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class StaticCompiler
    {

        readonly ConcurrentDictionary<string, Type> runtimeTypeCache = new();

        readonly IDiagnosticHandler diagnostics;
        internal Universe universe;
        internal Assembly runtimeAssembly;
        internal Assembly baseAssembly;
        internal ImportState rootTarget;
        internal int errorCount;

        internal Universe Universe => universe;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="diagnostics"></param>
        public StaticCompiler(IDiagnosticHandler diagnostics)
        {
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        /// <summary>
        /// Initializes the universe.
        /// </summary>
        /// <param name="nonDeterministicOutput"></param>
        /// <param name="debug"></param>
        /// <param name="libpaths"></param>
        /// <exception cref="Exception"></exception>
        internal void Init(bool nonDeterministicOutput, DebugMode debug, IList<string> libpaths)
        {
            var options = UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers;
            if (nonDeterministicOutput == false)
                options |= UniverseOptions.DeterministicOutput;

            // discover the core lib from the references
            var coreLibName = FindCoreLibName(rootTarget.unresolvedReferences, libpaths);
            if (coreLibName == null)
            {
                diagnostics.CoreClassesMissing();
                throw new Exception();
            }

            universe = new Universe(options, coreLibName);
            universe.ResolvedMissingMember += ResolvedMissingMember;

            // enable embedded symbol writer
            if (debug == DebugMode.Portable)
                universe.SetSymbolWriterFactory(module => new PortablePdbSymbolWriter(module));
        }

        /// <summary>
        /// Finds the first potential core library in the reference set.
        /// </summary>
        /// <param name="references"></param>
        /// <param name="libpaths"></param>
        /// <returns></returns>
        static string FindCoreLibName(IList<string> references, IList<string> libpaths)
        {
            if (references != null)
                foreach (var reference in references)
                    if (GetAssemblyNameIfCoreLib(reference) is string coreLibName)
                        return coreLibName;

            if (libpaths != null)
                foreach (var libpath in libpaths)
                    foreach (var dll in Directory.GetFiles(libpath, "*.dll"))
                        if (GetAssemblyNameIfCoreLib(dll) is string coreLibName)
                            return coreLibName;

            return null;
        }

        /// <summary>
        /// Returns the assembly name if the given assembly is a core library.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetAssemblyNameIfCoreLib(string path)
        {
            if (File.Exists(path) == false)
                return null;

            try
            {
                using var st = File.OpenRead(path);
                using var pe = new PEReader(st);
                var mr = pe.GetMetadataReader();

                foreach (var handle in mr.TypeDefinitions)
                    if (IsSystemObject(mr, handle))
                        return mr.GetString(mr.GetAssemblyDefinition().Name);

                return null;
            }
            catch (System.BadImageFormatException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the given type definition handle refers to "System.Object".
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="th"></param>
        /// <returns></returns>
        static bool IsSystemObject(MetadataReader reader, TypeDefinitionHandle th)
        {
            var td = reader.GetTypeDefinition(th);
            var ns = reader.GetString(td.Namespace);
            var nm = reader.GetString(td.Name);

            return ns == "System" && nm == "Object";
        }

        void ResolvedMissingMember(Module requestingModule, MemberInfo member)
        {
            if (requestingModule != null && member is Type)
            {
                diagnostics.UnableToResolveType(requestingModule.Name, ((Type)member).FullName, member.Module.FullyQualifiedName);
            }
        }

        internal Assembly Load(string assemblyString)
        {
            var asm = universe.Load(assemblyString);
            if (asm.__IsMissing)
                throw new FileNotFoundException(assemblyString);

            return asm;
        }

        internal Assembly LoadFile(string path)
        {
            return universe.LoadFile(path);
        }

        internal Type GetRuntimeType(string name)
        {
            return runtimeTypeCache.GetOrAdd(name, runtimeAssembly.GetType);
        }

        internal Type GetTypeForMapXml(RuntimeClassLoader loader, string name)
        {
            return GetType(loader, name) ?? throw new FatalCompilerErrorException(DiagnosticEvent.MapFileTypeNotFound(name));
        }

        internal RuntimeJavaType GetClassForMapXml(RuntimeClassLoader loader, string name)
        {
            return loader.TryLoadClassByName(name) ?? throw new FatalCompilerErrorException(DiagnosticEvent.MapFileClassNotFound(name));
        }

        internal RuntimeJavaField GetFieldForMapXml(RuntimeClassLoader loader, string clazz, string name, string sig)
        {
            var fw = GetClassForMapXml(loader, clazz).GetFieldWrapper(name, sig);
            if (fw == null)
                throw new FatalCompilerErrorException(DiagnosticEvent.MapFileFieldNotFound(name, clazz));

            fw.Link();
            return fw;
        }

        internal Type GetType(RuntimeClassLoader loader, string name)
        {
            var ccl = (ImportClassLoader)loader;
            return ccl.GetTypeFromReferencedAssembly(name);
        }

        internal static void LinkageError(string msg, RuntimeJavaType actualType, RuntimeJavaType expectedType, params object[] values)
        {
            object[] args = new object[values.Length + 2];
            values.CopyTo(args, 2);
            args[0] = AssemblyQualifiedName(actualType);
            args[1] = AssemblyQualifiedName(expectedType);
            string str = string.Format(msg, args);
            if (actualType is RuntimeUnloadableJavaType && (expectedType is RuntimeManagedByteCodeJavaType || expectedType is RuntimeManagedJavaType))
            {
                str += string.Format("\n\t(Please add a reference to {0})", expectedType.TypeAsBaseType.Assembly.Location);
            }

            throw new FatalCompilerErrorException(DiagnosticEvent.LinkageError(str));
        }

        static string AssemblyQualifiedName(RuntimeJavaType javaType)
        {
            var loader = javaType.ClassLoader;
            var acl = loader as RuntimeAssemblyClassLoader;
            if (acl != null)
                return javaType.Name + ", " + acl.GetAssembly(javaType).FullName;

            var ccl = loader as ImportClassLoader;
            if (ccl != null)
                return javaType.Name + ", " + ccl.GetTypeWrapperFactory().ModuleBuilder.Assembly.FullName;

            return javaType.Name + " (unknown assembly)";
        }

        internal void IssueMissingTypeMessage(Type type)
        {
            type = ReflectUtil.GetMissingType(type);
            if (type.Assembly.__IsMissing)
                diagnostics.MissingReference(type.FullName, type.Assembly.FullName);
            else
                diagnostics.MissingType(type.FullName, type.Assembly.FullName);
        }

        internal void SuppressWarning(ImportState options, Diagnostic diagnostic, string name)
        {
            options.suppressWarnings.Add($"{diagnostic.Id}:{name}");
        }

    }

}
