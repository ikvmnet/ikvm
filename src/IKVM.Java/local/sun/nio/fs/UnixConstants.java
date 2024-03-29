package sun.nio.fs;                         

class UnixConstants {
    static final int O_RDONLY;
    static final int O_WRONLY;
    static final int O_RDWR;
    static final int O_APPEND;
    static final int O_CREAT;
    static final int O_EXCL;
    static final int O_TRUNC;
    static final int O_SYNC;
    static final int O_DSYNC;
    static final int O_NOFOLLOW;
    static final int S_IAMB;
    static final int S_IRUSR;
    static final int S_IWUSR;
    static final int S_IXUSR;
    static final int S_IRGRP;
    static final int S_IWGRP;
    static final int S_IXGRP;
    static final int S_IROTH;
    static final int S_IWOTH;
    static final int S_IXOTH;
    static final int S_IFMT;
    static final int S_IFREG;
    static final int S_IFDIR;
    static final int S_IFLNK;
    static final int S_IFCHR;
    static final int S_IFBLK;
    static final int S_IFIFO;
    static final int R_OK;
    static final int W_OK;
    static final int X_OK;
    static final int F_OK;
    static final int ENOENT;
    static final int ENXIO;
    static final int EACCES;
    static final int EEXIST;
    static final int ENOTDIR;
    static final int EINVAL;
    static final int EXDEV;
    static final int EISDIR;
    static final int ENOTEMPTY;
    static final int ENOSPC;
    static final int EAGAIN;
    static final int ENOSYS;
    static final int ELOOP;
    static final int EROFS;
    static final int ENODATA;
    static final int ERANGE;
    static final int EMFILE;
    static final int AT_SYMLINK_NOFOLLOW;
    static final int AT_REMOVEDIR;

    static {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsLinux()) {
            O_RDONLY = 0;
            O_WRONLY = 1;
            O_RDWR = 2;
            O_APPEND = 0x400;
            O_CREAT = 0x40;
            O_EXCL = 0x80;
            O_TRUNC = 0x200;
            O_SYNC = 0x1000;
            O_DSYNC = 0x1000;
            O_NOFOLLOW = 0x20000;
            S_IAMB = 0x1ff;
            S_IRUSR = 256;
            S_IWUSR = 128;
            S_IXUSR = 64;
            S_IRGRP = 32;
            S_IWGRP = 16;
            S_IXGRP = 8;
            S_IROTH = 4;
            S_IWOTH = 2;
            S_IXOTH = 1;
            S_IFMT = 0xf000;
            S_IFREG = 0x8000;
            S_IFDIR = 0x4000;
            S_IFLNK = 0xa000;
            S_IFCHR = 0x2000;
            S_IFBLK = 0x6000;
            S_IFIFO = 0x1000;
            R_OK = 4;
            W_OK = 2;
            X_OK = 1;
            F_OK = 0;
            ENOENT = 2;
            ENXIO = -1;
            EACCES = 13;
            EEXIST = 17;
            ENOTDIR = 20;
            EINVAL = 22;
            EXDEV = 18;
            EISDIR = 21;
            ENOTEMPTY = 39;
            ENOSPC = 28;
            EAGAIN = 11;
            ENOSYS = 38;
            ELOOP = 40;
            EROFS = 30;
            ENODATA = 61;
            ERANGE = 34;
            EMFILE = 24;
            AT_SYMLINK_NOFOLLOW = 0x100;
            AT_REMOVEDIR = 0x200;
        } else if (cli.IKVM.Runtime.RuntimeUtil.get_IsOSX()) {
            O_RDONLY = 0x0000;
            O_WRONLY = 0x0001;
            O_RDWR = 0x0002;
            O_APPEND = 0x0008;
            O_CREAT = 0x0200;
            O_EXCL = 0x0800;
            O_TRUNC = 0x0400;
            O_SYNC = 0x0080;
            O_DSYNC = 0x400000;
            O_NOFOLLOW = 0x0100;
            S_IAMB = (0000400|0000200|0000100|0000040|0000020|0000010|0000004|0000002|0000001);
            S_IRUSR = 0000400;
            S_IWUSR = 0000200;
            S_IXUSR = 0000100;
            S_IRGRP = 0000040;
            S_IWGRP = 0000020;
            S_IXGRP = 0000010;
            S_IROTH = 0000004;
            S_IWOTH = 0000002;
            S_IXOTH = 0000001;
            S_IFMT = 0170000;
            S_IFREG = 0100000;
            S_IFDIR = 0040000;
            S_IFLNK = 0120000;
            S_IFCHR = 0020000;
            S_IFBLK = 0060000;
            S_IFIFO = 0010000;
            R_OK = (1<<2);
            W_OK = (1<<1);
            X_OK = (1<<0);
            F_OK = 0;
            ENOENT = 2;
            ENXIO = 6;
            EACCES = 13;
            EEXIST = 17;
            ENOTDIR = 20;
            EINVAL = 22;
            EXDEV = 18;
            EISDIR = 21;
            ENOTEMPTY = 66;
            ENOSPC = 28;
            EAGAIN = 35;
            ENOSYS = 78;
            ELOOP = 62;
            EROFS = 30;
            ENODATA = 96;
            ERANGE = 34;
            EMFILE = 24;
            AT_SYMLINK_NOFOLLOW = 0x0020;
            AT_REMOVEDIR = 0x0080;
        } else {
            O_RDONLY = -1;
            O_WRONLY = -1;
            O_RDWR = -1;
            O_APPEND = -1;
            O_CREAT = -1;
            O_EXCL = -1;
            O_TRUNC = -1;
            O_SYNC = -1;
            O_DSYNC = -1;
            O_NOFOLLOW = -1;
            S_IAMB = -1;
            S_IRUSR = -1;
            S_IWUSR = -1;
            S_IXUSR = -1;
            S_IRGRP = -1;
            S_IWGRP = -1;
            S_IXGRP = -1;
            S_IROTH = -1;
            S_IWOTH = -1;
            S_IXOTH = -1;
            S_IFMT = -1;
            S_IFREG = -1;
            S_IFDIR = -1;
            S_IFLNK = -1;
            S_IFCHR = -1;
            S_IFBLK = -1;
            S_IFIFO = -1;
            R_OK = -1;
            W_OK = -1;
            X_OK = -1;
            F_OK = -1;
            ENOENT = -1;
            ENXIO = -1;
            EACCES = -1;
            EEXIST = -1;
            ENOTDIR = -1;
            EINVAL = -1;
            EXDEV = -1;
            EISDIR = -1;
            ENOTEMPTY = -1;
            ENOSPC = -1;
            EAGAIN = -1;
            ENOSYS = -1;
            ELOOP = -1;
            EROFS = -1;
            ENODATA = -1;
            ERANGE = -1;
            EMFILE = -1;
            AT_SYMLINK_NOFOLLOW = -1;
            AT_REMOVEDIR = -1;
        }
    }
    
    private UnixConstants() {
        
    }

}
