using System;

namespace IKVM.Runtime.Accessors.Java.Util
{

    /// <summary>
    /// Provides runtime access to the 'java.util.Properties' type.
    /// </summary>
    internal sealed class PropertiesAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, string, object>> getProperty;
        MethodAccessor<Func<object, string, string, object>> setProperty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public PropertiesAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.util.Properties"))
        {

        }

        /// <summary>
        /// Sets the value of the 'getProperty' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object InvokeGetProperty(object self, string key) => GetMethod(ref getProperty, nameof(getProperty), "(Ljava.lang.String;)Ljava.lang.Object;").Invoker(self, key);

        /// <summary>
        /// Sets the value of the 'setProperty' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object InvokeSetProperty(object self, string key, string value) => GetMethod(ref setProperty, nameof(setProperty), "(Ljava.lang.String;Ljava.lang.String;)Ljava.lang.Object;").Invoker(self, key, value);

    }

}
