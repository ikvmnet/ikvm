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
using System.Runtime.Serialization;

using IKVM.CoreLib.Diagnostics;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;

using ProtectionDomain = java.security.ProtectionDomain;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides access to dynamically emitted Java types.
    /// </summary>
    internal sealed class DynamicClassLoader : RuntimeJavaTypeFactory
    {

#if !IMPORTER
        static AssemblyBuilder jniProxyAssemblyBuilder;
#endif

        readonly RuntimeContext context;
        readonly IDiagnosticHandler diagnostics;
        readonly ModuleBuilder moduleBuilder;
        readonly bool hasInternalAccess;

#if IMPORTER
        TypeBuilder proxiesContainer;
        List<TypeBuilder> proxies;
#endif

        Dictionary<string, TypeBuilder> unloadables;
        TypeBuilder unloadableContainer;
        Type[] delegates;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="diagnostics"></param>
        /// <param name="moduleBuilder"></param>
        /// <param name="hasInternalAccess"></param>
        internal DynamicClassLoader(RuntimeContext context, IDiagnosticHandler diagnostics, ModuleBuilder moduleBuilder, bool hasInternalAccess)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.moduleBuilder = moduleBuilder;
            this.hasInternalAccess = hasInternalAccess;
        }

        /// <summary>
        /// Attempts to preallocate a mangled type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal override bool ReserveName(string name)
        {
            return context.DynamicClassLoaderFactory.ReserveName(name);
        }

        /// <summary>
        /// Allocates a mangled name associated with the given Java type.
        /// </summary>
        /// <param name="javaType"></param>
        /// <returns></returns>
        internal override string AllocMangledName(RuntimeByteCodeJavaType javaType)
        {
            return DynamicClassLoaderFactory.TypeNameMangleImpl(context.DynamicClassLoaderFactory.dynamicTypes, javaType.Name, javaType);
        }

        internal sealed override RuntimeJavaType DefineClassImpl(Dictionary<string, RuntimeJavaType> types, RuntimeJavaType host, ClassFile f, RuntimeClassLoader classLoader, ProtectionDomain protectionDomain)
        {
#if IMPORTER
            var type = new RuntimeImportByteCodeJavaType(f, (ImportClassLoader)classLoader);
            type.CreateStep1();
            types[f.Name] = type;
            return type;
#elif FIRST_PASS
            return null;
#else
            // this step can throw a retargettable exception, if the class is incorrect
            var type = new RuntimeByteCodeJavaType(host, f, classLoader, protectionDomain);
            // This step actually creates the TypeBuilder. It is not allowed to throw any exceptions,
            // if an exception does occur, it is due to a programming error in the IKVM or CLR runtime
            // and will cause a CriticalFailure and exit the process.
            type.CreateStep1();
            type.CreateStep2();
            if (types == null)
            {
                // we're defining an anonymous class, so we don't need any locking
                TieClassAndWrapper(type, protectionDomain);
                return type;
            }
            lock (types)
            {
                // in very extreme conditions another thread may have beaten us to it
                // and loaded (not defined) a class with the same name, in that case
                // we'll leak the the Reflection.Emit defined type. Also see the comment
                // in ClassLoaderWrapper.RegisterInitiatingLoader().
                RuntimeJavaType race;
                types.TryGetValue(f.Name, out race);
                if (race == null)
                {
                    types[f.Name] = type;
                    TieClassAndWrapper(type, protectionDomain);
                }
                else
                {
                    throw new LinkageError("duplicate class definition: " + f.Name);
                }
            }

            return type;
#endif
        }

#if !IMPORTER && !FIRST_PASS

        private static java.lang.Class TieClassAndWrapper(RuntimeJavaType type, ProtectionDomain protectionDomain)
        {
            java.lang.Class clazz = new java.lang.Class(null);
            clazz.typeWrapper = type;
            clazz.pd = protectionDomain;
            type.SetClassObject(clazz);
            return clazz;
        }

#endif

#if IMPORTER

        internal TypeBuilder DefineProxy(string name, TypeAttributes typeAttributes, Type parent, Type[] interfaces)
        {
            if (proxiesContainer == null)
            {
                proxiesContainer = moduleBuilder.DefineType(TypeNameUtil.ProxiesContainer, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract);
                context.AttributeHelper.HideFromJava(proxiesContainer);
                context.AttributeHelper.SetEditorBrowsableNever(proxiesContainer);
                proxies = new List<TypeBuilder>();
            }
            TypeBuilder tb = proxiesContainer.DefineNestedType(name, typeAttributes, parent, interfaces);
            proxies.Add(tb);
            return tb;
        }

