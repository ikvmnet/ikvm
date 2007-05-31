/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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
package ikvm.internal;

import cli.System.GC;
import cli.System.WeakReference;

@ikvm.lang.Internal
public final class WeakIdentityMap
{
    private WeakReference[] keys = new WeakReference[16];
    private Object[] values = new Object[keys.length];

    public WeakIdentityMap()
    {
        for (int i = 0; i < keys.length; i++)
        {
            keys[i] = new WeakReference(null, true);
            // NOTE we suppress finalization, to make sure the WeakReference continues to work
            // while the AppDomain is finalizing for unload (note that for this to work,
            // the code that instantiates us also has to call SuppressFinalize on us.)
            GC.SuppressFinalize(keys[i]);
        }
    }

    protected void finalize()
    {
        for (int i = 0; i < keys.length; i++)
        {
            if (keys[i] != null)
            {
                GC.ReRegisterForFinalize(keys[i]);
            }
        }
    }

    public synchronized Object remove(Object key)
    {
        for (int i = 0; i < keys.length; i++)
        {
            if (keys[i].get_Target() == key)
            {
		Object value = values[i];
		keys[i].set_Target(null);
		values[i] = null;
                return value;
            }
        }
        return null;
    }

    // Note that null values are supported, null keys are not
    public synchronized void put(Object key, Object value)
    {
        if (key == null)
            throw new NullPointerException();
        putImpl(key, value, true);
    }

    private void putImpl(Object key, Object value, boolean tryGC)
    {
        int emptySlot = -1;
        int keySlot = -1;
        for (int i = 0; i < keys.length; i++)
        {
            Object k = keys[i].get_Target();
            if (k == null)
            {
                emptySlot = i;
                values[i] = null;
            }
            else if (k == key)
            {
                keySlot = i;
            }
        }
        if (keySlot != -1)
        {
            values[keySlot] = value;
        }
        else if (emptySlot != -1)
        {
            keys[emptySlot].set_Target(key);
            values[emptySlot] = value;
        }
        else
        {
            if (tryGC)
            {
                GC.Collect(0);
                putImpl(key, value, false);
                return;
            }
            int len = keys.length;
            WeakReference[] newkeys = new WeakReference[len * 2];
            Object[] newvalues = new Object[newkeys.length];
            cli.System.Array.Copy((cli.System.Array)(Object)keys, (cli.System.Array)(Object)newkeys, len);
            cli.System.Array.Copy((cli.System.Array)(Object)values, (cli.System.Array)(Object)newvalues, len);
            keys = newkeys;
            values = newvalues;
            for (int i = len; i < keys.length; i++)
            {
                keys[i] = new WeakReference(null, true);
                GC.SuppressFinalize(keys[i]);
            }
            keys[len].set_Target(key);
            values[len] = value;
        }
    }

    public synchronized Object get(Object key)
    {
        for (int i = 0; i < keys.length; i++)
        {
            if (keys[i].get_Target() == key)
            {
                return values[i];
            }
        }
        return null;
    }

    public synchronized boolean containsKey(Object key)
    {
        for (int i = 0; i < keys.length; i++)
        {
            if (keys[i].get_Target() == key)
            {
                return true;
            }
        }
        return false;
    }
}
