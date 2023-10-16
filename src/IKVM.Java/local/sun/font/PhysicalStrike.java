/*
  Copyright (C) 2009 - 2011 Volker Berlin (i-net software)

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
import java.awt.Rectangle;
import java.awt.font.FontRenderContext;
import java.awt.geom.GeneralPath;
import java.awt.geom.Point2D.Float;

import cli.System.Drawing.Bitmap;
import cli.System.Drawing.CharacterRange;
import cli.System.Drawing.FontFamily;
import cli.System.Drawing.FontStyle;
import cli.System.Drawing.Graphics;
import cli.System.Drawing.RectangleF;
import cli.System.Drawing.Region;
import cli.System.Drawing.SizeF;
import cli.System.Drawing.StringFormat;
import cli.System.Drawing.StringFormatFlags;
import cli.System.Drawing.StringTrimming;
import cli.System.Drawing.Drawing2D.PixelOffsetMode;
import cli.System.Drawing.Drawing2D.SmoothingMode;
import cli.System.Drawing.Text.TextRenderingHint;

import ikvm.internal.NotYetImplementedError;

/**
 * A FontStrike implementation that based on .NET fonts. 
 * It replace the equals naming Sun class
 */
public class PhysicalStrike extends FontStrike{

    private static final Bitmap BITMAP = new Bitmap( 1, 1 );
    private static Graphics FRACT_GRAPHICS = createGraphics(true);
    private static Graphics FIXED_GRAPHICS = createGraphics(false);

    private final Font font;
    private final FontFamily family;
    private final FontStyle style;
    private final FontRenderContext frc;
    private final float size2D;
    private final float factor;
    
    private StrikeMetrics strike;
    
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
     * Create a Graphics with the settings for fractional or fixed FontRenderContext
     * 
     * @return
     */
    private static Graphics createGraphics(boolean fractional){
        Graphics g = Graphics.FromImage(BITMAP);
        g.set_SmoothingMode(SmoothingMode.wrap(fractional ? SmoothingMode.None : SmoothingMode.AntiAlias));
        g.set_PixelOffsetMode(PixelOffsetMode.wrap(fractional ? PixelOffsetMode.None : PixelOffsetMode.HighQuality));
        g.set_TextRenderingHint(TextRenderingHint.wrap(fractional ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit));
        return g;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    float getCodePointAdvance( int cp ) {
        StringFormat format = new StringFormat(StringFormat.get_GenericTypographic());

        format.set_FormatFlags( StringFormatFlags.wrap( StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap |
                             StringFormatFlags.FitBlackBox ));
        format.set_Trimming( StringTrimming.wrap( StringTrimming.None ) );
        format.SetMeasurableCharacterRanges(new CharacterRange[] {new CharacterRange(0, 1)});
        boolean fractional = frc.usesFractionalMetrics();
        Graphics g = fractional ? FRACT_GRAPHICS : FIXED_GRAPHICS;
        SizeF size;
        synchronized (g) {
        	Region[] regions = g.MeasureCharacterRanges(String.valueOf((char)cp), font.getNetFont(),
        			new RectangleF(0, 0, Integer.MAX_VALUE, Integer.MAX_VALUE), format);
        	size = regions[0].GetBounds(g).get_Size();
        	regions[0].Dispose();
		}
        return frc.usesFractionalMetrics() ? size.get_Width() :  Math.round(size.get_Width());
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
