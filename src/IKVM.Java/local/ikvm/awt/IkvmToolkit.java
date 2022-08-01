/*
  Copyright (C) 2008 Volker Berlin (i-net software)
  Copyright (C) 2012 Jeroen Frijters

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
package ikvm.awt;

import java.awt.Toolkit;
import sun.awt.HeadlessToolkit;

public interface IkvmToolkit{
    
    public sun.print.PrintPeer getPrintPeer();
    
    public java.awt.Shape outline(java.awt.Font javaFont, java.awt.font.FontRenderContext frc, String text, float x, float y);

	public static class DefaultToolkit
	{
		public static IkvmToolkit get()
		{
			Toolkit tk = Toolkit.getDefaultToolkit();
			if (tk instanceof HeadlessToolkit)
			{
				tk = ((HeadlessToolkit)tk).getUnderlyingToolkit();
			}
			return (IkvmToolkit)tk;
		}
	}
}
