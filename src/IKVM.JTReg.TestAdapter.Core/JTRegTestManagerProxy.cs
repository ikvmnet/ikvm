using System;
using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    public class JTRegTestManagerProxy :
#if NETFRAMEWORK
        MarshalByRefObject,
#endif
        IJTRegTestManager
    {

        readonly IJTRegTestManager real;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JTRegTestManagerProxy()
        {
            this.real = new JTRegTestManager();
        }

        public void DiscoverTests(List<string> sources, IJTRegDiscoveryContext context)
        {
            real.DiscoverTests(sources, context);
        }

        public void RunTests(List<JTRegTestCase> tests, IJTRegExecutionContext context)
        {
            real.RunTests(tests, context);
        }

        public void RunTests(List<string> sources, IJTRegExecutionContext context)
        {
            real.RunTests(sources, context);
        }

        public void Cancel()
        {
            real.Cancel();
        }

    }

}
