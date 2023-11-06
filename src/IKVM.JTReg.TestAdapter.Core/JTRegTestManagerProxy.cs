using System;
using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Proxy to a <see cref="JTRegTestManager"/> instance that can be marshalled by reference.
    /// </summary>
    public class JTRegTestManagerProxy :
#if NETFRAMEWORK
        MarshalByRefObject
#else
        object
#endif
    {

        readonly JTRegTestManager real = new JTRegTestManager();

        /// <summary>
        /// Initiates a discovery of the tests of the given source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <param name="cancellationTokenProxy"></param>
        public void DiscoverTests(string source, IJTRegDiscoveryContext context, CancellationTokenProxy cancellationTokenProxy)
        {
            var ctp = new CancellationTokenCancellationProxy();

            try
            {
                cancellationTokenProxy.Register(ctp);
                real.DiscoverTests(source, context, ctp.Token);
            }
            finally
            {
                cancellationTokenProxy.Unregister(ctp);
            }
        }

        /// <summary>
        /// Initiates a run of the given sources, optionally with the specified tests.
        /// </summary>
        /// <param name="csource"></param>
        /// <param name="context"></param>
        /// <param name="cancellationTokenProxy"></param>
        public void RunTests(string source, IJTRegExecutionContext context, CancellationTokenProxy cancellationTokenProxy)
        {
            RunTests(source, null, context, cancellationTokenProxy);
        }

        /// <summary>
        /// Initiates a run of the given source, optionally with the specified tests.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tests"></param>
        /// <param name="context"></param>
        /// <param name="cancellationTokenProxy"></param>
        public void RunTests(string source, List<JTRegTestCase> tests, IJTRegExecutionContext context, CancellationTokenProxy cancellationTokenProxy)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (cancellationTokenProxy is null)
                throw new ArgumentNullException(nameof(cancellationTokenProxy));

            var ctp = new CancellationTokenCancellationProxy();

            try
            {
                cancellationTokenProxy.Register(ctp);
                real.RunTests(source, tests, context, ctp.Token);
            }
            finally
            {
                cancellationTokenProxy.Unregister(ctp);
            }
        }

#if NETFRAMEWORK

        public override object InitializeLifetimeService()
        {
            return null;
        }

#endif

    }

}
