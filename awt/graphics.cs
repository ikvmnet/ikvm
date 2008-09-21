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

        internal BitmapGraphics(Bitmap bitmap)
            : base(Graphics.FromImage(bitmap), null, Color.White)
        {
            this.bitmap = bitmap;
        }

        public override java.awt.Graphics create()
        {
            BitmapGraphics newGraphics = (BitmapGraphics)MemberwiseClone();
            newGraphics.init(Graphics.FromImage(bitmap));
            return newGraphics;
        }

		public override void copyArea(int x, int y, int width, int height, int dx, int dy)
		{
		}
    }

    internal class ComponentGraphics : NetGraphics
    {
        private readonly Control control;

        internal ComponentGraphics(NetComponentPeer peer)
            : base(peer.control.CreateGraphics(), peer.component.getFont(), peer.control.BackColor)
        {
            control = peer.control;
        }

        public override java.awt.Graphics create()
        {
            ComponentGraphics newGraphics = (ComponentGraphics)MemberwiseClone();
            newGraphics.init(control.CreateGraphics());
            return newGraphics;
        }

		public override void copyArea(int x, int y, int width, int height, int dx, int dy)
		{
			throw new NotImplementedException();
		}
    }

    internal abstract class NetGraphics : java.awt.Graphics2D
    {
        protected Graphics g;
        private java.awt.Color javaColor;
        private java.awt.Paint javaPaint;
        private Color color = SystemColors.WindowText;
        private Color bgcolor;
        private java.awt.Font font;
        private java.awt.Stroke stroke;
        private static java.awt.BasicStroke defaultStroke = new java.awt.BasicStroke();
        private Font netfont;
        private Brush brush;
        private Pen pen;

        protected NetGraphics(Graphics g, java.awt.Font font, Color bgcolor)
        {
            if (font == null)
            {
                font = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
            }
            this.font = font;
            netfont = ((NetFontPeer)font.getPeer()).netFont;
            this.bgcolor = bgcolor;
            init(g);
        }

        protected void init(Graphics graphics)
        {
            if (g != null)
            {
                //Transfer the state from the original graphics
                //occur on call of create()
                graphics.Transform = g.Transform;
                graphics.Clip = g.Clip;
                graphics.SmoothingMode = g.SmoothingMode;
                graphics.TextRenderingHint = g.TextRenderingHint;
                graphics.InterpolationMode = g.InterpolationMode;
            }
            g = graphics;
            brush = new SolidBrush(color);
            pen = new Pen(color);
        }

        public override void clearRect(int x, int y, int width, int height)
        {
            using (Brush br = new SolidBrush(bgcolor))
            {
                g.FillRectangle(br, x, y, width, height);
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
                // the API specification says that this will clear
                // the clip, but in fact the reference implementation throws a 
                // NullPointerException - see the following entry in the bug parade:
                // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6206189
                throw new java.lang.NullPointerException();
            }
            else
            {
                g.IntersectClip(new Region(J2C.ConvertShape(shape)));
            }
        }

        public override void dispose()
        {
            g.Dispose();
        }

        public override void drawArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
            g.DrawArc(pen, x, y, width, height, startAngle, arcAngle);
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
            Rectangle srcRect = new Rectangle(sx1, sy1, sx2 - sx1, sy2 - sy1);
            using (Brush brush = J2C.CreateBrush(color))
            {
                g.FillRectangle(brush, destRect);
            }
			lock (image)
			{
				g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
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
            Rectangle srcRect = new Rectangle(sx1, sy1, sx2 - sx1, sy2 - sy1);
			lock (image)
			{
				g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
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
			using (Brush brush = J2C.CreateBrush(bgcolor))
			{
				g.FillRectangle(brush, x, y, width, height);
			}
			lock (image)
			{
				g.DrawImage(image, x, y, width, height);
			}
			return true;
		}

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.Color bgcolor, java.awt.image.ImageObserver observer)
        {
			Image image = J2C.ConvertImage(img);
			if (image == null)
			{
				return false;
			}
			using (Brush brush = J2C.CreateBrush(bgcolor))
			{
				g.FillRectangle(brush, x, y, image.Width, image.Height);
			}
			lock (image)
			{
				g.DrawImage(image, x, y);
			}
			return true;
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
				g.DrawImage(image, x, y, width, height);
			}
			return true;
		}

        public override bool drawImage(java.awt.Image img, int x, int y, java.awt.image.ImageObserver observer)
        {
			Image image = J2C.ConvertImage(img);
			if (image == null)
			{
				return false;
			}
			lock (image)
			{
				g.DrawImage(image, x, y);
			}
			return true;
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
        public override void drawRoundRect(int x, int y, int w, int h, int radius, int param6)
        {
            using (GraphicsPath gp = J2C.ConvertRoundRect(x, y, w, h, radius))
                g.DrawPath(pen, gp);
        }

        public override void drawString(java.text.AttributedCharacterIterator param1, int param2, int param3)
        {
            throw new NotImplementedException();
        }

        public override void drawString(string str, int x, int y)
        {
            int descent = netfont.FontFamily.GetCellDescent(netfont.Style);
            int descentPixel = (int)Math.Round(netfont.Size * descent / netfont.FontFamily.GetEmHeight(netfont.Style));
			g.DrawString(str, netfont, brush, x, y - netfont.Height + descentPixel);
        }

        public override void fill3DRect(int param1, int param2, int param3, int param4, bool param5)
        {
            throw new NotImplementedException();
        }

        public override void fillArc(int x, int y, int width, int height, int startAngle, int arcAngle)
        {
            g.FillPie(brush, x, y, width, height, startAngle, arcAngle);
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

        public override void fillRoundRect(int x, int y, int w, int h, int radius, int param6)
        {
            GraphicsPath gp = J2C.ConvertRoundRect(x, y, w, h, radius);
            g.FillPath(brush, gp);
            gp.Dispose();
        }

        public override java.awt.Shape getClip()
        {
            return getClipBounds();
        }

        public override java.awt.Rectangle getClipBounds(java.awt.Rectangle r)
        {
            Region clip = g.Clip;
            if (!clip.IsInfinite(g))
            {
                RectangleF rec = clip.GetBounds(g);
                r.x = (int)rec.X;
                r.y = (int)rec.Y;
                r.width = (int)rec.Width;
                r.height = (int)rec.Height;
            }
            return r;
        }

        public override java.awt.Rectangle getClipBounds()
        {
            Region clip = g.Clip;
            if (clip.IsInfinite(g))
            {
                return null;
            }
            RectangleF rec = clip.GetBounds(g);
            return C2J.ConvertRectangle(rec);
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
                javaColor = new java.awt.Color(color.ToArgb());
            }
            return javaColor;
        }

        public override java.awt.Font getFont()
        {
            return font;
        }

        public override java.awt.FontMetrics getFontMetrics(java.awt.Font f)
        {
            return new NetFontMetrics(f);
        }

        public override java.awt.FontMetrics getFontMetrics()
        {
            return new NetFontMetrics(font);
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
            if (color == null)
            {
                // TODO is this the correct default color?
                color = java.awt.SystemColor.controlText;
                //throw new java.lang.IllegalArgumentException("Color can't be null");
            }
            this.javaPaint = this.javaColor = color;
            this.color = Color.FromArgb(color.getRGB());
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
            // TODO why is Component calling us with a null reference and is this legal?
            if (f != null)
            {
                netfont = ((NetFontPeer)f.getPeer()).netFont;
                font = f;
            }
            else
            {
                Console.WriteLine("Font is null");
                Console.WriteLine(new System.Diagnostics.StackTrace());
            }
        }

        public override void setPaintMode()
        {
            throw new NotImplementedException();
        }

        public override void setXORMode(java.awt.Color param)
        {
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

        public override bool drawImage(java.awt.Image image, java.awt.geom.AffineTransform xform, ImageObserver obs)
        {
            Console.WriteLine(new System.Diagnostics.StackTrace());
            throw new NotImplementedException();
        }

        public override void drawImage(java.awt.image.BufferedImage image, BufferedImageOp op, int x, int y)
        {
            Console.WriteLine(new System.Diagnostics.StackTrace());
            throw new NotImplementedException();
        }

        public override void drawRenderedImage(java.awt.image.RenderedImage image, java.awt.geom.AffineTransform xform)
        {
            throw new NotImplementedException();
        }

        public override void drawRenderableImage(java.awt.image.renderable.RenderableImage image, java.awt.geom.AffineTransform xform)
        {
            throw new NotImplementedException();
        }

        public override void drawString(string text, float x, float y)
        {
            g.DrawString(text, netfont, brush, x, y);
        }

        public override void drawString(java.text.AttributedCharacterIterator iterator, float x, float y)
        {
            throw new NotImplementedException();
        }

        public override void fill(java.awt.Shape shape)
        {
            using (Region region = new Region(J2C.ConvertShape(shape)))
            {
                g.FillRegion(brush, region);
            }
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
            throw new NotImplementedException();
        }

        public override void setComposite(java.awt.Composite comp)
        {
            throw new NotImplementedException();
        }

        public override void setPaint(java.awt.Paint paint)
        {
            if (paint is java.awt.Color)
            {
                setColor((java.awt.Color)paint);
                return;
            }

            if (this.javaPaint == paint)
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
                        J2C.ConvertColor(gradient.getColor1()),
                        J2C.ConvertColor(gradient.getColor2()));
                }
                else
                {
                    //HACK because .NET does not support continue gradient like Java else Tile Gradient
                    //that we receize the rectangle very large (factor z) and set 4 color values
                    // a exact solution will calculate the size of the Graphics with the current transform
                    Color color1 = J2C.ConvertColor(gradient.getColor1());
                    Color color2 = J2C.ConvertColor(gradient.getColor2());
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
                brush = new TextureBrush(
                    J2C.ConvertImage(texture.getImage()),
                    J2C.ConvertRect(texture.getAnchorRect()));
                pen.Brush = brush;
                return;
            }

            throw new NotImplementedException("setPaint("+paint.GetType().FullName+")");
        }

        public override void setStroke(java.awt.Stroke stroke)
        {
            if (defaultStroke.equals(stroke))
            {
                stroke = null;
                return;
            }
            this.stroke = stroke;
        }

        public override void setRenderingHint(java.awt.RenderingHints.Key hintKey, Object hintValue)
        {
            if (hintKey == java.awt.RenderingHints.KEY_ANTIALIASING)
            {
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_DEFAULT)
                {
                    g.SmoothingMode = SmoothingMode.Default;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_OFF)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_ANTIALIAS_ON)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    return;
                }
                return;
            }
            if (hintKey == java.awt.RenderingHints.KEY_INTERPOLATION)
            {
                if (hintValue == java.awt.RenderingHints.VALUE_INTERPOLATION_BILINEAR)
                {
                    g.InterpolationMode = InterpolationMode.Bilinear;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_INTERPOLATION_BICUBIC)
                {
                    g.InterpolationMode = InterpolationMode.Bicubic;
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
                if (hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_DEFAULT)
                {
                    g.TextRenderingHint = TextRenderingHint.SystemDefault;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_OFF)
                {
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    return;
                }
                if (hintValue == java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_ON)
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    return;
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

            switch (g.TextRenderingHint)
            {
                case TextRenderingHint.SystemDefault:
                    hints.put(java.awt.RenderingHints.KEY_TEXT_ANTIALIASING, java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_DEFAULT);
                    break;
                case TextRenderingHint.SingleBitPerPixelGridFit:
                case TextRenderingHint.SingleBitPerPixel:
                    hints.put(java.awt.RenderingHints.KEY_TEXT_ANTIALIASING, java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_OFF);
                    break;
                case TextRenderingHint.AntiAlias:
                case TextRenderingHint.AntiAliasGridFit:
                    hints.put(java.awt.RenderingHints.KEY_TEXT_ANTIALIASING, java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                    break;
            }
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
            return javaPaint;
        }

        public override java.awt.Composite getComposite()
        {
            throw new NotImplementedException();
        }

        public override void setBackground(java.awt.Color color)
        {
            bgcolor = Color.FromArgb(color.getRGB());
        }

        public override java.awt.Color getBackground()
        {
            return new java.awt.Color(bgcolor.ToArgb());
        }

        public override java.awt.Stroke getStroke()
        {
            if (stroke == null)
            {
                return defaultStroke;
            }
            return stroke;
        }

        public override java.awt.font.FontRenderContext getFontRenderContext()
        {
            throw new NotImplementedException();
        }

        public override void drawGlyphVector(java.awt.font.GlyphVector g, float x, float y)
        {
            throw new NotImplementedException();
        }
    }

    class NetGraphicsConfiguration : java.awt.GraphicsConfiguration
    {
        Screen screen;

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
            System.Drawing.Rectangle bounds = screen.Bounds;
            return new java.awt.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
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

        public override VolatileImage createCompatibleVolatileImage(int i1, int i2, int i3)
        {
            throw new NotImplementedException();
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
            return new NetGraphicsConfiguration(Screen.PrimaryScreen);
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

    class NetGraphicsEnvironment : java.awt.GraphicsEnvironment
    {
        // Create a bitmap with the dimensions of the argument image. Then
        // create a graphics objects from the bitmap. All paint operations will
        // then paint the bitmap.
        public override java.awt.Graphics2D createGraphics(BufferedImage bi)
        {
            Bitmap bitmap = new Bitmap(bi.getWidth(), bi.getHeight());
            return new BitmapGraphics(bitmap);
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
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    FontFamily[] families = FontFamily.GetFamilies(g);
                    String[] results = new String[families.Length];
                    for (int i = 0; i < results.Length; i++)
                    {
                        results[i] = families[i].GetName(language);
                    }
                    return results;
                }
            }
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