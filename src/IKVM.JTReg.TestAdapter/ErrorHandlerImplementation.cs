using System;
using System.IO;

namespace IKVM.JTReg.TestAdapter
{
    /// <summary>
    /// Proxied implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
    /// </summary>
    class ErrorHandlerImplementation
    {

        readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorHandlerImplementation(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void error(dynamic proxy, string msg)
        {
            writer.WriteLine(msg);
        }

    }

}
