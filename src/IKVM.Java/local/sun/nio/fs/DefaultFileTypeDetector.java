package sun.nio.fs;

import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.spi.FileTypeDetector;

public class DefaultFileTypeDetector {

    public static FileTypeDetector create()     {
        return new AbstractFileTypeDetector() {
            public String implProbeContentType(Path obj) throws IOException {
                return null;
            }
        };
    }

}
