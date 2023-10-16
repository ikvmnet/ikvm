package sun.nio.fs;

import java.io.*;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.util.*;

import cli.System.IO.FileInfo;

class DotNetDosFileAttributeView extends DotNetBasicFileAttributeView implements DosFileAttributeView {

    private static final String READONLY_NAME = "readonly";
    private static final String ARCHIVE_NAME = "archive";
    private static final String SYSTEM_NAME = "system";
    private static final String HIDDEN_NAME = "hidden";
    private static final Set<String> dosAttributeNames = Util.newSet(basicAttributeNames, READONLY_NAME, ARCHIVE_NAME, SYSTEM_NAME,  HIDDEN_NAME);

    DotNetDosFileAttributeView(String path) {
        super(path);
    }

    public String name() {
        return "dos";
    }

    public DosFileAttributes readAttributes() throws IOException {
        return DotNetDosFileAttributes.read(path);
    }

    public void setArchive(boolean value) throws IOException {
        setAttribute(cli.System.IO.FileAttributes.Archive, value);
    }

    public void setHidden(boolean value) throws IOException {
        setAttribute(cli.System.IO.FileAttributes.Hidden, value);
    }

    public void setReadOnly(boolean value) throws IOException {
        setAttribute(cli.System.IO.FileAttributes.ReadOnly, value);
    }

    public void setSystem(boolean value) throws IOException {
        setAttribute(cli.System.IO.FileAttributes.System, value);
    }

    private void setAttribute(int attr, boolean value) throws IOException {
        setAttribute0(path, attr, value);
    }

    private static native void setAttribute0(String path, int attr, boolean value) throws IOException;

    public Map<String, Object> readAttributes(String[] attributes) throws IOException {
        AttributesBuilder builder = AttributesBuilder.create(dosAttributeNames, attributes);
        DotNetDosFileAttributes attrs = DotNetDosFileAttributes.read(path);
        addRequestedBasicAttributes(attrs, builder);

        if (builder.match(READONLY_NAME)) {
            builder.add(READONLY_NAME, attrs.isReadOnly());
        }

        if (builder.match(ARCHIVE_NAME)) {
            builder.add(ARCHIVE_NAME, attrs.isArchive());
        }

        if (builder.match(SYSTEM_NAME)) {
            builder.add(SYSTEM_NAME, attrs.isSystem());
        }

        if (builder.match(HIDDEN_NAME)) {
            builder.add(HIDDEN_NAME, attrs.isHidden());
        }

        return builder.unmodifiableMap();
    }

    public void setAttribute(String attribute, Object value) throws IOException {
        switch (attribute) {
            case READONLY_NAME:
                setReadOnly((Boolean)value);
                break;
            case ARCHIVE_NAME:
                setArchive((Boolean)value);
                break;
            case SYSTEM_NAME:
                setSystem((Boolean)value);
                break;
            case HIDDEN_NAME:
                setHidden((Boolean)value);
                break;
            default:
                super.setAttribute(attribute, value);
                break;
        }
    }

}
