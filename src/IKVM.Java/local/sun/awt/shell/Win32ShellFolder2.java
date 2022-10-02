/*
 * Copyright (c) 2003, 2012, Oracle and/or its affiliates. All rights reserved.
 * Copyright (C) 2009 Volker Berlin (i-net software)
 * Copyright (C) 2010 Karsten Heinrich (i-net software)
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package sun.awt.shell;

import java.awt.Image;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.*;
import java.util.concurrent.*;
import javax.swing.SwingConstants;

import cli.System.IntPtr;
import cli.System.Drawing.Bitmap;

// NOTE: This class basically a conversion of the OpenJDK Wen32ShellFolder2, but uses
// .NET pointers and objects instead of representing pointers as long

/**
 * Win32 Shell Folders
 * <P>
 * <BR>
 * There are two fundamental types of shell folders : file system folders
 * and non-file system folders.  File system folders are relatively easy
 * to deal with.  Non-file system folders are items such as My Computer,
 * Network Neighborhood, and the desktop.  Some of these non-file system
 * folders have special values and properties.
 * <P>
 * <BR>
 * Win32 keeps two basic data structures for shell folders.  The first
 * of these is called an ITEMIDLIST.  Usually a pointer, called an
 * LPITEMIDLIST, or more frequently just "PIDL".  This structure holds
 * a series of identifiers and can be either relative to the desktop
 * (an absolute PIDL), or relative to the shell folder that contains them.
 * Some Win32 functions can take absolute or relative PIDL values, and
 * others can only accept relative values.
 * <BR>
 * The second data structure is an IShellFolder COM interface.  Using
 * this interface, one can enumerate the relative PIDLs in a shell
 * folder, get attributes, etc.
 * <BR>
 * All Win32ShellFolder2 objects which are folder types (even non-file
 * system folders) contain an IShellFolder object. Files are named in
 * directories via relative PIDLs.
 *
 * @author Michael Martak
 * @author Leif Samuelsson
 * @author Kenneth Russell
 * @author Volker Berlin
 * @author Karsten Heinrich
 * @since 1.4 */

final class Win32ShellFolder2 extends ShellFolder {

    // Win32 Shell Folder Constants
    public static final int DESKTOP = 0x0000;
    public static final int INTERNET = 0x0001;
    public static final int PROGRAMS = 0x0002;
    public static final int CONTROLS = 0x0003;
    public static final int PRINTERS = 0x0004;
    public static final int PERSONAL = 0x0005;
    public static final int FAVORITES = 0x0006;
    public static final int STARTUP = 0x0007;
    public static final int RECENT = 0x0008;
    public static final int SENDTO = 0x0009;
    public static final int BITBUCKET = 0x000a;
    public static final int STARTMENU = 0x000b;
    public static final int DESKTOPDIRECTORY = 0x0010;
    public static final int DRIVES = 0x0011;
    public static final int NETWORK = 0x0012;
    public static final int NETHOOD = 0x0013;
    public static final int FONTS = 0x0014;
    public static final int TEMPLATES = 0x0015;
    public static final int COMMON_STARTMENU = 0x0016;
    public static final int COMMON_PROGRAMS = 0X0017;
    public static final int COMMON_STARTUP = 0x0018;
    public static final int COMMON_DESKTOPDIRECTORY = 0x0019;
    public static final int APPDATA = 0x001a;
    public static final int PRINTHOOD = 0x001b;
    public static final int ALTSTARTUP = 0x001d;
    public static final int COMMON_ALTSTARTUP = 0x001e;
    public static final int COMMON_FAVORITES = 0x001f;
    public static final int INTERNET_CACHE = 0x0020;
    public static final int COOKIES = 0x0021;
    public static final int HISTORY = 0x0022;

    // Win32 shell folder attributes
    public static final int ATTRIB_CANCOPY          = 0x00000001;
    public static final int ATTRIB_CANMOVE          = 0x00000002;
    public static final int ATTRIB_CANLINK          = 0x00000004;
    public static final int ATTRIB_CANRENAME        = 0x00000010;
    public static final int ATTRIB_CANDELETE        = 0x00000020;
    public static final int ATTRIB_HASPROPSHEET     = 0x00000040;
    public static final int ATTRIB_DROPTARGET       = 0x00000100;
    public static final int ATTRIB_LINK             = 0x00010000;
    public static final int ATTRIB_SHARE            = 0x00020000;
    public static final int ATTRIB_READONLY         = 0x00040000;
    public static final int ATTRIB_GHOSTED          = 0x00080000;
    public static final int ATTRIB_HIDDEN           = 0x00080000;
    public static final int ATTRIB_FILESYSANCESTOR  = 0x10000000;
    public static final int ATTRIB_FOLDER           = 0x20000000;
    public static final int ATTRIB_FILESYSTEM       = 0x40000000;
    public static final int ATTRIB_HASSUBFOLDER     = 0x80000000;
    public static final int ATTRIB_VALIDATE         = 0x01000000;
    public static final int ATTRIB_REMOVABLE        = 0x02000000;
    public static final int ATTRIB_COMPRESSED       = 0x04000000;
    public static final int ATTRIB_BROWSABLE        = 0x08000000;
    public static final int ATTRIB_NONENUMERATED    = 0x00100000;
    public static final int ATTRIB_NEWCONTENT       = 0x00200000;

