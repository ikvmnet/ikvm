using System.Collections.Generic;

using com.sun.javatest;
using com.sun.javatest.regtest.config;

using java.io;

namespace IKVM.JTReg.TestAdapter
{

    class IkvmRegressionTestSuite : RegressionTestSuite
    {

        static Dictionary<File, System.WeakReference<IkvmRegressionTestSuite>> cache = new Dictionary<File, System.WeakReference<IkvmRegressionTestSuite>>();

        /// <summary>
        /// Gets the <see cref="IkvmRegressionTestSuite"/> for the given root path.
        /// </summary>
        /// <param name="testSuiteRoot"></param>
        /// <param name="errHandler"></param>
        /// <returns></returns>
        public static RegressionTestSuite Open(File testSuiteRoot, TestFinder.ErrorHandler errHandler)
        {
            var @ref = cache.TryGetValue(testSuiteRoot, out var __) ? __ : null;
            if (@ref == null || @ref.TryGetTarget(out var ts) == false)
            {
                ts = new IkvmRegressionTestSuite(testSuiteRoot, errHandler);
                cache.Add(testSuiteRoot, new System.WeakReference<IkvmRegressionTestSuite>(ts));
            }

            return ts;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="testSuiteRoot"></param>
        /// <param name="errHandler"></param>
        public IkvmRegressionTestSuite(File testSuiteRoot, TestFinder.ErrorHandler errHandler) :
            base(testSuiteRoot, errHandler)
        {

        }

    }

}
