/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.
  Copyright (C) 2006 - 2013 Volker Berlin (i-net software)

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

    class NetVolatileImage : java.awt.image.VolatileImage
    {
        internal readonly Bitmap bitmap;
        internal readonly java.awt.Component component;
        private java.awt.Font defaultFont;
        private readonly int width;
        private readonly int height;

        internal NetVolatileImage(java.awt.Component component, int width, int height)
        {
            this.component = component;
            bitmap = new Bitmap(width, height);
            this.width = width;
            this.height = height;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
            }
        }

        internal NetVolatileImage(int width, int height) : this(null, width, height)
        {
        }

        public override bool contentsLost()
        {
            return false;
        }

        private java.awt.Color getForeground()
        {
            if (component != null)
            {
                return component.getForeground();
            }
            else
            {
                return java.awt.Color.black;
            }
        }

        private java.awt.Color getBackground()
        {
            if (component != null)
            {
                return component.getBackground();
            }
            else
            {
                return java.awt.Color.white;
            }
        }

        private java.awt.Font getFont()
        {
            if (component != null)
            {
                return component.getFont();
            }
            else
            {
                if (defaultFont == null)
                {
                    defaultFont = new java.awt.Font("Dialog", java.awt.Font.PLAIN, 12);
                }
                return defaultFont;
            }
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
            return new BitmapGraphics(bitmap, this, getFont(), J2C.ConvertColor(getForeground()), J2C.ConvertColor(getBackground()));
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
        private sun.awt.image.InputStreamImageSource source;

        internal NoImage(sun.awt.image.InputStreamImageSource source) {
            this.source = source;
        }

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
            return source;
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