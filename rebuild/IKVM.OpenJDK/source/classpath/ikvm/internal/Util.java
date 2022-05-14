package ikvm.internal;

import ikvm.lang.Internal;

@Internal
public final class Util
{
    private Util() {}

    public static final boolean MONO = cli.System.Type.GetType("Mono.Runtime") != null;

    public static final boolean WINDOWS;
    public static final boolean MACOSX;

    static
    {
        switch (cli.System.Environment.get_OSVersion().get_Platform().Value)
        {
            case cli.System.PlatformID.Win32NT:
            case cli.System.PlatformID.Win32Windows:
            case cli.System.PlatformID.Win32S:
            case cli.System.PlatformID.WinCE:
                WINDOWS = true;
                MACOSX = false;
                break;
            case cli.System.PlatformID.MacOSX:
                WINDOWS = false;
                MACOSX = true;
                break;
            case cli.System.PlatformID.Unix:
                WINDOWS = false;
                // as of version 2.6, Mono still returns Unix when running on MacOSX
                MACOSX = "Darwin".equals(MonoUtils.unameProperty("sysname"));
                break;
            default:
                WINDOWS = false;
                MACOSX = false;
                break;
        }
    }

    public static boolean rangeCheck(int arrayLength, int offset, int length)
    {
        return offset >= 0
	    && offset <= arrayLength
            && length >= 0
            && length <= arrayLength - offset;
    }

    public static String SafeGetEnvironmentVariable(String name)
    {
        try
        {
            if (false) throw new cli.System.Security.SecurityException();
            return cli.System.Environment.GetEnvironmentVariable(name);
        }
        catch (cli.System.Security.SecurityException _)
        {
            return null;
        }
    }
}
