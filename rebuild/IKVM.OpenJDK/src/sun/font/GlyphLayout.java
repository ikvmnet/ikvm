/*
  Copyright (C) 2009, 2011 Volker Berlin (i-net software)

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
import java.awt.font.FontRenderContext;



/**
 * 
 */
public class GlyphLayout{
	
	private static GlyphLayout glyphLayout = new GlyphLayout();

    /**
     * Return a new instance of GlyphLayout, using the provided layout engine factory.
     * If null, the system layout engine factory will be used.
     */
    public static GlyphLayout get(Object lef) {
        return glyphLayout; //current this class has no state
    }

    /**
     * Return the old instance of GlyphLayout when you are done.  This enables reuse
     * of GlyphLayout objects.
     */
    public static void done(GlyphLayout gl) {
    }

    /**
     * Create a glyph vector.
     * @param font the font to use
     * @param frc the font render context
     * @param text the text, including optional context before start and after start + count
     * @param offset the start of the text to lay out
     * @param count the length of the text to lay out
     * @param flags bidi and context flags {@see #java.awt.Font}
     * @param result a StandardGlyphVector to modify, can be null
     * @return the layed out glyphvector, if result was passed in, it is returned
     */
    public StandardGlyphVector layout(Font font, FontRenderContext frc,
                                      char[] text, int offset, int count,
                                      int flags, StandardGlyphVector result)
    {
        if (text == null || offset < 0 || count < 0 || (count > text.length - offset)) {
            throw new IllegalArgumentException();
        }

		return new StandardGlyphVector(font, text, offset, count, frc);
    }


}
