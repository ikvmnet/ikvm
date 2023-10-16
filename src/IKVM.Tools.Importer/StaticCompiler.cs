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
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;

using IKVM.Reflection;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class StaticCompiler
    {

        readonly ConcurrentDictionary<string, Type> runtimeTypeCache = new();

        internal Universe universe;
        internal Assembly runtimeAssembly;
        internal Assembly baseAssembly;
        internal CompilerOptions rootTarget;
        internal int errorCount;

        internal Universe Universe => universe;

        internal void Init(bool nonDeterministicOutput, IList<string> libpaths)
        {
            var options = UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers;
            if (!nonDeterministicOutput)
                options |= UniverseOptions.DeterministicOutput;

            // discover the core lib from the references
            var coreLibName = FindCoreLibName(rootTarget.unresolvedReferences, libpaths);
            if (coreLibName == null)
                Console.Error.WriteLine("Error: core library not found");

            universe = new Universe(options, coreLibName);
            universe.ResolvedMissingMember += ResolvedMissingMember;
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
        /// Returns <c>true</c> if the given assembly is a core library.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetAssemblyNameIfCoreLib(string path)
        {
            if (File.Exists(path) == false)
                return null;

            using var st = File.OpenRead(path);
            using var pe = new PEReader(st);
            var mr = pe.GetMetadataReader();

            foreach (var handle in mr.TypeDefinitions)
                if (IsSystemObject(mr, handle))
                    return mr.GetString(mr.GetAssemblyDefinition().Name);

            return null;
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
                IssueMessage(Message.UnableToResolveType, requestingModule.Name, ((Type)member).FullName, member.Module.FullyQualifiedName);
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
            return GetType(loader, name) ?? throw new FatalCompilerErrorException(Message.MapFileTypeNotFound, name);
        }

        internal RuntimeJavaType GetClassForMapXml(RuntimeClassLoader loader, string name)
        {
            return loader.TryLoadClassByName(name) ?? throw new FatalCompilerErrorException(Message.MapFileClassNotFound, name);
        }

        internal RuntimeJavaField GetFieldForMapXml(RuntimeClassLoader loader, string clazz, string name, string sig)
        {
            var fw = GetClassForMapXml(loader, clazz).GetFieldWrapper(name, sig);
            if (fw == null)
                throw new FatalCompilerErrorException(Message.MapFileFieldNotFound, name, clazz);

            fw.Link();
            return fw;
        }

        internal Type GetType(RuntimeClassLoader loader, string name)
        {
            var ccl = (CompilerClassLoader)loader;
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

            throw new FatalCompilerErrorException(Message.LinkageError, str);
        }

        static string AssemblyQualifiedName(RuntimeJavaType tw)
        {
            RuntimeClassLoader loader = tw.GetClassLoader();
            RuntimeAssemblyClassLoader acl = loader as RuntimeAssemblyClassLoader;
            if (acl != null)
            {
                return tw.Name + ", " + acl.GetAssembly(tw).FullName;
            }
            CompilerClassLoader ccl = loader as CompilerClassLoader;
            if (ccl != null)
            {
                return tw.Name + ", " + ccl.GetTypeWrapperFactory().ModuleBuilder.Assembly.FullName;
            }
            return tw.Name + " (unknown assembly)";
        }

        internal void IssueMissingTypeMessage(Type type)
        {
            type = ReflectUtil.GetMissingType(type);
            IssueMessage(type.Assembly.__IsMissing ? Message.MissingReference : Message.MissingType, type.FullName, type.Assembly.FullName);
        }

        internal void SuppressWarning(CompilerOptions options, Message message, string name)
        {
            options.suppressWarnings[(int)message + ":" + name] = null;
        }

        internal void IssueMessage(Message msgId, params string[] values)
        {
            IssueMessage(msgId, values);
        }

        internal void IssueMessage(CompilerOptions options, Message msgId, params string[] values)
        {
            if (errorCount != 0 && msgId < Message.StartErrors && !options.warnaserror)
            {
                // don't display any warnings after we've emitted an error message
                return;
            }

            string key = ((int)msgId).ToString();
            for (int i = 0; ; i++)
            {
                if (options.suppressWarnings.ContainsKey(key))
                {
                    return;
                }
                if (i == values.Length)
                {
                    break;
                }
                key += ":" + values[i];
            }
            options.suppressWarnings.Add(key, key);
            if (options.writeSuppressWarningsFile != null)
            {
                File.AppendAllText(options.writeSuppressWarningsFile.FullName, "-nowarn:" + key + Environment.NewLine);
            }
            string msg;
            switch (msgId)
            {
                case Message.MainMethodFound:
                    msg = "Found main method in class \"{0}\"";
                    break;
                case Message.OutputFileIs:
                    msg = "Output file is \"{0}\"";
                    break;
                case Message.AutoAddRef:
                    msg = "Automatically adding reference to \"{0}\"";
                    break;
                case Message.MainMethodFromManifest:
                    msg = "Using main class \"{0}\" based on jar manifest";
                    break;
                case Message.ClassNotFound:
                    msg = "Class \"{0}\" not found";
                    break;
                case Message.ClassFormatError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (class format error \"{1}\")";
                    break;
                case Message.DuplicateClassName:
                    msg = "Duplicate class name: \"{0}\"";
                    break;
                case Message.IllegalAccessError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (illegal access error \"{1}\")";
                    break;
                case Message.VerificationError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (verification error \"{1}\")";
                    break;
                case Message.NoClassDefFoundError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (missing class \"{1}\")";
                    break;
                case Message.GenericUnableToCompileError:
                    msg = "Unable to compile class \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\": \"{2}\")";
                    break;
                case Message.DuplicateResourceName:
                    msg = "Skipping resource (name clash): \"{0}\"";
                    break;
                case Message.SkippingReferencedClass:
                    msg = "Skipping class: \"{0}\"" + Environment.NewLine +
                        "    (class is already available in referenced assembly \"{1}\")";
                    break;
                case Message.NoJniRuntime:
                    msg = "Unable to load runtime JNI assembly";
                    break;
                case Message.EmittedNoClassDefFoundError:
                    msg = "Emitted java.lang.NoClassDefFoundError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedIllegalAccessError:
                    msg = "Emitted java.lang.IllegalAccessError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedInstantiationError:
                    msg = "Emitted java.lang.InstantiationError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedIncompatibleClassChangeError:
                    msg = "Emitted java.lang.IncompatibleClassChangeError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedNoSuchFieldError:
                    msg = "Emitted java.lang.NoSuchFieldError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedAbstractMethodError:
                    msg = "Emitted java.lang.AbstractMethodError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedNoSuchMethodError:
                    msg = "Emitted java.lang.NoSuchMethodError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedLinkageError:
                    msg = "Emitted java.lang.LinkageError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedVerificationError:
                    msg = "Emitted java.lang.VerificationError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.EmittedClassFormatError:
                    msg = "Emitted java.lang.ClassFormatError in \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.InvalidCustomAttribute:
                    msg = "Error emitting \"{0}\" custom attribute" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.IgnoredCustomAttribute:
                    msg = "Custom attribute \"{0}\" was ignored" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.AssumeAssemblyVersionMatch:
                    msg = "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy";
                    break;
                case Message.InvalidDirectoryInLibOptionPath:
                    msg = "Directory \"{0}\" specified in -lib option is not valid";
                    break;
                case Message.InvalidDirectoryInLibEnvironmentPath:
                    msg = "Directory \"{0}\" specified in LIB environment is not valid";
                    break;
                case Message.LegacySearchRule:
                    msg = "Found assembly \"{0}\" using legacy search rule, please append '.dll' to the reference";
                    break;
                case Message.AssemblyLocationIgnored:
                    msg = "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identity \"{2}\"";
                    break;
                case Message.InterfaceMethodCantBeInternal:
                    msg = "Ignoring @ikvm.lang.Internal annotation on interface method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.DllExportMustBeStaticMethod:
                    msg = "Ignoring @ikvm.lang.DllExport annotation on non-static method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.DllExportRequiresSupportedPlatform:
                    msg = "Ignoring @ikvm.lang.DllExport annotation due to unsupported target platform";
                    break;
                case Message.NonPrimaryAssemblyReference:
                    msg = "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader group, please reference primary assembly \"{1}\" instead";
                    break;
                case Message.MissingType:
                    msg = "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found";
                    break;
                case Message.MissingReference:
                    msg = "The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}'";
                    break;
                case Message.DuplicateAssemblyReference:
                    msg = "Duplicate assembly reference \"{0}\"";
                    break;
                case Message.UnableToResolveType:
                    msg = "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not be found";
                    break;
                case Message.StubsAreDeprecated:
                    msg = "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.";
                    break;
                case Message.WrongClassName:
                    msg = "Unable to compile \"{0}\" (wrong name: \"{1}\")";
                    break;
                case Message.ReflectionCallerClassRequiresCallerID:
                    msg = "Reflection.getCallerClass() called from non-CallerID method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.LegacyAssemblyAttributesFound:
                    msg = "Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.";
                    break;
                case Message.UnableToCreateLambdaFactory:
                    msg = "Unable to create static lambda factory.";
                    break;
                case Message.UnableToCreateProxy:
                    msg = "Unable to create proxy \"{0}\"" + Environment.NewLine +
                        "    (\"{1}\")";
                    break;
                case Message.DuplicateProxy:
                    msg = "Duplicate proxy \"{0}\"";
                    break;
                case Message.MapXmlUnableToResolveOpCode:
                    msg = "Unable to resolve opcode in remap file: {0}";
                    break;
                case Message.MapXmlError:
                    msg = "Error in remap file: {0}";
                    break;
                case Message.InputFileNotFound:
                    msg = "Source file '{0}' not found";
                    break;
                case Message.UnknownFileType:
                    msg = "Unknown file type: {0}";
                    break;
                case Message.UnknownElementInMapFile:
                    msg = "Unknown element {0} in remap file, line {1}, column {2}";
                    break;
                case Message.UnknownAttributeInMapFile:
                    msg = "Unknown attribute {0} in remap file, line {1}, column {2}";
                    break;
                case Message.InvalidMemberNameInMapFile:
                    msg = "Invalid {0} name '{1}' in remap file in class {2}";
                    break;
                case Message.InvalidMemberSignatureInMapFile:
                    msg = "Invalid {0} signature '{3}' in remap file for {0} {1}.{2}";
                    break;
                case Message.InvalidPropertyNameInMapFile:
                    msg = "Invalid property {0} name '{3}' in remap file for property {1}.{2}";
                    break;
                case Message.InvalidPropertySignatureInMapFile:
                    msg = "Invalid property {0} signature '{3}' in remap file for property {1}.{2}";
                    break;
                case Message.UnknownWarning:
                    msg = "{0}";
                    break;
                case Message.CallerSensitiveOnUnsupportedMethod:
                    msg = "CallerSensitive annotation on unsupported method" + Environment.NewLine +
                        "    (\"{0}.{1}{2}\")";
                    break;
                case Message.RemappedTypeMissingDefaultInterfaceMethod:
                    msg = "{0} does not implement default interface method {1}";
                    break;
                default:
                    throw new InvalidProgramException();
            }
            bool error = msgId >= Message.StartErrors
                || (options.warnaserror && msgId >= Message.StartWarnings)
                || options.errorWarnings.ContainsKey(key)
                || options.errorWarnings.ContainsKey(((int)msgId).ToString());
            Console.Error.Write("{0} IKVMC{1:D4}: ", error ? "error" : msgId < Message.StartWarnings ? "note" : "warning", (int)msgId);
            if (error && Message.StartWarnings <= msgId && msgId < Message.StartErrors)
            {
                Console.Error.Write("Warning as Error: ");
            }
            Console.Error.WriteLine(msg, values);
            if (options != this.rootTarget && options.path != null)
            {
                Console.Error.WriteLine("    (in {0})", options.path);
            }
            if (error)
            {
                if (++errorCount == 100)
                {
                    throw new FatalCompilerErrorException(Message.MaximumErrorCountReached);
                }
            }
        }

    }

}
