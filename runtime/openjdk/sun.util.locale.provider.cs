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

static class Java_sun_util_locale_provider_HostLocaleProviderAdapterImpl
{
	public static bool initialize()
	{
		return false;
	}

	public static string getDefaultLocale(int cat)
	{
		throw new NotImplementedException();
	}

	public static string getDateTimePattern(int dateStyle, int timeStyle, string langTag)
	{
		throw new NotImplementedException();
	}

	public static int getCalendarID(string langTag)
	{
		throw new NotImplementedException();
	}

	public static string[] getAmPmStrings(string langTag, string[] ampm)
	{
		throw new NotImplementedException();
	}

	public static string[] getEras(string langTag, string[] eras)
	{
		throw new NotImplementedException();
	}

	public static string[] getMonths(string langTag, string[] months)
	{
		throw new NotImplementedException();
	}

	public static string[] getShortMonths(string langTag, string[] smonths)
	{
		throw new NotImplementedException();
	}

	public static string[] getWeekdays(string langTag, string[] wdays)
	{
		throw new NotImplementedException();
	}

	public static string[] getShortWeekdays(string langTag, string[] swdays)
	{
		throw new NotImplementedException();
	}

	public static string getNumberPattern(int numberStyle, string langTag)
	{
		throw new NotImplementedException();
	}

	public static bool isNativeDigit(string langTag)
	{
		throw new NotImplementedException();
	}

	public static string getCurrencySymbol(string langTag, string currencySymbol)
	{
		throw new NotImplementedException();
	}

	public static char getDecimalSeparator(string langTag, char decimalSeparator)
	{
		throw new NotImplementedException();
	}

	public static char getGroupingSeparator(string langTag, char groupingSeparator)
	{
		throw new NotImplementedException();
	}

	public static string getInfinity(string langTag, string infinity)
	{
		throw new NotImplementedException();
	}

	public static string getInternationalCurrencySymbol(string langTag, string internationalCurrencySymbol)
	{
		throw new NotImplementedException();
	}

	public static char getMinusSign(string langTag, char minusSign)
	{
		throw new NotImplementedException();
	}

	public static char getMonetaryDecimalSeparator(string langTag, char monetaryDecimalSeparator)
	{
		throw new NotImplementedException();
	}

	public static string getNaN(string langTag, string nan)
	{
		throw new NotImplementedException();
	}

	public static char getPercent(string langTag, char percent)
	{
		throw new NotImplementedException();
	}

	public static char getPerMill(string langTag, char perMill)
	{
		throw new NotImplementedException();
	}

	public static char getZeroDigit(string langTag, char zeroDigit)
	{
		throw new NotImplementedException();
	}

	public static int getCalendarDataValue(string langTag, int type)
	{
		throw new NotImplementedException();
	}

	public static string getDisplayString(string langTag, int key, string value)
	{
		throw new NotImplementedException();
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
