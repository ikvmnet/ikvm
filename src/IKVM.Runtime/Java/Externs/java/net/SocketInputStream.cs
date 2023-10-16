using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
    {

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate int __jniDelegate__socketRead0(IntPtr jniEnv, IntPtr self, IntPtr fdObj, IntPtr data, int off, int len, int timeout);
        static __jniDelegate__socketRead0 __jniPtr__socketRead0;

#endif

        public static int socketRead0(object this_, object fdObj, byte[] data, int off, int len, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return InvokeFunc<global::java.net.SocketInputStream, int>(this_, self =>
                {
                    if (fdObj == null)
                        throw new global::java.net.SocketException("Socket closed");

                    var socket = FileDescriptorAccessor.GetSocket(fdObj);
                    if (socket == null)
                        throw new global::java.net.SocketException("Socket closed");

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
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.net.SocketInputStream).TypeHandle);
                __jniPtr__socketRead0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__socketRead0>(JNIFrame.GetFuncPtr(__callerID, "java/net/SocketInputStream", nameof(socketRead0), "(Ljava/io/FileDescriptor;[BIII)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__socketRead0(jniEnv, jniFrm.MakeLocalRef(this_), jniFrm.MakeLocalRef(fdObj), jniFrm.MakeLocalRef(data), off, len, timeout);
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
