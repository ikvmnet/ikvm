package sun.nio.fs;

import java.nio.file.attribute.*;
import java.util.*;
import java.io.IOException;

class DotNetPosixFileAttributeView extends DotNetBasicFileAttributeView implements PosixFileAttributeView {

    private static final String PERMISSIONS_NAME = "permissions";
    private static final String OWNER_NAME = "owner";
    private static final String GROUP_NAME = "group";

    // the names of the posix attributes (incudes basic)
    static final Set<String> posixAttributeNames = Util.newSet(basicAttributeNames, PERMISSIONS_NAME, OWNER_NAME, GROUP_NAME);

    DotNetPosixFileAttributeView(DotNetPath path, boolean followLinks) {
        super(path, followLinks);
    }

    @Override
    public String name() {
        return "posix";
    }

    @Override
    @SuppressWarnings("unchecked")
    public void setAttribute(String attribute, Object value) throws IOException {
        if (attribute.equals(PERMISSIONS_NAME)) {
            setPermissions((Set<PosixFilePermission>)value);
            return;
        }

        if (attribute.equals(OWNER_NAME)) {
            setOwner((UserPrincipal)value);
            return;
        }

        if (attribute.equals(GROUP_NAME)) {
            setGroup((GroupPrincipal)value);
            return;
        }

        super.setAttribute(attribute, value);
    }

    final void addRequestedPosixAttributes(PosixFileAttributes attrs, AttributesBuilder builder) {
        addRequestedBasicAttributes(attrs, builder);

        if (builder.match(PERMISSIONS_NAME))
            builder.add(PERMISSIONS_NAME, attrs.permissions());

        if (builder.match(OWNER_NAME))
                builder.add(OWNER_NAME, attrs.owner());

        if (builder.match(GROUP_NAME))
            builder.add(GROUP_NAME, attrs.group());
    }

    @Override
    public Map<String, Object> readAttributes(String[] requested) throws IOException {
        AttributesBuilder builder = AttributesBuilder.create(posixAttributeNames, requested);
        PosixFileAttributes attrs = readAttributes();
        addRequestedPosixAttributes(attrs, builder);
        return builder.unmodifiableMap();
    }

    @Override
    public PosixFileAttributes readAttributes() throws IOException {
        return (PosixFileAttributes)super.readAttributes();
    }

    @Override
    public native void setOwner(UserPrincipal owner) throws IOException;

    @Override
    public native UserPrincipal getOwner() throws IOException;

    @Override
    public native void setGroup(GroupPrincipal group) throws IOException;

    @Override
    public native void setPermissions(Set<PosixFilePermission> perms) throws IOException;

}
