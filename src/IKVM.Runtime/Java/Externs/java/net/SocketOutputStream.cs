using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketOutputStream
    {

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__socketWrite0(IntPtr jniEnv, IntPtr self, IntPtr fdObj, IntPtr data, int off, int len);
        static __jniDelegate__socketWrite0 __jniPtr__socketWrite0;

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
        public static void socketWrite0(object this_, global::java.io.FileDescriptor fdObj, byte[] data, int off, int len)
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
                    InvokeActionWithSocket(fdObj, socket =>
                    {
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
                    var thisRef = jniFrm.MakeLocalRef(this_);
                    var fdObjRef = jniFrm.MakeLocalRef(fdObj);
                    var dataRef = jniFrm.MakeLocalRef(data);
                    __jniPtr__socketWrite0(jniEnv, thisRef, fdObjRef, dataRef, off, len);
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
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeActionWithSocket(global::java.io.FileDescriptor fd, Action<Socket> action)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            action(socket);
        }

#endif

    }

}
