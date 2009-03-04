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

package sun.awt;

import java.util.HashSet;
import java.util.Set;
import java.beans.PropertyChangeListener;

public final class AppContext extends java.util.Hashtable
{
    public static final String DISPOSED_PROPERTY_NAME = "disposed";
    private static final AppContext instance = new AppContext();

    private AppContext() {}

    public static AppContext getAppContext()
    {
        return instance;
    }
    
    public static Set<AppContext> getAppContexts()
    {
        HashSet<AppContext> set = new HashSet<AppContext>();
        set.add(instance);
        return set;
    }
    
    public void addPropertyChangeListener(String propertyName, PropertyChangeListener listener)
    {
        throw new Error("Not implemented");
    }
    
    public void removePropertyChangeListener(String propertyName, PropertyChangeListener listener)
    {
        throw new Error("Not implemented");
    }
    
    public boolean isDisposed()
    {
        return false;
    }
}
