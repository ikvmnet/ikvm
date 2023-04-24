using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.DotNetAsynchronousSocketChannelImpl' type.
    /// </summary>
    internal sealed class DotNetAsynchronousSocketChannelImplAccessor : Accessor<object>
    {

        Type javaIoFileDescriptor;
        Type javaNetSocketAddress;
        Type sunNioChDotNetAsynchronousChannelGroup;

        MethodAccessor<Func<object, object>> init1;
        MethodAccessor<Func<object, bool, object>> init2;
        MethodAccessor<Func<object, object, object, object>> init3;
        FieldAccessor<object, object> fd;
        MethodAccessor<Action<object>> begin;
        MethodAccessor<Action<object>> end;
        MethodAccessor<Action<object>> enableReading;
        MethodAccessor<Action<object>> enableWriting;
        MethodAccessor<Action<object, bool>> enableReading2;
        MethodAccessor<Action<object, bool>> enableWriting2;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetAsynchronousSocketChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.ch.DotNetAsynchronousSocketChannelImpl")
        {

        }

        Type JavaIoFileDescriptor => Resolve(ref javaIoFileDescriptor, "java.io.FileDescriptor");

        Type SunNioChDotNetAsynchronousChannelGroup => Resolve(ref sunNioChDotNetAsynchronousChannelGroup, "sun.nio.ch.DotNetAsynchronousChannelGroup");

        Type JavaNetInetSocketAddress => Resolve(ref javaNetSocketAddress, "java.net.InetSocketAddress");

        public object Init(object group) => GetConstructor(ref init1, SunNioChDotNetAsynchronousChannelGroup).Invoker(group);

        public object Init(object group, bool failIfGroupShutdown) => GetConstructor(ref init2, SunNioChDotNetAsynchronousChannelGroup, typeof(bool)).Invoker(group, failIfGroupShutdown);

        public object Init(object group, object fd, object remote) => GetConstructor(ref init3, SunNioChDotNetAsynchronousChannelGroup, JavaIoFileDescriptor, JavaNetInetSocketAddress).Invoker(group, fd, remote);

        public object GetFd(object self) => GetField(ref fd, nameof(fd)).GetValue(self);

        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd)).SetValue(self, value);

        public void InvokeBegin(object self) => GetMethod(ref begin, nameof(begin), typeof(void)).Invoker(self);

        public void InvokeEnd(object self) => GetMethod(ref end, nameof(end), typeof(void)).Invoker(self);

        public void InvokeEnableReading(object self) => GetMethod(ref enableReading, "enableReading", typeof(void), typeof(bool)).Invoker(self);

        public void InvokeEnableWriting(object self) => GetMethod(ref enableWriting, "enableWriting", typeof(void), typeof(bool)).Invoker(self);

        public void InvokeEnableReading(object self, bool killed) => GetMethod(ref enableReading2, "enableReading", typeof(void), typeof(bool)).Invoker(self, killed);

        public void InvokeEnableWriting(object self, bool killed) => GetMethod(ref enableWriting2, "enableWriting", typeof(void), typeof(bool)).Invoker(self, killed);

    }

#endif

}
