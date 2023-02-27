package sun.nio.fs;

import java.nio.file.attribute.*;
import java.util.*;
import java.io.IOException;

class DotNetBasicFileAttributeView extends AbstractBasicFileAttributeView {

    final DotNetPath path;
    final boolean followLinks;

    public DotNetBasicFileAttributeView(DotNetPath path, boolean followLinks) {
        this.path = path;
        this.followLinks = followLinks;
    }

    @Override
    public native void setTimes(FileTime lastModifiedTime, FileTime lastAccessTime, FileTime createTime) throws IOException;

    @Override
    public native BasicFileAttributes readAttributes() throws IOException;

}
