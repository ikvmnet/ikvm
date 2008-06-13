package sun.awt.image;

// HACK workaround to make /jdk/src/share/classes/com/sun/imageio/plugins/png/PNGImageReader.java compile

public abstract class ByteInterleavedRaster extends java.awt.image.WritableRaster
{
    ByteInterleavedRaster()
    {
	super(null, null);
    }
}
