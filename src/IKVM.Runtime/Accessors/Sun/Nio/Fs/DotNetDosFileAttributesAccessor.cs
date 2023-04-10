using System;
using System.IO;

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
            base(resolver("sun.nio.fs.DotNetDosFileAttributes"))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public object Init(object creationTime, object lastAccessTime, object lastModifiedTime, object fileKey, bool isDirectory, bool isOther, bool isRegularFile, bool isSymbolicLink, long size, bool isReadOnly, bool isHidden, bool isArchive, bool isSystem) => GetConstructor(ref init, "(Ljava.nio.file.attribute.FileTime;Ljava.nio.file.attribute.FileTime;Ljava.nio.file.attribute.FileTime;Ljava.lang.Object;ZZZZJZZZZ)V").Invoker(creationTime, lastAccessTime, lastModifiedTime, fileKey, isDirectory, isOther, isRegularFile, isSymbolicLink, size, isReadOnly, isHidden, isArchive, isSystem);

    }

#endif

}
