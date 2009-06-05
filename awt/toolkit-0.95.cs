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
using java.awt.datatransfer;
using java.awt.image;
using java.awt.peer;
using java.net;
using java.util;
using ikvm.awt.printing;
using ikvm.runtime;
using sun.awt;

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
	delegate void SetFont(java.awt.Font f);
    delegate void SetCursor(java.awt.Cursor cursor);
	delegate java.awt.Dimension GetDimension();
    delegate Rectangle ConvertRectangle(Rectangle r);
    delegate Point ConvertPoint(Point p);

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
        }

        internal void setFocusableWindow(bool value)
        {
            SetStyle(ControlStyles.Selectable, value);
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
        internal static java.awt.EventQueue eventQueue = new java.awt.EventQueue();
        internal static volatile Form bogusForm;
        private int resolution;

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
			ButtonPeer peer = new NetButtonPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
        {
			TextFieldPeer peer = new NetTextFieldPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
        {
			LabelPeer peer = new NetLabelPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ListPeer createList(java.awt.List target)
        {
			ListPeer peer = new NetListPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
        {
			CheckboxPeer peer = new NetCheckboxPeer(target);
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
			TextAreaPeer peer = new NetTextAreaPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
        {
			ChoicePeer peer = new NetChoicePeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
        {
			FramePeer peer = new NetFramePeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
        {
			WindowPeer peer = new NetWindowPeer(target);
			targetCreatedPeer(target, peer);
			return peer;
		}

        public override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
        {
			DialogPeer peer = new NetDialogPeer(target);
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

        public override ColorModel getColorModel()
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
                using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                {
                    return new BufferedImage(new Bitmap(Image.FromStream(stream)));
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
                return new BufferedImage(new Bitmap(Image.FromStream(mem)));
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
                return new BufferedImage(new Bitmap(new MemoryStream(imagedata, imageoffset, imagelength, false)));
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
#if !COMPACT_FRAMEWORK
            Console.Beep();
#endif
        }

        public override java.awt.datatransfer.Clipboard getSystemClipboard()
        {
            throw new NotImplementedException();
        }

        protected override java.awt.EventQueue getSystemEventQueueImpl()
        {
            return eventQueue;
        }

        public override java.awt.dnd.peer.DragSourceContextPeer createDragSourceContextPeer(java.awt.dnd.DragGestureEvent dge)
        {
            throw new NotImplementedException();
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
        protected override DesktopPeer createDesktopPeer(java.awt.Desktop target)
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
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
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

        public override RobotPeer createRobot(java.awt.Robot r, java.awt.GraphicsDevice screen)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows)
            {
                return new WindowsRobot(screen);
            }
            throw new java.awt.AWTException("Robot not supported for this OS");
        }

        public override SystemTrayPeer createSystemTray(java.awt.SystemTray st)
        {
            throw new NotImplementedException();
        }

        public override TrayIconPeer createTrayIcon(java.awt.TrayIcon ti)
        {
            throw new NotImplementedException();
        }

        public override java.awt.im.spi.InputMethodDescriptor getInputMethodAdapterDescriptor()
        {
            // Input Method needs .NET 3.0 or higher.
            // package System.Windows.Input requiered
            //return new NetInputMethodDescriptor();
            // we don't have to provide a native input method adapter
            return null;
        }

        protected override int getScreenHeight()
        {
            throw new NotImplementedException();
        }

        protected override int getScreenWidth()
        {
            throw new NotImplementedException();
        }

        public override void grab(java.awt.Window w)
        {
            throw new NotImplementedException();
        }

        public override bool isDesktopSupported()
        {
            throw new NotImplementedException();
        }

        public override bool isTraySupported()
        {
            throw new NotImplementedException();
        }

        protected override bool syncNativeQueue(long l)
        {
            throw new NotImplementedException();
        }

        public override void ungrab(java.awt.Window w)
        {
            throw new NotImplementedException();
        }

		internal static new object targetToPeer(object target)
		{
			return SunToolkit.targetToPeer(target);
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
		private static readonly AwtToolkit theInstance = new AwtToolkit();

		internal static AwtToolkit GetInstance() { return theInstance; }

		internal void SyncCall(ThreadStart del)
		{
			// TODO if we're not on the right thread we should lock (see awt_Toolkit.cpp)
			del();
		}

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

	abstract class NetComponentPeer : ComponentPeer
	{
		internal readonly java.awt.Component target;
		internal Control control;
        private bool isMouseClick;
        private bool isDoubleClick;
        private bool isPopupMenu;
		private int oldWidth = -1;
		private int oldHeight = -1;
		private bool sm_suppressFocusAndActivation;
		private bool m_callbacksEnabled;
        private static readonly java.lang.reflect.Field compX;
        private static readonly java.lang.reflect.Field compY;
        private static readonly java.lang.reflect.Field compWidth;
        private static readonly java.lang.reflect.Field compHeight;

		static NetComponentPeer()
        {
			java.lang.reflect.Field _compX = null;
			java.lang.reflect.Field _compY = null;
			java.lang.reflect.Field _compWidth = null;
			java.lang.reflect.Field _compHeight = null;
			java.security.AccessController.doPrivileged(Delegates.toPrivilegedAction(delegate
			{
				java.lang.Class clazz = typeof(java.awt.Component);
				_compX = clazz.getDeclaredField("x");
				_compX.setAccessible(true);
				_compY = clazz.getDeclaredField("y");
				_compY.setAccessible(true);
				_compWidth = clazz.getDeclaredField("width");
				_compWidth.setAccessible(true);
				_compHeight = clazz.getDeclaredField("height");
				_compHeight.setAccessible(true);
				return null;
			}));
			compX = _compX;
			compY = _compY;
			compWidth = _compWidth;
			compHeight = _compHeight;
        }

		public NetComponentPeer(java.awt.Component target)
		{
			this.target = target;
			//this.paintArea = new RepaintArea();
			java.awt.Container parent = SunToolkit.getNativeContainer(target);
			NetComponentPeer parentPeer = (NetComponentPeer)NetToolkit.targetToPeer(parent);
			create(parentPeer);
			/*
			// fix for 5088782: check if window object is created successfully
			checkCreation();
			this.winGraphicsConfig =
				(Win32GraphicsConfig)getGraphicsConfiguration();
			this.surfaceData =
				winGraphicsConfig.createSurfaceData(this, numBackBuffers);
			 */
			initialize();
			start();  // Initialize enable/disable state, turn on callbacks
		}

		void initialize()
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
			setBounds(r.x, r.y, r.width, r.height, ComponentPeer.__Fields.SET_BOUNDS);
		}

		void start()
		{
			lock (this)
			{
				AwtToolkit.GetInstance().SyncCall(_Start);
			}
		}

		void _Start()
		{
			if (control.IsHandleCreated)
			{
				initEvents();
				// JDK native code also disables the window here, but since that is already done in initialize(),
				// I don't see the point
				EnableCallbacks(true);
				control.Invalidate();
				control.Update();
			}
		}

		void EnableCallbacks(bool enabled)
		{
			m_callbacksEnabled = enabled;
		}

		internal abstract void create(NetComponentPeer parent);

		internal void Invoke(ThreadStart del)
		{
			control.Invoke(del);
		}

		void pShow()
		{
			lock (this)
			{
				AwtToolkit.GetInstance().SyncCall(_Show);
			}
		}

		void _Show()
		{
			if (control.IsHandleCreated)
			{
				Invoke(delegate { control.Visible = true; });
			}
		}

		void _Hide()
		{
			if (control.IsHandleCreated)
			{
				Invoke(delegate { control.Visible = false; });
			}
		}

		void _Enable()
		{
			if (control.IsHandleCreated)
			{
				Enable(true);
			}
		}

		void _Disable()
		{
			if (control.IsHandleCreated)
			{
				Enable(false);
			}
		}

		void Enable(bool enable)
		{
			sm_suppressFocusAndActivation = true;
			control.Enabled = enable;
			sm_suppressFocusAndActivation = false;
			//CriticalSection::Lock l(GetLock());
			//VerifyState();
		}

		internal virtual void initEvents()
		{
			// TODO we really only should hook these events when they are needed...
			control.KeyDown += new KeyEventHandler(OnKeyDown);
			control.KeyUp += new KeyEventHandler(OnKeyUp);
			control.KeyPress += new KeyPressEventHandler(OnKeyPress);
			control.MouseMove += new MouseEventHandler(OnMouseMove);
			control.MouseDown += new MouseEventHandler(OnMouseDown);
			control.Click += new EventHandler(OnClick);
			control.DoubleClick += new EventHandler(OnDoubleClick);
			control.MouseUp += new MouseEventHandler(OnMouseUp);
			control.MouseEnter += new EventHandler(OnMouseEnter);
			control.MouseLeave += new EventHandler(OnMouseLeave);
			control.GotFocus += new EventHandler(OnGotFocus);
			control.LostFocus += new EventHandler(OnLostFocus);
			control.SizeChanged += new EventHandler(OnBoundsChanged);
			control.Leave += new EventHandler(OnBoundsChanged);
			control.Paint += new PaintEventHandler(OnPaint);
			control.ContextMenu = new ContextMenu();
			control.ContextMenu.Popup += new EventHandler(OnPopupMenu);
		}

        /// <summary>
        /// This method is called from the same thread that call Commponent.addNotify().
        /// The constructor is called from the global event thread with form.   Invoke()
        /// Because addNotfy is synchronized with getTreeLock() and some classes are 
        /// also synchronized with it self in the GNU classpath there can be dead locks.
        /// You can use this method to modify the Component class thread safe.
        /// </summary>
        internal virtual void init()
        {
            // TODO temporaly disabled, because a Bug in classpath (Bug 30122)
            // http://gcc.gnu.org/bugzilla/show_bug.cgi?id=30122
            if(target.isFontSet())
            {
                //setFontImpl(component.getFont());
                setFont(target.getFont());
            }
        }

        protected virtual int getInsetsLeft()
        {
            return 0;
        }

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
                    ComponentPeer peer = parent.getPeer();
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
			if(!e.ClipRectangle.IsEmpty)
			{
                int x = getInsetsLeft();
                int y = getInsetsTop();
				java.awt.Rectangle rect = new java.awt.Rectangle(e.ClipRectangle.X + x, e.ClipRectangle.Y + y, e.ClipRectangle.Width, e.ClipRectangle.Height);
				postEvent(new java.awt.@event.PaintEvent(target, java.awt.@event.PaintEvent.UPDATE, rect));
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

        private static int GetMouseEventModifiers(MouseEventArgs ev)
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

        private static int GetModifiers(Keys keys)
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

		private static int GetButton(MouseEventArgs e)
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

		private void OnMouseDown(object sender, MouseEventArgs ev)
		{
			isMouseClick = false;
			isDoubleClick = false;
			isPopupMenu = false;
			postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_PRESSED, ev.Clicks);
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

        /// <summary>
        /// Set the size of the component to the size of the peer if different.
        /// </summary>
        private void componentSetBounds()
        {
            Point offset = getParentOffset();
            int x = control.Left + offset.X;
            int y = control.Top + offset.Y;
            int width = control.Width;
            int height = control.Height;
            //we set it via Reflection because Sun do it also
            //a call of setBounds produce another behaviuor
            compX.setInt(this.target, x);
            compY.setInt(this.target, y);
            compWidth.setInt(this.target, width);
            compHeight.setInt(this.target, height);
        }

        private void OnBoundsChanged(object sender, EventArgs e)
		{
			java.awt.EventQueue.invokeLater(Delegates.toRunnable(delegate {
                componentSetBounds();
				postEvent(new java.awt.@event.ComponentEvent(target, java.awt.@event.ComponentEvent.COMPONENT_RESIZED));
			}));
		}

        private void OnPopupMenu(object sender, EventArgs ev)
        {
            isPopupMenu = true;
        }

		protected void postEvent(java.awt.AWTEvent evt)
		{
			NetToolkit.eventQueue.postEvent(evt);
		}

		public int checkImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			return getToolkit().checkImage(img, width, height, ob);
		}

		public java.awt.Image createImage(java.awt.image.ImageProducer prod)
		{
            NetProducerImage npi = new NetProducerImage(prod);
            prod.startProduction(npi);
            return new BufferedImage(npi.getBitmap());
		}

		public java.awt.Image createImage(int width, int height)
		{
			return new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
		}

		public void disable()
		{
			lock (this)
			{
				AwtToolkit.GetInstance().SyncCall(_Disable);
			}
		}

		public void dispose()
		{
			control.Invoke(new SetVoid(disposeImpl));
		}

		private void disposeImpl()
		{
			// HACK we should dispose the control here, but that hangs in an infinite loop...
			control.Hide();
		}

		public void enable()
		{
			lock (this)
			{
				AwtToolkit.GetInstance().SyncCall(_Enable);
			}
		}

		public ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}

		public java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			return new NetFontMetrics(f);
		}

		public virtual java.awt.Graphics getGraphics()
		{
            return new ComponentGraphics(this);
		}

		public java.awt.Point getLocationOnScreen()
		{
			Point p = new Point(0, 0);
            p = control.InvokeRequired ?
                    (Point)control.Invoke(new ConvertPoint(control.PointToScreen), new object[] { p }) :
                    control.PointToScreen(p);
			return new java.awt.Point(p.X, p.Y);
		}

		public java.awt.Dimension getMinimumSize()
		{
			return minimumSize();
		}

		public java.awt.Dimension getPreferredSize()
		{
			return preferredSize();
		}

		public java.awt.Toolkit getToolkit()
		{
			return java.awt.Toolkit.getDefaultToolkit();
		}

		public void handleEvent(java.awt.AWTEvent e)
		{
            if (e is java.awt.@event.PaintEvent)
            {
                java.awt.Graphics g = target.getGraphics();
                try
                {
                    java.awt.Rectangle r = ((java.awt.@event.PaintEvent)e).getUpdateRect();
                    g.clipRect(r.x, r.y, r.width, r.height);
                    switch (e.getID())
                    {
                        case java.awt.@event.PaintEvent.UPDATE:
                            target.update(g);
                            break;
                        case java.awt.@event.PaintEvent.PAINT:
                            target.paint(g);
                            break;
                        default:
                            Console.WriteLine("Unknown PaintEvent: {0}", e.getID());
                            break;
                    }
                }
                finally
                {
                    g.dispose();
                }
            }
		}

        public void hide()
		{
			lock (this)
			{
				AwtToolkit.GetInstance().SyncCall(_Hide);
			}
		}

		public bool isFocusTraversable()
		{
			return true;
		}

		public virtual java.awt.Dimension minimumSize()
		{
			return target.getSize();
		}

		public virtual java.awt.Dimension preferredSize()
		{
			return minimumSize();
		}

		public void paint(java.awt.Graphics graphics)
		{
			//throw new NotImplementedException();
		}

		public bool prepareImage(java.awt.Image img, int width, int height, ImageObserver ob)
		{
			return getToolkit().prepareImage(img, width, height, ob);
		}

		public void print(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
		}

		public void repaint(long tm, int x, int y, int width, int height)
		{
			// TODO do something with the tm parameter
			java.awt.Rectangle rect = new java.awt.Rectangle(x, y, width, height);
			postEvent(new java.awt.@event.PaintEvent(target, java.awt.@event.PaintEvent.UPDATE, rect));
		}

		public void requestFocus()
		{
			control.Invoke(new SetVoid(requestFocusImpl), null);
		}

		private void requestFocusImpl()
		{
			control.Focus();
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
            if (!control.Enabled || !control.Visible)
            {
                return false;
            }
            postEvent(new java.awt.@event.FocusEvent(request, java.awt.@event.FocusEvent.FOCUS_GAINED, temporary, target));
			return true;
		}

		public void reshape(int x, int y, int width, int height)
		{
            setBounds(x, y, width, height, java.awt.peer.ComponentPeer.__Fields.DEFAULT_OPERATION);
		}

		public void setBackground(java.awt.Color color)
		{
			control.Invoke(new SetColor(SetBackColorImpl), new object[] { color });
		}

		private void SetBackColorImpl(java.awt.Color color)
		{
            control.BackColor = Color.FromArgb(color.getRGB());
		}

        protected virtual void SetBoundsImpl(int x, int y, int width, int height)
		{
            Point offset = getParentOffset(); 
            control.SetBounds(x - offset.X, y - offset.Y, width, height);
        }

		public void setBounds(int x, int y, int width, int height, int operation)
		{
			control.Invoke(new SetXYWH(SetBoundsImpl), new object[] { x, y, width, height });
            componentSetBounds();
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

		private void setEnabledImpl(bool enabled)
		{
			control.Enabled = enabled;
		}

		public void setEnabled(bool enabled)
		{
			control.Invoke(new SetBool(setEnabledImpl), new object[] { enabled });
		}

        private void setFontImpl(java.awt.Font font)
		{
            control.Font = font.getNetFont();
		}

		public void setFont(java.awt.Font font)
		{
			control.Invoke(new SetFont(setFontImpl), new object[] { font });
		}

		public void setForeground(java.awt.Color color)
		{
			control.Invoke(new SetColor(SetForeColorImpl), new object[] { color });
		}

		private void SetForeColorImpl(java.awt.Color color)
		{
            control.ForeColor = Color.FromArgb(color.getRGB());
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

		public void show()
		{
			java.awt.Dimension s = ((java.awt.Component)target).getSize();
			oldHeight = s.height;
			oldWidth = s.width;
			pShow();
		}

		public java.awt.GraphicsConfiguration getGraphicsConfiguration()
		{
			return new NetGraphicsConfiguration(Screen.FromControl(control));
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
		}

		public void updateCursorImmediately()
		{
		}

		public java.awt.image.VolatileImage createVolatileImage(int width, int height)
		{
			return new NetVolatileImage(width, height);
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

		public bool isFocusable()
		{
			// TODO
			return true;
		}

		public java.awt.Rectangle getBounds()
		{
			Rectangle r = control.Bounds;
			return new java.awt.Rectangle(r.X, r.Y, r.Width, r.Height);
		}

		public void reparent(java.awt.peer.ContainerPeer parent)
		{
			throw new NotImplementedException();
		}

		public bool isReparentSupported()
		{
			return false;
		}

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
                temporary,
                focusedWindowChangeAllowed,
                time);
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
                temporary,
                focusedWindowChangeAllowed,
                time,
                cause)).intValue();
            if (retval == SNFH_SUCCESS_HANDLED)
            {
                return true;
            }
            else if (retval == SNFH_SUCCESS_PROCEED)
            {
                if (control.Focused)
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
    }

	class NetButtonPeer : NetComponentPeer, ButtonPeer
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

	class NetTextComponentPeer : NetComponentPeer, TextComponentPeer
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

	class NetChoicePeer : NetComponentPeer, ChoicePeer
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

	class NetCheckboxPeer : NetComponentPeer, CheckboxPeer
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

	class NetLabelPeer : NetComponentPeer, LabelPeer
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

	class NetTextFieldPeer : NetTextComponentPeer, TextFieldPeer
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

		public void setEchoCharacter(char echo_char)
		{
			// TODO use control.Invoke
			((TextBox)control).PasswordChar = echo_char;
		}
	}

	class NetTextAreaPeer : NetTextComponentPeer, TextAreaPeer
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

	class NetContainerPeer : NetComponentPeer, ContainerPeer
	{
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

		public void beginValidate()
		{
		}

		public void endValidate()
		{
		}

		public void beginLayout()
		{
		}

		public void endLayout()
		{
		}

		public bool isPaintPending()
		{
			throw new NotImplementedException();
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

	class NetPanelPeer : NetContainerPeer, PanelPeer
	{
		public NetPanelPeer(java.awt.Panel panel)
			: base(panel)
		{
		}
	}

	class NewCanvasPeer : NetComponentPeer, CanvasPeer
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

	class NetWindowPeer : NetContainerPeer, WindowPeer
	{
        public NetWindowPeer(java.awt.Window window)
			: base(window)
		{
            //form.Shown += new EventHandler(OnOpened); Will already post in java.awt.Window.show()
            ((Form)control).Closing += new CancelEventHandler(OnClosing);
			((Form)control).Closed += new EventHandler(OnClosed);
			((Form)control).Activated += new EventHandler(OnActivated);
			((Form)control).Deactivate += new EventHandler(OnDeactivate);
            //Calculate the Insets one time
            //This is many faster because there no thread change is needed.
            Rectangle client = control.ClientRectangle;
            Rectangle r = control.RectangleToScreen( client );
            int x = r.Location.X - control.Location.X;
            int y = r.Location.Y - control.Location.Y;
            _insets = new java.awt.Insets(y, x, control.Height - client.Height - y, control.Width - client.Width - x);
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

        private void OnActivated(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)target, java.awt.@event.WindowEvent.WINDOW_ACTIVATED));
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)target, java.awt.@event.WindowEvent.WINDOW_DEACTIVATED));
        }

        public override java.awt.Graphics getGraphics()
        {
            java.awt.Graphics g = base.getGraphics();
            java.awt.Insets insets = getInsets();
            g.translate(-insets.left, -insets.top);
            g.setClip(insets.left, insets.top, control.ClientRectangle.Width, control.ClientRectangle.Height);
            return g;
        }

        protected override void SetBoundsImpl(int x, int y, int width, int height)
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

        public void setModalBlocked(java.awt.Dialog d, bool b)
        {
            throw new NotImplementedException();
        }

        public void updateFocusableWindowState()
        {
            ((UndecoratedForm)control).setFocusableWindow( ((java.awt.Window)target).isFocusableWindow());
        }

        public void updateIconImages()
        {
            throw new NotImplementedException();
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
			if (parent != null)
			{
				form.Owner = parent.control.FindForm();
			}
			NetToolkit.CreateNative(form);
			this.control = form;
		}
	}

	class NetFramePeer : NetWindowPeer, FramePeer
	{
		public NetFramePeer(java.awt.Frame frame)
			: base(frame)
		{
			setTitle(frame.getTitle());
			setResizable(frame.isResizable());
            setIconImage(frame.getIconImage());
        }

        internal override void init()
        {
            if (!target.isFontSet())
            {
                java.awt.Font font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
                target.setFont(font);
            }
            if (!target.isForegroundSet())
            {
                target.setForeground(java.awt.SystemColor.windowText);
            }
            if (!target.isBackgroundSet())
            {
                target.setBackground(java.awt.SystemColor.window);
            }
            base.init();
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

		public void setIconImage(java.awt.Image image)
		{
			if (image is BufferedImage)
			{
				Bitmap bitmap = ((BufferedImage)image).getBitmap();
				((Form)control).Icon = Icon.FromHandle(bitmap.GetHicon());
			}
		}

		public void setMenuBar(java.awt.MenuBar mb)
		{
			throw new NotImplementedException();
		}

		public void setResizable(bool resizable)
		{
			if (resizable)
			{
				((Form)control).FormBorderStyle = FormBorderStyle.Sizable;
			}
			else
			{
				((Form)control).FormBorderStyle = FormBorderStyle.Fixed3D;
			}
		}

		private void setTitleImpl(string title)
		{
			control.Text = title;
		}

		public void setTitle(string title)
		{
			control.Invoke(new SetString(setTitleImpl), new object[] { title });
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
			throw new NotImplementedException();
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
			if (parent != null)
			{
				form.Owner = parent.control.FindForm();
			}
			NetToolkit.CreateNative(form);
			this.control = form;
		}
    }

	class NetDialogPeer : NetWindowPeer, DialogPeer
	{
        public NetDialogPeer(java.awt.Dialog target)
			: base(target)
		{
            ((Form)control).MaximizeBox = false;
			((Form)control).MinimizeBox = false;
            control.Text = target.getTitle();
		}

		private void setTitleImpl(string title)
		{
			control.Text = title;
		}

		public void setTitle(string title)
		{
			control.Invoke(new SetString(setTitleImpl), new object[] { title });
		}

		public void setResizable(bool resizable)
		{
			throw new NotImplementedException();
		}

        public void blockWindows(List toBlock)
        {
            // code copies from sun.awt.windows.WDialogPeer.java
            for (Iterator it = toBlock.iterator(); it.hasNext();) {
                java.awt.Window w = (java.awt.Window)it.next();
                WindowPeer wp = (WindowPeer)ComponentAccessor.getPeer(w);
                if (wp != null) {
                    wp.setModalBlocked((java.awt.Dialog)target, true);
                }
            }
        }
    }

	class NetListPeer : NetComponentPeer, ListPeer
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

	class NetDesktopPeer : DesktopPeer
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

	class NetFileDialogPeer : java.awt.peer.FileDialogPeer
	{
		private readonly java.awt.FileDialog dialog;

		internal NetFileDialogPeer(java.awt.FileDialog dialog)
		{
			this.dialog = dialog;
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
			throw new NotImplementedException();
		}

		public bool requestWindowFocus()
		{
			throw new NotImplementedException();
		}

		public void toBack()
		{
			throw new NotImplementedException();
		}

		public void toFront()
		{
		}

		public void updateAlwaysOnTop()
		{
			throw new NotImplementedException();
		}

		public void beginLayout()
		{
		}

		public void beginValidate()
		{
		}

		public void cancelPendingPaint(int i1, int i2, int i3, int i4)
		{
		}

		public void endLayout()
		{
		}

		public void endValidate()
		{
		}

		public java.awt.Insets getInsets()
		{
			return new java.awt.Insets(0, 0, 0, 0);
		}

		public java.awt.Insets insets()
		{
			return getInsets();
		}

		public bool isPaintPending()
		{
			return false;
		}

		public bool isRestackSupported()
		{
			return false;
		}

		public void restack()
		{
		}

		public bool canDetermineObscurity()
		{
			return false;
		}

		public int checkImage(java.awt.Image i1, int i2, int i3, ImageObserver io)
		{
			throw new NotImplementedException();
		}

		public void coalescePaintEvent(java.awt.@event.PaintEvent pe)
		{
			throw new NotImplementedException();
		}

		public void createBuffers(int i, java.awt.BufferCapabilities bc)
		{
			throw new NotImplementedException();
		}

		public java.awt.Image createImage(int i1, int i2)
		{
			throw new NotImplementedException();
		}

		public java.awt.Image createImage(ImageProducer ip)
		{
			throw new NotImplementedException();
		}

		public VolatileImage createVolatileImage(int i1, int i2)
		{
			throw new NotImplementedException();
		}

		public void destroyBuffers()
		{
			throw new NotImplementedException();
		}

		public void disable()
		{
			throw new NotImplementedException();
		}

		public void dispose()
		{
		}

		public void enable()
		{
			throw new NotImplementedException();
		}

		public void flip(java.awt.BufferCapabilities.FlipContents bcfc)
		{
			throw new NotImplementedException();
		}

		public java.awt.Image getBackBuffer()
		{
			throw new NotImplementedException();
		}

		public java.awt.Rectangle getBounds()
		{
			throw new NotImplementedException();
		}

		public ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}

		public java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			throw new NotImplementedException();
		}

		public java.awt.Graphics getGraphics()
		{
			throw new NotImplementedException();
		}

		public java.awt.GraphicsConfiguration getGraphicsConfiguration()
		{
			throw new NotImplementedException();
		}

		public java.awt.Point getLocationOnScreen()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getMinimumSize()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getPreferredSize()
		{
			throw new NotImplementedException();
		}

		public java.awt.Toolkit getToolkit()
		{
			throw new NotImplementedException();
		}

		public void handleEvent(java.awt.AWTEvent awte)
		{
			throw new NotImplementedException();
		}

		public bool handlesWheelScrolling()
		{
			throw new NotImplementedException();
		}

		public void hide()
		{
		}

		public bool isFocusTraversable()
		{
			throw new NotImplementedException();
		}

		public bool isFocusable()
		{
			throw new NotImplementedException();
		}

		public bool isObscured()
		{
			throw new NotImplementedException();
		}

		public bool isReparentSupported()
		{
			throw new NotImplementedException();
		}

		public void layout()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension minimumSize()
		{
			throw new NotImplementedException();
		}

		public void paint(java.awt.Graphics g)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension preferredSize()
		{
			throw new NotImplementedException();
		}

		public bool prepareImage(java.awt.Image i1, int i2, int i3, ImageObserver io)
		{
			throw new NotImplementedException();
		}

		public void print(java.awt.Graphics g)
		{
			throw new NotImplementedException();
		}

		public void repaint(long l, int i1, int i2, int i3, int i4)
		{
			throw new NotImplementedException();
		}

		public void reparent(ContainerPeer cp)
		{
			throw new NotImplementedException();
		}

		public void requestFocus()
		{
			throw new NotImplementedException();
		}

		public bool requestFocus(java.awt.Component c, bool b1, bool b2, long l)
		{
			throw new NotImplementedException();
		}

		public void reshape(int i1, int i2, int i3, int i4)
		{
			throw new NotImplementedException();
		}

		public void setBackground(java.awt.Color c)
		{
			throw new NotImplementedException();
		}

		public void setBounds(int i1, int i2, int i3, int i4, int i5)
		{
			throw new NotImplementedException();
		}

		public void setBounds(int i1, int i2, int i3, int i4)
		{
			throw new NotImplementedException();
		}

		public void setCursor(java.awt.Cursor c)
		{
			throw new NotImplementedException();
		}

		public void setEnabled(bool b)
		{
			throw new NotImplementedException();
		}

		public void setEventMask(long l)
		{
		}

		public void setFont(java.awt.Font f)
		{
			throw new NotImplementedException();
		}

		public void setForeground(java.awt.Color c)
		{
			throw new NotImplementedException();
		}

		public void setVisible(bool b)
		{
			throw new NotImplementedException();
		}

		public void show()
		{
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

		public void updateCursorImmediately()
		{
			throw new NotImplementedException();
		}

        public void applyShape(sun.java2d.pipe.Region r)
        {
            throw new NotImplementedException();
        }

        public bool requestFocus(java.awt.Component c, bool b1, bool b2, long l, sun.awt.CausedFocusEvent.Cause cfec)
        {
            throw new NotImplementedException();
        }

        public void setAlwaysOnTop(bool b)
        {
            throw new NotImplementedException();
        }

        public void setModalBlocked(java.awt.Dialog d, bool b)
        {
            throw new NotImplementedException();
        }

        public void updateFocusableWindowState()
        {
            throw new NotImplementedException();
        }

        public void updateIconImages()
        {
            throw new NotImplementedException();
        }

        public void updateMinimumSize()
        {
            throw new NotImplementedException();
        }

        public void blockWindows(List l)
        {
            throw new NotImplementedException();
        }
    }
}
