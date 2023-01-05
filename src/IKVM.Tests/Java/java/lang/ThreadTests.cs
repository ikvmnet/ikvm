using System;

using FluentAssertions;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ThreadTests
    {

        class TestRunnable : Runnable
        {

            readonly Action action;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="action"></param>
            /// <exception cref="System.ArgumentNullException"></exception>
            public TestRunnable(Action action)
            {
                this.action = action ?? throw new ArgumentNullException(nameof(action));
            }

            public void run()
            {
                action();
            }

        }

        [TestMethod]
        public void CanJoinThreads()
        {
            var t = new Thread[16];
            for (var i = 0; i < t.Length; i++)
                t[i] = new Thread(new TestRunnable(() => Thread.sleep(1000)));
            for (var i = 0; i < t.Length; i++)
                t[i].start();
            for (var i = 0; i < t.Length; i++)
                t[i].join();
        }

        [TestMethod]
        public void CanInterruptSleepingThread()
        {
            var e = (global::java.lang.Exception)null;
            var t = new Thread(new TestRunnable(() =>
            {
                try
                {
                    Thread.sleep(10000);
                }
                catch (InterruptedException _)
                {
                    e = _;
                }
            }));
            t.start();
            Thread.sleep(1000);
            t.interrupt();
            t.join();
            e.Should().BeOfType<InterruptedException>();
        }

        [TestMethod]
        public void CanInterruptWaitingThread()
        {
            var e = (global::java.lang.Exception)null;
            var t = new Thread(new TestRunnable(() =>
            {
                try
                {
                    var o = new global::java.lang.Object();
                    lock (o)
                        o.wait(100000);
                }
                catch (InterruptedException _)
                {
                    e = _;
                }
            }));
            t.start();
            Thread.sleep(1000);
            t.interrupt();
            t.join();
            e.Should().BeOfType<InterruptedException>();
        }

    }

}
