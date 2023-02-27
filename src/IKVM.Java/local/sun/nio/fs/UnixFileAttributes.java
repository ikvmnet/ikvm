package sun.nio.fs;

import java.nio.file.attribute.*;
import java.util.*;
import java.io.IOException;

final class UnixFileAttributes implements PosixFileAttributes {

    private static FileTime toFileTime(long sec, long nsec) {
        if (nsec == 0) {
            return FileTime.from(sec, TimeUnit.SECONDS);
        } else {
            long micro = sec * 1000000L + nsec / 1000L;
            return FileTime.from(micro, TimeUnit.MICROSECONDS);
        }
    }

    private int     st_mode;
    private long    st_ino;
    private long    st_dev;
    private long    st_rdev;
    private int     st_nlink;
    private int     st_uid;
    private int     st_gid;
    private long    st_size;
    private long    st_atime_sec;
    private long    st_atime_nsec;
    private long    st_mtime_sec;
    private long    st_mtime_nsec;
    private long    st_ctime_sec;
    private long    st_ctime_nsec;
    private long    st_birthtime_sec;

    private volatile UserPrincipal owner;
    private volatile GroupPrincipal group;
    private volatile UnixFileKey key;

    public UnixFileAttributes(DotNetPath path) {
        this.path = path;
    }

    @Override
    public FileTime lastModifiedTime() {
        return toFileTime(st_mtime_sec, st_mtime_nsec);
    }

    @Override
    public FileTime lastAccessTime() {
        return toFileTime(st_atime_sec, st_atime_nsec);
    }

    @Override
    public FileTime creationTime() {
        return FileTime.from(st_birthtime_sec, TimeUnit.SECONDS);
    }

    @Override
    public boolean isRegularFile() {
       return ((st_mode & UnixConstants.S_IFMT) == UnixConstants.S_IFREG);
    }

    @Override
    public boolean isDirectory() {
        return ((st_mode & UnixConstants.S_IFMT) == UnixConstants.S_IFDIR);
    }

    @Override
    public boolean isSymbolicLink() {
        return ((st_mode & UnixConstants.S_IFMT) == UnixConstants.S_IFLNK);
    }

    @Override
    public boolean isOther() {
        int type = st_mode & UnixConstants.S_IFMT;
        return (type != UnixConstants.S_IFREG && type != UnixConstants.S_IFDIR && type != UnixConstants.S_IFLNK);
    }

    @Override
    public long size() {
        return st_size;
    }

    @Override
    public UnixFileKey fileKey() {
        if (key == null) {
            synchronized (this) {
                if (key == null) {
                    key = new UnixFileKey(st_dev, st_ino);
                }
            }
        }

        return key;
    }

    @Override
    public UserPrincipal owner() {
        if (owner == null) {
            synchronized (this) {
                if (owner == null) {
                    owner = UnixUserPrincipals.fromUid(st_uid);
                }
            }
        }

        return owner;
    }

    @Override
    public GroupPrincipal group() {
        if (group == null) {
            synchronized (this) {
                if (group == null) {
                    group = UnixUserPrincipals.fromGid(st_gid);
                }
            }
        }

        return group;
    }

    @Override
    public Set<PosixFilePermission> permissions() {
        int bits = (st_mode & UnixConstants.S_IAMB);
        HashSet<PosixFilePermission> perms = new HashSet<>();

        if ((bits & UnixConstants.S_IRUSR) > 0)
            perms.add(PosixFilePermission.OWNER_READ);
        if ((bits & UnixConstants.S_IWUSR) > 0)
            perms.add(PosixFilePermission.OWNER_WRITE);
        if ((bits & UnixConstants.S_IXUSR) > 0)
            perms.add(PosixFilePermission.OWNER_EXECUTE);

        if ((bits & UnixConstants.S_IRGRP) > 0)
            perms.add(PosixFilePermission.GROUP_READ);
        if ((bits & UnixConstants.S_IWGRP) > 0)
            perms.add(PosixFilePermission.GROUP_WRITE);
        if ((bits & UnixConstants.S_IXGRP) > 0)
            perms.add(PosixFilePermission.GROUP_EXECUTE);

        if ((bits & UnixConstants.S_IROTH) > 0)
            perms.add(PosixFilePermission.OTHERS_READ);
        if ((bits & UnixConstants.S_IWOTH) > 0)
            perms.add(PosixFilePermission.OTHERS_WRITE);
        if ((bits & UnixConstants.S_IXOTH) > 0)
            perms.add(PosixFilePermission.OTHERS_EXECUTE);

        return perms;
    }

}
