/*
  Copyright (C) 2004, 2005 Jeroen Frijters

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

final class VMTimeZone extends TimeZone
{
    private cli.System.TimeZone tz = cli.System.TimeZone.get_CurrentTimeZone();
    private int extraOffset;

    public int getOffset(int era, int year, int month, int day, int dayOfWeek, int milliseconds)
    {
	if(era != GregorianCalendar.AD)
	    throw new IllegalArgumentException("Unsupported era");

	return getOffset(new cli.System.DateTime(year, month + 1, day).AddMilliseconds(milliseconds));
    }

    private int getOffset(cli.System.DateTime d)
    {
	return extraOffset + (int)tz.GetUtcOffset(d).get_TotalMilliseconds();
    }

    public int getOffset(long date)
    {
	return getOffset(new cli.System.DateTime((62135596800000L + date) * 10000L).ToLocalTime());
    }

    public void setRawOffset(int offsetMillis)
    {
	extraOffset = offsetMillis - (getRawOffset() - extraOffset);
    }

    public int getRawOffset()
    {
	cli.System.DateTime now = cli.System.DateTime.get_Now();

	int offset = (int)tz.GetUtcOffset(now).get_TotalMilliseconds();
	if(tz.IsDaylightSavingTime(now))
	{
	    offset -= tz.GetDaylightChanges(now.get_Year()).get_Delta().get_TotalMilliseconds();
	}
	return offset + extraOffset;
    }

    public String getDisplayName(boolean daylight, int style, Locale locale)
    {
	return daylight ? tz.get_DaylightName() : tz.get_StandardName();
    }

    public int getDSTSavings()
    {
	return (int)tz.GetDaylightChanges(cli.System.DateTime.get_Today().get_Year()).get_Delta().get_TotalMilliseconds();
    }

    public boolean useDaylightTime()
    {
	return getDSTSavings() != 0;
    }

    public boolean inDaylightTime(Date date)
    {
	return tz.IsDaylightSavingTime(new cli.System.DateTime(date.getYear() + 1900, date.getMonth() + 1, date.getDate()));
    }

    public boolean hasSameRules(TimeZone other)
    {
	if(other instanceof VMTimeZone)
	{
	    VMTimeZone o = (VMTimeZone)other;
	    return o.tz == tz && o.extraOffset == extraOffset;
	}
	return false;
    }

    static TimeZone getDefaultTimeZoneId()
    {
	TimeZone nettz = new VMTimeZone();
	String[] ids = TimeZone.getAvailableIDs(nettz.getRawOffset());
outer:
	for(int i = 0; i < ids.length; i++)
	{
	    TimeZone tz = TimeZone.getTimeZone(ids[i]);
	    if(nettz.useDaylightTime() == tz.useDaylightTime())
	    {
		if(!tz.useDaylightTime())
		    return tz;
		// HACK extremely lame: we cycle through all days to check if the time zones are "the same"
		long d = new cli.System.DateTime(cli.System.DateTime.get_Today().get_Year(), 1, 1).get_Ticks() / 10000L - 62135596800000L;
		for(int j = 0; j < 365; j++)
		{
		    if(tz.getOffset(d) != nettz.getOffset(d))
		    {
			break outer;
		    }
		    d += 24 * 60 * 60 * 1000;
		}
		return tz;
	    }
	}
	// we didn't find a match, return our own implementation, this isn't really
	// a proper time zone (it isn't serializable), but it'll have to do.
	nettz.setID(".NET TimeZone");
	return nettz;
    }

/*
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
*/
}
