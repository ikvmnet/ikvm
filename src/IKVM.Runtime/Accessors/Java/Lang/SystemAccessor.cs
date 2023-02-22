using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.System' type.
    /// </summary>
    internal sealed class SystemAccessor : Accessor
    {

        StaticFieldAccessor<object> @in;
        StaticFieldAccessor<object> @out;
        StaticFieldAccessor<object> @err;
        StaticMethodAccessor<Action> initializeSystemClass;
        StaticMethodAccessor<Func<string, string>> getProperty;
        StaticMethodAccessor<Action<string, string>> setProperty;

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
        public void SetIn(object value) => GetStaticField(ref @in, nameof(@in)).SetValue(value);

        /// <summary>
        /// Sets the value of the 'out' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetOut(object value) => GetStaticField(ref @out, nameof(@out)).SetValue(value);

        /// <summary>
        /// Sets the value of the 'err' field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetErr(object value) => GetStaticField(ref @err, nameof(@err)).SetValue(value);

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
        public void InvokeSetProperty(string key, string value) => GetStaticVoidMethod(ref setProperty, nameof(setProperty)).Invoker(key, value);

    }

}
