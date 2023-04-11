using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.fs.DotNetDosFileAttributes' type.
    /// </summary>
    internal sealed class DotNetDosFileAttributesAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object, object, object, bool, bool, bool, bool, long, bool, bool, bool, bool, object>> init;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetDosFileAttributesAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.fs.DotNetDosFileAttributes")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public object Init(object creationTime, object lastAccessTime, object lastModifiedTime, object fileKey, bool isDirectory, bool isOther, bool isRegularFile, bool isSymbolicLink, long size, bool isReadOnly, bool isHidden, bool isArchive, bool isSystem) => GetConstructor(ref init, Resolve("java.nio.file.attribute.FileTime"), Resolve("java.nio.file.attribute.FileTime"), Resolve("java.nio.file.attribute.FileTime"), typeof(object), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(long), typeof(bool), typeof(bool), typeof(bool), typeof(bool)).Invoker(creationTime, lastAccessTime, lastModifiedTime, fileKey, isDirectory, isOther, isRegularFile, isSymbolicLink, size, isReadOnly, isHidden, isArchive, isSystem);

    }

#endif

}
