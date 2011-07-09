/*
  Copyright (C) 2009 Volker Berlin (i-net software)

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
package sun.java2d;

import java.awt.*;
import java.awt.RenderingHints.Key;
import java.awt.font.FontRenderContext;
import java.awt.font.GlyphVector;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ImageObserver;
import java.awt.image.RenderedImage;
import java.awt.image.renderable.RenderableImage;
import java.text.AttributedCharacterIterator;
import java.util.Map;

import sun.java2d.pipe.Region;
import ikvm.internal.NotYetImplementedError;

/**
 * A replacement of the Sun implementation that redirect to the NetGraphics
 */
public class SunGraphics2D extends Graphics2D{
    /*
     * Attribute States
     */
    /* Paint */
    public static final int PAINT_CUSTOM       = 6; /* Any other Paint object */
    public static final int PAINT_TEXTURE      = 5; /* Tiled Image */
    public static final int PAINT_RAD_GRADIENT = 4; /* Color RadialGradient */
    public static final int PAINT_LIN_GRADIENT = 3; /* Color LinearGradient */
    public static final int PAINT_GRADIENT     = 2; /* Color Gradient */
    public static final int PAINT_ALPHACOLOR   = 1; /* Non-opaque Color */
    public static final int PAINT_OPAQUECOLOR  = 0; /* Opaque Color */

    /* Composite*/
    public static final int COMP_CUSTOM = 3;/* Custom Composite */
    public static final int COMP_XOR    = 2;/* XOR Mode Composite */
    public static final int COMP_ALPHA  = 1;/* AlphaComposite */
    public static final int COMP_ISCOPY = 0;/* simple stores into destination,
                                             * i.e. Src, SrcOverNoEa, and other
                                             * alpha modes which replace
                                             * the destination.
                                             */

    /* Stroke */
    public static final int STROKE_CUSTOM = 3; /* custom Stroke */
    public static final int STROKE_WIDE   = 2; /* BasicStroke */
    public static final int STROKE_THINDASHED   = 1; /* BasicStroke */
    public static final int STROKE_THIN   = 0; /* BasicStroke */

    /* Transform */
    public static final int TRANSFORM_GENERIC = 4; /* any 3x2 */
    public static final int TRANSFORM_TRANSLATESCALE = 3; /* scale XY */
    public static final int TRANSFORM_ANY_TRANSLATE = 2; /* non-int translate */
    public static final int TRANSFORM_INT_TRANSLATE = 1; /* int translate */
    public static final int TRANSFORM_ISIDENT = 0; /* Identity */

    /* Clipping */
    public static final int CLIP_SHAPE       = 2; /* arbitrary clip */
    public static final int CLIP_RECTANGULAR = 1; /* rectangular clip */
    public static final int CLIP_DEVICE      = 0; /* no clipping set */

    private static SurfaceData surfaceData = new SurfaceData();
    
    public int strokeState;

    public Stroke stroke;
    public int strokeHint;

    public Region clipRegion;
    public int constrainX;
    public int constrainY;

    public AffineTransform transform;

    /** a instance of cli.ikvm.awt.NetGraphics */
    private final Graphics2D graphics;
    