#endif

        internal override Type DefineUnloadable(string name)
        {
            lock (this)
            {
                if (unloadables == null)
                {
                    unloadables = new Dictionary<string, TypeBuilder>();
                }
                TypeBuilder type;
                if (unloadables.TryGetValue(name, out type))
                {
                    return type;
                }
                if (unloadableContainer == null)
                {
                    unloadableContainer = moduleBuilder.DefineType(RuntimeUnloadableJavaType.ContainerTypeName, TypeAttributes.Interface | TypeAttributes.Abstract);
                    context.AttributeHelper.HideFromJava(unloadableContainer);
                }
                type = unloadableContainer.DefineNestedType(TypeNameUtil.MangleNestedTypeName(name), TypeAttributes.NestedPrivate | TypeAttributes.Interface | TypeAttributes.Abstract);
                unloadables.Add(name, type);
                return type;
            }
        }

        internal override Type DefineDelegate(int parameterCount, bool returnVoid)
        {
            lock (this)
            {
                if (delegates == null)
                {
                    delegates = new Type[512];
                }
                int index = parameterCount + (returnVoid ? 256 : 0);
                Type type = delegates[index];
                if (type != null)
                {
                    return type;
                }
                TypeBuilder tb = moduleBuilder.DefineType(returnVoid ? "__<>NVIV`" + parameterCount : "__<>NVI`" + (parameterCount + 1), TypeAttributes.NotPublic | TypeAttributes.Sealed, context.Types.MulticastDelegate);
                string[] names = new string[parameterCount + (returnVoid ? 0 : 1)];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = "P" + i;
                }
                if (!returnVoid)
                {
                    names[names.Length - 1] = "R";
                }
                Type[] genericParameters = tb.DefineGenericParameters(names);
                Type[] parameterTypes = genericParameters;
                if (!returnVoid)
                {
                    parameterTypes = new Type[genericParameters.Length - 1];
                    Array.Copy(genericParameters, parameterTypes, parameterTypes.Length);
                }
                tb.DefineMethod(ConstructorInfo.ConstructorName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, context.Types.Void, new Type[] { context.Types.Object, context.Types.IntPtr })
                    .SetImplementationFlags(MethodImplAttributes.Runtime);
                MethodBuilder mb = tb.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Virtual, returnVoid ? context.Types.Void : genericParameters[genericParameters.Length - 1], parameterTypes);
                mb.SetImplementationFlags(MethodImplAttributes.Runtime);
                type = tb.CreateType();
                delegates[index] = type;
                return type;
            }
        }

        internal override bool HasInternalAccess
        {
            get { return hasInternalAccess; }
        }

        internal void FinishAll()
        {
            var done = new Dictionary<RuntimeJavaType, RuntimeJavaType>();
            var more = true;

            while (more)
            {
                more = false;
                var l = context.DynamicClassLoaderFactory.dynamicTypes.ToArray();
                foreach (var i in l)
                {
                    var tw = i.Value;
                    if (tw != null && !done.ContainsKey(tw))
                    {
                        more = true;
                        done.Add(tw, tw);
                        diagnostics.GenericRuntimeTrace($"Finishing: {tw.TypeAsTBD.FullName}");
                        tw.Finish();
                    }
                }
            }

            if (unloadableContainer != null)
            {
                unloadableContainer.CreateType();
                foreach (var tb in unloadables.Values)
                    tb.CreateType();
            }

#if IMPORTER
            if (proxiesContainer != null)
            {
                proxiesContainer.CreateType();
                foreach (var tb in proxies)
                    tb.CreateType();
            }

#endif

        }

#if !IMPORTER

        internal static ModuleBuilder CreateJniProxyModuleBuilder()
        {
            AssemblyName name = new AssemblyName();
            name.Name = "jniproxy";
            jniProxyAssemblyBuilder = DefineDynamicAssembly(name, AssemblyBuilderAccess.Run, null);
            return jniProxyAssemblyBuilder.DefineDynamicModule("jniproxy.dll");
        }

#endif

        internal sealed override ModuleBuilder ModuleBuilder => moduleBuilder;

#if !IMPORTER

