using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ClassLoader' type.
    /// </summary>
    internal sealed class ClassLoaderAccessor : Accessor<object>
    {

        Type javaLangClass;
        Type javaLangClassLoader;
        Type javaSecurityProtectionDomain;

        FieldAccessor<object> scl;
        FieldAccessor<object> systemNativeLibraries;

        FieldAccessor<object, object> parent;
        FieldAccessor<object, object> nativeLibraries;
        MethodAccessor<Action<object, object, object>> checkPackageAccess;
        MethodAccessor<Func<object, string, object>> loadClassInternal;
        MethodAccessor<Func<object, string, bool>> checkName;
        MethodAccessor<Func<object, string, long>> findNative;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ClassLoaderAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ClassLoader")
        {

        }

        Type JavaLangClass => Resolve(ref javaLangClass, "java.lang.Class");

        Type JavaLangClassLoader => Resolve(ref javaLangClassLoader, "java.lang.ClassLoader");

        Type JavaSecurityProtectionDomain => Resolve(ref javaSecurityProtectionDomain, "java.security.ProtectionDomain");

        /// <summary>
        /// Gets the value for the 'scl' field.
        /// </summary>
        public object GetScl() => GetField(ref scl, nameof(scl)).GetValue();

        /// <summary>
        /// Sets the value for the 'scl' field.
        /// </summary>
        /// <param name="value"></param>
        public void SetScl(object value) => GetField(ref scl, nameof(scl)).SetValue(value);

        /// <summary>
        /// Gets the value for the 'systemNativeLibraries' field.
        /// </summary>
        public object GetSystemNativeLibraries() => GetField(ref systemNativeLibraries, nameof(systemNativeLibraries)).GetValue();

        /// <summary>
        /// Gets the value for the 'parent' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetParent(object self) => GetField(ref parent, nameof(parent)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'parent' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetParent(object self, object value) => GetField(ref parent, nameof(parent)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'nativeLibraries' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetNativeLibraries(object self) => GetField(ref nativeLibraries, nameof(nativeLibraries)).GetValue(self);

        /// <summary>
        /// Invokes the 'checkPackageAccess' method.
        /// </summary>
        public void InvokeCheckPackageAccess(object self, object cls, object pd) => GetMethod(ref checkPackageAccess, nameof(checkPackageAccess), typeof(void), JavaLangClass, JavaSecurityProtectionDomain).Invoker(self, cls, pd);

        /// <summary>
        /// Invokes the 'checkName' method.
        /// </summary>
        public bool InvokeCheckName(object self, string name) => GetMethod(ref checkName, nameof(checkName), typeof(bool), typeof(string)).Invoker(self, name);

        /// <summary>
        /// Invokes the 'loadClassInternal' method.
        /// </summary>
        public object InvokeLoadClassInternal(object self, string name) => GetMethod(ref loadClassInternal, nameof(loadClassInternal), JavaLangClass, typeof(string)).Invoker(self, name);

        /// <summary>
        /// Invokes the 'findNatve' method.
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public long InvokeFindNative(object loader, string name) => GetMethod(ref findNative, nameof(findNative), typeof(long), JavaLangClassLoader, typeof(string)).Invoker(loader, name);

    }

#endif

}
