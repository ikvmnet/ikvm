using System;
using System.IO;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetDosFileAttributeView'.
    /// </summary>
    static class DotNetDosFileAttributeView
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;

        static SystemAccessor SystemAccessor => JVM.Internal.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.Internal.BaseAccessors.Get(ref securityManagerAccessor);

#endif

        /// <summary>
        /// Implements the native method 'setAttribute0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attr"></param>
        /// <param name="value"></param>
        public static void setAttribute0(string path, int attr, bool value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var sm = SystemAccessor.InvokeGetSecurityManager();
            if (sm != null)
                SecurityManagerAccessor.InvokeCheckWrite(sm, path);

            if (JVM.Vfs.IsPath(path))
                throw new global::java.io.IOException("VFS entries cannot be modified.");

            try
            {
                var info = new FileInfo(path);

                if (value)
                    info.Attributes |= (FileAttributes)attr;
                else
                    info.Attributes &= ~(FileAttributes)attr;
            }
            catch (FileNotFoundException e)
            {
                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (ArgumentException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (IOException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

    }

}
