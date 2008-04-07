package ikvm.internal;

import ikvm.lang.Internal;

@Internal
public final class Util
{
    private Util() {}

    public static final boolean WINDOWS = runningOnWindows();
    public static final boolean MACOSX = runningOnMacOSX();

    private static boolean runningOnWindows()
    {
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int platform = os.get_Platform().Value;
        return platform == cli.System.PlatformID.Win32NT
            || platform == cli.System.PlatformID.Win32Windows
	    || platform == cli.System.PlatformID.Win32S
	    || platform == cli.System.PlatformID.WinCE;
    }

    private static boolean runningOnMacOSX()
    {
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int platform = os.get_Platform().Value;
        if (platform == 6 /* Silverlight MacOSX PlatformID constant */)
        {
	    return true;
        }
        if (platform == cli.System.PlatformID.Unix)
        {
	    // we're on some sort of Unix, that probably means we're on Mono,
	    // so we use its uname method to determine if we're on Darwin
	    if ("Darwin".equals(MonoUtils.unameProperty("sysname")))
	    {
		return true;
	    }
        }
        return false;
    }
    
    public static boolean rangeCheck(int arrayLength, int offset, int length)
    {
        return offset >= 0
	    && offset <= arrayLength
            && length >= 0
            && length <= arrayLength - offset;
    }
}
