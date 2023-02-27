package sun.nio.fs;

import java.nio.file.attribute.*;
import java.util.*;
import java.io.IOException;

class DotNetDosFileAttributeView extends DotNetBasicFileAttributeView implements DosFileAttributeView {

    private static final String READONLY_NAME = "readonly";
    private static final String ARCHIVE_NAME = "archive";
    private static final String SYSTEM_NAME = "system";
    private static final String HIDDEN_NAME = "hidden";
    private static final String ATTRIBUTES_NAME = "attributes";

    // the names of the DOS attribtues (includes basic)
    static final Set<String> dosAttributeNames = Util.newSet(basicAttributeNames, READONLY_NAME, ARCHIVE_NAME, SYSTEM_NAME,  HIDDEN_NAME, ATTRIBUTES_NAME);

    public DotNetDosFileAttributeView(DotNetPath path, boolean followLinks) {
        super(path, followLinks);
    }

    @Override
    public String name() {
        return "dos";
    }

    @Override
    public void setAttribute(String attribute, Object value) throws IOException {
        if (attribute.equals(READONLY_NAME)) {
            setReadOnly((Boolean)value);
            return;
        }

        if (attribute.equals(ARCHIVE_NAME)) {
            setArchive((Boolean)value);
            return;
        }

        if (attribute.equals(SYSTEM_NAME)) {
            setSystem((Boolean)value);
            return;
        }

        if (attribute.equals(HIDDEN_NAME)) {
            setHidden((Boolean)value);
            return;
        }

        super.setAttribute(attribute, value);
    }

    @Override
    public Map<String,Object> readAttributes(String[] attributes) throws IOException {
        AttributesBuilder builder = AttributesBuilder.create(dosAttributeNames, attributes);
        DosFileAttributes attrs = readAttributes();

        addRequestedBasicAttributes(attrs, builder);
        if (builder.match(READONLY_NAME))
            builder.add(READONLY_NAME, attrs.isReadOnly());
        if (builder.match(ARCHIVE_NAME))
            builder.add(ARCHIVE_NAME, attrs.isArchive());
        if (builder.match(SYSTEM_NAME))
            builder.add(SYSTEM_NAME, attrs.isSystem());
        if (builder.match(HIDDEN_NAME))
            builder.add(HIDDEN_NAME, attrs.isHidden());
        if (builder.match(ATTRIBUTES_NAME))
            builder.add(ATTRIBUTES_NAME, attrs.attributes());

        return builder.unmodifiableMap();
    }

    @Override
    public native void setReadOnly(boolean value) throws IOException;

    @Override
    public native void setHidden(boolean value) throws IOException;

    @Override
    public native void setArchive(boolean value) throws IOException;

    @Override
    public native void setSystem(boolean value) throws IOException;

}
