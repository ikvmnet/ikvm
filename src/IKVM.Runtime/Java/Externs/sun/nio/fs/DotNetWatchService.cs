using System;
using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using usnfs = IKVM.Runtime.Util.Sun.Nio.Fs;

namespace IKVM.Java.Externs.sun.nio.fs
{
    internal static partial class DotNetWatchService
    {

        public static void close0(object key)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            usnfs.DotNetWatchService.Close(key);
#endif
        }

        public static void register0(object fs, object key, object dir, object[] events, params object[] modifiers)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            usnfs.DotNetWatchService.Register(fs, key, dir, events, modifiers);
#endif
        }

    }

}