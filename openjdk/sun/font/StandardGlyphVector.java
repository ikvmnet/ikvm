/*
  Copyright (C) 2009 - 2013 Volker Berlin (i-net software)

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

import ikvm.awt.IkvmToolkit;

import java.awt.Font;
import java.awt.Shape;
import java.awt.font.FontRenderContext;
import java.awt.font.GlyphMetrics;
import java.awt.font.GlyphJustificationInfo;
import java.awt.font.GlyphVector;
import java.awt.font.LineMetrics;
import java.awt.geom.AffineTransform;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.text.CharacterIterator;

import ikvm.internal.NotYetImplementedError;

/**
 * Standard implementation of GlyphVector used by Font, GlyphList, and SunGraphics2D.
 * 
 */
public class StandardGlyphVector extends GlyphVector{
    private Font font;
    private FontRenderContext frc;
    private final String glyphs; // always
    private float[] positions; // only if not default advances



    private Font2D font2D;
    private FontStrike strike;
    
    
    /////////////////////////////
    // Constructors and Factory methods
    /////////////////////////////

    public StandardGlyphVector(Font font, String str, FontRenderContext frc) {
        if(str == null){
            throw new NullPointerException("Glyphs are null");
        }
        this.font = font;
        if( frc == null ){
            frc = new FontRenderContext( null, false, false );
        }
        this.frc = frc;
        this.glyphs = str;
        this.font2D = FontUtilities.getFont2D(font);
        this.strike = font2D.getStrike(font, frc);
    }

    public StandardGlyphVector(Font font, char[] text, FontRenderContext frc) {
        this(font, text, 0, text.length, frc);
    }

    public StandardGlyphVector(Font font, char[] text, int start, int count,
                               FontRenderContext frc) {
        this(font, new String(text, start, count), frc);
    }

    private float getTracking(Font font) {
        if (font.hasLayoutAttributes()) {
            AttributeValues values = ((AttributeMap)font.getAttributes()).getValues();
            return values.getTracking();
        }
        return 0;
    }

    public StandardGlyphVector(Font font, CharacterIterator iter, FontRenderContext frc) {
        this(font, getString(iter), frc);
    }

    public StandardGlyphVector( Font font, int[] glyphs, FontRenderContext frc ) {
        this( font, glyphs2chars(glyphs), frc );
    }
    
    /**
     * Symmetric to {@link #getGlyphCodes(int, int, int[])}
     * Currently there is no real mapping possible between the chars and the glyph IDs in the TTF file
     */
    private static char[] glyphs2chars( int[] glyphs ) {
        int count = glyphs.length;
        char[] text = new char[count];
        for( int i = 0; i < count; ++i ) {
            text[i] = (char)glyphs[i];
        }
        return text;
    }

    /////////////////////////////
    // GlyphVector API
    /////////////////////////////

    @Override
    public Font getFont() {
        return this.font;
    }

    @Override
    public FontRenderContext getFontRenderContext() {
        return this.frc;
    }

    @Override
    public void performDefaultLayout() {
        positions = null;
    }

    @Override
    public int getNumGlyphs() {
        return glyphs.length();
    }

    @Override
    public int getGlyphCode(int glyphIndex) {
        return glyphs.charAt(glyphIndex);
    }

    @Override
    public int[] getGlyphCodes(int start, int count, int[] result) {
        if (count < 0) {
            throw new IllegalArgumentException("count = " + count);
        }
        if (start < 0) {
            throw new IndexOutOfBoundsException("start = " + start);
        }        
        if (start > glyphs.length() - count) { // watch out for overflow if index + count overlarge
            throw new IndexOutOfBoundsException("start + count = " + (start + count));
        }

        if (result == null) {
            result = new int[count];
        }
        for (int i = 0; i < count; ++i) {
            result[i] = glyphs.charAt(i + start);
        }
        return result;
    }

    // !!! not cached, assume TextLayout will cache if necessary
    // !!! reexamine for per-glyph-transforms
    // !!! revisit for text-on-a-path, vertical
    @Override
    public Rectangle2D getLogicalBounds() {
        initPositions();

        LineMetrics lm = font.getLineMetrics("", frc);

        float minX, minY, maxX, maxY;
        // horiz only for now...
        minX = 0;
        minY = -lm.getAscent();
        maxX = 0;
        maxY = lm.getDescent() + lm.getLeading();
        if (glyphs.length() > 0) {
            maxX = positions[positions.length - 2];
        }

        return new Rectangle2D.Float(minX, minY, maxX - minX, maxY - minY);
    }

    // !!! not cached, assume TextLayout will cache if necessary
    @Override
    public Rectangle2D getVisualBounds() {
        return getOutline().getBounds2D();
    }

    @Override
    public Shape getOutline() {
        return getOutline( 0, 0 );
    }

    @Override
    public Shape getOutline(float x, float y) {
        return IkvmToolkit.DefaultToolkit.get().outline( font, frc, glyphs, x, y );
    }

    @Override
    public Shape getGlyphOutline( int glyphIndex ) {
        return getGlyphOutline( glyphIndex, 0, 0 );
    }

    @Override
    public Shape getGlyphOutline( int glyphIndex, float x, float y ) {
        initPositions();
        
        return IkvmToolkit.DefaultToolkit.get().outline( font, frc, glyphs.substring( glyphIndex, glyphIndex + 1 ), x + positions[glyphIndex * 2], y );
    }
    
    @Override
    public Point2D getGlyphPosition(int ix) {
        initPositions();

        ix *= 2;
        return new Point2D.Float(positions[ix], positions[ix + 1]);
    }

