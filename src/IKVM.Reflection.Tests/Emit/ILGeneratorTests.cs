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

    }

}
