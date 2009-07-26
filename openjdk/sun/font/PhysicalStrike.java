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
import java.awt.font.FontRenderContext;
import java.awt.geom.GeneralPath;
import java.awt.geom.Point2D.Float;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import cli.System.Drawing.*;

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
    
    private static final Bitmap bitmap = new Bitmap(1, 1);
    private static final Graphics g = Graphics.FromImage(bitmap);
    private static final StringFormat format = new cli.System.Drawing.StringFormat(StringFormat.get_GenericTypographic());
    static{
        StringFormatFlags flags = format.get_FormatFlags();
        flags = StringFormatFlags.wrap(flags.Value | StringFormatFlags.MeasureTrailingSpaces);
        format.set_FormatFlags( flags );
    }
    
    public PhysicalStrike(Font font, FontFamily family, FontStyle style, FontRenderContext frc){
        this.font = font;
        this.family = family;
        this.style = style;
        this.frc = frc;
        this.size2D = font.get_Size();
        this.factor = size2D / family.GetEmHeight(style);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    Float getCharMetrics(char ch){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    float getCodePointAdvance(int cp){
        SizeF sizeF = g.MeasureString(String.valueOf((char)cp), font, Integer.MAX_VALUE, format);
        if(frc.usesFractionalMetrics()){
            return sizeF.get_Width();
        } else {
            return (int)(sizeF.get_Width() + 0.5F);
        }
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
                    0, -ascent, 
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
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImageBounds(int glyphcode, Float pt, Rectangle result){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    long getGlyphImagePtr(int glyphcode){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    void getGlyphImagePtrs(int[] glyphCodes, long[] images, int len){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    Float getGlyphMetrics(int glyphcode){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphOutline(int glyphCode, float x, float y){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    java.awt.geom.Rectangle2D.Float getGlyphOutlineBounds(int glyphCode){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    GeneralPath getGlyphVectorOutline(int[] glyphs, float x, float y){
        throw new NotImplementedException();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public int getNumGlyphs(){
        throw new NotImplementedException();
    }

}
