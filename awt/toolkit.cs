/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
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
	delegate java.awt.Dimension GetDimension();

	public class NetToolkit : java.awt.Toolkit
	{
		private static java.awt.EventQueue eventQueue = new java.awt.EventQueue();
		private static Form bogusForm;
		private static Delegate createControlInstance;
		private int resolution;

		private delegate Control CreateControlInstanceDelegate(Type type);

		private static void MessageLoop()
		{
			createControlInstance = new CreateControlInstanceDelegate(CreateControlImpl);
			Form form = new Form();
			form.CreateControl();
			// HACK I have no idea why this line is necessary...
			IntPtr p = form.Handle;
			bogusForm = form;
			Application.Run();
		}

		internal static Control CreateControlImpl(Type type)
		{
			Control control = (Control)Activator.CreateInstance(type);
			control.CreateControl();
			// HACK here we go again...
			IntPtr p = control.Handle;
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
				if(bogusForm != null)
				{
					throw new InvalidOperationException();
				}
				Thread thread = new Thread(new ThreadStart(MessageLoop));
				thread.Start();
				// TODO don't use polling...
				while(bogusForm == null)
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
			throw new NotImplementedException();
		}

		protected override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		protected override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
		{
			return new NetFramePeer(target, (Form)CreateControl(typeof(Form)));
		}

		protected override java.awt.peer.CanvasPeer createCanvas(java.awt.Canvas target)
		{
			return new NewCanvasPeer(target, (Control)CreateControl(typeof(Control)));
		}

		protected override java.awt.peer.PanelPeer createPanel(java.awt.Panel target)
		{
			return new NetPanelPeer(target, (ContainerControl)CreateControl(typeof(ContainerControl)));
		}

		protected override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
		{
			throw new NotImplementedException();
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
		protected override java.awt.peer.FontPeer getFontPeer(string name, int style)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Dimension getScreenSize()
		{
			throw new NotImplementedException();
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

		public override string[] getFontList()
		{
			throw new NotImplementedException();
		}

		public override java.awt.FontMetrics getFontMetrics(java.awt.Font font)
		{
			return new NetFontMetrics(font, NetGraphics.NetFontFromJavaFont(font, getScreenResolution()), null, null);
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
			throw new NotImplementedException();
		}

		public override java.awt.Image createImage(string filename)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image createImage(URL url)
		{
			throw new NotImplementedException();
		}

		public override bool prepareImage(java.awt.Image image, int width, int height, java.awt.image.ImageObserver observer)
		{
			// HACK for now we call checkImage to obtain the status and fire the observer
			return (checkImage(image, width, height, observer) & 32) != 0;
		}

		public override int checkImage(java.awt.Image image, int width, int height, java.awt.image.ImageObserver observer)
		{
			if(image.getWidth(null) == -1)
			{
				if(observer != null)
				{
					observer.imageUpdate(image, 64, 0, 0, -1, -1);
				}
				return 64; // ERROR
			}
			if(observer != null)
			{
				observer.imageUpdate(image, 1 + 2 + 16 + 32, 0, 0, image.getWidth(null), image.getHeight(null));
			}
			// HACK we cannot use the constants defined in the interface from C#, so we hardcode the flags
			return 1 + 2 + 16 + 32; // WIDTH + HEIGHT + FRAMEBITS + ALLBITS
		}

		public override java.awt.Image createImage(java.awt.image.ImageProducer producer)
		{
			throw new NotImplementedException();
		}

		public override java.awt.Image createImage(sbyte[] imagedata, int imageoffset, int imagelength)
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

		protected override java.awt.peer.LightweightPeer createComponent(java.awt.Component target)
		{
			if(target is java.awt.Container)
			{
				return new NetLightweightContainerPeer((java.awt.Container)target);
			}
			return new NetLightweightComponentPeer(target);
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
			: base(target, (ContainerControl)((NetContainerPeer)target.getParent().getPeer()).control)
		{
		}
	}

	class NoImage : java.awt.Image
	{
		public override int getWidth(java.awt.image.ImageObserver observer)
		{
			return -1;
		}

		public override int getHeight(java.awt.image.ImageObserver observer)
		{
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
			return null;
		}

		public override void flush()
		{
		}
	}

	class NetGraphics : java.awt.Graphics2D
	{
		private bool disposable;
		private Graphics g;
		private java.awt.Color jcolor;
		private Color color = SystemColors.WindowText;
		private java.awt.Font font;
		private Font netfont;
		private java.awt.Rectangle _clip;

		public NetGraphics(Graphics g, java.awt.Font font, bool disposable)
		{
			if(font == null)
			{
				font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
			}
			this.g = g;
			this.font = font;
			netfont = NetFontFromJavaFont(font, g.DpiY);
			this.disposable = disposable;
			if(!g.IsClipEmpty)
			{
				_clip = new java.awt.Rectangle((int)Math.Round(g.ClipBounds.Left), (int)Math.Round(g.ClipBounds.Top), (int)Math.Round(g.ClipBounds.Width), (int)Math.Round(g.ClipBounds.Height));
			}
		}

		public override void clearRect(int x, int y, int width, int height)
		{
			// TODO get the background color from somewhere
			g.FillRectangle(SystemBrushes.Window, x, y, width, height);
		}

		public override void clipRect(int param1, int param2, int param3, int param4)
		{
		
		}

		public override void copyArea(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		
		}

		public override java.awt.Graphics create(int param1, int param2, int param3, int param4)
		{
			return null;
		}

		public override java.awt.Graphics create()
		{
			// TODO we need to actually recreate a new underlying Graphics object, but .NET doesn't
			// seem to have a way of doing that, so we probably need access to the underlying surface.
			// Sigh...
			NetGraphics newg = new NetGraphics(g, font, false);
			// TODO copy other attributes
			return newg;
		}

		public override void dispose()
		{
			if(disposable)
			{
				disposable = false;
				g.Dispose();
			}
			netfont.Dispose();
		}

		public override void draw3DRect(int param1, int param2, int param3, int param4, bool param5)
		{
		
		}

		public override void drawArc(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		
		}

		public override void drawBytes(sbyte[] param1, int param2, int param3, int param4, int param5)
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
			else
			{
				throw new NotImplementedException();
			}
			return true;
		}

		public override void drawLine(int x1, int y1, int x2, int y2)
		{
			using(Pen p = new Pen(color))
			{
				// HACK DrawLine doesn't appear to draw the last pixel, so for single pixel lines, we add one
				// TODO figure out if this applies to all lines
				if(x1 == x2 && y1 == y2)
				{
					x2++;
				}
				g.DrawLine(p, x1, y1, x2, y2);
			}
		}

		public override void drawOval(int param1, int param2, int param3, int param4)
		{
		
		}

		public override void drawPolygon(java.awt.Polygon param)
		{
		
		}

		public override void drawPolygon(int[] param1, int[] param2, int param3)
		{
		
		}

		public override void drawPolyline(int[] param1, int[] param2, int param3)
		{
		
		}

		public override void drawRect(int x, int y, int width, int height)
		{
			using(Pen pen = new Pen(color))
			{
				g.DrawRectangle(pen, x, y, width, height);
			}		
		}

		public override void drawRoundRect(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		
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

		public override void fillPolygon(java.awt.Polygon param)
		{
		
		}

		public override void fillPolygon(int[] param1, int[] param2, int param3)
		{
		
		}

		public override void fillRect(int x, int y, int width, int height)
		{
			using(Brush brush = new SolidBrush(color))
			{
				g.FillRectangle(brush, x, y, width, height);
			}
		}

		public override void fillRoundRect(int param1, int param2, int param3, int param4, int param5, int param6)
		{
		
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

		public override java.awt.Rectangle getClipRect()
		{
			if(_clip != null)
			{
				return (java.awt.Rectangle)_clip.clone();
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
					fam = new FontFamily(f.getName());
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
			return new NetFontMetrics(f, NetFontFromJavaFont(f, g.DpiY), g, null);
		}

		public override java.awt.FontMetrics getFontMetrics()
		{
			return new NetFontMetrics(font, netfont, g, null);
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
			this.jcolor = color;
			this.color = Color.FromArgb(color.getRGB());
		}

		public override void setFont(java.awt.Font f)
		{
			Font newfont = NetFontFromJavaFont(f, g.DpiY);
			netfont.Dispose();
			netfont = newfont;
			font = f;
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

	class NetFontMetrics : java.awt.FontMetrics
	{
		private Font netFont;
		private Graphics g;
		private Control c;

		public NetFontMetrics(java.awt.Font f, Font netFont, Graphics g, Control c) : base(f)
		{
			this.netFont = netFont;
			this.g = g;
			this.c = c;
		}

		public override int getHeight()
		{
			return netFont.Height;
		}

		public override int getLeading()
		{
			// HACK we always return 1
			return 1;
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
			int ascent = netFont.FontFamily.GetCellAscent(netFont.Style);
			return (int)Math.Round(netFont.Size * ascent / netFont.FontFamily.GetEmHeight(netFont.Style));
		}

		public override int getDescent()
		{
			int descent = netFont.FontFamily.GetCellDescent(netFont.Style);
			return (int)Math.Round(netFont.Size * descent / netFont.FontFamily.GetEmHeight(netFont.Style));
		}

		public override int stringWidth(string s)
		{
			if(g != null)
			{
				try
				{
					return (int)Math.Round(g.MeasureString(s, netFont).Width);
				}
				catch(ObjectDisposedException)
				{
					g = null;
				}
			}
			if(c != null)
			{
				using(Graphics g1 = c.CreateGraphics())
				{
					return (int)Math.Round(g1.MeasureString(s, netFont).Width);
				}
			}
			// as a last resort, we make a lame guess
			return s.Length * getHeight() / 2;
		}
	}

	class NetComponentPeer : ComponentPeer
	{
		internal readonly java.awt.Component component;
		internal readonly Control control;
		private int offsetX;
		private int offsetY;

		public NetComponentPeer(java.awt.Component component, Control control)
		{
			this.control = control;
			this.component = component;
			java.awt.Container parent = component.getParent();
			if(parent != null && !(this is java.awt.peer.LightweightPeer))
			{
				control.Parent = ((NetComponentPeer)parent.getPeer()).control;
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
			// TODO figure out if we need an update or a paint
			java.awt.Rectangle rect = new java.awt.Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, e.ClipRectangle.Height);
			//postEvent(new java.awt.@event.PaintEvent(component, java.awt.@event.PaintEvent.UPDATE, rect));
			postEvent(new java.awt.@event.PaintEvent(component, java.awt.@event.PaintEvent.PAINT, rect));
		}

		private void Setup()
		{
			// TODO we really only should hook these events when they are needed...
			control.KeyDown += new KeyEventHandler(OnKeyDown);
			control.KeyUp += new KeyEventHandler(OnKeyUp);
			control.KeyPress += new KeyPressEventHandler(OnKeyPress);
		}

		private static int MapKeyCode(Keys key)
		{
			switch(key)
			{
				default:
					return (int)key;
			}
		}

		protected virtual void OnKeyDown(object sender, KeyEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = MapKeyCode(e.KeyCode);
			char keyChar = ' ';
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_PRESSED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyUp(object sender, KeyEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = MapKeyCode(e.KeyCode);
			char keyChar = ' ';
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_RELEASED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = 0;
			char keyChar = e.KeyChar;
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_TYPED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected void postEvent(java.awt.AWTEvent evt)
		{
			getToolkit().getSystemEventQueue().postEvent(evt);
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
			throw new NotImplementedException();
		}

		public void disable()
		{
			throw new NotImplementedException();
		}

		public void dispose()
		{
			control.Parent = null;
		}

		public void enable()
		{
			throw new NotImplementedException();
		}

		public ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}

		public java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			// HACK this is a very heavy weight way to determine DPI, it should be possible
			// to do this without creating a Graphics object
			using(Graphics g = control.CreateGraphics())
			{
				return new NetFontMetrics(f, NetGraphics.NetFontFromJavaFont(f, g.DpiY), null, control);
			}
		}

		public virtual java.awt.Graphics getGraphics()
		{
			return new NetGraphics(control.CreateGraphics(), component.getFont(), true);
		}

		public java.awt.Point getLocationOnScreen()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getMinimumSize()
		{
			return minimumSize();
		}

		public virtual java.awt.Dimension getPreferredSize()
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
					component.update(g);
				}
				finally
				{
					g.dispose();
				}
			}
			else
			{
				Console.WriteLine("NOTE: NetComponentPeer.handleEvent not implemented: " + e);
			}
		}

		public void hide()
		{
			throw new NotImplementedException();
		}

		public bool isFocusTraversable()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension minimumSize()
		{
			return component.getSize();
		}

		public java.awt.Dimension preferredSize()
		{
			return minimumSize();
		}

		public void paint(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
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
			Rectangle rect = new Rectangle(x, y, width, height);
			rect.Offset(offsetX, offsetY);
			control.Invoke(new SetRectangle(control.Invalidate), new object[] { rect });
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
			throw new NotImplementedException();
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

		public void setFont(java.awt.Font font)
		{
			// TODO use control.Invoke
			control.Font = NetGraphics.NetFontFromJavaFont(font, component.getToolkit().getScreenResolution());
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
			throw new NotImplementedException();
		}

		public java.awt.GraphicsConfiguration getGraphicsConfiguration()
		{
			return new NetGraphicsConfiguration();
		}

		public void setEventMask (long mask)
		{
			Console.WriteLine("NOTE: NetComponentPeer.setEventMask not implemented");
		}

		public bool isObscured()
		{
			throw new NotImplementedException();
		}

		public bool canDetermineObscurity()
		{
			throw new NotImplementedException();
		}

		public void coalescePaintEvent(java.awt.@event.PaintEvent e)
		{
			throw new NotImplementedException();
		}

		public void updateCursorImmediately()
		{
			throw new NotImplementedException();
		}

		public VolatileImage createVolatileImage(int width, int height)
		{
			throw new NotImplementedException();
		}

		public bool handlesWheelScrolling()
		{
			throw new NotImplementedException();
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
			return new NetGraphics(g, null, true);
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
			throw new NotImplementedException();
		}

		public override java.awt.ImageCapabilities getImageCapabilities()
		{
			throw new NotImplementedException();
		}

		public override java.awt.geom.AffineTransform getNormalizingTransform()
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

		public override java.awt.Dimension getPreferredSize()
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
		public void setCaretPosition(int pos)
		{
			throw new NotImplementedException();
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

		public override java.awt.Dimension getPreferredSize()
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		public void setEchoCharacter(char echo_char)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		public java.awt.Dimension getMinimumSize(int rows, int cols)
		{
			throw new NotImplementedException();
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
		public NetContainerPeer(java.awt.Container awtcontainer, ContainerControl container)
			: base(awtcontainer, container)
		{
		}

		public java.awt.Insets insets()
		{
			throw new NotImplementedException();
		}

		public virtual java.awt.Insets getInsets()
		{
			Console.WriteLine("NOTE: NetContainerPeer.getInsets not implemented");
			return new java.awt.Insets(0, 0, 0, 0);
		}

		public void beginValidate()
		{
			Console.WriteLine("NOTE: NetContainerPeer.beginValidate not implemented");
		}

		public void endValidate()
		{
			Console.WriteLine("NOTE: NetContainerPeer.endValidate not implemented");
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
			form.Resize += new EventHandler(Resize);
			form.Closing += new CancelEventHandler(Closing);
		}

		private void Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_CLOSING));
		}

		private void Resize(object sender, EventArgs e)
		{
			// TODO I have no clue what I should do here...
			Rectangle r = control.Bounds;
			component.setBounds(r.X, r.Y, r.Width, r.Height);
			component.invalidate();
			component.validate();
			postEvent(new java.awt.@event.ComponentEvent(component, java.awt.@event.ComponentEvent.COMPONENT_RESIZED));
		}

		public override java.awt.Graphics getGraphics()
		{
			NetGraphics g = new NetGraphics(control.CreateGraphics(), component.getFont(), true);
			java.awt.Insets insets = ((java.awt.Frame)component).getInsets();
			g.translate(-insets.left, -insets.top);
			g.setClip(insets.left, insets.top, control.ClientRectangle.Width, control.ClientRectangle.Height);
			return g;
		}

		public void setIconImage(java.awt.Image image)
		{
			throw new NotImplementedException();
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
	}
}
