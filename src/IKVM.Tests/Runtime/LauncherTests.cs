using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class LauncherTests
    {

        [TestMethod]
        public void Can_handle_runtime_arg_prefix()
        {
            Launcher.SplitArguments(new[] { "-J-Dfoo=bar", "arg1", "arg2" }, out var jvmArgs, out var appArgs, "-J");
            jvmArgs.Should().HaveCount(1);
            jvmArgs.Should().ContainInOrder("-Dfoo=bar");
            appArgs.Should().HaveCount(2);
            appArgs.Should().ContainInOrder("arg1", "arg2");
        }

        [TestMethod]
        public void Can_handle_no_runtime_arg_prefix()
        {
            Launcher.SplitArguments(new[] { "-Dfoo=bar", "arg1", "arg2" }, out var jvmArgs, out var appArgs, "-J");
            jvmArgs.Should().HaveCount(0);
            appArgs.Should().HaveCount(3);
            appArgs.Should().ContainInOrder("-Dfoo=bar", "arg1", "arg2");
        }

        [TestMethod]
        public void Can_handle_runtime_arg_empty_prefix()
        {
            Launcher.SplitArguments(new[] { "-Dfoo=bar", "arg1", "arg2" }, out var jvmArgs, out var appArgs, "");
            jvmArgs.Should().HaveCount(3);
            jvmArgs.Should().ContainInOrder("-Dfoo=bar", "arg1", "arg2");
            appArgs.Should().HaveCount(0);
        }

        [TestMethod]
        public void Can_handle_runtime_arg_null_prefix()
        {
            Launcher.SplitArguments(new[] { "-Dfoo=bar", "arg1", "arg2" }, out var jvmArgs, out var appArgs, null);
            jvmArgs.Should().HaveCount(3);
            jvmArgs.Should().ContainInOrder("-Dfoo=bar", "arg1", "arg2");
            appArgs.Should().HaveCount(0);
        }

    }

}
