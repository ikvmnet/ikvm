using System;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Proxied implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
    /// </summary>
    class ErrorHandlerImplementation
    {

        readonly IJTRegLoggerContext logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorHandlerImplementation(IJTRegLoggerContext logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void error(dynamic proxy, string msg)
        {
            logger.SendMessage(JTRegTestMessageLevel.Error, msg);
        }

    }

}
