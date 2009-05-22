/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters
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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
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
    /// <summary>
    /// This class has some static convertion methods from Java to C# objects
    /// </summary>
    class J2C
    {

        internal static Color ConvertColor(java.awt.Color color)
        {
            return Color.FromArgb(color.getRGB());
        }

        internal static Image ConvertImage(java.awt.Image img)
        {
            if (img is BufferedImage)
            {
                return ((BufferedImage)img).getBitmap();
            }
            if (img is NetVolatileImage)
            {
                return ((NetVolatileImage)img).bitmap;
            }
            if (img is NetProducerImage)
            {
                return ((NetProducerImage)img).getBitmap();
            }
            if (img is NoImage)
            {
                return null;
            }
            Console.WriteLine(new System.Diagnostics.StackTrace());
            throw new NotImplementedException("Image class:" + img.GetType().FullName);
        }

        internal static PointF ConvertPoint(java.awt.geom.Point2D point)
        {
            return new PointF((float)point.getX(), (float)point.getY());
        }

        internal static RectangleF ConvertRect(java.awt.geom.Rectangle2D rect)
        {
            return new RectangleF((float)rect.getX(), (float)rect.getY(), (float)rect.getWidth(), (float)rect.getHeight());
        }

        internal static Rectangle ConvertRect(java.awt.Rectangle rect)
        {
            return new Rectangle(rect.x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        /// Create a rounded rectangle using lines and arcs
        /// </summary>
        /// <param name="x">upper left x coordinate</param>
        /// <param name="y">upper left y coordinate</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="arcWidth">the horizontal diameter of the arc at the four corners</param>
        /// <param name="arcHeight">the vertical diameter of the arc at the four corners</param>
        /// <returns></returns>
		internal static GraphicsPath ConvertRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
        {
            GraphicsPath gp = new GraphicsPath();
            bool drawArc = arcWidth > 0 && arcHeight > 0;
            int a = arcWidth / 2;
            int b = arcHeight / 2;
			gp.AddLine(x + a, y, x + w - a, y);
            if (drawArc)
            {
                gp.AddArc(x + w - arcWidth, y, arcWidth, arcHeight, 270, 90); //upper right arc
            }
            gp.AddLine(x + w, y + b, x + w, y + h - b);
            if (drawArc)
            {
                gp.AddArc(x + w - arcWidth, y + h - arcHeight, arcWidth, arcHeight, 0, 90); //lower right arc
            }
            gp.AddLine(x + w - a, y + h, x + a, y + h);
            if (drawArc)
            {
                gp.AddArc(x, y + h - arcHeight, arcWidth, arcHeight, 90, 90);//lower left arc
            }
            gp.AddLine(x, y + h - b, x, y + b);
            if (drawArc)
            {
                gp.AddArc(x, y, arcWidth, arcHeight, 180, 90); //upper left arc
            }
            gp.CloseFigure();

            return gp;
        }

        internal static GraphicsPath ConvertShape(java.awt.Shape shape)
        {
            java.awt.geom.GeneralPath path = new java.awt.geom.GeneralPath(shape);
            java.awt.geom.PathIterator iterator = path.getPathIterator(new java.awt.geom.AffineTransform());
            GraphicsPath gp = new GraphicsPath();
            switch (iterator.getWindingRule())
            {
                case java.awt.geom.PathIterator.__Fields.WIND_EVEN_ODD:
                    gp.FillMode = System.Drawing.Drawing2D.FillMode.Alternate;
                    break;
                case java.awt.geom.PathIterator.__Fields.WIND_NON_ZERO:
                    gp.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
                    break;
            }
            float[] coords = new float[6];
            float x = 0;
            float y = 0;
            while (!iterator.isDone())
            {
                int type = iterator.currentSegment(coords);
                switch (type)
                {
                    case java.awt.geom.PathIterator.__Fields.SEG_MOVETO:
                        x = coords[0];
                        y = coords[1];
                        gp.StartFigure();
                        break;
                    case java.awt.geom.PathIterator.__Fields.SEG_LINETO:
                        gp.AddLine(x, y, coords[0], coords[1]);
                        x = coords[0];
                        y = coords[1];
                        break;
                    case java.awt.geom.PathIterator.__Fields.SEG_QUADTO:
                        gp.AddBezier(x, y, coords[0], coords[1], coords[0], coords[1], coords[2], coords[3]);
                        x = coords[2];
                        y = coords[3];
                        break;
                    case java.awt.geom.PathIterator.__Fields.SEG_CUBICTO:
                        gp.AddBezier(x, y, coords[0], coords[1], coords[2], coords[3], coords[4], coords[5]);
                        x = coords[4];
                        y = coords[5];
                        break;
                    case java.awt.geom.PathIterator.__Fields.SEG_CLOSE:
                        gp.CloseFigure();
                        break;
                }
                iterator.next();
            }
            return gp;
        }

        internal static Brush CreateBrush(java.awt.Color color)
        {
            return new SolidBrush(Color.FromArgb(color.getRGB()));
        }

        internal static LineJoin ConvertLineJoin(int join)
        {
            switch (join)
            {
                case java.awt.BasicStroke.JOIN_MITER:
                    return LineJoin.Miter;
                case java.awt.BasicStroke.JOIN_ROUND:
                    return LineJoin.Round;
                case java.awt.BasicStroke.JOIN_BEVEL:
                    return LineJoin.Bevel;
                default:
                    throw new ArgumentException("Invalid LineJoin argument.");
            }
        }

        internal static LineCap ConvertLineCap(int cap)
        {
            switch (cap)
            {
                case java.awt.BasicStroke.CAP_BUTT:
                    return LineCap.Flat;
                case java.awt.BasicStroke.CAP_ROUND:
                    return LineCap.Round;
                case java.awt.BasicStroke.CAP_SQUARE:
                    return LineCap.Square;
                default:
                    throw new ArgumentException("Invalid LineCap argument.");
            }
        }

        internal static float[] ConvertDashArray(float[] dashArray, float lineWidth)
        {
            if (dashArray == null)
            {
                return null;
            }
            float[] dash = (float[])dashArray.Clone();
            for (int i = 0; i < dash.Length; i++)
            {
                //dividing by line thickness because of the representation difference
                dash[i] = dash[i] / lineWidth;
            }
            return dash;
        }

        internal static Matrix ConvertTransform(java.awt.geom.AffineTransform tx)
        {
            return new Matrix(
                (float)tx.getScaleX(),
                (float)tx.getShearY(),
                (float)tx.getShearX(),
                (float)tx.getScaleY(),
                (float)tx.getTranslateX(),
                (float)tx.getTranslateY());
        }

        internal static FontFamily CreateFontFamily(String name)
        {
            String name2 = name == null ? null : name.ToLower();
			switch (name2)
			{
				case "monospaced":
				case "courier":
					return FontFamily.GenericMonospace;
				case "serif":
					return FontFamily.GenericSerif;
				case "sansserif":
				case "dialog":
				case "dialoginput":
				case null:
				case "default":
					return FontFamily.GenericSansSerif;
				default:
					try
					{
						return new FontFamily(name);
					}
					catch (ArgumentException)
					{
						return FontFamily.GenericSansSerif;
					}
			}
        }

		private static FontStyle ConvertFontStyle(int style)
		{
			FontStyle fs = FontStyle.Regular;
			if ((style & java.awt.Font.BOLD) != 0)
			{
				fs |= FontStyle.Bold;
			}
			if ((style & java.awt.Font.ITALIC) != 0)
			{
				fs |= FontStyle.Italic;
			}
			return fs;
		}

        internal static Font ConvertFont(String name, int style, float size)
		{
            if (size <= 0)
            {
                size = 1;
            }
            FontFamily family = CreateFontFamily(name);
            FontStyle fontStyle = ConvertFontStyle(style);
            if (!family.IsStyleAvailable(fontStyle))
            {
                //Some Fonts (for example Aharoni) does not support Regular style. This throw an exception else it is not documented.
                if(family.IsStyleAvailable(FontStyle.Regular)){
                    fontStyle = FontStyle.Regular;
                }else
                if(family.IsStyleAvailable(FontStyle.Bold)){
                    fontStyle = FontStyle.Bold;
                }else
                if(family.IsStyleAvailable(FontStyle.Italic)){
                    fontStyle = FontStyle.Italic;
                }else
                if(family.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic)){
                    fontStyle = FontStyle.Bold | FontStyle.Italic;
                }
            }
            return new Font(family, size, fontStyle, GraphicsUnit.Pixel);
        }

    }

    /// <summary>
    /// This class has some static convertion function from C# to Java objects
    /// </summary>
    class C2J
    {
        internal static java.awt.geom.AffineTransform ConvertMatrix(Matrix matrix)
        {
            float[] elements = matrix.Elements;
            return new java.awt.geom.AffineTransform(elements);
        }

        internal static java.awt.Rectangle ConvertRectangle(RectangleF rec)
        {
            return new java.awt.Rectangle((int)rec.X, (int)rec.Y, (int)rec.Width, (int)rec.Height);
        }

    }
}