    // IShellFolder::GetDisplayNameOf constants
    public static final int SHGDN_NORMAL            = 0;
    public static final int SHGDN_INFOLDER          = 1;
    public static final int SHGDN_INCLUDE_NONFILESYS= 0x2000;
    public static final int SHGDN_FORADDRESSBAR     = 0x4000;
    public static final int SHGDN_FORPARSING        = 0x8000;

    // Values for system call LoadIcon()
    public enum SystemIcon {
        IDI_APPLICATION(32512),
        IDI_HAND(32513),
        IDI_ERROR(32513),
        IDI_QUESTION(32514),
        IDI_EXCLAMATION(32515),
        IDI_WARNING(32515),
        IDI_ASTERISK(32516),
        IDI_INFORMATION(32516),
        IDI_WINLOGO(32517);

        private final int iconID;

        SystemIcon(int iconID) {
            this.iconID = iconID;
        }

        public int getIconID() {
            return iconID;
        }
    }

    static class FolderDisposer implements sun.java2d.DisposerRecord {
        /*
         * This is cached as a concession to getFolderType(), which needs
         * an absolute PIDL.
         */
        cli.System.IntPtr absolutePIDL;
        /*
         * We keep track of shell folders through the IShellFolder
         * interface of their parents plus their relative PIDL.
         */
        cli.System.Object pIShellFolder;
        cli.System.IntPtr relativePIDL;

        boolean disposed;
 
        @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
        public void dispose() {
            if (disposed)
                return;
            if ( relativePIDL != null && !cli.System.IntPtr.Zero.Equals( relativePIDL ) ) {
                releasePIDL(relativePIDL);
            }
            if ( absolutePIDL != null && !cli.System.IntPtr.Zero.Equals( absolutePIDL ) ) {
                releasePIDL(absolutePIDL);
            }
            if ( pIShellFolder != null ) {
                releaseIShellFolder(pIShellFolder);
            }
            disposed = true;
        }
    }

    FolderDisposer disposer = new FolderDisposer();

    private void setIShellFolder( cli.System.Object iShellFolder ) {
        disposer.pIShellFolder = iShellFolder;
    }

    private void setRelativePIDL(cli.System.IntPtr relativePIDL) {
        disposer.relativePIDL = relativePIDL;
    }

    /*
     * The following are for caching various shell folder properties.
     */
    private cli.System.Object pIShellIcon = null;
    private String folderType = null;
    private String displayName = null;
    private Image smallIcon = null;
    private Image largeIcon = null;
    private Boolean isDir = null;

    /*
     * The following is to identify the My Documents folder as being special
     */
    private boolean isPersonal;

    /**
     * Create a system special shell folder, such as the
     * desktop or Network Neighborhood.
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    Win32ShellFolder2(final int csidl) throws IOException, InterruptedException {
        // Desktop is parent of DRIVES and NETWORK, not necessarily
        // other special shell folders.
        super ( null, (getFileSystemPath(csidl) == null) ? ("ShellFolder: 0x" + Integer.toHexString(csidl)) : getFileSystemPath(csidl));
        if (csidl == DESKTOP) {
        	// compared to the Java implementation we require two steps here since
        	// we don't have a callback from the native methods in to this instance
            setIShellFolder( initDesktopFolder() );
            setRelativePIDL( initDesktopPIDL() );
        } else {
        	cli.System.Object desktopFolder = getDesktop().getIShellFolder();
        	cli.System.IntPtr pidl = initSpecialPIDL( desktopFolder, csidl );
            setRelativePIDL( pidl );
            setIShellFolder( initSpecialFolder(desktopFolder, pidl) );
            // At this point, the native method initSpecial() has set our relativePIDL
            // relative to the Desktop, which may not be our immediate parent. We need
            // to traverse this ID list and break it into a chain of shell folders from
            // the top, with each one having an immediate parent and a relativePIDL
            // relative to that parent.
            bindToDesktop();
        }

        sun.java2d.Disposer.addRecord(this , disposer);
    }

        @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
	protected void bindToDesktop() {
		cli.System.IntPtr pIDL = disposer.relativePIDL;
		parent = getDesktop();
		while ( pIDL != null && !cli.System.IntPtr.Zero.Equals( pIDL ) ) {
		    // Get a child pidl relative to 'parent'
			cli.System.IntPtr childPIDL = copyFirstPIDLEntry(pIDL);
		    if (childPIDL != null && !cli.System.IntPtr.Zero.Equals( childPIDL ) ) {
		        // Get a handle to the the rest of the ID list
		        // i,e, parent's grandchilren and down
		        pIDL = getNextPIDLEntry(pIDL);
		        if ( pIDL != null && !cli.System.IntPtr.Zero.Equals( pIDL ) ) {
		            // Now we know that parent isn't immediate to 'this' because it
		            // has a continued ID list. Create a shell folder for this child
		            // pidl and make it the new 'parent'.
		            parent = new Win32ShellFolder2( (Win32ShellFolder2) parent, childPIDL );
		        } else {
		            // No grandchildren means we have arrived at the parent of 'this',
		            // and childPIDL is directly relative to parent.
		            disposer.relativePIDL = childPIDL;
		        }
		    } else {
		        break;
		    }
		}
	}

    /**
     * Create a system shell folder
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    Win32ShellFolder2(Win32ShellFolder2 parent, cli.System.Object pIShellFolder, cli.System.IntPtr relativePIDL, String path) {
        super(parent, (path != null) ? path : "ShellFolder: ");
        this.disposer.pIShellFolder = pIShellFolder;
        this.disposer.relativePIDL = relativePIDL;
        sun.java2d.Disposer.addRecord(this, disposer);
    }

    /**
     * Creates a shell folder with a parent and relative PIDL
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    Win32ShellFolder2(Win32ShellFolder2 parent, cli.System.IntPtr relativePIDL) {
        super (parent, getFileSystemPath(parent.getIShellFolder(), relativePIDL));
        this .disposer.relativePIDL = relativePIDL;
        getAbsolutePath();
        sun.java2d.Disposer.addRecord(this , disposer);
    }

    // Initializes the desktop shell folder
    /**
     * Returns the pIDL of the Desktop folder (pIDL root)
     * @return the pIDL of the Desktop folder (pIDL root)
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr initDesktopPIDL();
    /**
     * Returns the IShellFolder pointer of the Desktop folder (pIDL root)
     * @return the IShellFolder pointer of the Desktop folder (pIDL root)
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.Object initDesktopFolder();

    // Initializes a special, non-file system shell folder
    // from one of the above constants
    /**
     * initializes a special folder
     * @param desktopIShellFolder the IShellFolder reference of the desktop folder
     * @param csidl the CSIDL of the requested special folder
     * @return the pIDL of the special folder relative to the desktop root
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr initSpecialPIDL(cli.System.Object desktopIShellFolder, int csidl);
    /**
     * initializes a special folder
     * @param desktopIShellFolder the IShellFolder reference of the desktop folder
     * @param pidl the pIDL of the requested folder relative to the desktopIShellFolder
     * @return the IShellFolder reference for the requested folder
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.Object initSpecialFolder(cli.System.Object desktopIShellFolder, cli.System.IntPtr pidl);

    /** Marks this folder as being the My Documents (Personal) folder */
    public void setIsPersonal() {
        isPersonal = true;
    }

