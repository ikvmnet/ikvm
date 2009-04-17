/*
 * Copyright 1997-2007 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.awt.image;

import java.awt.geom.AffineTransform;
import java.awt.image.AffineTransformOp;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ByteLookupTable;
import java.awt.image.ConvolveOp;
import java.awt.image.Kernel;
import java.awt.image.LookupOp;
import java.awt.image.LookupTable;
import java.awt.image.RasterOp;
import java.awt.image.Raster;
import java.awt.image.WritableRaster;
import java.security.AccessController;
import java.security.PrivilegedAction;

/**
 * This class provides a hook to access platform-specific
 * imaging code.
 *
 * If the implementing class cannot handle the op, tile format or
 * image format, the method will return null;
 * If there is an error when processing the
 * data, the implementing class may either return null
 * (in which case our java code will be executed) or may throw
 * an exception.
 */
public class ImagingLib {

    public static WritableRaster filter(RasterOp op, Raster src,
                                        WritableRaster dst) {
        return null;
    }


    public static BufferedImage filter(BufferedImageOp op, BufferedImage src,
                                       BufferedImage dst)
    {
        return null;
    }
}
