using System;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

namespace IKVM.JTReg.TestAdapter.Core
{

    class DelegateTestFilterInterceptor : IInterceptor
    {

        static readonly ProxyGenerator DefaultProxyGenerator = new ProxyGenerator();
        static readonly MethodInfo AcceptsMethod = JTRegTypes.TestFilter.Type.GetMethods().Where(i => i.Name == "accepts" && i.IsAbstract).First();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.Harness$Observer'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(Func<dynamic, bool> func)
        {
            return DefaultProxyGenerator.CreateClassProxy(JTRegTypes.TestFilter.Type, Array.Empty<object>(), new DelegateTestFilterInterceptor(func));
        }

        readonly Func<dynamic, bool> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        public DelegateTestFilterInterceptor(Func<dynamic, bool> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>
        /// Invoked for any operation on the original type.
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "getName":
                    getName(invocation);
                    break;
                case "getDescription":
                    getDescription(invocation);
                    break;
                case "getReason":
                    getReason(invocation);
                    break;
                case "accepts" when invocation.Method == AcceptsMethod:
                    accepts(invocation, invocation.GetArgumentValue(0));
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

        void getName(IInvocation invocation)
        {
            invocation.ReturnValue = "";
        }

        void getDescription(IInvocation invocation)
        {
            invocation.ReturnValue = "";
        }

        void getReason(IInvocation invocation)
        {
            invocation.ReturnValue = "";
        }

        void accepts(IInvocation invocation, dynamic td)
        {
            invocation.ReturnValue = func(td);
        }

    }

}
