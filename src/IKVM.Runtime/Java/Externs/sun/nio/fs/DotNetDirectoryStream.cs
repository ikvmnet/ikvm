using System;
using System.IO;
using System.Linq;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Ikvm.Util;
using IKVM.Runtime.Accessors.Java.Nio.File;
using IKVM.Runtime.Accessors.Sun.Nio.Fs;


namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetDirectoryStream'.
    /// </summary>
    static class DotNetDirectoryStream
    {

#if FIRST_PASS == false

        static EnumeratorIteratorAccessor enumeratorIteratorAccessor;
        static DirectoryStreamFilterAccessor directoryStreamFilterAccessor;
        static DotNetPathAccessor dotNetPathAccessor;
        static DotNetDirectoryStreamAccessor dotNetDirectoryStreamAccessor;

        static EnumeratorIteratorAccessor EnumeratorIteratorAccessor => JVM.BaseAccessors.Get(ref enumeratorIteratorAccessor);

        static DirectoryStreamFilterAccessor DirectoryStreamFilterAccessor => JVM.BaseAccessors.Get(ref directoryStreamFilterAccessor);

        static DotNetPathAccessor DotNetPathAccessor => JVM.BaseAccessors.Get(ref dotNetPathAccessor);

        static DotNetDirectoryStreamAccessor DotNetDirectoryStreamAccessor => JVM.BaseAccessors.Get(ref dotNetDirectoryStreamAccessor);

#endif

        /// <summary>
        /// Implements the native method 'iterator'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static object iterator(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var path = DotNetDirectoryStreamAccessor.GetPath(self);
            if (path == null)
                throw new global::java.lang.NullPointerException();

            var fileSystem = DotNetPathAccessor.GetFs(path);
            if (fileSystem == null)
                throw new global::java.lang.NullPointerException();

            var directoryPath = DotNetPathAccessor.GetPath(path);
            if (directoryPath == null)
                throw new global::java.lang.NullPointerException();

            var files = DotNetDirectoryStreamAccessor.GetFiles(self);
            if (files == null)
                throw new global::java.lang.NullPointerException();

            var filter = DotNetDirectoryStreamAccessor.GetFilter(self);
            if (filter == null)
                throw new global::java.lang.NullPointerException();

            try
            {
                bool ApplyFilter(object i)
                {
                    try
                    {
                        return DirectoryStreamFilterAccessor.InvokeAccept(filter, i);
                    }
                    catch (global::java.io.IOException e)
                    {
                        throw new global::java.nio.file.DirectoryIteratorException(e);
                    }
                }

                return EnumeratorIteratorAccessor.Init(files.Select(i => DotNetPathAccessor.Init(fileSystem, Path.Combine(directoryPath, i))).Where(ApplyFilter).GetEnumerator());
            }
            catch (global::java.lang.Throwable)
            {
                throw;
            }
            catch (Exception e)
            {
                if (File.Exists(directoryPath))
                    throw new global::java.nio.file.NotDirectoryException(directoryPath);
                if (Directory.Exists(directoryPath) == false)
                    throw new global::java.nio.file.NotDirectoryException(directoryPath);

                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

    }

}
