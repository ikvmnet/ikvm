/*
 * Copyright (c) 2003, 2014, Oracle and/or its affiliates. All rights reserved.
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

import java.awt.*;
import java.awt.image.BufferedImage;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.*;
import java.util.List;
import java.util.concurrent.*;
import java.util.stream.Stream;

import cli.System.IntPtr;
import cli.System.Drawing.Bitmap;
import cli.System.Drawing.SystemIcons;

import static sun.awt.shell.Win32ShellFolder2.*;
import sun.awt.OSInfo;
import sun.misc.ThreadGroupUtils;

// NOTE: This class supersedes Win32ShellFolderManager, which was removed
//       from distribution after version 1.4.2.

/**
 * @author Michael Martak
 * @author Leif Samuelsson
 * @author Kenneth Russell
 * @since 1.4
 */

public class Win32ShellFolderManager2 extends ShellFolderManager {


    public ShellFolder createShellFolder(File file) throws FileNotFoundException {
        try {
            return createShellFolder(getDesktop(), file);
        } catch (InterruptedException e) {
            throw new FileNotFoundException("Execution was interrupted");
        }
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    static Win32ShellFolder2 createShellFolder(Win32ShellFolder2 parent, File file)
            throws FileNotFoundException, InterruptedException {
        cli.System.IntPtr pIDL = null;
        try {
            pIDL = parent.parseDisplayName(file.getCanonicalPath());
        } catch (IOException ex) {
            pIDL = null;
        }
        if (pIDL == null || cli.System.IntPtr.Zero.Equals(pIDL) ) {
            // Shouldn't happen but watch for it anyway
            throw new FileNotFoundException("File " + file.getAbsolutePath() + " not found");
        }

        try {
            return createShellFolderFromRelativePIDL(parent, pIDL);
        } finally {
            Win32ShellFolder2.releasePIDL(pIDL);
        }
    }
    
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    static Win32ShellFolder2 createShellFolderFromRelativePIDL(Win32ShellFolder2 parent, cli.System.IntPtr pIDL)
            throws InterruptedException {
        // Walk down this relative pIDL, creating new nodes for each of the entries
        while (pIDL != null && !cli.System.IntPtr.Zero.Equals( pIDL ) ) {
        	cli.System.IntPtr curPIDL = Win32ShellFolder2.copyFirstPIDLEntry(pIDL);
            if (curPIDL != null && !cli.System.IntPtr.Zero.Equals( curPIDL )) {
                parent = new Win32ShellFolder2(parent, curPIDL);
                pIDL = Win32ShellFolder2.getNextPIDLEntry(pIDL);
            } else {
                // The list is empty if the parent is Desktop and pIDL is a shortcut to Desktop
                break;
            }
        }
        return parent;
    }

    private static final int VIEW_LIST = 2;
    private static final int VIEW_DETAILS = 3;
    private static final int VIEW_PARENTFOLDER = 8;
    private static final int VIEW_NEWFOLDER = 11;

    private static final Image[] STANDARD_VIEW_BUTTONS = new Image[12];

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private static Image getStandardViewButton(int iconIndex) {
        Image result = STANDARD_VIEW_BUTTONS[iconIndex];

        if (result != null) {
            return result;
        }

        Bitmap bitmap = Win32ShellFolder2.getStandardViewButton0(iconIndex);
        if( bitmap != null ) {
        	result = new BufferedImage(bitmap);
        	STANDARD_VIEW_BUTTONS[iconIndex] = result;
        }

        return result;
    }

    // Special folders
    private static Win32ShellFolder2 desktop;
    private static Win32ShellFolder2 drives;
    private static Win32ShellFolder2 recent;
    private static Win32ShellFolder2 network;
    private static Win32ShellFolder2 personal;

    static Win32ShellFolder2 getDesktop() {
        if (desktop == null) {
            try {
                desktop = new Win32ShellFolder2(DESKTOP);
            } catch (SecurityException e) {
                // Ignore error
            } catch (IOException e) {
                // Ignore error
            } catch (InterruptedException e) {
                // Ignore error
            }
        }
        return desktop;
    }

    static Win32ShellFolder2 getDrives() {
        if (drives == null) {
            try {
                drives = new Win32ShellFolder2(DRIVES);
            } catch (SecurityException e) {
                // Ignore error
            } catch (IOException e) {
                // Ignore error
            } catch (InterruptedException e) {
                // Ignore error
            }
        }
        return drives;
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    static Win32ShellFolder2 getRecent() {
        if (recent == null) {
            try {
                String path = Win32ShellFolder2.getFileSystemPath(RECENT);
                if (path != null) {
                    recent = createShellFolder(getDesktop(), new File(path));
                }
            } catch (SecurityException e) {
                // Ignore error
            } catch (InterruptedException e) {
                // Ignore error
            } catch (IOException e) {
                // Ignore error
            }
        }
        return recent;
    }

    static Win32ShellFolder2 getNetwork() {
        if (network == null) {
            try {
                network = new Win32ShellFolder2(NETWORK);
            } catch (SecurityException e) {
                // Ignore error
            } catch (IOException e) {
                // Ignore error
            } catch (InterruptedException e) {
                // Ignore error
            }
        }
        return network;
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    static Win32ShellFolder2 getPersonal() {
        if (personal == null) {
            try {
                String path = Win32ShellFolder2.getFileSystemPath(PERSONAL);
                if (path != null) {
                    Win32ShellFolder2 desktop = getDesktop();
                    personal = desktop.getChildByPath(path);
                    if (personal == null) {
                        personal = createShellFolder(getDesktop(), new File(path));
                    }
                    if (personal != null) {
                        personal.setIsPersonal();
                    }
                }
            } catch (SecurityException e) {
                // Ignore error
            } catch (InterruptedException e) {
                // Ignore error
            } catch (IOException e) {
                // Ignore error
            }
        }
        return personal;
    }


    private static File[] roots;

    /**
     * @param key a <code>String</code>
     *  "fileChooserDefaultFolder":
     *    Returns a <code>File</code> - the default shellfolder for a new filechooser
     *  "roots":
     *    Returns a <code>File[]</code> - containing the root(s) of the displayable hierarchy
     *  "fileChooserComboBoxFolders":
     *    Returns a <code>File[]</code> - an array of shellfolders representing the list to
     *    show by default in the file chooser's combobox
     *   "fileChooserShortcutPanelFolders":
     *    Returns a <code>File[]</code> - an array of shellfolders representing well-known
     *    folders, such as Desktop, Documents, History, Network, Home, etc.
     *    This is used in the shortcut panel of the filechooser on Windows 2000
     *    and Windows Me.
     *  "fileChooserIcon <icon>":
     *    Returns an <code>Image</code> - icon can be ListView, DetailsView, UpFolder, NewFolder or
     *    ViewMenu (Windows only).
     *  "optionPaneIcon iconName":
     *    Returns an <code>Image</code> - icon from the system icon list
     *
     * @return An Object matching the key string.
     */
    public Object get(String key) {
        if (key.equals("fileChooserDefaultFolder")) {
            File file = getPersonal();
            if (file == null) {
                file = getDesktop();
            }
            return checkFile(file);
        } else if (key.equals("roots")) {
            // Should be "History" and "Desktop" ?
            if (roots == null) {
                File desktop = getDesktop();
                if (desktop != null) {
                    roots = new File[] { desktop };
                } else {
                    roots = (File[])super.get(key);
                }
            }
            return checkFiles(roots);
        } else if (key.equals("fileChooserComboBoxFolders")) {
            Win32ShellFolder2 desktop = getDesktop();

            if (desktop != null && checkFile(desktop) != null) {
                ArrayList<File> folders = new ArrayList<File>();
                Win32ShellFolder2 drives = getDrives();

                Win32ShellFolder2 recentFolder = getRecent();
                if (recentFolder != null && OSInfo.getWindowsVersion().compareTo(OSInfo.WINDOWS_2000) >= 0) {
                    folders.add(recentFolder);
                }

                folders.add(desktop);
                // Add all second level folders
                File[] secondLevelFolders = checkFiles(desktop.listFiles());
                Arrays.sort(secondLevelFolders);
                for (File secondLevelFolder : secondLevelFolders) {
                    Win32ShellFolder2 folder = (Win32ShellFolder2) secondLevelFolder;
                    if (!folder.isFileSystem() || (folder.isDirectory() && !folder.isLink())) {
                        folders.add(folder);
                        // Add third level for "My Computer"
                        if (folder.equals(drives)) {
                            File[] thirdLevelFolders = checkFiles(folder.listFiles());
                            if (thirdLevelFolders != null && thirdLevelFolders.length > 0) {
                                List<File> thirdLevelFoldersList = Arrays.asList(thirdLevelFolders);

                                folder.sortChildren(thirdLevelFoldersList);
                                folders.addAll(thirdLevelFoldersList);
                            }
                        }
                    }
                }
                return checkFiles(folders);
            } else {
                return super.get(key);
            }
        } else if (key.equals("fileChooserShortcutPanelFolders")) {
            Toolkit toolkit = Toolkit.getDefaultToolkit();
            ArrayList<File> folders = new ArrayList<File>();
            int i = 0;
            Object value;
            do {
                value = toolkit.getDesktopProperty("win.comdlg.placesBarPlace" + i++);
                try {
                    if (value instanceof Integer) {
                        // A CSIDL
                        folders.add(new Win32ShellFolder2((Integer)value));
                    } else if (value instanceof String) {
                        // A path
                        folders.add(createShellFolder(new File((String)value)));
                    }
                } catch (IOException e) {
                    // Skip this value
                } catch (InterruptedException e) {
                    // Return empty result
                    return new File[0];
                }
            } while (value != null);

            if (folders.size() == 0) {
                // Use default list of places
                for (File f : new File[] {
                    getRecent(), getDesktop(), getPersonal(), getDrives(), getNetwork()
                }) {
                    if (f != null) {
                        folders.add(f);
                    }
                }
            }
            return checkFiles(folders);
        } else if (key.startsWith("fileChooserIcon ")) {
            String name = key.substring(key.indexOf(" ") + 1);

            int iconIndex;

            if (name.equals("ListView") || name.equals("ViewMenu")) {
                iconIndex = VIEW_LIST;
            } else if (name.equals("DetailsView")) {
                iconIndex = VIEW_DETAILS;
            } else if (name.equals("UpFolder")) {
                iconIndex = VIEW_PARENTFOLDER;
            } else if (name.equals("NewFolder")) {
                iconIndex = VIEW_NEWFOLDER;
            } else {
                return null;
            }

            return getStandardViewButton(iconIndex);
        } else if (key.startsWith("optionPaneIcon ")) {
            cli.System.Drawing.Icon icon;
            if (key == "optionPaneIcon Error") {
                icon = SystemIcons.get_Error();
            } else if (key == "optionPaneIcon Information") {
                icon = SystemIcons.get_Information();
            } else if (key == "optionPaneIcon Question") {
                icon = SystemIcons.get_Question();
            } else if (key == "optionPaneIcon Warning") {
                icon = SystemIcons.get_Warning();
            } else {
                return null;
            }
            return new BufferedImage(icon.ToBitmap());
        } else if (key.startsWith("shell32Icon ") || key.startsWith("shell32LargeIcon ")) {
            String name = key.substring(key.indexOf(" ") + 1);
            try {
                int i = Integer.parseInt(name);
                if (i >= 0) {
                    return Win32ShellFolder2.getShell32Icon(i, key.startsWith("shell32LargeIcon "));
                }
            } catch (NumberFormatException ex) {
            }
        }
        return null;
    }

    private File checkFile(File file) {
        SecurityManager sm = System.getSecurityManager();
        return (sm == null || file == null) ? file : checkFile(file, sm);
    }

    private File checkFile(File file, SecurityManager sm) {
        try {
            sm.checkRead(file.getPath());
            return file;
        } catch (SecurityException se) {
            return null;
        }
    }

    private File[] checkFiles(File[] files) {
        SecurityManager sm = System.getSecurityManager();
        if (sm == null || files == null || files.length == 0) {
            return files;
        }
        return checkFiles(Arrays.stream(files), sm);
    }

    private File[] checkFiles(List<File> files) {
        SecurityManager sm = System.getSecurityManager();
        if (sm == null || files.isEmpty()) {
            return files.toArray(new File[files.size()]);
        }
        return checkFiles(files.stream(), sm);
    }

    private File[] checkFiles(Stream<File> filesStream, SecurityManager sm) {
        return filesStream.filter((file) -> checkFile(file, sm) != null)
                .toArray(File[]::new);
    }

    /**
     * Does <code>dir</code> represent a "computer" such as a node on the network, or
     * "My Computer" on the desktop.
     */
    public boolean isComputerNode(final File dir) {
        if (dir != null && dir == getDrives()) {
            return true;
        } else {
            String path = AccessController.doPrivileged(new PrivilegedAction<String>() {
                public String run() {
                    return dir.getAbsolutePath();
                }
            });

            return (path.startsWith("\\\\") && path.indexOf("\\", 2) < 0);      //Network path
        }
    }

    public boolean isFileSystemRoot(File dir) {
        //Note: Removable drives don't "exist" but are listed in "My Computer"
        if (dir != null) {
            Win32ShellFolder2 drives = getDrives();
            if (dir instanceof Win32ShellFolder2) {
                Win32ShellFolder2 sf = (Win32ShellFolder2)dir;
                if (sf.isFileSystem()) {
                    if (sf.parent != null) {
                        return sf.parent.equals(drives);
                    }
                    // else fall through ...
                } else {
                    return false;
                }
            }
            String path = dir.getPath();

            if (path.length() != 3 || path.charAt(1) != ':') {
                return false;
            }

            File[] files = drives.listFiles();

            return files != null && Arrays.asList(files).contains(dir);
        }
        return false;
    }

    private static List topFolderList = null;
    static int compareShellFolders(Win32ShellFolder2 sf1, Win32ShellFolder2 sf2) {
        boolean special1 = sf1.isSpecial();
        boolean special2 = sf2.isSpecial();

        if (special1 || special2) {
            if (topFolderList == null) {
                ArrayList tmpTopFolderList = new ArrayList();
                tmpTopFolderList.add(Win32ShellFolderManager2.getPersonal());
                tmpTopFolderList.add(Win32ShellFolderManager2.getDesktop());
                tmpTopFolderList.add(Win32ShellFolderManager2.getDrives());
                tmpTopFolderList.add(Win32ShellFolderManager2.getNetwork());
                topFolderList = tmpTopFolderList;
            }
            int i1 = topFolderList.indexOf(sf1);
            int i2 = topFolderList.indexOf(sf2);
            if (i1 >= 0 && i2 >= 0) {
                return (i1 - i2);
            } else if (i1 >= 0) {
                return -1;
            } else if (i2 >= 0) {
                return 1;
            }
        }

        // Non-file shellfolders sort before files
        if (special1 && !special2) {
            return -1;
        } else if (special2 && !special1) {
            return  1;
        }

        return compareNames(sf1.getAbsolutePath(), sf2.getAbsolutePath());
    }

    static int compareNames(String name1, String name2) {
        // First ignore case when comparing
        int diff = name1.compareToIgnoreCase(name2);
        if (diff != 0) {
            return diff;
        } else {
            // May differ in case (e.g. "mail" vs. "Mail")
            // We need this test for consistent sorting
            return name1.compareTo(name2);
        }
    }
}
