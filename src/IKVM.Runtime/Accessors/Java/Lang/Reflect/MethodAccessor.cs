using System;

namespace IKVM.Runtime.Accessors.Java.Lang.Reflect
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.reflect.Method' type.
    /// </summary>
    internal sealed class MethodAccessor : Accessor<object>
    {

        MethodAccessor<Action<object, bool>> setAccessible;
        MethodAccessor<Func<object, object, object[], object>> invoke;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public MethodAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.reflect.Method")
        {

        }

        /// <summary>
        /// Invokes the 'setAccessible' method.
        /// </summary>
        public void InvokeSetAccessible(object self, bool flag) => GetMethod(ref setAccessible, nameof(setAccessible), typeof(void), typeof(bool)).Invoker(self, flag);

        /// <summary>
        /// Invokes the 'invoke' method.
        /// </summary>
        public void InvokeInvoke(object self, object obj, object[] args) => GetMethod(ref invoke, nameof(invoke), typeof(object), typeof(object), typeof(object[])).Invoker(self, obj, args);

    }

#endif

}
