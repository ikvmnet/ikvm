/*
  Copyright (C) 2004 Jeroen Frijters

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
package java.util;

final class VMTimeZone
{
    static TimeZone getDefaultTimeZoneId()
    {
	return TimeZone.getDefaultTimeZone(getTimeZone());
    }

    private static String getTimeZone()
    {
	cli.System.TimeZone currentTimeZone = cli.System.TimeZone.get_CurrentTimeZone();
	cli.System.TimeSpan timeSpan = currentTimeZone.GetUtcOffset(cli.System.DateTime.get_Now());

	int hours = timeSpan.get_Hours();
	int mins = timeSpan.get_Minutes();

	if(mins != 0)
	{
	    if(hours < 0)
	    {
		return "GMT+" + ((-hours) * 60  + mins);
	    }
	    else
	    {
		return "GMT-" + (hours * 60  + mins);
	    }
	}
	else
	{
	    if(hours < 0)
	    {
		return "GMT+" + (-hours);
	    }
	    else
	    {
		return "GMT-" + (hours  + mins);
	    }
	}
    }  
}