#if NETFRAMEWORK

        internal sealed class ForgedKeyPair : StrongNameKeyPair
        {

            internal static readonly StrongNameKeyPair Instance;

            static ForgedKeyPair()
            {
                try
                {
                    // this public key byte array must be the same as the public key in DynamicAssemblySuffixAndPublicKey
                    Instance = new ForgedKeyPair(new byte[] {
                        0x00, 0x24, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0x94, 0x00, 0x00,
                        0x00, 0x06, 0x02, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x52, 0x53,
                        0x41, 0x31, 0x00, 0x04, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x9D,
                        0x67, 0x4F, 0x3D, 0x63, 0xB8, 0xD7, 0xA4, 0xC4, 0x28, 0xBD, 0x73,
                        0x88, 0x34, 0x1B, 0x02, 0x5C, 0x71, 0xAA, 0x61, 0xC6, 0x22, 0x4C,
                        0xD5, 0x3A, 0x12, 0xC2, 0x13, 0x30, 0xA3, 0x15, 0x9D, 0x30, 0x00,
                        0x51, 0xFE, 0x2E, 0xED, 0x15, 0x4F, 0xE3, 0x0D, 0x70, 0x67, 0x3A,
                        0x07, 0x9E, 0x45, 0x29, 0xD0, 0xFD, 0x78, 0x11, 0x3D, 0xCA, 0x77,
                        0x1D, 0xA8, 0xB0, 0xC1, 0xEF, 0x2F, 0x77, 0xB7, 0x36, 0x51, 0xD5,
                        0x56, 0x45, 0xB0, 0xA4, 0x29, 0x4F, 0x0A, 0xF9, 0xBF, 0x70, 0x78,
                        0x43, 0x2E, 0x13, 0xD0, 0xF4, 0x6F, 0x95, 0x1D, 0x71, 0x2C, 0x2F,
                        0xCF, 0x02, 0xEB, 0x15, 0x55, 0x2C, 0x0F, 0xE7, 0x81, 0x7F, 0xC0,
                        0xAE, 0xD5, 0x8E, 0x09, 0x84, 0xF8, 0x66, 0x61, 0xBF, 0x64, 0xD8,
                        0x82, 0xF2, 0x9B, 0x61, 0x98, 0x99, 0xDD, 0x26, 0x40, 0x41, 0xE7,
                        0xD4, 0x99, 0x25, 0x48, 0xEB, 0x9E
                    });
                }
                catch
                {

                }
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="publicKey"></param>
            private ForgedKeyPair(byte[] publicKey) :
                base(ToInfo(publicKey), new StreamingContext())
            {

            }

            private static SerializationInfo ToInfo(byte[] publicKey)
            {
                byte[] privateKey = publicKey;
                SerializationInfo info = new SerializationInfo(typeof(StrongNameKeyPair), new FormatterConverter());
                info.AddValue("_keyPairExported", true);
                info.AddValue("_keyPairArray", privateKey);
                info.AddValue("_keyPairContainer", null);
                info.AddValue("_publicKey", publicKey);
                return info;
            }
        }

#endif

        public static ModuleBuilder CreateModuleBuilder(RuntimeContext context)
        {
            AssemblyName name = new AssemblyName();
            name.Name = "ikvm_dynamic_assembly__" + (uint)Environment.TickCount;
            return CreateModuleBuilder(context, name);
        }

        public static ModuleBuilder CreateModuleBuilder(RuntimeContext context, AssemblyName name)
        {
            var now = DateTime.Now;
            name.Version = new Version(now.Year, (now.Month * 100) + now.Day, (now.Hour * 100) + now.Minute, (now.Second * 1000) + now.Millisecond);
            var attribs = new List<CustomAttributeBuilder>();
            AssemblyBuilderAccess access = AssemblyBuilderAccess.Run;

#if NETFRAMEWORK
            if (!AppDomain.CurrentDomain.IsFullyTrusted)
                attribs.Add(new CustomAttributeBuilder(typeof(System.Security.SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
#endif

            var assemblyBuilder = DefineDynamicAssembly(name, access, attribs);
            context.AttributeHelper.SetRuntimeCompatibilityAttribute(assemblyBuilder);

            // determine debugging mode
            var debugMode = System.Diagnostics.DebuggableAttribute.DebuggingModes.Default | System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints;
            if (JVM.EmitSymbols)
                debugMode |= System.Diagnostics.DebuggableAttribute.DebuggingModes.DisableOptimizations;

            context.AttributeHelper.SetDebuggingModes(assemblyBuilder, debugMode);

#if NETFRAMEWORK
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, JVM.EmitSymbols);
#else
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name);
#endif

            moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(IKVM.Attributes.JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
            return moduleBuilder;
        }

        static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
        {
#if NETFRAMEWORK
            return AppDomain.CurrentDomain.DefineDynamicAssembly(name, access, null, true, assemblyAttributes);
#else
            return AssemblyBuilder.DefineDynamicAssembly(name, access, assemblyAttributes);
#endif
        }

#endif

    }

}
