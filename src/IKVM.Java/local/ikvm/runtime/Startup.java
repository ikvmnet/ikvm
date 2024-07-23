package ikvm.runtime;

public final class Startup {

    private Startup() {
        
    }

    @ikvm.lang.ModuleInitializer
    public static void init() {
        cli.System.GC.KeepAlive(cli.System.Reflection.Assembly.Load("IKVM.Runtime"));
    };

    /**
     * Ensures the given assembly is added to the boot class path of the running JVM instance.
     */
    public static native void addBootClassPathAssembly(cli.System.Reflection.Assembly assembly);

}
