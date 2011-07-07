package sun.management;

import ikvm.internal.NotYetImplementedError;
import java.io.File;

public class FileSystemImpl extends FileSystem
{
    public boolean supportsFileSecurity(File f)
    {
        throw new NotYetImplementedError();
    }

    public boolean isAccessUserOnly(File f)
    {
        throw new NotYetImplementedError();
    }
}
