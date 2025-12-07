using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;

using FluentAssertions;

using IKVM.Reflection.Emit;
using IKVM.Tests.Util;

using Xunit;
using Xunit.Abstractions;

namespace IKVM.Reflection.Tests
{

    public partial class ModuleWriterTests
    {

        /// <summary>
        /// Provides resolution for ILVerify.
        /// </summary>
        class VerifyResolver : ILVerify.IResolver
        {

            readonly TestAssemblyResolver resolver;
            readonly ConcurrentDictionary<string, PEReader> cache = new ConcurrentDictionary<string, PEReader>();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver"></param>
            public VerifyResolver(TestAssemblyResolver resolver)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            public PEReader ResolveAssembly(System.Reflection.AssemblyName assemblyName)
            {
                return cache.GetOrAdd(assemblyName.Name, _ => resolver.Resolve(_) is string s ? new PEReader(File.OpenRead(s)) : null);
            }

            public PEReader ResolveModule(System.Reflection.AssemblyName referencingAssembly, string fileName)
            {
                return resolver.Resolve(fileName) is string s ? new PEReader(File.OpenRead(s)) : null;
            }

        }

        class MetadataAssemblyResolver : System.Reflection.MetadataAssemblyResolver
        {

            readonly TestAssemblyResolver resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public MetadataAssemblyResolver(TestAssemblyResolver resolver)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            public override System.Reflection.Assembly Resolve(MetadataLoadContext context, System.Reflection.AssemblyName assemblyName)
            {
                return resolver.Resolve(assemblyName.Name) is string s ? context.LoadFromAssemblyPath(s) : null;
            }

        }

