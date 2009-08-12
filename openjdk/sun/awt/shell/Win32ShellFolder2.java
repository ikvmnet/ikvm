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
import cli.System.Drawing.Imaging.BitmapData;
import cli.System.Drawing.Imaging.ImageLockMode;
import cli.System.Drawing.Imaging.PixelFormat;
import cli.System.Runtime.InteropServices.Marshal;

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

    public static final int DESKTOPDIRECTORY = cli.System.Environment.SpecialFolder.DesktopDirectory;

    public static final int DRIVES = cli.System.Environment.SpecialFolder.MyComputer;

    public static final int NETWORK = 0x0012;

    public static final int NETHOOD = 0x0013;

    public static final int FONTS = 0x0014;

    public static final int TEMPLATES = cli.System.Environment.SpecialFolder.Templates;

    public static final int COMMON_STARTMENU = 0x0016;

    public static final int COMMON_PROGRAMS = 0X0017;

    public static final int COMMON_STARTUP = 0x0018;

    public static final int COMMON_DESKTOPDIRECTORY = 0x0019;

    public static final int APPDATA = cli.System.Environment.SpecialFolder.ApplicationData;

    public static final int PRINTHOOD = 0x001b;

    public static final int ALTSTARTUP = 0x001d;

    public static final int COMMON_ALTSTARTUP = 0x001e;

    public static final int COMMON_FAVORITES = 0x001f;

    public static final int INTERNET_CACHE = cli.System.Environment.SpecialFolder.InternetCache;

    public static final int COOKIES = cli.System.Environment.SpecialFolder.Cookies;

    public static final int HISTORY = cli.System.Environment.SpecialFolder.History;

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

    static Image[] fileChooserIcons;
    
    private Image smallIcon = null;
    
    private Image largeIcon = null;
    
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
        // TODO Sun is using IShellFolder::GetDisplayNameOf instead of this hack
        String name = getName();
        if(name.endsWith(".lnk")){
            name = name.substring(0, name.length() - 4);
        }
        return name;
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

    static Image getFileChooserIcon(int idx){
        if(fileChooserIcons == null){
            fileChooserIcons = new Image[47];

            try{
                int[] data = getFileChooserBitmapHandle();
                Bitmap bitmap = new Bitmap(data.length/16, 16, PixelFormat.wrap(PixelFormat.Format32bppArgb));
                BitmapData bitmapData = bitmap.LockBits(new cli.System.Drawing.Rectangle(0,0,bitmap.get_Width(), bitmap.get_Height()), ImageLockMode.wrap(ImageLockMode.WriteOnly), PixelFormat.wrap(PixelFormat.Format32bppArgb));
                Marshal.Copy(data, 0, bitmapData.get_Scan0(), data.length);
                bitmap.UnlockBits(bitmapData);
                
                for(int i = 0; i < fileChooserIcons.length; i++){
                    cli.System.Drawing.Rectangle rect = new cli.System.Drawing.Rectangle(16 * i, 0, 16, 16);
                    Bitmap icon = bitmap.Clone(rect, bitmap.get_PixelFormat());
                    fileChooserIcons[i] = new BufferedImage(icon);
                }
            }catch(Throwable ex){
                ex.printStackTrace();
            }
        }
        return fileChooserIcons[idx];
    }
    
    private static native int[] getFileChooserBitmapHandle();
    
    private static native boolean DeleteObject(cli.System.IntPtr hDc);
    
    /**
     * Gets an icon from the Windows system icon list as an <code>Image</code>
     */
    static Image getShell32Icon(int iconID) {
        cli.System.IntPtr hIcon = getIconResource("shell32.dll", iconID, 16, 16);
        if(hIcon.ToInt32() == 0){
            return null;
        }
        Bitmap bitmap = Bitmap.FromHicon(hIcon);
        DeleteObject(hIcon);
        return new BufferedImage(bitmap);
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

    /**
     * @return An array of shell folders that are children of this shell folder
     *         object. The array will be empty if the folder is empty.  Returns
     *         <code>null</code> if this shell folder does not denote a directory.
     */
    @Override
    public File[] listFiles(final boolean includeHiddenFiles) {
        SecurityManager security = System.getSecurityManager();
        if (security != null) {
            security.checkRead(getPath());
        }
        if (!isDirectory()) {
            return null;
        }
        // Links to directories are not directories and cannot be parents.
        // This does not apply to folders in My Network Places (NetHood)
        // because they are both links and real directories!
        if (isLink() && !hasAttribute(ATTRIB_FOLDER)) {
            return new File[0];
        }
        
        File[] files = super.listFiles(includeHiddenFiles);
        Win32ShellFolder2[] shellFiles = new Win32ShellFolder2[files.length];
        for(int i = 0; i < files.length; i++){
            File file = files[i];
            shellFiles[i] = new Win32ShellFolder2( this, file.getPath());
        }
        return shellFiles;
    }
    
    /**
     * @return The icon image used to display this shell folder
     */
    @Override
    public Image getIcon(final boolean getLargeIcon) {
        Image icon = getLargeIcon ? largeIcon : smallIcon;
        if (icon == null) {
        	int size = getLargeIcon ? 32 : 16;
            cli.System.IntPtr hIcon = getIcon( getPath(), getLargeIcon);
            if(hIcon.ToInt32() == 0){
                return null;
            }
            DeleteObject(hIcon);
            int[] iconPixels = getIconBits(hIcon, size);
            Bitmap bitmap = new Bitmap(size, size, PixelFormat.wrap(PixelFormat.Format32bppArgb));
            BitmapData bitmapData = bitmap.LockBits(new cli.System.Drawing.Rectangle(0,0,size, size), ImageLockMode.wrap(ImageLockMode.WriteOnly), PixelFormat.wrap(PixelFormat.Format32bppArgb));
            Marshal.Copy(iconPixels, 0, bitmapData.get_Scan0(), iconPixels.length);
            bitmap.UnlockBits(bitmapData);
            
            icon = new BufferedImage(bitmap);
            if (getLargeIcon) {
                largeIcon = icon;
            } else {
                smallIcon = icon;
            }
        }
        return icon;
    }
    
    // Return the icon of a file system shell folder in the form of an HICON
    private static native cli.System.IntPtr getIcon(String absolutePath, boolean getLargeIcon);
    
    private static native int[] getIconBits(cli.System.IntPtr hIcon, int iconSize);

}
