using com.sun.javatest;
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
    /// <typeparam name="TTestSuite"></typeparam>
    class IkvmTestManager : TestManager
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

        public override Set getTestSuites()
        {
            var set = new LinkedHashSet();

            foreach (var e in map.AsDictionary<File, Entry>().Values)
            {
                if (e.testSuite == null)
                {
                    try
                    {
                        e.testSuite = IkvmRegressionTestSuite.Open(e.rootDir, errHandler);
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

    }

}
