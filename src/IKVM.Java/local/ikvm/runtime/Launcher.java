package ikvm.runtime;

import java.util.Properties;

import cli.System.Type;

public final class Launcher
{

    private Launcher()
    {
        
    }

    /**
     * Launches the given .NET type as a Java application class. This method should be invoked before any other JVM work is
     * conducted, ideally by generated executables.
     */
    @cli.IKVM.Attributes.HideFromJavaAttribute.Annotation
    public static native int run(Type main, String[] args, String jvmArgPrefix, Properties properties);

}
