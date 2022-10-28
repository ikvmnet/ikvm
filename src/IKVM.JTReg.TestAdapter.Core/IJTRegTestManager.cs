using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    public interface IJTRegTestManager
    {

        void DiscoverTests(List<string> sources, IJTRegDiscoveryContext context);

        void RunTests(List<JTRegTestCase> tests, IJTRegExecutionContext context);

        void RunTests(List<string> sources, IJTRegExecutionContext context);

        void Cancel();


    }

}
