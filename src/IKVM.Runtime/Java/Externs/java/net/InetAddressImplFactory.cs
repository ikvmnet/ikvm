using System.Net.Sockets;

namespace IKVM.Java.Externs.java.net
{

    static class InetAddressImplFactory
    {

        public static bool isIPv6Supported()
        {
            return Socket.OSSupportsIPv6;
        }

    }

}
