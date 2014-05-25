/*
  Copyright (C) 2014 Jeroen Frijters

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
using System.Globalization;

static class Java_sun_util_locale_provider_HostLocaleProviderAdapterImpl
{
	private static string[][] positivePatterns = new string[][] {
        // NF_NUMBER
        new string[] { "{0}" },
        // NF_CURRENCY
        new string[] { "\xA4{0}", "{0}\xA4", "\xA4 {0}", "{0} \xA4" },
        // NF_PERCENT
        new string[] { "{0} %", "{0}%", "%{0}", "% {0}" },
    };

	private static string[][] negativePatterns = new string[][] {
        // NF_NUMBER
        new string[] { "({0})", "-{0}", "- {0}", "{0}-", "{0} -" },
        // NF_CURRENCY
        new string[] { "(\xA4{0})", "-\xA4{0}", "\xA4-{0}", "\xA4{0}-", "({0}\xA4)", "-{0}\xA4", "{0}-\xA4", "{0}\xA4-", "-{0} \xA4", "-\xA4 {0}", "{0} \xA4-", "\xA4 {0}-", "\xA4 -{0}", "{0}- \xA4", "(\xA4 {0})", "({0} \xA4)" },
        // NF_PERCENT
        new string[] { "-{0} %", "-{0}%", "-%{0}", "%-{0}", "%{0}-", "{0}-%", "{0}%-", "-% {0}", "{0} %-", "% {0}-", "% -{0}", "{0}- %" },
    };

	public static bool initialize()
	{
		return true;
	}

	public static string getDefaultLocale(int cat)
	{
		const int CAT_DISPLAY = 0;
		const int CAT_FORMAT = 1;
		switch (cat)
		{
			case CAT_DISPLAY:
				return CultureInfo.CurrentUICulture.IetfLanguageTag;
			case CAT_FORMAT:
				return CultureInfo.CurrentCulture.IetfLanguageTag;
			default:
				throw new NotSupportedException();
		}
	}

	public static string getDateTimePattern(int dateStyle, int timeStyle, string langTag)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			switch (timeStyle)
			{
				case 0:
				case 1:
					return ci.DateTimeFormat.LongTimePattern;
				case 2:
				case 3:
					// NOTE prior to .NET 4.0 this does not return the user's customized time pattern
					return ci.DateTimeFormat.ShortTimePattern;
			}

			switch (dateStyle)
			{
				case 0:
				case 1:
					return ci.DateTimeFormat.LongDatePattern;
				case 2:
				case 3:
					return ci.DateTimeFormat.ShortDatePattern;
			}
		}
		return "";
	}

	public static int getCalendarID(string langTag)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return 0;
		}
		// this mapping is based on the CAL_* constants defined in
		// http://referencesource.microsoft.com/#mscorlib/system/globalization/calendar.cs#70
		Calendar cal = ci.Calendar;
		if (cal is GregorianCalendar)
		{
			switch (((GregorianCalendar)cal).CalendarType)
			{
				case GregorianCalendarTypes.Localized:
					return 1;
				case GregorianCalendarTypes.USEnglish:
					return 2;
				case GregorianCalendarTypes.MiddleEastFrench:
					return 9;
				case GregorianCalendarTypes.Arabic:
					return 10;
				case GregorianCalendarTypes.TransliteratedEnglish:
					return 11;
				case GregorianCalendarTypes.TransliteratedFrench:
					return 12;
				default:
					return 0;
			}
		}
		else if (cal is JapaneseCalendar)
		{
			return 3;
		}
		else if (cal is TaiwanCalendar)
		{
			return 4;
		}
		else if (cal is KoreanCalendar)
		{
			return 5;
		}
		else if (cal is HijriCalendar)
		{
			return 6;
		}
		else if (cal is ThaiBuddhistCalendar)
		{
			return 7;
		}
		else if (cal is HebrewCalendar)
		{
			return 8;
		}
		else if (cal is JulianCalendar)
		{
			return 13;
		}
		else if (cal is JapaneseLunisolarCalendar)
		{
			return 14;
		}
		else if (cal is ChineseLunisolarCalendar)
		{
			return 15;
		}
		else if (cal is KoreanLunisolarCalendar)
		{
			return 20;
		}
		else if (cal is TaiwanLunisolarCalendar)
		{
			return 21;
		}
		else if (cal is PersianCalendar)
		{
			return 22;
		}
		else if (cal is UmAlQuraCalendar)
		{
			return 23;
		}
		else
		{
			return 0;
		}
	}

	public static string[] getAmPmStrings(string langTag, string[] ampm)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			DateTimeFormatInfo info = ci.DateTimeFormat;
			ampm[0] = info.AMDesignator;
			ampm[1] = info.PMDesignator;
		}
		return ampm;
	}

	public static string[] getEras(string langTag, string[] eras)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			eras[1] = ci.DateTimeFormat.GetAbbreviatedEraName(ci.Calendar.GetEra(DateTime.Now));
		}
		return eras;
	}

	public static string[] getMonths(string langTag, string[] months)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			DateTimeFormatInfo dtfi = ci.DateTimeFormat;
			for (int i = 1; i <= 13; i++)
			{
				months[i - 1] = dtfi.GetMonthName(i);
			}
		}
		return months;
	}

	public static string[] getShortMonths(string langTag, string[] smonths)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			DateTimeFormatInfo dtfi = ci.DateTimeFormat;
			for (int i = 1; i <= 13; i++)
			{
				smonths[i - 1] = dtfi.GetAbbreviatedMonthName(i);
			}
		}
		return smonths;
	}

	public static string[] getWeekdays(string langTag, string[] wdays)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			DateTimeFormatInfo dtfi = ci.DateTimeFormat;
			for (int i = 0; i < 7; i++)
			{
				wdays[i + 1] = dtfi.GetDayName((DayOfWeek)i);
			}
		}
		return wdays;
	}

	public static string[] getShortWeekdays(string langTag, string[] swdays)
	{
		CultureInfo ci;
		if (TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			DateTimeFormatInfo dtfi = ci.DateTimeFormat;
			for (int i = 0; i < 7; i++)
			{
				swdays[i + 1] = dtfi.GetAbbreviatedDayName((DayOfWeek)i);
			}
		}
		return swdays;
	}

	public static string getNumberPattern(int numberStyle, string langTag)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			ci = CultureInfo.InvariantCulture;
		}
		NumberFormatInfo nfi= ci.NumberFormat;
		const int NF_NUMBER = 0;
		const int NF_CURRENCY = 1;
		const int NF_PERCENT = 2;
		const int NF_INTEGER = 3;
		int digits;
		int[] groupSizes;
		int positivePattern;
		int negativePattern;
		switch (numberStyle)
		{
			case NF_NUMBER:
				digits = nfi.NumberDecimalDigits;
				groupSizes = nfi.NumberGroupSizes;
				positivePattern = 0;
				negativePattern = nfi.NumberNegativePattern;
				break;
			case NF_CURRENCY:
				digits = nfi.CurrencyDecimalDigits;
				groupSizes = nfi.CurrencyGroupSizes;
				positivePattern = nfi.CurrencyPositivePattern;
				negativePattern = nfi.CurrencyNegativePattern;
				break;
			case NF_PERCENT:
				digits = nfi.PercentDecimalDigits;
				groupSizes = nfi.PercentGroupSizes;
				positivePattern = nfi.PercentPositivePattern;
				negativePattern = nfi.PercentNegativePattern;
				break;
			case NF_INTEGER:
				digits = 0;
				groupSizes = nfi.NumberGroupSizes;
				positivePattern = 0;
				negativePattern = nfi.NumberNegativePattern;
				break;
			default:
				throw new NotSupportedException();
		}
		string number = "";
		foreach (int group in groupSizes)
		{
			if (group > 0)
			{
				number += "#," + new String('#', group - 1);
			}
		}
		number += "0";
		if (digits > 0)
		{
			number += "." + new String('0', digits);
		}
		return String.Format(positivePatterns[numberStyle % 3][positivePattern] + ";" + negativePatterns[numberStyle % 3][negativePattern], number);
	}

	public static bool isNativeDigit(string langTag)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return false;
		}
		return ci.NumberFormat.DigitSubstitution == DigitShapes.NativeNational;
	}

	public static string getCurrencySymbol(string langTag, string currencySymbol)
	{
		RegionInfo ri;
		if (!TryGetRegionInfoFromLangTag(langTag, out ri))
		{
			return currencySymbol;
		}
		return ri.CurrencySymbol;
	}

	public static char getDecimalSeparator(string langTag, char decimalSeparator)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return decimalSeparator;
		}
		return ci.NumberFormat.NumberDecimalSeparator[0];
	}

	public static char getGroupingSeparator(string langTag, char groupingSeparator)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return groupingSeparator;
		}
		return ci.NumberFormat.NumberGroupSeparator[0];
	}

	public static string getInfinity(string langTag, string infinity)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return infinity;
		}
		return ci.NumberFormat.PositiveInfinitySymbol;
	}

	public static string getInternationalCurrencySymbol(string langTag, string internationalCurrencySymbol)
	{
		RegionInfo ri;
		if (!TryGetRegionInfoFromLangTag(langTag, out ri))
		{
			return internationalCurrencySymbol;
		}
		return ri.ISOCurrencySymbol;
	}

	public static char getMinusSign(string langTag, char minusSign)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return minusSign;
		}
		return ci.NumberFormat.NegativeSign[0];
	}

	public static char getMonetaryDecimalSeparator(string langTag, char monetaryDecimalSeparator)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return monetaryDecimalSeparator;
		}
		return ci.NumberFormat.CurrencyDecimalSeparator[0];
	}

	public static string getNaN(string langTag, string nan)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return nan;
		}
		return ci.NumberFormat.NaNSymbol;
	}

	public static char getPercent(string langTag, char percent)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return percent;
		}
		return ci.NumberFormat.PercentSymbol[0];
	}

	public static char getPerMill(string langTag, char perMill)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return perMill;
		}
		return ci.NumberFormat.PerMilleSymbol[0];
	}

	public static char getZeroDigit(string langTag, char zeroDigit)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return zeroDigit;
		}
		return ci.NumberFormat.NativeDigits[0][0];
	}

	public static int getCalendarDataValue(string langTag, int type)
	{
		CultureInfo ci;
		if (!TryGetCultureInfoFromLangTag(langTag, out ci))
		{
			return -1;
		}
		const int CD_FIRSTDAYOFWEEK = 0;
		switch (type)
		{
			case CD_FIRSTDAYOFWEEK:
				return (((int)ci.DateTimeFormat.FirstDayOfWeek) + 6) % 7;
			default:
				throw new NotSupportedException();
		}
	}

	public static string getDisplayString(string langTag, int key, string value)
	{
		const int DN_CURRENCY_NAME = 0;
		const int DN_CURRENCY_SYMBOL = 1;
		const int DN_LOCALE_LANGUAGE = 2;
		const int DN_LOCALE_REGION = 4;

		if (key == DN_LOCALE_LANGUAGE)
		{
			CultureInfo ci;
			if (!TryGetCultureInfoFromLangTag(value, out ci))
			{
				return null;
			}
			while (ci.Parent != CultureInfo.InvariantCulture)
			{
				ci = ci.Parent;
			}
			return ci.DisplayName;
		}
		else if (key == DN_LOCALE_REGION)
		{
			RegionInfo ri;
			if (!TryGetRegionInfoFromLangTag(value, out ri))
			{
				return null;
			}
			return ri.DisplayName;
		}
		else
		{
			RegionInfo ri;
			if (!TryGetRegionInfoFromLangTag(langTag, out ri))
			{
				return null;
			}
			switch (key)
			{
				case DN_CURRENCY_NAME:
					return ri.CurrencyNativeName;
				case DN_CURRENCY_SYMBOL:
					return ri.CurrencySymbol;
				default:
					throw new NotSupportedException();
			}
		}
	}

	private static bool TryGetCultureInfoFromLangTag(string langTag, out CultureInfo ci)
	{
		try
		{
			// we can't use CultureInfo.GetCultureInfo() here, because we want the UseUserOverride = true behavior
			ci = new CultureInfo(NormalizeLangTag(langTag));
			return true;
		}
		catch (ArgumentException) // .NET 2.0 throws an ArgumentException and .NET 4.0 throws CultureNotFoundException (a subclass of ArgumentException)
		{
			ci = null;
			return false;
		}
	}

	private static bool TryGetRegionInfoFromLangTag(string langTag, out RegionInfo ri)
	{
		try
		{
			ri = new RegionInfo(NormalizeLangTag(langTag));
			return true;
		}
		catch (ArgumentException) // .NET 2.0 throws an ArgumentException and .NET 4.0 throws CultureNotFoundException (a subclass of ArgumentException)
		{
			ri = null;
			return false;
		}
	}

	private static string NormalizeLangTag(string langTag)
	{
		if (langTag == "und")
		{
			langTag = "en-US";
		}
		return langTag;
	}
}

static class JRELocaleProviderAdapter
{
	// the Java implementation is redirected via map.xml
	internal static bool isNonENLangSupported()
	{
		return IKVM.Internal.ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("sun.text.resources.nl.FormatData_nl") != null;
	}
}
