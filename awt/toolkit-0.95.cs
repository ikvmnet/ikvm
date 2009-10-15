/*
 * Copyright 1996-2007 Sun Microsystems, Inc.  All Rights Reserved.
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

/*
Copyright (C) 2002, 2004-2009 Jeroen Frijters
Copyright (C) 2006 Active Endpoints, Inc.
Copyright (C) 2006, 2007, 2008, 2009 Volker Berlin (i-net software)

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

using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using java.net;
using java.util;
using ikvm.awt.printing;
using ikvm.runtime;
using sun.awt;
using System.Drawing.Imaging;
using sun.awt.dnd;

namespace ikvm.awt
{
	delegate void SetVoid();
	delegate void SetBool(bool b);
	delegate void SetInt(int i);
	delegate void SetXYWH(int x, int y, int w, int h);
	delegate void SetString(string s);
	delegate string GetString();
	delegate void SetStringInt(string s, int i);
	delegate void SetRectangle(Rectangle r);
	delegate void SetColor(java.awt.Color c);
    delegate void SetCursor(java.awt.Cursor cursor);
	delegate java.awt.Dimension GetDimension();
    delegate Rectangle ConvertRectangle(Rectangle r);
    delegate object GetObject();
    internal delegate T Func<T>();

	class UndecoratedForm : Form
	{
		public UndecoratedForm()
		{
			setBorderStyle();
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}

        protected virtual void setBorderStyle()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        internal void setFocusableWindow(bool value)
        {
            SetStyle(ControlStyles.Selectable, value);
        }

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// JDK sets a NULL background brush, we emulate that by not doing any background painting
		}
	}

    class MyForm : UndecoratedForm
	{
        /// <summary>
        /// Original MaximizedBounds
        /// </summary>
        private Rectangle maxBounds;
        private bool maxBoundsSet;

		public MyForm()
		{
		}

        protected override void setBorderStyle()
        {
            //nothing, default behaviour
        }

        public void setMaximizedBounds(java.awt.Rectangle rect)
        {
            if (rect == null)
            {
                // null means reset to the original system setting
                if (maxBoundsSet)
                {
                    MaximizedBounds = maxBounds;
                }
            }
            else
            {
                if (!maxBoundsSet)
                {
                    maxBounds = MaximizedBounds;
                    maxBoundsSet = true;
                }
                MaximizedBounds = J2C.ConvertRect(rect);
            }
        }
	}

	class MyControl : Control
	{
		public MyControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}
	}

	class MyContainerControl : ContainerControl
	{
		public MyContainerControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}
	}

    public class NetToolkit : sun.awt.SunToolkit, ikvm.awt.IkvmToolkit
    {
        public static readonly String DATA_TRANSFERER_CLASS_NAME = typeof(NetDataTransferer).AssemblyQualifiedName;

        internal static volatile Form bogusForm;
        private int resolution;
        private NetClipboard clipboard;

        private static void MessageLoop()
        {
            using (Form form = new Form())
            {
				CreateNative(form);
                bogusForm = form;
                // FXBUG to make sure we can be aborted (Thread.Abort) we need to periodically
                // fire an event (because otherwise we'll be blocking in unmanaged code and
                // the Abort cannot be handled there).
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 100;
                t.Start();
                Application.Run();
            }
        }

		internal static void CreateNative(Control control)
		{
			control.CreateControl();
			// HACK I have no idea why this line is necessary...
			IntPtr p = control.Handle;
			if (p == IntPtr.Zero)
			{
				// shut up compiler warning
			}
		}

        public NetToolkit()
        {
            lock (typeof(NetToolkit))
            {
                System.Diagnostics.Debug.Assert(bogusForm == null);
                setDataTransfererClassName(DATA_TRANSFERER_CLASS_NAME);

                Thread thread = new Thread(new ThreadStart(MessageLoop));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Name = "IKVM AWT WinForms Message Loop";
                thread.IsBackground = true;
                thread.Start();
                // TODO don't use polling...
                while (bogusForm == null && thread.IsAlive)
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// Run on a win 32 system
        /// </summary>
        /// <returns></returns>
        internal static bool isWin32()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows;
        }

        protected override void loadSystemColors(int[] systemColors)
        {
            // initialize all colors to purple to make the ones we might have missed stand out
            for (int i = 0; i < systemColors.Length; i++)
            {
                systemColors[i] = Color.Purple.ToArgb();
            }
            systemColors[java.awt.SystemColor.DESKTOP] = SystemColors.Desktop.ToArgb();
            systemColors[java.awt.SystemColor.ACTIVE_CAPTION] = SystemColors.ActiveCaption.ToArgb();
            systemColors[java.awt.SystemColor.ACTIVE_CAPTION_TEXT] = SystemColors.ActiveCaptionText.ToArgb();
            systemColors[java.awt.SystemColor.ACTIVE_CAPTION_BORDER] = SystemColors.ActiveBorder.ToArgb();
            systemColors[java.awt.SystemColor.INACTIVE_CAPTION] = SystemColors.InactiveCaption.ToArgb();
            systemColors[java.awt.SystemColor.INACTIVE_CAPTION_TEXT] = SystemColors.InactiveCaptionText.ToArgb();
            systemColors[java.awt.SystemColor.INACTIVE_CAPTION_BORDER] = SystemColors.InactiveBorder.ToArgb();
            systemColors[java.awt.SystemColor.WINDOW] = SystemColors.Window.ToArgb();
            systemColors[java.awt.SystemColor.WINDOW_BORDER] = SystemColors.WindowFrame.ToArgb();
            systemColors[java.awt.SystemColor.WINDOW_TEXT] = SystemColors.WindowText.ToArgb();
            systemColors[java.awt.SystemColor.MENU] = SystemColors.Menu.ToArgb();
            systemColors[java.awt.SystemColor.MENU_TEXT] = SystemColors.MenuText.ToArgb();
            systemColors[java.awt.SystemColor.TEXT] = SystemColors.Window.ToArgb();
            systemColors[java.awt.SystemColor.TEXT_TEXT] = SystemColors.WindowText.ToArgb();
            systemColors[java.awt.SystemColor.TEXT_HIGHLIGHT] = SystemColors.Highlight.ToArgb();
            systemColors[java.awt.SystemColor.TEXT_HIGHLIGHT_TEXT] = SystemColors.HighlightText.ToArgb();
            systemColors[java.awt.SystemColor.TEXT_INACTIVE_TEXT] = SystemColors.GrayText.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL] = SystemColors.Control.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL_TEXT] = SystemColors.ControlText.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL_HIGHLIGHT] = SystemColors.ControlLight.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL_LT_HIGHLIGHT] = SystemColors.ControlLightLight.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL_SHADOW] = SystemColors.ControlDark.ToArgb();
            systemColors[java.awt.SystemColor.CONTROL_DK_SHADOW] = SystemColors.ControlDarkDark.ToArgb();
            systemColors[java.awt.SystemColor.SCROLLBAR] = SystemColors.ScrollBar.ToArgb();
            systemColors[java.awt.SystemColor.INFO] = SystemColors.Info.ToArgb();
            systemColors[java.awt.SystemColor.INFO_TEXT] = SystemColors.InfoText.ToArgb();
        }

        public override java.awt.peer.ButtonPeer createButton(java.awt.Button target)
        {
            java.awt.peer.ButtonPeer peer = new NetButtonPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
        {
            java.awt.peer.TextFieldPeer peer = new NetTextFieldPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
        {
            java.awt.peer.LabelPeer peer = new NetLabelPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ListPeer createList(java.awt.List target)
        {
            java.awt.peer.ListPeer peer = new NetListPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
        {
            java.awt.peer.CheckboxPeer peer = new NetCheckboxPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ScrollbarPeer createScrollbar(java.awt.Scrollbar target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.ScrollPanePeer createScrollPane(java.awt.ScrollPane target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.TextAreaPeer createTextArea(java.awt.TextArea target)
        {
            java.awt.peer.TextAreaPeer peer = new NetTextAreaPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
        {
            java.awt.peer.ChoicePeer peer = new NetChoicePeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
        {
            java.awt.peer.FramePeer peer = (NetFramePeer)bogusForm.Invoke((GetObject)delegate { return new NetFramePeer(target); });
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
        {
            java.awt.peer.WindowPeer peer = (NetWindowPeer)bogusForm.Invoke((GetObject)delegate { return new NetWindowPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
        {
            java.awt.peer.DialogPeer peer = (java.awt.peer.DialogPeer)bogusForm.Invoke((GetObject)delegate { return new NetDialogPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.MenuBarPeer createMenuBar(java.awt.MenuBar target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.MenuPeer createMenu(java.awt.Menu target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.PopupMenuPeer createPopupMenu(java.awt.PopupMenu target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.MenuItemPeer createMenuItem(java.awt.MenuItem target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.FileDialogPeer createFileDialog(java.awt.FileDialog target)
        {
            return new NetFileDialogPeer(target);
        }

        public override java.awt.peer.CheckboxMenuItemPeer createCheckboxMenuItem(java.awt.CheckboxMenuItem target)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.FontPeer getFontPeer(string name, int style)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.KeyboardFocusManagerPeer createKeyboardFocusManagerPeer(java.awt.KeyboardFocusManager manager)
        {
            return new NetKeyboardFocusManagerPeer(manager);
        }

        public override java.awt.Dimension getScreenSize()
        {
            return new java.awt.Dimension(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        public override int getScreenResolution()
        {
            if (resolution == 0)
            {
                using (Graphics g = bogusForm.CreateGraphics())
                {
                    resolution = (int)Math.Round(g.DpiY);
                }
            }
            return resolution;
        }

        public override java.awt.image.ColorModel getColorModel()
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override string[] getFontList()
        {
            // This method is deprecated and Sun's JDK only returns these fonts as well
            return new string[] { "Dialog", "SansSerif", "Serif", "Monospaced", "DialogInput" };
        }

        [Obsolete]
        public override java.awt.FontMetrics getFontMetrics(java.awt.Font font)
        {
            return new NetFontMetrics(font);
        }

        public override void sync()
        {
            throw new NotImplementedException();
        }

        public override java.awt.Image getImage(string filename)
        {
            try
            {
                filename = new java.io.File(filename).getPath(); //convert a Java file name to .NET filename (slahes, backslasches, etc)
                using (System.IO.FileStream stream = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return new java.awt.image.BufferedImage(new Bitmap(Image.FromStream(stream)));
                }
            }
            catch (Exception)
            {
                return new NoImage();
            }
        }

        public override java.awt.Image getImage(URL url)
        {
            // TODO extremely lame...
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            java.io.InputStream inS = url.openStream();
            int b;
            while ((b = inS.read()) >= 0)
            {
                mem.WriteByte((byte)b);
            }
            try
            {
                mem.Position = 0;
                return new java.awt.image.BufferedImage(new Bitmap(Image.FromStream(mem)));
            }
            catch
            {
                return new NoImage();
            }
        }

        public override java.awt.Image createImage(string filename)
        {
            return getImage(filename);
        }

        public override java.awt.Image createImage(URL url)
        {
            return getImage(url);
        }

        const int ERROR = java.awt.image.ImageObserver.__Fields.ERROR;
        const int ABORT = java.awt.image.ImageObserver.__Fields.ABORT;
        const int WIDTH = java.awt.image.ImageObserver.__Fields.WIDTH;
        const int HEIGHT = java.awt.image.ImageObserver.__Fields.HEIGHT;
        const int FRAMEBITS = java.awt.image.ImageObserver.__Fields.FRAMEBITS;
        const int ALLBITS = java.awt.image.ImageObserver.__Fields.ALLBITS;

        public override bool prepareImage(java.awt.Image image, int width, int height, java.awt.image.ImageObserver observer)
        {
            // HACK for now we call checkImage to obtain the status and fire the observer
            return (checkImage(image, width, height, observer) & (ALLBITS | ERROR | ABORT)) != 0;
        }

        public override int checkImage(java.awt.Image image, int width, int height, java.awt.image.ImageObserver observer)
        {
            if (image.getWidth(null) == -1)
            {
                if (observer != null)
                {
                    observer.imageUpdate(image, ERROR | ABORT, 0, 0, -1, -1);
                }
                return ERROR | ABORT;
            }
            if (observer != null)
            {
                observer.imageUpdate(image, WIDTH + HEIGHT + FRAMEBITS + ALLBITS, 0, 0, image.getWidth(null), image.getHeight(null));
            }
            return WIDTH + HEIGHT + FRAMEBITS + ALLBITS;
        }

        public override java.awt.Image createImage(java.awt.image.ImageProducer producer)
        {
            NetProducerImage img = new NetProducerImage(producer);
            if (producer != null)
            {
                producer.startProduction(img);
            }
            return img;
        }

        public override java.awt.Image createImage(byte[] imagedata, int imageoffset, int imagelength)
        {
            try
            {
                return new java.awt.image.BufferedImage(new Bitmap(new MemoryStream(imagedata, imageoffset, imagelength, false)));
            }
            catch (Exception)
            {
                return new NoImage();//TODO should throw the exception unstead of NoImage()
            }
        }

        public override java.awt.PrintJob getPrintJob(java.awt.Frame frame, string jobtitle, Properties props)
        {
            throw new NotImplementedException();
        }

        public override void beep()
        {
            Console.Beep();
        }

        public override java.awt.datatransfer.Clipboard getSystemClipboard()
        {
            lock(this)
            {
                if (clipboard==null)
                {
                    clipboard = new NetClipboard();
                }
            }
            return clipboard;
        }

        public override java.awt.dnd.DragGestureRecognizer createDragGestureRecognizer(java.lang.Class abstractRecognizerClass, java.awt.dnd.DragSource ds, java.awt.Component c, int srcActions, java.awt.dnd.DragGestureListener dgl)
        {
            java.lang.Class clazz = typeof(java.awt.dnd.MouseDragGestureRecognizer);
            if (abstractRecognizerClass == clazz)
                return new NetMouseDragGestureRecognizer(ds, c, srcActions, dgl);
            else
                return null;
        }

        public override java.awt.dnd.peer.DragSourceContextPeer createDragSourceContextPeer(java.awt.dnd.DragGestureEvent dge)
        {
            return NetDragSourceContextPeer.createDragSourceContextPeer(dge);
        }

        public override Map mapInputMethodHighlight(java.awt.im.InputMethodHighlight highlight)
        {
            throw new NotImplementedException();
        }

#if false
		protected override java.awt.peer.LightweightPeer createComponent(java.awt.Component target)
		{
			if(target is java.awt.Container)
			{
				return new NetLightweightContainerPeer((java.awt.Container)target);
			}
			return new NetLightweightComponentPeer(target);
		}
#endif

/*        public override java.awt.Font createFont(int format, java.io.InputStream stream)
        {
            throw new NotImplementedException();
        }

        public override gnu.java.awt.peer.ClasspathFontPeer getClasspathFontPeer(string name, java.util.Map attrs)
        {
            return new NetFontPeer(name, attrs);
        }

        public override java.awt.GraphicsEnvironment getLocalGraphicsEnvironment()
        {
            return new NetGraphicsEnvironment();
        }

        public override RobotPeer createRobot(java.awt.GraphicsDevice screen)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows)
            {
                return new WindowsRobot(screen);
            }
            throw new java.awt.AWTException("Robot not supported for this OS");
        }

        public override gnu.java.awt.peer.EmbeddedWindowPeer createEmbeddedWindow(gnu.java.awt.EmbeddedWindow ew)
        {
            throw new NotImplementedException();
        }
*/
        protected override java.awt.peer.DesktopPeer createDesktopPeer(java.awt.Desktop target)
        {
            return new NetDesktopPeer();
        }

        public override java.awt.Dimension getBestCursorSize(int preferredWidth, int preferredHeight)
        {
            // TODO
            return new java.awt.Dimension(preferredWidth, preferredHeight);
        }

        public override java.awt.Cursor createCustomCursor(java.awt.Image cursor, java.awt.Point hotSpot, string name)
        {
            return new NetCustomCursor(cursor, hotSpot, name);
        }

        private object getRegistry(string subKey, string valueName)
        {
			using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, false))
			{
				return key == null ? null : key.GetValue(valueName);
			}
        }

        protected override void initializeDesktopProperties()
        {
            //copied from WToolkit.java
            desktopProperties.put("DnD.Autoscroll.initialDelay", java.lang.Integer.valueOf(50));
            desktopProperties.put("DnD.Autoscroll.interval", java.lang.Integer.valueOf(50));

            try
            {
                if (isWin32())
                {
                    desktopProperties.put("Shell.shellFolderManager", java.lang.Class.forName("sun.awt.shell.Win32ShellFolderManager2"));
                    object themeActive = getRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\ThemeManager", "ThemeActive");
//                    string dllName = (string)getRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\ThemeManager", "DllName");
//                    string sizeName = (string)getRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\ThemeManager", "SizeName");
//                    string colorName = (string)getRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\ThemeManager", "ColorName");
                    desktopProperties.put("win.xpstyle.themeActive", java.lang.Boolean.valueOf("1".Equals(themeActive)));
//                    desktopProperties.put("win.xpstyle.dllName", dllName);
//                    desktopProperties.put("win.xpstyle.sizeName", sizeName);
//                    desktopProperties.put("win.xpstyle.colorName", colorName);
                }
            }
            catch (java.lang.ClassNotFoundException)
            {
            }
        }

        protected override Object lazilyLoadDesktopProperty(String name)
        {
            if ("win.defaultGUI.font".Equals(name))
            {
                Font font = Control.DefaultFont;
                return C2J.ConvertFont(font);
            }
            return null;
        }

        /*===============================
         * Implementations of interface IkvmToolkit
         */

        public java.awt.Graphics2D createGraphics(System.Drawing.Bitmap bitmap)
        {
            return new BitmapGraphics(bitmap);
        }

        /// <summary>
        /// Get a helper class for implementing the print API
        /// </summary>
        /// <returns></returns>
        public sun.print.PrintPeer getPrintPeer()
        {
            if (isWin32())
            {
                return new Win32PrintPeer();
            }
            else
            {
                return new LinuxPrintPeer();
            }
        }

        /*===============================
         * Implementations of interface SunToolkit
         */

        public override bool isModalExclusionTypeSupported(java.awt.Dialog.ModalExclusionType dmet)
        {
            throw new NotImplementedException();
        }

        public override bool isModalityTypeSupported(java.awt.Dialog.ModalityType type)
        {
            return type.ordinal() == java.awt.Dialog.ModalityType.MODELESS.ordinal() ||
                type.ordinal() == java.awt.Dialog.ModalityType.APPLICATION_MODAL.ordinal();
        }

        public override java.awt.Window createInputMethodWindow(string __p1, sun.awt.im.InputContext __p2)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.RobotPeer createRobot(java.awt.Robot r, java.awt.GraphicsDevice screen)
        {
            if (isWin32())
            {
                return new WindowsRobot(screen);
            }
            throw new java.awt.AWTException("Robot not supported for this OS");
        }

        public override java.awt.peer.SystemTrayPeer createSystemTray(java.awt.SystemTray target)
        {
            NetSystemTrayPeer peer = new NetSystemTrayPeer(target);
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.peer.TrayIconPeer createTrayIcon(java.awt.TrayIcon target)
        {
            NetTrayIconPeer peer = new NetTrayIconPeer(target);
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.im.spi.InputMethodDescriptor getInputMethodAdapterDescriptor()
        {
            return null;
        }

        protected override int getScreenHeight()
        {
            return Screen.PrimaryScreen.Bounds.Height;
        }

        protected override int getScreenWidth()
        {
            return Screen.PrimaryScreen.Bounds.Width;
        }

        public override void grab(java.awt.Window window)
        {
            NetWindowPeer peer = (NetWindowPeer)window.getPeer();
            if (peer != null)
            {
                peer.Grab();
            }
        }

        public override bool isDesktopSupported()
        {
            return true;
        }

        public override bool isTraySupported()
        {
            return true;
        }

        public override bool isFrameStateSupported(int state)
        {
            switch (state)
            {
                case java.awt.Frame.NORMAL:
                case java.awt.Frame.ICONIFIED:
                case java.awt.Frame.MAXIMIZED_BOTH:
                    return true;
                default:
                    return false;
            }
        }
        
        protected override bool syncNativeQueue(long l)
        {
            throw new NotImplementedException();
        }

        public override void ungrab(java.awt.Window window)
        {
            NetWindowPeer peer = (NetWindowPeer)window.getPeer();
            if (peer != null)
            {
                peer.Ungrab(false);
            }
        }

		protected internal new static object targetToPeer(object target)
		{
			return SunToolkit.targetToPeer(target);
		}

        protected internal new static void targetDisposedPeer(object target, object peer)
        {
            SunToolkit.targetDisposedPeer(target, peer);
        }

        internal static void BeginInvoke(ThreadStart del)
        {
            if (bogusForm.InvokeRequired)
            {
                bogusForm.BeginInvoke(del);
            }
            else
            {
                del();
            }
        }

        internal static T Invoke<T>(Func<T> del)
        {
            if (bogusForm.InvokeRequired)
            {
                return (T)bogusForm.Invoke(del);
            }
            else
            {
                return del();
            }
        }

        internal static void Invoke(MethodInvoker del)
        {
            if (bogusForm.InvokeRequired)
            {
                bogusForm.Invoke(del);
            }
            else
            {
                del();
            }
        }
    }

    internal class NetDragSourceContextPeer : sun.awt.dnd.SunDragSourceContextPeer
    {
        private static readonly NetDragSourceContextPeer theInstance = new NetDragSourceContextPeer(null);
        private bool dragStart = false;

        private NetDragSourceContextPeer(java.awt.dnd.DragGestureEvent dge) : base(dge)
        {
        }

        public static NetDragSourceContextPeer createDragSourceContextPeer(java.awt.dnd.DragGestureEvent dge)
        {
            theInstance.setTrigger(dge);
            return theInstance;
        }

        public override void startSecondaryEventLoop()
        {
            //NetToolkit.startSecondaryEventLoop();
        }
        
        public override void quitSecondaryEventLoop()
        {
            //NetToolkit.quitSecondaryEventLoop();
        }

        internal static new java.awt.dnd.DragSourceContext getDragSourceContext()
        {
            return theInstance.getDragSourceContextCore();
        }

        internal static NetDragSourceContextPeer getInstance()
        {
            return theInstance;
        }

        internal java.awt.dnd.DragSourceContext getDragSourceContextCore()
        {
            return base.getDragSourceContext();
        }

        internal new void dragDropFinished(bool success, int operations, int x, int y)
        {
            if (dragStart)
                base.dragDropFinished(success, operations, x, y);
            dragStart = false;
        }

        protected override void startDrag(java.awt.datatransfer.Transferable trans, long[] formats, Map formatMap)
        {
            dragStart = true;
            long nativeCtxtLocal = 0;

            nativeCtxtLocal = createDragSource(getTrigger().getComponent(),
                                               trans,
                                               getTrigger().getTriggerEvent(),
                                               getTrigger().getSourceAsDragGestureRecognizer().getSourceActions(),
                                               formats,
                                               formatMap);
            NetDropTargetContextPeer.setCurrentJVMLocalSourceTransferable(trans);
        }

        private long createDragSource(java.awt.Component component,
                                 java.awt.datatransfer.Transferable transferable,
                                 java.awt.@event.InputEvent nativeTrigger,
                                 int actions,
                                 long[] formats,
                                 Map formatMap)
        {
            java.awt.Component controlOwner = component;
            while (controlOwner!=null && (controlOwner.getPeer() == null || controlOwner.getPeer() is sun.awt.NullComponentPeer))
            {
                controlOwner = controlOwner.getParent();
            }
            if (controlOwner != null)
            {
                NetComponentPeer peer = controlOwner.getPeer() as NetComponentPeer;
                if (peer != null)
                {
                    Control control = peer.control;
                    if (control != null)
                    {
                        IDataObject data = NetDataTransferer.getInstanceImpl().getDataObject(transferable, 
                            NetDataTransferer.adaptFlavorMap(getTrigger().getDragSource().getFlavorMap()));
                        NetToolkit.BeginInvoke(delegate
                                                   {
                                                       control.DoDragDrop(data, DragDropEffects.All);
                                                       try
                                                       {
                                                           setDragDropInProgress(false);
                                                       } catch
                                                       {
                                                       }
                                                   });
                    }
                }
            }
            return 0;
        }

        protected override void setNativeCursor(long nativeCtxt, java.awt.Cursor c, int cType)
        {
            
        }
    }

    internal class NetDropTargetContextPeer : sun.awt.dnd.SunDropTargetContextPeer
    {
        internal static NetDropTargetContextPeer getNetDropTargetContextPeer()
        {
            return new NetDropTargetContextPeer();
        }

        internal int handleEnterMessage(java.awt.Component component,
                                      int x, int y,
                                      int dropAction,
                                      int actions,
                                      long[] formats,
                                      long nativeCtxt)
        {
            return postDropTargetEvent(component, x, y, dropAction, actions,
                                       formats, nativeCtxt,
                                       SunDropTargetEvent.MOUSE_ENTERED,
                                       SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal void handleExitMessage(java.awt.Component component,
                                   long nativeCtxt)
        {
            postDropTargetEvent(component, 0, 0, java.awt.dnd.DnDConstants.ACTION_NONE,
                                java.awt.dnd.DnDConstants.ACTION_NONE, null, nativeCtxt,
                                SunDropTargetEvent.MOUSE_EXITED,
                                SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal int handleMotionMessage(java.awt.Component component,
                                    int x, int y,
                                    int dropAction,
                                    int actions, long[] formats,
                                    long nativeCtxt)
        {
            return postDropTargetEvent(component, x, y, dropAction, actions,
                                       formats, nativeCtxt,
                                       SunDropTargetEvent.MOUSE_DRAGGED,
                                       SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal void handleDropMessage(java.awt.Component component,
                               int x, int y,
                               int dropAction, int actions,
                                long[] formats,
                                long nativeCtxt)
        {
            postDropTargetEvent(component, x, y, dropAction, actions,
                                formats, nativeCtxt,
                                SunDropTargetEvent.MOUSE_DROPPED,
                                !SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal new int postDropTargetEvent(java.awt.Component component,
                                      int x, int y,
                                      int dropAction,
                                      int actions,
                                      long[] formats,
                                      long nativeCtxt,
                                      int eventID,
                                      bool dispatchType)
        {
            return base.postDropTargetEvent(component, x, y,
                                     dropAction, actions, formats, nativeCtxt, eventID, dispatchType);
        }

        protected override void doDropDone(bool success, int dropAction, bool isLocal)
        {
            // TODO: do something here... 
            throw new NotImplementedException();
        }

        protected override object getNativeData(long l)
        {
            throw new NotImplementedException();
        }
    }

    internal class NetMouseDragGestureRecognizer : java.awt.dnd.MouseDragGestureRecognizer
    {
        protected static int motionThreshold;

        protected static readonly int ButtonMask = java.awt.@event.InputEvent.BUTTON1_DOWN_MASK |
                                                   java.awt.@event.InputEvent.BUTTON2_DOWN_MASK |
                                                   java.awt.@event.InputEvent.BUTTON3_DOWN_MASK;

        public NetMouseDragGestureRecognizer(java.awt.dnd.DragSource source, java.awt.Component component1, int actions,
                                             java.awt.dnd.DragGestureListener listener) :
                                                 base(source, component1, actions, listener)
        {
        }

        protected int mapDragOperationFromModifiers(java.awt.@event.MouseEvent e)
        {
            int mods = e.getModifiersEx();
            int btns = mods & ButtonMask;

            // Prohibit multi-button drags.
            if (!(btns == java.awt.@event.InputEvent.BUTTON1_DOWN_MASK ||
                  btns == java.awt.@event.InputEvent.BUTTON2_DOWN_MASK ||
                  btns == java.awt.@event.InputEvent.BUTTON3_DOWN_MASK))
            {
                return java.awt.dnd.DnDConstants.ACTION_NONE;
            }

            return
                sun.awt.dnd.SunDragSourceContextPeer.convertModifiersToDropAction(mods,
                                                                      getSourceActions());
        }

        public override void mouseClicked(java.awt.@event.MouseEvent e)
        {
            // do nothing
        }

        public override void mousePressed(java.awt.@event.MouseEvent e)
        {
            events.clear();

            if (mapDragOperationFromModifiers(e) != java.awt.dnd.DnDConstants.ACTION_NONE)
            {
                try
                {
                    motionThreshold = java.awt.dnd.DragSource.getDragThreshold();
                }
                catch 
                {
                    motionThreshold = 5;
                }
                appendEvent(e);
            }
        }

        public override void mouseReleased(java.awt.@event.MouseEvent e)
        {
            events.clear();
        }

        public override void mouseEntered(java.awt.@event.MouseEvent e)
        {
            events.clear();
        }

        public override void mouseExited(java.awt.@event.MouseEvent e)
        {

            if (!events.isEmpty())
            { // gesture pending
                int dragAction = mapDragOperationFromModifiers(e);

                if (dragAction == java.awt.dnd.DnDConstants.ACTION_NONE)
                {
                    events.clear();
                }
            }
        }

        public override void mouseDragged(java.awt.@event.MouseEvent e)
        {
            if (!events.isEmpty())
            { // gesture pending
                int dop = mapDragOperationFromModifiers(e);

                if (dop == java.awt.dnd.DnDConstants.ACTION_NONE)
                {
                    return;
                }

                java.awt.@event.MouseEvent trigger = (java.awt.@event.MouseEvent)events.get(0);


                java.awt.Point origin = trigger.getPoint();
                java.awt.Point current = e.getPoint();

                int dx = java.lang.Math.abs(origin.x - current.x);
                int dy = java.lang.Math.abs(origin.y - current.y);

                if (dx > motionThreshold || dy > motionThreshold)
                {
                    fireDragGestureRecognized(dop, ((java.awt.@event.MouseEvent)getTriggerEvent()).getPoint());
                }
                else
                    appendEvent(e);
            }
        }

    }

    class NetInputMethodDescriptor : java.awt.im.spi.InputMethodDescriptor
    {
        public java.awt.im.spi.InputMethod createInputMethod()
        {
            throw new NotImplementedException();
        }

        public Locale[] getAvailableLocales()
        {
            // TODO Feature with .NET 3.0 available
            //IEnumerable languages = System.Windows.Input.InputLanguageManager.AvailableInputLanguages;
            // as a hack we return the default locale
            return new Locale[]{Locale.getDefault()};
        }

        public string getInputMethodDisplayName(Locale inputLocale, Locale displayLanguage)
        {
            // copied from WInputMethodDescriptor

            // We ignore the input locale.
            // When displaying for the default locale, rely on the localized AWT properties;
            // for any other locale, fall back to English.
            String name = "System Input Methods";
            if (Locale.getDefault().equals(displayLanguage))
            {
                name = java.awt.Toolkit.getProperty("AWT.HostInputMethodDisplayName", name);
            }
            return name;
        }

        public java.awt.Image getInputMethodIcon(Locale l)
        {
            //WInputMethodDescriptor return also ever null
            return null;
        }

        public bool hasDynamicLocaleList()
        {
            // Java return also true
            return true;
        }
    }

	class NetCustomCursor : java.awt.Cursor
	{
		private Cursor cursor;
		public Cursor Cursor
		{
			get { return cursor; }
		}

		internal NetCustomCursor(java.awt.Image cursorIm, java.awt.Point hotSpot, String name) // throws IndexOutOfBoundsException
			: base(name)
		{
			java.awt.Toolkit toolkit = java.awt.Toolkit.getDefaultToolkit();

			// Make sure image is fully loaded.
			java.awt.Component c = new java.awt.Canvas(); // for its imageUpdate method
			java.awt.MediaTracker tracker = new java.awt.MediaTracker(c);
			tracker.addImage(cursorIm, 0);
			try
			{
				tracker.waitForAll();
			}
			catch (java.lang.InterruptedException)
			{
			}
			int width = cursorIm.getWidth(c);
			int height = cursorIm.getHeight(c);

			// Fix for bug 4212593 The Toolkit.createCustomCursor does not
			//                     check absence of the image of cursor
			// If the image is invalid, the cursor will be hidden (made completely
			// transparent). In this case, getBestCursorSize() will adjust negative w and h,
			// but we need to set the hotspot inside the image here.
			if (tracker.isErrorAny() || width < 0 || height < 0)
			{
				hotSpot.x = hotSpot.y = 0;
			}

			// Scale image to nearest supported size.
			java.awt.Dimension nativeSize = toolkit.getBestCursorSize(width, height);
			if (nativeSize.width != width || nativeSize.height != height)
			{
				cursorIm = cursorIm.getScaledInstance(nativeSize.width,
												  nativeSize.height,
												  java.awt.Image.SCALE_DEFAULT);
				width = nativeSize.width;
				height = nativeSize.height;
			}

			// Verify that the hotspot is within cursor bounds.
			if (hotSpot.x >= width || hotSpot.y >= height || hotSpot.x < 0 || hotSpot.y < 0)
			{
				throw new ArgumentException("invalid hotSpot");
			}

			NetProducerImage npi = new NetProducerImage(cursorIm.getSource());
			cursorIm.getSource().startProduction(npi);
			Bitmap bitmap = npi.getBitmap();
			IntPtr hIcon = bitmap.GetHicon();
			cursor = new Cursor(hIcon);
		}
	}

	class NetLightweightComponentPeer : NetComponentPeer, java.awt.peer.LightweightPeer
	{
		public NetLightweightComponentPeer(java.awt.Component target)
			: base(target)
		{
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetLightweightContainerPeer : NetContainerPeer, java.awt.peer.LightweightPeer
    {
        public NetLightweightContainerPeer(java.awt.Container target)
            : base(target)
        {
        }

        private static ContainerControl GetContainerControl(java.awt.Container aContainer)
        {
            ContainerControl control = null;

            if (aContainer != null)
            {
                control = (ContainerControl)((NetContainerPeer)aContainer.getPeer()).control;
            }

            return control;
        }
    }

	sealed class AwtToolkit
	{
		internal delegate void CreateComponentDelegate(NetComponentPeer parent);

		internal static void CreateComponent(CreateComponentDelegate factory, NetComponentPeer parent)
		{
			NetToolkit.bogusForm.Invoke((ThreadStart)delegate
			{
				try
				{
					factory(parent);
				}
				catch (Exception x)
				{
					Console.WriteLine(x);
				}
			});
		}
	}

    abstract class NetComponentPeer : java.awt.peer.ComponentPeer
	{
		private static readonly java.awt.Font defaultFont = new java.awt.Font(java.awt.Font.DIALOG, java.awt.Font.PLAIN, 12);
		internal readonly java.awt.Component target;
		internal Control control;
        private bool isMouseClick;
        private bool isDoubleClick;
        private bool isPopupMenu;
		private int oldWidth = -1;
		private int oldHeight = -1;
		private bool sm_suppressFocusAndActivation;
		private bool m_callbacksEnabled;
		//private int m_validationNestCount;
		private int serialNum = 0;
		private bool isLayouting = false;
		private bool paintPending = false;
		private RepaintArea paintArea;
		private java.awt.Font font;
		private java.awt.Color foreground;
		private java.awt.Color background;
	    private volatile bool disposed;
        private NetDropTargetContextPeer dropTargetPeer;

		public NetComponentPeer(java.awt.Component target)
		{
			this.target = target;
			this.paintArea = new RepaintArea();
			java.awt.Container parent = SunToolkit.getNativeContainer(target);
			NetComponentPeer parentPeer = (NetComponentPeer)NetToolkit.targetToPeer(parent);
			create(parentPeer);
			// fix for 5088782: check if window object is created successfully
			//checkCreation();
			//this.winGraphicsConfig = (NetGraphicsConfiguration)getGraphicsConfiguration();
			/*
			this.surfaceData =
				winGraphicsConfig.createSurfaceData(this, numBackBuffers);
			 */
			initialize();
			start();  // Initialize enable/disable state, turn on callbacks
		}

		protected virtual void initialize()
		{
			if (target.isVisible())
			{
				show();  // the wnd starts hidden
			}
			java.awt.Color fg = target.getForeground();
			if (fg != null)
			{
				setForeground(fg);
			}
			// Set background color in C++, to avoid inheriting a parent's color.
			java.awt.Font f = target.getFont();
			if (f != null)
			{
				setFont(f);
			}
			if (!target.isEnabled())
			{
				disable();
			}
			java.awt.Rectangle r = target.getBounds();
            setBounds(r.x, r.y, r.width, r.height, java.awt.peer.ComponentPeer.__Fields.SET_BOUNDS);
		}

		void start()
		{
            NetToolkit.BeginInvoke(delegate
            {
                hookEvents();
                // JDK native code also disables the window here, but since that is already done in initialize(),
                // I don't see the point
                EnableCallbacks(true);
                control.Invalidate();
                control.Update();
            });
		}

		void EnableCallbacks(bool enabled)
		{
			m_callbacksEnabled = enabled;
		}

		internal abstract void create(NetComponentPeer parent);

	    void pShow()
		{
            NetToolkit.BeginInvoke(delegate { control.Visible = true; });
		}

		void Enable(bool enable)
		{
			sm_suppressFocusAndActivation = true;
			control.Enabled = enable;
			sm_suppressFocusAndActivation = false;
		}

		internal virtual void hookEvents()
		{
			// TODO we really only should hook these events when they are needed...
			control.KeyDown += new KeyEventHandler(OnKeyDown);
			control.KeyUp += new KeyEventHandler(OnKeyUp);
			control.KeyPress += new KeyPressEventHandler(OnKeyPress);
			control.MouseMove += new MouseEventHandler(OnMouseMove);
			control.MouseDown += new MouseEventHandler(OnMouseDown);
            control.MouseWheel += new MouseEventHandler(OnMouseWheel);
			control.Click += new EventHandler(OnClick);
			control.DoubleClick += new EventHandler(OnDoubleClick);
			control.MouseUp += new MouseEventHandler(OnMouseUp);
			control.MouseEnter += new EventHandler(OnMouseEnter);
			control.MouseLeave += new EventHandler(OnMouseLeave);
			control.GotFocus += new EventHandler(OnGotFocus);
			control.LostFocus += new EventHandler(OnLostFocus);
			//control.Leave += new EventHandler(OnBoundsChanged);
			control.Paint += new PaintEventHandler(OnPaint);
			control.ContextMenu = new ContextMenu();
			control.ContextMenu.Popup += new EventHandler(OnPopupMenu);
		    control.AllowDrop = true;
		    control.DragDrop += new DragEventHandler(OnDragDrop);
            control.DragOver += new DragEventHandler(OnDragOver);
            control.DragLeave += new EventHandler(OnDragLeave);
		    control.DragEnter += new DragEventHandler(OnDragEnter);
            control.QueryContinueDrag += new QueryContinueDragEventHandler(OnQueryContinueDrag);
		}

        internal virtual void unhookEvents()
        {
            control.KeyDown -= new KeyEventHandler(OnKeyDown);
            control.KeyUp -= new KeyEventHandler(OnKeyUp);
            control.KeyPress -= new KeyPressEventHandler(OnKeyPress);
            control.MouseMove -= new MouseEventHandler(OnMouseMove);
            control.MouseDown -= new MouseEventHandler(OnMouseDown);
            control.MouseWheel -= new MouseEventHandler(OnMouseWheel);
            control.Click -= new EventHandler(OnClick);
            control.DoubleClick -= new EventHandler(OnDoubleClick);
            control.MouseUp -= new MouseEventHandler(OnMouseUp);
            control.MouseEnter -= new EventHandler(OnMouseEnter);
            control.MouseLeave -= new EventHandler(OnMouseLeave);
            control.GotFocus -= new EventHandler(OnGotFocus);
            control.LostFocus -= new EventHandler(OnLostFocus);
            //control.Leave -= new EventHandler(OnBoundsChanged);
            control.Paint -= new PaintEventHandler(OnPaint);
            control.DragDrop -= new DragEventHandler(OnDragDrop);
            control.DragOver -= new DragEventHandler(OnDragOver);
            control.DragLeave -= new EventHandler(OnDragLeave);
            control.DragEnter -= new DragEventHandler(OnDragEnter);
            if (control.ContextMenu != null)
                control.ContextMenu.Popup -= new EventHandler(OnPopupMenu);
        }

		protected void SendEvent(java.awt.AWTEvent evt)
		{
			postEvent(evt);
		}

        /// <summary>
        /// Get the left insets of the .NET Window.
        /// In .NET the coordinate of a window start on the most left, top point with 0,0
        /// In Java the most left, top point with 0,0 is in the detail area of the window.
        /// In all not Windows Component this return ever 0.
        /// </summary>
        /// <returns></returns>
        protected virtual int getInsetsLeft()
        {
            return 0;
        }

        /// <summary>
        /// Get the top insets of the .NET Window.
        /// In .NET the coordinate of a window start on the most left, top point with 0,0
        /// In Java the most left, top point with 0,0 is in the detail area of the window.
        /// In all not Windows Component this return ever 0.
        /// </summary>
        /// <returns></returns>
        protected virtual int getInsetsTop()
        {
            return 0;
        }


        /// <summary>
        /// .NET calculate the offset relative to the detail area.
        /// Java is using the top left point of a window.
        /// That be must compensate the cordinate of a component
        /// if the parent is a window, frame or dialog.
        /// </summary>
        /// <returns>The offset of the details area in the parent</returns>
        private Point getParentOffset()
        {
            if (!(target is java.awt.Window))
            {
                java.awt.Container parent = target.getParent();
                if (parent != null)
                {
                    java.awt.peer.ComponentPeer peer = parent.getPeer();
                    if (peer is NetComponentPeer)
                    {
                        return new Point(
                            ((NetComponentPeer)peer).getInsetsLeft(),
                            ((NetComponentPeer)peer).getInsetsTop());
                    }
                }
            }
            return new Point();
        }

        private void OnPaint(object sender, PaintEventArgs e)
		{
			//CheckFontSmoothingSettings(GetHWnd());
			/* Set draw state */
			//SetDrawState(GetDrawState() | JAWT_LOCK_CLIP_CHANGED);
			WmPaint(e.Graphics, e.ClipRectangle);
		}

		private void WmPaint(Graphics g, Rectangle r)
		{
            handlePaint(r.X + getInsetsLeft(), r.Y + getInsetsTop(), r.Width, r.Height);
		}

		/* Invoke a paint() method call on the target, without clearing the
		 * damaged area.  This is normally called by a native control after
		 * it has painted itself.
		 *
		 * NOTE: This is called on the privileged toolkit thread. Do not
		 *       call directly into user code using this thread!
		 */
		private void handlePaint(int x, int y, int w, int h)
		{
			postPaintIfNecessary(x, y, w, h);
		}

		private void postPaintIfNecessary(int x, int y, int w, int h)
		{
			if (!ComponentAccessor.getIgnoreRepaint(target))
			{
				java.awt.@event.PaintEvent evt = PaintEventDispatcher.getPaintEventDispatcher().createPaintEvent(target, x, y, w, h);
				if (evt != null)
				{
					postEvent(evt);
				}
			}
		}

		private static int MapKeyCode(Keys key)
		{
			switch (key)
			{
				case Keys.Delete:
					return java.awt.@event.KeyEvent.VK_DELETE;

				case Keys.Enter:
					return java.awt.@event.KeyEvent.VK_ENTER;

				default:
					return (int)key;
			}
		}

        internal static int GetMouseEventModifiers(MouseEventArgs ev)
        {
            int modifiers = GetModifiers(Control.ModifierKeys);
            //Which button was pressed or released, because it can only one that it is a switch
            MouseButtons button = ev.Button;
            switch(button){
                case MouseButtons.Left:
                    modifiers |= java.awt.@event.InputEvent.BUTTON1_MASK;
                    break;
                case MouseButtons.Middle:
                    modifiers |= java.awt.@event.InputEvent.BUTTON2_MASK;
                    break;
                case MouseButtons.Right:
                    modifiers |= java.awt.@event.InputEvent.BUTTON3_MASK;
                    break;
            }
            return modifiers;
        }

        internal static int GetModifiers(Keys keys)
		{
			int modifiers = 0;
            if ((keys & Keys.Shift) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.SHIFT_DOWN_MASK;
			}
			if((keys & Keys.Control) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.CTRL_DOWN_MASK;
			}
			if((keys & Keys.Alt) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.ALT_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Left) != 0)
			{
                modifiers |= java.awt.@event.InputEvent.BUTTON1_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Middle) != 0)
			{
                modifiers |= java.awt.@event.InputEvent.BUTTON2_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Right) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.BUTTON3_DOWN_MASK;
			}
			return modifiers;
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(e.Modifiers);
			int keyCode = MapKeyCode(e.KeyCode);
			// TODO set keyChar
			char keyChar = ' ';
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_STANDARD;
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.KeyEvent(target, java.awt.@event.KeyEvent.KEY_PRESSED, when, modifiers, keyCode, keyChar, keyLocation));
			}));
		}

        private void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            IDataObject obj = e.Data;
            long[] formats = NetDataTransferer.getInstanceImpl().getClipboardFormatCodes(obj.GetFormats());
            dropTargetPeer = NetDropTargetContextPeer.getNetDropTargetContextPeer();
            int actions = dropTargetPeer.handleEnterMessage(target, e.X, e.Y, getDropAction(e.AllowedEffect, e.KeyState), getAction(e.AllowedEffect),
                                              formats, 0);
            e.Effect = getDragDropEffects(actions);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            IDataObject obj = e.Data;
            long[] formats = NetDataTransferer.getInstanceImpl().getClipboardFormatCodes(obj.GetFormats());
            dropTargetPeer = NetDropTargetContextPeer.getNetDropTargetContextPeer();
            int actions = dropTargetPeer.handleMotionMessage(target, e.X, e.Y, getDropAction(e.AllowedEffect, e.KeyState), getAction(e.AllowedEffect),
                                              formats, 0);
            e.Effect = getDragDropEffects(actions);
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            if (dropTargetPeer!=null)
                dropTargetPeer.handleExitMessage(target, 0);
            dropTargetPeer = null;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            IDataObject obj = e.Data;
            long[] formats = NetDataTransferer.getInstanceImpl().getClipboardFormatCodes(obj.GetFormats());
            int actions = getAction(e.Effect);
            NetDragSourceContextPeer.getInstance().dragDropFinished(true, actions, e.X, e.Y);
            if (dropTargetPeer != null)
                dropTargetPeer.handleDropMessage(target, e.X, e.Y, getAction(e.Effect), getAction(e.AllowedEffect),
                                                 formats, 0);
            dropTargetPeer = null;
        }

        private static DragDropEffects getDragDropEffects(int actions)
        {
            switch(actions)
            {
                case java.awt.dnd.DnDConstants.ACTION_COPY:
                    return DragDropEffects.Copy;
                case java.awt.dnd.DnDConstants.ACTION_MOVE:
                    return DragDropEffects.Move;
                case java.awt.dnd.DnDConstants.ACTION_COPY_OR_MOVE:
                    return DragDropEffects.Move | DragDropEffects.Copy;
                case java.awt.dnd.DnDConstants.ACTION_LINK:
                    return DragDropEffects.Link;
                default:
                    return DragDropEffects.None;
            }
        }

        private static int getDropAction(DragDropEffects effects, int keyState)
        {
            int ret = java.awt.dnd.DnDConstants.ACTION_NONE;
            const int MK_CONTROL = 0x8;
            const int MK_SHIFT = 0x4;
//            const int WM_MOUSEWHEEL = 0x20A;
//            const int MK_LBUTTON = 0x1;
//            const int MK_MBUTTON = 0x10;
//            const int MK_RBUTTON = 0x2;
//            const int MK_XBUTTON1 = 0x20;
//            const int MK_XBUTTON2 = 0x40;
            switch (keyState & (MK_CONTROL | MK_SHIFT))
            {
                case MK_CONTROL:
                    if ((effects & DragDropEffects.Copy) == DragDropEffects.Copy)
                        ret = java.awt.dnd.DnDConstants.ACTION_COPY;
                    else
                        ret = java.awt.dnd.DnDConstants.ACTION_NONE;
                    break;

                case MK_CONTROL | MK_SHIFT:
                    if ((effects & DragDropEffects.Link) == DragDropEffects.Link)
                        ret = java.awt.dnd.DnDConstants.ACTION_LINK;
                    else
                        ret = java.awt.dnd.DnDConstants.ACTION_NONE;
                    break;

                case MK_SHIFT:
                    if ((effects & DragDropEffects.Move) == DragDropEffects.Move)
                        ret = java.awt.dnd.DnDConstants.ACTION_MOVE;
                    else
                        ret = java.awt.dnd.DnDConstants.ACTION_NONE;
                    break;

                default:
                    if ((effects & DragDropEffects.Move) == DragDropEffects.Move)
                    {
                        ret = java.awt.dnd.DnDConstants.ACTION_MOVE;
                    }
                    else if ((effects & DragDropEffects.Copy) == DragDropEffects.Copy)
                    {
                        ret = java.awt.dnd.DnDConstants.ACTION_COPY;
                    }
                    else if ((effects & DragDropEffects.Link) == DragDropEffects.Link)
                    {
                        ret = java.awt.dnd.DnDConstants.ACTION_LINK;
                    }
                    break;
            }

            return ret;
        }
        private static int getAction(DragDropEffects effects)
        {
            int actions = java.awt.dnd.DnDConstants.ACTION_NONE;
            switch (effects)
            {
                case DragDropEffects.None:
                    actions = java.awt.dnd.DnDConstants.ACTION_NONE;
                    break;
                case DragDropEffects.Copy:
                    actions = java.awt.dnd.DnDConstants.ACTION_COPY;
                    break;
                case DragDropEffects.Move:
                    actions = java.awt.dnd.DnDConstants.ACTION_MOVE;
                    break;
                case DragDropEffects.Move | DragDropEffects.Copy:
                    actions = java.awt.dnd.DnDConstants.ACTION_COPY_OR_MOVE;
                    break;
                case DragDropEffects.Link:
                    actions = java.awt.dnd.DnDConstants.ACTION_LINK;
                    break;
            }
            return actions;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(e.Modifiers);
			int keyCode = MapKeyCode(e.KeyCode);
			// TODO set keyChar
			char keyChar = ' ';
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_STANDARD;
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.KeyEvent(target, java.awt.@event.KeyEvent.KEY_RELEASED, when, modifiers, keyCode, keyChar, keyLocation));
			}));
		}

		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			int keyCode = java.awt.@event.KeyEvent.VK_UNDEFINED;
			char keyChar = e.KeyChar;
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_UNKNOWN;
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.KeyEvent(target, java.awt.@event.KeyEvent.KEY_TYPED, when, modifiers, keyCode, keyChar, keyLocation));
			}));
		}

		private void postMouseEvent(MouseEventArgs ev, int id, int clicks)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = GetMouseEventModifiers(ev);
            int button = GetButton(ev);
			int clickCount = clicks;
            int x = ev.X + getInsetsLeft(); //The Inset correctur is needed for Window and extended classes
            int y = ev.Y + getInsetsTop();
            bool isPopup = isPopupMenu;
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.MouseEvent(target, id, when, modifiers, x, y, clickCount, isPopup, button));
			}));
            isPopupMenu = false;
        }

        private void postMouseEvent(EventArgs ev, int id)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = GetModifiers(Control.ModifierKeys);
            int button = 0;
            int clickCount = 0;
            int x = Control.MousePosition.X - control.Location.X;
            int y = Control.MousePosition.Y - control.Location.Y;
            bool isPopup = isPopupMenu;
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
	            postEvent(new java.awt.@event.MouseEvent(target, id, when, modifiers, x, y, clickCount, isPopup, button));
			}));
            isPopupMenu = false;
        }

        private void postMouseWheelEvent(EventArgs ev, int id, int delta)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = GetModifiers(Control.ModifierKeys);
            int scrollAmount = -delta * SystemInformation.MouseWheelScrollLines / 120;
            int clickCount = 0;
            int x = Control.MousePosition.X - control.Location.X;
            int y = Control.MousePosition.Y - control.Location.Y;
            bool isPopup = isPopupMenu;
            java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate
            {
                postEvent(new java.awt.@event.MouseWheelEvent(target, id, when, modifiers, x, y, clickCount, isPopup, java.awt.@event.MouseWheelEvent.WHEEL_UNIT_SCROLL, scrollAmount, scrollAmount));
            }));
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs ev)
		{
			if((ev.Button & (MouseButtons.Left | MouseButtons.Right)) != 0)
			{
				postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_DRAGGED, ev.Clicks);
			}
			else
			{
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_MOVED, ev.Clicks);
			}
		}

		internal static int GetButton(MouseEventArgs e)
		{
            if((e.Button & MouseButtons.Left) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON1;
			}
			else if((e.Button & MouseButtons.Middle) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON2;
			}
			else if((e.Button & MouseButtons.Right) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON3;
			}
			else
			{
				return java.awt.@event.MouseEvent.NOBUTTON;
			}
		}

		protected virtual void OnMouseDown(object sender, MouseEventArgs ev)
		{
			isMouseClick = false;
			isDoubleClick = false;
			isPopupMenu = false;
			postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_PRESSED, ev.Clicks);
		}

        private void OnMouseWheel(object sender, MouseEventArgs ev)
        {
            postMouseWheelEvent(ev, java.awt.@event.MouseEvent.MOUSE_WHEEL, ev.Delta);
        }

        private void OnClick(object sender, EventArgs ev)
        {
            isMouseClick = true;
        }

		private void OnDoubleClick(object sender, EventArgs ev)
        {
            isDoubleClick = true;
        }

		private void OnMouseUp(object sender, MouseEventArgs ev)
		{
			postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_RELEASED, ev.Clicks);
			if (isMouseClick)
			{
				//We make our own mouse click event because the event order is different in .NET
				//in .NET the click occured before MouseUp
				int clicks = ev.Clicks;
				if (isDoubleClick)
				{
					clicks = 2;
				}
				postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_CLICKED, clicks);
			}
			isMouseClick = false;
		}

		private void OnMouseEnter(object sender, EventArgs ev)
		{
			postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_ENTERED);
		}

		private void OnMouseLeave(object sender, EventArgs ev)
		{
			postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_EXITED);
		}

		private void OnGotFocus(object sender, EventArgs e)
		{
			if (sm_suppressFocusAndActivation)
			{
				return;
			}
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.FocusEvent(target, java.awt.@event.FocusEvent.FOCUS_GAINED));
			}));
		}

		private void OnLostFocus(object sender, EventArgs e)
		{
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
				postEvent(new java.awt.@event.FocusEvent(target, java.awt.@event.FocusEvent.FOCUS_LOST));
			}));
		}

		/*
		 * Called from native code (on Toolkit thread) in order to
		 * dynamically layout the Container during resizing
		 */
		internal void dynamicallyLayoutContainer() {
			// If we got the WM_SIZING, this must be a Container, right?
			// In fact, it must be the top-level Container.
			//if (log.isLoggable(Level.FINE)) {
			//    java.awt.Container parent = NetToolkit.getNativeContainer((java.awt.Component)target);
			//    if (parent != null) {
			//        log.log(Level.FINE, "Assertion (parent == null) failed");
			//    }
			//}
			java.awt.Container cont = (java.awt.Container)target;

			NetToolkit.executeOnEventHandlerThread(cont, Delegates.toRunnable(delegate {
				// Discarding old paint events doesn't seem to be necessary.
				cont.invalidate();
				cont.validate();

				//if (surfaceData instanceof OGLSurfaceData) {
				//    // 6290245: When OGL is enabled, it is necessary to
				//    // replace the SurfaceData for each dynamic layout
				//    // request so that the OGL viewport stays in sync
				//    // with the window bounds.
				//    try {
				//        replaceSurfaceData();
				//    } catch (InvalidPipeException e) {
				//        // REMIND: this is unlikely to occur for OGL, but
				//        // what do we do if surface creation fails?
				//    }
				//}

				// Forcing a paint here doesn't seem to be necessary.
				// paintDamagedAreaImmediately();
			}));
		}

		/*
		 * Paints any portion of the component that needs updating
		 * before the call returns (similar to the Win32 API UpdateWindow)
		 */
		internal void paintDamagedAreaImmediately()
		{
			// force Windows to send any pending WM_PAINT events so
			// the damage area is updated on the Java side
			updateWindow();
			// make sure paint events are transferred to main event queue
			// for coalescing
			NetToolkit.flushPendingEvents();
			// paint the damaged area
			paintArea.paint(target, shouldClearRectBeforePaint());
		}

        private void updateWindow()
        {
            NetToolkit.BeginInvoke(delegate
            {
                control.Update();
            });
        }

		/* override and return false on components that DO NOT require
		   a clearRect() before painting (i.e. native components) */
		public virtual bool shouldClearRectBeforePaint()
		{
			return true;
		}

        private void OnPopupMenu(object sender, EventArgs ev)
        {
            isPopupMenu = true;
        }

		/*
		 * Post an event. Queue it for execution by the callback thread.
		 */
		internal void postEvent(java.awt.AWTEvent evt)
		{
			NetToolkit.postEvent(NetToolkit.targetToAppContext(target), evt);
		}

		// Routines to support deferred window positioning.
		public void beginLayout()
		{
			// Skip all painting till endLayout
			isLayouting = true;
		}

		public void endLayout()
		{
			if (!paintArea.isEmpty() && !paintPending &&
				!target.getIgnoreRepaint())
			{
				// if not waiting for native painting repaint damaged area
				postEvent(new java.awt.@event.PaintEvent(target, java.awt.@event.PaintEvent.PAINT, new java.awt.Rectangle()));
			}
			isLayouting = false;
		}

		public void beginValidate()
		{
			//    Invoke(delegate
			//    {
			//        if (m_validationNestCount == 0)
			//        {
			//            m_hdwp = BeginDeferWindowPos();
			//        }
			//        m_validationNestCount++;
			//    });
		}

		public void endValidate()
		{
            //    Invoke(delegate
            //    {
			//    m_validationNestCount--;
			//    if (m_validationNestCount == 0) {
			//        // if this call to EndValidate is not nested inside another
			//        // Begin/EndValidate pair, end deferred window positioning
			//        ::EndDeferWindowPos(m_hdwp);
			//        m_hdwp = NULL;
			//    }
            //    });
        }

		// Returns true if we are inside begin/endLayout and
		// are waiting for native painting
		public bool isPaintPending()
		{
			return paintPending && isLayouting;
		}

		public int checkImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			return getToolkit().checkImage(img, width, height, ob);
		}

		public java.awt.Image createImage(java.awt.image.ImageProducer prod)
		{
            NetProducerImage npi = new NetProducerImage(prod);
            prod.startProduction(npi);
            return new java.awt.image.BufferedImage(npi.getBitmap());
		}

		public java.awt.Image createImage(int width, int height)
		{
            return new java.awt.image.BufferedImage(width, height, java.awt.image.BufferedImage.TYPE_INT_ARGB);
		}

		public void disable()
		{
            NetToolkit.BeginInvoke(delegate { Enable( false ); });
		}

		public void dispose()
		{
		    bool callDisposed = true;
            lock(this)
            {
                if (disposed)
                    callDisposed = false;
                disposed = true;
            }
            if (callDisposed)
            {
                disposeImpl();
            }
		}

		protected virtual void disposeImpl()
		{
            NetToolkit.targetDisposedPeer(target, this);
            NetToolkit.Invoke(delegate { nativeDispose(); });
        }

        protected void nativeDispose()
        {
            unhookEvents();
            control.Dispose();
        }


		public void enable()
		{
            NetToolkit.BeginInvoke(delegate { Enable(true); });
		}

        public java.awt.image.ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}

		public java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			return new NetFontMetrics(f);
		}

		public virtual java.awt.Graphics getGraphics()
		{
			if (!control.IsDisposed)
			{
				/* Fix for bug 4746122. Color and Font shouldn't be null */
				java.awt.Color bgColor = background;
				if (bgColor == null)
				{
					bgColor = java.awt.SystemColor.window;
				}
				java.awt.Color fgColor = foreground;
				if (fgColor == null)
				{
					fgColor = java.awt.SystemColor.windowText;
				}
				java.awt.Font font = this.font;
				if (font == null)
				{
					font = defaultFont;
				}
				return new ComponentGraphics(this.control, fgColor, bgColor, font);
			}
			return null;
		}

        public java.awt.Point getLocationOnScreen()
        {
            return NetToolkit.Invoke<java.awt.Point>(delegate
            {
                Point p = new Point();
                NetToolkit.BeginInvoke(delegate { p = control.PointToScreen(p); });
                return new java.awt.Point(p.X, p.Y);
            });
        }

		public java.awt.Dimension getMinimumSize()
		{
			return target.getSize();
		}

		public java.awt.Dimension getPreferredSize()
		{
			return getMinimumSize();
		}

		public java.awt.Toolkit getToolkit()
		{
			return java.awt.Toolkit.getDefaultToolkit();
		}

		// returns true if the event has been handled and shouldn't be propagated
		// though handleEvent method chain - e.g. WTextFieldPeer returns true
		// on handling '\n' to prevent it from being passed to native code
		public virtual bool handleJavaKeyEvent(java.awt.@event.KeyEvent e) { return false; }

		private void nativeHandleEvent(java.awt.AWTEvent e)
		{
				// TODO arrghh!! code from void AwtComponent::_NativeHandleEvent(void *param) in awt_Component.cpp should be here
		}

		public void handleEvent(java.awt.AWTEvent e)
		{
			int id = e.getID();

			if (((java.awt.Component)target).isEnabled() && (e is java.awt.@event.KeyEvent) && !((java.awt.@event.KeyEvent)e).isConsumed())
			{
				if (handleJavaKeyEvent((java.awt.@event.KeyEvent)e))
				{
					return;
				}
			}

			switch (id)
			{
				case java.awt.@event.PaintEvent.PAINT:
					// Got native painting
					paintPending = false;
					// Fallthrough to next statement
					goto case java.awt.@event.PaintEvent.UPDATE;
				case java.awt.@event.PaintEvent.UPDATE:
					// Skip all painting while layouting and all UPDATEs
					// while waiting for native paint
					if (!isLayouting && !paintPending)
					{
						paintArea.paint(target, shouldClearRectBeforePaint());
					}
					return;
				default:
					break;
			}

			// Call the native code
			nativeHandleEvent(e);
		}

        public void hide()
		{
            NetToolkit.BeginInvoke(delegate { control.Visible = false; });
		}

		public bool isFocusTraversable()
		{
			return true;
		}

		public virtual java.awt.Dimension minimumSize()
		{
			return getMinimumSize();
		}

		public virtual java.awt.Dimension preferredSize()
		{
			return getPreferredSize();
		}

		public void paint(java.awt.Graphics graphics)
		{
			target.paint(graphics);
		}

        public bool prepareImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			return getToolkit().prepareImage(img, width, height, ob);
		}

		public void print(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
		}

		public void repaint(long tm, int x, int y, int width, int height)
		{
		}

		public void requestFocus()
		{
            control.Invoke((MethodInvoker) delegate { control.Focus(); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">the component for which the focus is requested</param>
        /// <param name="temporary">indicates if the focus change is temporary (true) or permanent (false)</param>
        /// <param name="allowWindowFocus">indicates if it's allowed to change window focus</param>
        /// <param name="time">the timestamp</param>
        /// <returns></returns>
        public bool requestFocus(java.awt.Component request, bool temporary, bool allowWindowFocus, long time)
		{
            if (!getEnabled() || !getVisible())
            {
                return false;
            }
            postEvent(new java.awt.@event.FocusEvent(request, java.awt.@event.FocusEvent.FOCUS_GAINED, temporary, target));
			return true;
		}

        public void reshape(int x, int y, int width, int height)
        {
            NetToolkit.BeginInvoke(delegate
            {
                control.SetBounds(x, y, width, height);
                //If the .NET control does not accept the new bounds (minimum size, maximum size) 
                //then we need to reflect the real bounds on the .NET site to the Java site
                Rectangle bounds = control.Bounds;
                if (bounds.X != x)
                {
                    ComponentAccessor.setX(target, bounds.X);
                }
                if (bounds.Y != y)
                {
                    ComponentAccessor.setY(target, bounds.Y);
                }
                if (bounds.Width != width)
                {
                    ComponentAccessor.setWidth(target, bounds.Width);
                }
                if (bounds.Height != height)
                {
                    ComponentAccessor.setHeight(target, bounds.Height);
                }
            });
        }

		public void setBackground(java.awt.Color color)
		{
			lock (this)
			{
				this.background = color;
				NetToolkit.BeginInvoke(delegate { control.BackColor = J2C.ConvertColor(color); });
			}
		}

		private void reshapeNoCheck(int x, int y, int width, int height)
		{
            NetToolkit.BeginInvoke(delegate { control.SetBounds(x, y, width, height); });
		}

		public void setBounds(int x, int y, int width, int height, int op)
		{
			// Should set paintPending before reahape to prevent
			// thread race between paint events
			// Native components do redraw after resize
			paintPending = (width != oldWidth) || (height != oldHeight);

            if ((op & java.awt.peer.ComponentPeer.__Fields.NO_EMBEDDED_CHECK) != 0)
			{
				reshapeNoCheck(x, y, width, height);
			}
			else
			{
				reshape(x, y, width, height);
			}
			if ((width != oldWidth) || (height != oldHeight))
			{
				// Only recreate surfaceData if this setBounds is called
				// for a resize; a simple move should not trigger a recreation
				try
				{
					//replaceSurfaceData();
				}
				catch (sun.java2d.InvalidPipeException)
				{
					// REMIND : what do we do if our surface creation failed?
				}
				oldWidth = width;
				oldHeight = height;
			}

			serialNum++;
		}

		private void setCursorImpl(java.awt.Cursor cursor)
		{
			if (cursor is NetCustomCursor)
			{
				NetCustomCursor ncc = (NetCustomCursor)cursor;
				control.Cursor = ncc.Cursor;
				return;
			}
			switch(cursor.getType())
			{
				case java.awt.Cursor.WAIT_CURSOR:
					control.Cursor = Cursors.WaitCursor;
					break;
				case java.awt.Cursor.DEFAULT_CURSOR:
					control.Cursor = Cursors.Default;
					break;
				case java.awt.Cursor.HAND_CURSOR:
					control.Cursor = Cursors.Hand;
					break;
				case java.awt.Cursor.CROSSHAIR_CURSOR:
					control.Cursor = Cursors.Cross;
					break;
				case java.awt.Cursor.W_RESIZE_CURSOR:
				case java.awt.Cursor.E_RESIZE_CURSOR:
					control.Cursor = Cursors.SizeWE;
					break;
				case java.awt.Cursor.MOVE_CURSOR:
					control.Cursor = Cursors.SizeAll;
					break;
				case java.awt.Cursor.N_RESIZE_CURSOR:
				case java.awt.Cursor.S_RESIZE_CURSOR:
					control.Cursor = Cursors.SizeNS;
					break;
				case java.awt.Cursor.NE_RESIZE_CURSOR:
				case java.awt.Cursor.SW_RESIZE_CURSOR:
					control.Cursor = Cursors.SizeNESW;
					break;
				case java.awt.Cursor.NW_RESIZE_CURSOR:
				case java.awt.Cursor.SE_RESIZE_CURSOR:
					control.Cursor = Cursors.SizeNWSE;
					break;
				case java.awt.Cursor.TEXT_CURSOR:
					control.Cursor = Cursors.IBeam;
					break;
				default:
					Console.WriteLine("setCursor not implement for: " + cursor);
					break;
			}
		}

        public void setCursor(java.awt.Cursor cursor)
        {
            control.Invoke(new SetCursor(setCursorImpl), new object[] { cursor });
        }

        public bool getEnabled()
        {
            return NetToolkit.Invoke((Func<bool>)delegate { return control.Enabled; });
        }

        public bool getFocused()
        {
            return NetToolkit.Invoke((Func<bool>)delegate { return control.Focused; });
        }

        public bool getVisible()
        {
            return NetToolkit.Invoke((Func<bool>)delegate { return control.Visible; });
        }

        public void setEnabled(bool enabled)
		{
			if (enabled)
			{
				enable();
			}
			else
			{
				disable();
			}
		}

		public void setFont(java.awt.Font font)
		{
			lock (this)
			{
				this.font = font;
				NetToolkit.BeginInvoke(delegate { control.Font = font.getNetFont(); });
			}
		}

		public void setForeground(java.awt.Color color)
		{
			lock (this)
			{
				this.foreground = color;
				NetToolkit.BeginInvoke(delegate { control.ForeColor = J2C.ConvertColor(color); });
			}
		}

		public void setVisible(bool visible)
		{
			if (visible)
			{
				show();
			}
			else
			{
				hide();
			}
		}

		public virtual void show()
		{
			java.awt.Dimension s = ((java.awt.Component)target).getSize();
			oldHeight = s.height;
			oldWidth = s.width;
			pShow();
		}

		/*
		 * Return the GraphicsConfiguration associated with this peer, either
		 * the locally stored winGraphicsConfig, or that of the target Component.
		 */
        public java.awt.GraphicsConfiguration getGraphicsConfiguration()
        {
            // we don't need a treelock here, since
            // Component.getGraphicsConfiguration() gets it itself.
            return target.getGraphicsConfiguration();
        }

		public void setEventMask (long mask)
		{
			//Console.WriteLine("NOTE: NetComponentPeer.setEventMask not implemented");
		}

		public bool isObscured()
		{
			// should never be called because we return false from canDetermineObscurity()
			return true;
		}

		public bool canDetermineObscurity()
		{
			// JDK returns true here and uses GetClipBox to determine if the window is partially obscured,
			// this is an optimization for scrolling in javax.swing.JViewport, since there appears to be
			// no managed equivalent of GetClipBox, we'll simply return false and forgo the optimization.
			return false;
		}

		public void coalescePaintEvent(java.awt.@event.PaintEvent e)
		{
			java.awt.Rectangle r = e.getUpdateRect();
			if (!(e is sun.awt.@event.IgnorePaintEvent))
			{
				paintArea.add(r, e.getID());
			}
		}

		public void updateCursorImmediately()
		{
		}

		public java.awt.image.VolatileImage createVolatileImage(int width, int height)
		{
			return new NetVolatileImage(target, width, height);
		}

		public bool handlesWheelScrolling()
		{
			return true;
		}

		public void createBuffers(int x, java.awt.BufferCapabilities capabilities)
		{
			throw new NotImplementedException();
		}

		public java.awt.Image getBackBuffer()
		{
			throw new NotImplementedException();
		}

		public void flip(java.awt.BufferCapabilities.FlipContents contents)
		{
			throw new NotImplementedException();
		}

		public void destroyBuffers()
		{
			throw new NotImplementedException();
		}

		public virtual bool isFocusable()
		{
			return false;
		}

	    protected bool isDisposed()
	    {
	        return disposed;
	    }
        
		public java.awt.Rectangle getBounds()
		{
			return target.getBounds();
		}

		public void reparent(java.awt.peer.ContainerPeer parent)
		{
			throw new NotImplementedException();
		}

		public bool isReparentSupported()
		{
			return false;
		}

		// Do nothing for heavyweight implementation
		public void layout()
		{
		}

        public void applyShape(sun.java2d.pipe.Region shape)
        {
			control.Region = J2C.ConvertRegion(shape);
        }

        //copied form KeyboardFocusManager
        private const int SNFH_FAILURE = 0;
        private const int SNFH_SUCCESS_HANDLED = 1;
        private const int SNFH_SUCCESS_PROCEED = 2;

        private static java.lang.reflect.Method shouldNativelyFocusHeavyweight;
		private static java.lang.reflect.Method processSynchronousLightweightTransfer;
		private static java.lang.reflect.Method removeLastFocusRequest;

        public bool requestFocus(java.awt.Component lightweightChild, bool temporary, bool focusedWindowChangeAllowed, long time, sun.awt.CausedFocusEvent.Cause cause)
        {
            // this is a interpretation of the code in WComponentPeer.java and awt_component.cpp
            try
            {
                if (processSynchronousLightweightTransfer == null)
                {
					java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
					{
						java.lang.Class keyboardFocusManagerCls = typeof(java.awt.KeyboardFocusManager);
						java.lang.reflect.Method method = keyboardFocusManagerCls.getDeclaredMethod(
							"processSynchronousLightweightTransfer",
							typeof(java.awt.Component),
							typeof(java.awt.Component),
							java.lang.Boolean.TYPE,
							java.lang.Boolean.TYPE,
							java.lang.Long.TYPE);
						method.setAccessible(true);
						processSynchronousLightweightTransfer = method;
						return null;
					}));
                }
                processSynchronousLightweightTransfer.invoke(
                null,
                target,
                lightweightChild,
                java.lang.Boolean.valueOf(temporary),
                java.lang.Boolean.valueOf(focusedWindowChangeAllowed),
                java.lang.Long.valueOf(time));
            }
            catch
            {
                return true;
            }
            if (shouldNativelyFocusHeavyweight == null)
            {
				java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
				{
					java.lang.Class keyboardFocusManagerCls = typeof(java.awt.KeyboardFocusManager);
					java.lang.reflect.Method method = keyboardFocusManagerCls.getDeclaredMethod(
						"shouldNativelyFocusHeavyweight",
						typeof(java.awt.Component),
						typeof(java.awt.Component),
						java.lang.Boolean.TYPE,
						java.lang.Boolean.TYPE,
						java.lang.Long.TYPE,
						typeof(sun.awt.CausedFocusEvent.Cause));
					method.setAccessible(true);
					shouldNativelyFocusHeavyweight = method;
					return null;
				}));
            }
            int retval = ((java.lang.Integer)shouldNativelyFocusHeavyweight.invoke(
                null,
                target,
                lightweightChild,
                java.lang.Boolean.valueOf(temporary),
                java.lang.Boolean.valueOf(focusedWindowChangeAllowed),
                java.lang.Long.valueOf(time),
                cause)).intValue();
            if (retval == SNFH_SUCCESS_HANDLED)
            {
                return true;
            }
            else if (retval == SNFH_SUCCESS_PROCEED)
            {
                if (getFocused())
                {
                    return true;
                }
                if (removeLastFocusRequest == null)
                {
					java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
					{
						java.lang.Class keyboardFocusManagerCls = typeof(java.awt.KeyboardFocusManager);
						java.lang.reflect.Method method = keyboardFocusManagerCls.getDeclaredMethod(
							"removeLastFocusRequest",
							typeof(java.awt.Component));
						method.setAccessible(true);
						removeLastFocusRequest = method;
						return null;
					}));
                }
                removeLastFocusRequest.invoke(null, target);
            }
            //SNFH_FAILURE
            return false;
        }

		internal static NetComponentPeer FromControl(Control control)
		{
			return (NetComponentPeer)control.Tag;
		}
	}

    class NetButtonPeer : NetComponentPeer, java.awt.peer.ButtonPeer
	{
		public NetButtonPeer(java.awt.Button awtbutton)
			: base(awtbutton)
		{
			if(!awtbutton.isBackgroundSet())
			{
				awtbutton.setBackground(java.awt.SystemColor.control);
			}
			control.BackColor = Color.FromArgb(awtbutton.getBackground().getRGB());
			setLabel(awtbutton.getLabel());
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			((Button)control).Click += new EventHandler(OnClick);
		}

		private void OnClick(object sender, EventArgs e)
		{
			// TODO set all these properties correctly
			string cmd = "";
			long when = 0;
			int modifiers = 0;
			postEvent(new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
		}

		private void setLabelImpl(string label)
		{
			control.Text = label;
		}

		public void setLabel(string label)
		{
			control.Invoke(new SetString(setLabelImpl), new object[] { label });
		}

		public override java.awt.Dimension minimumSize()
		{
			using(Graphics g = control.CreateGraphics())
			{
				// TODO get these fudge factors from somewhere
				return new java.awt.Dimension((int)Math.Round(12 + g.MeasureString(control.Text, control.Font).Width) * 8 / 7, 6 + control.Font.Height * 8 / 7);
			}
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetTextComponentPeer : NetComponentPeer, java.awt.peer.TextComponentPeer
	{
		public NetTextComponentPeer(java.awt.TextComponent textComponent)
			: base(textComponent)
		{
			if(!target.isBackgroundSet())
			{
				target.setBackground(java.awt.SystemColor.window);
			}
			setBackground(target.getBackground());
			((TextBox)control).AutoSize = false;
			((TextBox)control).Text = ((java.awt.TextComponent)target).getText();
		}

        public override bool isFocusable()
        {
            return true;
        }

		protected override void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			base.OnKeyPress(sender, e);
			// TODO for TextAreas this probably isn't the right behaviour
			if(e.KeyChar == '\r')
			{
				// TODO set all these properties correctly
				string cmd = "";
				long when = 0;
				int modifiers = 0;
				postEvent(new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
			}
		}

		public int getSelectionEnd()
		{
			throw new NotImplementedException();
		}
		public int getSelectionStart()
		{
			throw new NotImplementedException();
		}

		private string getTextImpl()
		{
			return control.Text;
		}

		public string getText()
		{
			return (string)control.Invoke(new GetString(getTextImpl));
		}

		private void setTextImpl(string text)
		{
			control.Text = text;
		}

		public void setText(string text)
		{
			control.Invoke(new SetString(setTextImpl), new object[] { text });
		}

		public void select(int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
		public void setEditable(bool editable)
		{
			throw new NotImplementedException();
		}
		public int getCaretPosition()
		{
			throw new NotImplementedException();
		}

		private void setCaretPositionImpl(int pos)
		{
			((TextBox)control).SelectionStart = pos;
			((TextBox)control).SelectionLength = 0;
		}

		public void setCaretPosition(int pos)
		{
			control.Invoke(new SetInt(setCaretPositionImpl), new object[] { pos });
		}

		public long filterEvents(long filter)
		{
			throw new NotImplementedException();
		}

		public int getIndexAtPoint(int x, int y)
		{
			throw new NotImplementedException();
		}

		public java.awt.Rectangle getCharacterBounds(int pos)
		{
			throw new NotImplementedException();
		}

		public java.awt.im.InputMethodRequests getInputMethodRequests()
		{
			throw new NotImplementedException();
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetChoicePeer : NetComponentPeer, java.awt.peer.ChoicePeer
	{
		public NetChoicePeer(java.awt.Choice target)
			: base(target)
		{
		}

		public void add(string str, int i)
		{
			// TODO:  Add NetChoicePeer.add implementation
		}

		public void addItem(string str, int i)
		{
			// TODO:  Add NetChoicePeer.addItem implementation
		}

		public void select(int i)
		{
			// TODO:  Add NetChoicePeer.select implementation
		}

		public void removeAll()
		{
			// TODO:  Add NetChoicePeer.removeAll implementation
		}

		public void remove(int i)
		{
			// TODO:  Add NetChoicePeer.remove implementation
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetCheckboxPeer : NetComponentPeer, java.awt.peer.CheckboxPeer
	{
		public NetCheckboxPeer(java.awt.Checkbox target)
			: base(target)
		{
		}

		public void setCheckboxGroup(java.awt.CheckboxGroup cg)
		{
			// TODO:  Add NetCheckboxPeer.setCheckboxGroup implementation
		}

		public void setState(bool b)
		{
			// TODO:  Add NetCheckboxPeer.setState implementation
		}

		public void setLabel(string str)
		{
			// TODO:  Add NetCheckboxPeer.setLabel implementation
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetLabelPeer : NetComponentPeer, java.awt.peer.LabelPeer
	{
		public NetLabelPeer(java.awt.Label jlabel)
			: base(jlabel)
		{
			((Label)control).Text = jlabel.getText();
			setAlignment(jlabel.getAlignment());
		}

		public void setAlignment(int align)
		{
			switch(align)
			{
				case java.awt.Label.LEFT:
					control.Invoke(new SetInt(setAlignImpl), new object[] { ContentAlignment.TopLeft });
					break;
				case java.awt.Label.CENTER:
					control.Invoke(new SetInt(setAlignImpl), new object[] { ContentAlignment.TopCenter });
					break;
				case java.awt.Label.RIGHT:
					control.Invoke(new SetInt(setAlignImpl), new object[] { ContentAlignment.TopRight });
					break;
			}
		}

		private void setAlignImpl(int align)
		{
			((Label)control).TextAlign = (ContentAlignment)align;
		}

		public void setText(string s)
		{
			control.Invoke(new SetString(setTextImpl), new Object[] { s });
		}

		private void setTextImpl(string s)
		{
			control.Text = s;
		}

		public override java.awt.Dimension preferredSize()
		{
			return (java.awt.Dimension)control.Invoke(new GetDimension(getPreferredSizeImpl), null);
		}

		private java.awt.Dimension getPreferredSizeImpl()
		{
			Label lab = (Label)control;
			// HACK get these fudge factors from somewhere
			return new java.awt.Dimension(lab.PreferredWidth, 2 + lab.PreferredHeight);
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetTextFieldPeer : NetTextComponentPeer, java.awt.peer.TextFieldPeer
	{
		public NetTextFieldPeer(java.awt.TextField textField)
			: base(textField)
		{
			setEchoCharacter(textField.getEchoChar());
		}

		public java.awt.Dimension minimumSize(int len)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension preferredSize(int len)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getMinimumSize(int len)
		{
			return getPreferredSize(len);
		}

		public java.awt.Dimension getPreferredSize(int len)
		{
			// TODO use control.Invoke
			using(Graphics g = control.CreateGraphics())
			{
				return new java.awt.Dimension((int)Math.Round((g.MeasureString("abcdefghijklm", control.Font).Width * len) / 13), ((TextBox)control).PreferredHeight);
			}
		}

		public void setEchoChar(char echo_char)
		{
			setEchoCharacter(echo_char);
		}

        private void setEchoCharacterImpl(char echo_char)
        {
            ((TextBox)control).PasswordChar = echo_char;
        }

		public void setEchoCharacter(char echo_char)
		{
		    control.Invoke(new Action<char>(setEchoCharacterImpl), echo_char);
		}

        public override bool handleJavaKeyEvent(java.awt.@event.KeyEvent e)
        {
            switch (e.getID())
            {
                case java.awt.@event.KeyEvent.KEY_TYPED:
                    if ((e.getKeyChar() == '\n') && !e.isAltDown() && !e.isControlDown())
                    {
                        postEvent(new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED,
                                                  getText(), e.getWhen(), e.getModifiers()));
                        return true;
                    }
                    break;
            }
            return false;
        }
	}

    class NetTextAreaPeer : NetTextComponentPeer, java.awt.peer.TextAreaPeer
	{
		public NetTextAreaPeer(java.awt.TextArea textArea)
			: base(textArea)
		{
			((TextBox)control).ReadOnly = !((java.awt.TextArea)target).isEditable();
			((TextBox)control).WordWrap = false;
			((TextBox)control).ScrollBars = ScrollBars.Both;
			((TextBox)control).Multiline = true;
		}

		private void insertImpl(string text, int pos)
		{
			control.Text = control.Text.Insert(pos, text);
		}

		public void insert(string text, int pos)
		{
			control.Invoke(new SetStringInt(insertImpl), new Object[] { text, pos });
		}

		public void insertText(string text, int pos)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension minimumSize(int rows, int cols)
		{
			return getMinimumSize(rows, cols);
		}
		public java.awt.Dimension getMinimumSize(int rows, int cols)
		{
			return new java.awt.Dimension(0, 0);
		}
		public java.awt.Dimension preferredSize(int rows, int cols)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getPreferredSize(int rows, int cols)
		{
			Console.WriteLine("NOTE: NetTextAreaPeer.getPreferredSize not implemented");
			return new java.awt.Dimension(10 * cols, 15 * rows);
		}

		public void replaceRange(string text, int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
		public void replaceText(string text, int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
	}

    class NetContainerPeer : NetComponentPeer, java.awt.peer.ContainerPeer
	{
        /// <summary>
        /// The native insets of the .NET Window
        /// </summary>
		protected java.awt.Insets _insets = new java.awt.Insets(0, 0, 0, 0);

		public NetContainerPeer(java.awt.Container awtcontainer)
			: base(awtcontainer)
		{
		}

        protected override int getInsetsLeft()
        {
            return _insets.left; ;
        }

        protected override int getInsetsTop()
        {
            return _insets.top;
        }

        public java.awt.Insets insets()
		{
			return getInsets();
		}

		public java.awt.Insets getInsets()
		{
			return _insets;
		}

		public bool isRestackSupported()
		{
			return false;
		}

		public void cancelPendingPaint(int x, int y, int width, int height)
		{
			throw new NotImplementedException();
		}

		public void restack()
		{
			throw new NotImplementedException();
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetPanelPeer : NetContainerPeer, java.awt.peer.PanelPeer
	{
		public NetPanelPeer(java.awt.Panel panel)
			: base(panel)
		{
		}
	}

    class NewCanvasPeer : NetComponentPeer, java.awt.peer.CanvasPeer
	{
		public NewCanvasPeer(java.awt.Canvas canvas)
			: base(canvas)
		{
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetWindowPeer : NetContainerPeer, java.awt.peer.WindowPeer
	{
        // we can't use NetDialogPeer as blocker may be an instance of NetPrintDialogPeer that
        // extends NetWindowPeer, not NetDialogPeer
        private NetWindowPeer modalBlocker;
        private bool modalSavedEnabled;

        private static NetWindowPeer grabbedWindow;

        public NetWindowPeer(java.awt.Window window)
			: base(window)
		{
            //form.Shown += new EventHandler(OnOpened); Will already post in java.awt.Window.show()
            ((Form)control).Closing += new CancelEventHandler(OnClosing);
			((Form)control).Closed += new EventHandler(OnClosed);
			((Form)control).Activated += new EventHandler(OnActivated);
			((Form)control).Deactivate += new EventHandler(OnDeactivate);
			control.SizeChanged += new EventHandler(OnSizeChanged);
			control.Resize += new EventHandler(OnResize);
            control.Move += new EventHandler(OnMove);
        }

        protected override void initialize()
        {
            base.initialize();
            updateIconImages();
        }

		private void OnResize(object sender, EventArgs e)
		{
            // WmSizing
			SendComponentEvent(java.awt.@event.ComponentEvent.COMPONENT_RESIZED);
			dynamicallyLayoutContainer();
		}

        private void OnMove(object sender, EventArgs e)
        {
            // WmMove
            ComponentAccessor.setX(target, control.Left);
            ComponentAccessor.setY(target, control.Top);
            SendComponentEvent(java.awt.@event.ComponentEvent.COMPONENT_MOVED);
        }

		/*
		 * Although this function sends ComponentEvents, it needs to be defined
		 * here because only top-level windows need to have move and resize
		 * events fired from native code.  All contained windows have these events
		 * fired from common Java code.
		 */
		private void SendComponentEvent(int eventId)
		{
			SendEvent(new java.awt.@event.ComponentEvent(target, eventId));
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			// WmSize
			typeof(java.awt.Component).GetField("width", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(target, control.Width);
			typeof(java.awt.Component).GetField("height", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(target, control.Height);
        }

        private void OnOpened(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)target, java.awt.@event.WindowEvent.WINDOW_OPENED));
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)target, java.awt.@event.WindowEvent.WINDOW_CLOSING));
        }

        private void OnClosed(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)target, java.awt.@event.WindowEvent.WINDOW_CLOSED));
        }

		private const int WA_ACTIVE = 1;
		private const int WA_INACTIVE = 2;

        private void OnActivated(object sender, EventArgs e)
        {
			WmActivate(WA_ACTIVE, ((Form)control).WindowState == FormWindowState.Minimized, null);
        }

		private void OnDeactivate(object sender, EventArgs e)
		{
			WmActivate(WA_INACTIVE, ((Form)control).WindowState == FormWindowState.Minimized, null);
		}

		private void WmActivate(int nState, bool fMinimized, Control opposite)
		{
			int type;

			if (nState != WA_INACTIVE)
			{
				type = java.awt.@event.WindowEvent.WINDOW_GAINED_FOCUS;
			}
			else
			{
                if (grabbedWindow != null && !grabbedWindow.IsOneOfOwnersOf(this))
                {
                    grabbedWindow.Ungrab(true);
                }
                type = java.awt.@event.WindowEvent.WINDOW_LOST_FOCUS;
			}

			SendWindowEvent(type, opposite);
		}

		private void SendWindowEvent(int id, Control opposite) { SendWindowEvent(id, opposite, 0, 0); }

		private void SendWindowEvent(int id, Control opposite, int oldState, int newState)
		{
			java.awt.AWTEvent evt = new java.awt.@event.WindowEvent((java.awt.Window)target, id, null);

			if (id == java.awt.@event.WindowEvent.WINDOW_GAINED_FOCUS
				|| id == java.awt.@event.WindowEvent.WINDOW_LOST_FOCUS)
			{
				Type type = typeof(java.awt.Component).Assembly.GetType("java.awt.SequencedEvent");
				ConstructorInfo cons = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(java.awt.AWTEvent) }, null);
				evt = (java.awt.AWTEvent)cons.Invoke(new object[] { evt });
			}

			SendEvent(evt);
		}

        public override java.awt.Graphics getGraphics()
        {
            java.awt.Graphics g = base.getGraphics();
            java.awt.Insets insets = getInsets();
            g.translate(-insets.left, -insets.top);
            g.setClip(insets.left, insets.top, control.ClientRectangle.Width, control.ClientRectangle.Height);
            return g;
        }

        /// <summary>
        /// Set the border style of the window and recalc the insets
        /// </summary>
        /// <param name="style">the new style</param>
        protected void setFormBorderStyle(FormBorderStyle style)
        {
            NetToolkit.BeginInvoke(delegate
            {
                ((Form)control).FormBorderStyle = style;
                //Calculate the Insets one time
                //This is many faster because there no thread change is needed.
                Rectangle client = control.ClientRectangle;
                if (client.Height == 0)
                {
                    // HACK for .NET bug if form has the minimum size then ClientRectangle is not recalulate
                    // if the FormBorderStyle is changed
                    Size size = control.Size;
                    size.Height++;
                    control.Size = size;
                    size.Height--;
                    control.Size = size;
                    client = control.ClientRectangle;
                }
                Rectangle r = control.RectangleToScreen(client);
                int x = r.Location.X - control.Location.X;
                int y = r.Location.Y - control.Location.Y;
                _insets = new java.awt.Insets(y, x, control.Height - client.Height - y, control.Width - client.Width - x);
            });
        }

        protected void SetBoundsImpl(int x, int y, int width, int height)
        {
            Form form = (Form)control;
            form.DesktopBounds = new Rectangle(x, y, width, height);
        }

        public void toBack()
		{
            control.BeginInvoke(new SetVoid(((Form)control).SendToBack));
		}

		public void toFront()
		{
            control.BeginInvoke(new SetVoid(((Form)control).Activate));
		}

		public void updateAlwaysOnTop()
		{
			throw new NotImplementedException();
		}

		public bool requestWindowFocus()
		{
			return control.Focus();
		}

        public void setAlwaysOnTop(bool b)
        {
            throw new NotImplementedException();
        }

        public bool isModalBlocked()
        {
            return modalBlocker != null;
        }

        public void setModalBlocked(java.awt.Dialog dialog, bool blocked)
        {
            lock (target.getTreeLock()) // State lock should always be after awtLock
            {
                // use NetWindowPeer instead of NetDialogPeer because of FileDialogs and PrintDialogs
                NetWindowPeer blockerPeer = (NetWindowPeer)dialog.getPeer();
                if (blocked)
                {
                    modalBlocker = blockerPeer;
                    modalSavedEnabled = control.Enabled;
                    disable();
                }
                else
                {
                    modalBlocker = null;
                    if(modalSavedEnabled){
                        enable();
                    }
                    else
                    {
                        disable();
                    }
                }
            }
        }

        public void updateFocusableWindowState()
        {
            ((UndecoratedForm)control).setFocusableWindow( ((java.awt.Window)target).isFocusableWindow());
        }

        public void updateIconImages()
        {
            java.util.List imageList = ((java.awt.Window) target).getIconImages();
            Bitmap originalImage;
            if (imageList == null || imageList.size() == 0)
            {
                originalImage = null;
            }
            else
            {
                java.awt.Image image = (java.awt.Image)imageList.get(0);
                originalImage = ((java.awt.image.BufferedImage)image).getBitmap();
            }
            NetToolkit.BeginInvoke(delegate
               {
                   Size iconSize = SystemInformation.IconSize;
                   using (Bitmap scaleBitmap = originalImage == null ? null : new Bitmap(originalImage, iconSize))
                   {
                       ((Form) control).Icon = scaleBitmap == null ? null : Icon.FromHandle(scaleBitmap.GetHicon());
                   }
               });

        }

        public void updateMinimumSize()
        {
            throw new NotImplementedException();
        }

		internal override void create(NetComponentPeer parent)
		{
			AwtToolkit.CreateComponent(Create, parent);
		}

		void Create(NetComponentPeer parent)
		{
			Form form = new UndecoratedForm();
			form.Tag = this;
			if (parent != null)
			{
				form.Owner = parent.control.FindForm();
			}
			NetToolkit.CreateNative(form);
			this.control = form;
		}

        protected override void OnMouseDown(object sender, MouseEventArgs ev)
        {
            if (grabbedWindow != null && !grabbedWindow.IsOneOfOwnersOf(this))
            {
                grabbedWindow.Ungrab(true);
            }
            base.OnMouseDown(sender, ev);
        }

        internal void Grab()
        {
            //copy from file awt_Windows.cpp
            if (grabbedWindow != null)
            {
                grabbedWindow.Ungrab(true);
            }
            grabbedWindow = this;
            if (Form.ActiveForm == null)
            {
                Ungrab(true);
            }
            else if (control != Form.ActiveForm)
            {
                toFront();
            }
        }

        internal void Ungrab(bool doPost)
        {
            //copy from file awt_Windows.cpp
            if (grabbedWindow == this)
            {
                if (doPost)
                {
                    SendEvent(new sun.awt.UngrabEvent(this.target));
                }
                grabbedWindow = null;
            }
        }

        private bool IsOneOfOwnersOf(NetWindowPeer window)
        {
            while (window != null)
            {
                if (window == this)
                {
                    return true;
                }
                java.awt.Container parent = window.target.getParent();
                window = parent == null ? null : (NetWindowPeer)parent.getPeer();
            }
            return false;
        }
	}

    class NetFramePeer : NetWindowPeer, java.awt.peer.FramePeer
	{
		public NetFramePeer(java.awt.Frame frame)
			: base(frame)
		{
        }

		private class ValidateHelper : java.lang.Runnable
		{
			private java.awt.Component comp;

			internal ValidateHelper(java.awt.Component comp)
			{
				this.comp = comp;
			}

			public void run()
			{
				comp.validate();
			}
		}

        protected override void initialize()
        {
            base.initialize();
            java.awt.Frame target = (java.awt.Frame)this.target;

            if (target.getTitle() != null)
            {
                setTitle(target.getTitle());
            }
            setResizable(target.isResizable());
            setState(target.getExtendedState());
        }

		public void setMenuBar(java.awt.MenuBar mb)
		{
			throw new NotImplementedException();
		}

        public void setResizable(bool resizable)
        {
            if (resizable)
            {
                setFormBorderStyle(FormBorderStyle.Sizable);
            }
            else
            {
                setFormBorderStyle(FormBorderStyle.FixedSingle);
            }
        }

		public void setTitle(string title)
		{
            NetToolkit.BeginInvoke(delegate { control.Text = title; });
		}

		public int getState()
		{
            Form f = (Form)control;
            FormWindowState state = f.WindowState;
            switch (state)
            {
                case FormWindowState.Normal:
                    return java.awt.Frame.NORMAL;
                case FormWindowState.Maximized:
                    return java.awt.Frame.MAXIMIZED_BOTH;
                case FormWindowState.Minimized:
                    return java.awt.Frame.ICONIFIED;
                default:
                    throw new InvalidEnumArgumentException();
            }
		}

		public void setState(int state)
		{
			NetToolkit.BeginInvoke(delegate
               {
                   MyForm form = (MyForm) control;
			       switch(state)
			       {
                       case java.awt.Frame.NORMAL:
                           form.WindowState = FormWindowState.Normal;
                           break;
                       case java.awt.Frame.MAXIMIZED_BOTH:
			               form.WindowState = FormWindowState.Maximized;
			               break;
                       case java.awt.Frame.ICONIFIED:
                           form.WindowState = FormWindowState.Minimized;
                           break;
                   }
               });
		}

        public void setMaximizedBounds(java.awt.Rectangle rect)
		{
            ((MyForm)control).setMaximizedBounds(rect);
		}

		public void setBoundsPrivate(int x, int y, int width, int height)
		{
			// TODO use control.Invoke
			control.Bounds = new Rectangle(x, y, width, height);
		}

        public java.awt.Rectangle getBoundsPrivate()
        {
            throw new NotImplementedException();
        }

		internal override void create(NetComponentPeer parent)
		{
			AwtToolkit.CreateComponent(Create, parent);
		}

		void Create(NetComponentPeer parent)
		{
			Form form = new MyForm();
			form.Tag = this;
			if (parent != null)
			{
				form.Owner = parent.control.FindForm();
			}
			NetToolkit.CreateNative(form);
			this.control = form;
		}
    }

    class NetDialogPeer : NetWindowPeer, java.awt.peer.DialogPeer
	{
        public NetDialogPeer(java.awt.Dialog target)
			: base(target)
		{
            ((Form)control).MaximizeBox = false;
			((Form)control).MinimizeBox = false;
            ((Form)control).ShowInTaskbar = false;
            setTitle(target.getTitle());
            setResizable(target.isResizable());
        }

		public void setTitle(string title)
		{
            NetToolkit.BeginInvoke(delegate { control.Text = title; });
		}

        public void setResizable(bool resizable)
        {
            if (resizable)
            {
                setFormBorderStyle(FormBorderStyle.Sizable);
            }
            else
            {
                setFormBorderStyle(FormBorderStyle.FixedDialog);
            }
        }

        public void blockWindows(List toBlock)
        {
            // code copies from sun.awt.windows.WDialogPeer.java
            for (Iterator it = toBlock.iterator(); it.hasNext();) {
                java.awt.Window w = (java.awt.Window)it.next();
                java.awt.peer.WindowPeer wp = (java.awt.peer.WindowPeer)ComponentAccessor.getPeer(w);
                if (wp != null) {
                    wp.setModalBlocked((java.awt.Dialog)target, true);
                }
            }
        }

        internal override void create(NetComponentPeer parent)
        {
            AwtToolkit.CreateComponent(Create, parent);
        }

        void Create(NetComponentPeer parent)
        {
            Form form = new MyForm();
			form.Tag = this;
			if (parent != null)
            {
                form.Owner = parent.control.FindForm();
            }
            NetToolkit.CreateNative(form);
            this.control = form;
        }
    }

    sealed class NetKeyboardFocusManagerPeer : java.awt.peer.KeyboardFocusManagerPeer
    {
        private readonly java.awt.KeyboardFocusManager manager;
        private static java.lang.reflect.Method m_removeLastFocusRequest;

        public NetKeyboardFocusManagerPeer(java.awt.KeyboardFocusManager manager)
        {
            this.manager = manager;
        }

        public void clearGlobalFocusOwner(java.awt.Window activeWindow)
        {
        }

        public java.awt.Component getCurrentFocusOwner()
        {
            return getNativeFocusOwner();
        }

        public void setCurrentFocusOwner(java.awt.Component component)
        {
        }

        public java.awt.Window getCurrentFocusedWindow()
        {
            return getNativeFocusedWindow();
        }

		private static java.awt.Component getNativeFocusOwner()
		{
			return (java.awt.Component)NetToolkit.bogusForm.Invoke((Func<java.awt.Component>)delegate
			{
				UndecoratedForm form = Form.ActiveForm as UndecoratedForm;
				if (form != null)
				{
					Control control = form.ActiveControl;
					while (control is ContainerControl)
					{
						control = ((ContainerControl)control).ActiveControl;
					}
					NetComponentPeer peer;
					if (control == null)
					{
						peer = NetComponentPeer.FromControl(form);
					}
					else
					{
						while ((peer = NetComponentPeer.FromControl(form)) == null)
						{
							control = control.Parent;
						}
					}
					return peer.target;
				}
				return null;
			});
		}

		private static java.awt.Window getNativeFocusedWindow()
        {
            return (java.awt.Window)NetToolkit.bogusForm.Invoke((Func<java.awt.Window>)delegate
            {
				Form form = Form.ActiveForm;
                if (form != null)
                {
					NetComponentPeer peer = NetComponentPeer.FromControl(form);
					if (peer != null)
					{
						return (java.awt.Window)peer.target;
					}
                }
                return null;
            });
        }

		public static void removeLastFocusRequest(java.awt.Component heavyweight)
        {
            try
            {
                if (m_removeLastFocusRequest == null)
                {
                    m_removeLastFocusRequest = SunToolkit.getMethod(typeof(java.awt.KeyboardFocusManager), "removeLastFocusRequest",
                                                                  new java.lang.Class[] { typeof(java.awt.Component) });
                }
                m_removeLastFocusRequest.invoke(null, new Object[] { heavyweight });
            }
            catch (java.lang.reflect.InvocationTargetException ite)
            {
                ite.printStackTrace();
            }
            catch (java.lang.IllegalAccessException ex)
            {
                ex.printStackTrace();
            }
        }
    }

    class NetListPeer : NetComponentPeer, java.awt.peer.ListPeer
	{
		internal NetListPeer(java.awt.List target)
			: base(target)
		{
		}

		public void add(String item, int index)
		{
			throw new NotImplementedException();
		}

		public void addItem(String item, int index)
		{
			throw new NotImplementedException();
		}

		public void clear()
		{
			throw new NotImplementedException();
		}

		public void delItems(int start_index, int end_index)
		{
			throw new NotImplementedException();
		}

		public void deselect(int index)
		{
			throw new NotImplementedException();
		}

		public int[] getSelectedIndexes()
		{
			throw new NotImplementedException();
		}

		public void makeVisible(int index)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension minimumSize(int s)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension preferredSize(int s)
		{
			throw new NotImplementedException();
		}

		public void removeAll()
		{
			throw new NotImplementedException();
		}

		public void select(int index)
		{
			throw new NotImplementedException();
		}

		public void setMultipleMode(bool multi)
		{
			throw new NotImplementedException();
		}

		public void setMultipleSelections(bool multi)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getPreferredSize(int s)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getMinimumSize(int s)
		{
			throw new NotImplementedException();
		}

		internal override void create(NetComponentPeer parent)
		{
			throw new NotImplementedException();
		}
	}

    class NetDesktopPeer : java.awt.peer.DesktopPeer
	{
		private static void ShellExecute(string file, string verb)
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo(file);
				psi.UseShellExecute = true;
				psi.Verb = verb;
				Process p = Process.Start(psi);
				if (p != null)
				{
					p.Dispose();
				}
			}
			catch (System.ComponentModel.Win32Exception x)
			{
				throw new java.io.IOException(x.Message);
			}
		}

		public void browse(URI uri)
		{
			ShellExecute(uri.toString(), "open");
		}

		public void edit(java.io.File f)
		{
			ShellExecute(f.toString(), "edit");
		}

		public bool isSupported(java.awt.Desktop.Action da)
		{
			return da == java.awt.Desktop.Action.BROWSE
				|| da == java.awt.Desktop.Action.EDIT
				|| da == java.awt.Desktop.Action.MAIL
				|| da == java.awt.Desktop.Action.OPEN
				|| da == java.awt.Desktop.Action.PRINT;
		}

		public void mail(URI uri)
		{
			if (uri.getScheme().ToLower(System.Globalization.CultureInfo.InvariantCulture) != "mailto")
			{
				throw new java.lang.IllegalArgumentException("URI scheme is not \"mailto\"");
			}
			ShellExecute(uri.toString(), "open");
		}

		public void mail()
		{
			ShellExecute("mailto:", "open");
		}

		public void open(java.io.File f)
		{
			ShellExecute(f.toString(), "open");
		}

		public void print(java.io.File f)
		{
			ShellExecute(f.toString(), "print");
		}
	}

    //also WFileDialogPeer extends from WWindowPeer
	class NetFileDialogPeer : NetWindowPeer, java.awt.peer.FileDialogPeer
	{
		internal NetFileDialogPeer(java.awt.FileDialog dialog) : base(dialog)
		{
		}

		public void setDirectory(string str)
		{
		}

		public void setFile(string str)
		{
		}

		public void setFilenameFilter(java.io.FilenameFilter ff)
		{
		}

		public void setResizable(bool b)
		{
		}

		public void setTitle(string str)
		{
		}

		public override void show()
		{
            java.awt.FileDialog dialog = (java.awt.FileDialog)target;
			if (dialog.getMode() != java.awt.FileDialog.LOAD)
			{
				throw new NotImplementedException();
			}
			Thread t = new Thread((ThreadStart)delegate
			{
				using (OpenFileDialog dlg = new OpenFileDialog())
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						dialog.setFile(Path.GetFileName(dlg.FileName));
						dialog.setDirectory(Path.GetDirectoryName(dlg.FileName) + java.io.File.separator);
						dialog.hide();
					}
					else
					{
						dialog.setFile(null);
						dialog.hide();
					}
				}
			});
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
		}

        public void blockWindows(List l)
        {
            throw new NotImplementedException();
        }
	}

    class NetSystemTrayPeer : java.awt.peer.SystemTrayPeer
    {
        private java.awt.SystemTray target;

        internal NetSystemTrayPeer(java.awt.SystemTray target)
        {
            this.target = target;
        }

        public java.awt.Dimension getTrayIconSize()
        {
            return new java.awt.Dimension(NetTrayIconPeer.TRAY_ICON_WIDTH, NetTrayIconPeer.TRAY_ICON_HEIGHT);
        }

        public bool isSupported()
        {
            return ((NetToolkit) java.awt.Toolkit.getDefaultToolkit()).isTraySupported();
        }
    }

    class NetPopupMenuPeer : java.awt.peer.PopupMenuPeer
    {
        public void show(java.awt.Event e)
        {
            throw new NotImplementedException();
        }

        public void show(java.awt.Component origin, java.awt.Point p)
        {
            NetComponentPeer peer = (NetComponentPeer)NetToolkit.targetToPeer(origin);
            java.awt.Event e = new java.awt.Event(origin, 0, java.awt.Event.MOUSE_DOWN, p.x, p.y, 0, 0);
            if (peer == null)
            {
                java.awt.Component nativeOrigin = NetToolkit.getNativeContainer(origin);
                e.target = nativeOrigin;
            }
            e.x = p.x;
            e.y = p.y;
            nativeShow(e);
        }

        private void nativeShow(java.awt.Event e)
        {
            
        }

        public void dispose()
        {
            throw new NotImplementedException();
        }

        public void setFont(java.awt.Font f)
        {
            throw new NotImplementedException();
        }

        public void disable()
        {
            throw new NotImplementedException();
        }

        public void enable()
        {
            throw new NotImplementedException();
        }

        public void setEnabled(bool b)
        {
            throw new NotImplementedException();
        }

        public void setLabel(string str)
        {
            throw new NotImplementedException();
        }

        public void addItem(java.awt.MenuItem mi)
        {
            throw new NotImplementedException();
        }

        public void addSeparator()
        {
            throw new NotImplementedException();
        }

        public void delItem(int i)
        {
            throw new NotImplementedException();
        }
    }

    class NetTrayIconPeer : java.awt.peer.TrayIconPeer
    {
        internal const int TRAY_ICON_WIDTH = 16;
        internal const int TRAY_ICON_HEIGHT = 16;
        internal const int TRAY_ICON_MASK_SIZE = TRAY_ICON_WIDTH*TRAY_ICON_HEIGHT/8;

        private java.awt.TrayIcon target;
        private NotifyIcon notifyIcon;
        private java.awt.Frame popupParent = new java.awt.Frame("PopupMessageWindow");
        private java.awt.PopupMenu popup;
        private bool disposed;
        private bool isMouseClick;
        private bool isDoubleClick;
        private bool isPopupMenu;

        internal NetTrayIconPeer(java.awt.TrayIcon target)
        {
            this.target = target;
            popupParent.addNotify();
            create();
            updateImage();
        }

        public void displayMessage(string caption, string text, string messageType)
        {
            ToolTipIcon icon = ToolTipIcon.None;
            switch(messageType)
            {
                case "ERROR" :
                    icon = ToolTipIcon.Error;
                    break;
                case "WARNING" :
                    icon = ToolTipIcon.Warning;
                    break;
                case "INFO" :
                    icon = ToolTipIcon.Info;
                    break;
                case "NONE" :
                    icon = ToolTipIcon.None;
                    break;
            }
            NetToolkit.BeginInvoke(delegate
                                  {
                                      notifyIcon.ShowBalloonTip(10000, caption, text, icon);
                                  });
        }

        private void create()
        {
            NetToolkit.Invoke(delegate
                                  {
                                      notifyIcon = new NotifyIcon();
                                      hookEvents();
                                      notifyIcon.Visible = true;
                                  });
        }

        public void dispose()
        {
            bool callDisposed = true;
            lock (this)
            {
                if (disposed)
                    callDisposed = false;
                disposed = true;
            }
            if (callDisposed)
                disposeImpl();
        }

        protected void disposeImpl()
        {
            if (popupParent != null)
            {
                popupParent.dispose();
            }
            NetToolkit.targetDisposedPeer(target, this);
            NetToolkit.BeginInvoke(delegate { nativeDispose(); });
        }

        private void hookEvents()
        {
            notifyIcon.MouseClick += new MouseEventHandler(OnClick);
            notifyIcon.MouseDoubleClick += new MouseEventHandler(OnDoubleClick);
            notifyIcon.MouseDown += new MouseEventHandler(OnMouseDown);
            notifyIcon.MouseUp += new MouseEventHandler(OnMouseUp);
            notifyIcon.MouseMove += new MouseEventHandler(OnMouseMove);
        }

        private void unhookEvents()
        {
            notifyIcon.MouseClick -= new MouseEventHandler(OnClick);
            notifyIcon.MouseDoubleClick -= new MouseEventHandler(OnDoubleClick);
            notifyIcon.MouseDown -= new MouseEventHandler(OnMouseDown);
            notifyIcon.MouseUp -= new MouseEventHandler(OnMouseUp);
            notifyIcon.MouseMove -= new MouseEventHandler(OnMouseMove);
        }

        internal void postEvent(java.awt.AWTEvent evt)
        {
            NetToolkit.postEvent(NetToolkit.targetToAppContext(target), evt);
        }
        
        private void postMouseEvent(MouseEventArgs ev, int id, int clicks)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = NetComponentPeer.GetMouseEventModifiers(ev);
            int button = NetComponentPeer.GetButton(ev);
            int clickCount = clicks;
            int x = Control.MousePosition.X;
            int y = Control.MousePosition.Y;
            bool isPopup = isPopupMenu;
            java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate
            {
                java.awt.Component fake = new java.awt.TextField();
                java.awt.@event.MouseEvent mouseEvent = new java.awt.@event.MouseEvent(fake, id, when, modifiers, x, y, clickCount, isPopup, button);
                mouseEvent.setSource(target);
                postEvent(mouseEvent);
            }));
            isPopupMenu = false;
        }

        private void postMouseEvent(EventArgs ev, int id)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = NetComponentPeer.GetModifiers(Control.ModifierKeys);
            int button = 0;
            int clickCount = 0;
            int x = Control.MousePosition.X;
            int y = Control.MousePosition.Y;
            bool isPopup = isPopupMenu;
            java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate
            {
                java.awt.Component fake = new java.awt.TextField();
                java.awt.@event.MouseEvent mouseEvent = new java.awt.@event.MouseEvent(fake, id, when, modifiers, x, y, clickCount, isPopup, button);
                mouseEvent.setSource(target);
                postEvent(mouseEvent);
            }));
            isPopupMenu = false;
        }

        protected virtual void OnMouseDown(object sender, MouseEventArgs ev)
        {
            isPopupMenu = false;
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_PRESSED, ev.Clicks);
        }

        private void OnClick(object sender, MouseEventArgs ev)
        {
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_CLICKED, ev.Clicks);
        }

        private void OnDoubleClick(object sender, MouseEventArgs ev)
        {
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_CLICKED, ev.Clicks);
            long when = java.lang.System.currentTimeMillis();
            int modifiers = NetComponentPeer.GetModifiers(Control.ModifierKeys);
            postEvent(new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED, target.getActionCommand(), when, modifiers));
        }

        private void OnMouseUp(object sender, MouseEventArgs ev)
        {
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_RELEASED, ev.Clicks);
        }

        private void OnMouseEnter(object sender, EventArgs ev)
        {
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_ENTERED);
        }

        private void OnMouseLeave(object sender, EventArgs ev)
        {
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_EXITED);
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs ev)
        {
            if ((ev.Button & (MouseButtons.Left | MouseButtons.Right)) != 0)
            {
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_DRAGGED, ev.Clicks);
            }
            else
            {
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_MOVED, ev.Clicks);
            }
        }

        private void nativeDispose()
        {
            if (notifyIcon!=null)
            {
                unhookEvents();
                notifyIcon.Dispose();
            }
        }

        public void setToolTip(string str)
        {
            NetToolkit.BeginInvoke(delegate { notifyIcon.Text = str; });
        }

        protected bool isDisposed()
        {
            return disposed;
        }

        public void showPopupMenu(int x, int y)
        {
            if (isDisposed())
                return;
            java.lang.Runnable runnable = Delegates.toRunnable(delegate
               {
                   java.awt.PopupMenu newPopup = ((java.awt.TrayIcon)target).getPopupMenu();
                    if (popup != newPopup) {
                        if (popup != null) {
                            popupParent.remove(popup);
                        }
                        if (newPopup != null) {
                            popupParent.add(newPopup);
                        }
                        popup = newPopup;
                    }
                    if (popup != null) {
                        ((NetPopupMenuPeer)popup.getPeer()).show(popupParent, new java.awt.Point(x, y));
                    }
               });
            SunToolkit.executeOnEventHandlerThread(target, runnable);
        }

        public void updateImage()
        {
            java.awt.Image image = ((java.awt.TrayIcon) target).getImage();
            if (image != null)
            {
                updateNativeImage(image);
            }
        }

        private void updateNativeImage(java.awt.Image image)
        {
            lock (this)
            {
                if (isDisposed())
                    return;
                bool autosize = ((java.awt.TrayIcon) target).isImageAutoSize();

                using (Bitmap bitmap = getNativeImage(image, autosize))
                {
                    IntPtr hicon = bitmap.GetHicon();
                    Icon icon = Icon.FromHandle(hicon);
                    notifyIcon.Icon = icon;
                }
            }
        }

        private Bitmap getNativeImage(java.awt.Image image, bool autosize)
        {
            if (image is NoImage)
                return new Bitmap(TRAY_ICON_WIDTH, TRAY_ICON_HEIGHT);
            else
            {
                Image netImage = J2C.ConvertImage(image);
                if (autosize)
                    return new Bitmap(netImage, TRAY_ICON_WIDTH, TRAY_ICON_HEIGHT);
                else
                    return new Bitmap(netImage);
            }
        }
    }
    
    public class NetClipboard : java.awt.datatransfer.Clipboard
    {
        private System.Collections.Generic.Dictionary<int, java.awt.datatransfer.DataFlavor> flavorById =
            new System.Collections.Generic.Dictionary<int, java.awt.datatransfer.DataFlavor>();
        private System.Collections.Generic.Dictionary<string, java.awt.datatransfer.DataFlavor> flavorByName =
            new System.Collections.Generic.Dictionary<string, java.awt.datatransfer.DataFlavor>();
        public static readonly java.awt.datatransfer.FlavorTable flavorMap =
            (java.awt.datatransfer.FlavorTable)java.awt.datatransfer.SystemFlavorMap.getDefaultFlavorMap();

        public NetClipboard() : base("System") { }

        public override void setContents(java.awt.datatransfer.Transferable contents, java.awt.datatransfer.ClipboardOwner owner)
        {
            if (contents == null)
            {
                throw new java.lang.NullPointerException("contents");
            }

            java.awt.datatransfer.ClipboardOwner oldOwner = this.owner;
            java.awt.datatransfer.Transferable oldContents = this.contents;
            try
            {
                this.owner = owner;
                this.contents = new sun.awt.datatransfer.TransferableProxy(contents, true);

                setContentsNative(contents);
            }
            finally
            {
                if (oldOwner != null && oldOwner != owner)
                {
                    java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate() 
                        {
                            oldOwner.lostOwnership(this, oldContents);
                        }));
                }
            }
        }

        private void setContentsNative(java.awt.datatransfer.Transferable contents)
        {
            IDataObject clipObj = NetDataTransferer.getInstanceImpl().getDataObject(contents, flavorMap);
            NetToolkit.BeginInvoke(delegate
                                       {
                                           Clipboard.SetDataObject(clipObj, true);
                                       });
        }

        public override java.awt.datatransfer.Transferable getContents(object requestor)
        {
            if (contents != null)
            {
                return contents;
            }
            return new NetClipboardTransferable(this);
        }

        private java.awt.datatransfer.DataFlavor GetFlavor(DataFormats.Format format)
        {
            string name = format.Name;
            string mimeType;
            switch (name)
            {
                case "Unicode String":
                    mimeType = "text/string";
                    break;
                default:
                    mimeType = null;
                    break;
            }
            return new java.awt.datatransfer.DataFlavor(mimeType, name);
        }

        internal java.awt.datatransfer.DataFlavor GetFlavor(int id)
        {
            java.awt.datatransfer.DataFlavor flavor;
            if (!flavorById.TryGetValue(id, out flavor))
            {
                DataFormats.Format format = DataFormats.GetFormat(id);
                flavorById[id] = GetFlavor(format);
            }
            return flavor;
        }

        internal java.awt.datatransfer.DataFlavor GetFlavor(string name)
        {
            java.awt.datatransfer.DataFlavor flavor;
            if (!flavorByName.TryGetValue(name, out flavor))
            {
                DataFormats.Format format = DataFormats.GetFormat(name);
                flavorByName[name] = GetFlavor(format);
            }
            return flavor;
        }

    }

    public class NetClipboardTransferable : java.awt.datatransfer.Transferable
    {
        private HashMap flavorToData = new HashMap();
        private java.awt.datatransfer.DataFlavor[] flavors;
        public NetClipboardTransferable(NetClipboard clipboard)
        {
            IDataObject data = Clipboard.GetDataObject();
            java.awt.datatransfer.FlavorTable map = (java.awt.datatransfer.FlavorTable) java.awt.datatransfer.SystemFlavorMap.getDefaultFlavorMap();
            if (data != null)
            {
                string[] formats = data.GetFormats();
                if (formats != null && formats.Length > 0)
                {
                    long[] longFormats = NetDataTransferer.getInstanceImpl().getClipboardFormatCodes(formats);
                    Map/*<DataFlavor,long>*/ flavorMap = NetDataTransferer.getInstanceImpl().getFlavorsForFormats(longFormats, map);
                    flavors = (java.awt.datatransfer.DataFlavor[])(flavorMap.keySet().toArray(new java.awt.datatransfer.DataFlavor[0]));
                    object[] dataArr = new object[formats.Length];
                    for (int i = 0; i < formats.Length; i++)
                    {
                        string format = formats[i];
                        dataArr[i] = data.GetData(format);
                    }
                    for(Iterator iter = flavorMap.entrySet().iterator(); iter.hasNext();)
                    {
                        Map.Entry entry = (Map.Entry) iter.next();
                        long formatCode = ((java.lang.Long)entry.getValue()).longValue();
                        int idx;
                        for (idx = 0; idx < formats.Length; idx++)
                        {
                            if (longFormats[idx] == formatCode) break;
                        }
                        if (idx<formats.Length)
                        {
                            java.awt.datatransfer.DataFlavor flavor = (java.awt.datatransfer.DataFlavor) entry.getKey();
                            flavorToData.put(flavor, dataArr[idx]);
                        }
                    }
//                    Map flavorMap = NetDataTransferer.getInstanceImpl().getFlavorsForFormats(formats, map);
//                    flavors = (java.awt.datatransfer.DataFlavor[])(flavorMap.keySet().toArray(new java.awt.datatransfer.DataFlavor[0]));
//                    for(Iterator iter = flavorMap.entrySet().iterator(); iter.hasNext();)
//                    {
//                        Map.Entry entry = (Map.Entry) iter.next();
//                        string format = (string) entry.getValue();
//                        if (format == "UNICODE TEXT") format = "UnicodeText";
//                        if (format == "TEXT") format = "Text";
//                        java.awt.datatransfer.DataFlavor flavor = (java.awt.datatransfer.DataFlavor) entry.getKey();
//                        object objData = ConvertData(data.GetData(format), flavor, format);
//                        flavorToData.put(flavor, objData);
//                    }
                }
            }
        }

        public java.awt.datatransfer.DataFlavor[] getTransferDataFlavors()
        {
            return flavors;
        }

        public object getTransferData(java.awt.datatransfer.DataFlavor df)
        {
            return flavorToData.get(df);
        }

        public bool isDataFlavorSupported(java.awt.datatransfer.DataFlavor df)
        {
            return flavorToData.containsKey(df);
        }
    }

    public class NetDataTransferer : sun.awt.IkvmDataTransferer
    {
        public static readonly java.awt.datatransfer.FlavorTable flavorMap =
            (java.awt.datatransfer.FlavorTable)java.awt.datatransfer.SystemFlavorMap.getDefaultFlavorMap();

        class NetToolkitThreadBlockedHandler : sun.awt.datatransfer.ToolkitThreadBlockedHandler
        {
		    public void enter() 
            {
                if (!NetToolkit.bogusForm.InvokeRequired)
                    Application.DoEvents();
                //throw new NotImplementedException();
		    }

		    public void exit() 
            {
                //throw new NotImplementedException();
		    }

            public void @lock()
            {
                //throw new NotImplementedException();
            }

            public void unlock()
            {
                //throw new NotImplementedException();
            }
        }


        private static readonly NetDataTransferer instance = new NetDataTransferer();
        private static readonly NetToolkitThreadBlockedHandler handler = new NetToolkitThreadBlockedHandler();

        public static NetDataTransferer getInstanceImpl()
        {
            return instance;
        }

        internal long[] getClipboardFormatCodes(string[] formats)
        {
            long[] longData = new long[formats.Length];
            for(int i=0; i<formats.Length; i++)
            {
                DataFormats.Format dataFormat = DataFormats.GetFormat(formats[i]);
                longData[i] = dataFormat==null?0:dataFormat.Id;
            }
            return longData;
        }

        internal string getNativeClipboardFormatName(long format)
        {
            DataFormats.Format dataFormat = DataFormats.GetFormat((int)format);
            if (dataFormat == null)
                return null;
            else
                return dataFormat.Name;
        }

        internal IDataObject getDataObject(java.awt.datatransfer.Transferable transferable, java.awt.datatransfer.FlavorTable flavorMap)
        {
            DataObject obj = new DataObject();
            SortedMap formatMap = getFormatsForTransferable(transferable, flavorMap);
            for (Iterator iterator = formatMap.entrySet().iterator(); iterator.hasNext();)
            {
                Map.Entry entry = (Map.Entry) iterator.next();
                java.awt.datatransfer.DataFlavor flavor = (java.awt.datatransfer.DataFlavor) entry.getValue();
                try
                {
                    if (java.awt.datatransfer.DataFlavor.javaFileListFlavor.equals(flavor))
                    {
                        List list = (List) transferable.getTransferData(flavor);
                        System.Collections.Specialized.StringCollection files =
                            new System.Collections.Specialized.StringCollection();
                        for (int i = 0; i < list.size(); i++)
                        {
                            files.Add(((java.io.File) list.get(i)).getAbsolutePath());
                        }
                        obj.SetFileDropList(files);
                    }
                    else if (flavor.isFlavorTextType())
                    {
                        obj.SetText((string) transferable.getTransferData(flavor));
                    }
                    else if (java.awt.datatransfer.DataFlavor.imageFlavor.equals(flavor))
                    {
                        java.awt.Image image = transferable.getTransferData(flavor) as java.awt.Image;
                        if (image != null)
                        {
                            Image netImage = J2C.ConvertImage(image);
                            if (netImage != null)
                            {
                                obj.SetImage(netImage);
                            }
                        }
                    }
                }
                catch (java.io.IOException e)
                {
                    if (!(flavor.isMimeTypeEqual(java.awt.datatransfer.DataFlavor.javaJVMLocalObjectMimeType) &&
                          e is java.io.NotSerializableException))
                    {
                        e.printStackTrace();
                    }
                }
            }
            return obj;
        }

        protected override string getClipboardFormatName(long format)
        {
            return getNativeClipboardFormatName(format);
        }

        protected override byte[] imageToStandardBytes(java.awt.Image image, string mimeType)
        {
            if (image is NoImage) return null;
            Image netImage = J2C.ConvertImage(image);
            ImageFormat format;
            switch(mimeType)
            {
                case "image/jpg":
                case "image/jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case "image/png":
                    format = ImageFormat.Png;
                    break;
                case "image/gif":
                    format = ImageFormat.Gif;
                    break;
                case "image/x-win-metafile":
                case "image/x-wmf":
                case "image/wmf":
                    format = ImageFormat.Wmf;
                    break;
                default:
                    return null;
            }
            using(MemoryStream stream = new MemoryStream())
            {
                netImage.Save(stream, format);
                return stream.GetBuffer();
            }
        }

        public override sun.awt.datatransfer.ToolkitThreadBlockedHandler getToolkitThreadBlockedHandler()
        {
            return handler;
        }
    }

}
