using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'FileDescriptor'.
    /// </summary>
    static class FileDescriptor
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        /// <summary>
        /// Invokes the Win32 method 'GetStdHandle'.
        /// </summary>
        /// <param name="nStdHandle"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// Implements the native method 'getFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int getFd(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
                return fs.SafeFileHandle.DangerousGetHandle().ToInt32();
            if (FileDescriptorAccessor.GetSocket(self) is Socket ss)
                return ss.Handle.ToInt32();
            if (self == FileDescriptorAccessor.GetIn())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetStdHandle(-10).ToInt32() : 0;
            if (self == FileDescriptorAccessor.GetOut())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetStdHandle(-11).ToInt32() : 1;
            if (self == FileDescriptorAccessor.GetErr())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetStdHandle(-12).ToInt32() : 2;

            return -1;
#endif
        }

        /// <summary>
        /// Invokes the Win32 method 'WSADuplicateSocket'.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dwProcessId"></param>
        /// <param name="lpProtocolInfo"></param>
        /// <returns></returns>
        [DllImport("ws2_32", SetLastError = true)]
        unsafe static extern int WSADuplicateSocket(IntPtr s, int dwProcessId, WSAPROTOCOL_INFOW* lpProtocolInfo);

        /// <summary>
        /// Invokes the Win32 method 'closesocket'.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [DllImport("ws2_32", SetLastError = true)]
        static extern int closesocket(IntPtr s);

        /// <summary>
        /// Invokes teh Win32 method 'WSAGetLastError'.
        /// </summary>
        [DllImport("ws2_32", SetLastError = true)]
        static extern int WSAGetLastError();

        public const int SO_PROTOCOL_INFOW = 0x2005;

        /// <summary>
        /// Implementation of the Win32 'WSAPROTOCOL_INFOW' structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        unsafe struct WSAPROTOCOL_INFOW
        {

            internal const int WSAPROTOCOL_LEN = 255;

            internal uint dwServiceFlags1;
            internal uint dwServiceFlags2;
            internal uint dwServiceFlags3;
            internal uint dwServiceFlags4;
            internal uint dwProviderFlags;
            internal Guid ProviderId;
            internal uint dwCatalogEntryId;
            internal WSAPROTOCOLCHAIN ProtocolChain;
            internal int iVersion;
            internal AddressFamily iAddressFamily;
            internal int iMaxSockAddr;
            internal int iMinSockAddr;
            internal SocketType iSocketType;
            internal ProtocolType iProtocol;
            internal int iProtocolMaxOffset;
            internal int iNetworkByteOrder;
            internal int iSecurityScheme;
            internal uint dwMessageSize;
            internal uint dwProviderReserved;
            internal fixed char szProtocol[WSAPROTOCOL_LEN + 1];

        }

        /// <summary>
        /// Implementation of the Win32 'WSAPROTOCOLCHAIN' structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        unsafe struct WSAPROTOCOLCHAIN
        {

            internal const int MAX_PROTOCOL_CHAIN = 7;

            internal int ChainLen;
            internal fixed uint ChainEntries[MAX_PROTOCOL_CHAIN];

        }

        /// <summary>
        /// Implements the native method 'setFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static unsafe void setFd(object self, int fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var protocolInfo = new byte[sizeof(WSAPROTOCOL_INFOW)];
                fixed (byte* protocolInfoPtr = protocolInfo)
                {
                    var result = WSADuplicateSocket((IntPtr)fd, Process.GetCurrentProcess().Id, (WSAPROTOCOL_INFOW*)protocolInfoPtr);
                    if (result != 0)
                        throw new Win32Exception(WSAGetLastError());
                }

                FileDescriptorAccessor.SetSocket(self, new Socket(new SocketInformation() { ProtocolInformation = protocolInfo }));
                closesocket((IntPtr)fd);
                return;
            }

            throw new PlatformNotSupportedException();
#else
            FileDescriptorAccessor.SetSocket(self, new Socket(new SafeSocketHandle((IntPtr)fd, true)));
#endif
#endif
        }

        /// <summary>
        /// Implements the native method 'getHandle0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getHandle(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
            {
                if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
                {
                    return fs.SafeFileHandle.DangerousGetHandle().ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetIn())
                {
                    return GetStdHandle(-10).ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetOut())
                {
                    return GetStdHandle(-11).ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetErr())
                {
                    return GetStdHandle(-12).ToInt64();
                }
            }

            return -1;
#endif
        }

        /// <summary>
        /// Implements the native method 'setHandle'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handle"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void setHandle(object self, long handle)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native method 'standardStream'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static object standardStream(int fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return FileDescriptorAccessor.FromStream(fd switch
            {
                0 => System.Console.OpenStandardInput(),
                1 => System.Console.OpenStandardOutput(),
                2 => System.Console.OpenStandardError(),
                _ => throw new NotImplementedException(),
            });
#endif
        }

        /// <summary>
        /// Implements the native method 'sync'.
        /// </summary>
        /// <param name="self"></param>
        /// <exception cref="global::java.io.SyncFailedException"></exception>
        public static void sync(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stream = FileDescriptorAccessor.GetStream(self);
            if (stream == null)
                throw new global::java.io.SyncFailedException("Sync failed.");
            if (stream.CanWrite == false)
                return;

            try
            {
                stream.Flush();
            }
            catch (IOException e)
            {
                throw new global::java.io.SyncFailedException(e.Message);
            }
#endif
        }

    }

}
