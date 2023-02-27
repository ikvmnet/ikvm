package sun.nio.fs;

import java.nio.file.attribute.*;
import java.util.*;
import java.io.IOException;

class DotNetUnixFileAttributeView extends DotNetPosixFileAttributeView {

    private static final String MODE_NAME = "mode";
    private static final String INO_NAME = "ino";
    private static final String DEV_NAME = "dev";
    private static final String RDEV_NAME = "rdev";
    private static final String NLINK_NAME = "nlink";
    private static final String UID_NAME = "uid";
    private static final String GID_NAME = "gid";
    private static final String CTIME_NAME = "ctime";
    
    // the names of the unix attributes (including posix)
    static final Set<String> unixAttributeNames = Util.newSet(posixAttributeNames, MODE_NAME, INO_NAME, DEV_NAME, RDEV_NAME, NLINK_NAME, UID_NAME, GID_NAME, CTIME_NAME);

    DotNetUnixFileAttributeView(DotNetPath path, boolean followLinks) {
        super(path, followLinks);
    }

    @Override
    public String name() {
        return "unix";
    }

    @Override
    public void setAttribute(String attribute, Object value) throws IOException {
        if (attribute.equals(MODE_NAME)) {
            setMode((Integer)value);
            return;
        }

        if (attribute.equals(UID_NAME)) {
            setOwners((Integer)value, -1);
            return;
        }

        if (attribute.equals(GID_NAME)) {
            setOwners(-1, (Integer)value);
            return;
        }

        super.setAttribute(attribute, value);
    }

    @Override
    public Map<String, Object> readAttributes(String[] requested) throws IOException {
        AttributesBuilder builder = AttributesBuilder.create(unixAttributeNames, requested);
        UnixFileAttributes attrs = (UnixFileAttributes)readAttributes();

        addRequestedPosixAttributes(attrs, builder);

        if (builder.match(MODE_NAME))
            builder.add(MODE_NAME, attrs.mode());
        if (builder.match(INO_NAME))
            builder.add(INO_NAME, attrs.ino());
        if (builder.match(DEV_NAME))
            builder.add(DEV_NAME, attrs.dev());
        if (builder.match(RDEV_NAME))
            builder.add(RDEV_NAME, attrs.rdev());
        if (builder.match(NLINK_NAME))
            builder.add(NLINK_NAME, attrs.nlink());
        if (builder.match(UID_NAME))
            builder.add(UID_NAME, attrs.uid());
        if (builder.match(GID_NAME))
            builder.add(GID_NAME, attrs.gid());
        if (builder.match(CTIME_NAME))
            builder.add(CTIME_NAME, attrs.ctime());

        return builder.unmodifiableMap();
    }

}
