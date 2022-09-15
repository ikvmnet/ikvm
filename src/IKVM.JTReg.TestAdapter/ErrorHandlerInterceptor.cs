using System;
using System.Reflection;
using System.Reflection.Emit;

using Castle.DynamicProxy;

using com.sun.xml.@internal.ws.message;

namespace IKVM.JTReg.TestAdapter
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
        public static dynamic Create(ErrorHandlerImplementation implementation)
        {
            if (implementation is null)
                throw new ArgumentNullException(nameof(implementation));

            return DefaultProxyGenerator.CreateInterfaceProxyWithoutTarget(JTRegTypes.TestFinder.ErrorHandler.Type, new ErrorHandlerInterceptor(implementation));
        }

        readonly ErrorHandlerImplementation implementation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="implementation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorHandlerInterceptor(ErrorHandlerImplementation implementation)
        {
            this.implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        /// <summary>
        /// Gets the underlying type instance.
        /// </summary>
        public ErrorHandlerImplementation Implementation => implementation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "error":
                    implementation.error(invocation.Proxy, (string)invocation.GetArgumentValue(0));
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

    }

}
