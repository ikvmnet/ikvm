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
using System.IO;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Runtime;

namespace IKVM.Tools.Importer
{

    class StaticCompiler
    {

        readonly IDiagnosticHandler diagnostics;
        readonly IkvmReflectionSymbolContext symbols;
        internal Universe universe;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="universe"></param>
        /// <param name="symbols"></param>
        public StaticCompiler(IDiagnosticHandler diagnostics, Universe universe, IkvmReflectionSymbolContext symbols)
        {
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
        }

        /// <summary>
        /// Gets the symbol context.
        /// </summary>
        internal IkvmReflectionSymbolContext Symbols => symbols;

        /// <summary>
        /// Gets the universe of types.
        /// </summary>
        internal Universe Universe => universe;

        internal Assembly Load(string assemblyString)
        {
            var asm = universe.Load(assemblyString);
            if (asm.__IsMissing)
                throw new FileNotFoundException(assemblyString);

            return asm;
        }

        internal IAssemblySymbol LoadFile(string path)
        {
            return symbols.GetOrCreateAssemblySymbol( universe.LoadFile(path));
        }

        internal ITypeSymbol GetTypeForMapXml(RuntimeClassLoader loader, string name)
        {
            return GetType(loader, name) ?? throw new DiagnosticEventException(DiagnosticEvent.MapFileTypeNotFound(name));
        }

        internal RuntimeJavaType GetClassForMapXml(RuntimeClassLoader loader, string name)
        {
            return loader.TryLoadClassByName(name) ?? throw new DiagnosticEventException(DiagnosticEvent.MapFileClassNotFound(name));
        }

        internal RuntimeJavaField GetFieldForMapXml(RuntimeClassLoader loader, string clazz, string name, string sig)
        {
            var fw = GetClassForMapXml(loader, clazz).GetFieldWrapper(name, sig);
            if (fw == null)
                throw new DiagnosticEventException(DiagnosticEvent.MapFileFieldNotFound(name, clazz));

            fw.Link();
            return fw;
        }

        internal ITypeSymbol GetType(RuntimeClassLoader loader, string name)
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

            throw new DiagnosticEventException(DiagnosticEvent.LinkageError(str));
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

        internal void IssueMissingTypeMessage(ITypeSymbol type)
        {
            type = ReflectUtil.GetMissingType(type);
            if (type.Assembly.IsMissing)
                diagnostics.MissingReference(type.FullName, type.Assembly.FullName);
            else
                diagnostics.MissingType(type.FullName, type.Assembly.FullName);
        }

    }

}