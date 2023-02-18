package sun.nio.fs;

import java.nio.file.spi.FileSystemProvider;

public class DefaultFileSystemProvider {

    public static FileSystemProvider create() {
        return new DotNetFileSystemProvider();
    }

}
