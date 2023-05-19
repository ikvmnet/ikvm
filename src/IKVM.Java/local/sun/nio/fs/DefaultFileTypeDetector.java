package sun.nio.fs;

import java.io.IOException;
import java.nio.file.FileSystems;
import java.nio.file.Path;
import java.nio.file.spi.FileTypeDetector;
import java.nio.file.spi.FileSystemProvider;

public class DefaultFileTypeDetector {

    public static FileTypeDetector create() {
        return new AbstractFileTypeDetector() {
            public String implProbeContentType(Path obj) throws IOException {
                return null;
            }
        };

        //FileSystemProvider provider = FileSystems.getDefault().provider();
        //if (provider instanceof UnixFileSystemProvider) {
        //    return ((UnixFileSystemProvider)provider).getFileTypeDetector();
        //} else if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
        //    return new RegistryFileTypeDetector();
        //} else {
        //    return new AbstractFileTypeDetector() {
        //        public String implProbeContentType(Path obj) throws IOException {
        //            return null;
        //        }
        //    };
        //}
    }

}
