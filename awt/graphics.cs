/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.
  Copyright (C) 2006 - 2014 Volker Berlin (i-net software)
  Copyright (C) 2011 Karsten Heinrich (i-net software)

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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using java.awt.image;
using java.util;

namespace ikvm.awt
{

    internal class BitmapGraphics : NetGraphics
    {
        private readonly Bitmap bitmap;
        private readonly BufferedImage image;

        internal BitmapGraphics(Bitmap bitmap, Object destination, java.awt.Font font, Color fgcolor, Color bgcolor)
            : base(createGraphics(bitmap), destination, font, fgcolor, bgcolor)
        {
            this.bitmap = bitmap;
            image = destination as BufferedImage;
        }

        internal BitmapGraphics(Bitmap bitmap, Object destination)
            : this(bitmap, destination, null, Color.White, Color.Black)
        {
        }

        internal override Graphics g
        {
            get {
                if (image != null)
                {
                    image.toBitmap();
                }
                return base.g; 
            }
        }

        protected override SizeF GetSize() {
            return bitmap.Size;
        }

        private static Graphics createGraphics(Bitmap bitmap)
        {
            // lock to prevent the exception
            // System.InvalidOperationException: Object is currently in use elsewhere
            lock (bitmap)
            {
                return Graphics.FromImage(bitmap);
            }
        }

        public override java.awt.Graphics create()
        {
            BitmapGraphics newGraphics = (BitmapGraphics)MemberwiseClone();
            newGraphics.init(createGraphics(bitmap));
            return newGraphics;
        }

        public override void copyArea(int x, int y, int width, int height, int dx, int dy)
		{
            Bitmap copy = new Bitmap(width, height);
            using (Graphics gCopy = Graphics.FromImage(copy))
            {
                gCopy.DrawImage(bitmap, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }
            g.DrawImageUnscaled(copy, x + dx, y + dy);
		}
    }

    internal class ComponentGraphics : NetGraphics
    {
        private readonly Control control;

        internal ComponentGraphics(Control control, java.awt.Component target, java.awt.Color fgColor, java.awt.Color bgColor, java.awt.Font font)
            : base(control.CreateGraphics(), target, font, J2C.ConvertColor(fgColor), J2C.ConvertColor(bgColor))
        {
            this.control = control;
        }

        protected override SizeF GetSize() {
            return control.Size;
        }

        public override java.awt.Graphics create()
        {
            ComponentGraphics newGraphics = (ComponentGraphics)MemberwiseClone();
            newGraphics.init(control.CreateGraphics());
            return newGraphics;
        }

        private Point getPointToScreenImpl(Point point)
        {
            return this.control.PointToScreen(point);
        }

        private Point getPointToScreen(Point point)
        {
            return (Point)this.control.Invoke(new Converter<Point,Point>(getPointToScreenImpl),point);
        }

        public override void copyArea(int x, int y, int width, int height, int dx, int dy)
        {
            Matrix t = g.Transform;
            Point src = getPointToScreen(new Point(x + (int)t.OffsetX, y + (int)t.OffsetY));
            Bitmap copy = new Bitmap(width, height);
            using (Graphics gCopy = Graphics.FromImage(copy))
            {
                gCopy.CopyFromScreen(src, new Point(0, 0), new Size(width, height));
            }
            g.DrawImageUnscaled(copy, x + dx, y + dy);
        }

		public override void clip(java.awt.Shape shape)
		{
			if (shape == null)
			{
				// the API specification says that this will clear
				// the clip, but in fact the reference implementation throws a 
				// NullPointerException - see the following entry in the bug parade:
				// http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6206189
				throw new java.lang.NullPointerException();
			}
			base.clip(shape);
		}
    }

    internal class PrintGraphicsContext
    {
        internal PrintGraphics Current;
    }

    internal class PrintGraphics : NetGraphics
    {
        private NetGraphicsState myState;
        private PrintGraphicsContext baseContext;
        private bool disposed = false;
        private bool isBase = true;

        internal PrintGraphics(Graphics g)
            : base(g, null, null, Color.White, Color.Black)
        {
            baseContext = new PrintGraphicsContext();
            baseContext.Current = this;
        }

        public override java.awt.Graphics create()
        {
            checkState();
            myState = new NetGraphicsState();
            myState.saveGraphics(this);
            PrintGraphics newGraphics = (PrintGraphics)MemberwiseClone();
            newGraphics.myState = null;
            newGraphics.isBase = false;
            newGraphics.baseContext = baseContext;
            baseContext.Current = newGraphics; // since it is very likely that the next op will be on that graphics
            // this is similar to init
            myState.restoreGraphics(newGraphics);
            return newGraphics;
        }

        /// <summary>
        /// Checks whether the properties of this instance are set to the bse Graphics. If not, the context
        /// of the currently PrintGraphics is saved and the context if this instance is restored.
        /// </summary>
        private void checkState()
        {
            // this is required to simulate Graphics.create(), which is not possible in .NET
            // we simply call Save on create() an restore this state, if any method is called
            // on the current graphics. This will work for almost any use case of create()
            if (baseContext != null && baseContext.Current != this)
            {
                if (!baseContext.Current.disposed)
                {
                    if (baseContext.Current.myState == null)
                    {
                        baseContext.Current.myState = new NetGraphicsState(baseContext.Current);
                    }
                    else
                    {
                        baseContext.Current.myState.saveGraphics(baseContext.Current);
                    }
                }
                baseContext.Current = this;
                if (myState != null) // is only null, if this instance was already disposed
                {
                    myState.restoreGraphics(this);
                }
            }
        }

        public override void copyArea(int x, int y, int width, int height, int dx, int dy)
        {
            throw new NotImplementedException();
        }

        public override void clearRect(int x, int y, int width, int height)
        {
            checkState();
            base.clearRect(x, y, width, height);
        }

        public override void clipRect(int x, int y, int w, int h)
        {
            checkState();
            base.clipRect(x, y, w, h);
        }

        public override void clip(java.awt.Shape shape)
        {
            checkState();
            base.clip(shape);
        }

        public override void dispose()
        {            
            myState = null;
            if (pen != null) pen.Dispose();
            if (brush != null) brush.Dispose();
            disposed = true;
            if (!isBase)
            {
                // only dispose the underlying Graphics if this is the base PrintGraphics!
                return;
            }
            base.dispose();
        }

        public override void drawArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
            checkState();
            base.drawArc(x, y, width, height, startAngle, arcAngle);
        }

        public override void drawBytes(byte[] data, int offset, int length, int x, int y)
        {
            checkState();
            base.drawBytes(data, offset, length, x, y);
        }

        public override void drawChars(char[] data, int offset, int length, int x, int y)
        {
            checkState();
            base.drawChars(data, offset, length, x, y);
        }

        public override bool drawImage(java.awt.Image img, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2, java.awt.Color color, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage(img, dx1, dy1, dx2, dy2, sx1, sy1, sx2, sy2, color, observer);
        }

        public override bool drawImage(java.awt.Image img, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage(img, dx1, dy1, dx2, dy2, sx1, sy1, sx2, sy2, observer);
        }

        public override bool drawImage(java.awt.Image img, int x, int y, int width, int height, java.awt.Color bgcolor, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage( img, x, y, width, height, bgcolor, observer);
        }

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.Color bgcolor, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage(img, x, y, bgcolor, observer);
        }

