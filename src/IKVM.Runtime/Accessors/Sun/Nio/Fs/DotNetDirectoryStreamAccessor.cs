using System;
using System.Collections.Generic;

namespace IKVM.Runtime.Accessors.Sun.Nio.Fs
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal sealed class DotNetDirectoryStreamAccessor : Accessor<object>
    {

        Type sunNioFsDotNetPath;
        Type javaNioFileDirectoryStreamFilter;

        MethodAccessor<Func<object, IEnumerable<string>, object, object>> init;
        FieldAccessor<object, string> path;
        FieldAccessor<object, IEnumerable<string>> files;
        FieldAccessor<object, object> filter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetDirectoryStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.fs.DotNetDirectoryStream")
        {

        }

        Type SunNioFsDotNetPath => Resolve(ref sunNioFsDotNetPath, "sun.nio.fs.DotNetPath");

        Type JavaNioFileDirectoryStreamFilter => Resolve(ref javaNioFileDirectoryStreamFilter, "java.nio.file.DirectoryStream+Filter");

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="files"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object Init(object path, IEnumerable<string> files, object filter) => GetConstructor(ref init, SunNioFsDotNetPath, typeof(System.Collections.IEnumerable), JavaNioFileDirectoryStreamFilter).Invoker(path, files, filter);

        /// <summary>
        /// Gets the value of the 'path' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetPath(object self) => GetField(ref path, nameof(path)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'files' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFiles(object self) => GetField(ref files, nameof(files)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'filter' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetFilter(object self) => GetField(ref filter, nameof(filter)).GetValue(self);

    }

#endif

}