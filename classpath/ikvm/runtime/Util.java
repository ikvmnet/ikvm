package ikvm.runtime;

import cli.System.Type;
import cli.System.RuntimeTypeHandle;

public final class Util
{
    private Util()
    {
    }

    public static native Class getClassFromObject(Object o);

    public static native Class getClassFromTypeHandle(RuntimeTypeHandle handle);

    public static native Class getFriendlyClassFromType(Type type);

    public static Type getInstanceTypeFromClass(Class classObject)
    {
        return GetInstanceTypeFromTypeWrapper(VMClass.getWrapper(classObject));
    }

    private static native Type GetInstanceTypeFromTypeWrapper(Object wrapper);

    //[HideFromJava]
    public static Throwable mapException(Throwable x)
    {
        return ExceptionHelper.MapExceptionFast(x, true);
    }
}
