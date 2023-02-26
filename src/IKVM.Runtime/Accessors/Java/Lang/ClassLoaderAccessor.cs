using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.ClassLoader' type.
    /// </summary>
    internal sealed class ClassLoaderAccessor : Accessor<object>
    {

        FieldAccessor<object> scl;

        FieldAccessor<object, object> parent;
        MethodAccessor<Action<object, object, object>> checkPackageAccess;
        MethodAccessor<Func<object, string, object>> loadClassInternal;
        MethodAccessor<Func<object, string, bool>> checkName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ClassLoaderAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.ClassLoader"))
        {

        }

        /// <summary>
        /// Gets the value for the 'scl' field.
        /// </summary>
        public object GetScl() => GetField(ref scl, nameof(scl), "Ljava.lang.ClassLoader;").GetValue();

        /// <summary>
        /// Sets the value for the 'scl' field.
        /// </summary>
        public void SetScl(object value) => GetField(ref scl, nameof(scl), "Ljava.lang.ClassLoader;").SetValue(value);

        /// <summary>
        /// Gets the value for the 'parent' field.
        /// </summary>
        public object GetParent(object self) => GetField(ref parent, nameof(parent), "Ljava.lang.ClassLoader;").GetValue(self);

        /// <summary>
        /// Sets the value for the 'parent' field.
        /// </summary>
        public void SetParent(object self, object value) => GetField(ref parent, nameof(parent), "Ljava.lang.ClassLoader;").SetValue(self, value);

        /// <summary>
        /// Invokes the 'checkPackageAccess' method.
        /// </summary>
        public void InvokeCheckPackageAccess(object self, object cls, object pd) => GetMethod(ref checkPackageAccess, nameof(checkPackageAccess), "(Ljava.lang.Class;Ljava.lang.ProtectionDomain;)V").Invoker(self, cls, pd);

        /// <summary>
        /// Invokes the 'checkName' method.
        /// </summary>
        public bool InvokeCheckName(object self, string name) => GetMethod(ref checkName, nameof(checkName), "(Ljava.lang.String;)Z").Invoker(self, name);

        /// <summary>
        /// Invokes the 'loadClassInternal' method.
        /// </summary>
        public object InvokeLoadClassInternal(object self, string name) => GetMethod(ref loadClassInternal, nameof(loadClassInternal), "(Ljava.lang.String;)Ljava.lang.Class;").Invoker(self, name);

    }

}
