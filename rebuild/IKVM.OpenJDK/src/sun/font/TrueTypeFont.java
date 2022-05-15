/*
  Copyright (C) 2011 Volker Berlin (i-net software)

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

import cli.System.Drawing.FontFamily;

public class TrueTypeFont extends PhysicalFont {

    public static final int GPOSTag = 0x47504F53; // 'GPOS'
    public static final int GSUBTag = 0x47535542; // 'GSUB'

    public TrueTypeFont(String name, int style) {
		super(name, style);
	}

	public TrueTypeFont(FontFamily family, int style) {
		super(family, style);
	}

	Object getDirectoryEntry(int tag) {
		return null;
	}
}
