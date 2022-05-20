using System.IO;
using System.Security.AccessControl;

static class Java_ikvm_internal_io_FileStreamExtensions
{

    public static FileStream createInternal(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
    {
#if NET461
        return new FileStream(path, mode, rights, share, bufferSize, options);
#else
        return FileSystemAclExtensions.Create(new FileInfo(path), mode, rights, share, bufferSize, options, null);
#endif
    }

}
