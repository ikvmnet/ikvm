using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Fs
{

    internal sealed class DotNetPathAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, string, object>> init;
        FieldAccessor<object, object> fs;
        FieldAccessor<object, string> path;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetPathAccessor(AccessorTypeResolver resolver) :
            base(resolver("sun.nio.fs.DotNetPath"))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public object Init(object fs, string path) => GetConstructor(ref init, "(Lsun.nio.fs.DotNetFileSystem;Ljava.lang.String;)V").Invoker(fs, path);

        /// <summary>
        /// Gets the value of the 'fs' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetFs(object self) => GetField(ref fs, nameof(fs), "Lsun.nio.fs.DotNetFileSystem;").GetValue(self);

        /// <summary>
        /// Gets the value of the 'path' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public string GetPath(object self) => GetField(ref path, nameof(path), "Ljava.lang.String;").GetValue(self);

    }

}
