namespace IKVM.JTReg.TestAdapter.Core
{

    public interface IJTRegDiscoveryContext : IJTRegLoggerContext
    {

        void SendTestCase(JTRegTestCase testCase);

    }

}
