using System.Diagnostics;
using System.Reflection;

using com.sun.javatest.regtest.exec;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// <see cref="RegressionScript"/> implementation for IKVM.
    /// </summary>
    class IkvmRegressionScript : RegressionScript
    {

        /// <summary>
        /// Invoked when a process is executed.
        /// </summary>
        public System.Action<Process> ProcessEventHandler { get; set; }

        /// <summary>
        /// Invoked when a process is executed.
        /// </summary>
        /// <param name="process"></param>
        override protected void onProcessStart(java.lang.Process process)
        {
            var t = typeof(java.lang.Process).Assembly.GetType("java.lang.ProcessImpl");
            var f = t.GetField("handle", BindingFlags.NonPublic | BindingFlags.Instance);
            var p = (Process)f.GetValue(process);
            if (p.HasExited == false)
                ProcessEventHandler?.Invoke(p);
        }

    }

}