    @Override
    public void setGlyphPosition(int ix, Point2D pos) {
        initPositions();

        int ix2 = ix << 1;
        positions[ix2] = (float)pos.getX();
        positions[ix2 + 1] = (float)pos.getY();
    }

    @Override
    public AffineTransform getGlyphTransform(int ix) {
        throw new NotYetImplementedError();
    }

    @Override
    public float[] getGlyphPositions(int start, int count, float[] result) {
        if (count < 0) {
            throw new IllegalArgumentException("count = " + count);
        }
        if (start < 0) {
            throw new IndexOutOfBoundsException("start = " + start);
        }
        if (start > this.glyphs.length() + 1 - count) {
            throw new IndexOutOfBoundsException("start + count = " + (start + count));
        }
        int count2 = count * 2;
        if( result == null ) {
            result = new float[count2];
        }
        initPositions();
        System.arraycopy( positions, start * 2, result, 0, count2 );
        return result;
    }

    @Override
    public Shape getGlyphLogicalBounds(int ix) {
        if (ix < 0 || ix >= glyphs.length()) {
            throw new IndexOutOfBoundsException("ix = " + ix);
        }

        initPositions();
        StrikeMetrics metrics = strike.getFontMetrics();
        float x = positions[ix * 2];
        return new Rectangle2D.Float( x, -metrics.getAscent(), positions[(ix + 1) * 2] - x, metrics.getAscent()
                        + metrics.getDescent() + metrics.getLeading() );
    }
    
    @Override
    public Shape getGlyphVisualBounds(int ix) {
        if (ix < 0 || ix >= glyphs.length()) {
            throw new IndexOutOfBoundsException("ix = " + ix);
        }

        initPositions();
        return IkvmToolkit.DefaultToolkit.get().outline( font, frc, glyphs.substring( ix, ix + 1 ), positions[ix * 2], 0 );
    }

    @Override
    public GlyphMetrics getGlyphMetrics(int ix) {
        if (ix < 0 || ix >= glyphs.length()) {
            throw new IndexOutOfBoundsException("ix = " + ix);
        }

        Rectangle2D vb = getGlyphVisualBounds(ix).getBounds2D();
        Point2D pt = getGlyphPosition(ix);
        vb.setRect(vb.getMinX() - pt.getX(),
                   vb.getMinY() - pt.getY(),
                   vb.getWidth(),
                   vb.getHeight());
        Point2D.Float adv =
        		strike.getGlyphMetrics( glyphs.charAt( ix ) );
        GlyphMetrics gm = new GlyphMetrics(true, adv.x, adv.y,
                                           vb,
                                          (byte)0);
        return gm;
    }

    @Override
    public GlyphJustificationInfo getGlyphJustificationInfo(int ix) {
        if (ix < 0 || ix >= glyphs.length()) {
            throw new IndexOutOfBoundsException("ix = " + ix);
        }

        // currently we don't have enough information to do this right.  should
        // get info from the font and use real OT/GX justification.  Right now
        // sun/font/ExtendedTextSourceLabel assigns one of three infos
        // based on whether the char is kanji, space, or other.

        return null;
    }

    @Override
    public boolean equals(GlyphVector rhs) {
        if(!(rhs instanceof StandardGlyphVector)){
            return false;
        }
        StandardGlyphVector sgv = (StandardGlyphVector)rhs;
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

    /**
     * As a concrete subclass of GlyphVector, this must implement clone.
     */
    @Override
    public Object clone() {
        // positions, gti are mutable so we have to clone them
        // font2d can be shared
        // fsref is a cache and can be shared
        try {
            StandardGlyphVector result = (StandardGlyphVector)super.clone();

            if (positions != null) {
                result.positions = (float[])positions.clone();
            }

            return result;
        }
        catch (CloneNotSupportedException e) {
        }

        return this;
    }

    //////////////////////
    // StandardGlyphVector new public methods
    /////////////////////

    /**
     * Set all the glyph positions, including the 'after last glyph' position.
     * The srcPositions array must be of length (numGlyphs + 1) * 2.
     */
    public void setGlyphPositions(float[] srcPositions) {
        int requiredLength = glyphs.length() * 2 + 2;
        if (srcPositions.length != requiredLength) {
            throw new IllegalArgumentException("srcPositions.length != " + requiredLength);
        }

        positions = (float[])srcPositions.clone();

    }

    /**
     * This is a convenience overload that gets all the glyph positions, which
     * is what you usually want to do if you're getting more than one.
     * !!! should I bother taking result parameter?
     */
    public float[] getGlyphPositions(float[] result) {
        initPositions();
        return positions;
    }

    /**
     * For each glyph return posx, posy, advx, advy, visx, visy, visw, vish.
     */
    public float[] getGlyphInfo() {
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
    public void setGlyphTransform(int glyphIndex, AffineTransform newTX){
        throw new NotYetImplementedError();
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

    private void addDefaultGlyphAdvance(int glyphID, Point2D.Float result) {
        Point2D.Float adv = strike.getGlyphMetrics(glyphID);
        result.x += adv.x;
        result.y += adv.y;
    }
    
    /**
     * If the text is a simple text and we can use FontDesignMetrics without a stackoverflow.
     * @see FontDesignMetrics#stringWidth(String)
     * @return true, if a simple text. false it is a complex text.
     */
	public static boolean isSimpleString(Font font, String str) {
		if (font.hasLayoutAttributes()) {
			return false;
		}
		for (int i = 0; i < str.length(); ++i) {
			if (FontUtilities.isNonSimpleChar(str.charAt(i))) {
				return false;
			}
		}
		return true;
	}
}
