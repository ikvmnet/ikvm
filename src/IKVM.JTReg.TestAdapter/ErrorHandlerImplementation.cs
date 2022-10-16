using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{
    /// <summary>
    /// Proxied implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
    /// </summary>
    class ErrorHandlerImplementation
    {

        readonly IMessageLogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorHandlerImplementation(IMessageLogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void error(dynamic proxy, string msg)
        {
            logger.SendMessage(TestMessageLevel.Error, msg);
        }

    }

}
