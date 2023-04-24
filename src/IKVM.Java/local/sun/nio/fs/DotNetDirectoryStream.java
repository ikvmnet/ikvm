package sun.nio.fs;

import java.util.*;
import java.nio.file.*;

final class DotNetDirectoryStream implements DirectoryStream<DotNetPath> {

    private final DotNetPath path;
    private final cli.System.Collections.IEnumerable files;
    private final DirectoryStream.Filter<DotNetPath> filter;

    DotNetDirectoryStream(final DotNetPath path, final cli.System.Collections.IEnumerable files, final DirectoryStream.Filter<DotNetPath> filter) {
        this.path = path;
        this.files = files;
        this.filter = filter;
    }

    @Override
    public native Iterator<DotNetPath> iterator();

    @Override
    public native void close();

}
