using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.System' type.
    /// </summary>
    internal sealed class SystemAccessor : Accessor
    {

        StaticFieldAccessor<object> _in;
        StaticFieldAccessor<object> _out;
        StaticFieldAccessor<object> _err;
        StaticMethodAccessor<Action> initializeSystemClass;
        StaticMethodAccessor<Func<string, string>> getProperty;
        StaticMethodAccessor<Func<string, string, string>> setProperty;
        StaticMethodAccessor<Func<object>> getSecurityManager;

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
        public void SetIn(object value) => GetStaticField(ref _in, nameof(_in)).SetValue(value);

        /// <summary>
        /// Sets the value of the 'out' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetOut(object value) => GetStaticField(ref _out, nameof(_out)).SetValue(value);

        /// <summary>
        /// Sets the value of the 'err' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetErr(object value) => GetStaticField(ref _err, nameof(_err)).SetValue(value);

        /// <summary>
        /// Sets the value of the 'initializeSystemClass' field.
        /// </summary>
        /// <returns></returns>
        public void InvokeInitializeSystemClass() => GetStaticVoidMethod(ref initializeSystemClass, nameof(initializeSystemClass)).Invoker();

        /// <summary>
        /// Sets the value of the 'getProperty' field.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string InvokeGetProperty(string key) => GetStaticMethod(ref getProperty, nameof(getProperty)).Invoker(key);

        /// <summary>
        /// Sets the value of the 'setProperty' field.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string InvokeSetProperty(string key, string value) => GetStaticMethod(ref setProperty, nameof(setProperty)).Invoker(key, value);

        /// <summary>
        /// Invokes the 'getSecurityManager' method.
        /// </summary>
        /// <returns></returns>
        public object InvokeGetSecurityManager() => GetStaticMethod(ref getSecurityManager, nameof(getSecurityManager)).Invoker();

    }

}
