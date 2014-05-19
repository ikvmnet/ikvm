/*
  Copyright (C) 2007-2011 Jeroen Frijters
  Copyright (C) 2011 Trevor Bell (Siemens Energy, Inc.)

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

static class Java_sun_security_krb5_Config
{
	public static string getWindowsDirectory(bool isSystem)
	{
		if (isSystem)
		{
			return Environment.SystemDirectory;
		}
		return Environment.GetEnvironmentVariable("SystemRoot");
	}
}

static class Java_sun_security_krb5_Credentials
{
#if !FIRST_PASS
	private static java.util.Date ToJavaDate(long time)
	{
		// convert 100-nanosecond intervals to milliseconds offset milliseconds from Jan 1, 1601 to Jan 1, 1970
		const long DIFF_IN_MILLIS = 11644473600000L;
		return new java.util.Date((time / 10000) - DIFF_IN_MILLIS);
	}
#endif

	public static object acquireDefaultNativeCreds(int[] eTypes)
	{
#if FIRST_PASS
        return null;
#else
		Ticket ticket;
        try
        {
            ticket = Win32KerberosSupport.GetTicket();
        }
        catch (Win32Exception)
        {
            // there's no way to return more specific error information
            return null;
        }
        // we use reflection to instantiate the Credentials object,
		// because we don't want a static dependency on IKVM.OpenJDK.Security.dll
        return java.lang.Class.forName("sun.security.krb5.Credentials")
            .getConstructor(typeof(byte[]),
                            typeof(string),
                            typeof(string),
                            typeof(byte[]),
                            typeof(int),
                            typeof(bool[]),
                            typeof(java.util.Date),
                            typeof(java.util.Date),
                            typeof(java.util.Date),
                            typeof(java.util.Date),
                            typeof(java.net.InetAddress[]))
            .newInstance(ticket.EncodedTicket,
                            ticket.ClientNames[0],
                            ticket.TargetNames[0],
                            ticket.SessionKey,
                            java.lang.Integer.valueOf(ticket.SessionKeyType),
                            null,
                            ToJavaDate(ticket.StartTime),
                            ToJavaDate(ticket.StartTime),
                            ToJavaDate(ticket.EndTime),
                            ToJavaDate(ticket.RenewUntil),
                            null);
#endif
	}

    sealed class Ticket
    {
        public byte[] EncodedTicket;
        public string[] ClientNames;
        public string[] TargetNames;
        public byte[] SessionKey;
        public int SessionKeyType;
        public long StartTime;
        public long EndTime;
        public long RenewUntil;
    }

	static class Win32KerberosSupport
	{
        const int STATUS_SUCCESS = 0;

        [SecurityCritical]
        sealed class LsaSafeHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
        {
            internal LsaSafeHandle()
                : base(true)
            {
            }

            [SecurityCritical]
            override protected bool ReleaseHandle()
            {
                return LsaDeregisterLogonProcess(handle) == STATUS_SUCCESS;
            }
        }

        enum KERB_PROTOCOL_MESSAGE_TYPE
        {
            KerbRetrieveTicketMessage = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LsaString
        {
            public ushort Length;
            public ushort MaximumLength;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KERB_QUERY_TKT_CACHE_REQUEST
        {
            public KERB_PROTOCOL_MESSAGE_TYPE MessageType;
            public LUID LoginId;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KERB_CRYPTO_KEY
        {
            public int KeyType;
            public int Length;
            public IntPtr Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        sealed class KERB_RETRIEVE_TKT_RESPONSE
        {
            public KERB_EXTERNAL_TICKET Ticket;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KERB_EXTERNAL_TICKET
        {
            public IntPtr ServiceName;
            public IntPtr TargetName;
            public IntPtr ClientName;
            public UNICODE_STRING DomainName;
            public UNICODE_STRING TargetDomainName;
            public UNICODE_STRING AltTargetDomainName;
            public KERB_CRYPTO_KEY SessionKey;
            public uint TicketFlags;
            public uint Flags;
            public long KeyExpirationTime;
            public long StartTime;
            public long EndTime;
            public long RenewUntil;
            public long TimeSkew;
            public int EncodedTicketSize;
            public IntPtr EncodedTicket;
        }

        [DllImport("secur32.dll")]
        static extern int LsaDeregisterLogonProcess(IntPtr handle);

        [DllImport("secur32.dll")]
        static extern int LsaConnectUntrusted(out LsaSafeHandle LsaHandle);

		[DllImport("secur32.dll")]
		static extern int LsaCallAuthenticationPackage(
                LsaSafeHandle LsaHandle,
				uint AuthenticationPackage,
				ref KERB_QUERY_TKT_CACHE_REQUEST ProtocolSubmitBuffer,
				int SubmitBufferLength,
				out IntPtr ProtocolReturnBuffer,
				out int ReturnBufferLength,
				out int ProtocolStatus);

		[DllImport("secur32.dll")]
		static extern int LsaLookupAuthenticationPackage(
                LsaSafeHandle LsaHandle,
				ref LsaString PackageName,
				out uint AuthenticationPackage);

		[DllImport("advapi32.dll")]
		static extern int LsaNtStatusToWinError(int status);

        [DllImport("secur32.dll")]
        static extern int LsaFreeReturnBuffer(IntPtr buffer);

        static void Check(int ntstatus)
        {
            if (ntstatus != STATUS_SUCCESS)
            {
                throw new Win32Exception(LsaNtStatusToWinError(ntstatus));
            }
        }

		[SecuritySafeCritical]
		internal static Ticket GetTicket()
		{
            LsaSafeHandle lsaHandle = null;
            try
            {
                // connect to the LSA outside the TCB
                Check(LsaConnectUntrusted(out lsaHandle));

                string kerberos = "Kerberos";
                LsaString lsaString = new LsaString();
                lsaString.Length = (ushort)kerberos.Length;
                lsaString.MaximumLength = (ushort)kerberos.Length;
                lsaString.Buffer = kerberos;

                uint authenticationPackage = 0;

                // lookup the index for the Kerberos authentication package
                Check(LsaLookupAuthenticationPackage(lsaHandle, ref lsaString, out authenticationPackage));

                KERB_QUERY_TKT_CACHE_REQUEST request = new KERB_QUERY_TKT_CACHE_REQUEST();
                request.MessageType = KERB_PROTOCOL_MESSAGE_TYPE.KerbRetrieveTicketMessage;
                request.LoginId.LowPart = 0;
                request.LoginId.HighPart = 0;

                int submitBufferLength = Marshal.SizeOf(typeof(KERB_QUERY_TKT_CACHE_REQUEST));
                IntPtr responsePointer = IntPtr.Zero;
                int returnBufferLength = 0;
                int protocolStatus = 0;

                try
                {
                    // send the request to Kerberos and get a response
                    Check(LsaCallAuthenticationPackage(lsaHandle,
                            authenticationPackage,
                            ref request,
                            submitBufferLength,
                            out responsePointer,
                            out returnBufferLength,
                            out protocolStatus));

                    Check(protocolStatus);

                    if (responsePointer == IntPtr.Zero || returnBufferLength < Marshal.SizeOf(typeof(KERB_RETRIEVE_TKT_RESPONSE)))
                    {
                        throw new InvalidOperationException();
                    }

                    KERB_RETRIEVE_TKT_RESPONSE response = new KERB_RETRIEVE_TKT_RESPONSE();
                    Marshal.PtrToStructure(responsePointer, response);

                    Ticket ticket = new Ticket();
                    ticket.EncodedTicket = ReadBytes(response.Ticket.EncodedTicket, response.Ticket.EncodedTicketSize);
                    ticket.ClientNames = ReadExternalName(response.Ticket.ClientName);
                    ticket.TargetNames = ReadExternalName(response.Ticket.TargetName);
                    ticket.SessionKey = ReadBytes(response.Ticket.SessionKey.Value, response.Ticket.SessionKey.Length);
                    ticket.SessionKeyType = response.Ticket.SessionKey.KeyType;
                    ticket.StartTime = response.Ticket.StartTime;
                    ticket.EndTime = response.Ticket.EndTime;
                    ticket.RenewUntil = response.Ticket.RenewUntil;
                    return ticket;
                }
                finally
                {
                    if (responsePointer != IntPtr.Zero)
                    {
                        Check(LsaFreeReturnBuffer(responsePointer));
                    }
                }
            }
            finally
            {
                if (lsaHandle != null)
                {
                    lsaHandle.Close();
                }
            }
		}

        [SecurityCritical]
        private static string[] ReadExternalName(IntPtr ptr)
        {
            int nameCount = (ushort)Marshal.ReadInt16(ptr, 2);
            ptr = (IntPtr)((long)ptr + 4 + IntPtr.Size - 4);
            string[] names = new string[nameCount];
            for (int i = 0; i < nameCount; i++)
            {
                names[i] = ReadUnicodeString(ref ptr);
            }
            return names;
        }

        [SecurityCritical]
        private static string ReadUnicodeString(ref IntPtr ptr)
        {
            UNICODE_STRING str = (UNICODE_STRING)Marshal.PtrToStructure(ptr, typeof(UNICODE_STRING));
            ptr = (IntPtr)((long)ptr + Marshal.SizeOf(typeof(UNICODE_STRING)));
            return Marshal.PtrToStringUni(str.Buffer, str.Length / 2);
        }

        [SecurityCritical]
        private static byte[] ReadBytes(IntPtr ptr, int length)
        {
            byte[] buf = new byte[length];
            Marshal.Copy(ptr, buf, 0, length);
            return buf;
        }
    }
}

static class Java_sun_security_krb5_SCDynamicStoreConfig
{
	public static void installNotificationCallback()
	{
	}

	public static object getKerberosConfig()
	{
		return null;
	}
}
