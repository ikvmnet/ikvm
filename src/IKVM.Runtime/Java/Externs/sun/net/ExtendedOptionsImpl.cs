using System;

namespace IKVM.Runtime.Java.Externs.sun.net
{

    static class ExtendedOptionsImpl
    {

        public static void init()
        {

        }

        public static bool flowSupported()
        {
            return false;
        }

        public static void getFlowOption(object this_, object f_)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new global::java.lang.UnsupportedOperationException();
#endif
        }

        public static void setFlowOption(object this_, object f_)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new global::java.lang.UnsupportedOperationException();
#endif
        }

    }

}
