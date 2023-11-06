using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    internal static class FileDescriptorAccessorExtensions
    {

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
        /// Configures the FileDescriptor with the given .NET stream.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fdo"></param>
        /// <param name="stream"></param>
        public static void SetStream(this FileDescriptorAccessor self, object fdo, Stream stream)
        {
            lock (fdo)
            {
                self.SetPtr(fdo, stream is FileStream fs ? LibIkvm.Instance.io_duplicate_file((long)fs.SafeFileHandle.DangerousGetHandle()) : -1);
                self.SetObj(fdo, stream);
            }
        }

        /// <summary>
        /// Gets or creates a .NET stream for the FileDescriptor.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fdo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Stream GetStream(this FileDescriptorAccessor self, object fdo)
        {
            lock (fdo)
            {
                // socket is already cached
                if (self.GetObj(fdo) is Stream s)
                    return s;

                var handle = self.GetPtr(fdo);
                if (handle == -1)
                    return null;

                if (LibIkvm.Instance.io_is_file(handle))
                {
                    self.SetObj(fdo, s = new FileStream(new SafeFileHandle((IntPtr)handle, false), FileAccess.ReadWrite, 1, false));
                    return s;
                }

                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Configures the FileDescriptor with the given .NET socket.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fdo"></param>
        /// <param name="socket"></param>
        public static void SetSocket(this FileDescriptorAccessor self, object fdo, Socket socket)
        {
            lock (fdo)
            {
                self.SetPtr(fdo, LibIkvm.Instance.io_duplicate_socket((long)socket.Handle));
                self.SetObj(fdo, socket);
            }
        }

        /// <summary>
        /// Gets or creates a .NET socket for the FileDescriptor.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fdo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static unsafe Socket GetSocket(this FileDescriptorAccessor self, object fdo)
        {
            lock (fdo)
            {
                // socket is already cached
                if (self.GetObj(fdo) is Socket s)
                    return s;

                var handle = self.GetPtr(fdo);
                if (handle == -1)
                    return null;

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

                        self.SetObj(fdo, s = new Socket(new SocketInformation() { ProtocolInformation = protocolInfo }));
                        return s;
                    }
                    else
                    {
                        throw new PlatformNotSupportedException();
                    }
#else
                    self.SetObj(fdo, s = new Socket(new SafeSocketHandle((IntPtr)LibIkvm.Instance.io_duplicate_socket(handle), true)));
                    return s;
#endif
                }

                throw new InvalidOperationException();
            }
        }

    }

#endif

}
