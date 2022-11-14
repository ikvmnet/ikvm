using System;

using Castle.DynamicProxy;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.Harness$Observer'.
    /// </summary>
    class HarnessObserverInterceptor : IInterceptor
    {

        static readonly ProxyGenerator DefaultProxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.Harness$Observer'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(HarnessObserverImplementation implementation)
        {
            if (implementation is null)
                throw new ArgumentNullException(nameof(implementation));

            return DefaultProxyGenerator.CreateInterfaceProxyWithoutTarget(JTRegTypes.Harness.Observer.Type, new HarnessObserverInterceptor(implementation));
        }

        readonly HarnessObserverImplementation implementation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="implementation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HarnessObserverInterceptor(HarnessObserverImplementation implementation)
        {
            this.implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        /// <summary>
        /// Gets the underlying type instance.
        /// </summary>
        public HarnessObserverImplementation Implementation => implementation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "startingTestRun":
                    implementation.startingTestRun(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "startingTest":
                    implementation.startingTest(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "error":
                    implementation.error(invocation.Proxy, (string)invocation.GetArgumentValue(0));
                    break;
                case "stoppingTestRun":
                    implementation.stoppingTestRun(invocation.Proxy);
                    break;
                case "finishedTest":
                    implementation.finishedTest(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "finishedTesting":
                    implementation.finishedTesting(invocation.Proxy);
                    break;
                case "finishedTestRun":
                    implementation.finishedTestRun(invocation.Proxy, (bool)invocation.GetArgumentValue(0));
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

    }

}
