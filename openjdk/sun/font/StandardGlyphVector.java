/*
  Copyright (C) 2009, 2010 Volker Berlin (i-net software)

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
package sun.font;

import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.Shape;
import java.awt.Toolkit;
import java.awt.font.FontRenderContext;
import java.awt.font.GlyphJustificationInfo;
import java.awt.font.GlyphMetrics;
import java.awt.font.GlyphVector;
import java.awt.geom.AffineTransform;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.awt.image.BufferedImage;
import java.text.CharacterIterator;
import java.util.WeakHashMap;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

/**
 * Standard implementation of GlyphVector used by Font, GlyphList, and SunGraphics2D.
 * 
 */
public class StandardGlyphVector extends GlyphVector{

    private float[] positions; // only if not default advances

    private final Font font;

    private final FontRenderContext frc;

    private final String glyphs;

    private transient FontMetrics metrics;

    private Font2D font2D;
    private FontStrike strike;
    
    private static WeakHashMap<FontRenderContext, Graphics2D> graphicsMap = new WeakHashMap<FontRenderContext, Graphics2D>();

    public StandardGlyphVector(Font font, String str, FontRenderContext frc){
        if(str == null){
            throw new NullPointerException("Glyphs are null");
        }
        this.font = font;
        this.frc = frc;
        this.glyphs = str;
        this.font2D = FontManager.getFont2D(font);
        this.strike = font2D.getStrike(font, frc);
    }


    public StandardGlyphVector(Font font, CharacterIterator ci, FontRenderContext frc){
        this(font, getString(ci), frc);
    }


    public StandardGlyphVector(Font font, int[] glyphCodes, FontRenderContext frc){
        throw new NotImplementedException();
    }


    public StandardGlyphVector(Font font, char[] chars, FontRenderContext frc){
        this(font, chars, 0, chars.length, frc);
    }


    public StandardGlyphVector(Font font, char[] chars, int beginIndex, int length, FontRenderContext frc){
        this(font, new String(chars, beginIndex, length), frc);
    }


    /**
     * Create and get
     * 
     * @return
     */
    private FontMetrics getMetrics(){
        if(metrics == null){
            metrics = Toolkit.getDefaultToolkit().getFontMetrics(font);
        }
        return metrics;
    }


    /**
     * As a concrete subclass of GlyphVector, this must implement clone.
     */
    @Override
    public Object clone(){
        try{
            StandardGlyphVector result = (StandardGlyphVector)super.clone();

            return result;
        }catch(CloneNotSupportedException e){
            e.printStackTrace();
        }

        return this;
    }


    // ////////////////////
    // StandardGlyphVector new public methods
    // ///////////////////

    /**
     * Set all the glyph positions, including the 'after last glyph' position. The srcPositions array must be of length
     * (numGlyphs + 1) * 2.
     */
    public void setGlyphPositions(float[] srcPositions){
        positions = srcPositions.clone();
    }


    /**
     * This is a convenience overload that gets all the glyph positions, which is what you usually want to do if you're
     * getting more than one. !!! should I bother taking result parameter?
     */
    public float[] getGlyphPositions(float[] result){
        initPositions();
        return positions;
    }


    /**
     * For each glyph return posx, posy, advx, advy, visx, visy, visw, vish.
     */
    public float[] getGlyphInfo(){
        initPositions();
        float[] result = new float[glyphs.length() * 8];
        for (int i = 0, n = 0; i < glyphs.length(); ++i, n += 8) {
            float x = positions[i*2];
            float y = positions[i*2+1];
            result[n] = x;
            result[n+1] = y;

            int glyphID = glyphs.charAt(i);
            Point2D.Float adv = strike.getGlyphMetrics(glyphID);
            result[n+2] = adv.x;
            result[n+3] = adv.y;

            Rectangle2D vb = getGlyphVisualBounds(i).getBounds2D();
            result[n+4] = (float)(vb.getMinX());
            result[n+5] = (float)(vb.getMinY());
            result[n+6] = (float)(vb.getWidth());
            result[n+7] = (float)(vb.getHeight());
        }
        return result;
    }


    @Override
    public boolean equals(GlyphVector gv){
        if(!(gv instanceof StandardGlyphVector)){
            return false;
        }
        StandardGlyphVector sgv = (StandardGlyphVector)gv;
        if(!glyphs.equals(sgv.glyphs)){
            return false;
        }
        if(equals(font, sgv.font)){
            return false;
        }
        if(equals(frc, sgv.frc)){
            return false;
        }
        return true;
    }
    
    /**
     * Compare 2 objects via equals where both can be null
     */
    private static boolean equals(Object obj1, Object obj2){
        if(obj1 != null){
            if(!obj1.equals(obj2)){
                return false;
            }
        }else{
            if(obj2 != null){
                return false;
            }
        }
        return true;
    }


    @Override
    public Font getFont(){
        return font;
    }


    @Override
    public FontRenderContext getFontRenderContext(){
        return frc;
    }


    @Override
    public int getGlyphCode(int glyphIndex){
        return glyphs.charAt(glyphIndex);
    }


