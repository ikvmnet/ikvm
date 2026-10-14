using System.IO;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    /// <summary>
    /// Tests for <see cref="AssemblyBuilder"/>.
    /// </summary>
    public class AssemblyBuilderTests
    {

        /// <summary>
        /// Reads the sample icon embedded in the test assembly.
        /// </summary>
        /// <returns>The bytes of the icon file.</returns>
        static byte[] GetSampleIcon()
        {
            using var s = typeof(AssemblyBuilderTests).Assembly.GetManifestResourceStream("IKVM.Reflection.Tests.sample.ico")!;
            using var m = new MemoryStream();
            s.CopyTo(m);
            return m.ToArray();
        }

        /// <summary>
        /// Checks that a custom attribute with a constructor argument set on an assembly builder is written to the
        /// saved assembly with its constructor and argument.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSetCustomAttributeWithArgument(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var stringType = u.Import(typeof(string));

            var assembly = u.DefineAssembly();
            assembly.SetCustomAttribute(new CustomAttributeBuilder(u.Import(typeof(System.Reflection.AssemblyVersionAttribute)).GetConstructor([stringType]), ["1.0.0.0"]));
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            module.DefineType("Test").CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            var d = a.GetCustomAttributesData().Should().ContainSingle().Subject;
            d.Constructor.Should().BeSameAs(u.LoadContextType("System.Reflection.AssemblyVersionAttribute").GetConstructor([u.LoadContextType("System.String")]));
            d.ConstructorArguments.Should().ContainSingle().Which.Value.Should().Be("1.0.0.0");
        }

        /// <summary>
        /// Checks that an assembly with an icon resource defined by
        /// <see cref="AssemblyBuilder.__DefineIconResource(byte[])"/> saves, verifies and loads.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineIconResource(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            assembly.__DefineIconResource(GetSampleIcon());
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            module.DefineType("Test").CreateType();
            assembly.Save("Test.dll");

            u.VerifyAndLoad("Test.dll").GetName().Name.Should().Be("Test");
        }

        /// <summary>
        /// Checks that a console application saves with its entry point.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSaveConsoleApplication(string tfm)
        {
            CanSaveApplication(tfm, PEFileKinds.ConsoleApplication, false);
        }

        /// <summary>
        /// Checks that a Windows application with an icon resource saves with its entry point.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSaveWindowsApplicationWithIcon(string tfm)
        {
            CanSaveApplication(tfm, PEFileKinds.WindowApplication, true);
        }

        /// <summary>
        /// Saves an executable with a static <c>Main</c> entry point and checks that it verifies and that its entry point
        /// reads back.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        /// <param name="kind">The kind of executable to save.</param>
        /// <param name="icon">Whether to define an icon resource.</param>
        static void CanSaveApplication(string tfm, PEFileKinds kind, bool icon)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            if (icon)
                assembly.__DefineIconResource(GetSampleIcon());

            var module = assembly.DefineDynamicModule("Test", "Test.exe", false);
            var type = module.DefineType("Type");
            var mainMethod = type.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, u.Import(typeof(void)), [u.Import(typeof(string[]))]);
            mainMethod.GetILGenerator().Emit(OpCodes.Ret);
            assembly.SetEntryPoint(mainMethod, kind);
            type.CreateType();
            assembly.Save("Test.exe", PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64);

            var a = u.VerifyAndLoad("Test.exe");
            a.GetName().Name.Should().Be("Test");
            a.EntryPoint.Should().NotBeNull();
            a.EntryPoint!.Name.Should().Be("Main");
            a.GetType("Type").Should().HaveMethod("Main", [u.LoadContextType("System.String").MakeArrayType()]).Which.Should().Return(u.LoadContextType("System.Void"));
        }

    }

}
