using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    /// <summary>
    /// Tests for <see cref="MethodBuilder"/>.
    /// </summary>
    public class MethodBuilderTests
    {

        /// <summary>
        /// Checks that a DllImportAttribute set on a method builder becomes its P/Invoke import, readable from the created
        /// type before and after saving and written to the image.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSetDllImport(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var dllImport = u.Import(typeof(System.Runtime.InteropServices.DllImportAttribute));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type", TypeAttributes.Public);
            var method = type.DefineMethod("Beep", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl, u.Import(typeof(bool)), [u.Import(typeof(uint))]);
            method.SetCustomAttribute(new CustomAttributeBuilder(dllImport.GetConstructor([u.Import(typeof(string))]), ["user32.dll"], [dllImport.GetField("EntryPoint")], ["MessageBeep"]));
            var created = type.CreateType();

            // checks what the created type reports
            void Check()
            {
                created.GetMethod("Beep")!.__TryGetImplMap(out _, out var importName, out var importScope).Should().BeTrue();
                importName.Should().Be("MessageBeep");
                importScope.Should().Be("user32.dll");
            }

            Check();
            assembly.Save("Test.dll");
            Check();

            // MetadataLoadContext only synthesizes DllImportAttribute when its core assembly defines it, so read the import directly
            u.Verify("Test.dll");
            using var pe = new PEReader(File.OpenRead(Path.Combine(u.TempPath, "Test.dll")));
            var md = pe.GetMetadataReader();
            var import = md.MethodDefinitions.Select(md.GetMethodDefinition).Single(i => md.GetString(i.Name) == "Beep").GetImport();
            md.GetString(import.Name).Should().Be("MessageBeep");
            md.GetString(md.GetModuleReference(import.Module).Name).Should().Be("user32.dll");
        }

    

        /// <summary>
        /// A builder's parameter reports a decimal default recorded with <c>DecimalConstantAttribute</c>, as the same
        /// parameter does once the assembly is saved and read back.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void ParameterReportsDecimalConstantDefault(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type", TypeAttributes.Public);
            var method = type.DefineMethod("Method", MethodAttributes.Public | MethodAttributes.Static, u.Import(typeof(void)), [u.Import(typeof(decimal))]);
            var parameter = method.DefineParameter(1, ParameterAttributes.Optional, "value");
            var byteType = u.Import(typeof(byte));
            var uintType = u.Import(typeof(uint));
            var constant = u.Import(typeof(System.Runtime.CompilerServices.DecimalConstantAttribute)).GetConstructor([byteType, byteType, uintType, uintType, uintType]);
            parameter.SetCustomAttribute(new CustomAttributeBuilder(constant, [(byte)2, (byte)0, 0u, 0u, 1234u]));
            method.GetILGenerator().Emit(OpCodes.Ret);
            var created = type.CreateType();

            created.GetMethod("Method")!.GetParameters()[0].RawDefaultValue.Should().Be(12.34m);

            assembly.Save("Test.dll");
            using var reader = TestUniverse.Create(tfm);
            reader.Universe.LoadFile(System.IO.Path.Combine(u.TempPath, "Test.dll")).GetType("Type", true).GetMethod("Method")!.GetParameters()[0].RawDefaultValue.Should().Be(12.34m);
        }

    }

}
