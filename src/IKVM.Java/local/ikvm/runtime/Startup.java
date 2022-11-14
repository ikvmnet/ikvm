package ikvm.runtime;

public final class Startup
{

    private Startup()
    {
        
    }

    /**
     * Ensures the given assembly is added to the boot class path of the running JVM instance.
     */
    public static native void addBootClassPathAssembly(cli.System.Reflection.Assembly assembly);

}
