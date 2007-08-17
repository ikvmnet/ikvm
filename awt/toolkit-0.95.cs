/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.
  Copyright (C) 2006, 2007 Volker Berlin

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
			this.FormBorderStyle = FormBorderStyle.None;
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}
	}

	class MyForm : Form
	{
		public MyForm()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
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

    public class NetToolkit : gnu.java.awt.ClasspathToolkit
	{
        internal static java.awt.EventQueue eventQueue = new java.awt.EventQueue();
		internal static volatile Form bogusForm;
		private static Delegate createControlInstance;
		private int resolution;

        private delegate NetComponentPeer CreateControlInstanceDelegate(Type controlType, java.awt.Component target, Type peerType);

		private static void MessageLoop()
		{
			createControlInstance = new CreateControlInstanceDelegate(CreateControlImpl);
			using(Form form = new Form())
			{
				form.CreateControl();
				// HACK I have no idea why this line is necessary...
				IntPtr p = form.Handle;
				if(p == IntPtr.Zero)
				{
					// shut up compiler warning
				}
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

        internal static NetComponentPeer CreateControlImpl(Type controlType, java.awt.Component target, Type peerType)
		{
            Control control = (Control)Activator.CreateInstance(controlType);
            control.CreateControl();
            // HACK here we go again...
			IntPtr p = control.Handle;
			if(p == IntPtr.Zero)
			{
				// shut up compiler warning
			}
            NetComponentPeer peer = (NetComponentPeer)Activator.CreateInstance(peerType, new object[] { target, control });
            peer.initEvents();
            return peer;
        }

        internal static NetComponentPeer CreatePeer(Type controlType, java.awt.Component target, Type peerType)
		{
            java.awt.Container parent = target.getParent();
            if (parent != null && parent.getPeer() == null)
            {
                //This should do in Java, but it is a Bug in GNU classpath
                //because synchronized in Java this must be call with the caller thread
                parent.addNotify();
            }
            NetComponentPeer peer = (NetComponentPeer)bogusForm.Invoke(createControlInstance, new object[] { controlType, target, peerType });
            peer.init();
            return peer;
		}

		public NetToolkit()
		{
			lock(typeof(NetToolkit))
			{
				System.Diagnostics.Debug.Assert(bogusForm == null);

				Thread thread = new Thread(new ThreadStart(MessageLoop));
				thread.ApartmentState = ApartmentState.STA;
				thread.Name = "IKVM AWT WinForms Message Loop";
				thread.IsBackground = true;
				thread.Start();
				// TODO don't use polling...
				while(bogusForm == null && thread.IsAlive)
				{
					Thread.Sleep(1);
				}
			}
		}

		protected override void loadSystemColors(int[] systemColors)
		{
			// initialize all colors to purple to make the ones we might have missed stand out
			for(int i = 0; i < systemColors.Length; i++)
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

		protected override java.awt.peer.ButtonPeer createButton(java.awt.Button target)
		{
            return (NetButtonPeer)CreatePeer(typeof(Button), target, typeof(NetButtonPeer));
		}

		protected override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
		{
            return (NetTextFieldPeer)CreatePeer(typeof(TextBox), target, typeof(NetTextFieldPeer));
        }

		protected override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
		{
            return (NetLabelPeer)CreatePeer(typeof(Label), target, typeof(NetLabelPeer));
		}

		protected override java.awt.peer.ListPeer createList(java.awt.List target)
		{
            return (NetListPeer)CreatePeer(typeof(ListBox), target, typeof(NetListPeer));
		}

		protected override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
		{
            return (NetCheckboxPeer)CreatePeer(typeof(CheckBox), target, typeof(NetCheckboxPeer));
		}

		protected override java.awt.peer.ScrollbarPeer createScrollbar(java.awt.Scrollbar target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.ScrollPanePeer createScrollPane(java.awt.ScrollPane target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.TextAreaPeer createTextArea(java.awt.TextArea target)
		{
            return (NetTextAreaPeer)CreatePeer(typeof(TextBox), target, typeof(NetTextAreaPeer));
		}

		protected override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
		{
            return (NetChoicePeer)CreatePeer(typeof(ComboBox), target, typeof(NetChoicePeer));
		}

		protected override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
		{
            if (!target.isFontSet())
            {
                java.awt.Font font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
                target.setFont(font);
            }
            return (NetFramePeer)CreatePeer(typeof(MyForm), target, typeof(NetFramePeer));
		}

		protected override java.awt.peer.CanvasPeer createCanvas(java.awt.Canvas target)
		{
            return (NewCanvasPeer)CreatePeer(typeof(MyControl), target, typeof(NewCanvasPeer));
		}

		protected override java.awt.peer.PanelPeer createPanel(java.awt.Panel target)
		{
            return (NetPanelPeer)CreatePeer(typeof(ContainerControl), target, typeof(NetPanelPeer));
		}

		protected override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
		{
            return (NetWindowPeer)CreatePeer(typeof(UndecoratedForm), target, typeof(NetWindowPeer));
		}

		protected override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
		{
            return (NetDialogPeer)CreatePeer(typeof(MyForm), target, typeof(NetDialogPeer));
		}

		protected override java.awt.peer.MenuBarPeer createMenuBar(java.awt.MenuBar target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.MenuPeer createMenu(java.awt.Menu target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.PopupMenuPeer createPopupMenu(java.awt.PopupMenu target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.MenuItemPeer createMenuItem(java.awt.MenuItem target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.FileDialogPeer createFileDialog(java.awt.FileDialog target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.CheckboxMenuItemPeer createCheckboxMenuItem(java.awt.CheckboxMenuItem target)
		{
			throw new NotImplementedException();
		}

		[Obsolete]
		protected override java.awt.peer.FontPeer getFontPeer(string name, int style)
		{
			throw new NotImplementedException();
		}

		public override java.awt.Dimension getScreenSize()
		{
			return new java.awt.Dimension(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
		}

		public override int getScreenResolution()
		{
			if(resolution == 0)
			{
				using(Graphics g = bogusForm.CreateGraphics())
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
				using(System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
				{
					return new NetBufferedImage(new Bitmap(Image.FromStream(stream)));
				}
			}
			catch(Exception ex)
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
			while((b = inS.read()) >= 0)
			{
				mem.WriteByte((byte)b);
			}
			try
			{
				mem.Position = 0;
				return new NetBufferedImage(new Bitmap(Image.FromStream(mem)));
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
			if(image.getWidth(null) == -1)
			{
				if(observer != null)
				{
					observer.imageUpdate(image, ERROR | ABORT, 0, 0, -1, -1);
				}
				return ERROR | ABORT;
			}
			if(observer != null)
			{
				observer.imageUpdate(image, WIDTH + HEIGHT + FRAMEBITS + ALLBITS, 0, 0, image.getWidth(null), image.getHeight(null));
			}
			return WIDTH + HEIGHT + FRAMEBITS + ALLBITS;
		}

		public override java.awt.Image createImage(java.awt.image.ImageProducer producer)
		{
			NetProducerImage img = new NetProducerImage(producer);
			if(producer != null)
			{
				producer.startProduction(img);
			}
			return img;
		}

		public override java.awt.Image createImage(byte[] imagedata, int imageoffset, int imagelength)
		{
            try
            {
                return new NetBufferedImage(new Bitmap(new MemoryStream(imagedata, imageoffset, imagelength, false)));
            }
            catch (Exception ex)
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
#if WHIDBEY && !COMPACT_FRAMEWORK
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

		public override java.awt.Font createFont(int format, java.io.InputStream stream)
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

		protected override DesktopPeer createDesktopPeer(java.awt.Desktop target)
		{
			return new NetDesktopPeer();
		}
	}

	class NetLightweightComponentPeer : NetComponentPeer, java.awt.peer.LightweightPeer
	{
		public NetLightweightComponentPeer(java.awt.Component target)
			: base(target, ((NetComponentPeer)target.getParent().getPeer()).control)
		{
		}
	}

	class NetLightweightContainerPeer : NetContainerPeer, java.awt.peer.LightweightPeer
	{
		public NetLightweightContainerPeer(java.awt.Container target)
			: base(target, GetContainerControl(target.getParent()))
		{
		}

		private static ContainerControl GetContainerControl(java.awt.Container aContainer)
		{
			ContainerControl control = null;

			if (aContainer != null)
			{
				control = (ContainerControl) ((NetContainerPeer) aContainer.getPeer()).control;
			}

			return control;
		}
	}

	class NetComponentPeer : ComponentPeer
	{
		internal readonly java.awt.Component component;
		internal readonly Control control;
        private bool isMouseClick;

		public NetComponentPeer(java.awt.Component component, Control control)
		{
            this.control = control;
			this.component = component;
			control.TabStop = false;
			java.awt.Container parent = component.getParent();
			if(parent != null && !(this is java.awt.peer.LightweightPeer))
			{
				if(control is Form)
				{
                    NetComponentPeer parentPeer = (NetComponentPeer)parent.getPeer();
                    if (parentPeer != null)
					{
						((Form)control).Owner = (Form)parentPeer.control;
					}
				}
				else
				{
					java.awt.Container p = parent;
					while(p != null && p.getPeer() is java.awt.peer.LightweightPeer)
					{
						p = p.getParent();
					}
					if(p != null)
					{
						control.Parent = ((NetComponentPeer)p.getPeer()).control;
					}
				}
			}
            SetBoundsImpl(component.getX(), component.getY(), component.getWidth(), component.getHeight());
            // we need the null check, because for a Window, at this time it doesn't have a foreground yet
			if(component.getForeground() != null)
			{
                SetForeColorImpl(component.getForeground());
			}
			// we need the null check, because for a Window, at this time it doesn't have a background yet
			if(component.getBackground() != null)
			{
                SetBackColorImpl(component.getBackground());
			}
			setEnabled(component.isEnabled());
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
            control.MouseUp += new MouseEventHandler(OnMouseUp);
            control.MouseEnter += new EventHandler(OnMouseEnter);
            control.MouseLeave += new EventHandler(OnMouseLeave);
            control.GotFocus += new EventHandler(OnGotFocus);
            control.LostFocus += new EventHandler(OnLostFocus);
            control.SizeChanged += new EventHandler(OnBoundsChanged);
            control.Leave += new EventHandler(OnBoundsChanged);
            control.Paint += new PaintEventHandler(OnPaint);
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
            if(component.isFontSet())
            {
                //setFontImpl(component.getFont());
                setFont(component.getFont());
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
            if (!(component is java.awt.Window))
            {
                java.awt.Container parent = component.getParent();
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
				postEvent(new java.awt.@event.PaintEvent(component, java.awt.@event.PaintEvent.UPDATE, rect));
			}
		}

		private static int MapKeyCode(Keys key)
		{
			switch(key)
			{
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
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_PRESSED, when, modifiers, keyCode, keyChar, keyLocation));
		}

        private void OnKeyUp(object sender, KeyEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(e.Modifiers);
			int keyCode = MapKeyCode(e.KeyCode);
			// TODO set keyChar
			char keyChar = ' ';
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_STANDARD;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_RELEASED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			int keyCode = java.awt.@event.KeyEvent.VK_UNDEFINED;
			char keyChar = e.KeyChar;
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_STANDARD;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_TYPED, when, modifiers, keyCode, keyChar, keyLocation));
		}

        private void postMouseEvent(MouseEventArgs ev, int id)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = GetMouseEventModifiers(ev);
            int button = GetButton(ev);
            int clickCount = ev.Clicks;
            int x = ev.X + getInsetsLeft(); //The Inset correctur is needed for Window and extended classes
            int y = ev.Y + getInsetsTop();
            postEvent(new java.awt.@event.MouseEvent(component, id, when, modifiers, x, y, clickCount, false, button));
        }

        private void postMouseEvent(EventArgs ev, int id)
        {
            long when = java.lang.System.currentTimeMillis();
            int modifiers = GetModifiers(Control.ModifierKeys);
            int button = 0;
            int clickCount = 0;
            int x = Control.MousePosition.X - control.Location.X;
            int y = Control.MousePosition.Y - control.Location.Y;
            postEvent(new java.awt.@event.MouseEvent(component, id, when, modifiers, x, y, clickCount, false, button));
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs ev)
		{
			if((ev.Button & (MouseButtons.Left | MouseButtons.Right)) != 0)
			{
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_DRAGGED);
			}
			else
			{
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_MOVED);
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
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_PRESSED);
		}

        private void OnClick(object sender, EventArgs ev)
        {
            isMouseClick = true;
        }

        private void OnMouseUp(object sender, MouseEventArgs ev)
		{
            postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_RELEASED);
            if (isMouseClick)
			{
                //We make our own mouse click event because the event order is different in .NET
                //in .NET the click occured before MouseUp
                postMouseEvent(ev, java.awt.@event.MouseEvent.MOUSE_CLICKED);
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
			postEvent(new java.awt.@event.FocusEvent(component, java.awt.@event.FocusEvent.FOCUS_GAINED));
		}

		private void OnLostFocus(object sender, EventArgs e)
		{
			postEvent(new java.awt.@event.FocusEvent(component, java.awt.@event.FocusEvent.FOCUS_LOST));
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
            if (x != component.getX() ||
                y != component.getY() ||
                width != component.getWidth() ||
                height != component.getHeight())
            {
                component.setBounds(x, y, width, height);
            }
        }

        private void OnBoundsChanged(object sender, EventArgs e)
		{
            int x = control.Left;
            int y = control.Top;
            int width = control.Width;
            int height = control.Height;
            if (x != component.getX() ||
                y != component.getY() ||
                width != component.getWidth() ||
                height != component.getHeight())
            {
                //If the component different then we need to update.
                //We call this in a different thread
                //because this event can be a result of a size change with the API
                //If it a result of a API change then component can be synchronized in another thread.
                new SetVoid(componentSetBounds).BeginInvoke(null, null);
            }
            //Will allready send in component.setBounds
            //postEvent(new java.awt.@event.ComponentEvent(component, java.awt.@event.ComponentEvent.COMPONENT_RESIZED));
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
			throw new NotImplementedException();
		}

		public java.awt.Image createImage(int width, int height)
		{
			return new NetBufferedImage(width, height);
		}

		public void disable()
		{
			setEnabled(false);
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
			setEnabled(true);
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
                java.awt.Graphics g = component.getGraphics();
                try
                {
                    java.awt.Rectangle r = ((java.awt.@event.PaintEvent)e).getUpdateRect();
                    g.clipRect(r.x, r.y, r.width, r.height);
                    switch (e.getID())
                    {
                        case java.awt.@event.PaintEvent.UPDATE:
                            component.update(g);
                            break;
                        case java.awt.@event.PaintEvent.PAINT:
                            component.paint(g);
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
			setVisible(false);
		}

		public bool isFocusTraversable()
		{
			throw new NotImplementedException();
		}

		public virtual java.awt.Dimension minimumSize()
		{
			return component.getSize();
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
			postEvent(new java.awt.@event.PaintEvent(component, java.awt.@event.PaintEvent.UPDATE, rect));
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
            postEvent(new java.awt.@event.FocusEvent(request, java.awt.@event.FocusEvent.FOCUS_GAINED, temporary, component));
			return true;
		}

		public void reshape(int x, int y, int width, int height)
		{
			setBounds(x, y, width, height);
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

		public void setBounds(int x, int y, int width, int height)
		{
			control.Invoke(new SetXYWH(SetBoundsImpl), new object[] { x, y, width, height });
            componentSetBounds();
		}

		private void setCursorImpl(java.awt.Cursor cursor)
		{
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
            control.Font = ((NetFontPeer)font.getPeer()).netFont;
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

		private void setVisibleImpl(bool visible)
		{
			control.Visible = visible;
            //will already Post from GNU Classpath
			//postEvent(new java.awt.@event.ComponentEvent(component,
			//	visible ? java.awt.@event.ComponentEvent.COMPONENT_SHOWN : java.awt.@event.ComponentEvent.COMPONENT_HIDDEN));
		}

		public void setVisible(bool visible)
		{
			control.Invoke(new SetBool(setVisibleImpl), new object[] { visible });
		}

		public void show()
		{
			setVisible(true);
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
			return false;
		}

		public bool canDetermineObscurity()
		{
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

		public void setBounds(int x, int y, int width, int height, int z)
		{
            setBounds(x, y, width, height);
            //TODO changing the Z-Order
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
	}

	class NetButtonPeer : NetComponentPeer, ButtonPeer
	{
		public NetButtonPeer(java.awt.Button awtbutton, Button button)
			: base(awtbutton, button)
		{
			if(!awtbutton.isBackgroundSet())
			{
				awtbutton.setBackground(java.awt.SystemColor.control);
			}
			button.BackColor = Color.FromArgb(awtbutton.getBackground().getRGB());
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
			postEvent(new java.awt.@event.ActionEvent(component, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
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
	}

	class NetTextComponentPeer : NetComponentPeer, TextComponentPeer
	{
		public NetTextComponentPeer(java.awt.TextComponent textComponent, TextBox textBox)
			: base(textComponent, textBox)
		{
			if(!component.isBackgroundSet())
			{
				component.setBackground(java.awt.SystemColor.window);
			}
			setBackground(component.getBackground());
			textBox.AutoSize = false;
			textBox.Text = ((java.awt.TextComponent)component).getText();
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
				postEvent(new java.awt.@event.ActionEvent(component, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
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
	}

	class NetChoicePeer : NetComponentPeer, ChoicePeer
	{
		public NetChoicePeer(java.awt.Choice target, ComboBox combobox)
			: base(target, combobox)
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
	}

	class NetCheckboxPeer : NetComponentPeer, CheckboxPeer
	{
		public NetCheckboxPeer(java.awt.Checkbox target, CheckBox checkbox)
			: base(target, checkbox)
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
	}

	class NetLabelPeer : NetComponentPeer, LabelPeer
	{
		public NetLabelPeer(java.awt.Label jlabel, Label label)
			: base(jlabel, label)
		{
			label.Text = jlabel.getText();
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
	}

	class NetTextFieldPeer : NetTextComponentPeer, TextFieldPeer
	{
		public NetTextFieldPeer(java.awt.TextField textField, TextBox textBox)
			: base(textField, textBox)
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
		public NetTextAreaPeer(java.awt.TextArea textArea, TextBox textBox)
			: base(textArea, textBox)
		{
			textBox.ReadOnly = !((java.awt.TextArea)component).isEditable();
			textBox.WordWrap = false;
			textBox.ScrollBars = ScrollBars.Both;
			textBox.Multiline = true;
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

		public NetContainerPeer(java.awt.Container awtcontainer, ContainerControl container)
			: base(awtcontainer, container)
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
	}

	class NetPanelPeer : NetContainerPeer, PanelPeer
	{
		public NetPanelPeer(java.awt.Panel panel, ContainerControl container)
			: base(panel, container)
		{
		}
	}

	class NewCanvasPeer : NetComponentPeer, CanvasPeer
	{
		public NewCanvasPeer(java.awt.Canvas canvas, Control control)
			: base(canvas, control)
		{
		}
	}

	class NetWindowPeer : NetContainerPeer, WindowPeer
	{
		public NetWindowPeer(java.awt.Window window, Form form)
			: base(window, form)
		{
            //form.Shown += new EventHandler(OnOpened); Will already post in java.awt.Window.show()
            form.Closing += new CancelEventHandler(OnClosing);
            form.Closed += new EventHandler(OnClosed);
            form.Activated += new EventHandler(OnActivated);
            form.Deactivate += new EventHandler(OnDeactivate);

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
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_OPENED));
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_CLOSING));
        }

        private void OnClosed(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_CLOSED));
        }

        private void OnActivated(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_ACTIVATED));
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_DEACTIVATED));
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
	}

	class NetFramePeer : NetWindowPeer, FramePeer
	{
		public NetFramePeer(java.awt.Frame frame, Form form)
			: base(frame, form)
		{
			setTitle(frame.getTitle());
        }

        internal override void init()
        {
            if (!component.isFontSet())
            {
                java.awt.Font font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
                component.setFont(font);
            }
            if (!component.isForegroundSet())
            {
                component.setForeground(java.awt.SystemColor.windowText);
            }
            if (!component.isBackgroundSet())
            {
                component.setBackground(java.awt.SystemColor.window);
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
			Console.WriteLine("NOTE: setIconImage not implemented");
		}

		public void setMenuBar(java.awt.MenuBar mb)
		{
			throw new NotImplementedException();
		}

		public void setResizable(bool resizable)
		{
			throw new NotImplementedException();
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

		public void setMaximizedBounds(java.awt.Rectangle r)
		{
			throw new NotImplementedException();
		}

		public void setBoundsPrivate(int x, int y, int width, int height)
		{
			// TODO use control.Invoke
			control.Bounds = new Rectangle(x, y, width, height);
		}
	}

	class NetDialogPeer : NetWindowPeer, DialogPeer
	{
        public NetDialogPeer(java.awt.Dialog target, Form form)
			: base(target, form)
		{
            form.MaximizeBox = false;
            form.MinimizeBox = false;
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
	}

	class NetListPeer : NetComponentPeer, ListPeer
	{
		internal NetListPeer(java.awt.List target, ListBox listbox)
			: base(target, listbox)
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
}
