using com.sun.javatest;
using com.sun.javatest.regtest.exec;

namespace IKVM.JTReg.TestAdapter
{

    class IkvmRegressionScript : RegressionScript
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmRegressionScript()
        {

        }

        public override void run()
        {
            base.run();
        }

        public override Status run(string[] argv, TestDescription td, TestEnvironment env)
        {
            return base.run(argv, td, env);
        }

    }

}