        public override bool drawImage(java.awt.Image img, int x, int y, int width, int height, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage( img, x, y, width, height, observer);
        }

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.image.ImageObserver observer)
        {
            checkState();
            return base.drawImage(img, x, y, observer);
        }

        public override void drawLine(int x1, int y1, int x2, int y2)
        {
            checkState();
            base.drawLine(x1, y1, x2, y2);
        }

        public override void drawOval(int x, int y, int w, int h)
        {
            checkState();
            base.drawOval(x, y, w, h);
        }

        public override void drawPolygon(java.awt.Polygon polygon)
        {
            checkState();
            base.drawPolygon(polygon);
        }

        public override void drawPolygon(int[] aX, int[] aY, int aLength)
        {
            checkState();
            base.drawPolygon(aX, aY, aLength);
        }

        public override void drawPolyline(int[] aX, int[] aY, int aLength)
        {
            checkState();
            base.drawPolyline(aX, aY, aLength);
        }

        public override void drawRect(int x, int y, int width, int height)
        {
            checkState();
            base.drawRect(x, y, width, height);
        }

        public override void drawRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
        {
            checkState();
            base.drawRoundRect(x, y, w, h, arcWidth, arcHeight);
        }

        public override void fill3DRect(int x, int y, int width, int height, bool raised)
        {
            checkState();
            base.fill3DRect(x, y, width, height, raised);
        }

        public override void fillArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
            checkState();
            base.fillArc(x, y, width, height, startAngle, arcAngle);
        }

        public override void fillOval(int x, int y, int w, int h)
        {
            checkState();
            base.fillOval(x, y, w, h);
        }

        public override void fillPolygon(java.awt.Polygon polygon)
        {
            checkState();
            base.fillPolygon(polygon);
        }

        public override void fillPolygon(int[] aX, int[] aY, int aLength)
        {
            checkState();
            base.fillPolygon(aX, aY, aLength);
        }

        public override void fillRect(int x, int y, int width, int height)
        {
            checkState();
            base.fillRect(x, y, width, height);
        }

        public override void fillRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
        {
            checkState();
            base.fillRoundRect(x, y, w, h, arcWidth, arcHeight);
        }

        public override java.awt.Shape getClip()
        {
            checkState();
            return base.getClip();
        }

        public override java.awt.Rectangle getClipBounds(java.awt.Rectangle r)
        {
            checkState();
            return base.getClipBounds( r );
        }

        public override java.awt.Rectangle getClipBounds()
        {
            checkState();
            return base.getClipBounds();
        }

        [Obsolete]
        public override java.awt.Rectangle getClipRect()
        {
            checkState();
            return base.getClipRect();
        }

        public override java.awt.Color getColor()
        {
            checkState();
            return base.getColor();
        }

        public override java.awt.Font getFont()
        {
            checkState();
            return base.getFont();
        }

        public override java.awt.FontMetrics getFontMetrics(java.awt.Font f)
        {
            checkState();
            return base.getFontMetrics(f);
        }

        public override java.awt.FontMetrics getFontMetrics()
        {
            checkState();
            return base.getFontMetrics();
        }

        public override void setClip(int x, int y, int width, int height)
        {
            checkState();
            base.setClip(x,y,width,height);
        }

        public override void setClip(java.awt.Shape shape)
        {
            checkState();
            base.setClip(shape);
        }

        public override void setColor(java.awt.Color color)
        {
            checkState();
            base.setColor(color);
        }

        public override void setFont(java.awt.Font f)
        {
            checkState();
            base.setFont(f);
        }

        public override void setPaintMode()
        {
            checkState();
            base.setPaintMode();
        }

        public override void setXORMode(java.awt.Color param)
        {
            checkState();
            base.setXORMode(param);
        }

        public override void translate(int x, int y)
        {
            checkState();
            base.translate(x, y);
        }

        public override void draw(java.awt.Shape shape)
        {
            checkState();
            base.draw(shape);
        }

        public override bool drawImage(java.awt.Image img, java.awt.geom.AffineTransform xform, ImageObserver observer)
        {
            checkState();
            return base.drawImage(img, xform, observer);
        }

        public override void drawImage(java.awt.image.BufferedImage image, BufferedImageOp op, int x, int y)
        {
            checkState();
            base.drawImage(image, op, x, y);
        }
       
        public override void drawRenderedImage(java.awt.image.RenderedImage img, java.awt.geom.AffineTransform xform)
        {
            checkState();
            base.drawRenderedImage(img, xform);
        }

        public override void drawRenderableImage(java.awt.image.renderable.RenderableImage image, java.awt.geom.AffineTransform xform)
        {
            checkState();
            base.drawRenderableImage(image, xform);
        }

        public override void drawString(string str, int x, int y)
        {
            checkState();
            base.drawString(str, x, y);
        }

        public override void drawString(string text, float x, float y)
        {
            checkState();
            base.drawString(text, x, y);
        }

        public override void drawString(java.text.AttributedCharacterIterator iterator, int x, int y)
        {
            checkState();
            base.drawString(iterator, x, y);
        }

        public override void drawString(java.text.AttributedCharacterIterator iterator, float x, float y)
        {
            checkState();
            base.drawString(iterator, x, y);
        }

        public override void fill(java.awt.Shape shape)
        {
            checkState();
            base.fill(shape);
        }

        public override bool hit(java.awt.Rectangle rect, java.awt.Shape s, bool onStroke)
        {
            checkState();
            return base.hit(rect, s, onStroke);
        }

        public override java.awt.GraphicsConfiguration getDeviceConfiguration()
        {
            // no check here, since invariant
            return base.getDeviceConfiguration();
        }

        public override void setComposite(java.awt.Composite comp)
        {
            checkState();
            base.setComposite(comp);
        }

        public override void setPaint(java.awt.Paint paint)
        {
            checkState();
            base.setPaint(paint);
        }

        public override void setStroke(java.awt.Stroke stroke)
        {
            checkState();
            base.setStroke(stroke);
        }

        public override void setRenderingHint(java.awt.RenderingHints.Key hintKey, Object hintValue)
        {
            checkState();
            base.setRenderingHint(hintKey, hintValue);
        }

        public override object getRenderingHint(java.awt.RenderingHints.Key hintKey)
        {
            checkState();
            return base.getRenderingHint(hintKey);
        }

        public override void setRenderingHints(java.util.Map hints)
        {
            checkState();
            base.setRenderingHints(hints);            
        }

        public override void addRenderingHints(java.util.Map hints)
        {
            checkState();
            base.addRenderingHints(hints);
        }

        public override java.awt.RenderingHints getRenderingHints()
        {
            checkState();
            return base.getRenderingHints();
        }

        public override void translate(double x, double y)
        {
            checkState();
            base.translate(x, y);
        }

        public override void rotate(double theta)
        {
            checkState();
            base.rotate(theta);
        }

        public override void rotate(double theta, double x, double y)
        {
            checkState();
            base.rotate(theta, x, y);
        }

        public override void scale(double scaleX, double scaleY)
        {
            checkState();
            base.scale(scaleX, scaleY);
        }

        public override void shear(double shearX, double shearY)
        {
            checkState();
            base.shear(shearX, shearY);
        }

        public override void transform(java.awt.geom.AffineTransform tx)
        {
            checkState();
            base.transform(tx);
        }

        public override void setTransform(java.awt.geom.AffineTransform tx)
        {
            checkState();
            base.setTransform(tx);
        }

        public override java.awt.geom.AffineTransform getTransform()
        {
            checkState();
            return base.getTransform();
        }

        public override java.awt.Paint getPaint()
        {
            checkState();
            return base.getPaint();
        }

        public override java.awt.Composite getComposite()
        {
            checkState();
            return base.getComposite();
        }

        public override void setBackground(java.awt.Color color)
        {
            checkState();
            base.setBackground(color);
        }

        public override java.awt.Color getBackground()
        {
            checkState();
            return base.getBackground();
        }

        public override java.awt.Stroke getStroke()
        {
            checkState();
            return base.getStroke();
        }

        public override java.awt.font.FontRenderContext getFontRenderContext()
        {
            checkState();
            return base.getFontRenderContext();
        }

        public override void drawGlyphVector(java.awt.font.GlyphVector gv, float x, float y)
        {
            checkState();
            base.drawGlyphVector(gv, x, y);
        }
    }

    /// <summary>
    /// State to store/restore the state of a NetGraphics/Graphics object
    /// </summary>
    internal class NetGraphicsState
    {
        private Brush brush;
        private Pen pen;

        // Graphics State
        private Matrix Transform;
        private Region Clip;
        private SmoothingMode SmoothingMode;
        private PixelOffsetMode PixelOffsetMode;
        private TextRenderingHint TextRenderingHint;
        private InterpolationMode InterpolationMode;
        private CompositingMode CompositingMode;

        private bool savedGraphics = false;

        public NetGraphicsState()
        {
        }

        public NetGraphicsState( NetGraphics netG )
        {
            saveGraphics(netG);
        }

        public void saveGraphics(NetGraphics netG)
        {
            if (netG == null )
            {
                return;
            }
            if (netG.g != null )
            {
                this.Transform = netG.g.Transform;
                this.Clip = netG.g.Clip;
                this.SmoothingMode = netG.g.SmoothingMode;
                this.PixelOffsetMode = netG.g.PixelOffsetMode;
                this.TextRenderingHint = netG.g.TextRenderingHint;
                this.InterpolationMode = netG.g.InterpolationMode;
                this.CompositingMode = netG.g.CompositingMode;
                savedGraphics = true;
            }
            if (netG.pen != null && netG.brush != null)
            {
                pen = (Pen)netG.pen.Clone();
                brush = (Brush)netG.brush.Clone();
            }
        }

        public void restoreGraphics(NetGraphics netG)
        {
            if (netG == null)
            {
                return;
            }
            if (netG.g != null)
            {
                if (savedGraphics)
                {
                    netG.g.Transform = Transform;
                    netG.g.Clip = Clip;
                    netG.g.SmoothingMode = SmoothingMode;
                    netG.g.PixelOffsetMode = PixelOffsetMode;
                    netG.setTextRenderingHint(TextRenderingHint);
                    netG.g.InterpolationMode = InterpolationMode;
                    netG.g.CompositingMode = CompositingMode;
                }
                else
                {
                    // default values that Java used
                    netG.g.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
            }
            if ( pen != null && brush != null )
            {
                netG.pen = (Pen)pen.Clone();
                netG.brush = (Brush)brush.Clone();
            }
            else
            {
                netG.pen = new Pen(netG.color);
                netG.brush = new SolidBrush(netG.color);
                netG.setRenderingHint(java.awt.RenderingHints.KEY_TEXT_ANTIALIASING, java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_DEFAULT);
            }
        }
    }

    internal abstract class NetGraphics : java.awt.Graphics2D//sun.java2d.SunGraphics2D
    {
        private Graphics graphics;
        private java.awt.Color javaColor;
        private java.awt.Paint javaPaint;
        internal Color color;
        private Color bgcolor;
        private java.awt.Font font;
        private java.awt.Stroke stroke;
        private static java.awt.BasicStroke defaultStroke = new java.awt.BasicStroke();
        private Font netfont;
        private int baseline;
        internal Brush brush;
        internal Pen pen;
        private CompositeHelper composite;
        private java.awt.Composite javaComposite = java.awt.AlphaComposite.SrcOver;
        private Object textAntialiasHint;
        private Object fractionalHint = java.awt.RenderingHints.VALUE_FRACTIONALMETRICS_DEFAULT;

        private static System.Collections.Generic.Dictionary<String, Int32> baselines = new System.Collections.Generic.Dictionary<String, Int32>();

        internal static readonly StringFormat FORMAT = new StringFormat(StringFormat.GenericTypographic);
        static NetGraphics()
        {
            FORMAT.FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;
            FORMAT.Trimming = StringTrimming.None;
        }

        protected NetGraphics(Graphics g, Object destination, java.awt.Font font, Color fgcolor, Color bgcolor) //: base( new sun.java2d.SurfaceData(destination) )
        {
            if (font == null)
            {
                font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
            }
            this.font = font;
            netfont = font.getNetFont();
			this.color = fgcolor;
            this.bgcolor = bgcolor;
            composite = CompositeHelper.Create(javaComposite, g);
            init(g);
        }

        /// <summary>
        /// The current C# Graphics
        /// </summary>
        internal virtual Graphics g
        {
            get { return graphics; }
            set { graphics = value; }
        }

        protected void init(Graphics graphics)
        {
            NetGraphicsState state = new NetGraphicsState();
            state.saveGraphics(this);
            g = graphics;
            state.restoreGraphics(this);
        }

        /// <summary>
        /// Get the size of the graphics. This is used as a hind for some hacks.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeF GetSize() {
            return g.ClipBounds.Size;
        }

        public override void clearRect(int x, int y, int width, int height)
        {
            using (Brush br = bgcolor != Color.Empty ? new SolidBrush(bgcolor) : brush)
            {
                CompositingMode tempMode = g.CompositingMode;
                g.CompositingMode = CompositingMode.SourceCopy;
                g.FillRectangle(br, x, y, width, height);
                g.CompositingMode = tempMode;
            }
        }

        public override void clipRect(int x, int y, int w, int h)
        {
            g.IntersectClip(new Rectangle(x, y, w, h));
        }

        public override void clip(java.awt.Shape shape)
        {
            if (shape == null)
            {
				// note that ComponentGraphics overrides clip() to throw a NullPointerException when shape is null
				g.ResetClip();
            }
            else
            {
                g.IntersectClip(new Region(J2C.ConvertShape(shape)));
            }
        }

        public override void dispose()
        {
            if (pen!=null) pen.Dispose();
            if (brush!=null) brush.Dispose();
            graphics.Dispose(); //for dispose we does not need to synchronize the buffer of a bitmap
        }

        public override void drawArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
			g.DrawArc(pen, x, y, width, height, 360 - startAngle - arcAngle, arcAngle);
        }

        public override void drawBytes(byte[] data, int offset, int length, int x, int y)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)data[offset + i];
            }
            drawChars(chars, 0, length, x, y);
        }

        public override void drawChars(char[] data, int offset, int length, int x, int y)
        {
            drawString(new String(data, offset, length), x, y);
        }

        public override bool drawImage(java.awt.Image img, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2, java.awt.Color color, java.awt.image.ImageObserver observer)
        {
            Image image = J2C.ConvertImage(img);
            if (image == null)
            {
                return false;
            }
            Rectangle destRect = new Rectangle(dx1, dy1, dx2 - dx1, dy2 - dy1);
            using (Brush brush = new SolidBrush(composite.GetColor(color))) {
                g.FillRectangle(brush, destRect);
            }
			lock (image)
			{
                g.DrawImage(image, destRect, sx1, sy1, sx2 - sx1, sy2 - sy1, GraphicsUnit.Pixel, composite.GetImageAttributes());
			}
            return true;
        }

        public override bool drawImage(java.awt.Image img, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2, java.awt.image.ImageObserver observer)
        {
            Image image = J2C.ConvertImage(img);
            if (image == null)
            {
                return false;
            }
            Rectangle destRect = new Rectangle(dx1, dy1, dx2 - dx1, dy2 - dy1);
			lock (image)
			{
                g.DrawImage(image, destRect, sx1, sy1, sx2 - sx1, sy2 - sy1, GraphicsUnit.Pixel, composite.GetImageAttributes());
			}
            return true;
        }

        public override bool drawImage(java.awt.Image img, int x, int y, int width, int height, java.awt.Color bgcolor, java.awt.image.ImageObserver observer)
        {
			Image image = J2C.ConvertImage(img);
			if (image == null)
			{
				return false;
			}
            using (Brush brush = new SolidBrush(composite.GetColor(bgcolor))) {
                g.FillRectangle(brush, x, y, width, height);
            }
			lock (image)
			{
                g.DrawImage(image, new Rectangle( x, y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, composite.GetImageAttributes());
			}
			return true;
		}

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.Color bgcolor, java.awt.image.ImageObserver observer)
        {
            if (img == null) {
                return false;
            }
            return drawImage(img, x, y, img.getWidth(observer), img.getHeight(observer), bgcolor, observer);
		}

        public override bool drawImage(java.awt.Image img, int x, int y, int width, int height, java.awt.image.ImageObserver observer)
        {
			Image image = J2C.ConvertImage(img);
			if (image == null)
			{
				return false;
			}
			lock (image)
			{
                g.DrawImage(image, new Rectangle(x, y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, composite.GetImageAttributes());
			}
			return true;
		}

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.image.ImageObserver observer)
        {
            if (img == null) {
                return false;
            }
            return drawImage(img, x, y, img.getWidth(observer), img.getHeight(observer), observer);
		}

        public override void drawLine(int x1, int y1, int x2, int y2)
        {
            // HACK DrawLine doesn't appear to draw the last pixel, so for single pixel lines, we have
            // a freaky workaround
            if (x1 == x2 && y1 == y2)
            {
                g.DrawLine(pen, x1, y1, x1 + 0.01f, y2 + 0.01f);
            }
            else
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        public override void drawOval(int x, int y, int w, int h)
        {
            g.DrawEllipse(pen, x, y, w, h);
        }

        public override void drawPolygon(java.awt.Polygon polygon)
        {
            drawPolygon(polygon.xpoints, polygon.ypoints, polygon.npoints);
        }

        public override void drawPolygon(int[] aX, int[] aY, int aLength)
        {
            Point[] points = new Point[aLength];
            for (int i = 0; i < aLength; i++)
            {
                points[i].X = aX[i];
                points[i].Y = aY[i];
            }
            g.DrawPolygon(pen, points);
        }

        /// <summary>
        /// Draw a sequence of connected lines
        /// </summary>
        /// <param name="aX">Array of x coordinates</param>
        /// <param name="aY">Array of y coordinates</param>
        /// <param name="aLength">Length of coordinate arrays</param>
        public override void drawPolyline(int[] aX, int[] aY, int aLength)
        {
            for (int i = 0; i < aLength - 1; i++)
            {
                Point point1 = new Point(aX[i], aY[i]);
                Point point2 = new Point(aX[i + 1], aY[i + 1]);
                g.DrawLine(pen, point1, point2);
            }
        }

        public override void drawRect(int x, int y, int width, int height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }

        /// <summary>
        /// Apparently there is no rounded rec function in .Net. Draw the
        /// rounded rectangle by using lines and arcs.
        /// </summary>
		public override void drawRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
        {
    	    using (GraphicsPath gp = J2C.ConvertRoundRect(x, y, w, h, arcWidth, arcHeight))
                g.DrawPath(pen, gp);
        }

        public override void fill3DRect(int x, int y, int width, int height, bool raised)
        {
            java.awt.Paint p = getPaint();
            java.awt.Color c = getColor();
            java.awt.Color brighter = c.brighter();
            java.awt.Color darker = c.darker();

            if( !raised ) {
                setColor(darker);
            } else if( p != c ) {
                setColor(c);
            }
            fillRect(x + 1, y + 1, width - 2, height - 2);
            setColor(raised ? brighter : darker);
            fillRect(x, y, 1, height);
            fillRect(x + 1, y, width - 2, 1);
            setColor(raised ? darker : brighter);
            fillRect(x + 1, y + height - 1, width - 1, 1);
            fillRect(x + width - 1, y, 1, height - 1);
            setPaint(p);
        }

        public override void fillArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
			g.FillPie(brush, x, y, width, height, 360 - startAngle - arcAngle, arcAngle);
        }

        public override void fillOval(int x, int y, int w, int h)
        {
            g.FillEllipse(brush, x, y, w, h);
        }

        public override void fillPolygon(java.awt.Polygon polygon)
        {
            fillPolygon(polygon.xpoints, polygon.ypoints, polygon.npoints);
        }

        public override void fillPolygon(int[] aX, int[] aY, int aLength)
        {
            Point[] points = new Point[aLength];
            for (int i = 0; i < aLength; i++)
            {
                points[i].X = aX[i];
                points[i].Y = aY[i];
            }
            g.FillPolygon(brush, points);
        }

        public override void fillRect(int x, int y, int width, int height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }

		public override void fillRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
		{
			GraphicsPath gp = J2C.ConvertRoundRect(x, y, w, h, arcWidth, arcHeight);
			g.FillPath(brush, gp);
			gp.Dispose();
		}

        public override java.awt.Shape getClip()
        {
            return getClipBounds();
        }

        public override java.awt.Rectangle getClipBounds(java.awt.Rectangle r)
        {
            using (Region clip = g.Clip)
            {
                if (!clip.IsInfinite(g))
                {
                    RectangleF rec = clip.GetBounds(g);
                    r.x = (int) rec.X;
                    r.y = (int) rec.Y;
                    r.width = (int) rec.Width;
                    r.height = (int) rec.Height;
                }
                return r;
            }
        }

        public override java.awt.Rectangle getClipBounds()
        {
            using (Region clip = g.Clip)
            {
                if (clip.IsInfinite(g))
                {
                    return null;
                }
                RectangleF rec = clip.GetBounds(g);
                return C2J.ConvertRectangle(rec);
            }
        }

        [Obsolete]
        public override java.awt.Rectangle getClipRect()
        {
            return getClipBounds();
        }

        public override java.awt.Color getColor()
        {
            if (javaColor == null)
            {
                javaColor = composite.GetColor(color);
            }
            return javaColor;
        }

        public override java.awt.Font getFont()
        {
            return font;
        }

        public override java.awt.FontMetrics getFontMetrics(java.awt.Font f)
        {
            return sun.font.FontDesignMetrics.getMetrics(f);
        }

        public override java.awt.FontMetrics getFontMetrics()
        {
            return sun.font.FontDesignMetrics.getMetrics(font);
        }

        public override void setClip(int x, int y, int width, int height)
        {
            g.Clip = new Region(new Rectangle(x, y, width, height));
        }

        public override void setClip(java.awt.Shape shape)
        {
            if (shape == null)
            {
                Region clip = g.Clip;
                clip.MakeInfinite();
                g.Clip = clip;
            }
            else
            {
                g.Clip = new Region(J2C.ConvertShape(shape));
            }
        }

        public override void setColor(java.awt.Color color)
        {
            if (color == null || color == this.javaPaint)
            {
                // Does not change the color, if it is null like in SunGraphics2D
                return;
            }
            this.javaPaint = this.javaColor = color;
            this.color = composite.GetColor(color);
            if (brush is SolidBrush)
            {
                ((SolidBrush)brush).Color = this.color;
            }
            else
            {
                brush.Dispose();
                brush = new SolidBrush(this.color);
            }
            pen.Color = this.color;
            pen.Brush = brush;
        }

        public override void setFont(java.awt.Font f)
        {
            if (f != null && f != font)
            {
                netfont = f.getNetFont();
                font = f;
                baseline = getBaseline( netfont, g.TextRenderingHint );
            }
        }

        public override void setPaintMode()
        {
            throw new NotImplementedException();
        }

        public override void setXORMode(java.awt.Color param)
        {
            if( param == null ) {
                throw new java.lang.IllegalArgumentException("null XORColor");
            }
            throw new NotImplementedException();
        }

        public override void translate(int x, int y)
        {
            Matrix transform = g.Transform;
            transform.Translate(x, y);
            g.Transform = transform;
        }

        public override void draw(java.awt.Shape shape)
        {
            using (GraphicsPath gp = J2C.ConvertShape(shape))
            {
                g.DrawPath(pen, gp);
            }
        }

        public override bool drawImage(java.awt.Image img, java.awt.geom.AffineTransform xform, ImageObserver observer)
        {
            if (img == null) {
                return true;
            }
     
            if (xform == null || xform.isIdentity()) {
                return drawImage(img, 0, 0, null, observer);
            }

            NetGraphics clone = (NetGraphics)create();
            clone.transform(xform);
            bool rendered = clone.drawImage(img, 0, 0, null, observer);
            clone.dispose();
            return rendered;
        }

        public override void drawImage(java.awt.image.BufferedImage image, BufferedImageOp op, int x, int y)
        {

            if( op == null ) {
                drawImage(image, x, y, null);
            } else {
                if( !(op is AffineTransformOp) ) {
                    drawImage(op.filter(image, null), x, y, null);
                } else {
                    Console.WriteLine(new System.Diagnostics.StackTrace());
                    throw new NotImplementedException();
                }
            }
        }

        public override void drawRenderedImage(java.awt.image.RenderedImage img, java.awt.geom.AffineTransform xform)
        {
            if (img == null) {
                return;
            }
    
            // BufferedImage case: use a simple drawImage call
            if (img is BufferedImage) {
                BufferedImage bufImg = (BufferedImage)img;
                drawImage(bufImg,xform,null);
                return;
            }            
            throw new NotImplementedException("drawRenderedImage not implemented for images which are not BufferedImages.");
        }

        public override void drawRenderableImage(java.awt.image.renderable.RenderableImage image, java.awt.geom.AffineTransform xform)
        {
            throw new NotImplementedException();
        }

        public override void drawString(string str, int x, int y)
        {
            drawString(str, (float)x, (float)y);
        }

        public override void drawString(String text, float x, float y) {
            if (text.Length == 0) {
                return;
            }
            CompositingMode origCM = g.CompositingMode;
            try {
                if (origCM != CompositingMode.SourceOver) {
                    // Java has a different behaviar for AlphaComposite and Text Antialiasing
                    g.CompositingMode = CompositingMode.SourceOver;
                }

                bool fractional = isFractionalMetrics();
                if (fractional || !sun.font.StandardGlyphVector.isSimpleString(font, text)) {
                    g.DrawString(text, netfont, brush, x, y - baseline, FORMAT);
                } else {
                    // fixed metric for simple text, we position every character to simulate the Java behaviour
                    java.awt.font.FontRenderContext frc = new java.awt.font.FontRenderContext(null, isAntiAlias(), fractional);
                    sun.font.FontDesignMetrics metrics = sun.font.FontDesignMetrics.getMetrics(font, frc);
                    y -= baseline;
                    for (int i = 0; i < text.Length; i++) {
                        g.DrawString(text.Substring(i, 1), netfont, brush, x, y, FORMAT);
                        x += metrics.charWidth(text[i]);
                    }
                }
            } finally {
                if (origCM != CompositingMode.SourceOver) {
                    g.CompositingMode = origCM;
                }
            }
        }

        public override void drawString(java.text.AttributedCharacterIterator iterator, int x, int y)
        {
            drawString(iterator, (float) x, (float) y);
        }

        public override void drawString(java.text.AttributedCharacterIterator iterator, float x, float y)
        {
            if( iterator == null ) {
                throw new java.lang.NullPointerException("AttributedCharacterIterator is null");
            }
            if( iterator.getBeginIndex() == iterator.getEndIndex() ) {
                return; /* nothing to draw */
            }
            java.awt.font.TextLayout tl = new java.awt.font.TextLayout(iterator, getFontRenderContext());
            tl.draw(this, x, y);
        }

        public override void fill(java.awt.Shape shape)
        {
            g.FillPath(brush, J2C.ConvertShape(shape));
        }

        public override bool hit(java.awt.Rectangle rect, java.awt.Shape s, bool onStroke)
        {
            if (onStroke)
            {
                //TODO use stroke
                //s = stroke.createStrokedShape(s);
            }
            return s.intersects(rect);
        }

        public override java.awt.GraphicsConfiguration getDeviceConfiguration()
        {
			return new NetGraphicsConfiguration(Screen.PrimaryScreen);
        }

        public override void setComposite(java.awt.Composite comp)
        {
            if (javaComposite == comp) {
                return;
            }
            if (comp == null)
            {
                throw new java.lang.IllegalArgumentException("null Composite");
            }
            this.javaComposite = comp;
            java.awt.Paint oldPaint = getPaint(); //getPaint() is never null
            composite = CompositeHelper.Create(comp, g);
            javaPaint = null;
            setPaint(oldPaint);
        }

        public override void setPaint(java.awt.Paint paint)
        {
            if (paint is java.awt.Color)
            {
                setColor((java.awt.Color)paint);
                return;
            }

            if (paint == null || this.javaPaint == paint)
            {
                return;
            }
            this.javaPaint = paint;

            if (paint is java.awt.GradientPaint)
            {
                java.awt.GradientPaint gradient = (java.awt.GradientPaint)paint;
                LinearGradientBrush linear;
                if (gradient.isCyclic())
                {
                    linear = new LinearGradientBrush(
                        J2C.ConvertPoint(gradient.getPoint1()),
                        J2C.ConvertPoint(gradient.getPoint2()),
                        composite.GetColor(gradient.getColor1()),
                        composite.GetColor(gradient.getColor2()));
                }
                else
                {
                    //HACK because .NET does not support continue gradient like Java else Tile Gradient
                    //that we receize the rectangle very large (factor z) and set 4 color values
                    // a exact solution will calculate the size of the Graphics with the current transform
                    Color color1 = composite.GetColor(gradient.getColor1());
                    Color color2 = composite.GetColor(gradient.getColor2());
                    float x1 = (float)gradient.getPoint1().getX();
                    float x2 = (float)gradient.getPoint2().getX();
                    float y1 = (float)gradient.getPoint1().getY();
                    float y2 = (float)gradient.getPoint2().getY();
                    float diffX = x2 - x1;
                    float diffY = y2 - y1;
                    const float z = 60; //HACK zoom factor, with a larger factor .NET will make the gradient wider.
                    linear = new LinearGradientBrush(
                        new PointF(x1 - z * diffX, y1 - z * diffY),
                        new PointF(x2 + z * diffX, y2 + z * diffY),
                        color1,
                        color1);
                    ColorBlend colorBlend = new ColorBlend(4);
                    Color[] colors = colorBlend.Colors;
                    colors[0] = colors[1] = color1;
                    colors[2] = colors[3] = color2;
                    float[] positions = colorBlend.Positions;
                    positions[1] = z / (2 * z + 1);
                    positions[2] = (z + 1) / (2 * z + 1);
                    positions[3] = 1.0f;
                    linear.InterpolationColors = colorBlend;
                }
                linear.WrapMode = WrapMode.TileFlipXY;
                brush = linear;
                pen.Brush = brush;
                return;
            }

            if (paint is java.awt.TexturePaint)
            {
                java.awt.TexturePaint texture = (java.awt.TexturePaint)paint;
                Bitmap txtr = J2C.ConvertImage(texture.getImage());
                java.awt.geom.Rectangle2D anchor = texture.getAnchorRect();
                TextureBrush txtBrush;
                brush = txtBrush = new TextureBrush(txtr, new Rectangle(0, 0, txtr.Width, txtr.Height), composite.GetImageAttributes());
                txtBrush.TranslateTransform((float)anchor.getX(), (float)anchor.getY());
                txtBrush.ScaleTransform((float)anchor.getWidth() / txtr.Width, (float)anchor.getHeight() / txtr.Height);
                txtBrush.WrapMode = WrapMode.Tile;
                pen.Brush = brush;
                return;
            }

            if (paint is java.awt.LinearGradientPaint) {
                java.awt.LinearGradientPaint gradient = (java.awt.LinearGradientPaint)paint;
                PointF start = J2C.ConvertPoint(gradient.getStartPoint());
                PointF end = J2C.ConvertPoint(gradient.getEndPoint());

                java.awt.Color[] javaColors = gradient.getColors();
                ColorBlend colorBlend;
                Color[] colors;
                bool noCycle = gradient.getCycleMethod() == java.awt.MultipleGradientPaint.CycleMethod.NO_CYCLE;
                if (noCycle) {
                    //HACK because .NET does not support continue gradient like Java else Tile Gradient
                    //that we receize the rectangle very large (factor z) and set 2 additional color values
                    //an exact solution will calculate the size of the Graphics with the current transform
                    float diffX = end.X - start.X;
                    float diffY = end.Y - start.Y;
                    SizeF size = GetSize();
                    //HACK zoom factor, with a larger factor .NET will make the gradient wider.
                    float z = Math.Min(10, Math.Max(size.Width / diffX, size.Height / diffY));
                    start.X -= z * diffX;
                    start.Y -= z * diffY;
                    end.X += z * diffX;
                    end.Y += z * diffY;

                    colorBlend = new ColorBlend(javaColors.Length + 2);
                    colors = colorBlend.Colors;
                    float[] fractions = gradient.getFractions();
                    float[] positions = colorBlend.Positions;
                    for (int i = 0; i < javaColors.Length; i++) {
                        colors[i + 1] = composite.GetColor(javaColors[i]);
                        positions[i + 1] = (z + fractions[i]) / (2 * z + 1);
                    }
                    colors[0] = colors[1];
                    colors[colors.Length - 1] = colors[colors.Length - 2];
                    positions[positions.Length - 1] = 1.0f;
                } else {
                    colorBlend = new ColorBlend(javaColors.Length);
                    colors = colorBlend.Colors;
                    colorBlend.Positions = gradient.getFractions();
                    for (int i = 0; i < javaColors.Length; i++) {
                        colors[i] = composite.GetColor(javaColors[i]);
                    }
                }
                LinearGradientBrush linear = new LinearGradientBrush(start, end, colors[0], colors[colors.Length - 1]);
                linear.InterpolationColors = colorBlend;
                switch (gradient.getCycleMethod().ordinal()) {
                    case (int)java.awt.MultipleGradientPaint.CycleMethod.__Enum.NO_CYCLE:
                    case (int)java.awt.MultipleGradientPaint.CycleMethod.__Enum.REFLECT:
                        linear.WrapMode = WrapMode.TileFlipXY;
                        break;
                    case (int)java.awt.MultipleGradientPaint.CycleMethod.__Enum.REPEAT:
                        linear.WrapMode = WrapMode.Tile;
                        break;
                }
                brush = linear;
                pen.Brush = brush;
                return;
            }

            if (paint is java.awt.RadialGradientPaint )
            {
                java.awt.RadialGradientPaint gradient = (java.awt.RadialGradientPaint)paint;
                GraphicsPath path = new GraphicsPath();
                SizeF size = GetSize();

                PointF center = J2C.ConvertPoint(gradient.getCenterPoint());

                float radius = gradient.getRadius();
                int factor = (int)Math.Ceiling(Math.Max(size.Width, size.Height) / radius);

                float diameter = radius * factor;
                path.AddEllipse(center.X - diameter, center.Y - diameter, diameter * 2, diameter * 2);

                java.awt.Color[] javaColors = gradient.getColors();
                float[] fractions = gradient.getFractions();
                int length = javaColors.Length;
                ColorBlend colorBlend = new ColorBlend(length * factor);
                Color[] colors = colorBlend.Colors;
                float[] positions = colorBlend.Positions;

                for (int c = 0, j = length - 1; j >= 0; )
                {
                    positions[c] = (1 - fractions[j]) / factor;
                    colors[c++] = composite.GetColor(javaColors[j--]);
                }

                java.awt.MultipleGradientPaint.CycleMethod.__Enum cycle = (java.awt.MultipleGradientPaint.CycleMethod.__Enum)gradient.getCycleMethod().ordinal();
                for (int f = 1; f < factor; f++)
                {
                    int off = f * length;
                    for (int c = 0, j = length - 1; j >= 0; j--, c++)
                    {
                        switch (cycle)
                        {
                            case java.awt.MultipleGradientPaint.CycleMethod.__Enum.REFLECT:
                                if (f % 2 == 0)
                                {
                                    positions[off + c] = (f + 1 - fractions[j]) / factor;
                                    colors[off + c] = colors[c];
                                }
                                else
                                {
                                    positions[off + c] = (f + fractions[c]) / factor;
                                    colors[off + c] = colors[j];
                                }
                                break;
                            case java.awt.MultipleGradientPaint.CycleMethod.__Enum.NO_CYCLE:
                                positions[off + c] = (f + 1 - fractions[j]) / factor;
                                break;
                            default: //CycleMethod.REPEAT
                                positions[off + c] = (f + 1 - fractions[j]) / factor;
                                colors[off + c] = colors[c];
                                break;
                        }
                    }
                }
                if (cycle == java.awt.MultipleGradientPaint.CycleMethod.__Enum.NO_CYCLE && factor > 1)
                {
                    Array.Copy(colors, 0, colors, colors.Length - length, length);
                    Color color = colors[length - 1];
                    for (int i = colors.Length - length - 1; i >= 0; i--)
                    {
                        colors[i] = color;
                    }
                }

                PathGradientBrush pathBrush = new PathGradientBrush(path);
                pathBrush.CenterPoint = center;
                pathBrush.InterpolationColors = colorBlend;

                brush = pathBrush;
                pen.Brush = brush;
                return;
            }

            //generic paint to brush conversion for custom paints
            //the tranform of the graphics should not change between the creation and it usage
            using (Matrix transform = g.Transform)
            {
                SizeF size = GetSize();
                int width = (int)size.Width;
                int height = (int)size.Height;
                java.awt.Rectangle bounds = new java.awt.Rectangle(0, 0, width, height);

                java.awt.PaintContext context = paint.createContext(ColorModel.getRGBdefault(), bounds, bounds, C2J.ConvertMatrix(transform), getRenderingHints());
                WritableRaster raster = (WritableRaster)context.getRaster(0, 0, width, height);
                BufferedImage txtrImage = new BufferedImage(context.getColorModel(), raster, true, null);
                Bitmap txtr = J2C.ConvertImage(txtrImage);

                TextureBrush txtBrush;
                brush = txtBrush = new TextureBrush(txtr, new Rectangle(0, 0, width, height), composite.GetImageAttributes());
                transform.Invert();
                txtBrush.Transform = transform;
                txtBrush.WrapMode = WrapMode.Tile;
                pen.Brush = brush;
                return;
            }
        }

		public override void setStroke(java.awt.Stroke stroke)
		{
			if (this.stroke != null && this.stroke.Equals(stroke))
			{
				return;
			}
			this.stroke = stroke;
            if (stroke is java.awt.BasicStroke)
            {
                java.awt.BasicStroke s = (java.awt.BasicStroke)stroke;

                pen = new Pen(pen.Brush, s.getLineWidth());

                SetLineJoin(s);
                SetLineDash(s);
            }
            else
            {
                Console.WriteLine("Unknown Stroke type: " + stroke.GetType().FullName);
            }
		}

        private void SetLineJoin(java.awt.BasicStroke s)
        {
            pen.MiterLimit = s.getMiterLimit();
			pen.LineJoin = J2C.ConvertLineJoin(s.getLineJoin());
        }

        private void SetLineDash(java.awt.BasicStroke s)
        {
            float[] dash = s.getDashArray();
            if (dash == null)
            {
                pen.DashStyle = DashStyle.Solid;
            } else {
                if (dash.Length % 2 == 1)
                {
                    int len = dash.Length;
                    Array.Resize(ref dash, len * 2);
                    Array.Copy(dash, 0, dash, len, len);
                }
                float lineWidth = s.getLineWidth();
                if (lineWidth > 1) // for values < 0 there is no correctur needed
                {
                    for (int i = 0; i < dash.Length; i++)
                    {
                        //dividing by line thickness because of the representation difference
                        dash[i] = dash[i] / lineWidth;
                    }
                }
                // To fix the problem where solid style in Java can be represented at { 1.0, 0.0 }.
                // In .NET, however, array can only have positive value
                if (dash.Length == 2 && dash[dash.Length - 1] == 0)
                {
                    Array.Resize(ref dash, 1);
                }

                float dashPhase = s.getDashPhase();
                // correct the dash cap
                switch (s.getEndCap())
                {
                    case java.awt.BasicStroke.CAP_BUTT:
                        pen.DashCap = DashCap.Flat;
                        break;
                    case java.awt.BasicStroke.CAP_ROUND:
                        pen.DashCap = DashCap.Round;
                        break;
                    case java.awt.BasicStroke.CAP_SQUARE:
                        pen.DashCap = DashCap.Flat;
                        // there is no equals DashCap in .NET, we need to emulate it
                        dashPhase += lineWidth / 2;
                        for (int i = 0; i < dash.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                dash[i] += 1;
                            }
                            else
                            {
                                dash[i] = Math.Max(0.00001F, dash[i] - 1);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown dash cap type:" + s.getEndCap());
                        break;
                }

                // calc the dash offset
                if (lineWidth > 0)
                {
                    //dividing by line thickness because of the representation difference
                    pen.DashOffset = dashPhase / lineWidth;
                }
                else
                {
                    // thickness == 0
                    if (dashPhase > 0)
                    {
                        pen.Width = lineWidth = 0.001F; // hack to prevent a division with 0
                        pen.DashOffset = dashPhase / lineWidth;
                    }
                    else
                    {
                        pen.DashOffset = 0;
                    }
                }

                // set the final dash pattern 
                pen.DashPattern = dash;
            }
        }

        public override void setRenderingHint(java.awt.RenderingHints.Key hintKey, Object hintValue)
        {
            if (hintKey == java.awt.RenderingHints.KEY_ANTIALIASING)
            {
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_DEFAULT)
                {
                    g.SmoothingMode = SmoothingMode.Default;
                    g.PixelOffsetMode = PixelOffsetMode.Default;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_OFF)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = PixelOffsetMode.Default;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_ON)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    return;
                }
                return;
            }
            if (hintKey == java.awt.RenderingHints.KEY_INTERPOLATION)
            {
                if (hintValue == java.awt.RenderingHints.VALUE_INTERPOLATION_BILINEAR)
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_INTERPOLATION_BICUBIC)
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_INTERPOLATION_NEAREST_NEIGHBOR)
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    return;
                }
                return;
            }
            if (hintKey == java.awt.RenderingHints.KEY_TEXT_ANTIALIASING)
            {
                if (hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_DEFAULT ||
                    hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_OFF)
                {
                    setTextRenderingHint(TextRenderingHint.SingleBitPerPixelGridFit);
                    textAntialiasHint = hintValue;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_ON)
                {
                    setTextRenderingHint(TextRenderingHint.AntiAlias);
                    textAntialiasHint = hintValue;
                    return;
                }
                return;
            }
            if (hintKey == java.awt.RenderingHints.KEY_FRACTIONALMETRICS) 
            {
                if (hintValue == java.awt.RenderingHints.VALUE_FRACTIONALMETRICS_DEFAULT || 
                    hintValue == java.awt.RenderingHints.VALUE_FRACTIONALMETRICS_OFF ||
                    hintValue == java.awt.RenderingHints.VALUE_FRACTIONALMETRICS_ON) 
                {
                    fractionalHint = hintValue;
                }
                return;
            }

        }

        public override object getRenderingHint(java.awt.RenderingHints.Key hintKey)
        {
            return getRenderingHints().get(hintKey);
        }

        public override void setRenderingHints(java.util.Map hints)
        {
            addRenderingHints(hints);
            //TODO all not included values should reset to default, but was is default?
        }

        public override void addRenderingHints(java.util.Map hints)
        {
            Iterator iterator = hints.entrySet().iterator();
            while (iterator.hasNext())
            {
                java.util.Map.Entry entry = (java.util.Map.Entry)iterator.next();
                setRenderingHint((java.awt.RenderingHints.Key)entry.getKey(), entry.getValue());
            }
        }

        public override java.awt.RenderingHints getRenderingHints()
        {
            java.awt.RenderingHints hints = new java.awt.RenderingHints(null);
            switch (g.SmoothingMode)
            {
                case SmoothingMode.Default:
                    hints.put(java.awt.RenderingHints.KEY_ANTIALIASING, java.awt.RenderingHints.VALUE_ANTIALIAS_DEFAULT);
                    break;
                case SmoothingMode.None:
                    hints.put(java.awt.RenderingHints.KEY_ANTIALIASING, java.awt.RenderingHints.VALUE_ANTIALIAS_OFF);
                    break;
                case SmoothingMode.AntiAlias:
                    hints.put(java.awt.RenderingHints.KEY_ANTIALIASING, java.awt.RenderingHints.VALUE_ANTIALIAS_ON);
                    break;
            }

            switch (g.InterpolationMode)
            {
                case InterpolationMode.Bilinear:
                case InterpolationMode.HighQualityBilinear:
                    hints.put(java.awt.RenderingHints.KEY_INTERPOLATION, java.awt.RenderingHints.VALUE_INTERPOLATION_BILINEAR);
                    break;
                case InterpolationMode.Bicubic:
                case InterpolationMode.HighQualityBicubic:
                    hints.put(java.awt.RenderingHints.KEY_INTERPOLATION, java.awt.RenderingHints.VALUE_INTERPOLATION_BICUBIC);
                    break;
                case InterpolationMode.NearestNeighbor:
                    hints.put(java.awt.RenderingHints.KEY_INTERPOLATION, java.awt.RenderingHints.VALUE_INTERPOLATION_NEAREST_NEIGHBOR);
                    break;
            }

            hints.put(java.awt.RenderingHints.KEY_TEXT_ANTIALIASING, textAntialiasHint);
            hints.put(java.awt.RenderingHints.KEY_FRACTIONALMETRICS, fractionalHint);
            return hints;
        }

        public override void translate(double x, double y)
        {
            Matrix transform = g.Transform;
            transform.Translate((float)x, (float)y);
            g.Transform = transform;
        }

		private static double RadiansToDegrees(double radians)
		{
			return radians * (180 / Math.PI);
		}

        public override void rotate(double theta)
        {
            Matrix transform = g.Transform;
            transform.Rotate((float)RadiansToDegrees(theta));
            g.Transform = transform;
        }

        public override void rotate(double theta, double x, double y)
        {
            Matrix transform = g.Transform;
            transform.Translate((float)x, (float)y);
			transform.Rotate((float)RadiansToDegrees(theta));
            transform.Translate(-(float)x, -(float)y);
            g.Transform = transform;
        }

        public override void scale(double scaleX, double scaleY)
        {
            using (Matrix transform = g.Transform)
            {
                transform.Scale((float)scaleX, (float)scaleY);
                g.Transform = transform;
            }
        }

        public override void shear(double shearX, double shearY)
        {
            using (Matrix transform = g.Transform)
            {
                transform.Shear((float)shearX, (float)shearY);
                g.Transform = transform;
            }
        }

        public override void transform(java.awt.geom.AffineTransform tx)
        {
            using (Matrix transform = g.Transform,
                matrix = J2C.ConvertTransform(tx))
            {
                transform.Multiply(matrix);
                g.Transform = transform;
            }
        }

        public override void setTransform(java.awt.geom.AffineTransform tx)
        {
            g.Transform = J2C.ConvertTransform(tx);
        }

        public override java.awt.geom.AffineTransform getTransform()
        {
            using (Matrix matrix = g.Transform)
            {
                return C2J.ConvertMatrix(matrix);
            }
        }

        public override java.awt.Paint getPaint()
        {
            if( javaPaint == null ) {
                javaPaint = composite.GetColor(color);
            }
            return javaPaint;
        }

        public override java.awt.Composite getComposite()
        {
            return javaComposite;
        }

        public override void setBackground(java.awt.Color backcolor)
        {
            bgcolor = backcolor == null ? Color.Empty : Color.FromArgb(backcolor.getRGB());
        }

        public override java.awt.Color getBackground()
        {
            return bgcolor == Color.Empty ? null : new java.awt.Color(bgcolor.ToArgb(), true);
        }

        public override java.awt.Stroke getStroke()
        {
            if (stroke == null)
            {
                return defaultStroke;
            }
            return stroke; 
        }

        internal void setTextRenderingHint(TextRenderingHint hint)
        {
            g.TextRenderingHint = hint;
            baseline = getBaseline(netfont, hint);
        }

        /// <summary>
        /// Caclulate the baseline from a font and a TextRenderingHint
        /// </summary>
        /// <param name="font">the font</param>
        /// <param name="hint">the used TextRenderingHint</param>
        /// <returns></returns>
        private static int getBaseline(Font font, TextRenderingHint hint)
        {
            lock (baselines)
            {
                String key = font.ToString() + hint.ToString();
                int baseline;
                if (!baselines.TryGetValue(key, out baseline))
                {
                    FontFamily family = font.FontFamily;
                    FontStyle style = font.Style;
                    float ascent = family.GetCellAscent(style);
                    float lineSpace = family.GetLineSpacing(style);

                    baseline = (int)Math.Round(font.GetHeight() * ascent / lineSpace);

                    // Until this point the calulation use only the Font. But with different TextRenderingHint there are smal differences.
                    // There is no API that calulate the offset from TextRenderingHint that we messure it.
                    const int w = 3;
                    const int h = 3;

                    Bitmap bitmap = new Bitmap(w, h);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.TextRenderingHint = hint;
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
                    g.DrawString("A", font, new SolidBrush(Color.Black), 0, -baseline, FORMAT);
                    g.DrawString("X", font, new SolidBrush(Color.Black), 0, -baseline, FORMAT);
                    g.Dispose();

                    int y = 0;
                LINE:
                    while (y < h)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            Color color = bitmap.GetPixel(x, y);
                            if (color.GetBrightness() < 0.5)
                            {
                                //there is a black pixel, we continue in the next line.
                                baseline++;
                                y++;
                                goto LINE;
                            }
                        }
                        break; // there was a line without black pixel
                    }


                    baselines[key] = baseline;
                }
                return baseline;
            }
        }

        private bool isAntiAlias()
        {
            switch (g.TextRenderingHint)
            {
                case TextRenderingHint.AntiAlias:
                case TextRenderingHint.AntiAliasGridFit:
                case TextRenderingHint.ClearTypeGridFit:
                    return true;
                default:
                    return false;
            }
        }

        private bool isFractionalMetrics()
        {
            return fractionalHint == java.awt.RenderingHints.VALUE_FRACTIONALMETRICS_ON; 
        }

        public override java.awt.font.FontRenderContext getFontRenderContext()
        {
            return new java.awt.font.FontRenderContext(getTransform(), isAntiAlias(), isFractionalMetrics());
        }

        public override void drawGlyphVector(java.awt.font.GlyphVector gv, float x, float y)
        {
            java.awt.font.FontRenderContext frc = gv.getFontRenderContext();
            Matrix currentMatrix = null;
            Font currentFont = netfont;
            TextRenderingHint currentHint = g.TextRenderingHint;
            int currentBaseline = baseline;
            try
            {
                java.awt.Font javaFont = gv.getFont();
                if (javaFont != null)
                {
                    netfont = javaFont.getNetFont();
                }
                TextRenderingHint hint;
                if (frc.isAntiAliased()) {
                    if( frc.usesFractionalMetrics() ){
                        hint = TextRenderingHint.AntiAlias;
                    } else {
                        hint = TextRenderingHint.AntiAliasGridFit;
                    }
                } else {
                    if (frc.usesFractionalMetrics()) {
                        hint = TextRenderingHint.SingleBitPerPixel;
                    } else {
                        hint = TextRenderingHint.SingleBitPerPixelGridFit;
                    }
                }
                g.TextRenderingHint = hint;
                baseline = getBaseline(netfont, hint);
                if (!frc.getTransform().equals(getTransform()))
                {
                    // save the old context and use the transformation from the renderContext
                    currentMatrix = g.Transform;
                    g.Transform = J2C.ConvertTransform(frc.getTransform());
                }
                drawString(J2C.ConvertGlyphVector(gv), x, y);
            }
            finally
            {
                // Restore the old context if needed
                g.TextRenderingHint = currentHint;
                baseline = currentBaseline;
                netfont = currentFont;
                if (currentMatrix != null)
                {
                    g.Transform = currentMatrix;
                }
            }
        }
    }

    sealed class NetGraphicsConfiguration : java.awt.GraphicsConfiguration
    {
        internal readonly Screen screen;

        public NetGraphicsConfiguration(Screen screen)
        {
            this.screen = screen;
        }

        public override java.awt.image.BufferedImage createCompatibleImage(int width, int height, int transparency)
        {
            switch (transparency)
            {
                case java.awt.Transparency.__Fields.OPAQUE:
                    return new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);
                case java.awt.Transparency.__Fields.BITMASK:
                    return new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB_PRE);
                case java.awt.Transparency.__Fields.TRANSLUCENT:
                    return new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
                default:
                    throw new java.lang.IllegalArgumentException("transparency:" + transparency);
            }
        }

        public override java.awt.image.BufferedImage createCompatibleImage(int width, int height)
        {
            return new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
        }

        public override java.awt.image.VolatileImage createCompatibleVolatileImage(int param1, int param2, java.awt.ImageCapabilities param3)
        {
            throw new NotImplementedException();
        }

        public override java.awt.image.VolatileImage createCompatibleVolatileImage(int width, int height)
        {
            return new NetVolatileImage(width, height);
        }

        public override java.awt.Rectangle getBounds()
        {
            System.Drawing.Rectangle bounds = screen.Bounds;
            return new java.awt.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public override java.awt.BufferCapabilities getBufferCapabilities()
        {
            throw new NotImplementedException();
        }

        public override java.awt.image.ColorModel getColorModel(int transparency)
        {
            if (transparency == java.awt.Transparency.__Fields.TRANSLUCENT)
            {
                //we return the default ColorModel because this produce the fewest problems with convertions
                return ColorModel.getRGBdefault();
            }
            else
            {
                return null;
            }
        }

        public override java.awt.image.ColorModel getColorModel()
        {
            //we return the default ColorModel because this produce the fewest problems with convertions
            return ColorModel.getRGBdefault();
        }

        public override java.awt.geom.AffineTransform getDefaultTransform()
        {
            return new java.awt.geom.AffineTransform();
        }

        public override java.awt.GraphicsDevice getDevice()
        {
            return new NetGraphicsDevice(screen);
        }

        public override java.awt.ImageCapabilities getImageCapabilities()
        {
            throw new NotImplementedException();
        }

        public override java.awt.geom.AffineTransform getNormalizingTransform()
        {
            throw new NotImplementedException();
        }

        public override VolatileImage createCompatibleVolatileImage(int width, int height, int transparency)
        {
            return new NetVolatileImage(width, height);
        }

        public override VolatileImage createCompatibleVolatileImage(int width, int height, java.awt.ImageCapabilities caps, int transparency)
        {
            return new NetVolatileImage(width, height);
        }

        public override bool isTranslucencyCapable()
        {
            return true;
        }
    }

    class NetGraphicsDevice : java.awt.GraphicsDevice
    {
        internal readonly Screen screen;

        internal NetGraphicsDevice(Screen screen)
        {
            this.screen = screen;
        }

        public override java.awt.GraphicsConfiguration[] getConfigurations()
        {
            Screen[] screens = Screen.AllScreens;
            NetGraphicsConfiguration[] configs = new NetGraphicsConfiguration[screens.Length];
            for (int i = 0; i < screens.Length; i++)
            {
                configs[i] = new NetGraphicsConfiguration(screens[i]);
            }
            return configs;
        }

        public override java.awt.GraphicsConfiguration getDefaultConfiguration()
        {
            return new NetGraphicsConfiguration(screen);
        }

        public override string getIDstring()
        {
            return screen.DeviceName;
        }

        public override int getType()
        {
            return TYPE_RASTER_SCREEN;
        }
    }

    public class NetGraphicsEnvironment : sun.java2d.SunGraphicsEnvironment
    {

        public override bool isDisplayLocal()
        {
            return true;
        }

        // Create a bitmap with the dimensions of the argument image. Then
        // create a graphics objects from the bitmap. All paint operations will
        // then paint the bitmap.
		public override java.awt.Graphics2D createGraphics(BufferedImage bi)
		{
			return new BitmapGraphics(bi.getBitmap(), bi );
		}

        public override java.awt.Font[] getAllFonts()
        {
#if WINFX  
            System.Collections.Generic.ICollection<Typeface> typefaces = System.Windows.Media.Fonts.SystemTypefaces;
            java.awt.Font[] fonts = new java.awt.Font[typefaces.Count];
            int i = 0;
            foreach (Typeface face in typefaces)
            {
                FontFamily family = face.FontFamily;
                fonts[i++] = new java.awt.Font(family.GetName(0), face.Style, 1);
            }
#else
            String[] names = getAvailableFontFamilyNames();
            java.awt.Font[] fonts = new java.awt.Font[names.Length];
            for(int i=0; i<fonts.Length; i++)
            {
                fonts[i] = new java.awt.Font(names[i], 0, 1);
            }
            return fonts;
#endif
        }

        public override String[] getAvailableFontFamilyNames()
        {
            int language = CultureInfo.CurrentCulture.LCID;
            return getAvailableFontFamilyNames(language);
        }

        public override string[] getAvailableFontFamilyNames(Locale locale)
        {
            int language = CultureInfo.GetCultureInfo(locale.toString()).LCID;
            return getAvailableFontFamilyNames(language);
        }

        private String[] getAvailableFontFamilyNames(int language)
        {
			FontFamily[] families = FontFamily.Families;
            String[] results = new String[families.Length + 5];
            int i = 0;
            for (; i < families.Length; i++)
            {
                results[i] = families[i].GetName(language);
            }
            results[i++] = "Dialog";
            results[i++] = "DialogInput";
            results[i++] = "Serif";
            results[i++] = "SansSerif";
            results[i++] = "Monospaced";
            Array.Sort(results);
            return results;
        }

        public override java.awt.GraphicsDevice getDefaultScreenDevice()
        {
            return new NetGraphicsDevice(Screen.PrimaryScreen);
        }

        public override java.awt.GraphicsDevice[] getScreenDevices()
        {
            Screen[] screens = Screen.AllScreens;
            NetGraphicsDevice[] devices = new NetGraphicsDevice[screens.Length];
            for (int i = 0; i < screens.Length; i++)
            {
                devices[i] = new NetGraphicsDevice(screens[i]);
            }
            return devices;
        }
    }

}