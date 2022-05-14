/*
  Copyright (C) 2009, 2013 Volker Berlin (i-net software)

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
public abstract class SunGraphics2D extends Graphics2D{
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

    private final SurfaceData surfaceData;
    
    public int constrainX;
    public int constrainY;

    /**
     * Create a new SunGraphics2D
     */
    protected SunGraphics2D( SurfaceData surfaceData ) {
    	this.surfaceData = surfaceData;
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
    public void constrain(int x, int y, int w, int h){
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
    	return surfaceData.getDestination();
    }
}