    @Override
    public int[] getGlyphCodes(int beginGlyphIndex, int numEntries, int[] codeReturn){
        if(codeReturn == null || codeReturn.length < numEntries){
            codeReturn = new int[numEntries];
        }
        for(int i=0; i<numEntries; i++){
            codeReturn[i] = glyphs.charAt(i + beginGlyphIndex);
        }
        return codeReturn;
    }


    @Override
    public GlyphJustificationInfo getGlyphJustificationInfo(int glyphIndex){
        throw new NotImplementedException();
    }


    @Override
    public GlyphMetrics getGlyphMetrics(int glyphIndex){
        throw new NotImplementedException();
    }


    @Override
    public Shape getGlyphOutline(int glyphIndex){
        throw new NotImplementedException();
    }


    @Override
    public Point2D getGlyphPosition(int glyphIndex){
        throw new NotImplementedException();
    }


    @Override
    public float[] getGlyphPositions(int beginGlyphIndex, int numEntries, float[] positionReturn){
        throw new NotImplementedException();
    }


    @Override
    public AffineTransform getGlyphTransform(int glyphIndex){
        throw new NotImplementedException();
    }

    /**
     * Get/Create a Graphics with the settings of the current FontMetrics
     * 
     * @return
     */
    private Graphics2D getGraphics() {
        Graphics2D g = graphicsMap.get( frc );
        if( g == null ) {
            BufferedImage img = new BufferedImage( 1, 1, BufferedImage.TYPE_INT_ARGB );
            g = (Graphics2D)img.getGraphics();
            if( frc.usesFractionalMetrics() ) {
                g.setRenderingHint( RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_ON );
            }
            graphicsMap.put( frc, g );
        }
        return g;
    }
    

    @Override
    public Shape getGlyphLogicalBounds( int glyphIndex ) {
        return getMetrics().getStringBounds( glyphs.substring( glyphIndex, glyphIndex + 1 ), getGraphics() );
    }
    

    @Override
    public Shape getGlyphVisualBounds( int glyphIndex ) {
        // TODO Visual is a little smaller, see the JUnit test
        return getGlyphLogicalBounds( glyphIndex );
    }

    @Override
    public Rectangle2D getLogicalBounds(){
        return getMetrics().getStringBounds(glyphs, getGraphics());
    }


    @Override
    public Rectangle2D getVisualBounds(){
        // TODO Visual is a little smaller, see the JUnit test
        return getLogicalBounds();
    }


    @Override
    public int getNumGlyphs(){
        return glyphs.length();
    }


    @Override
    public Shape getOutline(){
        throw new NotImplementedException();
    }


    @Override
    public Shape getOutline(float x, float y){
        throw new NotImplementedException();
    }


    @Override
    public void performDefaultLayout(){
        throw new NotImplementedException();
    }


    @Override
    public void setGlyphPosition(int glyphIndex, Point2D newPos){
        throw new NotImplementedException();
    }


    @Override
    public void setGlyphTransform(int glyphIndex, AffineTransform newTX){
        throw new NotImplementedException();
    }


    /**
     * Convert a CharacterIterator to a string
     * @param iterator the iterator
     * @return the string
     */
    private static String getString(java.text.CharacterIterator iterator){
        iterator.first();
        StringBuilder sb = new StringBuilder();

        while(true){
            char c = iterator.current();
            if(c == CharacterIterator.DONE){
                break;
            }
            sb.append(c);
            iterator.next();
        }

        return sb.toString();
    }

    private float getTracking(Font font) {
        if (font.hasLayoutAttributes()) {
            AttributeValues values = ((AttributeMap)font.getAttributes()).getValues();
            return values.getTracking();
        }
        return 0;
    }

    /**
     * Ensure that the positions array exists and holds position data.
     * If the array is null, this allocates it and sets default positions.
     */
    private void initPositions() {
        if (positions == null) {
            positions = new float[glyphs.length() * 2 + 2];

            Point2D.Float trackPt = null;
            float track = getTracking(font);
            if (track != 0) {
                track *= font.getSize2D();
                trackPt = new Point2D.Float(track, 0); // advance delta
            }

            Point2D.Float pt = new Point2D.Float(0, 0);
            if (font.isTransformed()) {
                AffineTransform at = font.getTransform();
                at.transform(pt, pt);
                positions[0] = pt.x;
                positions[1] = pt.y;

                if (trackPt != null) {
                    at.deltaTransform(trackPt, trackPt);
                }
            }
            for (int i = 0, n = 2; i < glyphs.length(); ++i, n += 2) {
                addDefaultGlyphAdvance(glyphs.charAt(i), pt);
                if (trackPt != null) {
                    pt.x += trackPt.x;
                    pt.y += trackPt.y;
                }
                positions[n] = pt.x;
                positions[n+1] = pt.y;
            }
        }
    }

    void addDefaultGlyphAdvance(int glyphID, Point2D.Float result) {
        Point2D.Float adv = strike.getGlyphMetrics(glyphID);
        result.x += adv.x;
        result.y += adv.y;
    }
}
