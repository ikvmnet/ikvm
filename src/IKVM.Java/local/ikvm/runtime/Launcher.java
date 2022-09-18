package ikvm.runtime;

import java.util.Properties;

public final class Launcher
{

    private Launcher()
    {
        
    }

    public static native int run(Class main, String[] args, String jvmArgPrefix, Properties properties);

}
