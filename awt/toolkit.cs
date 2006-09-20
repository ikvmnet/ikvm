/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.

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
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using System.Text;
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
	delegate void SetColor(Color c);
	delegate void SetFont(Font f);
	delegate java.awt.Dimension GetDimension();

	class UndecoratedForm : Form
	{
		public UndecoratedForm()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}
	}

	class MyForm : Form
	{
		public MyForm()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}
	}

	class MyControl : Control
	{
		public MyControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}
	}

	class MyContainerControl : ContainerControl
	{
		public MyContainerControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}
	}

	public class NetToolkit : gnu.java.awt.ClasspathToolkit
	{
		internal static java.awt.EventQueue eventQueue = new java.awt.EventQueue();
		internal static volatile Form bogusForm;
		private static Delegate createControlInstance;
		private int resolution;

		private delegate Control CreateControlInstanceDelegate(Type type);

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

		internal static Control CreateControlImpl(Type type)
		{
			Control control = (Control)Activator.CreateInstance(type);
			control.CreateControl();
			// HACK here we go again...
			IntPtr p = control.Handle;
			if(p == IntPtr.Zero)
			{
				// shut up compiler warning
			}
			return control;
		}

		internal static Control CreateControl(Type type)
		{
			return (Control)bogusForm.Invoke(createControlInstance, new object[] { type });
		}

		public NetToolkit()
		{
			lock(typeof(NetToolkit))
			{
				System.Diagnostics.Debug.Assert(bogusForm == null);

				Thread thread = new Thread(new ThreadStart(MessageLoop));
				thread.ApartmentState = ApartmentState.STA;
				thread.Name = "IKVM AWT WinForms Message Loop";
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
			return new NetButtonPeer(target, (Button)CreateControl(typeof(Button)));
		}

		protected override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
		{
			return new NetTextFieldPeer(target, (TextBox)CreateControl(typeof(TextBox)));
		}

		protected override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
		{
			return new NetLabelPeer(target, (Label)CreateControl(typeof(Label)));
		}

		protected override java.awt.peer.ListPeer createList(java.awt.List target)
		{
			return new NetListPeer(target, (ListBox)CreateControl(typeof(ListBox)));
		}

		protected override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
		{
			return new NetCheckboxPeer(target, (CheckBox)CreateControl(typeof(CheckBox)));
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
			return new NetTextAreaPeer(target, (TextBox)CreateControl(typeof(TextBox)));
		}

		protected override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
		{
			return new NetChoicePeer(target, (ComboBox)CreateControl(typeof(ComboBox)));
		}

		protected override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
		{
			return new NetFramePeer(target, (Form)CreateControl(typeof(MyForm)));
		}

		protected override java.awt.peer.CanvasPeer createCanvas(java.awt.Canvas target)
		{
			return new NewCanvasPeer(target, (Control)CreateControl(typeof(MyControl)));
		}

		protected override java.awt.peer.PanelPeer createPanel(java.awt.Panel target)
		{
			return new NetPanelPeer(target, (ContainerControl)CreateControl(typeof(MyContainerControl)));
		}

		protected override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
		{
			return new NetWindowPeer(target, (Form)CreateControl(typeof(UndecoratedForm)));
		}

		protected override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
		{
			return new NetDialogPeer(target, (Form)CreateControl(typeof(MyForm)));
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
			return new NetFontMetrics(font, getScreenResolution());
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
			catch(Exception)
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
			throw new NotImplementedException();
		}

		public override java.awt.PrintJob getPrintJob(java.awt.Frame frame, string jobtitle, Properties props)
		{
			throw new NotImplementedException();
		}

		public override void beep()
		{
			throw new NotImplementedException();
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

		public override RobotPeer createRobot(java.awt.GraphicsDevice param)
		{
			throw new NotImplementedException();
		}

		public override gnu.java.awt.peer.EmbeddedWindowPeer createEmbeddedWindow(gnu.java.awt.EmbeddedWindow ew)
		{
			throw new NotImplementedException();
		}
	}

	class NetGraphicsEnvironment : java.awt.GraphicsEnvironment
	{
		// Create a bitmap with the dimensions of the argument image. Then
		// create a graphics objects from the bitmap. All paint operations will
		// then paint the bitmap.
		public override java.awt.Graphics2D createGraphics(BufferedImage bi)
		{
			Bitmap bitmap = new Bitmap(bi.getWidth(), bi.getHeight());
			NetGraphics g = new NetGraphics(Graphics.FromImage(bitmap), null, Color.Wheat, true);
			g.setBitmap(bitmap);
			return g;
		}

		public override java.awt.Font[] getAllFonts()
		{
			throw new NotImplementedException();
		}

		public override string[] getAvailableFontFamilyNames()
		{
			throw new NotImplementedException();
		}

		public override string[] getAvailableFontFamilyNames(Locale l)
		{
			throw new NotImplementedException();
		}

		public override java.awt.GraphicsDevice getDefaultScreenDevice()
		{
			return new NetGraphicsDevice();
		}

		public override java.awt.GraphicsDevice[] getScreenDevices()
		{
			throw new NotImplementedException();
		}
	}

	class NetGraphicsDevice : java.awt.GraphicsDevice
	{
		public override java.awt.GraphicsConfiguration[] getConfigurations()
		{
			throw new NotImplementedException();
		}

		public override java.awt.GraphicsConfiguration getDefaultConfiguration()
		{
			return new NetGraphicsConfiguration();
		}

		public override string getIDstring()
		{
			throw new NotImplementedException();
		}

		public override int getType()
		{
			return TYPE_RASTER_SCREEN;
		}
	}

	class NetFontPeer : gnu.java.awt.peer.ClasspathFontPeer
	{
		internal NetFontPeer(string name, java.util.Map attrs)
			: base(name, attrs)
		{
		}

		public override bool canDisplay(java.awt.Font param1, char param2)
		{
			throw new NotImplementedException();
		}

		public override int canDisplayUpTo(java.awt.Font param1, java.text.CharacterIterator param2, int param3, int param4)
		{
			throw new NotImplementedException();
		}

		public override java.awt.font.GlyphVector createGlyphVector(java.awt.Font param1, java.awt.font.FontRenderContext param2, int[] param3)
		{
			throw new NotImplementedException();
		}

		public override java.awt.font.GlyphVector createGlyphVector(java.awt.Font param1, java.awt.font.FontRenderContext param2, java.text.CharacterIterator param3)
		{
			throw new NotImplementedException();
		}

		public override byte getBaselineFor(java.awt.Font param1, char param2)
		{
			throw new NotImplementedException();
		}

		public override java.awt.FontMetrics getFontMetrics(java.awt.Font aFont)
		{
			return new NetFontMetrics(aFont, 0);
		}

		public override string getGlyphName(java.awt.Font param1, int param2)
		{
			throw new NotImplementedException();
		}

		public override java.awt.font.LineMetrics getLineMetrics(java.awt.Font aFont, java.text.CharacterIterator aCharacterIterator, int aBegin, int aLimit, java.awt.font.FontRenderContext aFontRenderContext)
		{
			string s = ToString(aCharacterIterator, aBegin, aLimit);
			return new NetLineMetrics(aFont, s);
		}

		public override java.awt.geom.Rectangle2D getMaxCharBounds(java.awt.Font param1, java.awt.font.FontRenderContext param2)
		{
			throw new NotImplementedException();
		}

		public override int getMissingGlyphCode(java.awt.Font param)
		{
			throw new NotImplementedException();
		}

		public override int getNumGlyphs(java.awt.Font param)
		{
			throw new NotImplementedException();
		}

		public override string getPostScriptName(java.awt.Font param)
		{
			throw new NotImplementedException();
		}

		public override java.awt.geom.Rectangle2D getStringBounds(java.awt.Font aFont, java.text.CharacterIterator aCharacterIterator, int aBegin, int aLimit, java.awt.font.FontRenderContext aFontRenderContext)
		{
			NetFontMetrics fontMetrics = (NetFontMetrics) getFontMetrics(aFont);
			string s = ToString(aCharacterIterator, aBegin, aLimit);

			return fontMetrics.GetStringBounds(s);
		}

		public override bool hasUniformLineMetrics(java.awt.Font param)
		{
			throw new NotImplementedException();
		}

		public override java.awt.font.GlyphVector layoutGlyphVector(java.awt.Font param1, java.awt.font.FontRenderContext param2, char[] param3, int param4, int param5, int param6)
		{
			throw new NotImplementedException();
		}

		public override string getSubFamilyName(java.awt.Font param1, Locale param2)
		{
			throw new NotImplementedException();
		}

		private static string ToString(java.text.CharacterIterator aCharacterIterator, int aBegin, int aLimit)
		{
			aCharacterIterator.setIndex(aBegin);
			StringBuilder sb = new StringBuilder();

			for (int i = aBegin; i <= aLimit; ++i)
			{
				char c = aCharacterIterator.current();
				sb.Append(c);
				aCharacterIterator.next();
			}

			return sb.ToString();
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

	class NoImage : java.awt.Image
	{
		public override int getWidth(java.awt.image.ImageObserver observer)
		{
			if(observer != null)
			{
				observer.imageUpdate(this, java.awt.image.ImageObserver.__Fields.ERROR | java.awt.image.ImageObserver.__Fields.ABORT, 0, 0, -1, -1);
			}
			return -1;
		}

		public override int getHeight(java.awt.image.ImageObserver observer)
		{
			if(observer != null)
			{
				observer.imageUpdate(this, java.awt.image.ImageObserver.__Fields.ERROR | java.awt.image.ImageObserver.__Fields.ABORT, 0, 0, -1, -1);
			}
			return -1;
		}

		public override ImageProducer getSource()
		{
			return null;
		}

		public override java.awt.Graphics getGraphics()
		{
			// TODO throw java.lang.IllegalAccessError: getGraphics() only valid for images created with createImage(w, h)
			return null;
		}

		public override object getProperty(string name, java.awt.image.ImageObserver observer)
		{
			if(observer != null)
			{
				observer.imageUpdate(this, java.awt.image.ImageObserver.__Fields.ERROR | java.awt.image.ImageObserver.__Fields.ABORT, 0, 0, -1, -1);
			}
			return null;
		}

		public override void flush()
		{
		}
	}

	public class NetGraphics : java.awt.Graphics2D
	{
		private bool disposable;
		private Graphics g;
		private java.awt.Color jcolor;
		private Color color = SystemColors.WindowText;
		private Color bgcolor;
		private java.awt.Font font;
		private Font netfont;
		private java.awt.Rectangle _clip;
		private Bitmap mBitmap;

		public NetGraphics(Graphics g, java.awt.Font font, Color bgcolor, bool disposable)
		{
			if(font == null)
			{
				font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
			}
			this.g = g;
			this.font = font;
			netfont = NetFontFromJavaFont(font, g.DpiY);
			this.bgcolor = bgcolor;
			this.disposable = disposable;
			if(!g.IsClipEmpty)
			{
				_clip = new java.awt.Rectangle((int)Math.Round(g.ClipBounds.Left), (int)Math.Round(g.ClipBounds.Top), (int)Math.Round(g.ClipBounds.Width), (int)Math.Round(g.ClipBounds.Height));
			}
		}

		public Bitmap getBitmap()
		{
			return mBitmap;
		}

		public void setBitmap(Bitmap aBitmap)
		{
			mBitmap = aBitmap;
		}

		public override void clearRect(int x, int y, int width, int height)
		{
			using(SolidBrush b = new SolidBrush(bgcolor))
			{
				g.FillRectangle(b, x, y, width, height);
			}
		}

		public override void clipRect(int param1, int param2, int param3, int param4)
		{
		}

		public override void copyArea(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		}

		public override java.awt.Graphics create(int x, int y, int width, int height)
		{
			java.awt.Graphics g = create();
			g.translate(x, y);
			g.setClip(0, 0, width, height);
			return g;
		}

		public override java.awt.Graphics create()
		{
			// TODO we need to actually recreate a new underlying Graphics object, but .NET doesn't
			// seem to have a way of doing that, so we probably need access to the underlying surface.
			// Sigh...
			NetGraphics newg = new NetGraphics(g, font, bgcolor, false);
			disposable = false;
			// TODO copy other attributes
			return newg;
		}

		public override void dispose()
		{
			if(disposable)
			{
				disposable = false;
				g.Dispose();
				g = null;
			}
			netfont.Dispose();
		}

		public override void draw3DRect(int param1, int param2, int param3, int param4, bool param5)
		{
		}

		public override void drawArc(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		}

		public override void drawBytes(byte[] param1, int param2, int param3, int param4, int param5)
		{
		}

		public override void drawChars(char[] param1, int param2, int param3, int param4, int param5)
		{
		}

		public override bool drawImage(java.awt.Image param1, int param2, int param3, int param4, int param5, int param6, int param7, int param8, int param9, java.awt.Color param10, java.awt.image.ImageObserver param11)
		{
			return true;
		}

		public override bool drawImage(java.awt.Image img, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2, java.awt.image.ImageObserver observer)
		{
			if(img is NetBufferedImage)
			{
				Rectangle destRect = new Rectangle(dx1, dy1, dx2 - dx1, dy2 - dy1);
				Rectangle srcRect = new Rectangle(sx1, sy1, sx2 - sx1, sy2 - sy1);
				g.DrawImage(((NetBufferedImage)img).bitmap, destRect, srcRect, GraphicsUnit.Pixel);
			}
			else if(img is NetVolatileImage)
			{
				Rectangle destRect = new Rectangle(dx1, dy1, dx2 - dx1, dy2 - dy1);
				Rectangle srcRect = new Rectangle(sx1, sy1, sx2 - sx1, sy2 - sy1);
				g.DrawImage(((NetVolatileImage)img).bitmap, destRect, srcRect, GraphicsUnit.Pixel);
			}
			else
			{
				throw new NotImplementedException();
			}
			return true;
		}

		public override bool drawImage(java.awt.Image param1, int param2, int param3, int param4, int param5, java.awt.Color param6, java.awt.image.ImageObserver param7)
		{
			return true;
		}

		public override bool drawImage(java.awt.Image param1, int param2, int param3, java.awt.Color param4, java.awt.image.ImageObserver param5)
		{
			return true;
		}

		public override bool drawImage(java.awt.Image param1, int param2, int param3, int param4, int param5, java.awt.image.ImageObserver param6)
		{
			return true;
		}

		public override bool drawImage(java.awt.Image img, int x, int y, java.awt.image.ImageObserver observer)
		{
			if(img is NetBufferedImage)
			{
				g.DrawImage(((NetBufferedImage)img).bitmap, x, y);
			}
			else if(img is NetProducerImage)
			{
				g.DrawImage(((NetProducerImage)img).getBitmap(), x, y);
			}
			else if(img is NetVolatileImage)
			{
				g.DrawImage(((NetVolatileImage)img).bitmap, x, y);
			}
			else if(img is java.awt.image.BufferedImage)
			{
				// TODO this is horrible...
				java.awt.image.BufferedImage bufImg = (java.awt.image.BufferedImage)img;
				for(int iy = 0; iy < bufImg.getHeight(); iy++)
				{
					for(int ix = 0; ix < bufImg.getWidth(); ix++)
					{
						using(Pen p = new Pen(Color.FromArgb(bufImg.getRGB(ix, iy))))
						{
							g.DrawLine(p, x + ix, y + iy, x + ix + 1, y + iy);
						}
					}
				}
			}
			else
			{
				throw new NotImplementedException(img.GetType().FullName);
			}
			return true;
		}

		public override void drawLine(int x1, int y1, int x2, int y2)
		{
			using(Pen p = new Pen(color, 1))
			{
				// HACK DrawLine doesn't appear to draw the last pixel, so for single pixel lines, we have
				// a freaky workaround
				if(x1 == x2 && y1 == y2)
				{
					g.DrawLine(p, x1, y1, x1 + 0.01f, y2 + 0.01f);
				}
				else
				{
					g.DrawLine(p, x1, y1, x2, y2);
				}
			}
		}

		public override void drawOval(int param1, int param2, int param3, int param4)
		{
		}

		public override void drawPolygon(java.awt.Polygon param)
		{
		}

		public override void drawPolygon(int[] aX, int[] aY, int aLength)
		{
			Point[] points = new Point[aLength];

			for (int i = 0; i < aLength; i++)
			{
				points[i].X = aX[i];
				points[i].Y = aY[i];
			}

			using (Pen pen = new Pen(color))
			{
				g.DrawPolygon(pen, points);
			}
		}

		/// <summary>
		/// Draw a sequence of connected lines
		/// </summary>
		/// <param name="aX">Array of x coordinates</param>
		/// <param name="aY">Array of y coordinates</param>
		/// <param name="aLength">Length of coordinate arrays</param>
		public override void drawPolyline(int[] aX, int[] aY, int aLength)
		{
			using (Pen pen = new Pen(color))
			{
				for (int i = 0; i < aLength - 1; i++)
				{
					Point point1 = new Point(aX[i], aY[i]);
					Point point2 = new Point(aX[i+1], aY[i+1]);
					g.DrawLine(pen, point1, point2);
				}
			}
		}

		public override void drawRect(int x, int y, int width, int height)
		{
			using(Pen pen = new Pen(color))
			{
				g.DrawRectangle(pen, x, y, width, height);
			}
		}

		/// <summary>
		/// Apparently there is no rounded rec function in .Net. Draw the
		/// rounded rectangle by using lines and arcs.
		/// </summary>
		public override void drawRoundRect(int x, int y, int w, int h, int radius, int param6)
		{
			using (GraphicsPath gp = createRoundRect(x, y, w, h, radius))
			using (Pen pen = new Pen(color))
			{
				g.DrawPath(pen, gp);
			}
		}

		/// <summary>
		/// Create a rounded rectangle using lines and arcs
		/// </summary>
		/// <param name="x">upper left x coordinate</param>
		/// <param name="y">upper left y coordinate</param>
		/// <param name="w">width</param>
		/// <param name="h">height</param>
		/// <param name="radius">radius of arc</param>
		/// <returns></returns>
		private GraphicsPath createRoundRect(int x, int y, int w, int h, int radius)
		{
			GraphicsPath gp = new GraphicsPath();

			gp.AddLine(x + radius, y, x + w - (radius * 2), y);
			gp.AddArc(x + w - (radius * 2), y, radius * 2, radius * 2, 270, 90);
			gp.AddLine(x + w, y + radius, x + w, y + h - (radius * 2));
			gp.AddArc(x + w - (radius * 2), y + h - (radius * 2), radius * 2, radius * 2, 0, 90);
			gp.AddLine(x + w - (radius * 2), y + h, x + radius, y + h);
			gp.AddArc(x, y + h - (radius * 2), radius * 2, radius * 2, 90, 90);
			gp.AddLine(x, y + h - (radius * 2), x, y + radius);
			gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);

			gp.CloseFigure();

			return gp;
		}

		public override void drawString(java.text.AttributedCharacterIterator param1, int param2, int param3)
		{
		}

		public override void drawString(string str, int x, int y)
		{
			using(Brush brush = new SolidBrush(color))
			{
				int descent = netfont.FontFamily.GetCellDescent(netfont.Style);
				int descentPixel = (int)Math.Round(netfont.Size * descent / netfont.FontFamily.GetEmHeight(netfont.Style));
				g.DrawString(str, netfont, brush, x, y - netfont.Height + descentPixel);
			}
		}

		public override void fill3DRect(int param1, int param2, int param3, int param4, bool param5)
		{
		}

		public override void fillArc(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		}

		public override void fillOval(int param1, int param2, int param3, int param4)
		{
		}

		public override void fillPolygon(java.awt.Polygon aPolygon)
		{
			Point[] points = new Point[aPolygon.npoints];

			for (int i = 0; i < aPolygon.npoints; i++)
			{
				points[i].X = aPolygon.xpoints[i];
				points[i].Y = aPolygon.ypoints[i];
			}

			using(Brush brush = new SolidBrush(color))
			{
				g.FillPolygon(brush, points);
			}
		}

		public override void fillPolygon(int[] aX, int[] aY, int aLength)
		{
			Point[] points = new Point[aLength];

			for (int i = 0; i < aLength; i++)
			{
				points[i].X = aX[i];
				points[i].Y = aY[i];
			}

			using(Brush brush = new SolidBrush(color))
			{
				g.FillPolygon(brush, points);
			}
		}

		public override void fillRect(int x, int y, int width, int height)
		{
			using(Brush brush = new SolidBrush(color))
			{
				g.FillRectangle(brush, x, y, width, height);
			}
		}

		public override void fillRoundRect(int x, int y, int w, int h, int radius, int param6)
		{
			GraphicsPath gp = createRoundRect(x, y, w, h, radius);

			using(Brush brush = new SolidBrush(color))
			{
				g.FillPath(brush, gp);
			}
			gp.Dispose();
		}

		public override java.awt.Shape getClip()
		{
			return getClipBounds();
		}

		public override java.awt.Rectangle getClipBounds(java.awt.Rectangle r)
		{
			if(_clip != null)
			{
				r.x = _clip.x;
				r.y = _clip.y;
				r.width = _clip.width;
				r.height = _clip.height;
			}
			return r;
		}

		public override java.awt.Rectangle getClipBounds()
		{
			return getClipRect();
		}

		[Obsolete]
		public override java.awt.Rectangle getClipRect()
		{
			if(_clip != null)
			{
				return new java.awt.Rectangle(_clip);
			}
			return null;
		}

		public override java.awt.Color getColor()
		{
			if(jcolor == null)
			{
				jcolor = new java.awt.Color(color.ToArgb());
			}
			return jcolor;
		}

		public override java.awt.Font getFont()
		{
			return font;
		}

		internal static Font NetFontFromJavaFont(java.awt.Font f, float dpi)
		{
			FontFamily fam;
			switch(f.getName())
			{
				case "Monospaced":
				case "Courier":
				case "courier":
					fam = FontFamily.GenericMonospace;
					break;
				case "Serif":
					fam = FontFamily.GenericSerif;
					break;
				case "SansSerif":
				case "Dialog":
				case "DialogInput":
				case null:
				case "Default":
					fam = FontFamily.GenericSansSerif;
					break;
				default:
					try
					{
						fam = new FontFamily(f.getName());
					}
					catch(ArgumentException)
					{
						fam = FontFamily.GenericSansSerif;
					}
					break;
			}
			// NOTE Regular is guaranteed zero
			FontStyle style = FontStyle.Regular;
			if(f.isBold())
			{
				style |= FontStyle.Bold;
			}
			if(f.isItalic())
			{
				style |= FontStyle.Italic;
			}
			float em = fam.GetEmHeight(style);
			float line = fam.GetLineSpacing(style);
			return new Font(fam, (int)Math.Round(((f.getSize() * dpi) / 72) * em / line), style, GraphicsUnit.Pixel);
		}

		public override java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			return new NetFontMetrics(f, g.DpiY);
		}

		public override java.awt.FontMetrics getFontMetrics()
		{
			return new NetFontMetrics(font, g.DpiY);
		}

		public override bool hitClip(int param1, int param2, int param3, int param4)
		{
			return true;
		}

		public override void setClip(int x, int y, int width, int height)
		{
			_clip = new java.awt.Rectangle(x, y, width, height);
			g.Clip = new Region(new Rectangle(x, y, width, height));
		}

		public override void setClip(java.awt.Shape param)
		{
			// NOTE we only support rectangular clipping for the moment
			java.awt.Rectangle r = param.getBounds();
			setClip(r.x, r.y, r.width, r.height);
		}

		public override void setColor(java.awt.Color color)
		{
			if(color == null)
			{
				// TODO is this the correct default color?
				color = java.awt.SystemColor.controlText;
			}
			this.jcolor = color;
			this.color = Color.FromArgb(color.getRGB());
		}

		public override void setFont(java.awt.Font f)
		{
			// TODO why is Component calling us with a null reference and is this legal?
			if(f != null)
			{
				Font newfont = NetFontFromJavaFont(f, g.DpiY);
				netfont.Dispose();
				netfont = newfont;
				font = f;
			}
		}

		public override void setPaintMode()
		{
		}

		public override void setXORMode(java.awt.Color param)
		{
		}

		public override void translate(int x, int y)
		{
			System.Drawing.Drawing2D.Matrix matrix = g.Transform;
			matrix.Translate(x, y);
			g.Transform = matrix;
		}

		public override void draw(java.awt.Shape shape)
		{
		}

		public override bool drawImage(java.awt.Image image, java.awt.geom.AffineTransform xform, ImageObserver obs)
		{
			return false;
		}

		public override void drawImage(java.awt.image.BufferedImage image, BufferedImageOp op, int x, int y)
		{
		}

		public override void drawRenderedImage(java.awt.image.RenderedImage image, java.awt.geom.AffineTransform xform)
		{
		}

		public override void drawRenderableImage(java.awt.image.renderable.RenderableImage image, java.awt.geom.AffineTransform xform)
		{
		}

		public override void drawString(string text, float x, float y)
		{
		}

		public override void drawString(java.text.AttributedCharacterIterator iterator, float x, float y)
		{
		}

		public override void fill(java.awt.Shape shape)
		{
		}

		public override bool hit(java.awt.Rectangle rect, java.awt.Shape text, bool onStroke)
		{
			return false;
		}

		public override java.awt.GraphicsConfiguration getDeviceConfiguration()
		{
			return null;
		}

		public override void setComposite(java.awt.Composite comp)
		{
		}

		public override void setPaint(java.awt.Paint paint)
		{
		}

		public override void setStroke(java.awt.Stroke stroke)
		{
		}

		public override void setRenderingHint(java.awt.RenderingHints.Key hintKey, Object hintValue)
		{
		}

		public override object getRenderingHint(java.awt.RenderingHints.Key hintKey)
		{
			return null;
		}

		public override void setRenderingHints(java.util.Map hints)
		{
		}

		public override void addRenderingHints(java.util.Map hints)
		{
		}

		public override java.awt.RenderingHints getRenderingHints()
		{
			return null;
		}

		public override void translate(double tx, double ty)
		{
		}

		public override void rotate(double theta)
		{
		}

		public override void rotate(double theta, double x, double y)
		{
		}

		public override void scale(double scaleX, double scaleY)
		{
		}

		public override void shear(double shearX, double shearY)
		{
		}

		public override void transform(java.awt.geom.AffineTransform Tx)
		{
		}

		public override void setTransform(java.awt.geom.AffineTransform Tx)
		{
		}

		public override java.awt.geom.AffineTransform getTransform()
		{
			return null;
		}

		public override java.awt.Paint getPaint()
		{
			return null;
		}

		public override java.awt.Composite getComposite()
		{
			return null;
		}

		public override void setBackground(java.awt.Color color)
		{
		}

		public override java.awt.Color getBackground()
		{
			return null;
		}

		public override java.awt.Stroke getStroke()
		{
			return null;
		}

		public override void clip(java.awt.Shape s)
		{
		}

		public override java.awt.font.FontRenderContext getFontRenderContext()
		{
			return null;
		}

		public override void drawGlyphVector(java.awt.font.GlyphVector g, float x, float y)
		{
		}
	}

	class NetFontMetrics : java.awt.FontMetrics, IDisposable
	{
		private float dpi;
		private Font mFont;

		public NetFontMetrics(java.awt.Font font, float dpi) : base(font)
		{
			this.dpi = dpi;
		}

		private Font RealizeFont()
		{
			if (mFont == null)
			{
				if (dpi == 0)
				{
					using (Graphics g = NetToolkit.bogusForm.CreateGraphics())
					{
						dpi = g.DpiY;
					}
				}

				mFont = NetGraphics.NetFontFromJavaFont(getFont(), dpi);
			}

			return mFont;
		}

		public override int getHeight()
		{
			return RealizeFont().Height;
		}

		public override int getLeading()
		{
			return (int) Math.Round(GetLeadingFloat());
		}

		public override int getMaxAdvance()
		{
			// HACK very lame
			return charWidth('M');
		}

		public override int charWidth(char ch)
		{
			// HACK we average 20 characters to decrease the influence of the pre/post spacing
			return stringWidth(new String(ch, 20)) / 20;
		}

		public override int charsWidth(char[] data, int off, int len)
		{
			return stringWidth(new String(data, off, len));
		}

		public override int getAscent()
		{
			return (int) Math.Round(GetAscentFloat());
		}

		public override int getDescent()
		{
			return (int) Math.Round(GetDescentFloat());
		}

		public override int stringWidth(string s)
		{
			return (int) Math.Round(GetStringBounds(s).getWidth());
		}

		public float GetAscentFloat()
		{
			Font f = RealizeFont();
			int ascent = f.FontFamily.GetCellAscent(f.Style);
			return f.Size * ascent / f.FontFamily.GetEmHeight(f.Style);
		}

		public float GetDescentFloat()
		{
			Font f = RealizeFont();
			int descent = f.FontFamily.GetCellDescent(f.Style);
			return f.Size * descent / f.FontFamily.GetEmHeight(f.Style);
		}

		public float GetLeadingFloat()
		{
			float leading = getHeight() - (GetAscentFloat() + GetDescentFloat());
			return Math.Max(0.0f, leading);
		}

		public java.awt.geom.Rectangle2D GetStringBounds(String aString)
		{
			using (Graphics g = NetToolkit.bogusForm.CreateGraphics())
			{
				// TODO (KR) Could replace with System.Windows.Forms.TextRenderer#MeasureText (to skip creating Graphics)
				//
				// From .NET Framework Class Library documentation for Graphics#MeasureString:
				//
				//    To obtain metrics suitable for adjacent strings in layout (for
				//    example, when implementing formatted text), use the
				//    MeasureCharacterRanges method or one of the MeasureString
				//    methods that takes a StringFormat, and pass GenericTypographic.
				//    Also, ensure the TextRenderingHint for the Graphics is
				//    AntiAlias.
				//
				// TODO (KR) Consider implementing with one of the Graphics#MeasureString methods that takes a StringFormat.
				// TODO (KR) Consider implementing with Graphics#MeasureCharacterRanges().
				SizeF size = g.MeasureString(aString, RealizeFont(), Int32.MaxValue, StringFormat.GenericTypographic);
				return new java.awt.geom.Rectangle2D.Float(0, 0, size.Width, size.Height);
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (mFont != null)
			{
				mFont.Dispose();
			}
		}

		#endregion
	}

	class NetComponentPeer : ComponentPeer
	{
		internal readonly java.awt.Component component;
		internal readonly Control control;
		private int offsetX;
		private int offsetY;
		private Point mouseDown;
		private long lastClick;
		private int clickCount;

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
					((Form)control).Owner = (Form)((NetComponentPeer)parent.getPeer()).control;
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
				if(parent is java.awt.Frame)
				{
					java.awt.Insets ins = ((NetFramePeer)parent.getPeer()).getInsets();
					offsetX = -ins.left;
					offsetY = -ins.top;
				}
			}
			if(component.isFontSet())
			{
				setFont(component.getFont());
			}
			// we need the null check, because for a Window, at this time it doesn't have a foreground yet
			if(component.getForeground() != null)
			{
				setForeground(component.getForeground());
			}
			// we need the null check, because for a Window, at this time it doesn't have a background yet
			if(component.getBackground() != null)
			{
				setBackground(component.getBackground());
			}
			setEnabled(component.isEnabled());
			//setBounds(component.getX(), component.getY(), component.getWidth(), component.getHeight());
			control.Invoke(new SetVoid(Setup));
			control.Paint += new PaintEventHandler(OnPaint);
			component.invalidate();
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			if(!e.ClipRectangle.IsEmpty)
			{
				int x = 0;
				int y = 0;
				if(component is java.awt.Frame)
				{
					java.awt.Insets insets = ((java.awt.Frame)component).getInsets();
					x = insets.left;
					y = insets.top;
				}
				java.awt.Rectangle rect = new java.awt.Rectangle(e.ClipRectangle.X + x, e.ClipRectangle.Y + y, e.ClipRectangle.Width, e.ClipRectangle.Height);
				postEvent(new java.awt.@event.PaintEvent(component, java.awt.@event.PaintEvent.UPDATE, rect));
			}
		}

		private void Setup()
		{
			// TODO we really only should hook these events when they are needed...
			control.KeyDown += new KeyEventHandler(OnKeyDown);
			control.KeyUp += new KeyEventHandler(OnKeyUp);
			control.KeyPress += new KeyPressEventHandler(OnKeyPress);
			control.MouseMove += new MouseEventHandler(OnMouseMove);
			control.MouseDown += new MouseEventHandler(OnMouseDown);
			control.MouseUp += new MouseEventHandler(OnMouseUp);
			control.MouseEnter += new EventHandler(OnMouseEnter);
			control.MouseLeave += new EventHandler(OnMouseLeave);
			control.GotFocus += new EventHandler(OnGotFocus);
			control.LostFocus += new EventHandler(OnLostFocus);
			control.SizeChanged += new EventHandler(OnSizeChanged);
		}

		private static int MapKeyCode(Keys key)
		{
			switch(key)
			{
				default:
					return (int)key;
			}
		}

		private static int GetModifiers(Keys keys)
		{
			int modifiers = 0;
			if((keys & Keys.Shift) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.SHIFT_DOWN_MASK;
			}
			if((keys & Keys.Control) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.CTRL_DOWN_MASK;
			}
			if((keys & Keys.Alt) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.ALT_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Left) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.BUTTON1_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Middle) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.BUTTON2_DOWN_MASK;
			}
			if((Control.MouseButtons & MouseButtons.Right) != 0)
			{
				modifiers |= java.awt.@event.KeyEvent.BUTTON3_DOWN_MASK;
			}
			return modifiers;
		}

		protected virtual void OnKeyDown(object sender, KeyEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(e.Modifiers);
			int keyCode = MapKeyCode(e.KeyCode);
			// TODO set keyChar
			char keyChar = ' ';
			int keyLocation = java.awt.@event.KeyEvent.KEY_LOCATION_STANDARD;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_PRESSED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyUp(object sender, KeyEventArgs e)
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

		protected virtual void OnMouseMove(object sender, MouseEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			if((e.Button & (MouseButtons.Left | MouseButtons.Right)) != 0)
			{
				postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_DRAGGED, when, modifiers, e.X, e.Y, 0, false));
			}
			else
			{
				postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_MOVED, when, modifiers, e.X, e.Y, 0, false));
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

		private static bool IsWithinDoubleClickRectangle(Point p, int x, int y)
		{
			return Math.Abs(x - p.X) <= SystemInformation.DoubleClickSize.Width / 2 &&
				Math.Abs(y - p.Y) <= SystemInformation.DoubleClickSize.Height / 2;
		}

		protected virtual void OnMouseDown(object sender, MouseEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			if(when > lastClick + SystemInformation.DoubleClickTime
				|| !IsWithinDoubleClickRectangle(mouseDown, e.X, e.Y))
			{
				clickCount = 0;
			}
			clickCount++;
			lastClick = when;
			mouseDown = new Point(e.X, e.Y);
			int modifiers = GetModifiers(Control.ModifierKeys);
			int button = GetButton(e);
			postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_PRESSED, when, modifiers, e.X, e.Y, clickCount, false, button));
		}

		protected virtual void OnMouseUp(object sender, MouseEventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			int button = GetButton(e);
			postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_RELEASED, when, modifiers, e.X, e.Y, clickCount, (e.Button & MouseButtons.Right) != 0, button));
			if(mouseDown.X == e.X && mouseDown.Y == e.Y)
			{
				postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_CLICKED, when, modifiers, e.X, e.Y, clickCount, false, button));
			}
		}

		private void OnMouseEnter(object sender, EventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			int x = Control.MousePosition.X;
			int y = Control.MousePosition.Y;
			postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_ENTERED, when, modifiers, x, y, 0, false));
		}

		private void OnMouseLeave(object sender, EventArgs e)
		{
			long when = java.lang.System.currentTimeMillis();
			int modifiers = GetModifiers(Control.ModifierKeys);
			int x = Control.MousePosition.X;
			int y = Control.MousePosition.Y;
			postEvent(new java.awt.@event.MouseEvent(component, java.awt.@event.MouseEvent.MOUSE_EXITED, when, modifiers, x, y, 0, false));
		}

		private void OnGotFocus(object sender, EventArgs e)
		{
			postEvent(new java.awt.@event.FocusEvent(component, java.awt.@event.FocusEvent.FOCUS_GAINED));
		}

		private void OnLostFocus(object sender, EventArgs e)
		{
			postEvent(new java.awt.@event.FocusEvent(component, java.awt.@event.FocusEvent.FOCUS_LOST));
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			postEvent(new java.awt.@event.ComponentEvent(component, java.awt.@event.ComponentEvent.COMPONENT_RESIZED));
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
			return new NetFontMetrics(f, 0);
		}

		public virtual java.awt.Graphics getGraphics()
		{
			return new NetGraphics(control.CreateGraphics(), component.getFont(), control.BackColor, true);
		}

		public java.awt.Point getLocationOnScreen()
		{
			// TODO use control.Invoke
			Point p = control.PointToScreen(new Point(0, 0));
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
			if(e is java.awt.@event.PaintEvent)
			{
				java.awt.Graphics g = component.getGraphics();
				try
				{
					java.awt.Rectangle r = ((java.awt.@event.PaintEvent)e).getUpdateRect();
					r = r.intersection(g.getClipRect());
					g.setClip(r);
					switch(e.getID())
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

		public bool requestFocus(java.awt.Component source, bool bool1, bool bool2, long x)
		{
			// TODO what do all the args mean?
			requestFocus();
			return true;
		}

		public void reshape(int x, int y, int width, int height)
		{
			throw new NotImplementedException();
		}

		public void setBackground(java.awt.Color color)
		{
			control.Invoke(new SetColor(SetBackColorImpl), new object[] { Color.FromArgb(color.getRGB()) });
		}

		private void SetBackColorImpl(Color c)
		{
			control.BackColor = c;
		}

		private void setBoundsImpl(int x, int y, int width, int height)
		{
			control.SetBounds(x, y, width, height);
		}

		public void setBounds(int x, int y, int width, int height)
		{
			control.Invoke(new SetXYWH(setBoundsImpl), new object[] { x + offsetX, y + offsetY, width, height });
		}

		public void setCursor(java.awt.Cursor cursor)
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

		private void setEnabledImpl(bool enabled)
		{
			control.Enabled = enabled;
		}

		public void setEnabled(bool enabled)
		{
			control.Invoke(new SetBool(setEnabledImpl), new object[] { enabled });
		}

		private void setFontImpl(Font font)
		{
			control.Font = font;
		}

		public void setFont(java.awt.Font font)
		{
			control.Invoke(new SetFont(setFontImpl), new object[] { NetGraphics.NetFontFromJavaFont(font, component.getToolkit().getScreenResolution()) });
		}

		public void setForeground(java.awt.Color color)
		{
			control.Invoke(new SetColor(SetForeColorImpl), new object[] { Color.FromArgb(color.getRGB()) });
		}

		private void SetForeColorImpl(Color c)
		{
			control.ForeColor = c;
		}

		private void setVisibleImpl(bool visible)
		{
			control.Visible = visible;
			postEvent(new java.awt.@event.ComponentEvent(component,
				visible ? java.awt.@event.ComponentEvent.COMPONENT_SHOWN : java.awt.@event.ComponentEvent.COMPONENT_HIDDEN));
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
			return new NetGraphicsConfiguration();
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
			control.Bounds = new Rectangle(x, y, width, height);
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

	class NetVolatileImage : java.awt.image.VolatileImage
	{
		internal Bitmap bitmap;

		internal NetVolatileImage(int width, int height)
		{
			bitmap = new Bitmap(width, height);
			using(Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.White);
			}
		}

		public override bool contentsLost()
		{
			return false;
		}

		public override int getHeight(ImageObserver io)
		{
			return bitmap.Height;
		}

		public override int getWidth(ImageObserver io)
		{
			return bitmap.Width;
		}

		public override object getProperty(string str, ImageObserver io)
		{
			throw new NotImplementedException();
		}

		public override java.awt.Graphics2D createGraphics()
		{
			Graphics g = Graphics.FromImage(bitmap);
			// HACK for off-screen images we don't want ClearType or anti-aliasing
			// TODO I'm sure Java 2D has a way to control text rendering quality, we should honor that
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
			return new NetGraphics(g, null, Color.White, true);
		}

		public override int getHeight()
		{
			return bitmap.Height;
		}

		public override int getWidth()
		{
			return bitmap.Width;
		}

		public override BufferedImage getSnapshot()
		{
			return new NetBufferedImage(bitmap);
		}

		public override int validate(java.awt.GraphicsConfiguration gc)
		{
			return 0;
		}

		public override java.awt.ImageCapabilities getCapabilities()
		{
			throw new NotImplementedException();
		}
	}

	// HACK Classpath should have a working BufferedImage, but currently it doesn't, until then, we
	// provide a hacked up version
	class NetBufferedImage : java.awt.image.BufferedImage
	{
		internal Bitmap bitmap;

		internal NetBufferedImage(Bitmap bitmap)
			: base(bitmap.Width, bitmap.Height, java.awt.image.BufferedImage.TYPE_INT_RGB)
		{
			this.bitmap = bitmap;
		}

		internal NetBufferedImage(int width, int height)
			: base(width, height, java.awt.image.BufferedImage.TYPE_INT_RGB)
		{
			bitmap = new Bitmap(width, height);
			using(Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.White);
			}
		}

		public override java.awt.Graphics2D createGraphics()
		{
			Graphics g = Graphics.FromImage(bitmap);
			// HACK for off-screen images we don't want ClearType or anti-aliasing
			// TODO I'm sure Java 2D has a way to control text rendering quality, we should honor that
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
			return new NetGraphics(g, null, Color.White, true);
		}

		public override java.awt.image.ImageProducer getSource()
		{
			int[] pix = new int[bitmap.Width * bitmap.Height];
			for(int y = 0; y < bitmap.Height; y++)
			{
				for(int x = 0; x < bitmap.Width; x++)
				{
					pix[x + y * bitmap.Width] = bitmap.GetPixel(x, y).ToArgb();
				}
			}
			return new java.awt.image.MemoryImageSource(bitmap.Width, bitmap.Height, pix, 0, bitmap.Width);
		}
	}

	class NetProducerImage : java.awt.Image, java.awt.image.ImageConsumer
	{
		private java.awt.image.ImageProducer source;

		private int mHeight = 0;

		private int mWidth = 0;

		private int mHintFlags = 0;

		private ColorModel mColorModel = null;

		private Hashtable mProperties;

		private Bitmap mBitmap;

		internal NetProducerImage(java.awt.image.ImageProducer source)
		{
			this.source = source;
		}

		public override void flush()
		{
		}

		public override java.awt.Graphics getGraphics()
		{
			return null;
		}

		public override int getHeight(ImageObserver param)
		{
			return mHeight;
		}

		public override int getWidth(ImageObserver param)
		{
			return mWidth;
		}

		public override object getProperty(string param, ImageObserver obs)
		{
			return null;
		}

		public override ImageProducer getSource()
		{
			return source;
		}

		public void setHints(int hintflags)
		{
			mHintFlags = hintflags;
		}

		public void setPixels(int x, int y, int w, int h, ColorModel model, byte[] pixels, int off, int scansize)
		{
			Console.WriteLine("NetBufferedImage: setPixels");
		}

		/// <summary>
		/// Create a bitmap from the pixel array. The bitmap will be used
		/// by drawImage.
		/// </summary>
		void java.awt.image.ImageConsumer.setPixels(int aX, int aY, int w, int h, ColorModel model, int[] pixels, int off, int scansize)
		{
			mWidth = w;
			mHeight = h;
			mColorModel = model;
			mBitmap = new Bitmap(mWidth, mHeight);
			int pixel = 0;
			for (int y = 0; y < mHeight; ++y)
			{
				for (int x = 0; x < mWidth; x++)
				{
					uint argb = (uint)pixels[pixel++];
					int blue = (int)argb & 0xff;
					argb >>= 8;
					int green = (int)argb & 0xff;
					argb >>= 8;
					int red = (int)argb & 0xff;
					argb >>= 8;
					int alpha = (int)argb & 0xff;
					mBitmap.SetPixel(x, y, Color.FromArgb(alpha, red, green, blue));
				}
			}
		}

		public Bitmap getBitmap()
		{
			return mBitmap;
		}

		public void setDimensions(int width, int height)
		{
			mWidth = width;
			mHeight = height;
		}

		public void imageComplete(int status)
		{
			// Console.WriteLine("NetBufferedImage: imageComplete");
		}

		public void setColorModel(ColorModel model)
		{
			mColorModel = model;
		}

		public void setProperties(Hashtable props)
		{
			mProperties = props;
		}
	}

	class NetGraphicsConfiguration : java.awt.GraphicsConfiguration
	{
		public override java.awt.image.BufferedImage createCompatibleImage(int param1, int param2, int param3)
		{
			throw new NotImplementedException();
		}

		public override java.awt.image.BufferedImage createCompatibleImage(int width, int height)
		{
			return new NetBufferedImage(width, height);
		}

		public override java.awt.image.VolatileImage createCompatibleVolatileImage(int param1, int param2, java.awt.ImageCapabilities param3)
		{
			throw new NotImplementedException();
		}

		public override java.awt.image.VolatileImage createCompatibleVolatileImage(int param1, int param2)
		{
			throw new NotImplementedException();
		}

		public override java.awt.Rectangle getBounds()
		{
			throw new NotImplementedException();
		}

		public override java.awt.BufferCapabilities getBufferCapabilities()
		{
			throw new NotImplementedException();
		}

		public override java.awt.image.ColorModel getColorModel(int param)
		{
			throw new NotImplementedException();
		}

		public override java.awt.image.ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}

		public override java.awt.geom.AffineTransform getDefaultTransform()
		{
			throw new NotImplementedException();
		}

		public override java.awt.GraphicsDevice getDevice()
		{
			return new NetGraphicsDevice();
		}

		public override java.awt.ImageCapabilities getImageCapabilities()
		{
			throw new NotImplementedException();
		}

		public override java.awt.geom.AffineTransform getNormalizingTransform()
		{
			throw new NotImplementedException();
		}

		public override VolatileImage createCompatibleVolatileImage(int i1, int i2, int i3)
		{
			throw new NotImplementedException();
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
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			if(!component.isBackgroundSet())
			{
				component.setBackground(java.awt.SystemColor.window);
			}
			TextBox textBox = (TextBox)control;
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
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			TextBox textBox = (TextBox)control;
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
		private java.awt.Insets _insets = new java.awt.Insets(0, 0, 0, 0);

		public NetContainerPeer(java.awt.Container awtcontainer, ContainerControl container)
			: base(awtcontainer, container)
		{
		}

		public java.awt.Insets insets()
		{
			return getInsets();
		}

		public virtual java.awt.Insets getInsets()
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
			throw new NotImplementedException();
		}

		public void endLayout()
		{
			throw new NotImplementedException();
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
			if(!window.isFontSet())
			{
				window.setFont(new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12));
			}
			if(!window.isForegroundSet())
			{
				window.setForeground(java.awt.SystemColor.windowText);
			}
			if(!window.isBackgroundSet())
			{
				window.setBackground(java.awt.SystemColor.window);
			}
			setFont(window.getFont());
			setForeground(window.getForeground());
			setBackground(window.getBackground());
			form.SetBounds(window.getX(), window.getY(), window.getWidth(), window.getHeight());
		}

		public void toBack()
		{
			((Form)control).SendToBack();
		}

		public void toFront()
		{
			((Form)control).Activate();
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
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			Form form = (Form)control;
			form.Closing += new CancelEventHandler(Closing);
		}

		private void Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_CLOSING));
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

		public override java.awt.Graphics getGraphics()
		{
			NetGraphics g = new NetGraphics(control.CreateGraphics(), component.getFont(), control.BackColor, true);
			java.awt.Insets insets = ((java.awt.Frame)component).getInsets();
			g.translate(-insets.left, -insets.top);
			g.setClip(insets.left, insets.top, control.ClientRectangle.Width, control.ClientRectangle.Height);
			return g;
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

		public override java.awt.Insets getInsets()
		{
			// TODO use control.Invoke
			Form f = (Form)control;
			Rectangle client = f.ClientRectangle;
			Rectangle r = f.RectangleToScreen(client);
			int x = r.Location.X - f.Location.X;
			int y = r.Location.Y - f.Location.Y;
			return new java.awt.Insets(y, x, control.Height - client.Height - y, control.Width - client.Width - x);
		}

		public int getState()
		{
			throw new NotImplementedException();
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
		internal NetDialogPeer(java.awt.Dialog target, Form form)
			: base(target, form)
		{
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

	class NetLineMetrics : java.awt.font.LineMetrics
	{
		private java.awt.Font mFont;
		private String mString;
		private NetFontMetrics mFontMetrics;

		public NetLineMetrics(java.awt.Font aFont, String aString)
		{
			mFont = aFont;
			mString = aString;
			mFontMetrics = new NetFontMetrics(aFont, 0);
		}

		public override float getAscent()
		{
			return mFontMetrics.GetAscentFloat();
		}

		public override int getBaselineIndex()
		{
			throw new NotImplementedException();
		}

		public override float[] getBaselineOffsets()
		{
			// TODO (KR) Probably could implement with Graphics#MeasureCharacterRanges(), if called.
			throw new NotImplementedException();
		}

		public override float getDescent()
		{
			return mFontMetrics.GetDescentFloat();
		}

		public override float getHeight()
		{
			// TODO (KR) Could implement with Graphics#MeasureString().
			// TODO (KR) Consider implementing with Graphics#MeasureCharacterRanges().
			return mFontMetrics.getHeight();
		}

		public override float getLeading()
		{
			return mFontMetrics.GetLeadingFloat();
		}

		public override int getNumChars()
		{
			return mString.Length;
		}

		public override float getStrikethroughOffset()
		{
			throw new NotImplementedException();
		}

		public override float getStrikethroughThickness()
		{
			throw new NotImplementedException();
		}

		public override float getUnderlineOffset()
		{
			throw new NotImplementedException();
		}

		public override float getUnderlineThickness()
		{
			throw new NotImplementedException();
		}
	}
}
