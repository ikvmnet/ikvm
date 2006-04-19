package gnu.java.lang;

import java.lang.instrument.ClassDefinition;
import java.lang.instrument.Instrumentation;

final class VMInstrumentationImpl
{
    static boolean isRedefineClassesSupported()
    {
        return false;
    }
    
    static void redefineClasses(Instrumentation inst, ClassDefinition[] definitions)
    {
        throw new Error("not supported");
    }
 
    static Class[] getAllLoadedClasses()
    {
        throw new Error("not implemented");
    }

    static Class[] getInitiatedClasses(ClassLoader loader)
    {
        throw new Error("not implemented");
    }

    static long getObjectSize(Object objectToSize)
    {
        throw new Error("not implemented");
    }
}
