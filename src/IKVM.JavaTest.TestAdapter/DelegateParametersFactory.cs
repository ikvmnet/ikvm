using System;

using com.sun.javatest.regtest.config;

namespace IKVM.JavaTest
{

    /// <summary>
    /// Implementation of <see cref="RegressionTestSuite.ParametersFactory"/> that invokes a delegate.
    /// </summary>
    class ParametersFactory : RegressionTestSuite.ParametersFactory
    {

        readonly Func<RegressionTestSuite, RegressionParameters> onCreate;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onCreate"></param>
        public ParametersFactory(Func<RegressionTestSuite, RegressionParameters> onCreate)
        {
            this.onCreate = onCreate;
        }

        public RegressionParameters create(RegressionTestSuite rts)
        {
            return onCreate(rts);
        }

    }

}
