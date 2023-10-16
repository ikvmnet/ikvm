using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime;
using IKVM.Runtime.JNI;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketOutputStream
    {

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__socketWrite0(IntPtr jniEnv, IntPtr self, IntPtr fdObj, IntPtr data, int off, int len);
        static __jniDelegate__socketWrite0 __jniPtr__socketWrite0;

#endif

        /// <summary>
        /// Implements the native method for 'socketWrite0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="fdObj"></param>
        /// <param name="data"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        public static void socketWrite0(object this_, object fdObj, byte[] data, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (data == null)
                    throw new global::java.lang.NullPointerException("data argument.");

                InvokeAction<global::java.net.SocketOutputStream>(this_, impl =>
                {
                    if (fdObj == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var socket = FileDescriptorAccessor.GetSocket(fdObj);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var prevBlocking = socket.Blocking;
                    var prevSendTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = false;
                        socket.Blocking = true;
                        socket.SendTimeout = 0;
                        socket.Send(data, off, Math.Min(len, data.Length - off), SocketFlags.None);
                    }
                    catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        throw new global::java.net.SocketException("Socket closed.");
                    }
                    catch (SocketException)
                    {
                        throw;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.SendTimeout = prevSendTimeout;
                    }
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.SocketOutputStream).TypeHandle);  
                __jniPtr__socketWrite0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketWrite0>(JNIFrame.GetFuncPtr(__callerID, "java/net/SocketOutputStream", nameof(socketWrite0), "(Ljava/io/FileDescriptor;[BII)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__socketWrite0(jniEnv, jniFrm.MakeLocalRef(this_), jniFrm.MakeLocalRef(fdObj), jniFrm.MakeLocalRef(data), off, len);
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
