/*
  Copyright (C) 2009 Jeroen Frijters
  Copyright (C) 2010 Volker Berlin (i-net software)

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

import com.sun.jdi.InvalidStackFrameException;

import cli.System.Drawing.Imaging.BitmapData;
import cli.System.Drawing.Imaging.ImageLockMode;
import cli.System.Drawing.Imaging.PixelFormat;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

public class ImageRepresentation extends ImageWatched implements ImageConsumer
{
    InputStreamImageSource src;
    ToolkitImage image;

    private int width = -1;
    private int height = -1;

    private int availinfo;
    
    private BufferedImage bimage;
    private cli.System.Drawing.Bitmap bitmap;

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
        bitmap = new cli.System.Drawing.Bitmap(w, h);
        
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
    public void setColorModel(ColorModel model){
        // TODO we use currently only the default color model ARGB
        
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
        synchronized (bitmap)
        {
            BitmapData data = bitmap.LockBits(new cli.System.Drawing.Rectangle(x, y, w, h), ImageLockMode.wrap(ImageLockMode.WriteOnly), PixelFormat.wrap(PixelFormat.Format32bppArgb));
            cli.System.Runtime.InteropServices.Marshal.Copy(pixels, off, data.get_Scan0(), data.get_Width() * data.get_Height());
            bitmap.UnlockBits(data);
        }
        
        availinfo |= ImageObserver.SOMEBITS;

        // Can't do this here since we might need to transform/clip
        // the region
        if (((availinfo & ImageObserver.FRAMEBITS) == 0)) {
            newInfo(image, ImageObserver.SOMEBITS, x, y, w, h);
        }
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

                if (bimage != null) {
                    bimage = new BufferedImage(bitmap);
                }
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
    
    /**
     * Get the .NET Bitmap object.
     */
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public cli.System.Drawing.Bitmap getBitmap(){
        if( bitmap == null ){
            throw new IllegalStateException("Image not complete.");
        }
        return bitmap;
    }
}
