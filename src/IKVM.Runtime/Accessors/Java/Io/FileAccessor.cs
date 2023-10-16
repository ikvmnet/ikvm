using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.File' type.
    /// </summary>
    internal sealed class FileAccessor : Accessor<object>
    {

        MethodAccessor<Func<string, object>> init;
        MethodAccessor<Func<object, string>> getPath;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.File")
        {

        }

        /// <summary>
        /// Creates a new instance of the object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object Init(string path) => GetConstructor(ref init, typeof(string)).Invoker(path);

        /// <summary>
        /// Invokes the 'getPath' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public string InvokeGetPath(object self) => GetMethod(ref getPath, nameof(getPath), typeof(string)).Invoker(self);

    }

#endif

}
