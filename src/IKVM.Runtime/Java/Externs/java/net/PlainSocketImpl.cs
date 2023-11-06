using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;
using IKVM.Runtime.Util.Java.Net;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    /// <summary>
    /// Implements the external methods for <see cref="global::java.net.PlainSocketImpl"/>.
    /// </summary>
    static class PlainSocketImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static PlainSocketImplAccessor plainSocketImplAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static PlainSocketImplAccessor PlainSocketImplAccessor => JVM.BaseAccessors.Get(ref plainSocketImplAccessor);

        [Flags]
        enum HANDLE_FLAGS
        {

            NONE = 0,
            INHERIT = 0x00000001,
            PROTECT_FROM_CLOSE = 0x00000002,

        }

#if NETFRAMEWORK

        /// <summary>
        /// Invokes the Win32 SetHandleInformation function.
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="dwMask"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetHandleInformation(IntPtr hObject, HANDLE_FLAGS dwMask, HANDLE_FLAGS dwFlags);
#else

        /// <summary>
        /// Invokes the Win32 SetHandleInformation function.
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="dwMask"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetHandleInformation(SafeHandle hObject, HANDLE_FLAGS dwMask, HANDLE_FLAGS dwFlags);

#endif

        /// <summary>
        /// Compiles a fast getter for a <see cref="FieldInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        static Func<T, R> MakeFieldGetter<T, R>(FieldInfo field)
        {
            var p = Expression.Parameter(typeof(T));
            var e = Expression.Lambda<Func<T, R>>(Expression.Field(Expression.ConvertChecked(p, field.DeclaringType), field), p);
            return e.Compile();
        }

        /// <summary>
        /// Compiles a fast setter for a <see cref="FieldInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        static Action<T, V> MakeFieldSetter<T, V>(FieldInfo field)
        {
            var p = Expression.Parameter(typeof(T));
            var v = Expression.Parameter(typeof(V));
            var e = Expression.Lambda<Action<T, V>>(Expression.Assign(Expression.Field(field.DeclaringType.IsValueType ? Expression.Unbox(p, field.DeclaringType) : Expression.ConvertChecked(p, field.DeclaringType), field), v), p, v);
            return e.Compile();
        }

        static readonly FieldInfo AbstractPlainSocketImplTrafficClassField = typeof(global::java.net.AbstractPlainSocketImpl).GetField("trafficClass", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.AbstractPlainSocketImpl, int> AbstractPlainSocketImplTrafficClassGetter = MakeFieldGetter<global::java.net.AbstractPlainSocketImpl, int>(AbstractPlainSocketImplTrafficClassField);
        static readonly FieldInfo AbstractPlainSocketImplServerSocketField = typeof(global::java.net.AbstractPlainSocketImpl).GetField("serverSocket", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.AbstractPlainSocketImpl, global::java.net.ServerSocket> AbstractPlainSocketImplServerSocketGetter = MakeFieldGetter<global::java.net.AbstractPlainSocketImpl, global::java.net.ServerSocket>(AbstractPlainSocketImplServerSocketField);
        static readonly FieldInfo Inet6AddressCachedScopeIdField = typeof(global::java.net.Inet6Address).GetField("cached_scope_id", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<global::java.net.Inet6Address, int> Inet6AddressCachedScopeIdSetter = MakeFieldSetter<global::java.net.Inet6Address, int>(Inet6AddressCachedScopeIdField);
        static readonly Func<global::java.net.Inet6Address, int> Inet6AddressCachedScopeIdGetter = MakeFieldGetter<global::java.net.Inet6Address, int>(Inet6AddressCachedScopeIdField);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__initProto(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__initProto __jniPtr__initProto;
        delegate void __jniDelegate__socketCreate(IntPtr jniEnv, IntPtr self, sbyte stream);
        static __jniDelegate__socketCreate __jniPtr__socketCreate;
        delegate void __jniDelegate__socketConnect(IntPtr jniEnv, IntPtr self, IntPtr iaObj, int port, int timeout);
        static __jniDelegate__socketConnect __jniPtr__socketConnect;
        delegate void __jniDelegate__socketBind(IntPtr jniEnv, IntPtr self, IntPtr iaObj, int localport);
        static __jniDelegate__socketBind __jniPtr__socketBind;
        delegate void __jniDelegate__socketListen(IntPtr jniEnv, IntPtr self, int count);
        static __jniDelegate__socketListen __jniPtr__socketListen;
        delegate void __jniDelegate__socketAccept(IntPtr jniEnv, IntPtr self, IntPtr socket);
        static __jniDelegate__socketAccept __jniPtr__socketAccept;
        delegate int __jniDelegate__socketAvailable(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__socketAvailable __jniPtr__socketAvailable;
        delegate int __jniDelegate__socketSetOption(IntPtr jniEnv, IntPtr self, int cmd, sbyte on, IntPtr value);
        static __jniDelegate__socketSetOption __jniPtr__socketSetOption;
        delegate int __jniDelegate__socketGetOption(IntPtr jniEnv, IntPtr self, int cmd, IntPtr iaContainerObj);
        static __jniDelegate__socketGetOption __jniPtr__socketGetOption;
        delegate int __jniDelegate__socketSendUrgentData(IntPtr jniEnv, IntPtr self, int data);
        static __jniDelegate__socketSendUrgentData __jniPtr__socketSendUrgentData;
        delegate int __jniDelegate__socketClose0(IntPtr jniEnv, IntPtr self, sbyte useDeferredClose);
        static __jniDelegate__socketClose0 __jniPtr__socketClose0;
        delegate int __jniDelegate__socketShutdown(IntPtr jniEnv, IntPtr self, int howto);
        static __jniDelegate__socketShutdown __jniPtr__socketShutdown;

        /// <summary>
        /// Converts the given <see cref="InetAddress"/> into an appropriate endpoint address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        static IPAddress GetEndPointAddress(global::java.net.InetAddress address)
        {
            var a = address.ToIPAddress();
            if (address is global::java.net.Inet6Address ip6 && a.IsIPv6LinkLocal && a.ScopeId == 0)
                a.ScopeId = GetDefaultScopeId(ip6);
            return a;
        }

        /// <summary>
        /// Attempts to get the scope ID of a link-local <see cref="global::java.net.Inet6Address"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        static int GetDefaultScopeId(global::java.net.Inet6Address address)
        {
            if (Inet6AddressCachedScopeIdGetter(address) is int s2 and not 0)
                return s2;

            // for Linux we need to obtain the default scope ID by effectively doing a route lookup
            if (RuntimeUtil.IsLinux && FindScopeId(address) is int s3 and not 0)
            {
                Inet6AddressCachedScopeIdSetter(address, s3);
                return s3;
            }

            return 0;
        }

        /// <summary>
        /// Attempts to lookup the scope id from a non-scoped address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        static int FindScopeId(global::java.net.Inet6Address address)
        {
            var l = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(i => i.Address.IsIPv6LinkLocal && i.Address.ScopeId != 0)
                .Where(i => MemoryExtensions.SequenceEqual(i.Address.GetAddressBytes().AsSpan(), address.getAddress().AsSpan()))
                .Take(2)
                .ToList();

            if (l.Count > 1)
                throw new global::java.net.SocketException("Duplicate link local addresses: must specify scope-id");
            if (l.Count > 0)
                return (int)l[0].Address.ScopeId;

            return 0;
        }

#endif

        static readonly byte[] PeekBuffer = new byte[1];

        /// <summary>
        /// Implements the native method for 'initProto'.
        /// </summary>
        public static void initProto()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__initProto ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__initProto>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(initProto), "()V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__initProto(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::java.net.PlainSocketImpl>.Value));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketCreate'.
        /// </summary>
        public static void socketCreate(object self, bool stream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    var socket = new Socket(stream ? SocketType.Stream : SocketType.Dgram, ProtocolType.Tcp);
                    socket.Blocking = true;

                    // if this is a server socket then enable SO_REUSEADDR automatically
                    if (AbstractPlainSocketImplServerSocketGetter(impl) != null)
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    FileDescriptorAccessor.SetSocket(impl.fd, socket);
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketCreate ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketCreate>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketCreate), "(Z)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketCreate(jniEnv, jniFrm.MakeLocalRef(self), stream ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketConnect'.
        /// </summary>
        public static void socketConnect(object self, object iaObj, int port, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl, global::java.net.InetAddress>(self, iaObj, (impl, address) =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var trafficClass = AbstractPlainSocketImplTrafficClassGetter(impl);
                    if (trafficClass != 0)
                        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.TypeOfService, trafficClass);

                    var prevBlocking = socket.Blocking;

                    try
                    {
                        if (timeout > 0)
                        {
                            socket.Blocking = false;

                            // non-blocking connect throws a WouldBlock exception, after which we Poll for completion
                            try
                            {
                                socket.Connect(GetEndPointAddress(address), port);
                            }
                            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
                            {
                                var re = new List<Socket>() { socket };
                                var wr = new List<Socket>() { socket };
                                var ex = new List<Socket>() { socket };
                                Socket.Select(re, wr, ex, (int)Math.Min(timeout * 1000L, int.MaxValue));
                                var er = (int)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
                                if (er != 0)
                                    throw new SocketException(er);
                                if (wr.Count == 0)
                                    throw new global::java.net.SocketTimeoutException("Connect timed out.");
                            }
                        }
                        else
                        {
                            socket.Blocking = true;
                            socket.Connect(GetEndPointAddress(address), port);
                        }

                        impl.address = address;
                        impl.port = port;

                        if (impl.localport == 0)
                            impl.localport = ((IPEndPoint)socket.LocalEndPoint).Port;
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new global::java.io.InterruptedIOException();
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.InvalidArgument)
                    {
                        throw new global::java.net.SocketException(e.Message);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        throw new global::java.net.ConnectException(e.Message);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.HostUnreachable)
                    {
                        throw new global::java.net.NoRouteToHostException(e.Message);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.AddressNotAvailable)
                    {
                        throw new global::java.net.NoRouteToHostException(e.Message);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.TimedOut)
                    {
                        throw new global::java.net.ConnectException(e.Message);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.IsConnected)
                    {
                        throw new global::java.net.SocketException("Socket closed");
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketConnect ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketConnect>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketConnect), "(Ljava/net/InetAddress;I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketConnect(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(iaObj), port, timeout);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketBind'.
        /// </summary>
        public static void socketBind(object self, global::java.net.InetAddress iaObj, int localport)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    try
                    {
                        socket.Bind(new IPEndPoint(iaObj.isAnyLocalAddress() ? IPAddress.IPv6Any : GetEndPointAddress(iaObj), localport));
                        impl.address = iaObj;
                        impl.localport = localport == 0 ? ((IPEndPoint)socket.LocalEndPoint).Port : localport;
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new global::java.io.InterruptedIOException("Bind failed");
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        throw new global::java.net.BindException("Bind failed");
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.AddressNotAvailable)
                    {
                        throw new global::java.net.BindException("Bind failed");
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.AccessDenied)
                    {
                        throw new global::java.net.BindException("Bind failed");
                    }
                    catch (SocketException)
                    {
                        throw new global::java.net.SocketException("Bind failed");
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketBind ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketBind>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketBind), "(Ljava/net/InetAddress;I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketBind(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(iaObj), localport);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketListen'.
        /// </summary>
        public static void socketListen(object self, int count)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    socket.Listen(count);
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketListen ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketListen>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketListen), "(I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketListen(jniEnv, jniFrm.MakeLocalRef(self), count);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketAccept'.
        /// </summary>
        public static void socketAccept(object self, object s_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl, global::java.net.SocketImpl>(self, s_, (impl, s) =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var prevBlocking = socket.Blocking;

                    try
                    {
                        socket.Blocking = true;

                        // wait for connection attempt
                        if (impl.timeout > 0)
                            if (socket.Poll((int)Math.Min(impl.timeout * 1000L, int.MaxValue), SelectMode.SelectRead) == false)
                                throw new global::java.net.SocketTimeoutException("Accept timed out.");

                        // accept new socket
                        var newSocket = socket.Accept();
                        if (newSocket == null)
                            throw new global::java.net.SocketException("Invalid socket.");

                        // associate new FileDescriptor with socket
                        var newfd = new global::java.io.FileDescriptor();
                        FileDescriptorAccessor.SetSocket(newfd, newSocket);

                        // populate newly accepted socket
                        var remoteIpEndPoint = (IPEndPoint)newSocket.RemoteEndPoint;
                        s.fd = newfd;
                        s.address = remoteIpEndPoint.ToInetAddress();
                        s.port = remoteIpEndPoint.Port;
                        s.localport = impl.port;
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new global::java.io.InterruptedIOException();
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        throw new global::java.io.InterruptedIOException(e.Message);
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketAccept ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketAccept>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketAccept), "(Ljava/net/SocketImpl;)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketAccept(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(s_));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketAvailable'.
        /// </summary>
        public static int socketAvailable(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return InvokeFunc<global::java.net.PlainSocketImpl, int>(this_, impl =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    try
                    {
                        return socket.Available;
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        throw new global::sun.net.ConnectionResetException();
                    }
                    catch (SocketException e)
                    {
                        throw new global::java.net.SocketException(e.Message);
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketAvailable ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketAvailable>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketAvailable), "()I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__socketAvailable(jniEnv, jniFrm.MakeLocalRef(this_));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketClose0'.
        /// </summary>
        public static void socketClose0(object self, bool useDeferredClose)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    // exit if descriptor already closed
                    if (impl.fd == null || FileDescriptorAccessor.GetHandle(impl.fd) == -1)
                        return;

                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket already closed.");

                    // if we're not configured to linger, disable sending, but continue to allow receive
                    if (socket.LingerState.Enabled == false)
                    {
                        try
                        {
                            socket.Shutdown(SocketShutdown.Send);
                        }
                        catch (SocketException)
                        {
                            // ignore
                        }
                    }

                    // null socket before close, as close may take a minute to flush
                    var h = FileDescriptorAccessor.GetHandle(impl.fd);
                    FileDescriptorAccessor.SetHandle(impl.fd, -1);
                    LibIkvm.Instance.io_close_socket(h);
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketClose0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketClose0>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketClose0), "(Z)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketClose0(jniEnv, jniFrm.MakeLocalRef(self), useDeferredClose ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketShutdown'.
        /// </summary>
        public static void socketShutdown(object self, int howto)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    if (impl.fd == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    socket.Shutdown((SocketShutdown)howto);
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketShutdown ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketShutdown>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketShutdown), "(I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketShutdown(jniEnv, jniFrm.MakeLocalRef(self), howto);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketSetOption0'.
        /// </summary>
        public static void socketSetOption0(object self, int cmd, bool on, object value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(self, impl =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    if (value == null)
                        throw new global::java.lang.NullPointerException(nameof(value));

                    // .NET provides property
                    if (cmd == global::java.net.SocketOptions.SO_TIMEOUT)
                    {
                        var timeout = (global::java.lang.Integer)value;
                        socket.ReceiveTimeout = timeout.intValue();
                        return;
                    }

                    // .NET provides property
                    if (cmd == global::java.net.SocketOptions.SO_LINGER)
                    {
                        var linger = (global::java.lang.Integer)value;
                        socket.LingerState = new LingerOption(on, linger.intValue());
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.SO_RCVBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.ReceiveBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.SO_SNDBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.SendBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.TCP_NODELAY)
                    {
                        socket.NoDelay = on;
                        return;
                    }

                    // lookup option options
                    if (SocketOptionUtil.TryGetDotNetSocketOption(cmd, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    // configure socket
                    socket.SetSocketOption(options.Level, options.Name, value switch
                    {
                        global::java.lang.Boolean b => GetSocketOptionSetValue(options.Type, b.booleanValue()),
                        global::java.lang.Integer i => GetSocketOptionSetValue(options.Type, i.intValue()),
                        _ => throw new global::java.net.SocketException("Invalid option value."),
                    });
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketSetOption ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketSetOption>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketSetOption0), "(IZLjava/lang/Object;)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketSetOption(jniEnv, jniFrm.MakeLocalRef(self), cmd, on ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE, jniFrm.MakeLocalRef(value));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketGetOption'.
        /// </summary>
        public static int socketGetOption(object self, int opt, object iaContainerObj_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return InvokeFunc<global::java.net.PlainSocketImpl, global::java.net.InetAddressContainer, int>(self, iaContainerObj_, (impl, iaContainerObj) =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_TIMEOUT)
                        return socket.ReceiveTimeout;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_LINGER)
                        return socket.LingerState.Enabled ? socket.LingerState.LingerTime : -1;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_RCVBUF)
                        return socket.ReceiveBufferSize;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_SNDBUF)
                        return socket.SendBufferSize;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_BINDADDR)
                    {
                        iaContainerObj.addr = ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();
                        return 0;
                    }

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.TCP_NODELAY)
                        return socket.NoDelay ? 1 : -1;

                    // lookup option options
                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    // configure socket
                    return socket.GetSocketOption(options.Level, options.Name) switch
                    {
                        bool b => GetSocketOptionGetValue(options.Type, b),
                        int i => GetSocketOptionGetValue(options.Type, i),
                        _ => throw new global::java.net.SocketException("Invalid option value."),
                    };
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketGetOption ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketGetOption>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketGetOption), "(I)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__socketGetOption(jniEnv, jniFrm.MakeLocalRef(self), opt, jniFrm.MakeLocalRef(iaContainerObj_));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Handles socket options stored as a boolean.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionSetValue(SocketOptionUtil.SocketOptionType type, bool value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value ? 1 : 0,
            SocketOptionUtil.SocketOptionType.Integer => value ? 1 : 0,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as an integer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionSetValue(SocketOptionUtil.SocketOptionType type, int value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => (value != -1) ? 1 : 0,
            SocketOptionUtil.SocketOptionType.Integer => value,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as a boolean.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, bool value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value ? 1 : -1,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as an integer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, int value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value == 0 ? -1 : 1,
            SocketOptionUtil.SocketOptionType.Integer => value,
            SocketOptionUtil.SocketOptionType.Unknown => value,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

#endif

        /// <summary>
        /// Implements the native method for 'socketSendUrgentData'.
        /// </summary>
        public static void socketSendUrgentData(object this_, int data)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
                {
                    var socket = FileDescriptorAccessor.GetSocket(impl.fd);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var prevBlocking = socket.Blocking;
                    var prevTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = true;
                        socket.SendTimeout = impl.timeout;

                        var buffer = new byte[] { (byte)data };
                        socket.Send(buffer, 0, 1, SocketFlags.OutOfBand);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        throw new global::java.io.InterruptedIOException(e.Message);
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.SendTimeout = prevTimeout;
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.PlainSocketImpl).TypeHandle);
                __jniPtr__socketSendUrgentData ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketSendUrgentData>(JNIFrame.GetFuncPtr(__callerID, "java/net/PlainSocketImpl", nameof(socketSendUrgentData), "(B)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketSendUrgentData(jniEnv, jniFrm.MakeLocalRef(this_), data);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

    }

}
