using System;
using System.Threading;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Proxy to a <see cref="CancellationToken"/> that can be marshalled by reference.
    /// </summary>
    public class CancellationTokenCancellationProxy : MarshalByRefObject
    {

        readonly CancellationTokenSource cts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="proxy"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CancellationTokenCancellationProxy()
        {
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Cancels the token.
        /// </summary>
        public void Cancel() => cts.Cancel();

        /// <summary>
        /// Underlying local token.
        /// </summary>
        public CancellationToken Token => cts.Token;

    }

}
