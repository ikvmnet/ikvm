namespace IKVM.JTReg.TestAdapter.Core
{

    public interface IJTRegDiscoveryContext : IJTRegLoggerContext
    {

        /// <summary>
        /// Gets the options applied to the tests.
        /// </summary>
        JTRegTestOptions Options { get; }

        void SendTestCase(JTRegTestCase testCase);

    }

}
