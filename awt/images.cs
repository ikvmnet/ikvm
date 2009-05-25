/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.
  Copyright (C) 2006, 2007, 2008 Volker Berlin (i-net software)

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
using System.Drawing.Imaging;


namespace ikvm.awt
{

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
			int[] pixeli = new int[pixels.Length];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixeli[i] = model.getRGB(pixels[i] & 0xff);
			}
			setPixels(x, y, w, h, model, pixeli, off, scansize);
		}

        /// <summary>
        /// Create a bitmap from the pixel array. The bitmap will be used
        /// by drawImage.
        /// </summary>
		public void setPixels(int x, int y, int w, int h, ColorModel model, int[] pixels, int off, int scansize)
		{
			lock (mBitmap)
			{
				BitmapData data = mBitmap.LockBits(new Rectangle(x, y, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				System.Runtime.InteropServices.Marshal.Copy(pixels, off, data.Scan0, w * h);
				mBitmap.UnlockBits(data);
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
			mBitmap = new Bitmap(mWidth, mHeight);
		}

        public void imageComplete(int status)
        {
            // Console.WriteLine("NetProducerImage: imageComplete");
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
        internal readonly Bitmap bitmap;
        private readonly int width;
        private readonly int height;

        internal NetVolatileImage(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            this.width = width;
            this.height = height;
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
            return height; // bitmap.Height --> need invoke or lock
        }

        public override int getWidth(ImageObserver io)
        {
            return width; // bitmap.Width --> need invoke or lock
        }

        public override object getProperty(string str, ImageObserver io)
        {
            throw new NotImplementedException();
        }

        public override java.awt.Graphics2D createGraphics()
        {
            //Graphics g = Graphics.FromImage(bitmap);
            // HACK for off-screen images we don't want ClearType or anti-aliasing
            // TODO I'm sure Java 2D has a way to control text rendering quality, we should honor that
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            return new BitmapGraphics(bitmap);
        }

        public override int getHeight()
        {
            return height; // bitmap.Height --> need invoke or lock
        }

        public override int getWidth()
        {
            return width; // bitmap.Width --> need invoke or lock
        }

        public override BufferedImage getSnapshot()
        {
            return new BufferedImage(bitmap);
        }

        public override int validate(java.awt.GraphicsConfiguration gc)
        {
            return 0;
        }

        public override java.awt.ImageCapabilities getCapabilities()
        {
            throw new NotImplementedException();
        }

		public override void flush()
		{
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