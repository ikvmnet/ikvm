/*
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
package sun.awt;

/* This class exists to decouple java.util.TimeZone from sun.awt.AppContext */
public abstract class AppContextDC
{
    public abstract boolean isDisposed();
    public abstract Object get(Object key);
    public abstract Object put(Object key, Object value);
    public abstract Object remove(Object key);

    public static AppContextDC getAppContext()
    {
        try
        {
            return (AppContextDC)Class.forName("sun.awt.AppContext").getMethod("getAppContext").invoke(null);
        }
        catch (Exception _)
        {
            return null;
        }
    }
}
