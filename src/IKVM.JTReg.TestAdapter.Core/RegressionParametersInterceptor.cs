using System;
using System.Collections.Generic;

using Castle.DynamicProxy;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.regtest.config.RegressionParameters'.
    /// </summary>
    class RegressionParametersInterceptor : IInterceptor
    {

        static readonly ProxyGenerator DefaultProxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.regtest.config.RegressionParameters'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(string defaultParamTag, object testSuite, Func<dynamic, bool> filterFunc)
        {
            return DefaultProxyGenerator.CreateClassProxy(JTRegTypes.RegressionParameters.Type, [defaultParamTag, testSuite], new RegressionParametersInterceptor(filterFunc));
        }

        readonly Func<dynamic, bool> filterFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="filterFunc"></param>
        public RegressionParametersInterceptor(Func<dynamic, bool> filterFunc)
        {
            this.filterFunc = filterFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "getRelevantTestFilter":
                    getRelevantTestFilter(invocation);
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

        void getRelevantTestFilter(IInvocation invocation)
        {
            invocation.Proceed();

            // generate set of filters, optionally adding a delegate filter
            var filters = new List<object>();
            filters.Add(invocation.ReturnValue);
            if (filterFunc != null)
                filters.Add(DelegateTestFilterInterceptor.Create(filterFunc));

            // copy set of filters to array
            var fa = Array.CreateInstance(JTRegTypes.TestFilter.Type, filters.Count);
            for (int i = 0; i < filters.Count; i++)
                fa.SetValue(filters[i], i);

            // return a composite filter of the assembled filters
            invocation.ReturnValue = JTRegTypes.CompositeFilter.New(fa);
        }

    }

}
