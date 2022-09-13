using System;
using System.Diagnostics;

using com.sun.javatest;
using com.sun.javatest.regtest.config;
using com.sun.javatest.util;

using java.io;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// <see cref="RegressionTestSuite"/> implementation for IKVM.
    /// </summary>
    class IkvmRegressionTestSuite : RegressionTestSuite
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="testSuiteRoot"></param>
        /// <param name="errHandler"></param>
        public IkvmRegressionTestSuite(File testSuiteRoot, TestFinder.ErrorHandler errHandler) :
            base(testSuiteRoot, errHandler)
        {

        }

        /// <summary>
        /// Invoked when a process is executed.
        /// </summary>
        public Action<Process> ProcessEventHandler { get; set; }

        /// <summary>
        /// Creates a new script instance.
        /// </summary>
        /// <param name="td"></param>
        /// <param name="exclTestCases"></param>
        /// <param name="scriptEnv"></param>
        /// <param name="workDir"></param>
        /// <param name="backupPolicy"></param>
        /// <returns></returns>
        public override Script createScript(TestDescription td, string[] exclTestCases, TestEnvironment scriptEnv, WorkDirectory workDir, BackupPolicy backupPolicy)
        {
            var s = new IkvmRegressionScript();
            s.initTestDescription(td);
            s.initExcludedTestCases(exclTestCases);
            s.initTestEnvironment(scriptEnv);
            s.initWorkDir(workDir);
            s.initBackupPolicy(backupPolicy);
            s.initClassLoader(getClassLoader());
            s.ProcessEventHandler = ProcessEventHandler;
            return s;
        }

    }

}
