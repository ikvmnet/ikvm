package gnu.java.awt.peer.gtk;

import java.awt.image.BufferedImage;

// HACK this is a placeholder to make gnu.java.awt.JavaPrinterGraphics compile
public class CairoSurface
{
    public static BufferedImage getBufferedImage(int width, int height)
    {
        return new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);
    }
}
