/*
  Copyright (C) 2009 Volker Berlin (i-net software)

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
 */

/*
 * Copyright 2003-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.awt.shell;

import java.awt.Image;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.ObjectStreamException;

import cli.System.Drawing.Bitmap;

/**
 * @author Volker Berlin
 */
public class Win32ShellFolder2 extends ShellFolder{

    private String folderType;

    // Win32 Shell Folder Constants
    public static final int DESKTOP = cli.System.Environment.SpecialFolder.Desktop;

    public static final int INTERNET = 0x0001;

    public static final int PROGRAMS = cli.System.Environment.SpecialFolder.Programs;

    public static final int CONTROLS = 0x0003;

    public static final int PRINTERS = 0x0004;

    public static final int PERSONAL = cli.System.Environment.SpecialFolder.Personal;

    public static final int FAVORITES = cli.System.Environment.SpecialFolder.Favorites;

    public static final int STARTUP = cli.System.Environment.SpecialFolder.Startup;

    public static final int RECENT = cli.System.Environment.SpecialFolder.Recent;

    public static final int SENDTO = cli.System.Environment.SpecialFolder.SendTo;

    public static final int BITBUCKET = 0x000a;

    public static final int STARTMENU = cli.System.Environment.SpecialFolder.StartMenu;

    public static final int DESKTOPDIRECTORY = 0x0010;

    public static final int DRIVES = 0x0011;

    public static final int NETWORK = 0x0012;

    public static final int NETHOOD = 0x0013;

    public static final int FONTS = 0x0014;

    public static final int TEMPLATES = cli.System.Environment.SpecialFolder.Templates;

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

    public static final int COOKIES = cli.System.Environment.SpecialFolder.Cookies;

    public static final int HISTORY = 0x0022;

    // Win32 shell folder attributes
    public static final int ATTRIB_CANCOPY = 0x00000001;

    public static final int ATTRIB_CANMOVE = 0x00000002;

    public static final int ATTRIB_CANLINK = 0x00000004;

    public static final int ATTRIB_CANRENAME = 0x00000010;

    public static final int ATTRIB_CANDELETE = 0x00000020;

    public static final int ATTRIB_HASPROPSHEET = 0x00000040;

    public static final int ATTRIB_DROPTARGET = 0x00000100;

    public static final int ATTRIB_LINK = 0x00010000;

    public static final int ATTRIB_SHARE = 0x00020000;

    public static final int ATTRIB_READONLY = 0x00040000;

    public static final int ATTRIB_GHOSTED = 0x00080000;

    public static final int ATTRIB_HIDDEN = 0x00080000;

    public static final int ATTRIB_FILESYSANCESTOR = 0x10000000;

    public static final int ATTRIB_FOLDER = 0x20000000;

    public static final int ATTRIB_FILESYSTEM = 0x40000000;

    public static final int ATTRIB_HASSUBFOLDER = 0x80000000;

    public static final int ATTRIB_VALIDATE = 0x01000000;

    public static final int ATTRIB_REMOVABLE = 0x02000000;

    public static final int ATTRIB_COMPRESSED = 0x04000000;

    public static final int ATTRIB_BROWSABLE = 0x08000000;

    public static final int ATTRIB_NONENUMERATED = 0x00100000;

    public static final int ATTRIB_NEWCONTENT = 0x00200000;

    // IShellFolder::GetDisplayNameOf constants
    public static final int SHGDN_NORMAL = 0;

    public static final int SHGDN_INFOLDER = 1;

    public static final int SHGDN_INCLUDE_NONFILESYS = 0x2000;

    public static final int SHGDN_FORADDRESSBAR = 0x4000;

    public static final int SHGDN_FORPARSING = 0x8000;

    /**
     * The following is to identify the My Documents folder as being special
     */
    private boolean isPersonal;

    static Image[] fileChooserIcons = new Image[47];
    
    /**
     * @param folder
     *            value of the Enumeration cli.System.Environment.SpecialFolder
     */
    Win32ShellFolder2(int folder) throws IOException{
        super(null, getFileSystemPath(folder));
    }


    Win32ShellFolder2(File file) throws IOException{
        super(getParent(file), file.getCanonicalPath());
    }


    Win32ShellFolder2(String filename){
        super(getParent(new File(filename)), filename);
    }


    Win32ShellFolder2(Win32ShellFolder2 parent, String filename){
        super(parent, filename);
    }


    static String getFileSystemPath(int folder) throws IOException{
        try{
            return cli.System.Environment.GetFolderPath(cli.System.Environment.SpecialFolder.wrap(folder));
        }catch(Throwable ex){
            throw new IOException(ex);
        }
    }


    private static Win32ShellFolder2 getParent(File file){
        String parent = file.getParent();
        return parent !=null ? new Win32ShellFolder2(parent) : null;
    }


    @Override
    public String getDisplayName(){
        return getName();
    }


    @Override
    public String getExecutableType(){
        if(!isFileSystem()){
            return null;
        }
        return getExecutableType(getAbsolutePath());
    }


    /**
     * Use FindExecutable in shell32
     */
    private static native String getExecutableType(String path);


    @Override
    public String getFolderType(){
        if(folderType == null){
            folderType = getFolderType(getAbsolutePath());
        }
        return folderType;
    }


    private static native String getFolderType(String path);


    @Override
    public ShellFolder getLinkLocation() throws FileNotFoundException{
        if(!isLink()){
            return null;
        }
        return new Win32ShellFolder2(getLinkLocation(getPath()));
    }


    private static native String getLinkLocation(String path);


    @Override
    public boolean isLink(){
        return hasAttribute(ATTRIB_LINK);
    }


    @Override
    protected Object writeReplace() throws ObjectStreamException{
        return new File(getPath());
    }


    /**
     * @return Whether this is a file system shell folder
     */
    @Override
    public boolean isFileSystem() {
        return hasAttribute(ATTRIB_FILESYSTEM);
    }

    public boolean hasAttribute(int attribute){
        return (getAttribute(getPath()) & attribute) == attribute;
    }


    private static native int getAttribute(String path);

    static Image getFileChooserIcon(int i) {
        if (fileChooserIcons[i] != null) {
            return fileChooserIcons[i];
        }
        return new BufferedImage(Bitmap.FromHbitmap(getFileChooserBitmapHandle()));
    }
    
    private static native cli.System.IntPtr getFileChooserBitmapHandle();
    
    /**
     * Gets an icon from the Windows system icon list as an <code>Image</code>
     */
    static Image getShell32Icon(int iconID) {
        cli.System.IntPtr hIcon = getIconResource("shell32.dll", iconID, 16, 16);
        return new BufferedImage(Bitmap.FromHicon(hIcon));
    }
    
    private static native cli.System.IntPtr getIconResource(String libName, int iconID, int cxDesired, int cyDesired);

    /** Marks this folder as being the My Documents (Personal) folder */
    public void setIsPersonal(){
        isPersonal = true;
    }


    /**
     * Indicates whether this is a special folder (includes My Documents)
     */
    public boolean isSpecial(){
        return isPersonal || !isFileSystem() || (this == Win32ShellFolderManager2.getDesktop());
    }

}