        readonly ITestOutputHelper output;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="output"></param>
        public ModuleWriterTests(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <summary>
        /// Initializes the variables requires to execute tests.
        /// </summary
        /// <param name="framework"></param>
        /// <param name="universe"></param>
        /// <param name="resolver"></param>
        /// <param name="verifier"></param>
        /// <param name="tempPath"></param>
        /// <param name="tempLoad"></param>
        bool Init(FrameworkSpec framework, out Universe universe, out TestAssemblyResolver resolver, out ILVerify.Verifier verifier, out string tempPath, out MetadataLoadContext tempLoad)
        {
            return Init(framework, null, out universe, out resolver, out verifier, out tempPath, out tempLoad);
        }

        /// <summary>
        /// Initializes the variables requires to execute tests.
        /// </summary
        /// <param name="framework"></param>
        /// <param name="searchPaths"></param>
        /// <param name="universe"></param>
        /// <param name="resolver"></param>
        /// <param name="verifier"></param>
        /// <param name="tempPath"></param>
        bool Init(FrameworkSpec framework, IEnumerable<string> searchPaths, out Universe universe, out TestAssemblyResolver resolver, out ILVerify.Verifier verifier, out string tempPath, out MetadataLoadContext tempLoad)
        {
            universe = null;
            resolver = null;
            verifier = null;
            tempPath = null;
            tempLoad = null;

            // set up temporary directory
            tempPath = Path.Combine(Path.GetTempPath(), typeof(ModuleWriterTests).Namespace, typeof(ModuleWriterTests).Name, Guid.NewGuid().ToString());
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            output.WriteLine("TempPath: {0}", tempPath);

            // initialize primary classes
            universe = new Universe(DotNetSdkUtil.GetCoreLibName(framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion));
            resolver = new TestAssemblyResolver(universe, framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion, searchPaths);
            verifier = new ILVerify.Verifier(new VerifyResolver(resolver), new ILVerify.VerifierOptions() { IncludeMetadataTokensInErrorMessages = true, SanityChecks = true });
            verifier.SetSystemModuleName(new System.Reflection.AssemblyName(universe.CoreLibName));
            tempLoad = new MetadataLoadContext(new MetadataAssemblyResolver(resolver));

            return true;
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteModule(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");

            var valueField = type.DefineField("value", universe.Import(typeof(object)), FieldAttributes.Private);

            var getValueMethod = type.DefineMethod("get_Value", MethodAttributes.Public, universe.Import(typeof(object)), Array.Empty<Type>());
            var getValueIL = getValueMethod.GetILGenerator();
            getValueIL.Emit(OpCodes.Ldarg_0);
            getValueIL.Emit(OpCodes.Ldfld, valueField);
            getValueIL.Emit(OpCodes.Ret);

            var setValueMethod = type.DefineMethod("set_Value", MethodAttributes.Public, universe.Import(typeof(void)), new[] { universe.Import(typeof(object)) });
            var setValueIL = setValueMethod.GetILGenerator();
            setValueIL.Emit(OpCodes.Ldarg_0);
            setValueIL.Emit(OpCodes.Ldarg_1);
            setValueIL.Emit(OpCodes.Stfld, valueField);
            setValueIL.Emit(OpCodes.Ret);

            var valueProperty = type.DefineProperty("Value", PropertyAttributes.None, universe.Import(typeof(object)), Array.Empty<Type>());
            valueProperty.SetGetMethod(getValueMethod);
            valueProperty.SetSetMethod(setValueMethod);

            var execMethod = type.DefineMethod("Exec", MethodAttributes.Public, universe.Import(typeof(object)), new[] { universe.Import(typeof(string[])) });
            execMethod.DefineParameter(0, ParameterAttributes.None, "args");
            var execMethodIL = execMethod.GetILGenerator();
            execMethodIL.Emit(OpCodes.Ldnull);
            execMethodIL.Emit(OpCodes.Ret);

            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            a.GetName().Name.Should().Be("Test");
            a.GetModule("Test").Should().NotBeNull();
            var t = a.GetType("Type");
            t.Should().NotBeNull();
            t.Should().NotBeStatic();
            t.Should().HaveMethod("Exec", new[] { tempLoad.CoreAssembly.GetType("System.String").MakeArrayType() }).Which.Should().Return(tempLoad.CoreAssembly.GetType("System.Object"));
            t.Should().HaveProperty(tempLoad.CoreAssembly.GetType("System.Object"), "Value");
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteInterfaceImplementation(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var ifaceBuilder = module.DefineType("Iface", TypeAttributes.Interface);
            var ifaceMethod = ifaceBuilder.DefineMethod("Method", MethodAttributes.Abstract, universe.Import(typeof(object)), Array.Empty<Type>());

            var implType = module.DefineType("Impl", TypeAttributes.Public, null, new[] { ifaceBuilder });
            var implMethod = implType.DefineMethod("Method", MethodAttributes.Public, universe.Import(typeof(object)), Array.Empty<Type>());
            implType.DefineMethodOverride(ifaceMethod, implMethod);

            var il = implMethod.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);

            ifaceBuilder.CreateType();
            implType.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            var i = a.GetType("Iface");
            i.IsInterface.Should().BeTrue();
            i.Should().HaveMethod("Method", Array.Empty<System.Type>());
            var t = a.GetType("Impl");
            t.IsInterface.Should().BeFalse();
            t.IsClass.Should().BeTrue();
            t.GetInterfaces().Should().Contain(i);
            var m = t.GetMethod("Method");
            m.Should().NotBeNull();
            m.Should().Return(tempLoad.CoreAssembly.GetType("System.Object"));
        }

        /// <summary>
        /// Due to the unique sorting requirements of InterfaceImpl, we check that we can emit two different sorted by a differnt class.
        /// </summary>
        /// <param name="framework"></param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteMultipleInterfaceImplementation(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var iface1Type = module.DefineType("Iface1", TypeAttributes.Interface);
            var iface1Method = iface1Type.DefineMethod("Method1", MethodAttributes.Abstract, universe.Import(typeof(object)), Array.Empty<Type>());

            var iface2Type = module.DefineType("Iface2", TypeAttributes.Interface);
            var iface2Method = iface2Type.DefineMethod("Method2", MethodAttributes.Abstract, universe.Import(typeof(object)), Array.Empty<Type>());

            var impl1Type = module.DefineType("Impl1", TypeAttributes.Public, null, new[] { iface1Type });
            var impl1Method = impl1Type.DefineMethod("Method1", MethodAttributes.Public, universe.Import(typeof(object)), Array.Empty<Type>());

            var il1 = impl1Method.GetILGenerator();
            il1.Emit(OpCodes.Ldnull);
            il1.Emit(OpCodes.Ret);

            var impl2Type = module.DefineType("Impl2", TypeAttributes.Public, null, new[] { iface2Type });
            var impl2Method = impl2Type.DefineMethod("Method2", MethodAttributes.Public, universe.Import(typeof(object)), Array.Empty<Type>());

            impl2Type.DefineMethodOverride(iface2Method, impl2Method);
            impl1Type.DefineMethodOverride(iface1Method, impl1Method);

            var il2 = impl2Method.GetILGenerator();
            il2.Emit(OpCodes.Ldnull);
            il2.Emit(OpCodes.Ret);

            iface1Type.CreateType();
            iface2Type.CreateType();
            impl1Type.CreateType();
            impl2Type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            var i1 = a.GetType("Iface1");
            i1.IsInterface.Should().BeTrue();
            i1.Should().HaveMethod("Method1", Array.Empty<System.Type>());
            var i2 = a.GetType("Iface2");
            i2.IsInterface.Should().BeTrue();
            i2.Should().HaveMethod("Method2", Array.Empty<System.Type>());
            var t1 = a.GetType("Impl1");
            t1.IsInterface.Should().BeFalse();
            t1.IsClass.Should().BeTrue();
            t1.GetInterfaces().Should().Contain(i1);
            var m1 = t1.GetMethod("Method1");
            m1.Should().NotBeNull();
            m1.Should().Return(tempLoad.CoreAssembly.GetType("System.Object"));
            var t2 = a.GetType("Impl2");
            t2.IsInterface.Should().BeFalse();
            t2.IsClass.Should().BeTrue();
            t2.GetInterfaces().Should().Contain(i2);
            var m2 = t2.GetMethod("Method2");
            m2.Should().NotBeNull();
            m2.Should().Return(tempLoad.CoreAssembly.GetType("System.Object"));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteConstantFieldValue(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var type = module.DefineType("Test");
            var field = type.DefineField("value", universe.Import(typeof(int)), FieldAttributes.Public | FieldAttributes.Static);
            field.SetConstant(128);
            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            var t = a.GetType("Test");
            var f = t.GetField("value");
            f.FieldType.Should().Be(tempLoad.CoreAssembly.GetType("System.Int32"));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteCustomAttributeWithArgument(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var cab = new CustomAttributeBuilder(universe.Import(typeof(AssemblyVersionAttribute)).GetConstructor(new[] { universe.Import(typeof(string)) }), new[] { "1.0.0.0" });
            assembly.SetCustomAttribute(cab);

            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            var d = a.GetCustomAttributesData();
            d.Should().HaveCount(1);
            d[0].ConstructorArguments.Should().HaveCount(1);
            d[0].ConstructorArguments[0].Value.Should().Be("1.0.0.0");
            d[0].Constructor.Should().BeSameAs(tempLoad.CoreAssembly.GetType("System.Reflection.AssemblyVersionAttribute").GetConstructor(new[] { tempLoad.CoreAssembly.GetType("System.String") }));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteManifestResources(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            module.DefineManifestResource("Resource1", new MemoryStream(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }), ResourceAttributes.Public);
            module.DefineManifestResource("Resource2", new MemoryStream(new byte[] { 0x06, 0x07, 0x08, 0x09, 0x0a }), ResourceAttributes.Public);
            module.DefineManifestResource("Resource3", new MemoryStream(new byte[] { 0x0b, 0x0c, 0x0d, 0x0e, 0x0f }), ResourceAttributes.Public);
            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
            a.GetManifestResourceNames().Should().HaveCount(3);

            var res1 = a.GetManifestResourceStream("Resource1");
            res1.Should().HaveLength(5);
            var buf1 = new byte[res1.Length];
            res1.Read(buf1, 0, (int)res1.Length);
            buf1.Should().BeEquivalentTo(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 });

            var res2 = a.GetManifestResourceStream("Resource2");
            res2.Should().HaveLength(5);
            var buf2 = new byte[res2.Length];
            res2.Read(buf2, 0, (int)res2.Length);
            buf2.Should().BeEquivalentTo(new byte[] { 0x06, 0x07, 0x08, 0x09, 0x0a });

            var res3 = a.GetManifestResourceStream("Resource3");
            res3.Should().HaveLength(5);
            var buf3 = new byte[res3.Length];
            res3.Read(buf3, 0, (int)res3.Length);
            buf3.Should().BeEquivalentTo(new byte[] { 0x0b, 0x0c, 0x0d, 0x0e, 0x0f });
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteWin32Icon(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            // obtain sample ico file
            var ico = typeof(ModuleWriterTests).Assembly.GetManifestResourceStream("IKVM.Reflection.Tests.sample.ico");
            var buf = new byte[ico.Length];
            ico.Read(buf, 0, (int)ico.Length);

            // define assembly with an ico file
            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            assembly.__DefineIconResource(buf);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            // reload the assembly
            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.dll"));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteConsoleApplication(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.exe", false);
            var type = module.DefineType("Type");

            var mainMethod = type.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, universe.Import(typeof(void)), new[] { universe.Import(typeof(string[])) });
            var mainMethodIL = mainMethod.GetILGenerator();
            mainMethodIL.Emit(OpCodes.Ret);
            assembly.SetEntryPoint(mainMethod, PEFileKinds.ConsoleApplication);

            type.CreateType();
            assembly.Save("Test.exe", PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64);

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.exe")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.exe"));
            a.GetName().Name.Should().Be("Test");
            a.GetModule("Test").Should().NotBeNull();
            var t = a.GetType("Type");
            t.Should().HaveMethod("Main", new[] { tempLoad.CoreAssembly.GetType("System.String").MakeArrayType() }).Which.Should().Return(tempLoad.CoreAssembly.GetType("System.Void"));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteWindowsApplication(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            // obtain sample ico file
            var ico = typeof(ModuleWriterTests).Assembly.GetManifestResourceStream("IKVM.Reflection.Tests.sample.ico");
            var buf = new byte[ico.Length];
            ico.Read(buf, 0, (int)ico.Length);

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            assembly.__DefineIconResource(buf);
            var module = assembly.DefineDynamicModule("Test", "Test.exe", false);
            var type = module.DefineType("Type");

            var mainMethod = type.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, universe.Import(typeof(void)), new[] { universe.Import(typeof(string[])) });
            var mainMethodIL = mainMethod.GetILGenerator();
            mainMethodIL.Emit(OpCodes.Ret);
            assembly.SetEntryPoint(mainMethod, PEFileKinds.WindowApplication);

