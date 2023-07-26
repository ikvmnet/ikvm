using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using usnfs = IKVM.Runtime.Util.Sun.Nio.Fs;

namespace IKVM.Runtime.Java.Externs.sun.nio.fs
{
    public static partial class DotNetWatchService
    {
        public static void close0(object key) => close0Impl(key);

        public static void register0(object key, object dir, object[] events, params object[] modifiers) => register0Impl(key, dir, events, modifiers);

        static partial void close0Impl(object key);

        static partial void register0Impl(object key, object dir, object[] events, object[] modifiers);
    }

#if !FIRST_PASS
    public static partial class DotNetWatchService
    {
        private static DotNetPathAccessor dotNetPath;

        private static DotNetPathAccessor DotNetPath => JVM.BaseAccessors.Get(ref dotNetPath);

        static partial void close0Impl(object key)
            => usnfs.DotNetWatchService.Close(key);

        static partial void register0Impl(object key, object dir, object[] events, object[] modifiers)
            => usnfs.DotNetWatchService.Register(key, DotNetPath.GetPath(dir), events, modifiers);
    }
#endif
}