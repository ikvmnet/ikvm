using System;

using com.sun.javatest;

namespace IKVM.JavaTest
{

    /// <summary>
    /// Implementation of <see cref="TestFinder.ErrorHandler"/> that invokes a delegate.
    /// </summary>
    class DelegateErrorHandler : TestFinder.ErrorHandler
    {

        readonly Action<string> onError;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onError"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateErrorHandler(Action<string> onError)
        {
            this.onError = onError ?? throw new ArgumentNullException(nameof(onError));
        }

        public void error(string str)
        {
            onError(str);
        }

    }

}
