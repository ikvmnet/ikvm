/*
  Copyright (C) 2007 Jeroen Frijters

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
package gnu.classpath;

import java.security.AccessController;
import java.security.PrivilegedAction;
import sun.security.action.GetPropertyAction;

/*
 * This is a temporary class while we're using the GNU Classpath AWT/Swing implementation.
 */

@ikvm.lang.Internal
public class SystemProperties
{
    public static String getProperty(String key)
    {
	return getProperty(key, null);
    }

    public static String getProperty(String key, String defaultValue)
    {
	return AccessController.doPrivileged(new GetPropertyAction(key, defaultValue));
    }

    public static void setProperty(final String key, final String value)
    {
	AccessController.doPrivileged(new PrivilegedAction() {
	    public Object run() {
		System.setProperty(key, value);
		return null;
	    }
	});
    }
}
