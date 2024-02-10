using FluentAssertions;

using IKVM.Java.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class DefaultInterfaceMethodTests
    {

        [TestMethod]
        [Ignore]
        public void ShouldResolveOverlappingDefaultImplementation()
        {
            var code = @"

public class TestClass {

    public static interface CommandInteractionPayload {

        int getValue();

    }

    public static interface CommandInteraction extends CommandInteractionPayload {

    }

    public static interface CommandInteractionPayloadMixin extends CommandInteractionPayload {

        @Override
        default int getValue() {
            return 1;
        }

    }

    public static class CommandInteractionImpl implements CommandInteraction, CommandInteractionPayloadMixin {
        
    }

    public static interface SlashCommandInteraction extends CommandInteraction {

    }

    public static class SlashCommandInteractionImpl extends CommandInteractionImpl implements SlashCommandInteraction {
        
    }

}

";
            var unit = new InMemoryCodeUnit("TestClass", code);
            var compiler = new InMemoryCompiler(new[] { unit });
            compiler.Compile();

            var clazz = compiler.GetClass("TestClass$SlashCommandInteractionImpl");
            var ctor = clazz.getConstructor();
            dynamic test = ctor.newInstance(System.Array.Empty<object>());
            ((int)test.getValue()).Should().Be(1);
        }

    }

}
