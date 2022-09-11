using com.sun.javatest;
using com.sun.javatest.regtest.config;

namespace IKVM.JTReg.TestAdapter
{

    class IkvmRegressionParameters : RegressionParameters
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="testSuite"></param>
        public IkvmRegressionParameters(string tag, RegressionTestSuite testSuite) :
            base(tag, testSuite)
        {

        }

        public override JDK getTestJDK()
        {
            return base.getTestJDK();
        }

        public override JDK getCompileJDK()
        {
            return base.getCompileJDK();
        }

        public override ExcludeList getExcludeList()
        {
            return base.getExcludeList();
        }

        public override Parameters.ExcludeListParameters getExcludeListParameters()
        {
            return base.getExcludeListParameters();
        }

        public override CachingTestFilter getExcludeListFilter()
        {
            return null;
        }

    }

}
