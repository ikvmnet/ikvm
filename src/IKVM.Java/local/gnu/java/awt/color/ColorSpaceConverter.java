/* ColorSpaceConverter.java -- an interface for colorspace conversion
   Copyright (C) 2004 Free Software Foundation

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */

package gnu.java.awt.color;


/**
 * ColorSpaceConverter - used by java.awt.color.ICC_ColorSpace
 *
 * Color space conversion can occur in several ways:
 *
 * -Directly (for the built in spaces sRGB, linear RGB, gray, CIE XYZ and PYCC
 * -ICC_ProfileRGB works through TRC curves and a matrix
 * -ICC_ProfileGray works through a single TRC
 * -Everything else is done through Color lookup tables.
 *
 * The different conversion methods are implemented through
 * an interface. The built-in colorspaces are implemented directly
 * with the relevant conversion equations.
 *
 * In this way, we hopefully will always use the fastest and most
 * accurate method available.
 *
 * @author Sven de Marothy
 */
public interface ColorSpaceConverter
{
  float[] toCIEXYZ(float[] in);

  float[] fromCIEXYZ(float[] in);

  float[] toRGB(float[] in);

  float[] fromRGB(float[] in);
}
