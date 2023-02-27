using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.System' type.
    /// </summary>
    internal sealed class SystemAccessor : Accessor<object>
    {

        FieldAccessor<object> _in;
        FieldAccessor<object> _out;
        FieldAccessor<object> _err;
        MethodAccessor<Action> initializeSystemClass;
        MethodAccessor<Func<string, string>> getProperty;
        MethodAccessor<Func<string, string, string>> setProperty;
        MethodAccessor<Func<object>> getSecurityManager;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public SystemAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.System"))
        {

        }

        /// <summary>
        /// Sets the value of the 'in' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetIn(object value) => GetField(ref _in, nameof(_in), "Ljava.io.InputStream;").SetValue(value);

        /// <summary>
        /// Sets the value of the 'out' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetOut(object value) => GetField(ref _out, nameof(_out), "Ljava.io.PrintStream;").SetValue(value);

        /// <summary>
        /// Sets the value of the 'err' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetErr(object value) => GetField(ref _err, nameof(_err), "Ljava.io.PrintStream;").SetValue(value);

        /// <summary>
        /// Sets the value of the 'initializeSystemClass' field.
        /// </summary>
        /// <returns></returns>
        public void InvokeInitializeSystemClass() => GetMethod(ref initializeSystemClass, nameof(initializeSystemClass), "()V").Invoker();

        /// <summary>
        /// Sets the value of the 'getProperty' field.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string InvokeGetProperty(string key) => GetMethod(ref getProperty, nameof(getProperty), "(Ljava.lang.String;)Ljava.lang.String;").Invoker(key);

        /// <summary>
        /// Sets the value of the 'setProperty' field.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string InvokeSetProperty(string key, string value) => GetMethod(ref setProperty, nameof(setProperty), "(Ljava.lang.String;Ljava.lang.String;)Ljava.lang.String;").Invoker(key, value);

        /// <summary>
        /// Invokes the 'getSecurityManager' method.
        /// </summary>
        /// <returns></returns>
        public object InvokeGetSecurityManager() => GetMethod(ref getSecurityManager, nameof(getSecurityManager), "()Ljava.lang.SecurityManager;").Invoker();

    }

}
