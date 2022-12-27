using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    public interface IJTRegExecutionContext : IJTRegLoggerContext
    {

        /// <summary>
        /// Gets the run directory of the test execution.
        /// </summary>
        string TestRunDirectory { get; }

        /// <summary>
        /// Filters the test case based on the test execution.
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        bool FilterTestCase(JTRegTestCase test);

        /// <summary>
        /// Returns <c>true</c> if the test host can attach a debugger.
        /// </summary>
        bool CanAttachDebuggerToProcess { get; }

        /// <summary>
        /// Attaches a debugger to the given process ID.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        bool AttachDebuggerToProcess(int pid);

        void RecordStart(JTRegTestCase test);

        void RecordEnd(JTRegTestCase test, JTRegTestOutcome outcome);

        void RecordResult(JTRegTestResult rslt);

    }

}
