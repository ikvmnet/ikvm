package sun.nio.fs;

import java.io.*;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.util.*;

class DotNetDosFileAttributes implements DosFileAttributes {

    public native static DotNetDosFileAttributes read(String path) throws IOException;

    private final FileTime creationTime;
    private final FileTime lastAccessTime;
    private final FileTime lastModifiedTime;
    private Object fileKey;
    private boolean isDirectory;
    private boolean isOther;
    private boolean isRegularFile;
    private boolean isSymbolicLink;
    private long size;
    private boolean isReadOnly;
    private boolean isHidden;
    private boolean isArchive;
    private boolean isSystem;

    public DotNetDosFileAttributes(FileTime creationTime, FileTime lastAccessTime, FileTime lastModifiedTime, Object fileKey, boolean isDirectory, boolean isOther, boolean isRegularFile, boolean isSymbolicLink, long size, boolean isReadOnly, boolean isHidden, boolean isArchive, boolean isSystem) {
      this.creationTime = creationTime;
      this.lastAccessTime = lastAccessTime;
      this.lastModifiedTime = lastModifiedTime;
      this.fileKey = fileKey;
      this.isDirectory = isDirectory;
      this.isOther = isOther;
      this.isRegularFile = isRegularFile;
      this.isSymbolicLink = isSymbolicLink;
      this.size = size;
      this.isReadOnly = isReadOnly;
      this.isHidden = isHidden;
      this.isArchive = isArchive;
      this.isSystem = isSystem;
    }

    public FileTime creationTime() {
        return creationTime;
    }

    public FileTime lastAccessTime() {
        return lastAccessTime;
    }

    public FileTime lastModifiedTime() {
        return lastModifiedTime;
    }

    public Object fileKey() {
        return fileKey;
    }

    public boolean isDirectory() {
        return isDirectory;
    }

    public boolean isOther() {
        return isOther;
    }

    public boolean isRegularFile() {
        return isRegularFile;
    }

    public boolean isSymbolicLink() {
        return isSymbolicLink;
    }

    public long size() {
        return size;
    }

    public boolean isReadOnly() {
        return isReadOnly;
    }

    public boolean isHidden() {
        return isHidden;
    }

    public boolean isArchive() {
        return isArchive;
    }

    public boolean isSystem() {
        return isSystem;
    }

}
