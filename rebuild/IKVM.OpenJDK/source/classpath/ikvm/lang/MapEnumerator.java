/*
  Copyright (C) 2008 Jeroen Frijters

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
package ikvm.lang;

import cli.System.Collections.IDictionaryEnumerator;
import cli.System.Collections.DictionaryEntry;
import java.util.Map;

public final class MapEnumerator implements IDictionaryEnumerator
{
    private final Map map;
    private final IterableEnumerator keys;

    public MapEnumerator(Map map)
    {
	this.map = map;
	keys = new IterableEnumerator(map.keySet());
    }
    
    public Object get_Current()
    {
	return new DictionaryEntry(get_Key(), get_Value());
    }
    
    public boolean MoveNext()
    {
	return keys.MoveNext();
    }

    public void Reset()
    {
	keys.Reset();
    }

    public DictionaryEntry get_Entry()
    {
	return new DictionaryEntry(get_Key(), get_Value());
    }
    
    public Object get_Key()
    {
	return keys.get_Current();
    }
    
    public Object get_Value()
    {
	return map.get(keys.get_Current());
    }
}
