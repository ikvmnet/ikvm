using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Fs
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal class DotNetWatchServiceKeyAccessor : Accessor<object>
    {
        private MethodAccessor<Action<object>> error0;
        private MethodAccessor<Action<object>> signalEvent0;
        private MethodAccessor<Action<object, object, object>> signalEvent1;
        private FieldAccessor<object, object> state;
        private Type voidType;
        private Type watchEventKind;

        internal FieldAccessor<object, object> State => GetField(ref state, "state");

        private Type Void => voidType ??= typeof(void);

        private Type WatchEventKind => watchEventKind ??= Resolve("java.nio.file.WatchEvent+Kind");

        public DotNetWatchServiceKeyAccessor(AccessorTypeResolver resolver)
            : base(resolver, "sun.nio.fs.DotNetWatchService+DotNetWatchKey")
        {
        }

        public void error(object self)
            => (error0 ??= GetMethod(ref error0, nameof(error), Void)).Invoker(self);

        public void signalEvent(object self)
            => (signalEvent0 ??= GetMethod(ref signalEvent0, nameof(signalEvent), Void)).Invoker(self);

        public void signalEvent(object self, object kind, object context)
            => (signalEvent1 ??= GetMethod(ref signalEvent1, nameof(signalEvent), Void, WatchEventKind, typeof(object))).Invoker(self, kind, context);
    }

#endif

}
