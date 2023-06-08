/*
  Copyright (C) 2008 - 2013 Volker Berlin (i-net software)

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
package com.sun.imageio.plugins.jpeg;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.*;
import java.util.Iterator;

import javax.imageio.*;
import javax.imageio.metadata.IIOMetadata;
import javax.imageio.spi.ImageReaderSpi;
import javax.imageio.stream.ImageInputStream;

/**
 * A image reader implementation that is calling the .NET API for reading the JPEG image.
 */
class JPEGImageReader extends ImageReader{
    
    private BufferedImage image;

    /**
     * Default constructor, Sun compatible.
     */
    protected JPEGImageReader(ImageReaderSpi originatingProvider){
        super(originatingProvider);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public int getHeight(int imageIndex) throws IOException{
        return getBufferedImage().getHeight();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata getImageMetadata(int imageIndex){
        throw new Error("Not Implemented");
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Iterator<ImageTypeSpecifier> getImageTypes(int imageIndex){
        throw new Error("Not Implemented");
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public int getNumImages(boolean allowSearch){
        return 1;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata getStreamMetadata(){
        throw new Error("Not Implemented");
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public int getWidth(int imageIndex) throws IOException{
        return getBufferedImage().getWidth();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public BufferedImage read(int imageIndex, ImageReadParam param) throws IOException{
        return getBufferedImage();
    }
    
    /**
     * Read the image with .NET API if not already read.
     */
    private BufferedImage getBufferedImage() throws IOException{
        if(image == null){
            ImageInputStream iis = (ImageInputStream)getInput();
            byte[] buffer;
            try {
				if( iis.length() >0){
				    //If length known then it it is simple
				    buffer = new byte[(int)iis.length()];
				    iis.readFully(buffer);
				}else{
				    // if the length not known then we need to read it in a loop
				    ByteArrayOutputStream baos = new ByteArrayOutputStream(8192);
				    buffer = new byte[8192];
				    int count;
				    while((count = iis.read(buffer)) > 0){
				        baos.write(buffer, 0, count);
				    }
				    buffer = baos.toByteArray();
				}
			} catch (IOException ioex) {
				processReadAborted();
				throw ioex;
			}
            processImageStarted(0);
            image = (BufferedImage)Toolkit.getDefaultToolkit().createImage(buffer);
            int width = image.getWidth();
            int height = image.getHeight();
            processPassStarted(image, 0, 0, 1, 0, 0, 1, 1, new int[0]);
            processImageProgress(100.0F);
            processImageUpdate(image, 0, 0, width, height, 1, 1, new int[0]);
            processPassComplete(image);
            processImageComplete();
        }
        return image;
    }

}