    /**
     * This method is implemented to make sure that no instances
     * of <code>ShellFolder</code> are ever serialized. If <code>isFileSystem()</code> returns
     * <code>true</code>, then the object is representable with an instance of
     * <code>java.io.File</code> instead. If not, then the object depends
     * on native PIDL state and should not be serialized.
     *
     * @returns a <code>java.io.File</code> replacement object. If the folder
     * is a not a normal directory, then returns the first non-removable
     * drive (normally "C:\").
     */
    protected Object writeReplace()
            throws java.io.ObjectStreamException {
        if (isFileSystem()) {
            return new File(getPath());
        } else {
            Win32ShellFolder2 drives = Win32ShellFolderManager2.getDrives();
            if (drives != null) {
                File[] driveRoots = drives.listFiles();
                if (driveRoots != null) {
                    for (int i = 0; i < driveRoots.length; i++) {
                        if (driveRoots[i] instanceof  Win32ShellFolder2) {
                            Win32ShellFolder2 sf = (Win32ShellFolder2) driveRoots[i];
                            if (sf.isFileSystem() && !sf.hasAttribute(ATTRIB_REMOVABLE)) {
                                return new File(sf.getPath());
                            }
                        }
                    }
                }
            }
            // Ouch, we have no hard drives. Return something "valid" anyway.
            return new File("C:\\");
        }
    }

    /**
     * Finalizer to clean up any COM objects or PIDLs used by this object.
     */
    protected void dispose() {
        disposer.dispose();
    }

    // Given a (possibly multi-level) relative PIDL (with respect to
    // the desktop, at least in all of the usage cases in this code),
    // return a pointer to the next entry. Does not mutate the PIDL in
    // any way. Returns 0 if the null terminator is reached.
    // Needs to be accessible to Win32ShellFolderManager2
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native cli.System.IntPtr getNextPIDLEntry(cli.System.IntPtr pIDL);

    // Given a (possibly multi-level) relative PIDL (with respect to
    // the desktop, at least in all of the usage cases in this code),
    // copy the first entry into a newly-allocated PIDL. Returns 0 if
    // the PIDL is at the end of the list.
    // Needs to be accessible to Win32ShellFolderManager2
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native cli.System.IntPtr copyFirstPIDLEntry(cli.System.IntPtr pIDL);

