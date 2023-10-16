using System;

using Castle.DynamicProxy;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
    /// </summary>
    class ErrorHandlerInterceptor : IInterceptor
    {

        static readonly ProxyGenerator DefaultProxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(IJTRegLoggerContext logger)
        {
            return DefaultProxyGenerator.CreateInterfaceProxyWithoutTarget(JTRegTypes.TestFinder.ErrorHandler.Type, new ErrorHandlerInterceptor(logger));
        }

        readonly IJTRegLoggerContext logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorHandlerInterceptor(IJTRegLoggerContext logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "error":
                    error(invocation.Proxy, (string)invocation.GetArgumentValue(0));
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

        public void error(dynamic proxy, string msg)
        {
            logger.SendMessage(JTRegTestMessageLevel.Error, msg);
        }

    }

}
