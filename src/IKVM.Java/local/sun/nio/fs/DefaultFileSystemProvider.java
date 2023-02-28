package sun.nio.fs;

import java.nio.file.spi.FileSystemProvider;

public class DefaultFileSystemProvider {

    public static FileSystemProvider create() {
        return new DotNetFileSystemProvider();

        //if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
        //    return new WindowsFileSystemProvider();
        //} else if (cli.IKVM.Runtime.RuntimeUtil.get_IsLinux()) {
        //    return new LinuxFileSystemProvider();
        //} else {
        //    return new DotNetFileSystemProvider();
        //}
    }

}
