using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    public class ILGeneratorTests
    {

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanEmitTryCatch(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");
            var method = type.DefineMethod("Exec", MethodAttributes.Public, null, Type.EmptyTypes);
            var il = method.GetILGenerator();
            il.BeginExceptionBlock();
            il.Emit(OpCodes.Nop);
            il.BeginCatchBlock(u.Import(typeof(System.Exception)));
            il.Emit(OpCodes.Nop);
            il.EndExceptionBlock();
            il.Emit(OpCodes.Ret);
            type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            var body = a.GetType("Type")!.GetMethod("Exec")!.GetMethodBody()!;
            body.ExceptionHandlingClauses.Should().ContainSingle().Which.CatchType!.FullName.Should().Be("System.Exception");
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanEmitManagedCalli(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var file = EmitCalli(u, il => il.EmitCalli(OpCodes.Calli, CallingConventions.Standard, u.Import(typeof(int)), [u.Import(typeof(string))], null));
            ReadStandAloneSignatures(u, file).Should().Equal("default(String) : Int32");
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanEmitUnmanagedCalli(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var file = EmitCalli(u, il => il.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.Cdecl, u.Import(typeof(int)), [u.Import(typeof(int))]));
            ReadStandAloneSignatures(u, file).Should().Equal("cdecl(Int32) : Int32");
        }

        /// <summary>
        /// A reference to a method with function pointers in its signature, as the importer makes when it calls such a
        /// .NET method with function pointers enabled, must reproduce the signature.
        /// </summary>
        [Fact]
        public void CanEmitCallToMethodWithFunctionPointers()
        {
            using var u = TestUniverse.CreateLikeImporter("net8.0");
            var methods = u.Universe.LoadFile(Fixture.GetAssemblyPath("net8.0")).GetType("IKVM.Reflection.Tests.Fixture.Methods", true)!;
            var target = methods.GetMethod("ManagedFunctionPointer")!;

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");
            var method = type.DefineMethod("Exec", MethodAttributes.Public | MethodAttributes.Static, null, [methods]);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Conv_I);
            il.Emit(OpCodes.Callvirt, target);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            type.CreateType();
            assembly.Save("Test.dll");

            using var pe = new PEReader(File.OpenRead(Path.Combine(u.TempPath, "Test.dll")));
            var md = pe.GetMetadataReader();
            var provider = new SignatureNameProvider(md);
            var signatures = md.MemberReferences.Select(md.GetMemberReference).Where(i => md.GetString(i.Name) == "ManagedFunctionPointer").Select(i => SignatureNameProvider.Format(i.DecodeMethodSignature(provider, null))).ToList();
            signatures.Should().Equal("default(fnptr default(Int32) : Void) : fnptr default(Int32) : Int32");
        }

        static string EmitCalli(TestUniverse u, System.Action<ILGenerator> calli)
        {
            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");
            var method = type.DefineMethod("Exec", MethodAttributes.Public | MethodAttributes.Static, null, Type.EmptyTypes);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Conv_I);
            calli(il);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            type.CreateType();
            assembly.Save("Test.dll");
            return Path.Combine(u.TempPath, "Test.dll");
        }

        static string[] ReadStandAloneSignatures(TestUniverse u, string file)
        {
            using var pe = new PEReader(File.OpenRead(file));
            var md = pe.GetMetadataReader();
            var provider = new SignatureNameProvider(md);
            return Enumerable.Range(1, System.Reflection.Metadata.Ecma335.MetadataReaderExtensions.GetTableRowCount(md, System.Reflection.Metadata.Ecma335.TableIndex.StandAloneSig))
                .Select(i => md.GetStandaloneSignature(System.Reflection.Metadata.Ecma335.MetadataTokens.StandaloneSignatureHandle(i)))
                .Where(i => i.GetKind() == StandaloneSignatureKind.Method)
                .Select(i => SignatureNameProvider.Format(i.DecodeMethodSignature(provider, null)))
                .ToArray();
        }

    }

}
