using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using FluentAssertions;

using IKVM.Reflection.Diagnostics;
using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Diagnostics
{

    public class PortablePdbSymbolWriterTests
    {

        /// <summary>
        /// Methods with sequence points get debug information that names the same local signature as their body, or none
        /// for a method without locals.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteSequencePoints(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            u.Universe.SetSymbolWriterFactory(module => new PortablePdbSymbolWriter(module));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", true);
            var document = module.DefineDocument("Test.java", Guid.Empty, Guid.Empty, Guid.Empty);
            var type = module.DefineType("Type", TypeAttributes.Public);

            var without = type.DefineMethod("Without", MethodAttributes.Public | MethodAttributes.Static, null, Type.EmptyTypes);
            var il = without.GetILGenerator();
            il.MarkSequencePoint(document, 1, 1, 1, 10);
            il.Emit(OpCodes.Ret);

            var with = type.DefineMethod("With", MethodAttributes.Public | MethodAttributes.Static, null, Type.EmptyTypes);
            il = with.GetILGenerator();
            var local = il.DeclareLocal(u.Import(typeof(int)));
            il.MarkSequencePoint(document, 2, 1, 2, 10);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Stloc, local);
            il.MarkSequencePoint(document, 3, 1, 3, 10);
            il.Emit(OpCodes.Ret);

            type.CreateType();
            assembly.Save("Test.dll");
            u.Verify("Test.dll");

            using var pe = new PEReader(File.OpenRead(Path.Combine(u.TempPath, "Test.dll")));
            var md = pe.GetMetadataReader();
            using var pdbProvider = MetadataReaderProvider.FromPortablePdbStream(File.OpenRead(Path.Combine(u.TempPath, "Test.pdb")));
            var pdb = pdbProvider.GetMetadataReader();

            void Check(string name, int sequencePoints)
            {
                var handle = md.MethodDefinitions.Single(i => md.GetString(md.GetMethodDefinition(i).Name) == name);
                var body = pe.GetMethodBody(md.GetMethodDefinition(handle).RelativeVirtualAddress);
                var info = pdb.GetMethodDebugInformation(handle);
                info.LocalSignature.Should().Be(body.LocalSignature);
                info.GetSequencePoints().Should().HaveCount(sequencePoints);
            }

            Check("Without", 1);
            Check("With", 2);
        }

    }

}
