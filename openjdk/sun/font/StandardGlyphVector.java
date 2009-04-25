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

import java.awt.font.GlyphVector;


/**
 * Standard implementation of GlyphVector used by Font, GlyphList, and
 * SunGraphics2D.
 *
 */
public abstract class StandardGlyphVector extends GlyphVector{
    private float[] positions; // only if not default advances
    
    /**
     * As a concrete subclass of GlyphVector, this must implement clone.
     */
    public Object clone() {
        try {
            StandardGlyphVector result = (StandardGlyphVector)super.clone();

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

        positions = (float[])srcPositions.clone();

    }

    /**
     * This is a convenience overload that gets all the glyph positions, which
     * is what you usually want to do if you're getting more than one.
     * !!! should I bother taking result parameter?
     */
    public float[] getGlyphPositions(float[] result) {
        return positions;
    }

    /**
     * For each glyph return posx, posy, advx, advy, visx, visy, visw, vish.
     */
    public float[] getGlyphInfo() {
        throw new Error("getGlyphInfo is not implemented.");
    }

}

