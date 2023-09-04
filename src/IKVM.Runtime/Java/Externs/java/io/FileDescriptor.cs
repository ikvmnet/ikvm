using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;

using Microsoft.Win32.SafeHandles;

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
        /// Contains the native methods for Windows.
        /// </summary>
        static class NativeWindows
        {

            /// <summary>
            /// Invokes the Win32 method 'GetStdHandle'.
            /// </summary>
            /// <param name="nStdHandle"></param>
            /// <returns></returns>
            [DllImport("kernel32")]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            /// <summary>
            /// Invokes the Win32 method 'WSADuplicateSocket'.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="dwProcessId"></param>
            /// <param name="lpProtocolInfo"></param>
            /// <returns></returns>
            [DllImport("ws2_32", SetLastError = true)]
            public unsafe static extern int WSADuplicateSocket(IntPtr s, int dwProcessId, WSAPROTOCOL_INFOW* lpProtocolInfo);

            /// <summary>
            /// Invokes the Win32 method 'closesocket'.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            [DllImport("ws2_32", SetLastError = true)]
            public static extern int closesocket(IntPtr s);

            /// <summary>
            /// Invokes teh Win32 method 'WSAGetLastError'.
            /// </summary>
            [DllImport("ws2_32", SetLastError = true)]
            public static extern int WSAGetLastError();

            public const int SO_PROTOCOL_INFOW = 0x2005;

            /// <summary>
            /// Implementation of the Win32 'WSAPROTOCOL_INFOW' structure.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public unsafe struct WSAPROTOCOL_INFOW
            {

                public const int WSAPROTOCOL_LEN = 255;

                public uint dwServiceFlags1;
                public uint dwServiceFlags2;
                public uint dwServiceFlags3;
                public uint dwServiceFlags4;
                public uint dwProviderFlags;
                public Guid ProviderId;
                public uint dwCatalogEntryId;
                public WSAPROTOCOLCHAIN ProtocolChain;
                public int iVersion;
                public AddressFamily iAddressFamily;
                public int iMaxSockAddr;
                public int iMinSockAddr;
                public SocketType iSocketType;
                public ProtocolType iProtocol;
                public int iProtocolMaxOffset;
                public int iNetworkByteOrder;
                public int iSecurityScheme;
                public uint dwMessageSize;
                public uint dwProviderReserved;
                public fixed char szProtocol[WSAPROTOCOL_LEN + 1];

            }

            /// <summary>
            /// Implementation of the Win32 'WSAPROTOCOLCHAIN' structure.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public unsafe struct WSAPROTOCOLCHAIN
            {

                internal const int MAX_PROTOCOL_CHAIN = 7;

                internal int ChainLen;
                internal fixed uint ChainEntries[MAX_PROTOCOL_CHAIN];

            }

        }

        /// <summary>
        /// Gets the file descriptor.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static long GetHandleOrFileDescriptor(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
            {
                fs.Flush();
                return fs.SafeFileHandle.DangerousGetHandle().ToInt64();
            }

            if (FileDescriptorAccessor.GetSocket(self) is Socket ss)
            {
                return ss.Handle.ToInt64();
            }

            if (self == FileDescriptorAccessor.GetIn())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? NativeWindows.GetStdHandle(-10).ToInt64() : 0;
            if (self == FileDescriptorAccessor.GetOut())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? NativeWindows.GetStdHandle(-11).ToInt64() : 1;
            if (self == FileDescriptorAccessor.GetErr())
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? NativeWindows.GetStdHandle(-12).ToInt64() : 2;

            return -1;
#endif
        }

        /// <summary>
        /// Sets the file descriptor.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handle"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static unsafe void SetHandleOrFileDescriptor(object self, long handle)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (LibIkvm.Instance.io_is_file(handle))
            {
                FileDescriptorAccessor.SetStream(self, new FileStream(new SafeFileHandle((IntPtr)handle, true), FileAccess.ReadWrite));
                return;
            }

            if (LibIkvm.Instance.io_is_socket(handle))
            {
#if NETFRAMEWORK
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var protocolInfo = new byte[sizeof(NativeWindows.WSAPROTOCOL_INFOW)];
                    fixed (byte* protocolInfoPtr = protocolInfo)
                    {
                        var result = NativeWindows.WSADuplicateSocket((IntPtr)handle, Process.GetCurrentProcess().Id, (NativeWindows.WSAPROTOCOL_INFOW*)protocolInfoPtr);
                        if (result != 0)
                            throw new Win32Exception(NativeWindows.WSAGetLastError());
                    }

                    FileDescriptorAccessor.SetSocket(self, new Socket(new SocketInformation() { ProtocolInformation = protocolInfo }));
                    NativeWindows.closesocket((IntPtr)handle);
                    return;
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
#else
                FileDescriptorAccessor.SetSocket(self, new Socket(new SafeSocketHandle((IntPtr)handle, true)));
                return;
#endif
            }

            throw new InvalidOperationException();
#endif
        }

        /// <summary>
        /// Implements the native method 'getFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int getFd(object self)
        {
            return (int)GetHandleOrFileDescriptor(self);
        }

        /// <summary>
        /// Implements the native method 'setFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static unsafe void setFd(object self, int fd)
        {
            SetHandleOrFileDescriptor(self, fd);
        }

        /// <summary>
        /// Implements the native method 'getHandle0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getHandle(object self)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return GetHandleOrFileDescriptor(self);
            else
                return -1;
        }

        /// <summary>
        /// Implements the native method 'setHandle'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handle"></param>
        public static void setHandle(object self, long handle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SetHandleOrFileDescriptor(self, handle);
            else
                throw new PlatformNotSupportedException();
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
