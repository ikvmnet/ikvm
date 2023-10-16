package ikvm.internal;

import ikvm.lang.Internal;

import cli.System.Environment;
import cli.System.Type;
import cli.System.Runtime.InteropServices.RuntimeInformation;
import cli.System.Runtime.InteropServices.OSPlatform;

@Internal
public final class Util {

    public static boolean rangeCheck(int arrayLength, int offset, int length) {
        return offset >= 0 && offset <= arrayLength && length >= 0 && length <= arrayLength - offset;
    }

    public static String SafeGetEnvironmentVariable(String name) {
        try {
            if (false) throw new cli.System.Security.SecurityException();
            return Environment.GetEnvironmentVariable(name);
        } catch (cli.System.Security.SecurityException _) {
            return null;
        }
    }

    private Util() { }

}
