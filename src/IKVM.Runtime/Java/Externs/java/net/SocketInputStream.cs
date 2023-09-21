using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
    {

        static global::ikvm.@internal.CallerID __callerID;
        delegate int __jniDelegate__socketRead0(IntPtr jniEnv, IntPtr self, IntPtr fdObj, IntPtr data, int off, int len, int timeout);
        static __jniDelegate__socketRead0 __jniPtr__socketRead0;

        public static int socketRead0(object this_, global::java.io.FileDescriptor fdObj, byte[] data, int off, int len, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return InvokeFunc<global::java.net.SocketInputStream, int>(this_, self =>
                {
                    return InvokeFuncWithSocket(fdObj, socket =>
                    {
                        if (timeout > 0 && socket.Available == 0)
                        {
                            try
                            {
                                if (socket.Poll((int)Math.Min(timeout * 1000L, int.MaxValue), SelectMode.SelectRead) == false)
                                    throw new global::java.net.SocketTimeoutException("Receive timed out.");
                            }
                            catch (SocketException e) when (e.SocketErrorCode == SocketError.TimedOut)
                            {
                                throw new global::java.net.SocketTimeoutException("Receive timed out.");
                            }
                            catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                            {
                                throw new global::java.net.SocketException("Socket closed.");
                            }
                            catch
                            {
                                throw;
                            }
                        }

                        try
                        {
                            return socket.EndReceive(socket.BeginReceive(data, off, len, SocketFlags.None, null, null));
                        }
                        catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                        {
                            throw new global::java.net.SocketException("Socket closed.");
                        }
                        catch (SocketException)
                        {
                            throw;
                        }
                    });
                });
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.SocketInputStream).TypeHandle);
                __jniPtr__socketRead0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketRead0>(JNIFrame.GetFuncPtr(__callerID, "java/net/SocketInputStream", nameof(socketRead0), "(Ljava/io/FileDescriptor;[BIII)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    var thisRef = jniFrm.MakeLocalRef(this_);
                    var fdObjRef = jniFrm.MakeLocalRef(fdObj);
                    var dataRef = jniFrm.MakeLocalRef(data);
                    return __jniPtr__socketRead0(jniEnv, thisRef, fdObjRef, dataRef, off, len, timeout);
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
        /// Invokes the given function with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fd"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static TResult InvokeFuncWithSocket<TResult>(global::java.io.FileDescriptor fd, Func<Socket, TResult> func)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            return func(socket);
        }

#endif

    }

}
