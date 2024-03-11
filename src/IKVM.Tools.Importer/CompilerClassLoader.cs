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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

using IKVM.Attributes;
using IKVM.ByteCode.Reading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    sealed class CompilerClassLoader : RuntimeClassLoader
    {

        const string DEFAULT_RUNTIME_ARGS_PREFIX = "-J";

        Dictionary<string, Jar.Item> classes;
        Dictionary<string, RemapperTypeWrapper> remapped = new Dictionary<string, RemapperTypeWrapper>();
        string assemblyName;
        string assemblyFile;
        string assemblyDir;
        bool targetIsModule;
        AssemblyBuilder assemblyBuilder;
        MapXml.Attribute[] assemblyAttributes;
        CompilerOptions options;
        RuntimeAssemblyClassLoader[] referencedAssemblies;
        Dictionary<string, string> nameMappings = new Dictionary<string, string>();
        Packages packages;
        Dictionary<string, List<RuntimeJavaType>> ghosts;
        RuntimeJavaType[] mappedExceptions;
        bool[] mappedExceptionsAllSubClasses;
        Dictionary<string, MapXml.Class> mapxml_Classes;
        Dictionary<MethodKey, MapXml.InstructionList> mapxml_MethodBodies;
        Dictionary<MethodKey, MapXml.ReplaceMethodCall[]> mapxml_ReplacedMethods;
        Dictionary<MethodKey, MapXml.InstructionList> mapxml_MethodPrologues;
        MapXml.Root map;
        List<string> classesToCompile;
        List<CompilerClassLoader> peerReferences = new List<CompilerClassLoader>();
        Dictionary<string, string> peerLoading = new Dictionary<string, string>();
        List<RuntimeClassLoader> internalsVisibleTo = new List<RuntimeClassLoader>();
        List<RuntimeJavaType> dynamicallyImportedTypes = new List<RuntimeJavaType>();
        List<string> jarList = new List<string>();
        List<RuntimeJavaType> allwrappers;
        FakeTypes fakeTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="referencedAssemblies"></param>
        /// <param name="options"></param>
        /// <param name="assemblyPath"></param>
        /// <param name="targetIsModule"></param>
        /// <param name="assemblyName"></param>
        /// <param name="classes"></param>
        internal CompilerClassLoader(RuntimeContext context, RuntimeAssemblyClassLoader[] referencedAssemblies, CompilerOptions options, FileInfo assemblyPath, bool targetIsModule, string assemblyName, Dictionary<string, Jar.Item> classes) :
            base(context, options.codegenoptions, null)
        {
            this.referencedAssemblies = referencedAssemblies;
            this.options = options;
            this.classes = classes;
            this.assemblyName = assemblyName;
            this.assemblyFile = assemblyPath.Name;
            this.assemblyDir = assemblyPath.DirectoryName;
            this.targetIsModule = targetIsModule;
            Tracer.Info(Tracer.Compiler, "Instantiate CompilerClassLoader for {0}", assemblyName);
        }

        internal bool ReserveName(string javaName)
        {
            return !classes.ContainsKey(javaName) && GetTypeWrapperFactory().ReserveName(javaName);
        }

        internal void AddNameMapping(string javaName, string typeName)
        {
            nameMappings.Add(javaName, typeName);
        }

        internal void AddReference(RuntimeAssemblyClassLoader acl)
        {
            referencedAssemblies = ArrayUtil.Concat(referencedAssemblies, acl);
        }

        internal void AddReference(CompilerClassLoader ccl)
        {
            peerReferences.Add(ccl);
        }

        internal AssemblyName GetAssemblyName()
        {
            return assemblyBuilder.GetName();
        }

        internal ModuleBuilder CreateModuleBuilder()
        {
            var name = new AssemblyName();
            name.Name = assemblyName;
            if (options.keyPair != null)
                name.KeyPair = options.keyPair;
            else if (options.publicKey != null)
                name.SetPublicKey(options.publicKey);

            name.Version = options.version;

            // define a dynamic assembly and module
            assemblyBuilder = Context.StaticCompiler.Universe.DefineDynamicAssembly(name, AssemblyBuilderAccess.ReflectionOnly, assemblyDir);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, EmitSymbols);

            // if configured to emit stack trace info set source file
            if (EmitStackTraceInfo)
                Context.AttributeHelper.SetSourceFile(moduleBuilder, null);

            // if configured to emit debug info or stack trace info, add debug DebuggableAttribute
            if (EmitSymbols || EmitStackTraceInfo)
            {
                var debugModes = DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints;
                if (EmitSymbols)
                    debugModes |= DebuggableAttribute.DebuggingModes.DisableOptimizations;

                Context.AttributeHelper.SetDebuggingModes(assemblyBuilder, debugModes);
            }

            Context.AttributeHelper.SetRuntimeCompatibilityAttribute(assemblyBuilder);

            if (options.baseAddress != 0)
                moduleBuilder.__ImageBase = options.baseAddress;
            if (options.fileAlignment != 0)
                moduleBuilder.__FileAlignment = options.fileAlignment;
            if (options.highentropyva)
                moduleBuilder.__DllCharacteristics |= DllCharacteristics.HighEntropyVA;

            // allow the runtime to "inject" dynamic classes into the assembly
            var mainAssemblyName = options.sharedclassloader != null && options.sharedclassloader[0] != this
                ? options.sharedclassloader[0].assemblyName
                : assemblyName;
            if (!DisableDynamicBinding)
                Context.AttributeHelper.SetInternalsVisibleToAttribute(assemblyBuilder, mainAssemblyName + Context.Options.DynamicAssemblySuffixAndPublicKey);

            return moduleBuilder;
        }

        public override string ToString()
        {
            return "CompilerClassLoader:" + options.assembly;
        }

        protected override RuntimeJavaType LoadClassImpl(string name, LoadMode mode)
        {
            foreach (RuntimeAssemblyClassLoader acl in referencedAssemblies)
            {
                var tw = acl.DoLoad(name);
                if (tw != null)
                    return tw;
            }

            if (!peerLoading.ContainsKey(name))
            {
                peerLoading.Add(name, null);
                try
                {
                    foreach (CompilerClassLoader ccl in peerReferences)
                    {
                        var tw = ccl.PeerLoad(name);
                        if (tw != null)
                            return tw;
                    }
                    if (options.sharedclassloader != null && options.sharedclassloader[0] != this)
                    {
                        var tw = options.sharedclassloader[0].PeerLoad(name);
                        if (tw != null)
                            return tw;
                    }
                }
                finally
                {
                    peerLoading.Remove(name);
                }
            }

            var tw1 = GetTypeWrapperCompilerHook(name);
            if (tw1 != null)
            {
                return tw1;
            }

            // HACK the peer loading mess above may have indirectly loaded the classes without returning it,
            // so we try once more here
            tw1 = FindLoadedClass(name);
            if (tw1 != null)
            {
                return tw1;
            }

            return FindOrLoadGenericClass(name, mode);
        }

        private RuntimeJavaType PeerLoad(string name)
        {
            // To keep the performance acceptable in cases where we're compiling many targets, we first check if the load can
            // possibly succeed on this class loader, otherwise we'll end up doing a lot of futile recursive loading attempts.
            if (classes.ContainsKey(name) || remapped.ContainsKey(name) || FindLoadedClass(name) != null)
            {
                var tw = TryLoadClassByName(name);
                // HACK we don't want to load classes referenced by peers, hence the "== this" check
                if (tw != null && tw.GetClassLoader() == this)
                {
                    return tw;
                }
            }
            if (options.sharedclassloader != null && options.sharedclassloader[0] == this)
            {
                foreach (CompilerClassLoader ccl in options.sharedclassloader)
                {
                    if (ccl != this)
                    {
                        var tw = ccl.PeerLoad(name);
                        if (tw != null)
                        {
                            return tw;
                        }
                    }
                }
            }
            return null;
        }

        private RuntimeJavaType GetTypeWrapperCompilerHook(string name)
        {
            if (remapped.TryGetValue(name, out var rtw))
            {
                return rtw;
            }
            else
            {
                if (classes.TryGetValue(name, out var itemRef))
                {
                    classes.Remove(name);

                    ClassFile f;
                    try
                    {
                        f = new ClassFile(Context, ClassReader.Read(itemRef.GetData()), name, ClassFileParseOptions, null);
                    }
                    catch (ClassFormatError x)
                    {
                        Context.StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
                        Context.StaticCompiler.IssueMessage(options, Message.ClassFormatError, name, x.Message);
                        return null;
                    }

                    if (f.Name != name)
                    {
                        Context.StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
                        Context.StaticCompiler.IssueMessage(options, Message.WrongClassName, name, f.Name);
                        return null;
                    }

                    if (f.IsPublic && options.privatePackages != null)
                    {
                        foreach (string p in options.privatePackages)
                        {
                            if (f.Name.StartsWith(p))
                            {
                                f.SetInternal();
                                break;
                            }
                        }
                    }

                    if (f.IsPublic && options.publicPackages != null)
                    {
                        bool found = false;
                        foreach (string package in options.publicPackages)
                        {
                            if (f.Name.StartsWith(package))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            f.SetInternal();
                        }
                    }

                    if (f.SourceFileAttribute != null)
                    {
                        var path = itemRef.Path;
                        if (path != null)
                        {
                            var sourceFile = Path.GetFullPath(Path.Combine(path.DirectoryName, f.SourceFileAttribute));
                            if (File.Exists(sourceFile))
                                f.SourcePath = sourceFile;
                        }

                        if (f.SourcePath == null)
                        {
                            if (options.sourcepath != null)
                            {
                                var package = f.Name;
                                var index = package.LastIndexOf('.');
                                package = index == -1 ? "" : package.Substring(0, index).Replace('.', '/');
                                f.SourcePath = Path.GetFullPath(Path.Combine(options.sourcepath + "/" + package, f.SourceFileAttribute));
                            }
                            else
                            {
                                f.SourcePath = f.SourceFileAttribute;
                            }
                        }
                    }

                    try
                    {
                        var tw = DefineClass(f, null);
                        // we successfully created the type, so we don't need to include the class as a resource
                        if (options.nojarstubs)
                        {
                            itemRef.Remove();
                        }
                        else
                        {
                            itemRef.MarkAsStub();
                        }

                        int pos = f.Name.LastIndexOf('.');
                        if (pos != -1)
                        {
                            var manifestJar = options.IsClassesJar(itemRef.Jar) ? null : itemRef.Jar.Name;
                            packages.DefinePackage(f.Name.Substring(0, pos), manifestJar);
                        }

                        return tw;
                    }
                    catch (ClassFormatError x)
                    {
                        Context.StaticCompiler.IssueMessage(options, Message.ClassFormatError, name, x.Message);
                    }
                    catch (IllegalAccessError x)
                    {
                        Context.StaticCompiler.IssueMessage(options, Message.IllegalAccessError, name, x.Message);
                    }
                    catch (VerifyError x)
                    {
                        Context.StaticCompiler.IssueMessage(options, Message.VerificationError, name, x.Message);
                    }
                    catch (NoClassDefFoundError x)
                    {
                        if ((options.codegenoptions & CodeGenOptions.DisableDynamicBinding) != 0)
                        {
                            Context.StaticCompiler.IssueMessage(options, Message.NoClassDefFoundError, name, x.Message);
                        }

                        Context.StaticCompiler.IssueMessage(options, Message.ClassNotFound, x.Message);
                    }
                    catch (RetargetableJavaException x)
                    {
                        Context.StaticCompiler.IssueMessage(options, Message.GenericUnableToCompileError, name, x.GetType().Name, x.Message);
                    }

                    Context.StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        // HACK when we're compiling multiple targets with -sharedclassloader, each target will have its own CompilerClassLoader,
        // so we need to consider them equivalent (because they represent the same class loader).
        internal bool IsEquivalentTo(RuntimeClassLoader other)
        {
            if (this == other)
            {
                return true;
            }

            var ccl = other as CompilerClassLoader;
            if (ccl != null && options.sharedclassloader != null && options.sharedclassloader.Contains(ccl))
            {
                if (!internalsVisibleTo.Contains(ccl))
                {
                    AddInternalsVisibleToAttribute(ccl);
                }

                return true;
            }

            return false;
        }

        internal override bool InternalsVisibleToImpl(RuntimeJavaType wrapper, RuntimeJavaType friend)
        {
            Debug.Assert(wrapper.GetClassLoader() == this);
            var other = friend.GetClassLoader();

            // TODO ideally we should also respect InternalsVisibleToAttribute.Annotation here
            if (this == other || internalsVisibleTo.Contains(other))
            {
                return true;
            }

            var ccl = other as CompilerClassLoader;
            if (ccl != null)
            {
                AddInternalsVisibleToAttribute(ccl);
                return true;
            }

            return false;
        }

        private void AddInternalsVisibleToAttribute(CompilerClassLoader ccl)
        {
            internalsVisibleTo.Add(ccl);

            var asm = ccl.assemblyBuilder;
            var asmName = asm.GetName();
            var name = asmName.Name;

            var pubkey = asmName.GetPublicKey();
            if (pubkey == null && asmName.KeyPair != null)
                pubkey = asmName.KeyPair.PublicKey;

            if (pubkey != null && pubkey.Length != 0)
            {
                var sb = new StringBuilder(name);
                sb.Append(", PublicKey=");
                foreach (byte b in pubkey)
                    sb.AppendFormat("{0:X2}", b);
                name = sb.ToString();
            }

            Context.AttributeHelper.SetInternalsVisibleToAttribute(this.assemblyBuilder, name);
        }

        /// <summary>
        /// Emits a global Main method that launches the Java main method on the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="properties"></param>
        /// <param name="noglobbing"></param>
        /// <param name="apartmentAttributeType"></param>
        void SetMain(RuntimeJavaType type, PEFileKinds target, IDictionary<string, string> properties, bool noglobbing, Type apartmentAttributeType)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            // global main method decorated with appropriate apartment type
            var mainMethodProxy = GetTypeWrapperFactory().ModuleBuilder.DefineGlobalMethod("Main", MethodAttributes.Public | MethodAttributes.Static, Context.Types.Int32, new[] { Context.Types.String.MakeArrayType() });
            if (apartmentAttributeType != null)
                mainMethodProxy.SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), Array.Empty<object>()));

            var ilgen = Context.CodeEmitterFactory.Create(mainMethodProxy);

            // first argument to Launch (type name)
            ilgen.Emit(OpCodes.Ldstr, type.Name);

            // second argument: is this a jar
            ilgen.Emit(OpCodes.Ldc_I4_0);

            // third argument: args
            ilgen.Emit(OpCodes.Ldarg_0);

            // fourth argument, runtime prefix
            ilgen.Emit(OpCodes.Ldstr, DEFAULT_RUNTIME_ARGS_PREFIX);

            // fifth argument, property set to initialize JVM
            if (properties.Count > 0)
            {
                var environmentType = Context.Resolver.ResolveCoreType(typeof(Environment).FullName);
                var environmentExpandMethod = environmentType.GetMethod(nameof(Environment.ExpandEnvironmentVariables), new[] { Context.Types.String });
                var dictionaryType = Context.Resolver.ResolveCoreType(typeof(Dictionary<,>).FullName).MakeGenericType(Context.Types.String, Context.Types.String);
                var dictionaryAddMethod = dictionaryType.GetMethod("Add", new[] { Context.Types.String, Context.Types.String });

                ilgen.EmitLdc_I4(properties.Count);
                ilgen.Emit(OpCodes.Newobj, dictionaryType.GetConstructor(new[] { Context.Types.Int32 }));

                foreach (var kvp in properties)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.Emit(OpCodes.Ldstr, kvp.Key);
                    ilgen.Emit(OpCodes.Ldstr, kvp.Value);

                    // property value can reference an environmental variable (reassess the requirment for this)
                    if (kvp.Value.IndexOf('%') < kvp.Value.LastIndexOf('%'))
                        ilgen.Emit(OpCodes.Call, environmentExpandMethod);

                    // add to properties dictionary
                    ilgen.Emit(OpCodes.Callvirt, dictionaryAddMethod);
                }
            }
            else
            {
                ilgen.Emit(OpCodes.Ldnull);
            }

            // invoke the launcher main method
            var launchMethod = Context.Resolver.ResolveRuntimeType("IKVM.Runtime.Launcher").GetMethod("Run");
            ilgen.Emit(OpCodes.Call, launchMethod);
            ilgen.Emit(OpCodes.Ret);

            // generate entry point
            ilgen.DoEmit();
            assemblyBuilder.SetEntryPoint(mainMethodProxy, target);
        }

        void PrepareSave()
        {
            ((DynamicClassLoader)this.GetTypeWrapperFactory()).FinishAll();
        }

        void Save()
        {
            var mb = GetTypeWrapperFactory().ModuleBuilder;
            if (targetIsModule)
            {
                // HACK force all referenced assemblies to end up as references in the assembly
                // (even if they are otherwise unused), to make sure that the assembly class loader
                // delegates to them at runtime.
                // NOTE now we only do this for modules, when we're an assembly we store the exported
                // assemblies in the ikvm.exports resource.
                for (int i = 0; i < referencedAssemblies.Length; i++)
                {
                    var types = referencedAssemblies[i].MainAssembly.GetExportedTypes();
                    if (types.Length > 0)
                        mb.GetTypeToken(types[0]);
                }
            }

            // initialize the global functions for the module
            mb.CreateGlobalFunctions();

            // signify we are creating a Java module
            AddJavaModuleAttribute(mb);

            // ignore access checks to referenced Java modules
            AddIgnoresAccessChecksToAttribute(mb);

            // add a package list and export map
            if (options.sharedclassloader == null || options.sharedclassloader[0] == this)
            {
                var packageListAttributeCtor = Context.Resolver.ResolveRuntimeType(typeof(PackageListAttribute).FullName).GetConstructor(new[] { Context.Types.String, Context.Types.String.MakeArrayType() });
                foreach (var args in packages.ToArray())
                {
                    args[1] = UnicodeUtil.EscapeInvalidSurrogates((string[])args[1]);
                    mb.SetCustomAttribute(new CustomAttributeBuilder(packageListAttributeCtor, args));
                }

                // We can't add the resource when we're a module, because a multi-module assembly has a single resource namespace
                // and since you cannot combine -target:module with -sharedclassloader we don't need an export map
                // (the wildcard exports have already been added above, by making sure that we statically reference the assemblies).
                if (targetIsModule == false)
                {
                    WriteExportMap();
                }
            }

            if (targetIsModule)
            {
                Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);

                try
                {
                    GetTypeWrapperFactory().ModuleBuilder.__Save(options.pekind, options.imageFileMachine);
                }
                catch (IOException x)
                {
                    throw new FatalCompilerErrorException(Message.ErrorWritingFile, GetTypeWrapperFactory().ModuleBuilder.FullyQualifiedName, x.Message);
                }
                catch (UnauthorizedAccessException x)
                {
                    throw new FatalCompilerErrorException(Message.ErrorWritingFile, GetTypeWrapperFactory().ModuleBuilder.FullyQualifiedName, x.Message);
                }
            }
            else
            {
                Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);

                try
                {
                    assemblyBuilder.Save(assemblyFile, options.pekind, options.imageFileMachine);
                }
                catch (IOException x)
                {
                    throw new FatalCompilerErrorException(Message.ErrorWritingFile, Path.Combine(assemblyDir, assemblyFile), x.Message);
                }
                catch (UnauthorizedAccessException x)
                {
                    throw new FatalCompilerErrorException(Message.ErrorWritingFile, Path.Combine(assemblyDir, assemblyFile), x.Message);
                }
            }
        }

        /// <summary>
        /// Adds a 'IgnoresAccessChecksToAttribute' type to the assembly, and attributes on the assembly that ignore
        /// access checks to all Java modules which are referenced. This ensures package-private references across
        /// assemblies are possible.
        /// </summary>
        /// <param name="mb"></param>
        void AddIgnoresAccessChecksToAttribute(ModuleBuilder mb)
        {
            if (targetIsModule == false)
            {
                // add IgnoresAccessChecksToAttribute type
                var iactBuilder = mb.DefineType("System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute", TypeAttributes.Class | TypeAttributes.Sealed, Context.Types.Attribute);
                iactBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { Context.Types.String });
                Context.AttributeHelper.SetAttributeUsage(iactBuilder, AttributeTargets.Assembly, true);
                var iact = iactBuilder.CreateType();
                var iactCtor = iact.GetConstructor(new[] { Context.Types.String });

                // add IgnoresAccessChecksToAttribute to each referenced assembly
                for (int i = 0; i < referencedAssemblies.Length; i++)
                    if (Context.AttributeHelper.IsJavaModule(referencedAssemblies[i].MainAssembly.ManifestModule))
                        assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(iactCtor, new[] { UnicodeUtil.EscapeInvalidSurrogates(referencedAssemblies[i].MainAssembly.FullName) }));
            }
        }

        void AddJavaModuleAttribute(ModuleBuilder mb)
        {
            var typeofJavaModuleAttribute = Context.Resolver.ResolveRuntimeType(typeof(JavaModuleAttribute).FullName);
            var propInfos = new[] { typeofJavaModuleAttribute.GetProperty("Jars") };
            var propValues = new object[] { jarList.ToArray() };

            if (nameMappings.Count > 0)
            {
                var list = new string[nameMappings.Count * 2];
                int i = 0;
                foreach (var kv in nameMappings)
                {
                    list[i++] = kv.Key;
                    list[i++] = kv.Value;
                }

                list = UnicodeUtil.EscapeInvalidSurrogates(list);
                var cab = new CustomAttributeBuilder(typeofJavaModuleAttribute.GetConstructor(new Type[] { Context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType() }), new object[] { list }, propInfos, propValues);
                mb.SetCustomAttribute(cab);
            }
            else
            {
                var cab = new CustomAttributeBuilder(typeofJavaModuleAttribute.GetConstructor(Type.EmptyTypes), new object[0], propInfos, propValues);
                mb.SetCustomAttribute(cab);
            }
        }

        static void AddExportMapEntry(Dictionary<string, List<string>> map, CompilerClassLoader ccl, string name)
        {
            string assemblyName = ccl.assemblyBuilder.FullName;

            if (map.TryGetValue(assemblyName, out var list) == false)
            {
                list = new List<string>();
                map.Add(assemblyName, list);
            }

            if (list != null) // if list is null, we already have a wildcard export for this assembly
                list.Add(name);
        }

        void AddWildcardExports(Dictionary<string, List<string>> exportedNamesPerAssembly)
        {
            foreach (var acl in referencedAssemblies)
                exportedNamesPerAssembly[acl.MainAssembly.FullName] = null;
        }

        void WriteExportMap()
        {
            var exportedNamesPerAssembly = new Dictionary<string, List<string>>();

            AddWildcardExports(exportedNamesPerAssembly);
            foreach (var tw in dynamicallyImportedTypes)
                AddExportMapEntry(exportedNamesPerAssembly, (CompilerClassLoader)tw.GetClassLoader(), tw.Name);

            if (options.sharedclassloader == null)
            {
                foreach (var ccl in peerReferences)
                    exportedNamesPerAssembly[ccl.assemblyBuilder.FullName] = null;
            }
            else
            {
                foreach (var ccl in options.sharedclassloader)
                {
                    if (ccl != this)
                    {
                        ccl.AddWildcardExports(exportedNamesPerAssembly);
                        foreach (var jar in ccl.options.jars)
                            foreach (var item in jar)
                                if (item.IsStub == false)
                                    AddExportMapEntry(exportedNamesPerAssembly, ccl, item.Name);

                        if (ccl.options.externalResources != null)
                            foreach (string name in ccl.options.externalResources.Keys)
                                AddExportMapEntry(exportedNamesPerAssembly, ccl, name);
                    }
                }
            }

            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(exportedNamesPerAssembly.Count);

            foreach (var kv in exportedNamesPerAssembly)
            {
                bw.Write(kv.Key);
                if (kv.Value == null)
                {
                    // wildcard export
                    bw.Write(0);
                }
                else
                {
                    Debug.Assert(kv.Value.Count != 0);
                    bw.Write(kv.Value.Count);
                    foreach (var name in kv.Value)
                        bw.Write(JVM.PersistableHash(name));
                }
            }
            ms.Position = 0;
            GetTypeWrapperFactory().ModuleBuilder.DefineManifestResource("ikvm.exports", ms, ResourceAttributes.Public);
        }

        void WriteResources()
        {
            Tracer.Info(Tracer.Compiler, "CompilerClassLoader adding resources...");

            // BUG we need to call GetTypeWrapperFactory() to make sure that the assemblyBuilder is created (when building an empty target)
            var moduleBuilder = GetTypeWrapperFactory().ModuleBuilder;

            for (int i = 0; i < options.jars.Count; i++)
            {
                var hasEntries = false;
                var mem = new MemoryStream();
                using (var zip = new ZipArchive(mem, ZipArchiveMode.Create))
                {
                    var stubs = new List<string>();
                    foreach (Jar.Item item in options.jars[i])
                    {
                        if (item.IsStub)
                        {
                            // we don't want stub class pseudo resources for classes loaded from the file system
                            if (i != options.classesJar)
                                stubs.Add(item.Name);

                            continue;
                        }
                        var zipEntry = zip.CreateEntry(item.Name, options.compressedResources ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

                        byte[] data = item.GetData();

                        using Stream stream = zipEntry.Open();
                        stream.Write(data, 0, data.Length);

                        hasEntries = true;
                    }

                    if (stubs.Count != 0)
                    {
                        // generate the --ikvm-classes-- file in the jar
                        var zipEntry = zip.CreateEntry(JVM.Internal.JarClassList);

                        using Stream stream = zipEntry.Open();
                        using BinaryWriter bw = new BinaryWriter(stream);

                        bw.Write(stubs.Count);
                        foreach (string classFile in stubs)
                            bw.Write(classFile);

                        hasEntries = true;
                    }
                }

                // don't include empty classes.jar
                if (i != options.classesJar || hasEntries)
                {
                    mem = new MemoryStream(mem.ToArray());
                    var name = options.jars[i].Name;
                    if (options.targetIsModule)
                        name = Path.GetFileNameWithoutExtension(name) + "-" + moduleBuilder.ModuleVersionId.ToString("N") + Path.GetExtension(name);

                    jarList.Add(name);
                    moduleBuilder.DefineManifestResource(name, mem, ResourceAttributes.Public);
                }
            }
        }

        private static MethodAttributes MapMethodAccessModifiers(IKVM.Tools.Importer.MapXml.MapModifiers mod)
        {
            const IKVM.Tools.Importer.MapXml.MapModifiers access = IKVM.Tools.Importer.MapXml.MapModifiers.Public | IKVM.Tools.Importer.MapXml.MapModifiers.Protected | IKVM.Tools.Importer.MapXml.MapModifiers.Private;
            switch (mod & access)
            {
                case IKVM.Tools.Importer.MapXml.MapModifiers.Public:
                    return MethodAttributes.Public;
                case IKVM.Tools.Importer.MapXml.MapModifiers.Protected:
                    return MethodAttributes.FamORAssem;
                case IKVM.Tools.Importer.MapXml.MapModifiers.Private:
                    return MethodAttributes.Private;
                default:
                    return MethodAttributes.Assembly;
            }
        }

        private static FieldAttributes MapFieldAccessModifiers(IKVM.Tools.Importer.MapXml.MapModifiers mod)
        {
            const IKVM.Tools.Importer.MapXml.MapModifiers access = IKVM.Tools.Importer.MapXml.MapModifiers.Public | IKVM.Tools.Importer.MapXml.MapModifiers.Protected | IKVM.Tools.Importer.MapXml.MapModifiers.Private;
            switch (mod & access)
            {
                case IKVM.Tools.Importer.MapXml.MapModifiers.Public:
                    return FieldAttributes.Public;
                case IKVM.Tools.Importer.MapXml.MapModifiers.Protected:
                    return FieldAttributes.FamORAssem;
                case IKVM.Tools.Importer.MapXml.MapModifiers.Private:
                    return FieldAttributes.Private;
                default:
                    return FieldAttributes.Assembly;
            }
        }

        private sealed class RemapperTypeWrapper : RuntimeJavaType
        {
            private CompilerClassLoader classLoader;
            private TypeBuilder typeBuilder;
            private TypeBuilder helperTypeBuilder;
            private Type shadowType;
            private IKVM.Tools.Importer.MapXml.Class classDef;
            private RuntimeJavaType baseTypeWrapper;
            private RuntimeJavaType[] interfaceWrappers;

            internal override RuntimeClassLoader GetClassLoader()
            {
                return classLoader;
            }

            internal override bool IsRemapped
            {
                get
                {
                    return true;
                }
            }

            private static RuntimeJavaType GetBaseWrapper(RuntimeContext context, IKVM.Tools.Importer.MapXml.Class c)
            {
                if ((c.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Interface) != 0)
                {
                    return null;
                }
                if (c.Name == "java.lang.Object")
                {
                    return null;
                }

                return context.JavaBase.TypeOfJavaLangObject;
            }

            internal RemapperTypeWrapper(RuntimeContext context, CompilerClassLoader classLoader, IKVM.Tools.Importer.MapXml.Class c, IKVM.Tools.Importer.MapXml.Root map)
                : base(context, TypeFlags.None, (Modifiers)c.Modifiers, c.Name)
            {
                this.classLoader = classLoader;
                this.baseTypeWrapper = GetBaseWrapper(context, c);
                classDef = c;
                bool baseIsSealed = false;
                shadowType = context.StaticCompiler.Universe.GetType(c.Shadows, true);
                classLoader.SetRemappedType(shadowType, this);
                Type baseType = shadowType;
                Type baseInterface = null;
                if (baseType.IsInterface)
                {
                    baseInterface = baseType;
                }
                TypeAttributes attrs = TypeAttributes.Public;
                if ((c.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Interface) == 0)
                {
                    attrs |= TypeAttributes.Class;
                    if (baseType.IsSealed)
                    {
                        baseIsSealed = true;
                        attrs |= TypeAttributes.Abstract | TypeAttributes.Sealed;
                    }
                }
                else
                {
                    attrs |= TypeAttributes.Interface | TypeAttributes.Abstract;
                    baseType = null;
                }
                if ((c.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Abstract) != 0)
                {
                    attrs |= TypeAttributes.Abstract;
                }
                string name = c.Name.Replace('/', '.');
                typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name, attrs, baseIsSealed ? Context.Types.Object : baseType);
                if (c.Attributes != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in c.Attributes)
                    {
                        Context.AttributeHelper.SetCustomAttribute(classLoader, typeBuilder, custattr);
                    }
                }
                if (baseInterface != null)
                {
                    typeBuilder.AddInterfaceImplementation(baseInterface);
                }
                if (classLoader.EmitStackTraceInfo)
                {
                    Context.AttributeHelper.SetSourceFile(typeBuilder, classLoader.options.remapfile.Name);
                }

                if (baseIsSealed)
                {
                    Context.AttributeHelper.SetModifiers(typeBuilder, (Modifiers)c.Modifiers, false);
                }

                if (c.Scope == MapXml.Scope.Public)
                {
                    // FXBUG we would like to emit an attribute with a Type argument here, but that doesn't work because
                    // of a bug in SetCustomAttribute that causes type arguments to be serialized incorrectly (if the type
                    // is in the same assembly). Normally we use AttributeHelper.FreezeDry to get around this, but that doesn't
                    // work in this case (no attribute is emitted at all). So we work around by emitting a string instead
                    Context.AttributeHelper.SetRemappedClass(classLoader.assemblyBuilder, name, shadowType);

                    Context.AttributeHelper.SetRemappedType(typeBuilder, shadowType);
                }

                var methods = new List<RuntimeJavaMethod>();

                if (c.Constructors != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Constructor m in c.Constructors)
                    {
                        methods.Add(new RemappedConstructorWrapper(this, m));
                    }
                }

                if (c.Methods != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Method m in c.Methods)
                    {
                        methods.Add(new RemappedMethodWrapper(this, m, map, false));
                    }
                }
                // add methods from our super classes (e.g. Throwable should have Object's methods)
                if (!this.IsFinal && !this.IsInterface && this.BaseTypeWrapper != null)
                {
                    foreach (var mw in BaseTypeWrapper.GetMethods())
                    {
                        var rmw = mw as RemappedMethodWrapper;
                        if (rmw != null && (rmw.IsPublic || rmw.IsProtected))
                        {
                            if (!FindMethod(methods, rmw.Name, rmw.Signature))
                            {
                                methods.Add(new RemappedMethodWrapper(this, rmw.XmlMethod, map, true));
                            }
                        }
                    }
                }

                SetMethods(methods.ToArray());
            }

            internal sealed override RuntimeJavaType BaseTypeWrapper
            {
                get { return baseTypeWrapper; }
            }

            internal void LoadInterfaces(IKVM.Tools.Importer.MapXml.Class c)
            {
                if (c.Interfaces != null)
                {
                    interfaceWrappers = new RuntimeJavaType[c.Interfaces.Length];
                    for (int i = 0; i < c.Interfaces.Length; i++)
                    {
                        var iface = classLoader.LoadClassByName(c.Interfaces[i].Class);
                        interfaceWrappers[i] = iface;
                        foreach (var mw in iface.GetMethods())
                        {
                            // make sure default interface methods are implemented (they currently have to be explicitly implemented in map.xml)
                            if (mw.IsVirtual && !mw.IsAbstract)
                            {
                                if (GetMethodWrapper(mw.Name, mw.Signature, true) == null)
                                {
                                    DeclaringTypeWrapper.Context.StaticCompiler.IssueMessage(Message.RemappedTypeMissingDefaultInterfaceMethod, Name, iface.Name + "." + mw.Name + mw.Signature);
                                }
                            }
                        }
                    }
                }
                else
                {
                    interfaceWrappers = Array.Empty<RuntimeJavaType>();
                }
            }

            private static bool FindMethod(List<RuntimeJavaMethod> methods, string name, string sig)
            {
                foreach (var mw in methods)
                {
                    if (mw.Name == name && mw.Signature == sig)
                    {
                        return true;
                    }
                }
                return false;
            }

            abstract class RemappedMethodBaseWrapper : RuntimeJavaMethod
            {

                internal RemappedMethodBaseWrapper(RemapperTypeWrapper typeWrapper, string name, string sig, Modifiers modifiers) :
                    base(typeWrapper, name, sig, null, null, null, modifiers, MemberFlags.None)
                {

                }

                internal abstract MethodBase DoLink();

                internal abstract void Finish();

            }

            sealed class RemappedConstructorWrapper : RemappedMethodBaseWrapper
            {

                private IKVM.Tools.Importer.MapXml.Constructor m;
                private MethodBuilder mbHelper;

                internal RemappedConstructorWrapper(RemapperTypeWrapper typeWrapper, IKVM.Tools.Importer.MapXml.Constructor m)
                    : base(typeWrapper, "<init>", m.Sig, (Modifiers)m.Modifiers)
                {
                    this.m = m;
                }

                internal override void EmitCall(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Call, GetMethod());
                }

                internal override void EmitNewobj(CodeEmitter ilgen)
                {
                    if (mbHelper != null)
                    {
                        ilgen.Emit(OpCodes.Call, mbHelper);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Newobj, GetMethod());
                    }
                }

                internal override MethodBase DoLink()
                {
                    MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
                    RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
                    Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);

                    MethodBuilder cbCore = null;

                    if (typeWrapper.shadowType.IsSealed)
                    {
                        mbHelper = typeWrapper.typeBuilder.DefineMethod("newhelper", attr | MethodAttributes.Static, CallingConventions.Standard, typeWrapper.shadowType, paramTypes);
                        if (m.Attributes != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in m.Attributes)
                            {
                                DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbHelper, custattr);
                            }
                        }
                        SetParameters(DeclaringType.GetClassLoader(), mbHelper, m.Parameters);
                        DeclaringType.Context.AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers, false);
                        DeclaringType.Context.AttributeHelper.SetNameSig(mbHelper, "<init>", m.Sig);
                        AddDeclaredExceptions(DeclaringType.Context, mbHelper, m.Throws);
                    }
                    else
                    {
                        cbCore = ReflectUtil.DefineConstructor(typeWrapper.typeBuilder, attr, paramTypes);
                        if (m.Attributes != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in m.Attributes)
                            {
                                DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), cbCore, custattr);
                            }
                        }
                        SetParameters(DeclaringType.GetClassLoader(), cbCore, m.Parameters);
                        AddDeclaredExceptions(DeclaringType.Context, cbCore, m.Throws);
                    }
                    return cbCore;
                }

                internal override void Finish()
                {
                    // TODO we should insert method tracing (if enabled)

                    Type[] paramTypes = this.GetParametersForDefineMethod();

                    MethodBuilder cbCore = GetMethod() as MethodBuilder;

                    if (cbCore != null)
                    {
                        CodeEmitter ilgen = DeclaringType.Context.CodeEmitterFactory.Create(cbCore);
                        // TODO we need to support ghost (and other funky?) parameter types
                        if (m.Body != null)
                        {
                            // TODO do we need return type conversion here?
                            m.Body.Emit(DeclaringType.GetClassLoader(), ilgen);
                        }
                        else
                        {
                            ilgen.Emit(OpCodes.Ldarg_0);
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i + 1);
                            }
                            if (m.Redirect != null)
                            {
                                throw new NotImplementedException();
                            }
                            else
                            {
                                ConstructorInfo baseCon = DeclaringType.TypeAsTBD.GetConstructor(paramTypes);
                                if (baseCon == null)
                                {
                                    // TODO better error handling
                                    throw new InvalidOperationException("base class constructor not found: " + DeclaringType.Name + ".<init>" + m.Sig);
                                }
                                ilgen.Emit(OpCodes.Call, baseCon);
                            }
                            ilgen.Emit(OpCodes.Ret);
                        }
                        ilgen.DoEmit();
                        if (this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
                        {
                            ilgen.EmitLineNumberTable(cbCore);
                        }
                    }

                    if (mbHelper != null)
                    {
                        CodeEmitter ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mbHelper);
                        if (m.Redirect != null)
                        {
                            m.Redirect.Emit(DeclaringType.GetClassLoader(), ilgen);
                        }
                        else if (m.AlternateBody != null)
                        {
                            m.AlternateBody.Emit(DeclaringType.GetClassLoader(), ilgen);
                        }
                        else if (m.Body != null)
                        {
                            // <body> doesn't make sense for helper constructors (which are actually factory methods)
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            ConstructorInfo baseCon = DeclaringType.TypeAsTBD.GetConstructor(paramTypes);
                            if (baseCon == null)
                            {
                                // TODO better error handling
                                throw new InvalidOperationException("constructor not found: " + DeclaringType.Name + ".<init>" + m.Sig);
                            }
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i);
                            }
                            ilgen.Emit(OpCodes.Newobj, baseCon);
                            ilgen.Emit(OpCodes.Ret);
                        }
                        ilgen.DoEmit();
                        if (this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
                        {
                            ilgen.EmitLineNumberTable(mbHelper);
                        }
                    }
                }
            }

            sealed class RemappedMethodWrapper : RemappedMethodBaseWrapper
            {

                private IKVM.Tools.Importer.MapXml.Method m;
                private IKVM.Tools.Importer.MapXml.Root map;
                private MethodBuilder mbHelper;
                private List<RemapperTypeWrapper> overriders = new List<RemapperTypeWrapper>();
                private bool inherited;

                internal RemappedMethodWrapper(RemapperTypeWrapper typeWrapper, IKVM.Tools.Importer.MapXml.Method m, IKVM.Tools.Importer.MapXml.Root map, bool inherited)
                    : base(typeWrapper, m.Name, m.Sig, (Modifiers)m.Modifiers)
                {
                    this.m = m;
                    this.map = map;
                    this.inherited = inherited;
                }

                internal IKVM.Tools.Importer.MapXml.Method XmlMethod
                {
                    get
                    {
                        return m;
                    }
                }

                internal override void EmitCall(CodeEmitter ilgen)
                {
                    if (!IsStatic && IsFinal)
                    {
                        // When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
                        // then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
                        // is final, this won't affect the semantics.
                        EmitCallvirt(ilgen);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Call, (MethodInfo)GetMethod());
                    }
                }

                internal override void EmitCallvirt(CodeEmitter ilgen)
                {
                    EmitCallvirtImpl(ilgen, this.IsProtected && !mbHelper.IsPublic);
                }

                private void EmitCallvirtImpl(CodeEmitter ilgen, bool cloneOrFinalizeHack)
                {
                    if (mbHelper != null && !cloneOrFinalizeHack)
                    {
                        ilgen.Emit(OpCodes.Call, mbHelper);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Callvirt, (MethodInfo)GetMethod());
                    }
                }

                internal override MethodBase DoLink()
                {
                    RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;

                    if (typeWrapper.IsInterface)
                    {
                        if (m.Override == null)
                        {
                            throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
                        }
                        MethodInfo interfaceMethod = typeWrapper.shadowType.GetMethod(m.Override.Name, typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig));
                        if (interfaceMethod == null)
                        {
                            throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
                        }
                        // if any of the remapped types has a body for this interface method, we need a helper method
                        // to special invocation through this interface for that type
                        List<IKVM.Tools.Importer.MapXml.Class> specialCases = null;
                        foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                        {
                            if (c.Methods != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Method mm in c.Methods)
                                {
                                    if (mm.Name == m.Name && mm.Sig == m.Sig && mm.Body != null)
                                    {
                                        if (specialCases == null)
                                        {
                                            specialCases = new List<IKVM.Tools.Importer.MapXml.Class>();
                                        }
                                        specialCases.Add(c);
                                        break;
                                    }
                                }
                            }
                        }
                        string[] throws;
                        if (m.Throws == null)
                        {
                            throws = new string[0];
                        }
                        else
                        {
                            throws = new string[m.Throws.Length];
                            for (int i = 0; i < throws.Length; i++)
                            {
                                throws[i] = m.Throws[i].Class;
                            }
                        }
                        DeclaringType.Context.AttributeHelper.SetRemappedInterfaceMethod(typeWrapper.typeBuilder, m.Name, m.Override.Name, throws);
                        MethodBuilder helper = null;
                        if (specialCases != null)
                        {
                            CodeEmitter ilgen;
                            Type[] argTypes = ArrayUtil.Concat(typeWrapper.shadowType, typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig));
                            if (typeWrapper.helperTypeBuilder == null)
                            {
                                typeWrapper.helperTypeBuilder = typeWrapper.typeBuilder.DefineNestedType("__Helper", TypeAttributes.NestedPublic | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract);
                                DeclaringType.Context.AttributeHelper.HideFromJava(typeWrapper.helperTypeBuilder);
                            }
                            helper = typeWrapper.helperTypeBuilder.DefineMethod(m.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType, argTypes);
                            if (m.Attributes != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in m.Attributes)
                                {
                                    DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), helper, custattr);
                                }
                            }
                            SetParameters(DeclaringType.GetClassLoader(), helper, m.Parameters);
                            ilgen = DeclaringType.Context.CodeEmitterFactory.Create(helper);
                            foreach (IKVM.Tools.Importer.MapXml.Class c in specialCases)
                            {
                                var tw = typeWrapper.GetClassLoader().LoadClassByName(c.Name);
                                ilgen.Emit(OpCodes.Ldarg_0);
                                ilgen.Emit(OpCodes.Isinst, tw.TypeAsTBD);
                                ilgen.Emit(OpCodes.Dup);
                                CodeEmitterLabel label = ilgen.DefineLabel();
                                ilgen.EmitBrfalse(label);
                                for (int i = 1; i < argTypes.Length; i++)
                                {
                                    ilgen.EmitLdarg(i);
                                }
                                var mw = tw.GetMethodWrapper(m.Name, m.Sig, false);
                                mw.Link();
                                mw.EmitCallvirt(ilgen);
                                ilgen.Emit(OpCodes.Ret);
                                ilgen.MarkLabel(label);
                                ilgen.Emit(OpCodes.Pop);
                            }
                            for (int i = 0; i < argTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i);
                            }
                            ilgen.Emit(OpCodes.Callvirt, interfaceMethod);
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();
                        }
                        mbHelper = helper;
                        return interfaceMethod;
                    }
                    else
                    {
                        MethodBuilder mbCore = null;
                        Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);
                        Type retType = typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType;

                        if (typeWrapper.shadowType.IsSealed && (m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Static) == 0)
                        {
                            // skip instance methods in sealed types, but we do need to add them to the overriders
                            if (typeWrapper.BaseTypeWrapper != null && (m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Private) == 0)
                            {
                                RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(m.Name, m.Sig, true) as RemappedMethodWrapper;
                                if (baseMethod != null &&
                                    !baseMethod.IsFinal &&
                                    !baseMethod.IsPrivate &&
                                    (baseMethod.m.Override != null ||
                                    baseMethod.m.Redirect != null ||
                                    baseMethod.m.Body != null ||
                                    baseMethod.m.AlternateBody != null))
                                {
                                    baseMethod.overriders.Add(typeWrapper);
                                }
                            }
                        }
                        else
                        {
                            MethodInfo overrideMethod = null;
                            MethodAttributes attr = m.MethodAttributes | MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.HideBySig;
                            if ((m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Static) != 0)
                            {
                                attr |= MethodAttributes.Static;
                            }
                            else if ((m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Private) == 0 && (m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Final) == 0)
                            {
                                attr |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
                                if (!typeWrapper.shadowType.IsSealed)
                                {
                                    MethodInfo autoOverride = typeWrapper.shadowType.GetMethod(m.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                                    if (autoOverride != null && autoOverride.ReturnType == retType && !autoOverride.IsFinal)
                                    {
                                        // the method we're processing is overriding a method in its shadowType (which is the actual base type)
                                        attr &= ~MethodAttributes.NewSlot;
                                    }
                                }
                                if (typeWrapper.BaseTypeWrapper != null)
                                {
                                    RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(m.Name, m.Sig, true) as RemappedMethodWrapper;
                                    if (baseMethod != null)
                                    {
                                        baseMethod.overriders.Add(typeWrapper);
                                        if (baseMethod.m.Override != null)
                                        {
                                            overrideMethod = typeWrapper.BaseTypeWrapper.TypeAsTBD.GetMethod(baseMethod.m.Override.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                                            if (overrideMethod == null)
                                            {
                                                throw new InvalidOperationException();
                                            }
                                        }
                                    }
                                }
                            }
                            mbCore = GetDefineMethodHelper().DefineMethod(DeclaringType.GetClassLoader().GetTypeWrapperFactory(), typeWrapper.typeBuilder, m.Name, attr);
                            if (m.Attributes != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in m.Attributes)
                                {
                                    DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbCore, custattr);
                                }
                            }
                            SetParameters(DeclaringType.GetClassLoader(), mbCore, m.Parameters);
                            if (overrideMethod != null && !inherited)
                            {
                                typeWrapper.typeBuilder.DefineMethodOverride(mbCore, overrideMethod);
                            }
                            if (inherited)
                            {
                                DeclaringType.Context.AttributeHelper.HideFromReflection(mbCore);
                            }
                            AddDeclaredExceptions(DeclaringType.Context, mbCore, m.Throws);
                        }

                        if ((m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Static) == 0 && !IsHideFromJava(m))
                        {
                            // instance methods must have an instancehelper method
                            MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.HideBySig | MethodAttributes.Static;
                            // NOTE instancehelpers for protected methods are made internal
                            // and special cased in DotNetTypeWrapper.LazyPublishMembers
                            if ((m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Protected) != 0)
                            {
                                attr &= ~MethodAttributes.MemberAccessMask;
                                attr |= MethodAttributes.Assembly;
                            }
                            mbHelper = typeWrapper.typeBuilder.DefineMethod("instancehelper_" + m.Name, attr, CallingConventions.Standard, retType, ArrayUtil.Concat(typeWrapper.shadowType, paramTypes));
                            if (m.Attributes != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in m.Attributes)
                                {
                                    DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbHelper, custattr);
                                }
                            }
                            IKVM.Tools.Importer.MapXml.Parameter[] parameters;
                            if (m.Parameters == null)
                            {
                                parameters = new IKVM.Tools.Importer.MapXml.Parameter[1];
                            }
                            else
                            {
                                parameters = new IKVM.Tools.Importer.MapXml.Parameter[m.Parameters.Length + 1];
                                m.Parameters.CopyTo(parameters, 1);
                            }
                            parameters[0] = new IKVM.Tools.Importer.MapXml.Parameter();
                            parameters[0].Name = "this";
                            SetParameters(DeclaringType.GetClassLoader(), mbHelper, parameters);
                            if (!typeWrapper.IsFinal)
                            {
                                DeclaringType.Context.AttributeHelper.SetEditorBrowsableNever(mbHelper);
                            }
                            DeclaringType.Context.AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers, false);
                            DeclaringType.Context.AttributeHelper.SetNameSig(mbHelper, m.Name, m.Sig);
                            AddDeclaredExceptions(DeclaringType.Context, mbHelper, m.Throws);
                            mbHelper.SetCustomAttribute(new CustomAttributeBuilder(DeclaringType.Context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor(new Type[] { DeclaringType.Context.Types.String }), new object[] { "This function will be removed from future versions. Please use extension methods from ikvm.extensions namespace instead." }));
                        }
                        return mbCore;
                    }
                }

                private static bool IsHideFromJava(IKVM.Tools.Importer.MapXml.Method m)
                {
                    if (m.Attributes != null)
                    {
                        foreach (MapXml.Attribute attr in m.Attributes)
                        {
                            if (attr.Type == "IKVM.Attributes.HideFromJavaAttribute")
                            {
                                return true;
                            }
                        }
                    }
                    return m.Name.StartsWith("__<", StringComparison.Ordinal);
                }

                internal override void Finish()
                {
                    // TODO we should insert method tracing (if enabled)
                    Type[] paramTypes = this.GetParametersForDefineMethod();

                    MethodBuilder mbCore = GetMethod() as MethodBuilder;

                    // NOTE sealed types don't have instance methods (only instancehelpers)
                    if (mbCore != null)
                    {
                        CodeEmitter ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mbCore);
                        MethodInfo baseMethod = null;
                        if (m.Override != null)
                        {
                            baseMethod = DeclaringType.TypeAsTBD.GetMethod(m.Override.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                            if (baseMethod == null)
                            {
                                throw new InvalidOperationException();
                            }
                            ((TypeBuilder)DeclaringType.TypeAsBaseType).DefineMethodOverride(mbCore, baseMethod);
                        }
                        // TODO we need to support ghost (and other funky?) parameter types
                        if (m.Body != null)
                        {
                            // we manually walk the instruction list, because we need to special case the ret instructions
                            IKVM.Tools.Importer.MapXml.CodeGenContext context = new IKVM.Tools.Importer.MapXml.CodeGenContext(DeclaringType.GetClassLoader());
                            foreach (IKVM.Tools.Importer.MapXml.Instruction instr in m.Body.Instructions)
                            {
                                if (instr is IKVM.Tools.Importer.MapXml.Ret)
                                {
                                    this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                                }
                                instr.Generate(context, ilgen);
                            }
                        }
                        else
                        {
                            if (m.Redirect != null && m.Redirect.LineNumber != -1)
                                ilgen.SetLineNumber((ushort)m.Redirect.LineNumber);

                            int thisOffset = 0;
                            if ((m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Static) == 0)
                            {
                                thisOffset = 1;
                                ilgen.Emit(OpCodes.Ldarg_0);
                            }
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i + thisOffset);
                            }
                            if (m.Redirect != null)
                            {
                                EmitRedirect(DeclaringType.TypeAsTBD, ilgen);
                            }
                            else
                            {
                                if (baseMethod == null)
                                {
                                    throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
                                }
                                ilgen.Emit(OpCodes.Call, baseMethod);
                            }
                            this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                            ilgen.Emit(OpCodes.Ret);
                        }
                        ilgen.DoEmit();
                        if (this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
                        {
                            ilgen.EmitLineNumberTable(mbCore);
                        }
                    }

                    // NOTE static methods don't have helpers
                    // NOTE for interface helpers we don't have to do anything,
                    // because they've already been generated in DoLink
                    // (currently this only applies to Comparable.compareTo).
                    if (mbHelper != null && !this.DeclaringType.IsInterface)
                    {
                        CodeEmitter ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mbHelper);
                        // check "this" for null
                        if (m.Override != null && m.Redirect == null && m.Body == null && m.AlternateBody == null)
                        {
                            // we're going to be calling the overridden version, so we don't need the null check
                        }
                        else if (!m.NoNullCheck)
                        {
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.EmitNullCheck();
                        }
                        if (mbCore != null &&
                            (m.Override == null || m.Redirect != null) &&
                            (m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Private) == 0 && (m.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Final) == 0)
                        {
                            // TODO we should have a way to supress this for overridden methods
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Isinst, DeclaringType.TypeAsBaseType);
                            ilgen.Emit(OpCodes.Dup);
                            CodeEmitterLabel skip = ilgen.DefineLabel();
                            ilgen.EmitBrfalse(skip);
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i + 1);
                            }
                            ilgen.Emit(OpCodes.Callvirt, mbCore);
                            this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.MarkLabel(skip);
                            ilgen.Emit(OpCodes.Pop);
                        }
                        foreach (RemapperTypeWrapper overrider in overriders)
                        {
                            RemappedMethodWrapper mw = (RemappedMethodWrapper)overrider.GetMethodWrapper(Name, Signature, false);
                            if (mw.m.Redirect == null && mw.m.Body == null && mw.m.AlternateBody == null)
                            {
                                // the overridden method doesn't actually do anything special (that means it will end
                                // up calling the .NET method it overrides), so we don't need to special case this
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Ldarg_0);
                                ilgen.Emit(OpCodes.Isinst, overrider.TypeAsTBD);
                                ilgen.Emit(OpCodes.Dup);
                                CodeEmitterLabel skip = ilgen.DefineLabel();
                                ilgen.EmitBrfalse(skip);
                                for (int i = 0; i < paramTypes.Length; i++)
                                {
                                    ilgen.EmitLdarg(i + 1);
                                }
                                mw.Link();
                                mw.EmitCallvirtImpl(ilgen, false);
                                this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                                ilgen.Emit(OpCodes.Ret);
                                ilgen.MarkLabel(skip);
                                ilgen.Emit(OpCodes.Pop);
                            }
                        }
                        if (m.Body != null || m.AlternateBody != null)
                        {
                            IKVM.Tools.Importer.MapXml.InstructionList body = m.AlternateBody == null ? m.Body : m.AlternateBody;
                            // we manually walk the instruction list, because we need to special case the ret instructions
                            IKVM.Tools.Importer.MapXml.CodeGenContext context = new IKVM.Tools.Importer.MapXml.CodeGenContext(DeclaringType.GetClassLoader());
                            foreach (IKVM.Tools.Importer.MapXml.Instruction instr in body.Instructions)
                            {
                                if (instr is IKVM.Tools.Importer.MapXml.Ret)
                                {
                                    this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                                }
                                instr.Generate(context, ilgen);
                            }
                        }
                        else
                        {
                            if (m.Redirect != null && m.Redirect.LineNumber != -1)
                            {
                                ilgen.SetLineNumber((ushort)m.Redirect.LineNumber);
                            }

                            var shadowType = ((RemapperTypeWrapper)DeclaringType).shadowType;
                            for (int i = 0; i < paramTypes.Length + 1; i++)
                                ilgen.EmitLdarg(i);

                            if (m.Redirect != null)
                            {
                                EmitRedirect(shadowType, ilgen);
                            }
                            else if (m.Override != null)
                            {
                                var baseMethod = shadowType.GetMethod(m.Override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
                                if (baseMethod == null)
                                    throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);

                                ilgen.Emit(OpCodes.Callvirt, baseMethod);
                            }
                            else
                            {
                                var baseMethod = DeclaringType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true) as RemappedMethodWrapper;
                                if (baseMethod == null || baseMethod.m.Override == null)
                                    throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);

                                var overrideMethod = shadowType.GetMethod(baseMethod.m.Override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
                                if (overrideMethod == null)
                                    throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);

                                ilgen.Emit(OpCodes.Callvirt, overrideMethod);
                            }

                            ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
                            ilgen.Emit(OpCodes.Ret);
                        }

                        ilgen.DoEmit();

                        if (DeclaringType.GetClassLoader().EmitStackTraceInfo)
                            ilgen.EmitLineNumberTable(mbHelper);
                    }

                    // do we need a helper for non-virtual reflection invocation?
                    if (m.NonVirtualAlternateBody != null || (m.Override != null && overriders.Count > 0))
                    {
                        var tw = (RemapperTypeWrapper)DeclaringType;
                        var mb = tw.typeBuilder.DefineMethod("nonvirtualhelper/" + Name, MethodAttributes.Private | MethodAttributes.Static, ReturnTypeForDefineMethod, ArrayUtil.Concat(tw.TypeAsSignatureType, GetParametersForDefineMethod()));

                        // apply custom attributes from map XML
                        if (m.Attributes != null)
                            foreach (var custattr in m.Attributes)
                                DeclaringType.Context.AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mb, custattr);

                        SetParameters(DeclaringType.GetClassLoader(), mb, m.Parameters);
                        DeclaringType.Context.AttributeHelper.HideFromJava(mb);

                        var ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mb);
                        if (m.NonVirtualAlternateBody != null)
                        {
                            m.NonVirtualAlternateBody.Emit(DeclaringType.GetClassLoader(), ilgen);
                        }
                        else
                        {
                            var shadowType = ((RemapperTypeWrapper)DeclaringType).shadowType;
                            var baseMethod = shadowType.GetMethod(m.Override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
                            if (baseMethod == null)
                                throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);

                            ilgen.Emit(OpCodes.Ldarg_0);
                            for (int i = 0; i < paramTypes.Length; i++)
                                ilgen.EmitLdarg(i + 1);

                            ilgen.Emit(OpCodes.Call, baseMethod);
                            ilgen.Emit(OpCodes.Ret);
                        }

                        ilgen.DoEmit();
                    }
                }

                private void EmitRedirect(Type baseType, CodeEmitter ilgen)
                {
                    var redirName = m.Redirect.Name ?? m.Name;
                    var redirSig = m.Redirect.Sig ?? m.Sig;
                    var classLoader = DeclaringType.GetClassLoader();

                    // type specified, or class missing, assume loading .NET type
                    if (m.Redirect.Type != null || m.Redirect.Class == null)
                    {
                        var type = m.Redirect.Type != null ? DeclaringType.Context.StaticCompiler.Universe.GetType(m.Redirect.Type, true) : baseType;
                        var redirParamTypes = classLoader.ArgTypeListFromSig(redirSig);
                        var mi = type.GetMethod(m.Redirect.Name, redirParamTypes) ?? throw new InvalidOperationException();
                        ilgen.Emit(OpCodes.Call, mi);
                    }
                    else
                    {
                        var tw = classLoader.LoadClassByName(m.Redirect.Class);
                        var mw = tw.GetMethodWrapper(redirName, redirSig, false) ?? throw new InvalidOperationException("Missing redirect method: " + tw.Name + "." + redirName + redirSig);
                        mw.Link();
                        mw.EmitCall(ilgen);
                    }
                }
            }

            private static void SetParameters(RuntimeClassLoader loader, MethodBuilder mb, IKVM.Tools.Importer.MapXml.Parameter[] parameters)
            {
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterBuilder pb = mb.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
                        if (parameters[i].Attributes != null)
                        {
                            for (int j = 0; j < parameters[i].Attributes.Length; j++)
                            {
                                loader.Context.AttributeHelper.SetCustomAttribute(loader, pb, parameters[i].Attributes[j]);
                            }
                        }
                    }
                }
            }

            internal void Process2ndPassStep1()
            {
                if (!shadowType.IsSealed)
                {
                    foreach (var ifaceTypeWrapper in interfaceWrappers)
                    {
                        typeBuilder.AddInterfaceImplementation(ifaceTypeWrapper.TypeAsBaseType);
                    }
                }
                Context.AttributeHelper.SetImplementsAttribute(typeBuilder, interfaceWrappers);
            }

            internal void Process2ndPassStep2(IKVM.Tools.Importer.MapXml.Root map)
            {
                var c = classDef;
                var tb = typeBuilder;

                var fields = new List<RuntimeJavaField>();

                // TODO fields should be moved to the RemapperTypeWrapper constructor as well
                if (c.Fields != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Field f in c.Fields)
                    {
                        {
                            FieldAttributes attr = MapFieldAccessModifiers(f.Modifiers);
                            if (f.Constant != null)
                            {
                                attr |= FieldAttributes.Literal;
                            }
                            else if ((f.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Final) != 0)
                            {
                                attr |= FieldAttributes.InitOnly;
                            }
                            if ((f.Modifiers & IKVM.Tools.Importer.MapXml.MapModifiers.Static) != 0)
                            {
                                attr |= FieldAttributes.Static;
                            }
                            FieldBuilder fb = tb.DefineField(f.Name, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType, attr);
                            if (f.Attributes != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Attribute custattr in f.Attributes)
                                {
                                    Context.AttributeHelper.SetCustomAttribute(classLoader, fb, custattr);
                                }
                            }
                            object constant;
                            if (f.Constant != null)
                            {
                                switch (f.Sig[0])
                                {
                                    case 'J':
                                        constant = long.Parse(f.Constant);
                                        break;
                                    default:
                                        // TODO support other types
                                        throw new NotImplementedException("remapped constant field of type: " + f.Sig);
                                }
                                fb.SetConstant(constant);
                                fields.Add(new RuntimeConstantJavaField(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, constant, MemberFlags.None));
                            }
                            else
                            {
                                fields.Add(RuntimeJavaField.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow), fb, f.Name, f.Sig, new ExModifiers((Modifiers)f.Modifiers, false)));
                            }
                        }
                    }
                }
                SetFields(fields.ToArray());
            }

            internal void Process3rdPass()
            {
                foreach (RemappedMethodBaseWrapper m in GetMethods())
                {
                    m.Link();
                }
            }

            internal void Process4thPass(ICollection<RemapperTypeWrapper> remappedTypes)
            {
                foreach (RemappedMethodBaseWrapper m in GetMethods())
                {
                    m.Finish();
                }

                if (classDef.Clinit != null)
                {
                    MethodBuilder cb = ReflectUtil.DefineTypeInitializer(typeBuilder, classLoader);
                    CodeEmitter ilgen = Context.CodeEmitterFactory.Create(cb);
                    // TODO emit code to make sure super class is initialized
                    classDef.Clinit.Body.Emit(classLoader, ilgen);
                    ilgen.DoEmit();
                }

                // FXBUG because the AppDomain.TypeResolve event doesn't work correctly for inner classes,
                // we need to explicitly finish the interface we implement (if they are ghosts, we need the nested __Interface type)
                if (classDef.Interfaces != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Implements iface in classDef.Interfaces)
                    {
                        GetClassLoader().LoadClassByName(iface.Class).Finish();
                    }
                }

                CreateShadowInstanceOf(remappedTypes);
                CreateShadowCheckCast(remappedTypes);

                if (!shadowType.IsInterface)
                {
                    // For all inherited methods, we emit a method that hides the inherited method and
                    // annotate it with EditorBrowsableAttribute(EditorBrowsableState.Never) to make
                    // sure the inherited methods don't show up in Intellisense.
                    var methods = new Dictionary<string, MethodBuilder>();
                    foreach (var mw in GetMethods())
                    {
                        var mb = mw.GetMethod() as MethodBuilder;
                        if (mb != null)
                            methods.Add(MakeMethodKey(mb), mb);
                    }

                    foreach (var mi in typeBuilder.BaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                    {
                        string key = MakeMethodKey(mi);
                        if (!methods.ContainsKey(key))
                        {
                            ParameterInfo[] paramInfo = mi.GetParameters();
                            Type[] paramTypes = new Type[paramInfo.Length];
                            for (int i = 0; i < paramInfo.Length; i++)
                            {
                                paramTypes[i] = paramInfo[i].ParameterType;
                            }
                            MethodBuilder mb = typeBuilder.DefineMethod(mi.Name, mi.Attributes & (MethodAttributes.MemberAccessMask | MethodAttributes.SpecialName | MethodAttributes.Static), mi.ReturnType, paramTypes);
                            Context.AttributeHelper.HideFromJava(mb);
                            Context.AttributeHelper.SetEditorBrowsableNever(mb);
                            CodeEmitter ilgen = Context.CodeEmitterFactory.Create(mb);
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                ilgen.EmitLdarg(i);
                            }
                            if (!mi.IsStatic)
                            {
                                ilgen.EmitLdarg(paramTypes.Length);
                                ilgen.Emit(OpCodes.Callvirt, mi);
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Call, mi);
                            }
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();
                            methods[key] = mb;
                        }
                    }

                    foreach (var pi in typeBuilder.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                    {
                        ParameterInfo[] paramInfo = pi.GetIndexParameters();
                        Type[] paramTypes = new Type[paramInfo.Length];
                        for (int i = 0; i < paramInfo.Length; i++)
                        {
                            paramTypes[i] = paramInfo[i].ParameterType;
                        }
                        PropertyBuilder pb = typeBuilder.DefineProperty(pi.Name, PropertyAttributes.None, pi.PropertyType, paramTypes);
                        if (pi.GetGetMethod() != null)
                        {
                            pb.SetGetMethod(methods[MakeMethodKey(pi.GetGetMethod())]);
                        }
                        if (pi.GetSetMethod() != null)
                        {
                            pb.SetSetMethod(methods[MakeMethodKey(pi.GetSetMethod())]);
                        }
                        Context.AttributeHelper.SetEditorBrowsableNever(pb);
                    }
                }

                typeBuilder.CreateType();
                if (helperTypeBuilder != null)
                    helperTypeBuilder.CreateType();
            }

            private static string MakeMethodKey(MethodInfo method)
            {
                var sb = new StringBuilder();
                sb.Append(method.ReturnType.AssemblyQualifiedName).Append(":").Append(method.Name);
                var paramInfo = method.GetParameters();
                var paramTypes = new Type[paramInfo.Length];
                for (int i = 0; i < paramInfo.Length; i++)
                {
                    paramTypes[i] = paramInfo[i].ParameterType;
                    sb.Append(":").Append(paramInfo[i].ParameterType.AssemblyQualifiedName);
                }

                return sb.ToString();
            }

            private void CreateShadowInstanceOf(ICollection<RemapperTypeWrapper> remappedTypes)
            {
                // FXBUG .NET 1.1 doesn't allow static methods on interfaces
                if (typeBuilder.IsInterface)
                    return;

                MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
                MethodBuilder mb = typeBuilder.DefineMethod("__<instanceof>", attr, Context.Types.Boolean, new Type[] { Context.Types.Object });
                Context.AttributeHelper.HideFromJava(mb);
                Context.AttributeHelper.SetEditorBrowsableNever(mb);
                CodeEmitter ilgen = Context.CodeEmitterFactory.Create(mb);

                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Isinst, shadowType);
                CodeEmitterLabel retFalse = ilgen.DefineLabel();
                ilgen.EmitBrfalse(retFalse);

                if (!shadowType.IsSealed)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Isinst, typeBuilder);
                    ilgen.EmitBrtrue(retFalse);
                }

                if (shadowType == Context.Types.Object)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Isinst, Context.Types.Array);
                    ilgen.EmitBrtrue(retFalse);
                }

                foreach (RemapperTypeWrapper r in remappedTypes)
                {
                    if (!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
                    {
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Isinst, r.shadowType);
                        ilgen.EmitBrtrue(retFalse);
                    }
                }
                ilgen.Emit(OpCodes.Ldc_I4_1);
                ilgen.Emit(OpCodes.Ret);

                ilgen.MarkLabel(retFalse);
                ilgen.Emit(OpCodes.Ldc_I4_0);
                ilgen.Emit(OpCodes.Ret);

                ilgen.DoEmit();
            }

            private void CreateShadowCheckCast(ICollection<RemapperTypeWrapper> remappedTypes)
            {
                // FXBUG .NET 1.1 doesn't allow static methods on interfaces
                if (typeBuilder.IsInterface)
                {
                    return;
                }
                MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
                MethodBuilder mb = typeBuilder.DefineMethod("__<checkcast>", attr, shadowType, new Type[] { Context.Types.Object });
                Context.AttributeHelper.HideFromJava(mb);
                Context.AttributeHelper.SetEditorBrowsableNever(mb);
                CodeEmitter ilgen = Context.CodeEmitterFactory.Create(mb);

                CodeEmitterLabel fail = ilgen.DefineLabel();
                bool hasfail = false;

                if (!shadowType.IsSealed)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Isinst, typeBuilder);
                    ilgen.EmitBrtrue(fail);
                    hasfail = true;
                }

                if (shadowType == Context.Types.Object)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Isinst, Context.Types.Array);
                    ilgen.EmitBrtrue(fail);
                    hasfail = true;
                }

                foreach (RemapperTypeWrapper r in remappedTypes)
                {
                    if (!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
                    {
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Isinst, r.shadowType);
                        ilgen.EmitBrtrue(fail);
                        hasfail = true;
                    }
                }
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.EmitCastclass(shadowType);
                ilgen.Emit(OpCodes.Ret);

                if (hasfail)
                {
                    ilgen.MarkLabel(fail);
                    ilgen.ThrowException(Context.Resolver.ResolveCoreType(typeof(InvalidCastException).FullName));
                }

                ilgen.DoEmit();
            }

            internal override MethodBase LinkMethod(RuntimeJavaMethod mw)
            {
                return ((RemappedMethodBaseWrapper)mw).DoLink();
            }

            internal override RuntimeJavaType[] Interfaces
            {
                get
                {
                    return interfaceWrappers;
                }
            }

            internal override Type TypeAsTBD
            {
                get
                {
                    return shadowType;
                }
            }

            internal override Type TypeAsBaseType
            {
                get
                {
                    return typeBuilder;
                }
            }

            internal override bool IsMapUnsafeException
            {
                get
                {
                    // any remapped exceptions are automatically unsafe
                    return shadowType == Context.Types.Exception || shadowType.IsSubclassOf(Context.Types.Exception);
                }
            }

            internal override bool IsFastClassLiteralSafe
            {
                get { return true; }
            }
        }

        internal static void AddDeclaredExceptions(RuntimeContext context, MethodBuilder mb, IKVM.Tools.Importer.MapXml.Throws[] throws)
        {
            if (throws != null)
            {
                string[] exceptions = new string[throws.Length];
                for (int i = 0; i < exceptions.Length; i++)
                {
                    exceptions[i] = throws[i].Class;
                }
                context.AttributeHelper.SetThrowsAttribute(mb, exceptions);
            }
        }

        internal void EmitRemappedTypes()
        {
            Tracer.Info(Tracer.Compiler, "Emit remapped types");

            assemblyAttributes = map.Assembly.Attributes;

            if (map.Assembly.Classes != null)
            {
                // 1st pass, put all types in remapped to make them loadable
                bool hasRemappedTypes = false;
                foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                {
                    if (c.Shadows != null)
                    {
                        if (classes.ContainsKey(c.Name))
                        {
                            Context.StaticCompiler.IssueMessage(Message.DuplicateClassName, c.Name);
                        }
                        remapped.Add(c.Name, new RemapperTypeWrapper(Context, this, c, map));
                        hasRemappedTypes = true;
                    }
                }

                if (hasRemappedTypes)
                {
                    SetupGhosts(map);
                    foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                    {
                        if (c.Shadows != null)
                        {
                            remapped[c.Name].LoadInterfaces(c);
                        }
                    }
                }
            }
        }

        internal void EmitRemappedTypes2ndPass()
        {
            if (map != null && map.Assembly != null && map.Assembly.Classes != null)
            {
                // 2nd pass, resolve interfaces, publish methods/fields
                foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                {
                    if (c.Shadows != null)
                    {
                        RemapperTypeWrapper typeWrapper = remapped[c.Name];
                        typeWrapper.Process2ndPassStep1();
                    }
                }
                foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                {
                    if (c.Shadows != null)
                    {
                        RemapperTypeWrapper typeWrapper = remapped[c.Name];
                        typeWrapper.Process2ndPassStep2(map);
                    }
                }
            }
        }

        internal bool IsMapUnsafeException(RuntimeJavaType tw)
        {
            if (mappedExceptions != null)
                for (int i = 0; i < mappedExceptions.Length; i++)
                    if (mappedExceptions[i].IsSubTypeOf(tw) || (mappedExceptionsAllSubClasses[i] && tw.IsSubTypeOf(mappedExceptions[i])))
                        return true;

            return false;
        }

        internal void LoadMappedExceptions(MapXml.Root map)
        {
            if (map.ExceptionMappings.Length > 0)
            {
                mappedExceptionsAllSubClasses = new bool[map.ExceptionMappings.Length];
                mappedExceptions = new RuntimeJavaType[map.ExceptionMappings.Length];
                for (int i = 0; i < mappedExceptions.Length; i++)
                {
                    var dst = map.ExceptionMappings[i].Destination;
                    if (dst[0] == '*')
                    {
                        mappedExceptionsAllSubClasses[i] = true;
                        dst = dst.Substring(1);
                    }

                    mappedExceptions[i] = LoadClassByName(dst);
                }

                // HACK we need to find the <exceptionMapping /> element and bind it
                foreach (var c in map.Assembly.Classes)
                    foreach (var m in c.Methods)
                        if (m.Body != null)
                            foreach (var instr in m.Body.Instructions)
                                if (instr is MapXml.EmitExceptionMapping eem)
                                    eem.mapping = map.ExceptionMappings;
            }
        }

        internal sealed class ExceptionMapEmitter
        {

            readonly RuntimeContext rcontext;
            readonly MapXml.ExceptionMapping[] map;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="map"></param>
            internal ExceptionMapEmitter(RuntimeContext context, MapXml.ExceptionMapping[] map)
            {
                this.rcontext = context;
                this.map = map;
            }

            internal void Emit(MapXml.CodeGenContext context, CodeEmitter ilgen)
            {
                var mwSuppressFillInStackTrace = rcontext.JavaBase.TypeOfjavaLangThrowable.GetMethodWrapper("__<suppressFillInStackTrace>", "()V", false);
                mwSuppressFillInStackTrace.Link();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Callvirt, rcontext.CompilerFactory.GetTypeMethod);

                for (int i = 0; i < map.Length; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.Emit(OpCodes.Ldtoken, rcontext.Resolver.ResolveCoreType(map[i].Source));
                    ilgen.Emit(OpCodes.Call, rcontext.CompilerFactory.GetTypeFromHandleMethod);
                    ilgen.Emit(OpCodes.Ceq);
                    var label = ilgen.DefineLabel();
                    ilgen.EmitBrfalse(label);
                    ilgen.Emit(OpCodes.Pop);
                    if (map[i].Code != null)
                    {
                        ilgen.Emit(OpCodes.Ldarg_0);

                        if (map[i].Code.Instructions.Length > 0)
                        {
                            foreach (var instr in map[i].Code.Instructions)
                            {
                                var newobj = instr as MapXml.NewObj;
                                if (newobj != null && newobj.Class != null && context.ClassLoader.LoadClassByName(newobj.Class).IsSubTypeOf(rcontext.JavaBase.TypeOfjavaLangThrowable))
                                    mwSuppressFillInStackTrace.EmitCall(ilgen);

                                instr.Generate(context, ilgen);
                            }
                        }

                        ilgen.Emit(OpCodes.Ret);
                    }
                    else
                    {
                        var tw = context.ClassLoader.LoadClassByName(map[i].Destination);
                        var mw = tw.GetMethodWrapper("<init>", "()V", false);
                        mw.Link();
                        mwSuppressFillInStackTrace.EmitCall(ilgen);
                        mw.EmitNewobj(ilgen);
                        ilgen.Emit(OpCodes.Ret);
                    }

                    ilgen.MarkLabel(label);
                }

                ilgen.Emit(OpCodes.Pop);
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ret);
            }
        }

        internal void LoadMapXml()
        {
            if (map.Assembly.Classes.Length > 0)
            {
                mapxml_Classes = new Dictionary<string, MapXml.Class>();
                mapxml_MethodBodies = new Dictionary<MethodKey, MapXml.InstructionList>();
                mapxml_ReplacedMethods = new Dictionary<MethodKey, MapXml.ReplaceMethodCall[]>();
                mapxml_MethodPrologues = new Dictionary<MethodKey, MapXml.InstructionList>();

                foreach (var c in map.Assembly.Classes)
                {
                    // if it is not a remapped type, it must be a container for native, patched or augmented methods
                    if (c.Shadows == null)
                    {
                        string className = c.Name;
                        mapxml_Classes.Add(className, c);
                        AddMapXmlMethods(className, c.Constructors);
                        AddMapXmlMethods(className, c.Methods);
                        if (c.Clinit != null)
                            AddMapXmlMethod(className, c.Clinit);
                    }
                }
            }
        }

        private void AddMapXmlMethods(string className, MapXml.MethodBase[] methods)
        {
            if (methods != null)
                foreach (var method in methods)
                    AddMapXmlMethod(className, method);
        }

        private void AddMapXmlMethod(string className, MapXml.MethodBase method)
        {
            if (method.Body != null)
                mapxml_MethodBodies.Add(method.ToMethodKey(className), method.Body);

            if (method.ReplaceMethodCalls.Length > 0)
                mapxml_ReplacedMethods.Add(method.ToMethodKey(className), method.ReplaceMethodCalls);

            if (method.Prologue != null)
                mapxml_MethodPrologues.Add(method.ToMethodKey(className), method.Prologue);
        }

        internal MapXml.InstructionList GetMethodPrologue(MethodKey method)
        {
            if (mapxml_MethodPrologues == null)
                return null;

            mapxml_MethodPrologues.TryGetValue(method, out var prologue);
            return prologue;
        }

        internal MapXml.ReplaceMethodCall[] GetReplacedMethodsFor(RuntimeJavaMethod mw)
        {
            if (mapxml_ReplacedMethods == null)
                return null;

            mapxml_ReplacedMethods.TryGetValue(new MethodKey(mw.DeclaringType.Name, mw.Name, mw.Signature), out var rmc);
            return rmc;
        }

        internal Dictionary<string, MapXml.Class> GetMapXmlClasses()
        {
            return mapxml_Classes;
        }

        internal Dictionary<MethodKey, MapXml.InstructionList> GetMapXmlMethodBodies()
        {
            return mapxml_MethodBodies;
        }

        internal MapXml.Parameter[] GetXmlMapParameters(string classname, string method, string sig)
        {
            if (mapxml_Classes != null)
            {
                if (mapxml_Classes.TryGetValue(classname, out var clazz))
                {
                    if (method == "<init>" && clazz.Constructors.Length > 0)
                    {
                        for (int i = 0; i < clazz.Constructors.Length; i++)
                            if (clazz.Constructors[i].Sig == sig)
                                return clazz.Constructors[i].Parameters;
                    }
                    else if (clazz.Methods.Length > 0)
                    {
                        for (int i = 0; i < clazz.Methods.Length; i++)
                            if (clazz.Methods[i].Name == method && clazz.Methods[i].Sig == sig)
                                return clazz.Methods[i].Parameters;
                    }
                }
            }

            return null;
        }

        internal bool IsGhost(RuntimeJavaType tw)
        {
            return ghosts != null && tw.IsInterface && ghosts.ContainsKey(tw.Name);
        }

        void SetupGhosts(MapXml.Root map)
        {
            ghosts = new Dictionary<string, List<RuntimeJavaType>>();

            // find the ghost interfaces
            foreach (var c in map.Assembly.Classes)
            {
                if (c.Shadows != null && c.Interfaces.Length > 0)
                {
                    // NOTE we don't support interfaces that inherit from other interfaces
                    // (actually, if they are explicitly listed it would probably work)
                    var typeWrapper = FindLoadedClass(c.Name);

                    foreach (var iface in c.Interfaces)
                    {
                        var ifaceWrapper = FindLoadedClass(iface.Class);
                        if (ifaceWrapper == null || !ifaceWrapper.TypeAsTBD.IsAssignableFrom(typeWrapper.TypeAsTBD))
                            AddGhost(iface.Class, typeWrapper);
                    }
                }
            }

            // we manually add the array ghost interfaces
            var array = Context.ClassLoaderFactory.GetJavaTypeFromType(Context.Types.Array);
            AddGhost("java.io.Serializable", array);
            AddGhost("java.lang.Cloneable", array);
        }

        private void AddGhost(string interfaceName, RuntimeJavaType implementer)
        {
            if (!ghosts.TryGetValue(interfaceName, out var list))
            {
                list = new List<RuntimeJavaType>();
                ghosts[interfaceName] = list;
            }

            list.Add(implementer);
        }

        internal RuntimeJavaType[] GetGhostImplementers(RuntimeJavaType wrapper)
        {
            return ghosts.TryGetValue(wrapper.Name, out var list) ? list.ToArray() : Array.Empty<RuntimeJavaType>();
        }

        internal void FinishRemappedTypes()
        {
            // 3rd pass, link the methods. Note that a side effect of the linking is the
            // twiddling with the overriders array in the base methods, so we need to do this
            // as a separate pass before we compile the methods
            foreach (var typeWrapper in remapped.Values)
                typeWrapper.Process3rdPass();

            // 4th pass, implement methods/fields and bake the type
            foreach (var typeWrapper in remapped.Values)
                typeWrapper.Process4thPass(remapped.Values);

            if (assemblyAttributes != null)
                foreach (MapXml.Attribute attr in assemblyAttributes)
                    Context.AttributeHelper.SetCustomAttribute(this, assemblyBuilder, attr);
        }

        private static bool IsSigned(Assembly asm)
        {
            byte[] key = asm.GetName().GetPublicKey();
            return key != null && key.Length != 0;
        }

        internal static int Compile(RuntimeContext context, StaticCompiler compiler, string runtimeAssembly, List<CompilerOptions> optionsList)
        {
            try
            {
                compiler.runtimeAssembly = compiler.LoadFile(runtimeAssembly ?? Path.Combine(Path.GetDirectoryName(typeof(CompilerClassLoader).Assembly.Location), "IKVM.Runtime.dll"));
            }
            catch (FileNotFoundException)
            {
                // runtime assembly is required
                if (compiler.runtimeAssembly == null)
                    throw new FatalCompilerErrorException(Message.RuntimeNotFound);

                // some unknown error
                throw new FatalCompilerErrorException(Message.FileNotFound);
            }

            Tracer.Info(Tracer.Compiler, "Loaded runtime assembly: {0}", compiler.runtimeAssembly.FullName);
            List<CompilerClassLoader> loaders = new List<CompilerClassLoader>();
            foreach (CompilerOptions options in optionsList)
            {
                CompilerClassLoader loader = null;
                int rc = CreateCompiler(context, compiler, options, out loader);
                if (rc != 0)
                {
                    return rc;
                }

                loaders.Add(loader);
                options.sharedclassloader?.Add(loader);
            }

            foreach (var loader1 in loaders)
                foreach (var loader2 in loaders)
                    if (loader1 != loader2 && (loader1.options.crossReferenceAllPeers || (loader1.options.peerReferences != null && Array.IndexOf(loader1.options.peerReferences, loader2.options.assembly) != -1)))
                        loader1.AddReference(loader2);

            foreach (var loader in loaders)
                loader.CompilePass0();

            var mainAssemblyTypes = new Dictionary<CompilerClassLoader, Type>();
            foreach (var loader in loaders)
            {
                if (loader.options.sharedclassloader != null)
                {
                    Type mainAssemblyType;
                    if (!mainAssemblyTypes.TryGetValue(loader.options.sharedclassloader[0], out mainAssemblyType))
                    {
                        var tb = loader.options.sharedclassloader[0].GetTypeWrapperFactory().ModuleBuilder.DefineType("__<MainAssembly>", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.SpecialName);
                        loader.Context.AttributeHelper.HideFromJava(tb);
                        mainAssemblyType = tb.CreateType();
                        mainAssemblyTypes.Add(loader.options.sharedclassloader[0], mainAssemblyType);
                    }
                    if (loader.options.sharedclassloader[0] != loader)
                    {
                        ((AssemblyBuilder)loader.GetTypeWrapperFactory().ModuleBuilder.Assembly).__AddTypeForwarder(mainAssemblyType);
                    }
                }
                loader.CompilePass1();
            }

            foreach (CompilerClassLoader loader in loaders)
            {
                loader.CompilePass2();
            }

            if (context.Bootstrap)
                foreach (CompilerClassLoader loader in loaders)
                    loader.EmitRemappedTypes2ndPass();

            foreach (CompilerClassLoader loader in loaders)
            {
                int rc = loader.CompilePass3();
                if (rc != 0)
                {
                    return rc;
                }
            }

            Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
            foreach (CompilerClassLoader loader in loaders)
            {
                loader.PrepareSave();
            }
            if (compiler.errorCount > 0)
            {
                return 1;
            }
            foreach (CompilerClassLoader loader in loaders)
            {
                loader.Save();
            }
            return compiler.errorCount == 0 ? 0 : 1;
        }

        static int CreateCompiler(RuntimeContext context, StaticCompiler compiler, CompilerOptions options, out CompilerClassLoader loader)
        {
            Tracer.Info(Tracer.Compiler, "JVM.Compile path: {0}, assembly: {1}", options.path, options.assembly);
            AssemblyName runtimeAssemblyName = compiler.runtimeAssembly.GetName();
            bool allReferencesAreStrongNamed = IsSigned(compiler.runtimeAssembly);
            List<Assembly> references = new List<Assembly>();
            foreach (Assembly reference in options.references ?? new Assembly[0])
            {
                references.Add(reference);
                allReferencesAreStrongNamed &= IsSigned(reference);
                Tracer.Info(Tracer.Compiler, "Loaded reference assembly: {0}", reference.FullName);
                // if it's an IKVM compiled assembly, make sure that it was compiled
                // against same version of the runtime
                foreach (AssemblyName asmref in reference.GetReferencedAssemblies())
                {
                    if (asmref.Name == runtimeAssemblyName.Name)
                    {
                        if (IsSigned(compiler.runtimeAssembly))
                        {
                            // TODO we really should support binding redirects here to allow different revisions to be mixed
                            if (asmref.FullName != runtimeAssemblyName.FullName)
                            {
                                throw new FatalCompilerErrorException(Message.RuntimeMismatch, reference.Location, runtimeAssemblyName.FullName, asmref.FullName);
                            }
                        }
                        else
                        {
                            if (asmref.GetPublicKeyToken() != null && asmref.GetPublicKeyToken().Length != 0)
                            {
                                throw new FatalCompilerErrorException(Message.RuntimeMismatch, reference.Location, runtimeAssemblyName.FullName, asmref.FullName);
                            }
                        }
                    }
                }
            }
            Tracer.Info(Tracer.Compiler, "Parsing class files");
            // map the class names to jar entries
            Dictionary<string, Jar.Item> h = new Dictionary<string, Jar.Item>();
            List<string> classNames = new List<string>();
            foreach (Jar jar in options.jars)
            {
                if (options.IsResourcesJar(jar))
                {
                    continue;
                }
                foreach (Jar.Item item in jar)
                {
                    string name = item.Name;
                    if (name.EndsWith(".class", StringComparison.Ordinal)
                        && name.Length > 6
                        && name.IndexOf('.') == name.Length - 6)
                    {
                        string className = name.Substring(0, name.Length - 6).Replace('/', '.');
                        if (h.ContainsKey(className))
                        {
                            compiler.IssueMessage(Message.DuplicateClassName, className);
                            Jar.Item itemRef = h[className];
                            if ((options.classesJar != -1 && itemRef.Jar == options.jars[options.classesJar]) || jar != itemRef.Jar)
                            {
                                // the previous class stays, because it was either in an earlier jar or we're processing the classes.jar
                                // which contains the classes loaded from the file system (where the first encountered class wins)
                                continue;
                            }
                            else
                            {
                                // we have a jar that contains multiple entries with the same name, the last one wins
                                h.Remove(className);
                                classNames.Remove(className);
                            }
                        }
                        h.Add(className, item);
                        classNames.Add(className);
                    }
                }
            }

            if (options.assemblyAttributeAnnotations == null)
            {
                // look for "assembly" type that acts as a placeholder for assembly attributes
                Jar.Item assemblyType;
                if (h.TryGetValue("assembly", out assemblyType))
                {
                    try
                    {
                        var f = new ClassFile(context, ClassReader.Read(assemblyType.GetData()), null, ClassFileParseOptions.None, null);

                        // NOTE the "assembly" type in the unnamed package is a magic type
                        // that acts as the placeholder for assembly attributes
                        if (f.Name == "assembly" && f.Annotations != null)
                        {
                            options.assemblyAttributeAnnotations = f.Annotations;
                            // HACK remove "assembly" type that exists only as a placeholder for assembly attributes
                            h.Remove(f.Name);
                            assemblyType.Remove();
                            compiler.IssueMessage(Message.LegacyAssemblyAttributesFound);
                        }
                    }
                    catch (ClassFormatError)
                    {

                    }
                }
            }

            // now look for a main method
            if (options.mainClass == null && (options.guessFileKind || options.target != PEFileKinds.Dll))
            {
                foreach (string className in classNames)
                {
                    try
                    {
                        var f = new ClassFile(context, ClassReader.Read(h[className].GetData()), null, ClassFileParseOptions.None, null);
                        if (f.Name == className)
                        {
                            foreach (var m in f.Methods)
                            {
                                if (m.IsPublic && m.IsStatic && m.Name == "main" && m.Signature == "([Ljava.lang.String;)V")
                                {
                                    compiler.IssueMessage(Message.MainMethodFound, f.Name);
                                    options.mainClass = f.Name;
                                    goto break_outer;
                                }
                            }
                        }
                    }
                    catch (ClassFormatError)
                    {
                    }
                }
            break_outer:;
            }

            if (options.guessFileKind && options.mainClass == null)
            {
                options.target = PEFileKinds.Dll;
            }

            if (options.target != PEFileKinds.Dll && options.mainClass == null)
            {
                throw new FatalCompilerErrorException(Message.ExeRequiresMainClass);
            }

            if (options.target == PEFileKinds.Dll && options.props.Count != 0)
            {
                throw new FatalCompilerErrorException(Message.PropertiesRequireExe);
            }

            if (options.path == null)
            {
                if (options.target == PEFileKinds.Dll)
                {
                    if (options.targetIsModule)
                    {
                        options.path = IkvmImporterInternal.GetFileInfo(options.assembly + ".netmodule");
                    }
                    else
                    {
                        options.path = IkvmImporterInternal.GetFileInfo(options.assembly + ".dll");
                    }
                }
                else
                {
                    options.path = IkvmImporterInternal.GetFileInfo(options.assembly + ".exe");
                }
                compiler.IssueMessage(Message.OutputFileIs, options.path.ToString());
            }

            if (options.targetIsModule)
            {
                if (options.classLoader != null)
                {
                    throw new FatalCompilerErrorException(Message.ModuleCannotHaveClassLoader);
                }
                // TODO if we're overwriting a user specified assembly name, we need to emit a warning
                options.assembly = options.path.Name;
            }

            Tracer.Info(Tracer.Compiler, "Constructing compiler");
            var referencedAssemblies = new List<RuntimeAssemblyClassLoader>(references.Count);
            for (int i = 0; i < references.Count; i++)
            {
                // if reference is to base assembly, set it explicitly for resolution
                if (compiler.baseAssembly == null && options.bootstrap == false && IsBaseAssembly(context, references[i]))
                    compiler.baseAssembly = references[i];

                var acl = context.AssemblyClassLoaderFactory.FromAssembly(references[i]);
                if (referencedAssemblies.Contains(acl))
                    compiler.IssueMessage(options, Message.DuplicateAssemblyReference, acl.MainAssembly.FullName);

                referencedAssemblies.Add(acl);
            }

            loader = new CompilerClassLoader(context, referencedAssemblies.ToArray(), options, options.path, options.targetIsModule, options.assembly, h);
            loader.classesToCompile = new List<string>(h.Keys);
            if (options.remapfile != null)
            {
                Tracer.Info(Tracer.Compiler, "Loading remapped types (1) from {0}", options.remapfile);

                FileStream fs;
                try
                {
                    // NOTE: Using FileShare.ReadWrite ensures other FileStreams (from other processes) can be opened
                    // simultaneously on this file while we are reading it.
                    fs = new FileStream(options.remapfile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    throw new FatalCompilerErrorException(Message.ErrorReadingFile, options.remapfile, e.Message);
                }

                try
                {
                    MapXml.Root map;

                    try
                    {
                        map = new MapXml.MapXmlSerializer().Read(XDocument.Load(fs, LoadOptions.SetLineInfo));
                    }
                    catch (MapXml.MapXmlException x)
                    {
                        throw new FatalCompilerErrorException(Message.ErrorParsingMapFile, options.remapfile, x.Message);
                    }

                    if (loader.ValidateAndSetMap(map) == false)
                        return 1;
                }
                finally
                {
                    fs.Close();
                }

                if (options.bootstrap)
                    context.ClassLoaderFactory.SetBootstrapClassLoader(loader);
            }

            // If we do not yet have a reference to the base assembly and we are not compiling the base assembly,
            // try to find the base assembly by looking at the assemblies that the runtime references
            if (compiler.baseAssembly == null && options.bootstrap == false)
            {
                foreach (var name in compiler.runtimeAssembly.GetReferencedAssemblies())
                {
                    Assembly asm = null;

                    try
                    {
                        var path = Path.Combine(Path.GetDirectoryName(compiler.runtimeAssembly.Location), name.Name + ".dll");
                        if (File.Exists(path))
                            asm = LoadReferencedAssembly(compiler, path);
                    }
                    catch (FileNotFoundException)
                    {

                    }

                    if (asm != null && IsBaseAssembly(context, asm))
                    {
                        RuntimeAssemblyClassLoader.PreloadExportedAssemblies(context.StaticCompiler, asm);
                        compiler.baseAssembly = asm;
                        break;
                    }
                }

                if (compiler.baseAssembly == null)
                {
                    throw new FatalCompilerErrorException(Message.BootstrapClassesMissing);
                }

                // we need to scan again for remapped types, now that we've loaded the core library
                context.ClassLoaderFactory.LoadRemappedTypes();
            }

            if (options.bootstrap == false)
            {
                allReferencesAreStrongNamed &= IsSigned(context.Resolver.ResolveBaseAssembly());
                loader.AddReference(context.AssemblyClassLoaderFactory.FromAssembly(context.Resolver.ResolveBaseAssembly()));
            }

            if ((options.keyPair != null || options.publicKey != null) && !allReferencesAreStrongNamed)
            {
                throw new FatalCompilerErrorException(Message.StrongNameRequiresStrongNamedRefs);
            }

            if (loader.map != null)
            {
                loader.LoadMapXml();
            }

            if (options.bootstrap == false)
            {
                loader.fakeTypes = context.FakeTypes;
                loader.fakeTypes.Load(context.Resolver.ResolveBaseAssembly());
            }

            return 0;
        }

        static bool IsBaseAssembly(RuntimeContext context, Assembly asm)
        {
            return asm.IsDefined(context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.RemappedClassAttribute).FullName), false);
        }

        private static Assembly LoadReferencedAssembly(StaticCompiler compiler, string r)
        {
            Assembly asm = compiler.LoadFile(r);
            return asm;
        }

        private void CompilePass0()
        {
            if (options.sharedclassloader != null && options.sharedclassloader[0] != this)
            {
                packages = options.sharedclassloader[0].packages;
            }
            else
            {
                packages = new Packages();
            }
        }

        void CompilePass1()
        {
            Tracer.Info(Tracer.Compiler, "Compiling class files (1)");
            if (options.bootstrap)
                EmitRemappedTypes();

            // if we're compiling the core class library, generate the "fake" generic types
            // that represent the not-really existing types (i.e. the Java enums that represent .NET enums,
            // the Method interface for delegates and the Annotation annotation for custom attributes)
            if (options.bootstrap)
            {
                fakeTypes = Context.FakeTypes;
                fakeTypes.Create(GetTypeWrapperFactory().ModuleBuilder, this);
            }

            allwrappers = new List<RuntimeJavaType>();
            foreach (var s in classesToCompile)
            {
                var wrapper = TryLoadClassByName(s);
                if (wrapper != null)
                {
                    var loader = wrapper.GetClassLoader();
                    if (loader != this)
                    {
                        if (loader is RuntimeAssemblyClassLoader)
                            Context.StaticCompiler.IssueMessage(options, Message.SkippingReferencedClass, s, ((RuntimeAssemblyClassLoader)loader).GetAssembly(wrapper).FullName);

                        continue;
                    }

                    if (options.sharedclassloader != null && options.sharedclassloader[0] != this)
                        options.sharedclassloader[0].dynamicallyImportedTypes.Add(wrapper);

                    allwrappers.Add(wrapper);
                }
            }
        }

        private void CompilePass2()
        {
            Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
            foreach (RuntimeJavaType tw in allwrappers)
            {
                RuntimeByteCodeJavaType dtw = tw as RuntimeByteCodeJavaType;
                if (dtw != null)
                {
                    dtw.CreateStep2();
                }
            }
        }

        private int CompilePass3()
        {
            Tracer.Info(Tracer.Compiler, "Compiling class files (3)");
            if (map != null && options.bootstrap)
            {
                fakeTypes.Finish(this);
            }
            foreach (string proxy in options.proxies)
            {
                Context.ProxyGenerator.Create(this, proxy);
            }

            if (options.mainClass != null)
            {
                RuntimeJavaType wrapper = null;

                try
                {
                    wrapper = TryLoadClassByName(options.mainClass);
                }
                catch (RetargetableJavaException)
                {
                }
                if (wrapper == null)
                {
                    throw new FatalCompilerErrorException(Message.MainClassNotFound);
                }

                var mw = wrapper.GetMethodWrapper("main", "([Ljava.lang.String;)V", false);
                if (mw == null || !mw.IsStatic)
                    throw new FatalCompilerErrorException(Message.MainMethodNotFound);

                mw.Link();

                var method = mw.GetMethod() as MethodInfo;
                if (method == null)
                    throw new FatalCompilerErrorException(Message.UnsupportedMainMethod);

                if (!ReflectUtil.IsFromAssembly(method.DeclaringType, assemblyBuilder) && (!method.IsPublic || !method.DeclaringType.IsPublic))
                {
                    throw new FatalCompilerErrorException(Message.ExternalMainNotAccessible);
                }

                var apartmentAttributeType = options.apartment switch
                {
                    ApartmentState.STA => Context.Resolver.ResolveCoreType(typeof(STAThreadAttribute).FullName),
                    ApartmentState.MTA => Context.Resolver.ResolveCoreType(typeof(MTAThreadAttribute).FullName),
                    _ => throw new NotImplementedException(),
                };

                SetMain(wrapper, options.target, options.props, options.noglobbing, apartmentAttributeType);
            }

            if (map != null)
            {
                LoadMappedExceptions(map);
                Tracer.Info(Tracer.Compiler, "Loading remapped types (2)");
                try
                {
                    FinishRemappedTypes();
                }
                catch (IKVM.Reflection.MissingMemberException x)
                {
                    Context.StaticCompiler.IssueMissingTypeMessage((Type)x.MemberInfo);
                    return 1;
                }
            }
            Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
            WriteResources();

            if (options.externalResources != null)
                foreach (KeyValuePair<string, string> kv in options.externalResources)
                    assemblyBuilder.AddResourceFile(JVM.MangleResourceName(kv.Key), kv.Value);

            if (options.fileversion != null)
            {
                CustomAttributeBuilder filever = new CustomAttributeBuilder(Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyFileVersionAttribute).FullName).GetConstructor(new Type[] { Context.Types.String }), new object[] { options.fileversion });
                assemblyBuilder.SetCustomAttribute(filever);
            }
            if (options.assemblyAttributeAnnotations != null)
            {
                foreach (object[] def in options.assemblyAttributeAnnotations)
                {
                    Annotation annotation = Annotation.LoadAssemblyCustomAttribute(this, def);
                    if (annotation != null)
                    {
                        annotation.Apply(this, assemblyBuilder, def);
                    }
                }
            }

            if (options.classLoader != null)
            {
                RuntimeJavaType classLoaderType = null;
                try
                {
                    classLoaderType = TryLoadClassByName(options.classLoader);
                }
                catch (RetargetableJavaException)
                {

                }

                if (classLoaderType == null)
                    throw new FatalCompilerErrorException(Message.ClassLoaderNotFound);

                if (classLoaderType.IsPublic == false && ReflectUtil.IsFromAssembly(classLoaderType.TypeAsBaseType, assemblyBuilder) == false)
                    throw new FatalCompilerErrorException(Message.ClassLoaderNotAccessible);

                if (classLoaderType.IsAbstract)
                    throw new FatalCompilerErrorException(Message.ClassLoaderIsAbstract);

                if (classLoaderType.IsAssignableTo(Context.ClassLoaderFactory.LoadClassCritical("java.lang.ClassLoader")) == false)
                    throw new FatalCompilerErrorException(Message.ClassLoaderNotClassLoader);

                var classLoaderInitMethod = classLoaderType.GetMethodWrapper("<init>", "(Lcli.System.Reflection.Assembly;)V", false);
                if (classLoaderInitMethod == null)
                    throw new FatalCompilerErrorException(Message.ClassLoaderConstructorMissing);

                // apply custom attribute specifying custom class loader
                var ci = Context.Resolver.ResolveRuntimeType(typeof(CustomAssemblyClassLoaderAttribute).FullName).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { Context.Types.Type }, null);
                assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(ci, new object[] { classLoaderType.TypeAsTBD }));

                // the class loader type defines a module initialize method, ensure we call it upon module load
                var mwModuleInit = classLoaderType.GetMethodWrapper("InitializeModule", "(Lcli.System.Reflection.Module;)V", false);
                if (mwModuleInit != null && mwModuleInit.IsStatic == false)
                {
                    var moduleInit = GetTypeWrapperFactory().ModuleBuilder.DefineGlobalMethod(".cctor", MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, null, Type.EmptyTypes);
                    var moduleInitIL = moduleInit.GetILGenerator();
                    moduleInitIL.Emit(OpCodes.Ldtoken, moduleInit);
                    moduleInitIL.Emit(OpCodes.Call, Context.Resolver.ResolveCoreType(typeof(System.Reflection.MethodBase).FullName).GetMethod("GetMethodFromHandle", new[] { Context.Resolver.ResolveCoreType(typeof(RuntimeMethodHandle).FullName) }));
                    moduleInitIL.Emit(OpCodes.Callvirt, Context.Resolver.ResolveCoreType(typeof(System.Reflection.MemberInfo).FullName).GetProperty("Module").GetGetMethod());
                    moduleInitIL.Emit(OpCodes.Call, Context.Resolver.ResolveRuntimeType("IKVM.Runtime.ByteCodeHelper").GetMethod("InitializeModule"));
                    moduleInitIL.Emit(OpCodes.Ret);
                }
            }

            if (options.iconfile != null)
            {
                assemblyBuilder.__DefineIconResource(IkvmImporterInternal.ReadAllBytes(options.iconfile));
            }

            if (options.manifestFile != null)
            {
                assemblyBuilder.__DefineManifestResource(IkvmImporterInternal.ReadAllBytes(options.manifestFile));
            }

            assemblyBuilder.DefineVersionInfoResource();

            return 0;
        }

        private bool ValidateAndSetMap(IKVM.Tools.Importer.MapXml.Root map)
        {
            bool valid = true;
            if (map.Assembly != null)
            {
                if (map.Assembly.Classes != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Class c in map.Assembly.Classes)
                    {
                        if (c.Fields != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Field f in c.Fields)
                            {
                                ValidateNameSig("field", c.Name, f.Name, f.Sig, ref valid, true);
                            }
                        }
                        if (c.Methods != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Method m in c.Methods)
                            {
                                ValidateNameSig("method", c.Name, m.Name, m.Sig, ref valid, false);
                            }
                        }
                        if (c.Constructors != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Constructor ctor in c.Constructors)
                            {
                                ValidateNameSig("constructor", c.Name, "<init>", ctor.Sig, ref valid, false);
                            }
                        }
                        if (c.Properties != null)
                        {
                            foreach (IKVM.Tools.Importer.MapXml.Property prop in c.Properties)
                            {
                                ValidateNameSig("property", c.Name, prop.Name, prop.Sig, ref valid, false);
                                ValidatePropertyGetterSetter("getter", c.Name, prop.Name, prop.Getter, ref valid);
                                ValidatePropertyGetterSetter("setter", c.Name, prop.Name, prop.Setter, ref valid);
                            }
                        }
                    }
                }
            }
            this.map = map;
            return valid;
        }

        private void ValidateNameSig(string member, string clazz, string name, string sig, ref bool valid, bool field)
        {
            if (!IsValidName(name))
            {
                valid = false;
                Context.StaticCompiler.IssueMessage(Message.InvalidMemberNameInMapFile, member, name, clazz);
            }
            if (!IsValidSig(sig, field))
            {
                valid = false;
                Context.StaticCompiler.IssueMessage(Message.InvalidMemberSignatureInMapFile, member, clazz, name, sig);
            }
        }

        private void ValidatePropertyGetterSetter(string getterOrSetter, string clazz, string property, IKVM.Tools.Importer.MapXml.Method method, ref bool valid)
        {
            if (method != null)
            {
                if (!IsValidName(method.Name))
                {
                    valid = false;
                    Context.StaticCompiler.IssueMessage(Message.InvalidPropertyNameInMapFile, getterOrSetter, clazz, property, method.Name);
                }
                if (!ClassFile.IsValidMethodSig(method.Sig))
                {
                    valid = false;
                    Context.StaticCompiler.IssueMessage(Message.InvalidPropertySignatureInMapFile, getterOrSetter, clazz, property, method.Sig);
                }
            }
        }

        private static bool IsValidName(string name)
        {
            return name != null && name.Length != 0;
        }

        private static bool IsValidSig(string sig, bool field)
        {
            return sig != null && (field ? ClassFile.IsValidFieldSig(sig) : ClassFile.IsValidMethodSig(sig));
        }

        internal Type GetTypeFromReferencedAssembly(string name)
        {
            foreach (RuntimeAssemblyClassLoader acl in referencedAssemblies)
            {
                Type type = acl.MainAssembly.GetType(name, false);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        internal override void IssueMessage(Message msgId, params string[] values)
        {
            Context.StaticCompiler.IssueMessage(options, msgId, values);
        }

        internal override bool WarningLevelHigh
        {
            get { return options.warningLevelHigh; }
        }

        internal override bool NoParameterReflection
        {
            get { return options.noParameterReflection; }
        }

        protected override void CheckProhibitedPackage(string className)
        {
            if (!options.bootstrap)
            {
                base.CheckProhibitedPackage(className);
            }
        }
    }

    sealed class Jar
    {
        internal readonly string Name;
        private readonly List<JarItem> Items = new List<JarItem>();

        internal Jar(string name)
        {
            this.Name = name;
        }

        internal Jar Copy()
        {
            Jar newJar = new Jar(Name);
            newJar.Items.AddRange(Items);
            return newJar;
        }

        internal void Add(string name, byte[] data, FileInfo fileInfo = null)
        {
            Items.Add(new JarItem(name, data, fileInfo));
        }

        private readonly struct JarItem
        {
            internal readonly string name;
            internal readonly byte[] data;
            internal readonly FileInfo path;            // path of the original file, if it was individual file (used to construct source file path)

            internal JarItem(string name, byte[] data, FileInfo path)
            {
                this.name = name;
                this.data = data;
                this.path = path;
            }
        }

        public struct Item
        {
            internal readonly Jar Jar;
            private readonly int Index;

            internal Item(Jar jar, int index)
            {
                this.Jar = jar;
                this.Index = index;
            }

            internal string Name
            {
                get { return Jar.Items[Index].name; }
            }

            internal byte[] GetData()
            {
                return Jar.Items[Index].data;
            }

            internal FileInfo Path
            {
                get { return Jar.Items[Index].path; }
            }

            internal void Remove()
            {
                Jar.Items[Index] = new JarItem();
            }

            internal void MarkAsStub()
            {
                Jar.Items[Index] = new JarItem(Jar.Items[Index].name, null, null);
            }

            internal bool IsStub
            {
                get { return Jar.Items[Index].data == null; }
            }
        }

        internal struct JarEnumerator
        {
            private readonly Jar jar;
            private int index;

            internal JarEnumerator(Jar jar)
            {
                this.jar = jar;
                this.index = -1;
            }

            public Item Current
            {
                get { return new Item(jar, index); }
            }

            public bool MoveNext()
            {
                if (index + 1 < jar.Items.Count)
                {
                    index++;
                    return true;
                }
                return false;
            }
        }

        public JarEnumerator GetEnumerator()
        {
            return new JarEnumerator(this);
        }
    }

    sealed class CompilerOptions
    {
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
        internal PEFileKinds target;
        internal bool guessFileKind;
        internal string[] unresolvedReferences; // only used during command line parsing
        internal Dictionary<string, string> legacyStubReferences = new Dictionary<string, string>();    // only used during command line parsing
        internal Assembly[] references;
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
        internal PortableExecutableKinds pekind = PortableExecutableKinds.ILOnly;
        internal ImageFileMachine imageFileMachine = ImageFileMachine.I386;
        internal ulong baseAddress;
        internal uint fileAlignment;
        internal bool highentropyva;
        internal List<CompilerClassLoader> sharedclassloader; // should *not* be deep copied in Copy(), because we want the list of all compilers that share a class loader
        internal Dictionary<string, string> suppressWarnings = new Dictionary<string, string>();
        internal Dictionary<string, string> errorWarnings = new Dictionary<string, string>();   // treat specific warnings as errors
        internal bool warnaserror; // treat all warnings as errors
        internal FileInfo writeSuppressWarningsFile;
        internal List<string> proxies = new List<string>();
        internal object[] assemblyAttributeAnnotations;
        internal bool warningLevelHigh;
        internal bool noParameterReflection;
        internal bool bootstrap;

        internal CompilerOptions Copy()
        {
            CompilerOptions copy = (CompilerOptions)MemberwiseClone();
            copy.jars = Copy(jars);
            copy.jarMap = new Dictionary<string, int>(jarMap);
            if (props != null)
            {
                copy.props = new Dictionary<string, string>(props);
            }
            if (externalResources != null)
            {
                copy.externalResources = new Dictionary<string, string>(externalResources);
            }
            copy.suppressWarnings = new Dictionary<string, string>(suppressWarnings);
            copy.errorWarnings = new Dictionary<string, string>(errorWarnings);
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
            int existingJar;
            if (jarMap.TryGetValue(file, out existingJar))
            {
                return jars[existingJar];
            }
            jarMap.Add(file, jars.Count);
            return CreateJar(Path.GetFileName(file));
        }

        private Jar CreateJar(string jarName)
        {
            int count = 0;
            string name = jarName;
        retry:
            foreach (Jar jar in jars)
            {
                if (jar.Name == name)
                {
                    name = Path.GetFileNameWithoutExtension(jarName) + "-" + (++count) + Path.GetExtension(jarName);
                    goto retry;
                }
            }
            Jar newJar = new Jar(name);
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
            {
                for (int i = 0; i < classesToExclude.Length; i++)
                {
                    if (Regex.IsMatch(className, classesToExclude[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class ManagedResolver : IManagedTypeResolver
    {

        readonly StaticCompiler compiler;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        public ManagedResolver(StaticCompiler compiler)
        {
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        }

        public Assembly ResolveBaseAssembly()
        {
            return compiler.baseAssembly;
        }

        public Assembly ResolveAssembly(string assemblyName)
        {
            return compiler.Universe.Load(assemblyName);
        }

        public Type ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return t;

            return null;
        }

        public Type ResolveRuntimeType(string typeName)
        {
            return compiler.GetRuntimeType(typeName);
        }

    }

}
