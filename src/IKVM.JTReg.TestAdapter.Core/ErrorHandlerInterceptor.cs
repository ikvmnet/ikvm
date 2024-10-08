using System;
using System.Linq;
using System.Reflection;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
    /// </summary>
    public class ErrorHandlerInterceptor : DispatchProxy
    {

        static readonly MethodInfo CreateMethodInfo = typeof(DispatchProxy).GetMethods()
            .Where(i => i.Name == "Create")
            .Where(i => i.GetGenericArguments().Length == 2)
            .First();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.TestFinder$ErrorHandler'.
        /// </summary>
        /// <returns></returns>
        public static ErrorHandlerInterceptor Create(IJTRegLoggerContext logger)
        {
            var proxy = (ErrorHandlerInterceptor)CreateMethodInfo.MakeGenericMethod(JTRegTypes.TestFinder.ErrorHandler.Type, typeof(ErrorHandlerInterceptor)).Invoke(null, []);
            proxy.SetLogger(logger);
            return proxy;
        }

        IJTRegLoggerContext logger;

        /// <summary>
        /// Sets the logger instance.
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(IJTRegLoggerContext logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Intercepts method calls to the underlying type.
        /// </summary>
        /// <param name="targetMethod"></param>
        /// <param name="args"></param>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            switch (targetMethod.Name)
            {
                case "error":
                    error((string)args[0]);
                    break;
            }

            return null;
        }

        void error(string msg)
        {
            logger.SendMessage(JTRegTestMessageLevel.Error, msg);
        }

    }

}
