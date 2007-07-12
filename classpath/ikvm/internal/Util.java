package ikvm.internal;

import ikvm.lang.Internal;

@Internal
public final class Util
{
    private Util() {}

    public static final boolean WINDOWS = runningOnWindows();

    private static boolean runningOnWindows()
    {
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int platform = os.get_Platform().Value;
        return platform == cli.System.PlatformID.Win32NT
            || platform == cli.System.PlatformID.Win32Windows
	    || platform == cli.System.PlatformID.Win32S
	    || platform == cli.System.PlatformID.WinCE;
    }
	
    public static boolean rangeCheck(int arrayLength, int offset, int length)
    {
        return offset >= 0
	    && offset <= arrayLength
            && length >= 0
            && length <= arrayLength - offset;
    }
}