    // Given a parent's absolute PIDL and our relative PIDL, build an absolute PIDL
    /**
     * Combines a parent pIDL with a descendant pIDL. It doesn't matter whether the parent pIDL
     * is relative or absolute since this is only a concatenation of the IDLs
     * @param ppIDL the parent pIDL
     * @param pIDL the pIDL relative to the ppIDL
     * @return a pIDL for the item referenced by the original pIDL but relative to the parent of ppIDL 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr combinePIDLs(cli.System.IntPtr ppIDL, cli.System.IntPtr pIDL);

    // Release a PIDL object
    // Needs to be accessible to Win32ShellFolderManager2
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native void releasePIDL(cli.System.IntPtr pIDL);

    // Release an IShellFolder object
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native void releaseIShellFolder( cli.System.Object iShellFolder );

    /**
     * Accessor for IShellFolder
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public cli.System.Object getIShellFolder() {
        if (disposer.pIShellFolder == null ) {
            assert (isDirectory());
            assert (parent != null);
            cli.System.Object parentIShellFolder = getParentIShellFolder();
            if (parentIShellFolder == null) {
                throw new InternalError( "Parent IShellFolder was null for " + getAbsolutePath() );
            }
            // We are a directory with a parent and a relative PIDL.
            // We want to bind to the parent so we get an IShellFolder instance associated with us.
            disposer.pIShellFolder = bindToObject(parentIShellFolder, disposer.relativePIDL);
            if (disposer.pIShellFolder == null ) {
                throw new InternalError("Unable to bind " + getAbsolutePath() + " to parent");
            }
        }
        return disposer.pIShellFolder;
    }

    /**
     * Get the parent ShellFolder's IShellFolder interface
     */
    public cli.System.Object getParentIShellFolder() {
        Win32ShellFolder2 parent = (Win32ShellFolder2) getParentFile();
        cli.System.Object parentFolder;
        if (parent == null) {
            // Parent should only be null if this is the desktop, whose
            // relativePIDL is relative to its own IShellFolder.
        	parentFolder = getIShellFolder();
        } else {
        	parentFolder = parent.getIShellFolder();
        }
        return parentFolder;
    }

