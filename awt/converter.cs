/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters, Volker Berlin
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

        internal static Image ConvertImage(java.awt.Image img)
        {
            if (img is NetBufferedImage)
            {
                return ((NetBufferedImage)img).bitmap;
            }
            if (img is NetVolatileImage)
            {
                return ((NetVolatileImage)img).bitmap;
            }
            if (img is BufferedImage)
            {
                return ConvertImage((BufferedImage)img);
            }
            if (img is NoImage)
            {
                return null;
            }
            Console.WriteLine(new System.Diagnostics.StackTrace());
            throw new NotImplementedException("Image class:" + img.GetType().FullName);
        }

        private static Image ConvertImage(BufferedImage img)
        {
            //First map the pixel from Java type to .NET type
            PixelFormat format;
            switch (img.getType())
            {
                case BufferedImage.TYPE_INT_ARGB:
                    format = PixelFormat.Format32bppArgb;
                    break;
                default:
                    throw new NotImplementedException("BufferedImage Type:" + img.getType());
            }

            //Create a .NET BufferedImage (alias Bitmap)
            int width = img.getWidth();
            int height = img.getHeight();
            Bitmap bitmap = new Bitmap(width, height, format);

            //Request the .NET pixel pointer
            Rectangle rec = new Rectangle(0, 0, width, height);
            BitmapData data = bitmap.LockBits(rec, ImageLockMode.WriteOnly, format);
            IntPtr pixelPtr = data.Scan0;

            //Request the pixel data from Java and copy it to .NET
            WritableRaster raster = img.getRaster();
            int[] pixelData = raster.getPixels(0, 0, width, height, (int[])null);
            Marshal.Copy(pixelData, 0, pixelPtr, pixelData.Length);

            bitmap.UnlockBits(data);
            return bitmap;
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
        internal static GraphicsPath ConvertRoundRect(int x, int y, int w, int h, int radius)
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
            switch (name)
            {
                case "Monospaced":
                case "Courier":
                case "courier":
                    return FontFamily.GenericMonospace;
                case "Serif":
                    return FontFamily.GenericSerif;
                case "SansSerif":
                case "Dialog":
                case "DialogInput":
                case null:
                case "Default":
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
            FontFamily fam = CreateFontFamily(name);
            return new Font(fam, size, ConvertFontStyle(style), GraphicsUnit.Pixel);
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