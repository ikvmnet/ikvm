using System;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

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
                return resolver.Resolve(assemblyName.Name) is string s ? new PEReader(File.OpenRead(s)) : null;
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
        bool Init(FrameworkSpec framework, out Universe universe, out TestAssemblyResolver resolver, out ILVerify.Verifier verifier, out string tempPath, out MetadataLoadContext tempLoad)
        {
            universe = null;
            resolver = null;
            verifier = null;
            tempPath = null;
            tempLoad = null;

            // no reference assemblies for NetFX on Unix
            if (framework.TargetFrameworkIdentifier == ".NETFramework")
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    return false;

            // set up temporary directory
            tempPath = Path.Combine(Path.GetTempPath(), typeof(ModuleWriterTests).Namespace, typeof(ModuleWriterTests).Name, Guid.NewGuid().ToString());
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            output.WriteLine("TempPath: {0}", tempPath);

            // initialize primary classes
            universe = new Universe(DotNetSdkUtil.GetCoreLibName(framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion));
            resolver = new TestAssemblyResolver(universe, framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion);
            verifier = new ILVerify.Verifier(new VerifyResolver(resolver), new ILVerify.VerifierOptions() { SanityChecks = true });
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
            var m = t.GetMethod("Method");
            m.Should().NotBeNull();
            m.Should().Return(tempLoad.CoreAssembly.GetType("System.Object"));
        }

    }

}
