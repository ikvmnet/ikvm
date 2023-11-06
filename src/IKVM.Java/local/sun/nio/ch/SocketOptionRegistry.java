package sun.nio.ch;

import java.net.SocketOption;
import java.net.StandardSocketOptions;
import java.net.ProtocolFamily;
import java.net.StandardProtocolFamily;
import java.util.Map;
import java.util.HashMap;

import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketOptionName;

class SocketOptionRegistry
{

    private SocketOptionRegistry()
    {
    
    }

    private static class RegistryKey
    {

        private final SocketOption<?> name;
        private final ProtocolFamily family;

        RegistryKey(SocketOption<?> name, ProtocolFamily family)
        {
            this.name = name;
            this.family = family;
        }             

        public int hashCode()
        {
            return name.hashCode() + family.hashCode();
        }

        public boolean equals(Object ob)
        {
            if (ob == null)
                return false;                                      
            if (!(ob instanceof RegistryKey))
            return false;

            RegistryKey other = (RegistryKey)ob;
            if (this.name != other.name)
                return false;
            if (this.family != other.family)
                return false;

            return true;
        }

    }                        

    private static class LazyInitialization
    {

        static final Map<RegistryKey, OptionKey> options = options();

        private static Map<RegistryKey, OptionKey> options()
        {
            Map<RegistryKey, OptionKey> map = new HashMap<RegistryKey, OptionKey>();
            map.put(new RegistryKey(StandardSocketOptions.SO_BROADCAST, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.Broadcast));
            map.put(new RegistryKey(StandardSocketOptions.SO_KEEPALIVE, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.KeepAlive));
            map.put(new RegistryKey(StandardSocketOptions.SO_LINGER, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.Linger));
            map.put(new RegistryKey(StandardSocketOptions.SO_SNDBUF, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.SendBuffer));
            map.put(new RegistryKey(StandardSocketOptions.SO_RCVBUF, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer));
            map.put(new RegistryKey(StandardSocketOptions.SO_REUSEADDR, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress));
            map.put(new RegistryKey(StandardSocketOptions.TCP_NODELAY, Net.UNSPEC), new OptionKey(SocketOptionLevel.Tcp, SocketOptionName.NoDelay));
            map.put(new RegistryKey(StandardSocketOptions.IP_TOS, StandardProtocolFamily.INET), new OptionKey(SocketOptionLevel.IP, SocketOptionName.TypeOfService));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_IF, StandardProtocolFamily.INET), new OptionKey(SocketOptionLevel.IP, SocketOptionName.MulticastInterface));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_TTL, StandardProtocolFamily.INET), new OptionKey(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_LOOP, StandardProtocolFamily.INET), new OptionKey(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback));
            map.put(new RegistryKey(StandardSocketOptions.IP_TOS, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, 39)); // TODO this whole file needs to be auto generated for OS
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_IF, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_TTL, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.IpTimeToLive));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_LOOP, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback));
            map.put(new RegistryKey(ExtendedSocketOption.SO_OOBINLINE, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.OutOfBandInline));
            return map;
        }
    }

    public static OptionKey findOption(SocketOption<?> name, ProtocolFamily family)
    {

        RegistryKey key = new RegistryKey(name, family);
        return LazyInitialization.options.get(key);

    }

}
