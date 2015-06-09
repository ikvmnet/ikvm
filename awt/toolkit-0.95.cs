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
Copyright (C) 2006-2013 Volker Berlin (i-net software)
Copyright (C) 2010-2011 Karsten Heinrich (i-net software)

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
using System.Drawing.Imaging;
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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace ikvm.awt
{
    internal delegate TResult Func<TResult>();
    internal delegate void Action<T>(T t);

	class UndecoratedForm : Form
	{
	    private bool focusableWindow = true;
	    private bool alwaysOnTop;

		public UndecoratedForm()
		{
			setBorderStyle();
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
		}

        protected virtual void setBorderStyle()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        internal void SetWindowState(bool focusableWindow, bool alwaysOnTop)
        {
            this.focusableWindow = focusableWindow;
            this.alwaysOnTop = alwaysOnTop;
        }

        protected override bool ShowWithoutActivation {
            // This work not like in Java. In Java it is not possible to click on a not focusable Window
            // But now the windows is not stealing the focus on showing
            get
            {
                return !focusableWindow;
            }
        }

	    private const int WS_EX_TOPMOST = 0x00000008;
        private const int WS_EX_NOACTIVATE = 0x08000000;
	    private const int WS_DISABLED = 0x08000000;

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;
                int exStyle = baseParams.ExStyle;

                // This work not like in Java. In Java it is not possible to click on a not focusable Window
                // But now the windows is not stealing the focus on showing
                exStyle = focusableWindow ? exStyle & ~WS_EX_NOACTIVATE : exStyle | WS_EX_NOACTIVATE;
                
                // we need to set TOPMOST here because the property TopMost does not work with ShowWithoutActivation
                baseParams.ExStyle = alwaysOnTop ? exStyle | WS_EX_TOPMOST : exStyle & ~WS_EX_TOPMOST;

                // the Enabled on Forms has no effect. In Java a window beep if ot is disabled
                // the same effect have we with this flag
                baseParams.Style = Enabled ? baseParams.Style & ~WS_DISABLED : baseParams.Style | WS_DISABLED;
                return baseParams;
            }
        }

		private const int WM_MOUSEACTIVATE = 0x0021;
		private const int MA_NOACTIVATE = 0x0003;

		protected override void WndProc(ref Message m)
		{
			if (!focusableWindow && m.Msg == WM_MOUSEACTIVATE)
			{
				m.Result = (IntPtr)MA_NOACTIVATE;
				return;
			}
			base.WndProc(ref m);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			NetComponentPeer peer = NetComponentPeer.FromControl(this);
			if (peer.eraseBackground)
			{
				base.OnPaintBackground(e);
			}
		}
	}

    class MyForm : UndecoratedForm
	{
        /// <summary>
        /// Original MaximizedBounds
        /// </summary>
        private Rectangle maxBounds;
        private bool maxBoundsSet;
        public java.awt.Insets peerInsets;

        /// <summary>
        /// Creates the native form
        /// </summary>
        /// <param name="peerInsets">the insets instance of the peer instance</param>
		public MyForm( java.awt.Insets peerInsets )
		{
            this.peerInsets = peerInsets;
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
                    SetMaximizedBoundsImpl(maxBounds);
                }
            }
            else
            {
                if (!maxBoundsSet)
                {
                    maxBounds = MaximizedBounds;
                    maxBoundsSet = true;
                }
                SetMaximizedBoundsImpl(J2C.ConvertRect(rect));
            }
        }

		private void SetMaximizedBoundsImpl(Rectangle rect)
		{
			NetToolkit.Invoke(delegate { MaximizedBounds = rect; });
		}
	}

	sealed class EventQueueSynchronizationContext : SynchronizationContext
	{
		public override SynchronizationContext CreateCopy()
		{
			return new EventQueueSynchronizationContext();
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate { d(state); }));
		}

		public override void Send(SendOrPostCallback d, object state)
		{
			java.awt.EventQueue.invokeAndWait(Delegates.toRunnable(delegate { d(state); }));
		}

		[System.Security.SecuritySafeCritical]
		internal static void Install()
		{
			SynchronizationContext.SetSynchronizationContext(new EventQueueSynchronizationContext());
		}
	}

	static class WinFormsMessageLoop
	{
		private static readonly object mylock = new object();
		private static Form theForm;

		private static Form GetForm()
		{
			lock (mylock)
			{
				if (theForm == null)
				{
					Thread thread = new Thread(MessageLoop);
					thread.SetApartmentState(ApartmentState.STA);
					thread.Name = "IKVM AWT WinForms Message Loop";
					thread.IsBackground = true;
					thread.Start();
					while (theForm == null && thread.IsAlive)
					{
						Thread.Sleep(1);
					}
				}
			}
			return theForm;
		}

		private static void MessageLoop()
		{
			using (Form form = new Form())
			{
				NetToolkit.CreateNative(form);
				theForm = form;
				Application.Run();
			}
		}

		internal static bool InvokeRequired
		{
			get { return GetForm().InvokeRequired; }
		}

		internal static object Invoke(Delegate method)
		{
			return GetForm().Invoke(method);
		}

		internal static object Invoke(Delegate method, params object[] args)
		{
			return GetForm().Invoke(method, args);
		}

		internal static IAsyncResult BeginInvoke(Delegate method)
		{
			return GetForm().BeginInvoke(method);
		}

		internal static IAsyncResult BeginInvoke(Delegate method, params object[] args)
		{
			return GetForm().BeginInvoke(method, args);
		}
	}

	public sealed class NetToolkit : sun.awt.SunToolkit, ikvm.awt.IkvmToolkit, sun.awt.KeyboardFocusManagerPeerProvider
    {
        private int resolution;
        private NetClipboard clipboard;
		private bool eventQueueSynchronizationContext;

		protected internal override java.awt.EventQueue getSystemEventQueueImpl()
		{
			java.awt.EventQueue eq = base.getSystemEventQueueImpl();
			if (!eventQueueSynchronizationContext)
			{
				InstallEventQueueSynchronizationContext(eq);
			}
			return eq;
		}

		private void InstallEventQueueSynchronizationContext(java.awt.EventQueue eq)
		{
			bool install;
			lock (this)
			{
				install = !eventQueueSynchronizationContext;
				eventQueueSynchronizationContext = true;
			}
			if (install)
			{
				eq.postEvent(new java.awt.@event.InvocationEvent(this, Delegates.toRunnable(EventQueueSynchronizationContext.Install), null, true));
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
        }

        /// <summary>
        /// Run on a win 32 system
        /// </summary>
        /// <returns></returns>
        internal static bool isWin32()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows;
        }

        protected internal override void loadSystemColors(int[] systemColors)
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
			java.awt.peer.ButtonPeer peer = Invoke<NetButtonPeer>(delegate { return new NetButtonPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

		// MONOBUG mcs refuses to override these two methods, so we disable them when building with mcs
		// (since AWT isn't supported anyway)
#if !__MonoCS__
        public override java.awt.peer.CanvasPeer createCanvas(java.awt.Canvas target)
        {
            java.awt.peer.CanvasPeer peer = Invoke<NetCanvasPeer>(delegate { return new NetCanvasPeer(target); });
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.peer.PanelPeer createPanel(java.awt.Panel target)
        {
            java.awt.peer.PanelPeer peer = Invoke<NetPanelPeer>(delegate { return new NetPanelPeer(target); });
            targetCreatedPeer(target, peer);
            return peer;
        }
#endif

        public override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
        {
			java.awt.peer.TextFieldPeer peer = Invoke<NetTextFieldPeer>(delegate { return new NetTextFieldPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
        {
			java.awt.peer.LabelPeer peer = Invoke<NetLabelPeer>(delegate { return new NetLabelPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ListPeer createList(java.awt.List target)
        {
			java.awt.peer.ListPeer peer = Invoke<NetListPeer>(delegate { return new NetListPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
        {
			java.awt.peer.CheckboxPeer peer = Invoke<NetCheckboxPeer>(delegate { return new NetCheckboxPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ScrollbarPeer createScrollbar(java.awt.Scrollbar target)
        {
			java.awt.peer.ScrollbarPeer peer = Invoke<NetScrollbarPeer>(delegate { return new NetScrollbarPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ScrollPanePeer createScrollPane(java.awt.ScrollPane target)
        {
			java.awt.peer.ScrollPanePeer peer = Invoke<NetScrollPanePeer>(delegate { return new NetScrollPanePeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.TextAreaPeer createTextArea(java.awt.TextArea target)
        {
			java.awt.peer.TextAreaPeer peer = Invoke<NetTextAreaPeer>(delegate { return new NetTextAreaPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
        {
			java.awt.peer.ChoicePeer peer = Invoke<NetChoicePeer>(delegate { return new NetChoicePeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
        {
			bool isFocusableWindow = target.isFocusableWindow();
			bool isAlwaysOnTop = target.isAlwaysOnTop();
            java.awt.peer.FramePeer peer = Invoke<NetFramePeer>(delegate { return new NetFramePeer(target, isFocusableWindow, isAlwaysOnTop); });
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
        {
			bool isFocusableWindow = target.isFocusableWindow();
			bool isAlwaysOnTop = target.isAlwaysOnTop();
			java.awt.peer.WindowPeer peer = Invoke<NetWindowPeer>(delegate { return new NetWindowPeer(target, isFocusableWindow, isAlwaysOnTop); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
        {
			bool isFocusableWindow = target.isFocusableWindow();
			bool isAlwaysOnTop = target.isAlwaysOnTop();
			java.awt.peer.DialogPeer peer = Invoke<NetDialogPeer>(delegate { return new NetDialogPeer(target, isFocusableWindow, isAlwaysOnTop); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.MenuBarPeer createMenuBar(java.awt.MenuBar target)
        {
			// we need to force peer creation of the sub menus here, because we're
			// transitioning to the UI thread to do the rest of the work and there
			// we cannot acquire the AWT tree lock (because it is owned by the current thread)
			for (int i = 0; i < target.getMenuCount(); i++)
			{
				target.getMenu(i).addNotify();
			}
			java.awt.Menu help = target.getHelpMenu();
			if (help != null)
			{
				help.addNotify();
			}
			java.awt.peer.MenuBarPeer peer = Invoke<NetMenuBarPeer>(delegate { return new NetMenuBarPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
        }

        public override java.awt.peer.MenuPeer createMenu(java.awt.Menu target)
        {
			for (int i = 0; i < target.getItemCount(); i++)
			{
				target.getItem(i).addNotify();
			}
			java.awt.peer.MenuPeer peer = Invoke<NetMenuPeer>(delegate { return new NetMenuPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
        }

        public override java.awt.peer.PopupMenuPeer createPopupMenu(java.awt.PopupMenu target)
        {
			for (int i = 0; i < target.getItemCount(); i++)
			{
				target.getItem(i).addNotify();
			}
			java.awt.peer.PopupMenuPeer peer = Invoke<NetPopupMenuPeer>(delegate { return new NetPopupMenuPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
        }

        public override java.awt.peer.MenuItemPeer createMenuItem(java.awt.MenuItem target)
        {
			java.awt.peer.MenuItemPeer peer = Invoke<NetMenuItemPeer>(delegate { return new NetMenuItemPeer(target); });
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.FileDialogPeer createFileDialog(java.awt.FileDialog target)
        {
			bool isFocusableWindow = target.isFocusableWindow();
			bool isAlwaysOnTop = target.isAlwaysOnTop();
			java.awt.peer.FileDialogPeer peer = Invoke<NetFileDialogPeer>(delegate { return new NetFileDialogPeer(target, isFocusableWindow, isAlwaysOnTop); });
            targetCreatedPeer(target, peer);
            return peer;
        }

        public override java.awt.peer.CheckboxMenuItemPeer createCheckboxMenuItem(java.awt.CheckboxMenuItem target)
        {
			return new NetCheckboxMenuItemPeer(target);
		}

        public override java.awt.peer.FontPeer getFontPeer(string name, int style)
        {
            throw new NotImplementedException();
        }

        public override java.awt.peer.KeyboardFocusManagerPeer getKeyboardFocusManagerPeer()
        {
            return new NetKeyboardFocusManagerPeer();
        }

        public override java.awt.Dimension getScreenSize()
        {
            return new java.awt.Dimension(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        public override int getScreenResolution()
        {
            if (resolution == 0)
            {
                using (Form form = new Form())
                using (Graphics g = form.CreateGraphics())
                {
                    resolution = (int)Math.Round(g.DpiY);
                }
            }
            return resolution;
        }

        public override java.awt.image.ColorModel getColorModel()
        {
            //we return the default ColorModel because this produce the fewest problems with convertions
            return java.awt.image.ColorModel.getRGBdefault();
        }

        public override void sync()
        {
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
                return new NoImage(new sun.awt.image.FileImageSource(filename));
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
                return new NoImage(new sun.awt.image.URLImageSource(url));
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

        public override java.awt.Image createImage(byte[] imagedata, int imageoffset, int imagelength)
        {
            try
            {
                return new java.awt.image.BufferedImage(new Bitmap(new MemoryStream(imagedata, imageoffset, imagelength, false)));
            }
            catch (Exception)
            {
                return new NoImage(new sun.awt.image.ByteArrayImageSource(imagedata, imageoffset, imagelength));
            }
        }

        public override java.awt.PrintJob getPrintJob(java.awt.Frame frame, string jobtitle, java.util.Properties props)
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
        protected internal override java.awt.peer.DesktopPeer createDesktopPeer(java.awt.Desktop target)
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

        protected internal override void initializeDesktopProperties()
        {
            //copied from WToolkit.java
            desktopProperties.put("DnD.Autoscroll.initialDelay", java.lang.Integer.valueOf(50));
            desktopProperties.put("DnD.Autoscroll.interval", java.lang.Integer.valueOf(50));

            try
            {
                if (isWin32())
                {
                    desktopProperties.put("Shell.shellFolderManager", "sun.awt.shell.Win32ShellFolderManager2" );
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

        protected internal override Object lazilyLoadDesktopProperty(String name)
        {
            switch (name)
            {
                case "win.defaultGUI.font":
                    return C2J.ConvertFont(Control.DefaultFont);
                case "win.highContrast.on":
                    return java.lang.Boolean.valueOf(SystemInformation.HighContrast);
                default:
                    return null;
            }
        }

        protected internal override java.awt.peer.MouseInfoPeer getMouseInfoPeer() {
            return new NetMouseInfoPeer();
        }

        /*===============================
         * Implementations of interface IkvmToolkit
         */

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

        /// <summary>
        /// Create a outline from the given text and font parameter
        /// </summary>
        /// <param name="javaFont">the font</param>
        /// <param name="frc">font render context</param>
        /// <param name="text">the text</param>
        /// <param name="x">x - position</param>
        /// <param name="y">y - position</param>
        /// <returns></returns>
        public java.awt.Shape outline(java.awt.Font javaFont, java.awt.font.FontRenderContext frc, string text, float x, float y) {
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            Font netFont = javaFont.getNetFont();
            FontFamily family = netFont.FontFamily;
            FontStyle style = netFont.Style;
            float factor = netFont.Size / family.GetEmHeight(style);
            float ascent = family.GetCellAscent(style) * factor;
            y -= ascent;

            StringFormat format = new StringFormat(StringFormat.GenericTypographic);
            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;
            format.Trimming = StringTrimming.None;

            path.AddString(text, family, (int)style, netFont.Size, new PointF(x, y), format);
            return C2J.ConvertShape(path);
        }

        /*===============================
         * Implementations of interface SunToolkit
         */

        public override bool isModalExclusionTypeSupported(java.awt.Dialog.ModalExclusionType dmet)
        {
            return false;
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

        protected internal override int getScreenHeight()
        {
            return Screen.PrimaryScreen.Bounds.Height;
        }

        protected internal override int getScreenWidth()
        {
            return Screen.PrimaryScreen.Bounds.Width;
        }

		public override java.awt.Insets getScreenInsets(java.awt.GraphicsConfiguration gc)
		{
			NetGraphicsConfiguration ngc = gc as NetGraphicsConfiguration;
			if (ngc != null)
			{
				Rectangle rectWorkingArea = ngc.screen.WorkingArea;
				Rectangle rectBounds = ngc.screen.Bounds;
				return new java.awt.Insets(
					rectWorkingArea.Top - rectBounds.Top,
					rectWorkingArea.Left - rectBounds.Left,
					rectBounds.Bottom - rectWorkingArea.Bottom,
					rectBounds.Right - rectWorkingArea.Right);
			}
			else
			{
				return base.getScreenInsets(gc);
			}
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
        
        protected internal override bool syncNativeQueue(long l)
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

		internal new static object targetToPeer(object target)
		{
			return SunToolkit.targetToPeer(target);
		}

        internal new static void targetDisposedPeer(object target, object peer)
        {
            SunToolkit.targetDisposedPeer(target, peer);
        }

		internal static void BeginInvoke(MethodInvoker del)
        {
            if (WinFormsMessageLoop.InvokeRequired)
            {
                WinFormsMessageLoop.BeginInvoke(del);
            }
            else
            {
                del();
            }
        }

        internal static void BeginInvoke<T>(Action<T> del, T t)
        {
            if (WinFormsMessageLoop.InvokeRequired)
            {
                WinFormsMessageLoop.BeginInvoke(del, t);
            }
            else
            {
                del(t);
            }
        }
        internal static void Invoke<T>(Action<T> del, T t)
        {
            if (WinFormsMessageLoop.InvokeRequired)
            {
                WinFormsMessageLoop.Invoke(del, t);
            }
            else
            {
                del(t);
            }
        }

        internal static TResult Invoke<TResult>(Func<TResult> del)
        {
            if (WinFormsMessageLoop.InvokeRequired)
            {
                return (TResult)WinFormsMessageLoop.Invoke(del);
            }
            else
            {
                return del();
            }
        }

        internal static void Invoke(MethodInvoker del)
        {
            if (WinFormsMessageLoop.InvokeRequired)
            {
                WinFormsMessageLoop.Invoke(del);
            }
            else
            {
                del();
            }
        }

		public override bool areExtraMouseButtonsEnabled()
		{
			return true;
		}

		public override java.awt.peer.FramePeer createLightweightFrame(sun.awt.LightweightFrame lf)
		{
			throw new NotImplementedException();
		}

		public override sun.awt.datatransfer.DataTransferer getDataTransferer()
		{
			return NetDataTransferer.getInstanceImpl();
		}
	}

	sealed class NetMenuBarPeer : java.awt.peer.MenuBarPeer
	{
		internal readonly MainMenu menu = new MainMenu();

		internal NetMenuBarPeer(java.awt.MenuBar target)
		{
			menu.Tag = target;
			for (int i = 0; i < target.getMenuCount(); i++)
			{
				addMenu(target.getMenu(i));
			}
		}

		public void addHelpMenu(java.awt.Menu m)
		{
			addMenu(m);
		}

		public void addMenu(java.awt.Menu m)
		{
			if (m.getPeer() == null)
			{
				m.addNotify();
			}
			NetToolkit.Invoke(delegate { menu.MenuItems.Add(((NetMenuPeer)m.getPeer()).menu); });
		}

		public void delMenu(int i)
		{
			NetToolkit.Invoke(delegate { menu.MenuItems.RemoveAt(i); });
		}

		public void dispose()
		{
			NetToolkit.Invoke(delegate { menu.Dispose(); });
		}

		public void setFont(java.awt.Font f)
		{
			throw new NotImplementedException();
		}
	}

	sealed class NetMenuPeer : java.awt.peer.MenuPeer
	{
		internal readonly MenuItem menu = new MenuItem();

		internal NetMenuPeer(java.awt.Menu target)
		{
			menu.Tag = target;
			menu.Text = target.getLabel();
			for (int i = 0; i < target.getItemCount(); i++)
			{
				addItem(target.getItem(i));
			}
		}

		public void addItem(java.awt.MenuItem item)
		{
			if (item.getPeer() == null)
			{
				item.addNotify();
			}
			if (item.getPeer() is NetMenuItemPeer)
			{
				NetToolkit.Invoke(delegate { menu.MenuItems.Add(((NetMenuItemPeer)item.getPeer()).menuitem); });
			}
			else
			{
				NetToolkit.Invoke(delegate { menu.MenuItems.Add(((NetMenuPeer)item.getPeer()).menu); });
			}
		}

		public void addSeparator()
		{
			NetToolkit.Invoke(delegate { menu.MenuItems.Add(new MenuItem("-")); });
		}

		public void delItem(int i)
		{
			NetToolkit.Invoke(delegate { menu.MenuItems.RemoveAt(i); });
		}

		public void dispose()
		{
			NetToolkit.Invoke(delegate { menu.Dispose(); });
		}

		public void setFont(java.awt.Font f)
		{
			throw new NotImplementedException();
		}

		public void disable()
		{
			setEnabled(false);
		}

		public void enable()
		{
			setEnabled(true);
		}

		public void setEnabled(bool b)
		{
			NetToolkit.Invoke(delegate { menu.Enabled = b; });
		}

		public void setLabel(string str)
		{
			NetToolkit.Invoke(delegate { menu.Text = str; });
		}
	}

	class NetMenuItemPeer : java.awt.peer.MenuItemPeer
	{
		protected readonly java.awt.MenuItem target;
		internal readonly MenuItem menuitem = new MenuItem();

		internal NetMenuItemPeer(java.awt.MenuItem target)
		{
			this.target = target;
			setEnabled(target.isEnabled());
			setLabel(target.getLabel());
			menuitem.Click += OnClick;
		}

		protected virtual void OnClick(object sender, EventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = NetComponentPeer.GetModifiers(Control.ModifierKeys);
			NetToolkit.executeOnEventHandlerThread(target, Delegates.toRunnable(delegate
			{
				NetToolkit.postEvent(NetToolkit.targetToAppContext(target), new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED,
						  target.getActionCommand(), when, modifiers));
			}));
		}

		public void disable()
		{
			setEnabled(false);
		}

		public void enable()
		{
			setEnabled(true);
		}

		public void setEnabled(bool b)
		{
			NetToolkit.Invoke(delegate { menuitem.Enabled = b; });
		}

		public void setLabel(string str)
		{
			NetToolkit.Invoke(delegate { menuitem.Text = str; });
		}

		public void dispose()
		{
			NetToolkit.Invoke(delegate { menuitem.Dispose(); });
		}

		public void setFont(java.awt.Font f)
		{
		}
	}

	sealed class NetCheckboxMenuItemPeer : NetMenuItemPeer, java.awt.peer.CheckboxMenuItemPeer
	{
		internal NetCheckboxMenuItemPeer(java.awt.CheckboxMenuItem target)
			: base(target)
		{
			setState(target.getState());
		}

		protected override void OnClick(object sender, EventArgs e)
		{
			java.awt.CheckboxMenuItem target = (java.awt.CheckboxMenuItem)this.target;
			NetToolkit.executeOnEventHandlerThread(target, Delegates.toRunnable(delegate
			{
				bool state = !menuitem.Checked;
				target.setState(state);
				NetToolkit.postEvent(NetToolkit.targetToAppContext(target), new java.awt.@event.ItemEvent(target, java.awt.@event.ItemEvent.ITEM_STATE_CHANGED,
										target.getLabel(), (state)
										  ? java.awt.@event.ItemEvent.SELECTED
										  : java.awt.@event.ItemEvent.DESELECTED));
			}));
		}

		public void setState(bool b)
		{
			NetToolkit.Invoke(delegate { menuitem.Checked = b; });
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
            {
			    java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
                        base.dragDropFinished(success, operations, x, y);
			    }));
            }
            dragStart = false;
        }

        protected internal override void startDrag(java.awt.datatransfer.Transferable trans, long[] formats, Map formatMap)
        {
            dragStart = true;

            createDragSource(getTrigger().getComponent(),
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
                    peer.performedDragDropEffects = DragDropEffects.None;
                    Control control = peer.Control;
                    if (control != null)
                    {
                        java.awt.dnd.DragSource dragSource = getTrigger().getDragSource();
                        IDataObject data = NetDataTransferer.getInstanceImpl().getDataObject(transferable, 
                            NetDataTransferer.adaptFlavorMap(dragSource.getFlavorMap()));
                        NetToolkit.BeginInvoke(delegate
                                                   {
                                                       DragDropEffects effects = control.DoDragDrop(data, DragDropEffects.All);
                                                       if (effects == DragDropEffects.None && peer.performedDragDropEffects!=DragDropEffects.None) 
                                                       {
                                                           effects = peer.performedDragDropEffects;
                                                       }
                                                       peer.performedDragDropEffects = DragDropEffects.None;
                                                       dragDropFinished(effects != DragDropEffects.None,
                                                                        NetComponentPeer.getAction(effects),
                                                                        Control.MousePosition.X, Control.MousePosition.Y);
                                                   });
                    }
                }
            }
            return 0;
        }

        protected internal override void setNativeCursor(long nativeCtxt, java.awt.Cursor c, int cType)
        {
            
        }
    }

    internal class NetDropTargetContextPeer : sun.awt.dnd.SunDropTargetContextPeer
    {
        private IDataObject data;

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
                                       sun.awt.dnd.SunDropTargetEvent.MOUSE_ENTERED,
                                       sun.awt.dnd.SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal void handleExitMessage(java.awt.Component component,
                                   long nativeCtxt)
        {
            postDropTargetEvent(component, 0, 0, java.awt.dnd.DnDConstants.ACTION_NONE,
                                java.awt.dnd.DnDConstants.ACTION_NONE, null, nativeCtxt,
                                sun.awt.dnd.SunDropTargetEvent.MOUSE_EXITED,
                                sun.awt.dnd.SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal int handleMotionMessage(java.awt.Component component,
                                    int x, int y,
                                    int dropAction,
                                    int actions, long[] formats,
                                    long nativeCtxt)
        {
            return postDropTargetEvent(component, x, y, dropAction, actions,
                                       formats, nativeCtxt,
                                       sun.awt.dnd.SunDropTargetEvent.MOUSE_DRAGGED,
                                       sun.awt.dnd.SunDropTargetContextPeer.DISPATCH_SYNC);
        }

        internal void handleDropMessage(java.awt.Component component,
                               int x, int y,
                               int dropAction, int actions,
                                long[] formats,
                                long nativeCtxt, IDataObject data)
        {
            this.data = data;
            postDropTargetEvent(component, x, y, dropAction, actions,
                                formats, nativeCtxt,
                                sun.awt.dnd.SunDropTargetEvent.MOUSE_DROPPED,
                                !sun.awt.dnd.SunDropTargetContextPeer.DISPATCH_SYNC);
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
            NetComponentPeer peer = (NetComponentPeer)component.getPeer();
            Control control = peer.Control;
            Point screenPt = new Point(x, y);
            Point clientPt = control.PointToClient(screenPt);
            return base.postDropTargetEvent(component, clientPt.X, clientPt.Y,
                                     dropAction, actions, formats, nativeCtxt, eventID, dispatchType);
        }

        protected internal override void doDropDone(bool success, int dropAction, bool isLocal)
        {
            // Don't do anything as .NET framework already handle the message pump
        }

        public override bool isDataFlavorSupported(java.awt.datatransfer.DataFlavor df)
        {
            if (isTransferableJVMLocal())
                return base.isDataFlavorSupported(df);
            return base.isDataFlavorSupported(df);
        }

        public override object getTransferData(java.awt.datatransfer.DataFlavor df)
        {
            if (isTransferableJVMLocal())
                return base.getTransferData(df);
            return new NetClipboardTransferable(data).getTransferData(df);
        }

        protected internal override object getNativeData(long l)
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

            Bitmap bitmap = J2C.ConvertImage(cursorIm);
			IntPtr hIcon = bitmap.GetHicon();
			cursor = new Cursor(hIcon);
		}
	}

	sealed class NetLightweightComponentPeer : NetComponentPeer<java.awt.Component, Control>, java.awt.peer.LightweightPeer
	{
		public NetLightweightComponentPeer(java.awt.Component target)
			: base(target)
		{
		}

		protected override Control CreateControl()
		{
			throw new NotImplementedException();
		}
	}

    sealed class NetLightweightContainerPeer : NetContainerPeer<java.awt.Container, ContainerControl>, java.awt.peer.LightweightPeer
    {
        public NetLightweightContainerPeer(java.awt.Container target)
            : base(target)
        {
        }
    }

	abstract class NetComponentPeer : java.awt.peer.ComponentPeer
	{
		internal bool eraseBackground = true;

		public abstract void applyShape(sun.java2d.pipe.Region r);
		public abstract bool canDetermineObscurity();
		public abstract int checkImage(java.awt.Image i1, int i2, int i3, java.awt.image.ImageObserver io);
		public abstract void coalescePaintEvent(java.awt.@event.PaintEvent pe);
		public abstract void createBuffers(int i, java.awt.BufferCapabilities bc);
		public abstract java.awt.Image createImage(int i1, int i2);
		public abstract java.awt.Image createImage(java.awt.image.ImageProducer ip);
		public abstract java.awt.image.VolatileImage createVolatileImage(int i1, int i2);
		public abstract void destroyBuffers();
		public abstract void disable();
		public abstract void dispose();
		public abstract void enable();
		public abstract void flip(java.awt.BufferCapabilities.FlipContents bcfc);
		public abstract java.awt.Image getBackBuffer();
		public abstract java.awt.Rectangle getBounds();
		public abstract java.awt.image.ColorModel getColorModel();
		public abstract java.awt.FontMetrics getFontMetrics(java.awt.Font f);
		public abstract java.awt.Graphics getGraphics();
		public abstract java.awt.GraphicsConfiguration getGraphicsConfiguration();
		public abstract java.awt.Point getLocationOnScreen();
		public abstract java.awt.Dimension getMinimumSize();
		public abstract java.awt.Dimension getPreferredSize();
		public abstract java.awt.Toolkit getToolkit();
		public abstract void handleEvent(java.awt.AWTEvent awte);
		public abstract bool handlesWheelScrolling();
		public abstract void hide();
		public abstract bool isFocusable();
		public abstract bool isObscured();
		public abstract bool isReparentSupported();
		public abstract void layout();
		public abstract java.awt.Dimension minimumSize();
		public abstract void paint(java.awt.Graphics g);
		public abstract java.awt.Dimension preferredSize();
		public abstract bool prepareImage(java.awt.Image i1, int i2, int i3, java.awt.image.ImageObserver io);
		public abstract void print(java.awt.Graphics g);
		public abstract void repaint(long l, int i1, int i2, int i3, int i4);
		public abstract void reparent(java.awt.peer.ContainerPeer cp);
		public abstract bool requestFocus(java.awt.Component c, bool b1, bool b2, long l, CausedFocusEvent.Cause cfec);
		public abstract void reshape(int i1, int i2, int i3, int i4);
		public abstract void setBackground(java.awt.Color c);
		public abstract void setBounds(int i1, int i2, int i3, int i4, int i5);
		public abstract void setEnabled(bool b);
		public abstract void setFont(java.awt.Font f);
		public abstract void setForeground(java.awt.Color c);
		public abstract void setVisible(bool b);
		public abstract void show();
		public abstract void updateCursorImmediately();
		public abstract void flip(int x1, int y1, int x2, int y2, java.awt.BufferCapabilities.FlipContents flipAction);
		public abstract void setZOrder(java.awt.peer.ComponentPeer above);
        public abstract bool updateGraphicsData(java.awt.GraphicsConfiguration gc);

		internal DragDropEffects performedDragDropEffects = DragDropEffects.None;

		internal abstract Control Control { get; }
		internal abstract java.awt.Component Target { get; }

        internal abstract int getInsetsLeft();
		internal abstract int getInsetsTop();

		internal static int getAction(DragDropEffects effects)
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

		internal static int GetMouseEventModifiers(MouseEventArgs ev)
		{
			int modifiers = GetModifiers(Control.ModifierKeys);
			//Which button was pressed or released, because it can only one that it is a switch
			MouseButtons button = ev.Button;
			switch (button)
			{
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
            switch (keys & (Keys.Control | Keys.Alt))
            {
                case Keys.Control:
                    modifiers |= java.awt.@event.InputEvent.CTRL_DOWN_MASK;
                    break;
                case Keys.Alt:
                    modifiers |= java.awt.@event.InputEvent.ALT_DOWN_MASK;
                    break;
                case Keys.Control | Keys.Alt:
                    modifiers |= java.awt.@event.InputEvent.ALT_GRAPH_DOWN_MASK;
                    break;
            }
			if ((Control.MouseButtons & MouseButtons.Left) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.BUTTON1_DOWN_MASK;
			}
			if ((Control.MouseButtons & MouseButtons.Middle) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.BUTTON2_DOWN_MASK;
			}
			if ((Control.MouseButtons & MouseButtons.Right) != 0)
			{
				modifiers |= java.awt.@event.InputEvent.BUTTON3_DOWN_MASK;
			}
			return modifiers;
		}

		internal static int GetButton(MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON1;
			}
			else if ((e.Button & MouseButtons.Middle) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON2;
			}
			else if ((e.Button & MouseButtons.Right) != 0)
			{
				return java.awt.@event.MouseEvent.BUTTON3;
			}
			else
			{
				return java.awt.@event.MouseEvent.NOBUTTON;
			}
		}

		internal static NetComponentPeer FromControl(Control control)
		{
			return (NetComponentPeer)control.Tag;
		}
	}

    abstract class NetComponentPeer<T, C> : NetComponentPeer
		where T : java.awt.Component
		where C : Control
	{
		protected static readonly java.awt.Font defaultFont = new java.awt.Font(java.awt.Font.DIALOG, java.awt.Font.PLAIN, 12);
		internal readonly T target;
		internal readonly C control;
        private bool isMouseClick;
        private bool isDoubleClick;
        private bool isPopupMenu;
		private int oldWidth = -1;
		private int oldHeight = -1;
		private bool sm_suppressFocusAndActivation;
		//private bool m_callbacksEnabled;
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

		internal override Control Control
		{
			get { return control; }
		}

		internal override java.awt.Component Target
		{
			get { return target; }
		}

		public NetComponentPeer(T target)
		{
			this.target = target;
			this.paintArea = new RepaintArea();
			java.awt.Container parent = SunToolkit.getNativeContainer(target);
			NetComponentPeer parentPeer = (NetComponentPeer)NetToolkit.targetToPeer(parent);
			control = Create(parentPeer);
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

			// this is from initialize() in WCanvasPeer.java
			eraseBackground = !SunToolkit.getSunAwtNoerasebackground();
			if (!PaintEventDispatcher.getPaintEventDispatcher().shouldDoNativeBackgroundErase(target))
			{
				eraseBackground = false;
			}
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
			//m_callbacksEnabled = enabled;
		}

		private C Create(NetComponentPeer parent)
		{
			C control = CreateControl();
			control.Tag = this;
			if (parent != null)
			{
				Form form = control as Form;
				if (form != null)
				{
					form.Owner = parent.Control.FindForm();
				}
				else
				{
					control.Parent = parent.Control;
				}
			}
			NetToolkit.CreateNative(control);
			return control;
		}

		protected abstract C CreateControl();

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
		internal override int getInsetsLeft()
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
		internal override int getInsetsTop()
        {
            return 0;
        }


        /// <summary>
        /// .NET calculates the offset relative to the detail area.
        /// Java uses the top left point of a window.
        /// That means we must compensate the coordinate of a component
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
                    NetComponentPeer peer = parent.getPeer() as NetComponentPeer;
                    if (peer != null)
                    {
						return new Point(peer.getInsetsLeft(), peer.getInsetsTop());
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
			if (!AWTAccessor.getComponentAccessor().getIgnoreRepaint(target))
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
            if (dropTargetPeer != null)
                dropTargetPeer.handleDropMessage(target, e.X, e.Y, getAction(e.Effect), getAction(e.AllowedEffect),
                                                 formats, 0, e.Data);
            NetDragSourceContextPeer.getInstance().dragDropFinished(true, actions, e.X, e.Y);
            performedDragDropEffects = e.Effect;
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
            if (isMouseClick || isDoubleClick) // there can only be an Click OR an DoubleClick event - both count as click here
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
			java.awt.Container cont = (java.awt.Container)(object)target;

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

		public override int checkImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			return getToolkit().checkImage(img, width, height, ob);
		}

		public override java.awt.Image createImage(java.awt.image.ImageProducer prod)
		{
            return new sun.awt.image.ToolkitImage(prod);
		}

		public override java.awt.Image createImage(int width, int height)
		{
            return new java.awt.image.BufferedImage(width, height, java.awt.image.BufferedImage.TYPE_INT_ARGB);
		}

		public override void disable()
		{
            NetToolkit.BeginInvoke(delegate { Enable( false ); });
		}

		public override void dispose()
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
            NetToolkit.Invoke(nativeDispose);
        }

        protected virtual void nativeDispose()
        {
            unhookEvents();
            control.Dispose();
        }

		public override void enable()
		{
            NetToolkit.BeginInvoke(delegate { Enable(true); });
		}

		public override java.awt.image.ColorModel getColorModel()
		{
            //we return the default ColorModel because this causes the least problems with conversions
            return java.awt.image.ColorModel.getRGBdefault();
        }

		public override java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
            return sun.font.FontDesignMetrics.getMetrics(f);
		}

		public override java.awt.Graphics getGraphics()
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
				return new ComponentGraphics(this.control, target, fgColor, bgColor, font);
			}
			return null;
		}

		public override java.awt.Point getLocationOnScreen()
        {
            return NetToolkit.Invoke<java.awt.Point>(delegate
            {
				Point p = new Point(0 - getInsetsLeft(), 0 - getInsetsTop());
                p = control.PointToScreen(p);
                return new java.awt.Point(p.X, p.Y);
            });
        }

		public override java.awt.Dimension getMinimumSize()
		{
			return target.getSize();
		}

		public override java.awt.Dimension getPreferredSize()
		{
			return getMinimumSize();
		}

		public override java.awt.Toolkit getToolkit()
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

		public override void handleEvent(java.awt.AWTEvent e)
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

		public override void hide()
		{
            NetToolkit.BeginInvoke(delegate { control.Visible = false; });
		}

		public bool isFocusTraversable()
		{
			return true;
		}

		public override java.awt.Dimension minimumSize()
		{
			return getMinimumSize();
		}

		public override java.awt.Dimension preferredSize()
		{
			return getPreferredSize();
		}

		public override void paint(java.awt.Graphics graphics)
		{
			target.paint(graphics);
		}

		public override bool prepareImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			return getToolkit().prepareImage(img, width, height, ob);
		}

		public override void print(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
		}

		public override void repaint(long tm, int x, int y, int width, int height)
		{
		}

		public void requestFocus()
		{
			NetToolkit.Invoke<bool>(control.Focus);
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

		public override void reshape(int x, int y, int width, int height)
        {
            NetToolkit.BeginInvoke(delegate
            {
                Form window = control.FindForm();
                java.awt.Insets insets;
                if (window is MyForm)
                {
                    insets = ((MyForm)window).peerInsets;
                }
                else
                {
                    insets = new java.awt.Insets(0, 0, 0, 0);
                }
                control.SetBounds(x - insets.left, y - insets.top, width, height);
                //If the .NET control does not accept the new bounds (minimum size, maximum size) 
                //then we need to reflect the real bounds on the .NET site to the Java site
                Rectangle bounds = control.Bounds;
                if (bounds.X + insets.left != x || bounds.Y + insets.top != y)
                {
                    AWTAccessor.getComponentAccessor().setLocation(target, bounds.X + insets.left, bounds.Y + insets.top);
                }
                if (bounds.Width != width || bounds.Height != height)
                {
                    AWTAccessor.getComponentAccessor().setSize(target, bounds.Width, bounds.Height);
                }
            });
        }

		public override void setBackground(java.awt.Color color)
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

		public override void setBounds(int x, int y, int width, int height, int op)
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
            NetToolkit.Invoke(setCursorImpl, cursor);
        }

        public bool getEnabled()
        {
            return NetToolkit.Invoke<bool>(delegate { return control.Enabled; });
        }

        public bool getFocused()
        {
            return NetToolkit.Invoke<bool>(delegate { return control.Focused; });
        }

        public bool getVisible()
        {
            return NetToolkit.Invoke<bool>(delegate { return control.Visible; });
        }

		public override void setEnabled(bool enabled)
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

		public override void setFont(java.awt.Font font)
		{
			lock (this)
			{
				this.font = font;
				NetToolkit.BeginInvoke(delegate { control.Font = font.getNetFont(); });
			}
		}

		public override void setForeground(java.awt.Color color)
		{
			lock (this)
			{
				this.foreground = color;
				NetToolkit.BeginInvoke(delegate { control.ForeColor = J2C.ConvertColor(color); });
			}
		}

		public override void setVisible(bool visible)
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

		public override void show()
		{
			java.awt.Dimension s = target.getSize();
			oldHeight = s.height;
			oldWidth = s.width;
			pShow();
		}

		/*
		 * Return the GraphicsConfiguration associated with this peer, either
		 * the locally stored winGraphicsConfig, or that of the target Component.
		 */
		public override java.awt.GraphicsConfiguration getGraphicsConfiguration()
        {
            // we don't need a treelock here, since
            // Component.getGraphicsConfiguration() gets it itself.
            return target.getGraphicsConfiguration();
        }

		public void setEventMask (long mask)
		{
			//Console.WriteLine("NOTE: NetComponentPeer.setEventMask not implemented");
		}

		public override bool isObscured()
		{
			// should never be called because we return false from canDetermineObscurity()
			return true;
		}

		public override bool canDetermineObscurity()
		{
			// JDK returns true here and uses GetClipBox to determine if the window is partially obscured,
			// this is an optimization for scrolling in javax.swing.JViewport, since there appears to be
			// no managed equivalent of GetClipBox, we'll simply return false and forgo the optimization.
			return false;
		}

		public override void coalescePaintEvent(java.awt.@event.PaintEvent e)
		{
			java.awt.Rectangle r = e.getUpdateRect();
			if (!(e is sun.awt.@event.IgnorePaintEvent))
			{
				paintArea.add(r, e.getID());
			}
		}

		public override void updateCursorImmediately()
		{
		}

		public override java.awt.image.VolatileImage createVolatileImage(int width, int height)
		{
			return new NetVolatileImage(target, width, height);
		}

		public override bool handlesWheelScrolling()
		{
			return true;
		}

		public override void createBuffers(int x, java.awt.BufferCapabilities capabilities)
		{
			throw new NotImplementedException();
		}

		public override java.awt.Image getBackBuffer()
		{
			throw new NotImplementedException();
		}

		public override void flip(java.awt.BufferCapabilities.FlipContents contents)
		{
			throw new NotImplementedException();
		}

		public override void destroyBuffers()
		{
			throw new NotImplementedException();
		}

		public override bool isFocusable()
		{
			return false;
		}

	    protected bool isDisposed()
	    {
	        return disposed;
	    }

		public override java.awt.Rectangle getBounds()
		{
			return target.getBounds();
		}

		public override void reparent(java.awt.peer.ContainerPeer parent)
		{
			throw new NotImplementedException();
		}

		public override bool isReparentSupported()
		{
			return false;
		}

		// Do nothing for heavyweight implementation
		public override void layout()
		{
		}

        public override void applyShape(sun.java2d.pipe.Region shape)
        {
            NetToolkit.BeginInvoke(ApplyShapeImpl, shape);
        }

        private void ApplyShapeImpl(sun.java2d.pipe.Region shape)
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

		public override bool requestFocus(java.awt.Component lightweightChild, bool temporary, bool focusedWindowChangeAllowed, long time, sun.awt.CausedFocusEvent.Cause cause)
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

        /**
         * Move the back buffer to the front buffer.
         *
         * @param x1 the area to be flipped, upper left X coordinate
         * @param y1 the area to be flipped, upper left Y coordinate
         * @param x2 the area to be flipped, lower right X coordinate
         * @param y2 the area to be flipped, lower right Y coordinate
         * @param flipAction the flip action to perform
         *
         * @see Component.FlipBufferStrategy#flip
         */
        public override void flip(int x1, int y1, int x2, int y2, java.awt.BufferCapabilities.FlipContents flipAction)
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }

        /**
         * Lowers this component at the bottom of the above HW peer. If the above parameter
         * is null then the method places this component at the top of the Z-order.
         */
        public override void setZOrder(java.awt.peer.ComponentPeer above)
        {
            Control.ControlCollection controls = control.Controls;
            if (!controls.Contains(control))
            {
                // Control was not added to any window. Occur if you call addNotify without
                return;
            }
            if (above == null)
            {
                controls.SetChildIndex(control, 0);
            }
            else
            {
                NetComponentPeer<T, C> netPeer = (NetComponentPeer<T, C>)above;
                controls.SetChildIndex(control, controls.GetChildIndex(netPeer.control));
            }
        }

        /**
         * Updates internal data structures related to the component's GC.
         *
         * @return if the peer needs to be recreated for the changes to take effect
         * @since 1.7
         */
        public override bool updateGraphicsData(java.awt.GraphicsConfiguration gc)
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }

	}

	sealed class NetScrollbarPeer : NetComponentPeer<java.awt.Scrollbar, ScrollBar>, java.awt.peer.ScrollbarPeer
	{
		internal NetScrollbarPeer(java.awt.Scrollbar target)
			: base(target)
		{
		}

		public void setLineIncrement(int i)
		{
		}

		public void setPageIncrement(int i)
		{
		}

		public void setValues(int i1, int i2, int i3, int i4)
		{
		}

		protected override ScrollBar CreateControl()
		{
			switch (target.getOrientation())
			{
				case java.awt.Scrollbar.VERTICAL:
					return new VScrollBar();
				default:
					return new HScrollBar();
			}
		}
	}

	sealed class NetScrollPanePeer : NetComponentPeer<java.awt.ScrollPane, ScrollableControl>, java.awt.peer.ScrollPanePeer
	{
		internal NetScrollPanePeer(java.awt.ScrollPane pane)
			: base(pane)
		{
		}

		public void childResized(int i1, int i2)
		{
		}

		public int getHScrollbarHeight()
		{
			return NetToolkit.Invoke<int>(delegate { return 0; });
		}

		public int getVScrollbarWidth()
		{
			return NetToolkit.Invoke<int>(delegate { return 0; });
		}

		public void setScrollPosition(int i1, int i2)
		{
		}

		public void setUnitIncrement(java.awt.Adjustable a, int i)
		{
		}

		public void setValue(java.awt.Adjustable a, int i)
		{
		}

		public java.awt.Insets getInsets()
		{
			return NetToolkit.Invoke<java.awt.Insets>(delegate { return new java.awt.Insets(0, 0, 0, 0); });
		}

		public java.awt.Insets insets()
		{
			return getInsets();
		}

		public bool isRestackSupported()
		{
			return false;
		}

		public void restack()
		{
			throw new NotImplementedException();
		}

		protected override ScrollableControl CreateControl()
		{
			return new ScrollableControl();
		}
	}

    sealed class NetButtonPeer : NetComponentPeer<java.awt.Button, Button>, java.awt.peer.ButtonPeer
	{
		public NetButtonPeer(java.awt.Button awtbutton)
			: base(awtbutton)
		{
			if (!awtbutton.isBackgroundSet())
			{
				awtbutton.setBackground(java.awt.SystemColor.control);
			}
			control.BackColor = Color.FromArgb(awtbutton.getBackground().getRGB());
			control.Text = awtbutton.getLabel();
			control.Click += new EventHandler(OnClick);
		}

		private void OnClick(object sender, EventArgs e)
		{
			// TODO set all these properties correctly
			string cmd = "";
			long when = 0;
			int modifiers = 0;
			postEvent(new java.awt.@event.ActionEvent(target, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
		}

		public void setLabel(string label)
		{
			NetToolkit.Invoke(delegate { control.Text = label; });
		}

		public override java.awt.Dimension getMinimumSize()
		{
			using(Graphics g = control.CreateGraphics())
			{
				// TODO get these fudge factors from somewhere
				return new java.awt.Dimension((int)Math.Round(12 + g.MeasureString(control.Text, control.Font).Width) * 8 / 7, 6 + control.Font.Height * 8 / 7);
			}
		}

        public override bool shouldClearRectBeforePaint()
        {
            return false;
        }

		protected override Button CreateControl()
		{
			return new Button();
		}
	}

    abstract class NetTextComponentPeer<T> : NetComponentPeer<T, TextBox>, java.awt.peer.TextComponentPeer
		where T : java.awt.TextComponent
	{
		public NetTextComponentPeer(java.awt.TextComponent textComponent)
			: base((T)textComponent)
		{
#if __MonoCS__
			// MONOBUG mcs generates a ldflda on a readonly field, so we use a temp
			T target = this.target;
#endif
			if (!target.isBackgroundSet())
			{
				target.setBackground(java.awt.SystemColor.window);
			}
			setBackground(target.getBackground());
			control.AutoSize = false;
			control.Text = target.getText();
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
			return NetToolkit.Invoke<int>(delegate { return control.SelectionStart + control.SelectionLength; });
		}

		public int getSelectionStart()
		{
			return NetToolkit.Invoke<int>(delegate { return control.SelectionStart; });
		}

		public string getText()
		{
		    return NetToolkit.Invoke<string>(delegate { return control.Text; });
		}

		public void setText(string text)
		{
			NetToolkit.Invoke(delegate { control.Text = text; });
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
			return getSelectionStart();
		}

		private void setCaretPositionImpl(int pos)
		{
			control.SelectionStart = pos;
			control.SelectionLength = 0;
		}

		public void setCaretPosition(int pos)
		{
			NetToolkit.Invoke(setCaretPositionImpl, pos);
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

		protected sealed override TextBox CreateControl()
		{
			return new TextBox();
		}
	}

	sealed class NetChoicePeer : NetComponentPeer<java.awt.Choice, RadioButton>, java.awt.peer.ChoicePeer
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

		protected override RadioButton CreateControl()
		{
			return new RadioButton();
		}
	}

    sealed class NetCheckboxPeer : NetComponentPeer<java.awt.Checkbox, CheckBox>, java.awt.peer.CheckboxPeer
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

		protected override CheckBox CreateControl()
		{
			return new CheckBox();
		}
	}

    sealed class NetLabelPeer : NetComponentPeer<java.awt.Label, Label>, java.awt.peer.LabelPeer
	{
		public NetLabelPeer(java.awt.Label jlabel)
			: base(jlabel)
		{
			control.Text = jlabel.getText();
			setAlignment(jlabel.getAlignment());
		}

		public void setAlignment(int align)
		{
		    ContentAlignment alignment;
			switch(align)
			{
				case java.awt.Label.LEFT:
			        alignment = ContentAlignment.TopLeft;
					break;
				case java.awt.Label.CENTER:
                    alignment = ContentAlignment.TopCenter;
					break;
				case java.awt.Label.RIGHT:
                    alignment = ContentAlignment.TopRight;
					break;
                default:
			        return;
			}
		    NetToolkit.Invoke(setAlignImpl, alignment);
		}

		private void setAlignImpl(ContentAlignment alignment)
		{
            control.TextAlign = (ContentAlignment)alignment;
		}

		public void setText(string s)
		{
            NetToolkit.Invoke(setTextImpl, s);
		}

		private void setTextImpl(string s)
		{
			control.Text = s;
		}

		public override java.awt.Dimension preferredSize()
		{
            return NetToolkit.Invoke<java.awt.Dimension>(getPreferredSizeImpl);
		}

		private java.awt.Dimension getPreferredSizeImpl()
		{
			// HACK get these fudge factors from somewhere
			return new java.awt.Dimension(control.PreferredWidth, 2 + control.PreferredHeight);
		}

        public override bool shouldClearRectBeforePaint()
        {
            // is native control, don't clear 
            return false;
        }

		protected override Label CreateControl()
		{
			return new Label();
		}
	}

    sealed class NetTextFieldPeer : NetTextComponentPeer<java.awt.TextField>, java.awt.peer.TextFieldPeer
	{
		public NetTextFieldPeer(java.awt.TextField textField)
			: base(textField)
		{
			setEchoCharacterImpl(textField.getEchoChar());
		}

		public java.awt.Dimension minimumSize(int len)
		{
			return getMinimumSize(len);
		}

		public java.awt.Dimension preferredSize(int len)
		{
			return getPreferredSize(len);
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
            control.PasswordChar = echo_char;
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

    sealed class NetTextAreaPeer : NetComponentPeer<java.awt.TextArea, RichTextBox>, java.awt.peer.TextAreaPeer
	{
		public NetTextAreaPeer(java.awt.TextArea textArea)
			: base(textArea)
		{
			control.ReadOnly = !((java.awt.TextArea)target).isEditable();
			control.WordWrap = false;
			control.ScrollBars = RichTextBoxScrollBars.Both;
			control.Multiline = true;
			control.AutoSize = false;
			control.Text = target.getText();
		}

		public override bool isFocusable()
		{
			return true;
		}

		public int getSelectionEnd()
		{
			return NetToolkit.Invoke<int>(delegate { return control.SelectionStart + control.SelectionLength; });
		}

		public int getSelectionStart()
		{
			return NetToolkit.Invoke<int>(delegate { return control.SelectionStart; });
		}

		public string getText()
		{
			return NetToolkit.Invoke<string>(delegate { return control.Text; });
		}

		public void setText(string text)
		{
			NetToolkit.Invoke(delegate { control.Text = text; });
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
			return getSelectionStart();
		}

		private void setCaretPositionImpl(int pos)
		{
			control.SelectionStart = pos;
			control.SelectionLength = 0;
		}

		public void setCaretPosition(int pos)
		{
			NetToolkit.Invoke(setCaretPositionImpl, pos);
		}

		public void insert(string text, int pos)
		{
			NetToolkit.Invoke(delegate { control.Text = control.Text.Insert(pos, text); });
		}

		public void insertText(string text, int pos)
		{
			insert(text, pos);
		}

        public override java.awt.Dimension getMinimumSize()
        {
            return getMinimumSize(10, 60);
        }

        public java.awt.Dimension minimumSize(int rows, int cols)
        {
            return getMinimumSize(rows, cols);
        }
        
        public java.awt.Dimension getMinimumSize(int rows, int cols)
		{
            java.awt.FontMetrics fm = getFontMetrics(target.getFont());
            return new java.awt.Dimension(fm.charWidth('0') * cols + 20, fm.getHeight() * rows + 20);
        }

        public java.awt.Dimension preferredSize(int rows, int cols)
        {
            return getPreferredSize(rows, cols);
        }

        public java.awt.Dimension getPreferredSize(int rows, int cols)
		{
            return getMinimumSize(rows, cols);
		}

		public void replaceRange(string text, int start_pos, int end_pos)
		{
			NetToolkit.Invoke(delegate { control.Text = control.Text.Substring(0, start_pos) + text + control.Text.Substring(end_pos); });
		}

		public void replaceText(string text, int start_pos, int end_pos)
		{
			replaceRange(text, start_pos, end_pos);
		}

		public java.awt.im.InputMethodRequests getInputMethodRequests()
		{
			throw new NotImplementedException();
		}

		protected sealed override RichTextBox CreateControl()
		{
			return new RichTextBox();
		}
	}

    class NetContainerPeer<T, C> : NetComponentPeer<T, C>, java.awt.peer.ContainerPeer
		where T : java.awt.Container
		where C : Control
	{
        /// <summary>
        /// The native insets of the .NET Window
        /// </summary>
		protected java.awt.Insets _insets = new java.awt.Insets(0, 0, 0, 0);

		public NetContainerPeer(java.awt.Container awtcontainer)
			: base((T)awtcontainer)
		{
		}

        internal override int getInsetsLeft()
        {
            return _insets.left; ;
        }

        internal override int getInsetsTop()
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

		protected override C CreateControl()
		{
			throw new NotImplementedException();
		}
	}

    sealed class NetPanelPeer : NetContainerPeer<java.awt.Panel, ContainerControl>, java.awt.peer.PanelPeer
	{
		public NetPanelPeer(java.awt.Panel panel)
			: base(panel)
		{
		}

        protected override ContainerControl CreateControl()
		{
            return new ContainerControl();
		}
	}

    sealed class NetCanvasPeer : NetComponentPeer<java.awt.Canvas, Control>, java.awt.peer.CanvasPeer
	{
		public NetCanvasPeer(java.awt.Canvas canvas)
			: base(canvas)
		{
		}

		protected override Control CreateControl()
		{
            return new Control();
		}

        /**
         * Requests a GC that best suits this Canvas. The returned GC may differ
         * from the requested GC passed as the argument to this method. This method
         * must return a non-null value (given the argument is non-null as well).
         *
         * @since 1.7
         */
        public java.awt.GraphicsConfiguration getAppropriateGraphicsConfiguration(java.awt.GraphicsConfiguration gc)
        {
            return gc;
        }
    }

    class NetWindowPeer : NetContainerPeer<java.awt.Window, Form>, java.awt.peer.WindowPeer
	{
        // we can't use NetDialogPeer as blocker may be an instance of NetPrintDialogPeer that
        // extends NetWindowPeer, not NetDialogPeer
        private NetWindowPeer modalBlocker;
        private bool modalSavedEnabled;

        private static NetWindowPeer grabbedWindow;

		public NetWindowPeer(java.awt.Window window, bool isFocusableWindow, bool isAlwaysOnTop)
			: base(window)
		{
            //form.Shown += new EventHandler(OnOpened); Will already post in java.awt.Window.show()
			control.Closing += new CancelEventHandler(OnClosing);
			control.Closed += new EventHandler(OnClosed);
			control.Activated += new EventHandler(OnActivated);
			control.Deactivate += new EventHandler(OnDeactivate);
			control.SizeChanged += new EventHandler(OnSizeChanged);
			control.Resize += new EventHandler(OnResize);
            control.Move += new EventHandler(OnMove);
			((UndecoratedForm)control).SetWindowState(isFocusableWindow, isAlwaysOnTop);
		}

        protected override void initialize()
        {
            base.initialize();
            updateIconImages();
            if (target.getBackground() == null)
            {
                AWTAccessor.getComponentAccessor().setBackground(target, target is java.awt.Dialog ? java.awt.SystemColor.control : java.awt.SystemColor.window);
            }
            control.BackColor = J2C.ConvertColor(target.getBackground());
            if (target.getForeground() == null)
            {
                target.setForeground(java.awt.SystemColor.windowText);
            }
            if (target.getFont() == null)
            {
                //target.setFont(defaultFont);
                //HACK: Sun is calling setFont(Font) here and this is calling firePropertyChange("font", oldFont, newFont)
                //but this produce a deadlock with getTreeLock() because the creating of the peer is already in this synchronized
                java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
                {
                    java.lang.Class component = typeof(java.awt.Component);
                    java.lang.reflect.Field field = component.getDeclaredField("font");
                    field.setAccessible(true);
                    field.set(target, defaultFont);
                    java.lang.reflect.Method method = component.getDeclaredMethod(
                        "firePropertyChange",
                        typeof(java.lang.String),
                        typeof(java.lang.Object),
                        typeof(java.lang.Object));
                    method.setAccessible(true);
                    method.invoke(target, "font", null, defaultFont);
                    return null;
                }));
            }
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
            AWTAccessor.getComponentAccessor().setLocation(target, control.Left, control.Top);
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
			SendComponentEvent(java.awt.@event.ComponentEvent.COMPONENT_RESIZED);
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
			WmActivate(WA_ACTIVE, control.WindowState == FormWindowState.Minimized, null);
        }

		private void OnDeactivate(object sender, EventArgs e)
		{
			WmActivate(WA_INACTIVE, control.WindowState == FormWindowState.Minimized, null);
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

        public override bool shouldClearRectBeforePaint()
        {
            // clearing the window before repainting causes the controls to "flicker" on screen
            return false;
        }

        /// <summary>
        /// Set the border style of the window and recalc the insets
        /// </summary>
        /// <param name="style">the new style</param>
        protected void setFormBorderStyle(FormBorderStyle style)
        {
            NetToolkit.BeginInvoke(delegate
            {
				control.FormBorderStyle = style;
                //Calculate the Insets one time
                //This is many faster because there no thread change is needed.
				CalcInsetsImpl();
            });
        }

		protected void CalcInsetsImpl()
		{
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
			// only modify this instance, since it's shared by the control-peers of this form
			_insets.top = y;
			_insets.left = x;
			_insets.bottom = control.Height - client.Height - y;
			if (control.Menu != null)
			{
				_insets.bottom += SystemInformation.MenuHeight;
			}
			_insets.right = control.Width - client.Width - x;
		}

        public override void reshape(int x, int y, int width, int height)
        {
            NetToolkit.BeginInvoke(delegate
            {
                control.SetBounds(x, y, width, height);
                //If the .NET control does not accept the new bounds (minimum size, maximum size) 
                //then we need to reflect the real bounds on the .NET site to the Java site
                Rectangle bounds = control.Bounds;
                if (bounds.X != x || bounds.Y != y)
                {
                    AWTAccessor.getComponentAccessor().setLocation(target, bounds.X, bounds.Y);
                }
                if (bounds.Width != width || bounds.Height != height)
                {
                    AWTAccessor.getComponentAccessor().setSize(target, bounds.Width, bounds.Height);
                }
            });
        }

        public void toBack()
		{
			NetToolkit.BeginInvoke(control.SendToBack);
		}

		public void toFront()
		{
			NetToolkit.BeginInvoke(control.Activate);
		}

		public bool requestWindowFocus()
		{
			return NetToolkit.Invoke<bool>(control.Focus);
		}

        public void updateAlwaysOnTopState()
        {
            // The .NET property TopMost does not work with a not focusable Window
            // that we need to set the window flags directly. To reduce double code
            // we call updateFocusableWindowState().
            updateFocusableWindowState();
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
            ((UndecoratedForm)control).SetWindowState(((java.awt.Window)target).isFocusableWindow(), ((java.awt.Window)target).isAlwaysOnTop());
        }

        public void updateIconImages()
        {
            java.util.List imageList = ((java.awt.Window)target).getIconImages();
            Icon icon;
            if (imageList == null || imageList.size() == 0)
            {
                icon = null;
            }
            else
            {
                IconFactory factory = new IconFactory();
                icon = factory.CreateIcon(imageList, SystemInformation.IconSize);
            }
            NetToolkit.BeginInvoke(delegate
               {
                   ((Form)control).Icon = icon;
               });
        }

        public void updateMinimumSize()
        {
            java.awt.Dimension dim = target.getMinimumSize();
            NetToolkit.BeginInvoke(delegate
            {
				control.MinimumSize = new Size(dim.width, dim.height);
            });
        }

        /**
         * Sets the level of opacity for the window.
         *
         * @see Window#setOpacity(float)
         */
        public void setOpacity(float opacity)
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }

        /**
         * Enables the per-pixel alpha support for the window.
         *
         * @see Window#setBackground(Color)
         */
        public void setOpaque(bool isOpaque)
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }


        /**
         * Updates the native part of non-opaque window.
         *
         * @see Window#setBackground(Color)
         */
        public void updateWindow()
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }


        /**
         * Instructs the peer to update the position of the security warning.
         */
        public void repositionSecurityWarning()
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }



		protected override Form CreateControl()
		{
			return new UndecoratedForm();
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

    sealed class NetFramePeer : NetWindowPeer, java.awt.peer.FramePeer
	{
		public NetFramePeer(java.awt.Frame frame, bool isFocusableWindow, bool isAlwaysOnTop)
			: base(frame, isFocusableWindow, isAlwaysOnTop)
		{
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
			if (mb == null)
			{
				NetToolkit.Invoke(delegate
				{
					control.Menu = null;
					CalcInsetsImpl();
				});
			}
			else
			{
				mb.addNotify();
				NetToolkit.Invoke(delegate
				{
					control.Menu = ((NetMenuBarPeer)mb.getPeer()).menu;
					CalcInsetsImpl();
				});
			}
		}

        public void setResizable(bool resizable)
        {
            if (((java.awt.Frame)target).isUndecorated())
            {
                setFormBorderStyle(FormBorderStyle.None);
            }
            else
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
			NetToolkit.Invoke(delegate { control.Bounds = new Rectangle(x, y, width, height); });
		}

        public java.awt.Rectangle getBoundsPrivate()
        {
            throw new NotImplementedException();
        }

		protected override Form CreateControl()
		{
			return new MyForm(_insets);
		}

		public void emulateActivation(bool b)
		{
			throw new NotImplementedException();
		}
    }

    sealed class NetDialogPeer : NetWindowPeer, java.awt.peer.DialogPeer
	{
		public NetDialogPeer(java.awt.Dialog target, bool isFocusableWindow, bool isAlwaysOnTop)
			: base(target, isFocusableWindow, isAlwaysOnTop)
		{
			control.MaximizeBox = false;
			control.MinimizeBox = false;
			control.ShowInTaskbar = false;
            setTitle(target.getTitle());
            setResizable(target.isResizable());
        }

		public void setTitle(string title)
		{
            NetToolkit.Invoke(delegate { control.Text = title; });
		}

        public void setResizable(bool resizable)
        {
            if (((java.awt.Dialog)target).isUndecorated())
            {
                setFormBorderStyle(FormBorderStyle.None);
            }
            else
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
        }

        public void blockWindows(List toBlock)
        {
            // code copies from sun.awt.windows.WDialogPeer.java
            for (Iterator it = toBlock.iterator(); it.hasNext();) {
                java.awt.Window w = (java.awt.Window)it.next();
                java.awt.peer.WindowPeer wp = (java.awt.peer.WindowPeer)AWTAccessor.getComponentAccessor().getPeer(w);
                if (wp != null) {
                    wp.setModalBlocked((java.awt.Dialog)target, true);
                }
            }
        }

		protected override Form CreateControl()
		{
			return new MyForm(_insets);
		}
    }

    sealed class NetKeyboardFocusManagerPeer : java.awt.peer.KeyboardFocusManagerPeer
    {
        private static java.lang.reflect.Method m_removeLastFocusRequest;

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

		public void setCurrentFocusedWindow(java.awt.Window w)
		{
		}

		private static java.awt.Component getNativeFocusOwner()
		{
			return NetToolkit.Invoke<java.awt.Component>(delegate
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
					return peer.Target;
				}
				return null;
			});
		}

		private static java.awt.Window getNativeFocusedWindow()
        {
			return NetToolkit.Invoke<java.awt.Window>(delegate
            {
				Form form = Form.ActiveForm;
                if (form != null)
                {
					NetComponentPeer peer = NetComponentPeer.FromControl(form);
					if (peer != null)
					{
						return (java.awt.Window)peer.Target;
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
					java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
					{
						java.lang.Class keyboardFocusManagerCls = typeof(java.awt.KeyboardFocusManager);
						java.lang.reflect.Method method = keyboardFocusManagerCls.getDeclaredMethod(
							"removeLastFocusRequest",
							typeof(java.awt.Component));
						method.setAccessible(true);
						m_removeLastFocusRequest = method;
						return null;
					}));
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

    sealed class NetListPeer : NetComponentPeer<java.awt.List, ListBox>, java.awt.peer.ListPeer
	{
		internal NetListPeer(java.awt.List target)
			: base(target)
		{
			control.IntegralHeight = false;
			setMultipleMode(target.isMultipleMode());
			for (int i = 0; i < target.getItemCount(); i++)
			{
				add(target.getItem(i), i);
				if (target.isSelected(i))
				{
					select(i);
				}
			}
			makeVisible(target.getVisibleIndex());
		}

		public void add(string item, int index)
		{
			NetToolkit.Invoke(delegate { control.Items.Insert(index, item); });
		}

		public void addItem(string item, int index)
		{
			add(item, index);
		}

		public void clear()
		{
			NetToolkit.Invoke(delegate { control.Items.Clear(); });
		}

		public void delItems(int start_index, int end_index)
		{
			NetToolkit.Invoke(delegate
			{
				for (int i = start_index; i < end_index; i++)
				{
					control.Items.RemoveAt(start_index);
				}
			});
		}

		public void deselect(int index)
		{
			NetToolkit.Invoke(delegate { control.SelectedIndices.Remove(index); });
		}

		public int[] getSelectedIndexes()
		{
			return NetToolkit.Invoke<int[]>(delegate
			{
				ListBox.SelectedIndexCollection sic = control.SelectedIndices;
				int[] indexes = new int[sic.Count];
				sic.CopyTo(indexes, 0);
				return indexes;
			});
		}

		public void makeVisible(int index)
		{
			NetToolkit.Invoke(delegate { control.TopIndex = index; });
		}

		public java.awt.Dimension minimumSize(int s)
		{
			return getMinimumSize(s);
		}

		public java.awt.Dimension preferredSize(int s)
		{
			return getPreferredSize(s);
		}

		public void removeAll()
		{
			clear();
		}

		public void select(int index)
		{
			NetToolkit.Invoke(delegate { control.SelectedIndices.Add(index); });
		}

		public void setMultipleMode(bool multi)
		{
			NetToolkit.Invoke(delegate { control.SelectionMode = multi ? SelectionMode.MultiSimple : SelectionMode.One; });
		}

		public void setMultipleSelections(bool multi)
		{
			setMultipleMode(multi);
		}

		public java.awt.Dimension getPreferredSize(int s)
		{
			return getMinimumSize(s);
		}

		public java.awt.Dimension getMinimumSize(int s)
		{
			return new java.awt.Dimension(100, 100);
		}

		protected override ListBox CreateControl()
		{
			return new ListBox();
		}
	}

    sealed class NetDesktopPeer : java.awt.peer.DesktopPeer
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
		internal NetFileDialogPeer(java.awt.FileDialog dialog, bool isFocusableWindow, bool isAlwaysOnTop)
			: base(dialog, isFocusableWindow, isAlwaysOnTop)
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

        public void blockWindows(List toBlock)
        {
            // code copies from sun.awt.windows.WFileDialogPeer.java
            for (Iterator it = toBlock.iterator(); it.hasNext(); ) {
                java.awt.Window w = (java.awt.Window)it.next();
                java.awt.peer.WindowPeer wp = (java.awt.peer.WindowPeer)AWTAccessor.getComponentAccessor().getPeer(w);
                if (wp != null) {
                    wp.setModalBlocked((java.awt.Dialog)target, true);
                }
            }
        }
	}

    class NetSystemTrayPeer : java.awt.peer.SystemTrayPeer
    {
        //private java.awt.SystemTray target;

        internal NetSystemTrayPeer(java.awt.SystemTray target)
        {
            //this.target = target;
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

    sealed class NetPopupMenuPeer : java.awt.peer.PopupMenuPeer
    {
		private readonly java.awt.PopupMenu target;
		private readonly ContextMenu menu = new ContextMenu();

		internal NetPopupMenuPeer(java.awt.PopupMenu target)
		{
			this.target = target;
			for (int i = 0; i < target.getItemCount(); i++)
			{
				addItem(target.getItem(i));
			}
		}

		public void show(java.awt.Event e)
        {
			show((java.awt.Component)e.target, new java.awt.Point(e.x, e.y));
        }

        public void show(java.awt.Component origin, java.awt.Point p)
        {
			NetComponentPeer peer = (NetComponentPeer)origin.getPeer();
			Point pt = new Point(p.x, p.y);
			pt.Offset(- peer.getInsetsLeft(), - peer.getInsetsTop());
			NetToolkit.Invoke(delegate { menu.Show(peer.Control, pt); });
        }

        public void dispose()
        {
			NetToolkit.Invoke(delegate { menu.Dispose(); });
        }

        public void setFont(java.awt.Font f)
        {
            throw new NotImplementedException();
        }

        public void disable()
        {
			setEnabled(false);
        }

        public void enable()
        {
			setEnabled(true);
		}

        public void setEnabled(bool b)
        {
			NetToolkit.Invoke(delegate
			{
				for (int i = 0; i < target.getItemCount(); i++)
				{
					menu.MenuItems[i].Enabled = b && target.getItem(i).isEnabled();
				}
			});
		}

        public void setLabel(string str)
        {
        }

        public void addItem(java.awt.MenuItem item)
        {
			if (item.getPeer() == null)
			{
				item.addNotify();
			}
			if (item.getPeer() is NetMenuItemPeer)
			{
				NetToolkit.Invoke(delegate { menu.MenuItems.Add(((NetMenuItemPeer)item.getPeer()).menuitem); });
			}
			else
			{
				NetToolkit.Invoke(delegate { menu.MenuItems.Add(((NetMenuPeer)item.getPeer()).menu); });
			}
		}

        public void addSeparator()
        {
			NetToolkit.Invoke(delegate { menu.MenuItems.Add(new MenuItem("-")); });
        }

        public void delItem(int i)
        {
			NetToolkit.Invoke(delegate { menu.MenuItems.RemoveAt(i); });
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
            NetToolkit.BeginInvoke(nativeDispose);
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


    internal class NetMouseInfoPeer : java.awt.peer.MouseInfoPeer
    {
        public int fillPointWithCoords(java.awt.Point p)
        {
            p.x = Cursor.Position.X;
            p.y = Cursor.Position.Y;
            //TODO multi screen device
            return 0; //return the number of the screen device
        }

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            internal POINT(Point pt)
            {
                this.X = pt.X;
                this.Y = pt.Y;
            }
        }

        [System.Security.SecuritySafeCritical]
        public bool isWindowUnderMouse(java.awt.Window window)
        {
            if (NetToolkit.isWin32())
            {
                NetWindowPeer peer = (NetWindowPeer)window.getPeer();
                if (peer != null)
                {
                    IntPtr hWnd = WindowFromPoint(new POINT(Cursor.Position));
                    return peer.control.Handle.Equals(hWnd);
                }
                return false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
    
    public class NetClipboard : java.awt.datatransfer.Clipboard
    {
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
            return new NetClipboardTransferable(NetToolkit.Invoke<IDataObject>(Clipboard.GetDataObject));
        }
    }

    public class NetClipboardTransferable : java.awt.datatransfer.Transferable
    {
        private readonly Map flavorToData = new HashMap();
        private readonly java.awt.datatransfer.DataFlavor[] flavors;
        public NetClipboardTransferable(IDataObject data)
        {
            flavorToData = NetDataTransferer.getInstanceImpl().translateFromClipboard(data);
            flavors = (java.awt.datatransfer.DataFlavor[])flavorToData.keySet().toArray(new java.awt.datatransfer.DataFlavor[0]);
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
        class NetToolkitThreadBlockedHandler : sun.awt.datatransfer.ToolkitThreadBlockedHandler
        {
            private bool locked;
            private Thread owner;
            
            protected bool isOwned()
            {
                return (locked && Thread.CurrentThread == owner);
            }

		    public void enter() 
            {
                if (!isOwned())
                {
                    throw new java.lang.IllegalMonitorStateException();
                }
                unlock();
				if (Application.MessageLoop)
				{
					Application.DoEvents();
				}
		        @lock();
		    }

		    public void exit() 
            {
                if (!isOwned())
                {
                    throw new java.lang.IllegalMonitorStateException();
                }
            }

            public void @lock() {
                lock(this)
                {
                    if (locked && Thread.CurrentThread == owner)
                    {
                        throw new java.lang.IllegalMonitorStateException();
                    }
                    do
                    {
                        if (!locked)
                        {
                            locked = true;
                            owner = Thread.CurrentThread;
                        }
                        else
                        {
                            try
                            {
                                Monitor.Wait(this);
                            }
                            catch (ThreadInterruptedException)
                            {
                                // try again
                            }
                        }
                    } while (owner != Thread.CurrentThread);
                }
            }

            public void unlock()
            {
                lock (this)
                {
                    if (Thread.CurrentThread != owner)
                    {
                        throw new java.lang.IllegalMonitorStateException();
                    }
                    owner = null;
                    locked = false;
                    Monitor.Pulse(this);
                }
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

        internal Map translateFromClipboard(IDataObject data)
        {
            java.awt.datatransfer.FlavorTable defaultFlavorMap = (java.awt.datatransfer.FlavorTable)java.awt.datatransfer.SystemFlavorMap.getDefaultFlavorMap();
            Map/*<DataFlavor,object>*/ map = new HashMap();
            if (data == null)
            {
                return map;
            }
            string[] formats = data.GetFormats();
            if (formats != null && formats.Length > 0)
            {
                long[] longFormats = getClipboardFormatCodes(formats);
                Map /*<DataFlavor,long>*/ flavorMap = getFlavorsForFormats(longFormats, defaultFlavorMap);
                java.awt.datatransfer.DataFlavor[] flavors =
                    (java.awt.datatransfer.DataFlavor[])
                    (flavorMap.keySet().toArray(new java.awt.datatransfer.DataFlavor[0]));
                for(int i=0; i<flavors.Length; i++)
                {
                    java.awt.datatransfer.DataFlavor df = flavors[i];
                    long format = ((java.lang.Long) flavorMap.get(df)).longValue();
                    string stringFormat = getNativeClipboardFormatName(format);
                    if (stringFormat==null) continue; // clipboard format is not registered in Windows system
                    object formatData = data.GetData(stringFormat);
                    if (formatData == null) continue; // no data for that format
                    object translatedData = null;
                    if (df.isFlavorJavaFileListType())
                    {
                        // translate string[] into java.util.List<java.io.File>
                        string[] nativeFileList = (string[])formatData;
                        List fileList = new ArrayList(nativeFileList.Length);
                        for (int j = 0; j < nativeFileList.Length; j++)
                        {
                            java.io.File file = new java.io.File(nativeFileList[i]);
                            fileList.add(file);
                        }
                        translatedData = fileList;
                    }
                    else if (java.awt.datatransfer.DataFlavor.imageFlavor.equals(df) && formatData is Bitmap)
                    {
                        // translate System.Drawing.Bitmap into java.awt.Image
                        translatedData = new java.awt.image.BufferedImage((Bitmap) formatData);
                    }
                    else if (formatData is string)
                    {
                        if (df.isFlavorTextType())
                            translatedData = formatData;
                        else if (((java.lang.Class)typeof(java.io.Reader)).equals(df.getRepresentationClass()))
                            translatedData = new java.io.StringReader((string) formatData);
                        else if (((java.lang.Class)typeof(java.io.InputStream)).equals(df.getRepresentationClass()))
                            translatedData = new java.io.StringBufferInputStream((string)formatData);
                        else
                            throw new java.awt.datatransfer.UnsupportedFlavorException(df);
                    }
                    if (translatedData!=null)
                        map.put(df, translatedData);
                }
            }
            return map;
        }

        internal IDataObject getDataObject(java.awt.datatransfer.Transferable transferable, java.awt.datatransfer.FlavorTable flavorMap)
        {
            DataObject obj = new DataObject();
            SortedMap/*<java.lang.Long,java.awt.datatransfer.DataFlavor>*/ formatMap = getFormatsForTransferable(transferable, flavorMap);
            for (Iterator iterator = formatMap.entrySet().iterator(); iterator.hasNext();)
            {
                Map.Entry entry = (Map.Entry) iterator.next();
                java.lang.Long lFormat = (java.lang.Long) entry.getKey();
                long format = lFormat == null ? -1 : lFormat.longValue();
                java.awt.datatransfer.DataFlavor flavor = (java.awt.datatransfer.DataFlavor) entry.getValue();
                object contents = transferable.getTransferData(flavor);
                if (contents==null) continue;
                try
                {
                    if (java.awt.datatransfer.DataFlavor.javaFileListFlavor.equals(flavor))
                    {
                        List list = (List)contents;
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
                        if (contents is string) 
                        {
                            obj.SetText((string) transferable.getTransferData(flavor));
                        }
                        else
                        {
                            try
                            {
                                java.io.Reader reader = flavor.getReaderForText(transferable);
                                java.io.StringWriter writer = new java.io.StringWriter();
                                char[] buffer = new char[1024];
                                int n;
                                while ((n = reader.read(buffer)) != -1)
                                {
                                    writer.write(buffer, 0, n);
                                }
                                obj.SetText(writer.toString());
                            }
                            catch
                            {
                            }
                        }
                    }
                    else if (java.awt.datatransfer.DataFlavor.imageFlavor.equals(flavor))
                    {
                        java.awt.Image image = contents as java.awt.Image;
                        if (image != null)
                        {
                            Image netImage = J2C.ConvertImage(image);
                            if (netImage != null)
                            {
                                obj.SetImage(netImage);
                            }
                        }
                    }
                    else if (flavor.isRepresentationClassCharBuffer())
                    {
                        if (!(isFlavorCharsetTextType(flavor) && isTextFormat(format)))
                        {
                            throw new IOException("cannot transfer non-text data as CharBuffer");
                        }
                        java.nio.CharBuffer buffer = (java.nio.CharBuffer)contents;
                        int size = buffer.remaining();
                        char[] chars = new char[size];
                        buffer.get(chars, 0, size);
                        obj.SetText(new string(chars));
                    }
                    else
                    {
                        // don't know what to do with it...
                        obj.SetData(transferable.getTransferData(flavor));
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

        protected internal override string getClipboardFormatName(long format)
        {
            return getNativeClipboardFormatName(format);
        }

        protected internal override byte[] imageToStandardBytes(java.awt.Image image, string mimeType)
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

        protected internal override java.io.ByteArrayOutputStream convertFileListToBytes(java.util.ArrayList fileList)
        {
            throw new ikvm.@internal.NotYetImplementedError();
        }

		protected internal override java.awt.Image platformImageBytesToImage(byte[] barr, long l)
		{
			throw new NotImplementedException();
		}
	}

}
