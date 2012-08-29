/*
  Copyright (C) 2012 Volker Berlin (i-net software)

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

namespace ikvm.awt
{

    using java.awt.image;
    using System.IO;
    using System.Drawing;

    /// <summary>
    /// Encodes images in ICO format.
    /// </summary>
    internal class IconFactory
    {

        /// <summary>
        /// Indicates that ICO data represents an icon (.ICO).
        /// </summary>
        private const int TYPE_ICON = 1;

        private BinaryWriter writer;

        /// <summary>
        /// Creates a new instance of IconFactory </summary>
        internal IconFactory()
        {
        }

        /// <summary>
        /// Create a Icon from the given list of images
        /// </summary>
        /// <param name="images">list of images</param>
        /// <returns></returns>
        internal Icon CreateIcon(java.util.List images, Size size)
        {
            MemoryStream stream = new MemoryStream();
            Write(images, stream);
            stream.Position = 0;
            return new Icon(stream, size);
        }

        /// <summary>
        /// Encodes and writes multiple images without colour depth conversion.
        /// </summary>
        /// <param name="images">
        ///            the list of source images to be encoded </param>
        /// <param name="stream">
        ///            the output to which the encoded image will be written </param>
        internal void Write(java.util.List images, System.IO.Stream stream)
        {
            writer = new BinaryWriter(stream);

            int count = images.size();

            // file header 6
            WriteFileHeader(count, TYPE_ICON);

            // file offset where images start
            int fileOffset = 6 + count * 16;

            // icon entries 16 * count
            for (int i = 0; i < count; i++)
            {
                BufferedImage imgc = (BufferedImage)images.get(i);
                fileOffset += WriteIconEntry(imgc, fileOffset);
            }

            // images
            for (int i = 0; i < count; i++)
            {
                BufferedImage imgc = (BufferedImage)images.get(i);

                // info header
                WriteInfoHeader(imgc);
                // color map
                if (imgc.getColorModel().getPixelSize() <= 8)
                {
                    IndexColorModel icm = (IndexColorModel)imgc.getColorModel();
                    WriteColorMap(icm);
                }
                // xor bitmap
                WriteXorBitmap(imgc);
                // and bitmap
                WriteAndBitmap(imgc);

            }
        }

        /// <summary>
        /// Writes the ICO file header for an ICO containing the given number of
        /// images.
        /// </summary>
        /// <param name="count">
        ///            the number of images in the ICO </param>
        /// <param name="type">
        ///            TYPE_ICON
        private void WriteFileHeader(int count, int type)
        {
            // reserved 2
            writer.Write((short)0);
            // type 2
            writer.Write((short)type);
            // count 2
            writer.Write((short)count);
        }

        /// <summary>
        /// Encodes the <em>AND</em> bitmap for the given image according the its
        /// alpha channel (transparency) and writes it to the given output.
        /// </summary>
        /// <param name="img">
        ///            the image to encode as the <em>AND</em> bitmap. </param>
        private void WriteAndBitmap(BufferedImage img)
        {
            WritableRaster alpha = img.getAlphaRaster();

            // indexed transparency (eg. GIF files)
            if (img.getColorModel() is IndexColorModel && img.getColorModel().hasAlpha())
            {
                int w = img.getWidth();
                int h = img.getHeight();

                int bytesPerLine = GetBytesPerLine1(w);

                byte[] line = new byte[bytesPerLine];

                IndexColorModel icm = (IndexColorModel)img.getColorModel();
                Raster raster = img.getRaster();

                for (int y = h - 1; y >= 0; y--)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int bi = x / 8;
                        int i = x % 8;
                        // int a = alpha.getSample(x, y, 0);
                        int p = raster.getSample(x, y, 0);
                        int a = icm.getAlpha(p);
                        // invert bit since and mask is applied to xor mask
                        int b = ~a & 1;
                        line[bi] = SetBit(line[bi], i, b);
                    }

                    writer.Write(line);
                }
            }
            // no transparency
            else if (alpha == null)
            {
                int h = img.getHeight();
                int w = img.getWidth();
                // calculate number of bytes per line, including 32-bit padding
                int bytesPerLine = GetBytesPerLine1(w);

                byte[] line = new byte[bytesPerLine];
                for (int i = 0; i < bytesPerLine; i++)
                {
                    line[i] = (byte)0;
                }

                for (int y = h - 1; y >= 0; y--)
                {
                    writer.Write(line);
                }
            }
            // transparency (ARGB, etc. eg. PNG)
            else
            {
                int w = img.getWidth();
                int h = img.getHeight();

                int bytesPerLine = GetBytesPerLine1(w);

                byte[] line = new byte[bytesPerLine];

                for (int y = h - 1; y >= 0; y--)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int bi = x / 8;
                        int i = x % 8;
                        int a = alpha.getSample(x, y, 0);
                        // invert bit since and mask is applied to xor mask
                        int b = ~a & 1;
                        line[bi] = SetBit(line[bi], i, b);
                    }

                    writer.Write(line);
                }
            }
        }

        private void WriteXorBitmap(BufferedImage img)
        {
            Raster raster = img.getRaster();
            switch (img.getColorModel().getPixelSize())
            {
                case 1:
                    Write1(raster);
                    break;
                case 4:
                    Write4(raster);
                    break;
                case 8:
                    Write8(raster);
                    break;
                case 24:
                    Write24(raster);
                    break;
                case 32:
                    Raster alpha = img.getAlphaRaster();
                    Write32(raster, alpha);
                    break;
            }
        }

        /// <summary>
        /// Writes the <tt>InfoHeader</tt> structure to output
        /// 
        /// </summary>
        private void WriteInfoHeader(BufferedImage img)
        {
            // Size of InfoHeader structure = 40
            writer.Write(40);
            // Width
            writer.Write(img.getWidth());
            // Height
            writer.Write(img.getHeight() * 2);
            // Planes (=1)
            writer.Write((short)1);
            // Bit count
            writer.Write((short)img.getColorModel().getPixelSize());
            // Compression
            writer.Write(0);
            // Image size - compressed size of image or 0 if Compression = 0
            writer.Write(0);
            // horizontal resolution pixels/meter
            writer.Write(0);
            // vertical resolution pixels/meter
            writer.Write(0);
            // Colors used - number of colors actually used
            writer.Write(0);
            // Colors important - number of important colors 0 = all
            writer.Write(0);
        }

        /// <summary>
        /// Writes the <tt>IconEntry</tt> structure to output
        /// </summary>
        private int WriteIconEntry(BufferedImage img, int fileOffset)
        {
            // Width 1 byte Cursor Width (16, 32 or 64)
            int width = img.getWidth();
            writer.Write((byte)(width == 256 ? 0 : width));
            // Height 1 byte Cursor Height (16, 32 or 64 , most commonly = Width)
            int height = img.getHeight();
            writer.Write((byte)(height == 256 ? 0 : height));
            // ColorCount 1 byte Number of Colors (2,16, 0=256)
            short BitCount = (short)img.getColorModel().getPixelSize();
            int NumColors = 1 << (BitCount == 32 ? 24 : (int)BitCount);
            byte ColorCount = (byte)(NumColors >= 256 ? 0 : NumColors);
            writer.Write((byte)ColorCount);
            // Reserved 1 byte =0
            writer.Write((byte)0);
            // Planes 2 byte =1
            writer.Write((short)1);
            // BitCount 2 byte bits per pixel (1, 4, 8)
            writer.Write((short)BitCount);
            // SizeInBytes 4 byte Size of (InfoHeader + ANDbitmap + XORbitmap)
            int cmapSize = GetColorMapSize(BitCount);
            int xorSize = GetBitmapSize(width, height, BitCount);
            int andSize = GetBitmapSize(width, height, 1);
            int size = 40 + cmapSize + xorSize + andSize;
            writer.Write(size);
            // FileOffset 4 byte FilePos, where InfoHeader starts
            writer.Write(fileOffset);
            return size;
        }

        /// <summary>
        /// Writes the colour map resulting from the source <tt>IndexColorModel</tt>.
        /// </summary>
        /// <param name="icm">
        ///            the source <tt>IndexColorModel</tt> </param>
        private void WriteColorMap(IndexColorModel icm)
        {
            int mapSize = icm.getMapSize();
            for (int i = 0; i < mapSize; i++)
            {
                int rgb = icm.getRGB(i);
                byte r = (byte)(rgb >> 16);
                byte g = (byte)(rgb >> 8);
                byte b = (byte)(rgb);
                writer.Write(b);
                writer.Write(g);
                writer.Write(r);
                writer.Write((byte)0);
            }
        }

        /// <summary>
        /// Calculates the number of bytes per line required for the given width in
        /// pixels, for a 1-bit bitmap. Lines are always padded to the next 4-byte
        /// boundary.
        /// </summary>
        /// <param name="width">
        ///            the width in pixels </param>
        /// <returns> the number of bytes per line </returns>
        private int GetBytesPerLine1(int width)
        {
            int ret = (int)width / 8;
            if (ret % 4 != 0)
            {
                ret = (ret / 4 + 1) * 4;
            }
            return ret;
        }

        /// <summary>
        /// Calculates the number of bytes per line required for the given with in
        /// pixels, for a 4-bit bitmap. Lines are always padded to the next 4-byte
        /// boundary.
        /// </summary>
        /// <param name="width">
        ///            the width in pixels </param>
        /// <returns> the number of bytes per line </returns>
        private int GetBytesPerLine4(int width)
        {
            int ret = (int)width / 2;
            if (ret % 4 != 0)
            {
                ret = (ret / 4 + 1) * 4;
            }
            return ret;
        }

        /// <summary>
        /// Calculates the number of bytes per line required for the given with in
        /// pixels, for a 8-bit bitmap. Lines are always padded to the next 4-byte
        /// boundary.
        /// </summary>
        /// <param name="width">
        ///            the width in pixels </param>
        /// <returns> the number of bytes per line </returns>
        private int GetBytesPerLine8(int width)
        {
            int ret = width;
            if (ret % 4 != 0)
            {
                ret = (ret / 4 + 1) * 4;
            }
            return ret;
        }

        /// <summary>
        /// Calculates the number of bytes per line required for the given with in
        /// pixels, for a 24-bit bitmap. Lines are always padded to the next 4-byte
        /// boundary.
        /// </summary>
        /// <param name="width">
        ///            the width in pixels </param>
        /// <returns> the number of bytes per line </returns>
        private int GetBytesPerLine24(int width)
        {
            int ret = width * 3;
            if (ret % 4 != 0)
            {
                ret = (ret / 4 + 1) * 4;
            }
            return ret;
        }

        /// <summary>
        /// Calculates the size in bytes of a bitmap with the specified size and
        /// colour depth.
        /// </summary>
        /// <param name="w">
        ///            the width in pixels </param>
        /// <param name="h">
        ///            the height in pixels </param>
        /// <param name="bpp">
        ///            the colour depth (bits per pixel) </param>
        /// <returns> the size of the bitmap in bytes </returns>
        private int GetBitmapSize(int w, int h, int bpp)
        {
            int bytesPerLine = 0;
            switch (bpp)
            {
                case 1:
                    bytesPerLine = GetBytesPerLine1(w);
                    break;
                case 4:
                    bytesPerLine = GetBytesPerLine4(w);
                    break;
                case 8:
                    bytesPerLine = GetBytesPerLine8(w);
                    break;
                case 24:
                    bytesPerLine = GetBytesPerLine24(w);
                    break;
                case 32:
                    bytesPerLine = w * 4;
                    break;
            }
            int ret = bytesPerLine * h;
            return ret;
        }

        /// <summary>
        /// Encodes and writes raster data as a 1-bit bitmap.
        /// </summary>
        /// <param name="raster">
        ///            the source raster data </param>
        private void Write1(Raster raster)
        {
            int bytesPerLine = GetBytesPerLine1(raster.getWidth());

            byte[] line = new byte[bytesPerLine];

            for (int y = raster.getHeight() - 1; y >= 0; y--)
            {
                for (int i = 0; i < bytesPerLine; i++)
                {
                    line[i] = 0;
                }

                for (int x = 0; x < raster.getWidth(); x++)
                {
                    int bi = x / 8;
                    int i = x % 8;
                    int index = raster.getSample(x, y, 0);
                    line[bi] = SetBit(line[bi], i, index);
                }

                writer.Write(line);
            }
        }

        /// <summary>
        /// Encodes and writes raster data as a 4-bit bitmap.
        /// </summary>
        /// <param name="raster">
        ///            the source raster data </param>
        private void Write4(Raster raster)
        {
            int width = raster.getWidth();
            int height = raster.getHeight();

            // calculate bytes per line
            int bytesPerLine = GetBytesPerLine4(width);

            // line buffer
            byte[] line = new byte[bytesPerLine];

            // encode and write lines
            for (int y = height - 1; y >= 0; y--)
            {

                // clear line buffer
                for (int i = 0; i < bytesPerLine; i++)
                {
                    line[i] = 0;
                }

                // encode raster data for line
                for (int x = 0; x < width; x++)
                {

                    // calculate buffer index
                    int bi = x / 2;

                    // calculate nibble index (high order or low order)
                    int i = x % 2;

                    // get color index
                    int index = raster.getSample(x, y, 0);
                    // set color index in buffer
                    line[bi] = SetNibble(line[bi], i, index);
                }

                // write line data (padding bytes included)
                writer.Write(line);
            }
        }

        /// <summary>
        /// Encodes and writes raster data as an 8-bit bitmap.
        /// </summary>
        /// <param name="raster">
        ///            the source raster data </param>
        private void Write8(Raster raster)
        {
            int width = raster.getWidth();
            int height = raster.getHeight();

            // calculate bytes per line
            int bytesPerLine = GetBytesPerLine8(width);

            // write lines
            for (int y = height - 1; y >= 0; y--)
            {

                // write raster data for each line
                for (int x = 0; x < width; x++)
                {

                    // get color index for pixel
                    byte index = (byte)raster.getSample(x, y, 0);

                    // write color index
                    writer.Write(index);
                }

                // write padding bytes at end of line
                for (int i = width; i < bytesPerLine; i++)
                {
                    writer.Write((byte)0);
                }

            }
        }

        /// <summary>
        /// Encodes and writes raster data as a 24-bit bitmap.
        /// </summary>
        /// <param name="raster">
        ///            the source raster data </param>
        private void Write24(Raster raster)
        {
            int width = raster.getWidth();
            int height = raster.getHeight();

            // calculate bytes per line
            int bytesPerLine = GetBytesPerLine24(width);

            // write lines
            for (int y = height - 1; y >= 0; y--)
            {

                // write pixel data for each line
                for (int x = 0; x < width; x++)
                {

                    // get RGB values for pixel
                    byte r = (byte)raster.getSample(x, y, 0);
                    byte g = (byte)raster.getSample(x, y, 1);
                    byte b = (byte)raster.getSample(x, y, 2);

                    // write RGB values
                    writer.Write(b);
                    writer.Write(g);
                    writer.Write(r);
                }

                // write padding bytes at end of line
                for (int i = width * 3; i < bytesPerLine; i++)
                {
                    writer.Write((byte)0);
                }
            }
        }

        /// <summary>
        /// Encodes and writes raster data, together with alpha (transparency) data,
        /// as a 32-bit bitmap.
        /// </summary>
        /// <param name="raster">
        ///            the source raster data </param>
        /// <param name="alpha">
        ///            the source alpha data </param>
        private void Write32(Raster raster, Raster alpha)
        {
            int width = raster.getWidth();
            int height = raster.getHeight();

            // write lines
            for (int y = height - 1; y >= 0; y--)
            {

                // write pixel data for each line
                for (int x = 0; x < width; x++)
                {

                    // get RGBA values
                    byte r = (byte)raster.getSample(x, y, 0);
                    byte g = (byte)raster.getSample(x, y, 1);
                    byte b = (byte)raster.getSample(x, y, 2);
                    byte a = (byte)alpha.getSample(x, y, 0);

                    // write RGBA values
                    writer.Write(b);
                    writer.Write(g);
                    writer.Write(r);
                    writer.Write(a);
                }
            }
        }

        /// <summary>
        /// Sets a particular bit in a byte.
        /// </summary>
        /// <param name="bits">
        ///            the source byte </param>
        /// <param name="index">
        ///            the index of the bit to set </param>
        /// <param name="bit">
        ///            the value for the bit, which should be either <tt>0</tt> or
        ///            <tt>1</tt>. </param>
        /// <param name="the">
        ///            resultant byte </param>
        private byte SetBit(byte bits, int index, int bit)
        {
            if (bit == 0)
            {
                bits &= (byte)~(1 << (7 - index));
            }
            else
            {
                bits |= (byte)(1 << (7 - index));
            }
            return bits;
        }

        /// <summary>
        /// Sets a particular nibble (4 bits) in a byte.
        /// </summary>
        /// <param name="nibbles">
        ///            the source byte </param>
        /// <param name="index">
        ///            the index of the nibble to set </param>
        /// <param name="the">
        ///            value for the nibble, which should be in the range
        ///            <tt>0x0..0xF</tt>. </param>
        private byte SetNibble(byte nibbles, int index, int nibble)
        {
            nibbles |= (byte)(nibble << ((1 - index) * 4));

            return nibbles;
        }

        /// <summary>
        /// Calculates the size in bytes for a colour map with the specified bit
        /// count.
        /// </summary>
        /// <param name="sBitCount">
        ///            the bit count, which represents the colour depth </param>
        /// <returns> the size of the colour map, in bytes if <tt>sBitCount</tt> is
        ///         less than or equal to 8, otherwise <tt>0</tt> as colour maps are
        ///         only used for bitmaps with a colour depth of 8 bits or less. </returns>
        private int GetColorMapSize(short sBitCount)
        {
            int ret = 0;
            if (sBitCount <= 8)
            {
                ret = (1 << sBitCount) * 4;
            }
            return ret;
        }

    }
}
