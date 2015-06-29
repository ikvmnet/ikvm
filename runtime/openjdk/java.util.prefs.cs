/*
  Copyright (C) 2007-2013 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;

static class Java_java_util_prefs_FileSystemPreferences
{
	public static int chmod(string filename, int permission)
	{
		// TODO
		return 0;
	}

	public static int[] lockFile0(string filename, int permission, bool shared)
	{
		// TODO
		return new int[] { 1, 0 };
	}

	public static int unlockFile0(int fd)
	{
		// TODO
		return 0;
	}
}

static class Java_java_util_prefs_WindowsPreferences
{
	// HACK we currently support only 16 handles at a time
	private static readonly Microsoft.Win32.RegistryKey[] keys = new Microsoft.Win32.RegistryKey[16];

	private static Microsoft.Win32.RegistryKey MapKey(int hKey)
	{
		switch (hKey)
		{
			case unchecked((int)0x80000001):
				return Microsoft.Win32.Registry.CurrentUser;
			case unchecked((int)0x80000002):
				return Microsoft.Win32.Registry.LocalMachine;
			default:
				return keys[hKey - 1];
		}
	}

	private static int AllocHandle(Microsoft.Win32.RegistryKey key)
	{
		lock (keys)
		{
			if (key != null)
			{
				for (int i = 0; i < keys.Length; i++)
				{
					if (keys[i] == null)
					{
						keys[i] = key;
						return i + 1;
					}
				}
			}
			return 0;
		}
	}

	private static string BytesToString(byte[] bytes)
	{
		int len = bytes.Length;
		if (bytes[len - 1] == 0)
		{
			len--;
		}
		return Encoding.ASCII.GetString(bytes, 0, len);
	}

	private static byte[] StringToBytes(string str)
	{
		if (str.Length == 0 || str[str.Length - 1] != 0)
		{
			str += '\u0000';
		}
		return Encoding.ASCII.GetBytes(str);
	}

	public static int[] WindowsRegOpenKey(int hKey, byte[] subKey, int securityMask)
	{
		// writeable = DELETE == 0x10000 || KEY_SET_VALUE == 2 || KEY_CREATE_SUB_KEY == 4 || KEY_WRITE = 0x20006;
		// !writeable : KEY_ENUMERATE_SUB_KEYS == 8 || KEY_READ == 0x20019 || KEY_QUERY_VALUE == 1
		bool writable = (securityMask & 0x10006) != 0;
		Microsoft.Win32.RegistryKey resultKey = null;
		int error = 0;
		try
		{
			Microsoft.Win32.RegistryKey parent = MapKey(hKey);
			// HACK we check if we can write in the system preferences 
			// we want not user registry virtualization for compatibility
			if (writable && parent.Name.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.Ordinal) && UACVirtualization.Enabled)
			{
				resultKey = parent.OpenSubKey(BytesToString(subKey), false);
				if (resultKey != null)
				{
					// error only if key exists
					resultKey.Close();
					error = 5;
					resultKey = null;
				}
			}
			else
			{
				resultKey = parent.OpenSubKey(BytesToString(subKey), writable);
			}
		}
		catch (SecurityException)
		{
			error = 5;
		}
		catch (UnauthorizedAccessException)
		{
			error = 5;
		}
		return new int[] { AllocHandle(resultKey), error };
	}

	public static int WindowsRegCloseKey(int hKey)
	{
		keys[hKey - 1].Close();
		lock (keys)
		{
			keys[hKey - 1] = null;
		}
		return 0;
	}

	public static int[] WindowsRegCreateKeyEx(int hKey, byte[] subKey)
	{
		Microsoft.Win32.RegistryKey resultKey = null;
		int error = 0;
		int disposition = -1;
		try
		{
			Microsoft.Win32.RegistryKey key = MapKey(hKey);
			string name = BytesToString(subKey);
			resultKey = key.OpenSubKey(name);
			disposition = 2;
			if (resultKey == null)
			{
				resultKey = key.CreateSubKey(name);
				disposition = 1;
			}
		}
		catch (SecurityException)
		{
			error = 5;
		}
		catch (UnauthorizedAccessException)
		{
			error = 5;
		}
		return new int[] { AllocHandle(resultKey), error, disposition };
	}

	public static int WindowsRegDeleteKey(int hKey, byte[] subKey)
	{
		try
		{
			MapKey(hKey).DeleteSubKey(BytesToString(subKey), false);
			return 0;
		}
		catch (SecurityException)
		{
			return 5;
		}
	}

	public static int WindowsRegFlushKey(int hKey)
	{
		MapKey(hKey).Flush();
		return 0;
	}

	public static byte[] WindowsRegQueryValueEx(int hKey, byte[] valueName)
	{
		try
		{
			string value = MapKey(hKey).GetValue(BytesToString(valueName)) as string;
			if (value == null)
			{
				return null;
			}
			return StringToBytes(value);
		}
		catch (SecurityException)
		{
			return null;
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
	}

	public static int WindowsRegSetValueEx(int hKey, byte[] valueName, byte[] data)
	{
		if (valueName == null || data == null)
		{
			return -1;
		}
		try
		{
			MapKey(hKey).SetValue(BytesToString(valueName), BytesToString(data));
			return 0;
		}
		catch (SecurityException)
		{
			return 5;
		}
		catch (UnauthorizedAccessException)
		{
			return 5;
		}
	}

	public static int WindowsRegDeleteValue(int hKey, byte[] valueName)
	{
		try
		{
			MapKey(hKey).DeleteValue(BytesToString(valueName));
			return 0;
		}
		catch (System.ArgumentException)
		{
			return 2; //ERROR_FILE_NOT_FOUND
		}
		catch (SecurityException)
		{
			return 5; //ERROR_ACCESS_DENIED
		}
		catch (UnauthorizedAccessException)
		{
			return 5; //ERROR_ACCESS_DENIED
		}
	}

	public static int[] WindowsRegQueryInfoKey(int hKey)
	{
		int[] result = new int[5] { -1, -1, -1, -1, -1 };
		try
		{
			Microsoft.Win32.RegistryKey key = MapKey(hKey);
			result[0] = key.SubKeyCount;
			result[1] = 0;
			result[2] = key.ValueCount;
			foreach (string s in key.GetSubKeyNames())
			{
				result[3] = Math.Max(result[3], s.Length);
			}
			foreach (string s in key.GetValueNames())
			{
				result[4] = Math.Max(result[4], s.Length);
			}
		}
		catch (SecurityException)
		{
			result[1] = 5;
		}
		catch (UnauthorizedAccessException)
		{
			result[1] = 5;
		}
		return result;
	}

	public static byte[] WindowsRegEnumKeyEx(int hKey, int subKeyIndex, int maxKeyLength)
	{
		try
		{
			return StringToBytes(MapKey(hKey).GetSubKeyNames()[subKeyIndex]);
		}
		catch (SecurityException)
		{
			return null;
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
	}

	public static byte[] WindowsRegEnumValue(int hKey, int valueIndex, int maxValueNameLength)
	{
		try
		{
			return StringToBytes(MapKey(hKey).GetValueNames()[valueIndex]);
		}
		catch (SecurityException)
		{
			return null;
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
	}
}

static class UACVirtualization
{
	private enum TOKEN_INFORMATION_CLASS
	{
		TokenVirtualizationEnabled = 24
	}

	[DllImport("advapi32.dll")]
	private static extern int GetTokenInformation(
		IntPtr TokenHandle,
		TOKEN_INFORMATION_CLASS TokenInformationClass,
		out int TokenInformation,
		int TokenInformationLength,
		out int ReturnLength);

	internal static bool Enabled
	{
		[SecuritySafeCritical]
		get
		{
			OperatingSystem os = Environment.OSVersion;
			if (os.Platform != PlatformID.Win32NT || os.Version.Major < 6)
			{
				return false;
			}
			int enabled, length;
			return GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenVirtualizationEnabled, out enabled, 4, out length) != 0
				&& enabled != 0;
		}
	}
}
