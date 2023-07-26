using IKVM.Runtime.Accessors.Com.Sun.Nio.File;
using IKVM.Runtime.Accessors.Java.Nio.File;
using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using IKVM.Runtime.Extensions;
using System;
using System.ComponentModel;
using System.IO;

namespace IKVM.Runtime.Util.Sun.Nio.Fs
{

#if !FIRST_PASS

    internal static class DotNetWatchService
    {
        private static DotNetPathAccessor dotNetPath;
        private static DotNetWatchServiceKeyAccessor dotNetWatchServiceKey;
        private static ExtendedWatchEventModifierAccessor extendedWatchEventModifier;
        private static SensitivityWatchEventModifierAccessor sensitivityWatchEventModifier;
        private static StandardWatchEventKindsAccessor standardWatchEventKinds;

        private static DotNetPathAccessor DotNetPath => JVM.BaseAccessors.Get(ref dotNetPath);

        private static DotNetWatchServiceKeyAccessor DotNetWatchServiceKey => JVM.BaseAccessors.Get(ref dotNetWatchServiceKey);

        private static ExtendedWatchEventModifierAccessor ExtendedWatchEventModifier => JVM.BaseAccessors.Get(ref extendedWatchEventModifier);

        private static SensitivityWatchEventModifierAccessor SensitivityWatchEventModifier => JVM.BaseAccessors.Get(ref sensitivityWatchEventModifier);

        private static StandardWatchEventKindsAccessor StandardWatchEventKinds => JVM.BaseAccessors.Get(ref standardWatchEventKinds);

        internal static void Close(object key)
        {
            if (DotNetWatchServiceKey.State.ExchangeValue(key, null) is not FileSystemWatcher fsw)
            {
                return;
            }

            fsw.Dispose();
        }

        internal static void Register(object fs, object key, string dir, object[] events, object[] modifiers)
        {
            // we could reuse the FileSystemWatcher, but for now we just recreate it
            // (and we run the risk of missing some events while we're doing that)
            Close(key);
            FileSystemWatcher watcher = null;
            try
            {
                watcher = new FileSystemWatcher(dir);
                WatchServiceHandler handler = new WatchServiceHandler(fs, key);
                bool valid = false;
                foreach (var @event in events)
                {
                    if (@event == StandardWatchEventKinds.OVERFLOW)
                    {
                        handler.Overflow = true;
                    }
                    else
                    {
                        valid = true;
                        if (@event == StandardWatchEventKinds.ENTRY_CREATE)
                        {
                            watcher.Created += handler.EventHandler;
                        }
                        else if (@event == StandardWatchEventKinds.ENTRY_DELETE)
                        {
                            watcher.Deleted += handler.EventHandler;
                        }
                        else if (@event == StandardWatchEventKinds.ENTRY_MODIFY)
                        {
                            watcher.Changed += handler.EventHandler;
                        }
                        else
                        {
                            @event.GetType();
                            throw new global::java.lang.UnsupportedOperationException();
                        }
                    }
                }

                if (!valid)
                {
                    throw new global::java.lang.IllegalArgumentException();
                }

                watcher.Error += handler.ErrorHandler;
                foreach (var modifier in modifiers)
                {
                    if (modifier == ExtendedWatchEventModifier.FILE_TREE)
                    {
                        watcher.IncludeSubdirectories = true;
                    }
                    else if (SensitivityWatchEventModifier.Is(modifier))
                    {
                        // Ignore
                    }
                    else
                    {
                        modifier.GetType();
                        throw new global::java.lang.UnsupportedOperationException();
                    }
                }

                watcher.EnableRaisingEvents = true;
                DotNetWatchServiceKey.State.SetValue(key, watcher);
                watcher = null;
            }
            catch (Exception e) when (e is ArgumentException or FileNotFoundException)
            {
                throw new global::java.io.FileNotFoundException();
            }
            finally
            {
                watcher?.Dispose();
            }
        }

        private class WatchServiceHandler
        {
            private readonly object fs;
            private readonly object key;

            public readonly ErrorEventHandler ErrorHandler;

            public readonly FileSystemEventHandler EventHandler;

            public bool Overflow { get; set; }

            public WatchServiceHandler(object fs, object key)
            {
                this.key = key;
                ErrorHandler = OnError;
                EventHandler = OnEvent;
            }

            private void OnError(object sender, ErrorEventArgs e)
            {
                const int E_FAIL = unchecked((int)0x80004005);

                if (e.GetException() is Win32Exception win32Exception
                    && win32Exception.ErrorCode == E_FAIL)
                {
                    DotNetWatchServiceKey.error(key);
                }
                else if (Overflow)
                {
                    DotNetWatchServiceKey.signalEvent(key,
                        StandardWatchEventKinds.OVERFLOW, null);
                }
            }

            private void OnEvent(object sender, FileSystemEventArgs e)
            {
                DotNetWatchServiceKey.signalEvent(key,
                    e.ChangeType switch
                    {
                        WatcherChangeTypes.Created => StandardWatchEventKinds.ENTRY_CREATE,
                        WatcherChangeTypes.Deleted => StandardWatchEventKinds.ENTRY_DELETE,
                        _ => StandardWatchEventKinds.ENTRY_MODIFY
                    }, DotNetPath.Init(fs, e.Name));
            }

        }

    }

#endif

}
