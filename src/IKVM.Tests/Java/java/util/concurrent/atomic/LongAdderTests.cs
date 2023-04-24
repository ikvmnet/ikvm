using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using java.util.concurrent.atomic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.concurrent.atomic
{

    [TestClass]
    public class LongAdderTests
    {

        [TestMethod]
        public void ShouldSumAcrossMultipleThreads()
        {
            var taskCount = 8;
            var incrCount = 128;

            var counter = new LongAdder();

            void Action()
            {
                for (int i = 0; i < incrCount; i++)
                    counter.increment();
            }

            var l = new List<Task>();
            for (int i = 0; i < taskCount; i++)
                l.Add(Task.Run(Action));
            Task.WhenAll(l).Wait();

            counter.sumThenReset().Should().Be(taskCount * incrCount);
            counter.sum().Should().Be(0);
        }

    }

}
