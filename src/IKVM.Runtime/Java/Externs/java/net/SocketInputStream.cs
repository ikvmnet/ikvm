using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
    {

        public static int socketRead0(object this_, global::java.io.FileDescriptor fd, byte[] b, int off, int len, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.SocketInputStream, int>(this_, impl =>
            {
                return InvokeFuncWithSocket(fd, socket =>
                {
                    if (timeout > 0 && socket.Available == 0)
                    {
                        // Windows Poll method reports errors as readable, however, Linux reports it as errored, so
                        // we can use Poll on Windows for both errors, but must use Select on Linux to trap both
                        // read and error states.
                        //
                        // OS X select hangs, so we use Poll there too.
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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
                        else
                        {
                            try
                            {
                                var rl = new List<Socket>() { socket };
                                var el = new List<Socket>() { socket };
                                Socket.Select(rl, null, el, (int)Math.Min(timeout * 1000L, int.MaxValue));
                                if (rl.Count == 0 && el.Count == 0)
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
                    }

                    try
                    {
                        return socket.EndReceive(socket.BeginReceive(b, off, len, SocketFlags.None, null, null));
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
