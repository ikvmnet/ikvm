/*
  Copyright (C) 2009 Jeroen Frijters
  Copyright (C) 2010 Volker Berlin (i-net software)
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

package sun.awt.image;

import java.awt.image.BufferedImage;
import java.awt.image.ColorModel;
import java.awt.image.ImageConsumer;
import java.awt.image.ImageObserver;
import java.util.Hashtable;

import cli.System.Drawing.Bitmap;
import cli.System.Drawing.Imaging.BitmapData;
import cli.System.Drawing.Imaging.ImageLockMode;
import cli.System.Drawing.Imaging.PixelFormat;

public class ImageRepresentation extends ImageWatched implements ImageConsumer{
	
	private static final int DEFAULT_PIXEL_FORMAT = PixelFormat.Format32bppArgb;
    InputStreamImageSource src;
    ToolkitImage image;

    private int width = -1;
    private int height = -1;

    private int availinfo;
    
    private BufferedImage bimage;
    private cli.System.Drawing.Bitmap bitmap;
    private int pixelFormat = DEFAULT_PIXEL_FORMAT;

    ImageRepresentation(ToolkitImage im){
        image = im;

        if (image.getSource() instanceof InputStreamImageSource) {
            src = (InputStreamImageSource) image.getSource();
        }
    }
    
    /* REMIND: Only used for Frame.setIcon - should use ImageWatcher instead */
    public synchronized void reconstruct(int flags) {
        if (src != null) {
            src.checkSecurity(null, false);
        }
        int missinginfo = flags & ~availinfo;
        if ((availinfo & ImageObserver.ERROR) == 0 && missinginfo != 0) {
            numWaiters++;
            try {
                startProduction();
                missinginfo = flags & ~availinfo;
                while ((availinfo & ImageObserver.ERROR) == 0 &&
                       missinginfo != 0)
                {
                    try {
                        wait();
                    } catch (InterruptedException e) {
                        Thread.currentThread().interrupt();
                        return;
                    }
                    missinginfo = flags & ~availinfo;
                }
            } finally {
                decrementWaiters();
            }
        }
    }
    
    @Override
    public void setDimensions(int w, int h){
        if (src != null) {
            src.checkSecurity(null, false);
        }
        
        image.setDimensions(w, h);
//        bitmap = new cli.System.Drawing.Bitmap(w, h);
        
        newInfo(image, (ImageObserver.WIDTH | ImageObserver.HEIGHT),
                0, 0, w, h);

        if (w <= 0 || h <= 0) {
            imageComplete(ImageConsumer.IMAGEERROR);
            return;
        }

        width = w;
        height = h;

        availinfo |= ImageObserver.WIDTH | ImageObserver.HEIGHT;
    }
    
    private Bitmap getBitmapRef(){
    	if( bitmap == null ){
    		bitmap = new Bitmap(width, height, PixelFormat.wrap(pixelFormat) );
    	}
    	return bitmap;
    }

    public int getWidth(){
        return width;
    }
    
    public int getHeight(){
        return height;
    }
    
    public BufferedImage getBufferedImage(){
        return bimage;
    }
    
    @Override
    public void setProperties(Hashtable<?, ?> props){
        // ignore it
        
    }

    @Override
    public synchronized void setColorModel(ColorModel model){
        int newPixelFormat = getPixelFormatForColorModel(model);
        if( model.getPixelSize() <= 8 ){
        	newPixelFormat = DEFAULT_PIXEL_FORMAT;
        }
        if( newPixelFormat != pixelFormat && bitmap != null ){
        	// force reconstruct of the bitmap due to a color model change
        	bitmap.Dispose();
        	bitmap = null;
        }
        pixelFormat = newPixelFormat;
    }

    @Override
    public void setHints(int hintflags){
        // ignore it
    }

    @Override
    public void setPixels(int x, int y, int w, int h, ColorModel model, byte[] pixels, int off, int scansize){
        int[] pixeli = new int[pixels.length];
        for (int i = 0; i < pixels.length; i++)
        {
            pixeli[i] = model.getRGB(pixels[i] & 0xff);
        }
        setPixels(x, y, w, h, model, pixeli, off, scansize);
    }

    @Override
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public void setPixels(int x, int y, int w, int h, ColorModel model, int[] pixels, int off, int scansize){
    	// FIXME this method will fail for scan lines
        if( x < 0) {
            w -= x;
            x = 0;
        }
        if (y < 0) {
            h -= y;
            y = 0;
        }
        if (w <= 0 || h <= 0) {
            // nothing to set
            return;
        }
        if ( off < 0 ) {
            throw new java.lang.ArrayIndexOutOfBoundsException( "Data offset out of bounds." );
        }
        long length = w * h;
        if (length > pixels.length - off)
        {
            throw new java.lang.ArrayIndexOutOfBoundsException("Data offset out of bounds.");
        }
        synchronized (this)
        {
            int pixelFormat = getPixelFormatForColorModel( model );
			int bpp = model.getPixelSize();
			if( bpp == 32 ){ // 32 can be copies 1:1 using an int array
				copyInt(x, y, w, h, pixels, off, pixelFormat);				
            }else if( bpp <= 8 ){
            	// transform all pixels using the color model (for indexed color models)
            	int[] newData = new int[pixels.length];
            	for( int i=0; i < newData.length; i++ ){
            		newData[i] = model.getRGB(pixels[i]);
            	}
            	copyInt(x, y, w, h, pixels, off, DEFAULT_PIXEL_FORMAT);
            }else {
            	// byte per scanline, must be a multitude of 4
            	// see http://stackoverflow.com/questions/2185944/why-must-stride-in-the-system-drawing-bitmap-constructor-be-a-multiple-of-4
            	int bytesPerLine = (bpp * w) / 8;
            	int scanLine = ((bytesPerLine + 3) / 4) * 4;
				int offset = scanLine - bytesPerLine; 
            	byte[] newData = new byte[h * scanLine];
            	int position = 0;
            	int pixel;
            	for( int i=0; i<pixels.length; i++ ){
            		 pixel = pixels[i];
            		 switch( bpp ){
            		 	case 16: newData[position] = (byte)(pixel & 0xFF);
            		 			 newData[position + 1] = (byte)((pixel >> 8) & 0xFF); break;
            		 	case 24: newData[position] = (byte)(pixel & 0xFF);
            		 			 newData[position + 1] = (byte)((pixel >> 8) & 0xFF);
   		 			 			 newData[position + 2] = (byte)((pixel >> 16) & 0xFF); break;
            		 }
            		 position += bpp / 8;
            		 if( position % scanLine == bytesPerLine ){
            			 position += offset;
            		 }
            	}
				copyByte(x, y, w, h, newData, off, pixelFormat, bpp);				            	
            }
        }
        
        availinfo |= ImageObserver.SOMEBITS;

        // Can't do this here since we might need to transform/clip
        // the region
        if (((availinfo & ImageObserver.FRAMEBITS) == 0)) {
            newInfo(image, ImageObserver.SOMEBITS, x, y, w, h);
        }
    }

	@cli.System.Security.SecurityCriticalAttribute.Annotation
	private void copyInt(int x, int y, int w, int h, int[] pixels, int off, int pixelFormat ) {
		BitmapData data = getBitmapRef().LockBits(new cli.System.Drawing.Rectangle(x, y, w, h), ImageLockMode.wrap(ImageLockMode.WriteOnly), PixelFormat.wrap(pixelFormat));
		cli.System.Runtime.InteropServices.Marshal.Copy(pixels, off, data.get_Scan0(), data.get_Width() * data.get_Height());
		getBitmapRef().UnlockBits(data);
	}
    
	@cli.System.Security.SecurityCriticalAttribute.Annotation
	private void copyByte(int x, int y, int w, int h, byte[] pixels, int off, int pixelFormat, int bpp) {
		BitmapData data = getBitmapRef().LockBits(new cli.System.Drawing.Rectangle(x, y, w, h), ImageLockMode.wrap(ImageLockMode.WriteOnly), PixelFormat.wrap(pixelFormat));
		cli.System.Runtime.InteropServices.Marshal.Copy(pixels, off, data.get_Scan0(), pixels.length);
		getBitmapRef().UnlockBits(data);
	}
    
    
    private int getPixelFormatForColorModel( ColorModel cm ){
    	if( cm == null ){
    		return DEFAULT_PIXEL_FORMAT; // TODO is PixelFormat.Canonical better here?
    	}
    	int bpp = cm.getPixelSize();
    	int[] sizes = cm.getComponentSize();
    	switch( bpp ){
    		case 1: return PixelFormat.Undefined; // Indexed is invalid and there is no 1bpp
    		case 4: return PixelFormat.Format4bppIndexed;
    		case 8: return PixelFormat.Format8bppIndexed;
    		case 16:
    			if( sizes.length <= 1) {
    				return PixelFormat.Format16bppGrayScale;
    			}
    			if( sizes.length == 3 ){
    				if( sizes[0] == 5 && sizes[2] == 5 ){
    					return sizes[1] == 5 ? PixelFormat.Format16bppRgb555 : PixelFormat.Format16bppRgb565;
    				}
    			}
    			if( sizes.length == 4 && cm.hasAlpha() ){
    				return PixelFormat.Format16bppArgb1555;
    			}
    			break;
    		case 24:
    			return PixelFormat.Format24bppRgb;
    		case 32:
    			if(!cm.hasAlpha()){
    				return PixelFormat.Format32bppRgb;
    			} else {
    				return cm.isAlphaPremultiplied() ? PixelFormat.Format32bppPArgb : PixelFormat.Format32bppArgb;
    			}
    		case 48:
    			return PixelFormat.Format48bppRgb;
    		case 64:
    			return cm.isAlphaPremultiplied() ? PixelFormat.Format64bppPArgb : PixelFormat.Format64bppArgb;    			
    	}
    	return PixelFormat.Undefined;
    }

    private boolean consuming = false;

    public void imageComplete(int status) {
        if (src != null) {
            src.checkSecurity(null, false);
        }
        boolean done;
        int info;
        switch (status) {
        default:
        case ImageConsumer.IMAGEABORTED:
            done = true;
            info = ImageObserver.ABORT;
            break;
        case ImageConsumer.IMAGEERROR:
            image.addInfo(ImageObserver.ERROR);
            done = true;
            info = ImageObserver.ERROR;
            dispose();
            break;
        case ImageConsumer.STATICIMAGEDONE:
            done = true;
            info = ImageObserver.ALLBITS;
            break;
        case ImageConsumer.SINGLEFRAMEDONE:
            done = false;
            info = ImageObserver.FRAMEBITS;
            break;
        }
        synchronized (this) {
            if (done) {
                image.getSource().removeConsumer(this);
                consuming = false;
            }
            if (bimage == null ) {
            	bimage = new BufferedImage(getBitmapRef());
            }
            availinfo |= info;
            notifyAll();
        }

        newInfo(image, info, 0, 0, width, height);

        image.infoDone(status);
    }

    /*synchronized*/ void startProduction() {
        if (!consuming) {
            consuming = true;
            image.getSource().startProduction(this);
        }
    }

    private int numWaiters;
    
    private synchronized void checkConsumption() {
        if (isWatcherListEmpty() && numWaiters == 0 &&
            ((availinfo & ImageObserver.ALLBITS) == 0))
        {
            dispose();
        }
    }

    @Override
    public synchronized void notifyWatcherListEmpty() {
        checkConsumption();
    }

    private synchronized void decrementWaiters() {
        --numWaiters;
        checkConsumption();
    }

    public boolean prepare(ImageObserver iw) {
        if (src != null) {
            src.checkSecurity(null, false);
        }
        if ((availinfo & ImageObserver.ERROR) != 0) {
            if (iw != null) {
                iw.imageUpdate(image, ImageObserver.ERROR|ImageObserver.ABORT,
                               -1, -1, -1, -1);
            }
            return false;
        }
        boolean done = ((availinfo & ImageObserver.ALLBITS) != 0);
        if (!done) {
            addWatcher(iw);
            startProduction();
            // Some producers deliver image data synchronously
            done = ((availinfo & ImageObserver.ALLBITS) != 0);
        }
        return done;
    }

    public int check(ImageObserver iw) {

        if (src != null) {
            src.checkSecurity(null, false);
        }
        if ((availinfo & (ImageObserver.ERROR | ImageObserver.ALLBITS)) == 0) {
            addWatcher(iw);
        }

        return availinfo;
    }
    
    synchronized void abort() {
        image.getSource().removeConsumer(this);
        consuming = false;
        bimage = null;
        bitmap = null;

        newInfo(image, ImageObserver.ABORT, -1, -1, -1, -1);
        availinfo &= ~(ImageObserver.SOMEBITS
                       | ImageObserver.FRAMEBITS
                       | ImageObserver.ALLBITS
                       | ImageObserver.ERROR);
    }

    synchronized void dispose() {
        image.getSource().removeConsumer(this);
        consuming = false;
        availinfo &= ~(ImageObserver.SOMEBITS
                       | ImageObserver.FRAMEBITS
                       | ImageObserver.ALLBITS);
    }
}