            type.CreateType();
            assembly.Save("Test.exe", PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64);

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.exe")))))
                v.Code.Should().Be(ILVerify.VerifierError.None);

            var a = tempLoad.LoadFromAssemblyPath(Path.Combine(tempPath, "Test.exe"));
            a.GetName().Name.Should().Be("Test");
            a.GetModule("Test").Should().NotBeNull();
            var t = a.GetType("Type");
            t.Should().HaveMethod("Main", new[] { tempLoad.CoreAssembly.GetType("System.String").MakeArrayType() }).Which.Should().Return(tempLoad.CoreAssembly.GetType("System.Void"));
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteTryCatch(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver, out var verifier, out var tempPath, out var tempLoad) == false)
                return;

            var assembly = universe.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, tempPath);
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");

            var execMethod = type.DefineMethod("Exec", MethodAttributes.Public, null, Array.Empty<Type>());
            var execMethodIL = execMethod.GetILGenerator();

            var end = execMethodIL.BeginExceptionBlock();
            execMethodIL.Emit(OpCodes.Nop);
            execMethodIL.BeginCatchBlock(universe.Import(typeof(Exception)));
            execMethodIL.Emit(OpCodes.Nop);
            execMethodIL.EndExceptionBlock();
            execMethodIL.Emit(OpCodes.Ret);

            type.CreateType();
            assembly.Save("Test.dll");

            foreach (var v in verifier.Verify(new PEReader(File.OpenRead(Path.Combine(tempPath, "Test.dll")))))
                if (v.Code != ILVerify.VerifierError.None)
                    throw new Exception(string.Format(v.Message, v.Args ?? Array.Empty<object>()));
        }

        static string ToString(MetadataReader metadata, TypeDefinitionHandle typeHandle)
        {
            var b = new StringBuilder();

            if (typeHandle.IsNil)
            {
                b.Append("<global>");
            }
            else
            {
                var type = metadata.GetTypeDefinition(typeHandle);
                if (type.GetDeclaringType().IsNil)
                {
                    var typeNamespace = type.Namespace.IsNil == false ? metadata.GetString(type.Namespace) : null;
                    var typeName = type.Name.IsNil == false ? metadata.GetString(type.Name) : null;
                    if (typeNamespace != null)
                        b.Append(typeNamespace).Append(".");

                    b.Append(typeName ?? "<UNKNOWN>");
                }
                else
                {
                    b.Append(ToString(metadata, type.GetDeclaringType())).Append("+");
                }
            }

            return b.ToString();
        }

        static string ToString(MetadataReader metadata, TypeDefinitionHandle typeHandle, MethodDefinitionHandle methodHandle)
        {
            var b = new StringBuilder();
            var method = metadata.GetMethodDefinition(methodHandle);
            typeHandle = method.GetDeclaringType();

            b.Append(ToString(metadata, typeHandle));
            b.Append(":");

            var methodName = method.Name.IsNil == false ? metadata.GetString(method.Name) : null;
            b.Append(methodName ?? "<UNKNOWN>");

            return b.ToString();
        }

    }

}
