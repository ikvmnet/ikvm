using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Makes a <see cref="CancellationToken"/> available across AppDomains.
    /// </summary>
    public class CancellationTokenProxy :
#if NETFRAMEWORK
        MarshalByRefObject
#else
        object
#endif
    {

        readonly CancellationToken token;
        readonly HashSet<CancellationTokenCancellationProxy> registered = new HashSet<CancellationTokenCancellationProxy>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="token"></param>
        public CancellationTokenProxy(CancellationToken token)
        {
            this.token = token;

            token.Register(OnCancel);
        }

        /// <summary>
        /// Invoked when the token is cancelled.
        /// </summary>
        void OnCancel()
        {
            lock (registered)
            {
                while (registered.Count > 0)
                {
                    var i = registered.First();

                    try
                    {
                        registered.Remove(i);
                        i.Cancel();
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        /// <summary>
        /// Returns <c>true</c> if cancellation has been requested.
        /// </summary>
        public bool IsCancellationRequested => token.IsCancellationRequested;

        /// <summary>
        /// Registers the given proxy to be signaled when the wrapped <see cref="CancellationToken"/> is cancelled.
        /// </summary>
        /// <param name="cancellationTokenCancellationProxy"></param>
        public void Register(CancellationTokenCancellationProxy cancellationTokenCancellationProxy)
        {
            lock (registered)
                registered.Add(cancellationTokenCancellationProxy);
        }

        /// <summary>
        /// Unregisters the given proxy.
        /// </summary>
        /// <param name="cancellationTokenCancellationProxy"></param>
        public void Unregister(CancellationTokenCancellationProxy cancellationTokenCancellationProxy)
        {
            lock (registered)
                registered.Remove(cancellationTokenCancellationProxy);
        }

    }

}
