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
package sun.font;

import java.awt.Rectangle;
import java.awt.geom.GeneralPath;
import java.awt.geom.Point2D.Float;

import cli.System.Drawing.FontFamily;
import cli.System.Drawing.FontStyle;

/**
 * A FontStrike implementation that based on .NET fonts. 
 * It replace the equals naming Sun class
 */
public class PhysicalStrike extends FontStrike{

    private final FontFamily family;
    private final FontStyle style;
    private final float size2D;
    private final float factor;
    
    private StrikeMetrics strike;
    
    
    public PhysicalStrike(float size2D, FontFamily family, FontStyle style){
        this.family = family;
        this.style = style;
        this.size2D = size2D;
        factor = size2D / family.GetEmHeight(style);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    Float getCharMetrics(char ch){
        // TODO Auto-generated method stub
        return null;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    float getCodePointAdvance(int cp){
        // TODO Auto-generated method stub
        return 0;
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
                    0, ascent, 
                    0, descent, 
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
        // TODO Auto-generated method stub
        return 0;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImageBounds(int glyphcode, Float pt, Rectangle result){
        // TODO Auto-generated method stub

    }


    /**
     * {@inheritDoc}
     */
    @Override
    long getGlyphImagePtr(int glyphcode){
        // TODO Auto-generated method stub
        return 0;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImagePtrs(int[] glyphCodes, long[] images, int len){
        // TODO Auto-generated method stub

    }


    /**
     * {@inheritDoc}
     */
    @Override
    Float getGlyphMetrics(int glyphcode){
        // TODO Auto-generated method stub
        return null;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphOutline(int glyphCode, float x, float y){
        // TODO Auto-generated method stub
        return null;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    java.awt.geom.Rectangle2D.Float getGlyphOutlineBounds(int glyphCode){
        // TODO Auto-generated method stub
        return null;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphVectorOutline(int[] glyphs, float x, float y){
        // TODO Auto-generated method stub
        return null;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public int getNumGlyphs(){
        // TODO Auto-generated method stub
        return 0;
    }

}
