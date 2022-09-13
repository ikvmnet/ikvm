using System;
using System.Collections;

using com.sun.javatest;
using com.sun.javatest.logging;
using com.sun.javatest.regtest;
using com.sun.javatest.regtest.config;
using com.sun.javatest.util;

using java.io;
using java.lang;
using java.util;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Specialized version of <see cref="TestManager"/> for IKVM.
    /// </summary>
    class IkvmTestManager : TestManager, IDisposable
    {

        static readonly I18NResourceBundle i18n = I18NResourceBundle.getBundleForClass(typeof(TestManager));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="out"></param>
        /// <param name="baseDir"></param>
        /// <param name="errHandler"></param>
        public IkvmTestManager(PrintWriter @out, File baseDir, TestFinder.ErrorHandler errHandler) :
            base(@out, baseDir, errHandler)
        {

        }

        /// <summary>
        /// Gets the set of the available test suites.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AssertionError"></exception>
        /// <exception cref="Main.Fault"></exception>
        public override Set getTestSuites()
        {
            var set = new LinkedHashSet();

            foreach (var e in map.AsDictionary<File, Entry>().Values)
            {
                if (e.testSuite == null)
                {
                    try
                    {
                        e.testSuite = new IkvmRegressionTestSuite(e.rootDir, errHandler);
                        if (!e.testSuite.getRootDir().equals(e.rootDir))
                        {
                            java.lang.System.err.println("e.testSuite.getRootDir(): " + e.testSuite.getRootDir());
                            java.lang.System.err.println("e.rootDir: " + e.rootDir);
                            java.lang.System.err.println(e.testSuite.getRootDir().equals(e.rootDir));
                            throw new AssertionError();
                        }
                    }
                    catch (TestSuite.Fault f)
                    {
                        throw new Main.Fault(i18n, "tm.cantOpenTestSuite", e.testSuite, f);
                    }
                }

                set.add(e.testSuite);
            }

            return set;
        }

        /// <summary>
        /// Attempts to close the given test manager.
        /// </summary>
        public void Dispose()
        {
            try
            {
                foreach (IkvmRegressionTestSuite testSuite in (IEnumerable)getTestSuites())
                {
                    try
                    {
                        // log file is left open
                        var log = testSuite.getLog(getWorkDirectory(testSuite), I18NResourceBundle.getBundleForClass(typeof(TestResultCache)).getString("core.log.name"));
                        if (log != null && log.getHandlers() != null)
                            foreach (var handler in log.getHandlers())
                                if (handler is WorkDirLogHandler h)
                                    h.close();
                    }
                    catch (System.Exception)
                    {
                        // ignore
                    }

                    try
                    {
                        // test result cache is left open
                        var trt = getWorkDirectory(testSuite).getTestResultTable();
                        trt.dispose();
                    }
                    catch (System.Exception)
                    {
                        // ignore
                    }
                }
            }
            catch (System.Exception)
            {
                // ignore
            }
        }

    }

}
