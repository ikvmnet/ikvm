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
import javax.imageio.IIOException;
import javax.imageio.IIOImage;
import javax.imageio.ImageTypeSpecifier;
import javax.imageio.metadata.IIOMetadata;
import javax.imageio.plugins.jpeg.JPEGImageWriteParam;
import javax.imageio.plugins.jpeg.JPEGQTable;
import javax.imageio.spi.ImageWriterSpi;
import javax.imageio.stream.ImageOutputStream;
import java.awt.image.BufferedImage;

import java.io.IOException;

import cli.System.Drawing.Imaging.Encoder;
import cli.System.Drawing.Imaging.EncoderParameter;
import cli.System.Drawing.Imaging.EncoderParameters;
import cli.System.Drawing.Imaging.ImageCodecFlags;
import cli.System.Drawing.Imaging.ImageCodecInfo;
import cli.System.Drawing.Imaging.ImageFormat;

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
    
        ImageCodecInfo codec = null;
        for (ImageCodecInfo ici : ImageCodecInfo.GetImageEncoders()) {
            if (ici.get_FormatID().equals(ImageFormat.get_Jpeg().get_Guid())
                && (ici.get_Flags().Value & ImageCodecFlags.Builtin) != 0) {
                codec = ici;
                break;
            }
        }
        if (codec == null) {
            throw new IIOException("JPEG codec not found");
        }
    
        BufferedImage img = (BufferedImage)image.getRenderedImage();
        
        ImageOutputStream imgOutput = (ImageOutputStream)getOutput();
        
        JPEGImageWriteParam jparam = null;
        JPEGQTable[] qTables = null;
        
        if (param != null) {
            switch (param.getCompressionMode()) {
                case ImageWriteParam.MODE_DISABLED:
                    throw new IIOException("JPEG compression cannot be disabled");
                case ImageWriteParam.MODE_EXPLICIT:
                    float quality = param.getCompressionQuality();
                    quality = JPEG.convertToLinearQuality(quality);
                    qTables = new JPEGQTable[2];
                    qTables[0] = JPEGQTable.K1Luminance.getScaledInstance(quality, true);
                    qTables[1] = JPEGQTable.K2Chrominance.getScaledInstance(quality, true);
                    break;
                case ImageWriteParam.MODE_DEFAULT:
                    qTables = new JPEGQTable[2];
                    qTables[0] = JPEGQTable.K1Div2Luminance;
                    qTables[1] = JPEGQTable.K2Div2Chrominance;
                    break;
            }
            if (param instanceof JPEGImageWriteParam) {
                jparam = (JPEGImageWriteParam)param;
            }
        }
        
        if (qTables == null) {
            if (jparam != null && jparam.areTablesSet()) {
                qTables = jparam.getQTables();
            } else {
                qTables = JPEG.getDefaultQTables();
            }
        }
        
        // Create a MemoryStream with publicly visible buffer
        cli.System.IO.MemoryStream stream = new cli.System.IO.MemoryStream(1024);
        EncoderParameters params = new EncoderParameters(2);
        try {
            params.get_Param()[0] = new EncoderParameter(Encoder.LuminanceTable, qTableToShortArray(qTables[0]));
            params.get_Param()[1] = new EncoderParameter(Encoder.ChrominanceTable, qTableToShortArray(qTables[1]));
            cli.System.Drawing.Bitmap bitmap = img.getBitmap();
            synchronized( bitmap ) {
                bitmap.Save(stream, codec, params);
            }
        }
        finally {
            params.Dispose();
        }
        
        imgOutput.write(stream.GetBuffer(), 0, (int)stream.get_Length());
    }
    
    private static short[] qTableToShortArray(JPEGQTable table) {
        int[] array = table.getTable();
        short[] s = new short[64];
        for (int i = 0; i < 64; i++)
            s[i] = (short)array[i];
        return s;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public ImageWriteParam getDefaultWriteParam() {
        return new JPEGImageWriteParam(null);
    }
}
