/*
  Copyright (C) 2008, 2009 Volker Berlin (i-net software)

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

import javax.imageio.ImageWriter;
import javax.imageio.ImageWriteParam;
import javax.imageio.IIOImage;
import javax.imageio.ImageTypeSpecifier;
import javax.imageio.metadata.IIOMetadata;
import javax.imageio.spi.ImageWriterSpi;
import javax.imageio.stream.ImageOutputStream;
import java.awt.image.BufferedImage;

import java.io.IOException;

/**
 * JPEGImageWriter that use .NET features to write the the JPG file.
 */
public class JPEGImageWriter extends ImageWriter {

    /**
     * Default constructor, Sun compatible.
     */
    protected JPEGImageWriter(ImageWriterSpi originatingProvider){
        super(originatingProvider);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata convertImageMetadata(IIOMetadata inData, ImageTypeSpecifier imageType, ImageWriteParam param){
        return inData;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata convertStreamMetadata(IIOMetadata inData, ImageWriteParam param){
        return inData;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata getDefaultImageMetadata(ImageTypeSpecifier imageType, ImageWriteParam param){
        return null;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public IIOMetadata getDefaultStreamMetadata(ImageWriteParam param){
        return null;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void write(IIOMetadata streamMetadata, IIOImage image, ImageWriteParam param) throws IOException{
        BufferedImage img = (BufferedImage)image.getRenderedImage();
        cli.System.Drawing.Bitmap bitmap = img.getBitmap();
        
        ImageOutputStream imgOutput = (ImageOutputStream)getOutput();
        
        // Create a MemoryStream with publicly visible buffer
        cli.System.IO.MemoryStream stream = new cli.System.IO.MemoryStream(1024);
        bitmap.Save(stream, cli.System.Drawing.Imaging.ImageFormat.get_Jpeg() );
        
        imgOutput.write(stream.GetBuffer(), 0, (int)stream.get_Length());
    }


}
