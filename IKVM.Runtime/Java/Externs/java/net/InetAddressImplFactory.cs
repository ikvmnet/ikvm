/*
  Copyright (C) 2007-2015 Jeroen Frijters

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
using System.Net.Sockets;

namespace IKVM.Java.Externs.java.net
{

    static class InetAddressImplFactory
    {
        private static readonly bool ipv6supported = Init();

        private static bool Init()
        {
            string env = IKVM.Internal.JVM.SafeGetEnvironmentVariable("IKVM_IPV6");
            int val;
            if (env != null && Int32.TryParse(env, out val))
            {
                return (val & 1) != 0;
            }
            // On Linux we can't bind both an IPv4 and IPv6 to the same port, so we have to disable IPv6 until we have a dual-stack implementation.
            // Mono on Windows doesn't appear to support IPv6 either (Mono on Linux does).
            return Type.GetType("Mono.Runtime") == null
                && Environment.OSVersion.Platform == PlatformID.Win32NT
                && Socket.OSSupportsIPv6;
        }

        public static bool isIPv6Supported()
        {
            return ipv6supported;
        }

    }

}
