package ikvm.internal.io;

import cli.System.IO.FileAccess;
import cli.System.IO.FileMode;
import cli.System.IO.FileOptions;
import cli.System.IO.FileShare;
import cli.System.IO.FileStream;
import cli.System.Security.AccessControl.FileSystemRights;

import ikvm.lang.Internal;

@Internal
public final class FileStreamExtensions
{

    public static FileStream create(String path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options) throws java.io.IOException
    {
        return createInternal(path, mode, rights, share, bufferSize, options);
    }

    static native FileStream createInternal(String path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options) throws java.io.IOException;

}
