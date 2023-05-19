using FluentAssertions;

using java.lang;
using java.lang.management;
using java.util;

using javax.management;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.management
{

    [TestClass]
    public class ThreadMXBeanTests
    {

        /// <summary>
        /// Mirrored from jdk.javax/management/mxbean/ThreadMXBeanTest.
        /// Ensures that invalid thread ID values passed to ThreadMXBean result in null info results.
        /// </summary>
        [TestMethod]
        public void ShouldReturnNullForInvalidThreads()
        {
            var mbs = MBeanServerFactory.newMBeanServer();
            var tmb = ManagementFactory.getThreadMXBean();
            var smb = new StandardMBean(tmb, typeof(ThreadMXBean), true);
            var on = new ObjectName("a:type=ThreadMXBean");
            mbs.registerMBean(smb, on);

            var proxy = (ThreadMXBean)JMX.newMXBeanProxy(mbs, on, typeof(ThreadMXBean));
            var ids1 = proxy.getAllThreadIds();

            // add some random ids to the list so we'll get back null ThreadInfo
            var ids2 = new long[ids1.Length + 10];
            global::java.lang.System.arraycopy(ids1, 0, ids2, 0, ids1.Length);
            var r = new Random();
            for (var i = ids1.Length; i < ids2.Length; i++)
                ids2[i] = Math.abs(r.nextLong());

            // produces an exception if null values not handled
            var info = proxy.getThreadInfo(ids2);
            var sawNull = false;
            foreach (var ti in info)
                if (ti == null)
                    sawNull = true;

            sawNull.Should().BeTrue();
        }

    }

}