    /**
     * Accessor for relative PIDL
     */
    public cli.System.IntPtr getRelativePIDL() {
        if (disposer.relativePIDL == null) {
            throw new InternalError( "Should always have a relative PIDL" );
        }
        return disposer.relativePIDL;
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private cli.System.IntPtr getAbsolutePIDL() {
        if (parent == null) {
            // This is the desktop
            return getRelativePIDL();
        } else {
            if (disposer.absolutePIDL == null || disposer.absolutePIDL.Equals( IntPtr.Zero )) {
                disposer.absolutePIDL = combinePIDLs( ((Win32ShellFolder2) parent).getAbsolutePIDL(), getRelativePIDL());
            }

            return disposer.absolutePIDL;
        }
    }

    /**
     * Helper function to return the desktop
     */
    public Win32ShellFolder2 getDesktop() {
        return Win32ShellFolderManager2.getDesktop();
    }

    /**
     * Helper function to return the desktop IShellFolder interface
     */
    public cli.System.Object getDesktopIShellFolder() {
        return getDesktop().getIShellFolder();
    }

    private static boolean pathsEqual(String path1, String path2) {
        // Same effective implementation as Win32FileSystem
        return path1.equalsIgnoreCase(path2);
    }

    /**
     * Check to see if two ShellFolder objects are the same
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public boolean equals(Object o) {
        if (o == null || !(o instanceof Win32ShellFolder2)) {
            // Short-circuit circuitous delegation path
            if (!(o instanceof File)) {
                return super.equals(o);
            }
            return pathsEqual(getPath(), ((File) o).getPath());
        }
        Win32ShellFolder2 rhs = (Win32ShellFolder2) o;
        if ((parent == null && rhs.parent != null) ||
            (parent != null && rhs.parent == null)) {
            return false;
        }

        if (isFileSystem() && rhs.isFileSystem()) {
            // Only folders with identical parents can be equal
            return (pathsEqual(getPath(), rhs.getPath()) &&
                    (parent == rhs.parent || parent.equals(rhs.parent)));
        }

        if (parent == rhs.parent || parent.equals(rhs.parent)) {
            try {
                return pidlsEqual(getParentIShellFolder(), disposer.relativePIDL, rhs.disposer.relativePIDL);
            } catch (InterruptedException e) {
                return false;
            }
        }

        return false;
    }

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static boolean pidlsEqual(final cli.System.Object pIShellFolder, final cli.System.IntPtr pidl1, final cli.System.IntPtr pidl2)
            throws InterruptedException {
        return invoke(new Callable<Boolean>() {
            @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
            public Boolean call() {
                return compareIDs(pIShellFolder, pidl1, pidl2) == 0;
            }
        }, RuntimeException.class);
    }

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native int compareIDs(cli.System.Object pParentIShellFolder, cli.System.IntPtr pidl1, cli.System.IntPtr pidl2);

    private volatile Boolean cachedIsFileSystem;

    /**
     * @return Whether this is a file system shell folder
     */
    public boolean isFileSystem() {
        if (cachedIsFileSystem == null) {
            cachedIsFileSystem = hasAttribute(ATTRIB_FILESYSTEM);
        }

        return cachedIsFileSystem;
    }

    /**
     * Return whether the given attribute flag is set for this object
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public boolean hasAttribute(int attribute) {
        // Caching at this point doesn't seem to be cost efficient
        return (getAttributes0(getParentIShellFolder(), getRelativePIDL(), attribute) & attribute) != 0;
    }

    /**
     * Returns the queried attributes specified in attrsMask.
     *
     * Could plausibly be used for attribute caching but have to be
     * very careful not to touch network drives and file system roots
     * with a full attrsMask
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native int getAttributes0(cli.System.Object pParentIShellFolder, cli.System.IntPtr pIDL, int attrsMask);

    // Return the path to the underlying file system object
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static String getFileSystemPath(cli.System.Object parentIShellFolder, cli.System.IntPtr relativePIDL) {
        int linkedFolder = ATTRIB_LINK | ATTRIB_FOLDER;
        if (parentIShellFolder == Win32ShellFolderManager2.getNetwork().getIShellFolder() &&
                getAttributes0(parentIShellFolder, relativePIDL, linkedFolder) == linkedFolder) {

        	cli.System.Object desktopIShellFolder = Win32ShellFolderManager2.getDesktop().getIShellFolder();
            String path = getDisplayNameOf(parentIShellFolder, relativePIDL, SHGDN_FORPARSING );
			String s = getFileSystemPath(desktopIShellFolder, getLinkLocation( path, false));
            if (s != null && s.startsWith("\\\\")) {
                return s;
            }
        }
        return getDisplayNameOf(parentIShellFolder, relativePIDL, SHGDN_NORMAL | SHGDN_FORPARSING);
    }

    // Needs to be accessible to Win32ShellFolderManager2
    static String getFileSystemPath(final int csidl) throws IOException, InterruptedException {
        String path = invoke(new Callable<String>() {
            public String call() throws IOException {
                return getFileSystemPath0(csidl);
            }
        }, IOException.class);
        if (path != null) {
            SecurityManager security = System.getSecurityManager();
            if (security != null) {
                security.checkRead(path);
            }
        }
        return path;
    }

    // NOTE: this method uses COM and must be called on the 'COM thread'. See ComInvoker for the details
    private static native String getFileSystemPath0(int csidl) throws IOException;

    // Return whether the path is a network root.
    // Path is assumed to be non-null
    private static boolean isNetworkRoot(String path) {
        return (path.equals("\\\\") || path.equals("\\") || path.equals("//") || path.equals("/"));
    }

    /**
     * @return The parent shell folder of this shell folder, null if
     * there is no parent
     */
    public File getParentFile() {
        return parent;
    }

    public boolean isDirectory() {
        if (isDir == null) {
            // Folders with SFGAO_BROWSABLE have "shell extension" handlers and are
            // not traversable in JFileChooser.
            if (hasAttribute(ATTRIB_FOLDER) && !hasAttribute(ATTRIB_BROWSABLE)) {
                isDir = Boolean.TRUE;
            } else if (isLink()) {
                ShellFolder linkLocation = getLinkLocation(false);
                isDir = Boolean.valueOf(linkLocation != null && linkLocation.isDirectory());
            } else {
                isDir = Boolean.FALSE;
            }
        }
        return isDir.booleanValue();
    }

    /*
     * Functions for enumerating an IShellFolder's children
     */
    // Returns an IEnumIDList interface for an IShellFolder.  The value
    // returned must be released using releaseEnumObjects().
    /**
     * Returns an IEnumIDList interface for an IShellFolder.  The value 
     * returned must be released using releaseEnumObjects().
     * @param pIShellFolder the IShellFolder instance of the parent shell folder
     * @param includeHiddenFiles if true, hidden files will be included in the enumeration
     * @return an instance of IEnumIDList 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    cli.System.Object getEnumObjects(cli.System.Object pIShellFolder, boolean includeHiddenFiles) {
        boolean isDesktop = (disposer.pIShellFolder == getDesktopIShellFolder());
        return getEnumObjects(disposer.pIShellFolder, isDesktop, includeHiddenFiles);
    }

    /**
     * Returns an IEnumIDList interface for an IShellFolder.  The value 
     * returned must be released using releaseEnumObjects().
     * @param pIShellFolder the IShellFolder instance of the parent shell folder
     * @param isDesktop must be set to true, if the pIShellFolder is the desktop shell folder
     * @param includeHiddenFiles if true, hidden files will be included in the enumeration
     * @return an instance of IEnumIDList 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.Object getEnumObjects(cli.System.Object pIShellFolder, boolean isDesktop, boolean includeHiddenFiles);

    /**
     * Returns the next sequential child as a relative PIDL
     * from an IEnumIDList interface.  The value returned must
     * be released using releasePIDL().
     * @param pEnumObjects the IEnumIDList instance to get the next child from
     * @return the next child or {@link IntPtr#Zero} if the end of the enumeration is reached 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native cli.System.IntPtr getNextChild(cli.System.Object pEnumObjects);

    /**
     * Releases the IEnumIDList interface
     * @param pEnumObjects an IEnumIDList instance 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native void releaseEnumObjects(cli.System.Object pEnumObjects);

    /**
     * Returns the IShellFolder of a child from a parent IShellFolder and a relative pIDL. The pIDL
     * may as well be any other  descendant of the shell folder - at least this is, what the windows API 
     * documentation says.  
     * The value returned must be released using releaseIShellFolder().
     * @param parentIShellFolder an IShellFolder instance as root for the pIDL 
     * @param pIDL a pIDL relative to the parent shell folder
     * @return a NEW instance of an IShellFolder for the path given by the pIDL, may be null if the path is invalid
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.Object bindToObject(cli.System.Object parentIShellFolder, cli.System.IntPtr pIDL);

    /**
     * @return An array of shell folders that are children of this shell folder
     *         object. The array will be empty if the folder is empty.  Returns
     *         <code>null</code> if this shellfolder does not denote a directory.
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public File[] listFiles(final boolean includeHiddenFiles) {
        SecurityManager security = System.getSecurityManager();
        if (security != null) {
            security.checkRead(getPath());
        }

        try {
            return invoke(new Callable<File[]>() {
                @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
                public File[] call() throws InterruptedException {
                    if (!isDirectory()) {
                        return null;
                    }
                    // Links to directories are not directories and cannot be parents.
                    // This does not apply to folders in My Network Places (NetHood)
                    // because they are both links and real directories!
                    if (isLink() && !hasAttribute(ATTRIB_FOLDER)) {
                        return new File[0];
                    }

                    Win32ShellFolder2 desktop = Win32ShellFolderManager2.getDesktop();
                    Win32ShellFolder2 personal = Win32ShellFolderManager2.getPersonal();

                    // If we are a directory, we have a parent and (at least) a
                    // relative PIDL. We must first ensure we are bound to the
                    // parent so we have an IShellFolder to query.
                    cli.System.Object pIShellFolder = getIShellFolder();
                    // Now we can enumerate the objects in this folder.
                    ArrayList<Win32ShellFolder2> list = new ArrayList<Win32ShellFolder2>();
                    cli.System.Object pEnumObjects = getEnumObjects(pIShellFolder, includeHiddenFiles);
                    if (pEnumObjects != null) {
                        cli.System.IntPtr childPIDL = null;
                        int testedAttrs = ATTRIB_FILESYSTEM | ATTRIB_FILESYSANCESTOR;
                        do {
                            if (Thread.currentThread().isInterrupted()) {
                                return new File[0];
                            }
                            childPIDL = getNextChild(pEnumObjects);
                            boolean releasePIDL = true;
                            if ( childPIDL != null && !cli.System.IntPtr.Zero.Equals( childPIDL ) && (getAttributes0(pIShellFolder, childPIDL, testedAttrs) & testedAttrs) != 0) {
                                Win32ShellFolder2 childFolder = null;
                                if (this .equals(desktop) && personal != null
                                        && pidlsEqual(pIShellFolder, childPIDL, personal.disposer.relativePIDL) ) {
                                    childFolder = personal;
                                } else {
                                    childFolder = new Win32ShellFolder2(Win32ShellFolder2.this , childPIDL);
                                    releasePIDL = false;
                                }
                                list.add(childFolder);
                            }
                            if (releasePIDL) {
                                releasePIDL(childPIDL);
                            }
                        } while (childPIDL != null && !childPIDL.Equals( cli.System.IntPtr.Zero ));
                        releaseEnumObjects(pEnumObjects);
                    }
                    return Thread.currentThread().isInterrupted()
                        ? new File[0]
                        : list.toArray(new ShellFolder[list.size()]);
                }
            }, InterruptedException.class);
        } catch (InterruptedException e) {
            return new File[0];
        }
    }


    /**
     * Look for (possibly special) child folder by it's path. Note: this will not work an an ancestor(not child)
     * of the current folder. 
     * @return The child shell folder, or null if not found.
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    Win32ShellFolder2 getChildByPath(String filePath) {
    	cli.System.Object pIShellFolder = getIShellFolder();
    	cli.System.Object pEnumObjects = getEnumObjects(pIShellFolder, true);
        Win32ShellFolder2 child = null;
        cli.System.IntPtr childPIDL = null;
        
        childPIDL = getNextChild(pEnumObjects);
        while ( childPIDL != null && !cli.System.IntPtr.Zero.Equals( childPIDL ) ) {
            if (getAttributes0(pIShellFolder, childPIDL, ATTRIB_FILESYSTEM) != 0) {
                String path = getFileSystemPath(pIShellFolder, childPIDL);
                if (path != null && path.equalsIgnoreCase(filePath)) {
                	cli.System.Object childIShellFolder = bindToObject( pIShellFolder, childPIDL);
                    child = new Win32ShellFolder2(this, childIShellFolder, childPIDL, path);
                    break;
                }
            }
            releasePIDL(childPIDL);
            childPIDL = getNextChild(pEnumObjects);
        }
        releaseEnumObjects(pEnumObjects);
        return child;
    }

    private volatile Boolean cachedIsLink;

    /**
     * @return Whether this shell folder is a link
     */
    public boolean isLink() {
        if (cachedIsLink == null) {
            cachedIsLink = hasAttribute(ATTRIB_LINK);
        }

        return cachedIsLink;
    }

    /**
     * @return Whether this shell folder is marked as hidden
     */
    public boolean isHidden() {
        return hasAttribute(ATTRIB_HIDDEN);
    }

    // Return the link location of a shell folder
    /**
     * Resolves the link location of an item to an ABSOLUTE pIDL
     * @param parentIShellFolder the pointer to the parent IShellFolder of the item
     * @param relativePIDL a single-level pIDL to the item
     * @param resolve 
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native cli.System.IntPtr getLinkLocation( String path, boolean resolve);

    /**
     * @return The shell folder linked to by this shell folder, or null
     * if this shell folder is not a link or is a broken or invalid link
     */
    public ShellFolder getLinkLocation()  {
        return getLinkLocation(true);
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private ShellFolder getLinkLocation(final boolean resolve) {
        return invoke(new Callable<ShellFolder>() {
            @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
            public ShellFolder call() {
                if (!isLink()) {
                    return null;
                }

                ShellFolder location = null;
                cli.System.IntPtr linkLocationPIDL = getLinkLocation( getAbsolutePath(), resolve);
                if (linkLocationPIDL != null && !cli.System.IntPtr.Zero.Equals( linkLocationPIDL ) ) {
                    try {
                        location =
                                Win32ShellFolderManager2.createShellFolderFromRelativePIDL(getDesktop(),
                                        linkLocationPIDL);
                    } catch (InterruptedException e) {
                        // Return null
                    } catch (InternalError e) {
                        // Could be a link to a non-bindable object, such as a network connection
                        // TODO: getIShellFolder() should throw FileNotFoundException instead
                    }
                }
                return location;
            }
        });
    }

    /**
     * Parse a display name into a PIDL relative to the current IShellFolder.
     * @param name the name or relative path
     * @return a pIDL for the path, may be {@link IntPtr#Zero} if not found
     * @throws FileNotFoundException
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    cli.System.IntPtr parseDisplayName(String name) throws FileNotFoundException {
        try {
            return parseDisplayName0(getIShellFolder(), name);
        } catch (IOException e) {
            throw new FileNotFoundException("Could not find file " + name);
        }
    }

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr parseDisplayName0(cli.System.Object pIShellFolder, String name) throws IOException;

    /**
     * Returns the display name of an item in a folder
     * @param parentIShellFolder the pointer to the IShellFolder interface of the parent folder
     * @param relativePIDL single-level pIDL to the requested item within the parent folder
     * @param attrs formatting attributes for the display name, refer to SHGDN in MSDN
     * @return
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native String getDisplayNameOf( cli.System.Object parentIShellFolder, cli.System.IntPtr relativePIDL, int attrs);

    /**
     * @return The name used to display this shell folder
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public String getDisplayName() {
        if (displayName == null) {
            displayName = getDisplayNameOf(getParentIShellFolder(), getRelativePIDL(), SHGDN_NORMAL);
        }
        return displayName;
    }

    // Return the folder type of a shell folder
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native String getFolderType(cli.System.IntPtr pIDL);

    /**
     * @return The type of shell folder as a string
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public String getFolderType() {
        if (folderType == null) {
            folderType = getFolderType(getAbsolutePIDL());
        }
        return folderType;
    }

    // Return the executable type of a file system shell folder
    private static native String getExecutableType(String path);

    /**
     * @return The executable type as a string
     */
    public String getExecutableType() {
        if (!isFileSystem()) {
            return null;
        }
        return getExecutableType(getAbsolutePath());
    }

    // Icons

    private static Map smallSystemImages = new HashMap();
    private static Map largeSystemImages = new HashMap();
    private static Map smallLinkedSystemImages = new HashMap();
    private static Map largeLinkedSystemImages = new HashMap();

    /**
     * Returns the icon index in the system image list  
     * @param parentIShellIcon the the pointer to the IShellIcon instance of the parent folder 
     * @param relativePIDL the relative pIDL to the requested item
     * @return the system image list index for the icon of the item or zero, if there is no entry
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native int getIconIndex(cli.System.Object parentIShellFolder, cli.System.IntPtr relativePIDL);

    // Return the icon of a file system shell folder in the form of an HICON
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr getIcon(String absolutePath, boolean getLargeIcon);

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.IntPtr extractIcon(cli.System.Object parentIShellFolder, cli.System.IntPtr relativePIDL,
                                           boolean getLargeIcon);

    /**
     * Returns the {@link Bitmap} for a HICON.
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native Bitmap getIconBits(cli.System.IntPtr hIcon, int size);

    /**
     * Disposes a icon handle
     * @param hIcon the handle to be disposed
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native void disposeIcon(cli.System.IntPtr hIcon);

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native Bitmap getStandardViewButton0(int iconIndex);

    /**
     * Creates a Java icon for a HICON pointer
     * @param hIcon the handle for the icon
     * @param getLargeIcon true for a large icon, false for a small icon
     * @return the created image or null, if the handle is invalid
     */
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static Image makeIcon(cli.System.IntPtr hIcon, boolean getLargeIcon) {
        if (hIcon != null ) {
            // Get the bits.  This has the side effect of setting the imageHash value for this object.
            Bitmap bitmap = getIconBits(hIcon, getLargeIcon ? 32 : 16 );
            if (bitmap == null) {
                return null;
            }            
            return new BufferedImage(bitmap);
        }
        return null;
    }


    /**
     * @return The icon image used to display this shell folder
     */
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public Image getIcon(boolean getLargeIcon) {
        Image icon = getLargeIcon ? largeIcon : smallIcon;
        if (icon == null) {
            cli.System.IntPtr relativePIDL = getRelativePIDL();

            if (isFileSystem() && parent != null) {
                // These are cached per type (using the index in the system image list)
                int index = getIconIndex( ((Win32ShellFolder2)parent).getIShellFolder(), relativePIDL);
                if (index > 0) {
                    Map imageCache;
                    if (isLink()) {
                        imageCache = getLargeIcon ? largeLinkedSystemImages : smallLinkedSystemImages;
                    } else {
                        imageCache = getLargeIcon ? largeSystemImages : smallSystemImages;
                    }
                    icon = (Image) imageCache.get(Integer.valueOf(index));
                    if (icon == null) {
                    	cli.System.IntPtr hIcon = getIcon(getAbsolutePath(), getLargeIcon);
                        icon = makeIcon(hIcon, getLargeIcon);
                        disposeIcon(hIcon);
                        if (icon != null) {
                            imageCache.put(Integer.valueOf(index), icon);
                        }
                    }
                }
            }

            if (icon == null) {
                // These are only cached per object
            	cli.System.IntPtr hIcon = extractIcon(getParentIShellFolder(), getRelativePIDL(), getLargeIcon);
                icon = makeIcon(hIcon, getLargeIcon);
                disposeIcon(hIcon);
            }

            if (getLargeIcon) {
                largeIcon = icon;
            } else {
                smallIcon = icon;
            }
        }
        if (icon == null) {
            icon = super .getIcon(getLargeIcon);
        }
        return icon;
    }

    /**
     * Gets an icon from the Windows system icon list as an <code>Image</code>
     */
    static Image getShell32Icon(int iconID, boolean getLargeIcon) {
        Bitmap bitmap = getShell32IconResourceAsBitmap(iconID, getLargeIcon);
        if (bitmap == null) {
            return null;
        }
        return new BufferedImage(bitmap);
    }
    
    private static native Bitmap getShell32IconResourceAsBitmap(int iconID, boolean getLargeIcon);

    /**
     * Returns the canonical form of this abstract pathname.  Equivalent to
     * <code>new&nbsp;Win32ShellFolder2(getParentFile(), this.{@link java.io.File#getCanonicalPath}())</code>.
     *
     * @see java.io.File#getCanonicalFile
     */
    public File getCanonicalFile() throws IOException {
        return this;
    }

    /*
     * Indicates whether this is a special folder (includes My Documents)
     */
    public boolean isSpecial() {
        return isPersonal || !isFileSystem() || (this == getDesktop());
    }

    /**
     * Compares this object with the specified object for order.
     *
     * @see sun.awt.shell.ShellFolder#compareTo(File)
     */
    public int compareTo(File file2) {
        if (!(file2 instanceof Win32ShellFolder2)) {
            if (isFileSystem() && !isSpecial()) {
                return super.compareTo(file2);
            } else {
                return -1; // Non-file shellfolders sort before files
            }
        }
        return Win32ShellFolderManager2.compareShellFolders(this, (Win32ShellFolder2) file2);
    }

    // native constants from commctrl.h
    private static final int LVCFMT_LEFT = 0;
    private static final int LVCFMT_RIGHT = 1;
    private static final int LVCFMT_CENTER = 2;

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public ShellFolderColumnInfo[] getFolderColumns() {
    	Object o = doGetColumnInfo(getIShellFolder());
        ShellFolderColumnInfo[] columns = (ShellFolderColumnInfo[]) o;

        if (columns != null) {
            List<ShellFolderColumnInfo> notNullColumns = new ArrayList<ShellFolderColumnInfo>();
            for (int i = 0; i < columns.length; i++) {
                ShellFolderColumnInfo column = columns[i];
                if (column != null) {
                    column.setAlignment(column.getAlignment() == LVCFMT_RIGHT ? SwingConstants.RIGHT
                                    : column.getAlignment() == LVCFMT_CENTER ? SwingConstants.CENTER
                                            : SwingConstants.LEADING);

                    column.setComparator(new ColumnComparator(i));

                    notNullColumns.add(column);
                }
            }
            columns = new ShellFolderColumnInfo[notNullColumns.size()];
            notNullColumns.toArray(columns);
        }
        return columns;
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public Object getFolderColumnValue(int column) {
        return doGetColumnValue(getParentIShellFolder(), getRelativePIDL(), column);
    }

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native cli.System.Object /*ShellFolderColumnInfo[]*/ doGetColumnInfo( cli.System.Object iShellFolder2 );

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    private static native Object doGetColumnValue(cli.System.Object parentIShellFolder2, cli.System.IntPtr childPIDL, int columnIdx);

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static native int compareIDsByColumn(cli.System.Object pParentIShellFolder, cli.System.IntPtr pidl1, cli.System.IntPtr pidl2, int columnIdx);

    private class ColumnComparator implements Comparator {
        private final int columnIdx;

        public ColumnComparator(int columnIdx) {
            this.columnIdx = columnIdx;
        }

        // compares 2 objects within this folder by the specified column
        @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
        public int compare(Object o, Object o1) {
            if (o instanceof  Win32ShellFolder2 && o1 instanceof  Win32ShellFolder2) {
                // delegates comparison to native method
                return compareIDsByColumn(getIShellFolder(),
                        ((Win32ShellFolder2) o).getRelativePIDL(),
                        ((Win32ShellFolder2) o1).getRelativePIDL(),
                        columnIdx);
            }
            return 0;
        }
    }
}