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

import java.awt.*;
import java.awt.font.FontRenderContext;
import java.awt.geom.GeneralPath;
import java.awt.geom.Rectangle2D;
import java.awt.geom.Point2D.Float;

import cli.System.Drawing.FontFamily;
import cli.System.Drawing.FontStyle;
import ikvm.internal.NotYetImplementedError;

/**
 * A FontStrike implementation that based on .NET fonts. 
 * It replace the equals naming Sun class
 */
public class PhysicalStrike extends FontStrike{

    private final Font font;
    private final FontFamily family;
    private final FontStyle style;
    private final FontRenderContext frc;
    private final float size2D;
    private final float factor;
    
    private StrikeMetrics strike;
    
    private FontMetrics metrics;
    
    public PhysicalStrike(Font font, FontFamily family, FontStyle style, FontRenderContext frc){
        this.font = font;
        this.family = family;
        this.style = style;
        this.frc = frc;
        this.size2D = font.getNetFont().get_Size();
        this.factor = size2D / family.GetEmHeight(style);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    Float getCharMetrics(char ch){
        return new Float(getCodePointAdvance(ch), 0);
    }


    /**
     * {@inheritDoc}
     */
    @Override
    float getCodePointAdvance( int cp ) {
        Graphics2D g = StandardGlyphVector.createGraphics( frc );
        if( metrics == null ) {
            metrics = Toolkit.getDefaultToolkit().getFontMetrics( font );
        }
        Rectangle2D.Float bounds = (Rectangle2D.Float)metrics.getStringBounds( String.valueOf( (char)cp ), g );
        return bounds.width;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    StrikeMetrics getFontMetrics(){
        if(strike == null){
            float ascent = family.GetCellAscent(style) * factor;
            float descent = family.GetCellDescent(style) * factor;
            float height = family.GetLineSpacing(style) * factor;
            float leading = height - ascent - descent;
            strike = new StrikeMetrics(
                    0, -ascent - leading/2, 
                    0, descent - leading/2, 
                    0.25f, 0, 
                    0, leading, 
                    size2D * 2, 0);
        }
        return strike;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    float getGlyphAdvance(int glyphCode){
        return getCodePointAdvance( glyphCode );
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImageBounds(int glyphcode, Float pt, Rectangle result){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    long getGlyphImagePtr(int glyphcode){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImagePtrs(int[] glyphCodes, long[] images, int len){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    Float getGlyphMetrics(int glyphcode){
        return getCharMetrics((char)glyphcode);
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphOutline(int glyphCode, float x, float y){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    java.awt.geom.Rectangle2D.Float getGlyphOutlineBounds(int glyphCode){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphVectorOutline(int[] glyphs, float x, float y){
        throw new NotYetImplementedError();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public int getNumGlyphs(){
        throw new NotYetImplementedError();
    }

}
