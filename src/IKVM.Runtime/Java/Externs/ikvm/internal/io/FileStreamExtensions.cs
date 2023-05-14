using System.IO;
using System.Security.AccessControl;

namespace IKVM.Java.Externs.ikvm.@internal.io
{

    static class FileStreamExtensions
    {

        public static FileStream createInternal(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
        {
#if NETFRAMEWORK
            return new FileStream(path, mode, rights, share, bufferSize, options);
#else
            var access = 0;
            if ((rights & FileSystemRights.Read) != 0)
                access |= (int)FileAccess.Read;
            if ((rights & (FileSystemRights.Write | FileSystemRights.AppendData)) != 0)
                access |= (int)FileAccess.Write;
            if (access == 0)
                access = (int)FileAccess.ReadWrite;

            return new FileStream(path, mode, (FileAccess)access, share, bufferSize, options);
#endif
        }

    }

}
