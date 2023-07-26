using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using usnfs = IKVM.Runtime.Util.Sun.Nio.Fs;

namespace IKVM.Java.Externs.sun.nio.fs
{
    internal static partial class DotNetWatchService
    {

        public static void close0(object key) => close0Impl(key);

        public static void register0(object fs, object key, object dir, object[] events, params object[] modifiers) => register0Impl(fs, key, dir, events, modifiers);

        static partial void close0Impl(object key);

        static partial void register0Impl(object fs, object key, object dir, object[] events, object[] modifiers);

    }

#if !FIRST_PASS

    internal static partial class DotNetWatchService
    {

        private static DotNetPathAccessor dotNetPath;

        private static DotNetPathAccessor DotNetPath => JVM.BaseAccessors.Get(ref dotNetPath);

        static partial void close0Impl(object key)
            => usnfs.DotNetWatchService.Close(key);

        static partial void register0Impl(object fs, object key, object dir, object[] events, object[] modifiers)
            => usnfs.DotNetWatchService.Register(fs, key, DotNetPath.GetPath(dir), events, modifiers);

    }

#endif

}