    /**
     * TODO implement the real Constructor
     */
    private SunGraphics2D(){
        graphics = null;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void addRenderingHints(Map<?, ?> hints){
        graphics.addRenderingHints(hints);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void clearRect(int x, int y, int width, int height){
        graphics.clearRect(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void clip(Shape s){
        graphics.clip(s);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void clipRect(int x, int y, int width, int height){
        graphics.clipRect(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void copyArea(int x, int y, int width, int height, int dx, int dy){
        graphics.copyArea(x, y, width, height, dx, dy);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Graphics create(){
        return graphics.create();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Graphics create(int x, int y, int width, int height){
        return graphics.create(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void dispose(){
        graphics.dispose();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void draw(Shape s){
        graphics.draw(s);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void draw3DRect(int x, int y, int width, int height, boolean raised){
        graphics.draw3DRect(x, y, width, height, raised);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawArc(int x, int y, int width, int height, int arcStart, int arcAngle){
        graphics.drawArc(x, y, width, height, arcStart, arcAngle);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawBytes(byte[] data, int offset, int length, int x, int y){
        graphics.drawBytes(data, offset, length, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawChars(char[] data, int offset, int length, int x, int y){
        graphics.drawChars(data, offset, length, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawGlyphVector(GlyphVector g, float x, float y){
        graphics.drawGlyphVector(g, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawImage(BufferedImage img, BufferedImageOp op, int x, int y){
        graphics.drawImage(img, op, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image img, AffineTransform xform, ImageObserver obs){
        return graphics.drawImage(img, xform, obs);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int x, int y, Color bgcolor, ImageObserver observer){
        return graphics.drawImage(image, x, y, bgcolor, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int x, int y, ImageObserver observer){
        return graphics.drawImage(image, x, y, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int x, int y, int width, int height, Color bgcolor, ImageObserver observer){
        return graphics.drawImage(image, x, y, width, height, bgcolor, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int x, int y, int width, int height, ImageObserver observer){
        return graphics.drawImage(image, x, y, width, height, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2,
            Color bgcolor, ImageObserver observer){
        return graphics.drawImage(image, dx1, dy1, dx2, dy2, sx1, sy1, sx2, sy2, bgcolor, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean drawImage(Image image, int dx1, int dy1, int dx2, int dy2, int sx1, int sy1, int sx2, int sy2,
            ImageObserver observer){
        return graphics.drawImage(image, dx1, dy1, dx2, dy2, sx1, sy1, sx2, sy2, observer);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawLine(int x1, int y1, int x2, int y2){
        graphics.drawLine(x1, y1, x2, y2);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawOval(int x, int y, int width, int height){
        graphics.drawOval(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawPolygon(int[] points, int[] points2, int npoints){
        graphics.drawPolygon(points, points2, npoints);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawPolygon(Polygon polygon){
        graphics.drawPolygon(polygon);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawPolyline(int[] points, int[] points2, int npoints){
        graphics.drawPolyline(points, points2, npoints);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawRect(int x, int y, int width, int height){
        graphics.drawRect(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawRenderableImage(RenderableImage img, AffineTransform xform){
        graphics.drawRenderableImage(img, xform);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawRenderedImage(RenderedImage img, AffineTransform xform){
        graphics.drawRenderedImage(img, xform);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawRoundRect(int x, int y, int width, int height, int arcWidth, int arcHeight){
        graphics.drawRoundRect(x, y, width, height, arcWidth, arcHeight);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawString(AttributedCharacterIterator iterator, float x, float y){
        graphics.drawString(iterator, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawString(AttributedCharacterIterator iterator, int x, int y){
        graphics.drawString(iterator, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawString(String str, float x, float y){
        graphics.drawString(str, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void drawString(String str, int x, int y){
        graphics.drawString(str, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean equals(Object obj){
        return graphics.equals(obj);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fill(Shape s){
        graphics.fill(s);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fill3DRect(int x, int y, int width, int height, boolean raised){
        graphics.fill3DRect(x, y, width, height, raised);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillArc(int x, int y, int width, int height, int arcStart, int arcAngle){
        graphics.fillArc(x, y, width, height, arcStart, arcAngle);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillOval(int x, int y, int width, int height){
        graphics.fillOval(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillPolygon(int[] points, int[] points2, int npoints){
        graphics.fillPolygon(points, points2, npoints);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillPolygon(Polygon polygon){
        graphics.fillPolygon(polygon);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillRect(int x, int y, int width, int height){
        graphics.fillRect(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void fillRoundRect(int x, int y, int width, int height, int arcWidth, int arcHeight){
        graphics.fillRoundRect(x, y, width, height, arcWidth, arcHeight);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void finalize(){
        graphics.finalize();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Color getBackground(){
        return graphics.getBackground();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Shape getClip(){
        return graphics.getClip();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Rectangle getClipBounds(){
        return graphics.getClipBounds();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Rectangle getClipBounds(Rectangle r){
        return graphics.getClipBounds(r);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Rectangle getClipRect(){
        return graphics.getClipRect();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Color getColor(){
        return graphics.getColor();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Composite getComposite(){
        return graphics.getComposite();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public GraphicsConfiguration getDeviceConfiguration(){
        return graphics.getDeviceConfiguration();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Font getFont(){
        return graphics.getFont();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public FontMetrics getFontMetrics(){
        return graphics.getFontMetrics();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public FontMetrics getFontMetrics(Font font){
        return graphics.getFontMetrics(font);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public FontRenderContext getFontRenderContext(){
        return graphics.getFontRenderContext();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Paint getPaint(){
        return graphics.getPaint();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Object getRenderingHint(Key hintKey){
        return graphics.getRenderingHint(hintKey);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public RenderingHints getRenderingHints(){
        return graphics.getRenderingHints();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Stroke getStroke(){
        return graphics.getStroke();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public AffineTransform getTransform(){
        return graphics.getTransform();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public int hashCode(){
        return graphics.hashCode();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean hit(Rectangle rect, Shape s, boolean onStroke){
        return graphics.hit(rect, s, onStroke);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean hitClip(int x, int y, int width, int height){
        return graphics.hitClip(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void rotate(double theta, double x, double y){
        graphics.rotate(theta, x, y);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void rotate(double theta){
        graphics.rotate(theta);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void scale(double sx, double sy){
        graphics.scale(sx, sy);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setBackground(Color color){
        graphics.setBackground(color);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setClip(int x, int y, int width, int height){
        graphics.setClip(x, y, width, height);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setClip(Shape clip){
        graphics.setClip(clip);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setColor(Color color){
        graphics.setColor(color);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setComposite(Composite comp){
        graphics.setComposite(comp);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setFont(Font font){
        graphics.setFont(font);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setPaint(Paint paint){
        graphics.setPaint(paint);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setPaintMode(){
        graphics.setPaintMode();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setRenderingHint(Key hintKey, Object hintValue){
        graphics.setRenderingHint(hintKey, hintValue);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setRenderingHints(Map<?, ?> hints){
        graphics.setRenderingHints(hints);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setStroke(Stroke s){
        graphics.setStroke(s);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setTransform(AffineTransform Tx){
        graphics.setTransform(Tx);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void setXORMode(Color color){
        graphics.setXORMode(color);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void shear(double shx, double shy){
        graphics.shear(shx, shy);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public String toString(){
        return graphics.toString();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void transform(AffineTransform Tx){
        graphics.transform(Tx);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void translate(double tx, double ty){
        graphics.translate(tx, ty);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void translate(int x, int y){
        graphics.translate(x, y);
    }
    
    /*
     * Special functions of the original SunGraphics2D
     * 
     */
    
    /*
     * Intersect usrClip bounds and device bounds to determine the composite
     * rendering boundaries.
     */
    public Region getCompClip() {
        throw new NotYetImplementedError();
    }

    /**
     * Constrain rendering for lightweight objects.
     *
     * REMIND: This method will back off to the "workaround"
     * of using translate and clipRect if the Graphics
     * to be constrained has a complex transform.  The
     * drawback of the workaround is that the resulting
     * clip and device origin cannot be "enforced".
     *
     * @exception IllegalStateException If the Graphics
     * to be constrained has a complex transform.
     */
    public void constrain(int i, int j, int k, int l){
        throw new NotYetImplementedError();
    }
    
    /**
     * Return the SurfaceData object assigned to manage the destination
     * drawable surface of this Graphics2D.
     */
    public final SurfaceData getSurfaceData() {
        return surfaceData;
    }
    
    /**
     * Returns destination that this Graphics renders to.  This could be
     * either an Image or a Component; subclasses of SurfaceData are
     * responsible for returning the appropriate object.
     */
    public Object getDestination() {
        throw new NotYetImplementedError();
    }
}
