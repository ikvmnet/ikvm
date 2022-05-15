/*
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

import java.awt.image.*;
import java.io.IOException;
import java.io.InputStream;

import cli.System.Drawing.Bitmap;
import cli.System.Drawing.Imaging.ImageLockMode;
import cli.System.Drawing.Imaging.PixelFormat;
import cli.System.IO.SeekOrigin;
import cli.System.IO.Stream;
import cli.System.NotSupportedException;
import ikvm.runtime.Util;

abstract class IkvmImageDecoder extends ImageDecoder {

    IkvmImageDecoder(InputStreamImageSource src, InputStream is){
        super(src, is);
    }

    @Override
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public void produceImage() throws IOException, ImageFormatException{
        Stream stream = new Stream(){

            @Override
            public void Flush(){
                Util.throwException(new NotSupportedException());
            }

            @Override
            public int Read(byte[] bytes, int off, int len){
                try{
                    int count = input.read(bytes, off, len);
                    if( count < 0 ){
                        return 0;
                    }
                    return count;
                }catch(IOException ex){
                    throw new RuntimeException(ex);
                }
            }

            @Override
            public long Seek(long arg0, SeekOrigin arg1){
                Util.throwException(new NotSupportedException());
                return 0;
            }

            @Override
            public void SetLength(long arg0){
                Util.throwException(new NotSupportedException());
            }

            @Override
            public void Write(byte[] arg0, int arg1, int arg2){
                Util.throwException(new NotSupportedException());
            }

            @Override
            public boolean get_CanRead(){
                return true;
            }

            @Override
            public boolean get_CanSeek(){
                return false;
            }

            @Override
            public boolean get_CanWrite(){
                return true;
            }

            @Override
            public long get_Length(){
                try{
                    return input.available();
                }catch(IOException ex){
                    throw new RuntimeException(ex);
                }
            }

            @Override
            public long get_Position(){
                Util.throwException(new NotSupportedException());
                return 0;
            }

            @Override
            public void set_Position(long arg0){
                Util.throwException(new NotSupportedException());
            }
            
        };
        try{
            Bitmap bitmap = new Bitmap(stream);
            int width = bitmap.get_Width();
            int height = bitmap.get_Height();
            int size = width * height;
            int[] pixelData = new int[size];
            
            cli.System.Drawing.Rectangle rect = new cli.System.Drawing.Rectangle(0, 0, width, height);
            cli.System.Drawing.Imaging.BitmapData data = bitmap.LockBits(rect, ImageLockMode.wrap(ImageLockMode.ReadOnly), PixelFormat.wrap(PixelFormat.Format32bppArgb));
            cli.System.IntPtr pixelPtr = data.get_Scan0();
            cli.System.Runtime.InteropServices.Marshal.Copy(pixelPtr, pixelData, 0, size);        
            bitmap.UnlockBits(data);
            
            //source.
            
            setDimensions(width, height);
            ColorModel cm = ColorModel.getRGBdefault();
            setColorModel(cm);
            //setHints(flags);
            headerComplete();
            
            setPixels(0,0,width,height, cm, pixelData,0,width);
            imageComplete(ImageConsumer.STATICIMAGEDONE, true);
        }catch(Throwable th){
            th.printStackTrace();
            imageComplete(ImageConsumer.IMAGEERROR|ImageConsumer.STATICIMAGEDONE, true);
            throw new IOException(th);
        } finally {
            try { close(); } catch(Throwable e){e.printStackTrace();}
        }
    }
}
