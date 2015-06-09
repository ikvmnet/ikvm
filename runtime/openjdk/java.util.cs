/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System;

static class Java_java_util_TimeZone
{
	private static string GetCurrentTimeZoneID()
	{
#if NET_4_0
		return TimeZoneInfo.Local.Id;
#else
		// we don't want a static dependency on System.Core (to be able to run on .NET 2.0)
		Type typeofTimeZoneInfo = Type.GetType("System.TimeZoneInfo, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
		if (typeofTimeZoneInfo != null)
		{
			try
			{
				return (string)typeofTimeZoneInfo.GetProperty("Id").GetValue(typeofTimeZoneInfo.GetProperty("Local").GetValue(null, null), null);
			}
			catch (Exception x)
			{
				// older Mono versions did not wrap the exception in a TargetInvocationExcception,
				// so we check both x and x.InnerException
				if (typeofTimeZoneInfo.Assembly.GetType("System.TimeZoneNotFoundException").IsInstanceOfType(x)
					|| typeofTimeZoneInfo.Assembly.GetType("System.TimeZoneNotFoundException").IsInstanceOfType(x.InnerException))
				{
					// MONOBUG Mono's TimeZoneInfo.Local property throws a TimeZoneNotFoundException on Windows
					// (https://bugzilla.novell.com/show_bug.cgi?id=622524)
					return TimeZone.CurrentTimeZone.StandardName;
				}
				else
				{
					throw;
				}
			}
		}
		else
		{
			// HACK this is very lame and probably won't work on localized windows versions
			return TimeZone.CurrentTimeZone.StandardName;
		}
#endif
	}

	public static string getSystemTimeZoneID(string javaHome)
	{
		// (the switch was generated from the contents of $JAVA_HOME/lib/tzmappings)
		switch (GetCurrentTimeZoneID())
		{
			case "Romance":
			case "Romance Standard Time":
				return "Europe/Paris";
			case "Warsaw":
				return "Europe/Warsaw";
			case "Central Europe":
			case "Central Europe Standard Time":
			case "Prague Bratislava":
				return "Europe/Prague";
			case "W. Central Africa Standard Time":
				return "Africa/Luanda";
			case "FLE":
			case "FLE Standard Time":
				return "Europe/Helsinki";
			case "GFT":
			case "GFT Standard Time":
			case "GTB":
			case "GTB Standard Time":
				return "Europe/Athens";
			case "Israel":
			case "Israel Standard Time":
				return "Asia/Jerusalem";
			case "Arab":
			case "Arab Standard Time":
				return "Asia/Riyadh";
			case "Arabic Standard Time":
				return "Asia/Baghdad";
			case "E. Africa":
			case "E. Africa Standard Time":
				return "Africa/Nairobi";
			case "Saudi Arabia":
			case "Saudi Arabia Standard Time":
				return "Asia/Riyadh";
			case "Iran":
			case "Iran Standard Time":
				return "Asia/Tehran";
			case "Afghanistan":
			case "Afghanistan Standard Time":
				return "Asia/Kabul";
			case "India":
			case "India Standard Time":
				return "Asia/Calcutta";
			case "Myanmar Standard Time":
				return "Asia/Rangoon";
			case "Nepal Standard Time":
				return "Asia/Katmandu";
			case "Sri Lanka":
			case "Sri Lanka Standard Time":
				return "Asia/Colombo";
			case "Beijing":
			case "China":
			case "China Standard Time":
				return "Asia/Shanghai";
			case "AUS Central":
			case "AUS Central Standard Time":
				return "Australia/Darwin";
			case "Cen. Australia":
			case "Cen. Australia Standard Time":
				return "Australia/Adelaide";
			case "Vladivostok":
			case "Vladivostok Standard Time":
				return "Asia/Vladivostok";
			case "West Pacific":
			case "West Pacific Standard Time":
				return "Pacific/Guam";
			case "E. South America":
			case "E. South America Standard Time":
				return "America/Sao_Paulo";
			case "Greenland Standard Time":
				return "America/Godthab";
			case "Newfoundland":
			case "Newfoundland Standard Time":
				return "America/St_Johns";
			case "Pacific SA":
			case "Pacific SA Standard Time":
				return "America/Santiago";
			case "SA Western":
			case "SA Western Standard Time":
				return "America/Caracas";
			case "SA Pacific":
			case "SA Pacific Standard Time":
				return "America/Bogota";
			case "US Eastern":
			case "US Eastern Standard Time":
				return "America/Indianapolis";
			case "Central America Standard Time":
				return "America/Regina";
			case "Mexico":
			case "Mexico Standard Time":
				return "America/Mexico_City";
			case "Canada Central":
			case "Canada Central Standard Time":
				return "America/Regina";
			case "US Mountain":
			case "US Mountain Standard Time":
				return "America/Phoenix";
			case "GMT":
			case "GMT Standard Time":
				return "Europe/London";
			case "Ekaterinburg":
			case "Ekaterinburg Standard Time":
				return "Asia/Yekaterinburg";
			case "West Asia":
			case "West Asia Standard Time":
				return "Asia/Karachi";
			case "Central Asia":
			case "Central Asia Standard Time":
				return "Asia/Dhaka";
			case "N. Central Asia Standard Time":
				return "Asia/Novosibirsk";
			case "Bangkok":
			case "Bangkok Standard Time":
				return "Asia/Bangkok";
			case "North Asia Standard Time":
				return "Asia/Krasnoyarsk";
			case "SE Asia":
			case "SE Asia Standard Time":
				return "Asia/Bangkok";
			case "North Asia East Standard Time":
				return "Asia/Ulaanbaatar";
			case "Singapore":
			case "Singapore Standard Time":
				return "Asia/Singapore";
			case "Taipei":
			case "Taipei Standard Time":
				return "Asia/Taipei";
			case "W. Australia":
			case "W. Australia Standard Time":
				return "Australia/Perth";
			case "Korea":
			case "Korea Standard Time":
				return "Asia/Seoul";
			case "Tokyo":
			case "Tokyo Standard Time":
				return "Asia/Tokyo";
			case "Yakutsk":
			case "Yakutsk Standard Time":
				return "Asia/Yakutsk";
			case "Central European":
			case "Central European Standard Time":
				return "Europe/Belgrade";
			case "W. Europe":
			case "W. Europe Standard Time":
				return "Europe/Berlin";
			case "Tasmania":
			case "Tasmania Standard Time":
				return "Australia/Hobart";
			case "AUS Eastern":
			case "AUS Eastern Standard Time":
				return "Australia/Sydney";
			case "E. Australia":
			case "E. Australia Standard Time":
				return "Australia/Brisbane";
			case "Sydney Standard Time":
				return "Australia/Sydney";
			case "Central Pacific":
			case "Central Pacific Standard Time":
				return "Pacific/Guadalcanal";
			case "Dateline":
			case "Dateline Standard Time":
				return "GMT-1200";
			case "Fiji":
			case "Fiji Standard Time":
				return "Pacific/Fiji";
			case "Samoa":
			case "Samoa Standard Time":
				return "Pacific/Apia";
			case "Hawaiian":
			case "Hawaiian Standard Time":
				return "Pacific/Honolulu";
			case "Alaskan":
			case "Alaskan Standard Time":
				return "America/Anchorage";
			case "Pacific":
			case "Pacific Standard Time":
				return "America/Los_Angeles";
			case "Mexico Standard Time 2":
				return "America/Chihuahua";
			case "Mountain":
			case "Mountain Standard Time":
				return "America/Denver";
			case "Central":
			case "Central Standard Time":
				return "America/Chicago";
			case "Eastern":
			case "Eastern Standard Time":
				return "America/New_York";
			case "E. Europe":
			case "E. Europe Standard Time":
				return "Europe/Minsk";
			case "Egypt":
			case "Egypt Standard Time":
				return "Africa/Cairo";
			case "South Africa":
			case "South Africa Standard Time":
				return "Africa/Harare";
			case "Atlantic":
			case "Atlantic Standard Time":
				return "America/Halifax";
			case "SA Eastern":
			case "SA Eastern Standard Time":
				return "America/Buenos_Aires";
			case "Mid-Atlantic":
			case "Mid-Atlantic Standard Time":
				return "Atlantic/South_Georgia";
			case "Azores":
			case "Azores Standard Time":
				return "Atlantic/Azores";
			case "Cape Verde Standard Time":
				return "Atlantic/Cape_Verde";
			case "Russian":
			case "Russian Standard Time":
				return "Europe/Moscow";
			case "New Zealand":
			case "New Zealand Standard Time":
				return "Pacific/Auckland";
			case "Tonga Standard Time":
				return "Pacific/Tongatapu";
			case "Arabian":
			case "Arabian Standard Time":
				return "Asia/Muscat";
			case "Caucasus":
			case "Caucasus Standard Time":
				return "Asia/Yerevan";
			case "Greenwich":
			case "Greenwich Standard Time":
				return "GMT";
			case "Central Brazilian Standard Time":
				return "America/Manaus";
			case "Central Standard Time (Mexico)":
				return "America/Mexico_City";
			case "Georgian Standard Time":
				return "Asia/Tbilisi";
			case "Mountain Standard Time (Mexico)":
				return "America/Chihuahua";
			case "Namibia Standard Time":
				return "Africa/Windhoek";
			case "Pacific Standard Time (Mexico)":
				return "America/Tijuana";
			case "Western Brazilian Standard Time":
				return "America/Rio_Branco";
			case "Azerbaijan Standard Time":
				return "Asia/Baku";
			case "Jordan Standard Time":
				return "Asia/Amman";
			case "Middle East Standard Time":
				return "Asia/Beirut";
			default:
				// this means fall back to GMT offset
				return getSystemGMTOffsetID();
		}
	}

	public static string getSystemGMTOffsetID()
	{
		TimeSpan sp = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
		int hours = sp.Hours;
		int mins = sp.Minutes;
		if (hours >= 0 && mins >= 0)
		{
			return String.Format("GMT+{0:D2}:{1:D2}", hours, mins);
		}
		else
		{
			return String.Format("GMT-{0:D2}:{1:D2}", -hours, -mins);
		}
	}
}

