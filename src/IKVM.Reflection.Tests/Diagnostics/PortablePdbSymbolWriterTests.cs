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

    /// <summary>
    /// Tests for <see cref="PortablePdbSymbolWriter"/>.
    /// </summary>
    public class PortablePdbSymbolWriterTests
    {

        /// <summary>
        /// Checks that methods with sequence points get debug information holding those sequence points and naming the
        /// same local signature as their body, or none for a method without locals.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
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

            // checks the debug information of the named method against its body
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

        /// <summary>
        /// Each local scope lists exactly its own variables, including a scope that declares none followed by one
        /// that does, which is where a nil variable list would make readers attribute the wrong rows.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanWriteLocalScopes(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            u.Universe.SetSymbolWriterFactory(module => new PortablePdbSymbolWriter(module));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", true);
            var document = module.DefineDocument("Test.java", Guid.Empty, Guid.Empty, Guid.Empty);
            var type = module.DefineType("Type", TypeAttributes.Public);
            var int32 = u.Import(typeof(int));

            // an outer scope with a local, holding an inner scope without one
            var first = type.DefineMethod("First", MethodAttributes.Public | MethodAttributes.Static, null, Type.EmptyTypes);
            var il = first.GetILGenerator();
            il.BeginScope();
            var a = il.DeclareLocal(int32);
            a.SetLocalSymInfo("a");
            il.MarkSequencePoint(document, 1, 1, 1, 10);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Stloc, a);
            il.BeginScope();
            il.Emit(OpCodes.Nop);
            il.EndScope();
            il.EndScope();
            il.Emit(OpCodes.Ret);

            // a method whose scope has two locals, written after the scope without any
            var second = type.DefineMethod("Second", MethodAttributes.Public | MethodAttributes.Static, null, Type.EmptyTypes);
            il = second.GetILGenerator();
            il.BeginScope();
            var b = il.DeclareLocal(int32);
            b.SetLocalSymInfo("b");
            var c = il.DeclareLocal(int32);
            c.SetLocalSymInfo("c");
            il.MarkSequencePoint(document, 2, 1, 2, 10);
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Stloc, b);
            il.Emit(OpCodes.Ldc_I4_3);
            il.Emit(OpCodes.Stloc, c);
            il.EndScope();
            il.Emit(OpCodes.Ret);

            type.CreateType();
            assembly.Save("Test.dll");
            u.Verify("Test.dll");

            using var pe = new PEReader(File.OpenRead(Path.Combine(u.TempPath, "Test.dll")));
            var md = pe.GetMetadataReader();
            using var pdbProvider = MetadataReaderProvider.FromPortablePdbStream(File.OpenRead(Path.Combine(u.TempPath, "Test.pdb")));
            var pdb = pdbProvider.GetMetadataReader();

            // the variable names of each scope of the named method, outermost first; the first is the root scope the
            // writer adds for the whole method
            string[][] Scopes(string name)
            {
                var handle = md.MethodDefinitions.Single(i => md.GetString(md.GetMethodDefinition(i).Name) == name);
                return pdb.GetLocalScopes(handle)
                    .Select(pdb.GetLocalScope)
                    .Select(s => s.GetLocalVariables().Select(v => pdb.GetString(pdb.GetLocalVariable(v).Name)).ToArray())
                    .ToArray();
            }

            Scopes("First").Should().BeEquivalentTo(new[] { new string[0], new[] { "a" }, new string[0] }, o => o.WithStrictOrdering());
            Scopes("Second").Should().BeEquivalentTo(new[] { new string[0], new[] { "b", "c" } }, o => o.WithStrictOrdering());
        }

    }

}
