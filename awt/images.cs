/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters
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
using java.awt.image;
using java.util;


namespace ikvm.awt
{

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
            using (Graphics g = Graphics.FromImage(bitmap))
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
            return new BitmapGraphics(bitmap);
        }

        public override java.awt.image.ImageProducer getSource()
        {
            int[] pix = new int[bitmap.Width * bitmap.Height];
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
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

    class NetVolatileImage : java.awt.image.VolatileImage
    {
        internal Bitmap bitmap;

        internal NetVolatileImage(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
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
            return new BitmapGraphics(bitmap);
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

    class NoImage : java.awt.Image
    {

        public override int getWidth(java.awt.image.ImageObserver observer)
        {
            if (observer != null)
            {
                observer.imageUpdate(this, java.awt.image.ImageObserver.__Fields.ERROR | java.awt.image.ImageObserver.__Fields.ABORT, 0, 0, -1, -1);
            }
            return -1;
        }

        public override int getHeight(java.awt.image.ImageObserver observer)
        {
            if (observer != null)
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
            if (observer != null)
            {
                observer.imageUpdate(this, java.awt.image.ImageObserver.__Fields.ERROR | java.awt.image.ImageObserver.__Fields.ABORT, 0, 0, -1, -1);
            }
            return null;
        }

        public override void flush()
        {
        }
    }


}