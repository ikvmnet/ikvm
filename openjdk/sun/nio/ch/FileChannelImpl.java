package sun.nio.ch;

import java.io.*;

public final class FileChannelImpl
{
    public static java.nio.channels.FileChannel open(FileDescriptor fd, boolean readable, boolean writable, Object parent)
    {
	return gnu.java.nio.FileChannelImpl.create(fd.getStream());
    }

    public static java.nio.channels.FileChannel open(FileDescriptor fd, boolean readable, boolean writable, Object parent, boolean append)
    {
	return gnu.java.nio.FileChannelImpl.create(fd.getStream());
    }
}
