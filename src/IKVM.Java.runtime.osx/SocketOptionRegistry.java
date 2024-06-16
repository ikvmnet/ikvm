package sun.nio.ch;
import java.net.SocketOption;
import java.net.StandardSocketOptions;
import java.net.ProtocolFamily;
import java.net.StandardProtocolFamily;
import java.util.Map;
import java.util.HashMap;
class SocketOptionRegistry {

    private SocketOptionRegistry() { }

    private static class RegistryKey {
        private final SocketOption<?> name;
        private final ProtocolFamily family;
        RegistryKey(SocketOption<?> name, ProtocolFamily family) {
            this.name = name;
            this.family = family;
        }
        public int hashCode() {
            return name.hashCode() + family.hashCode();
        }
        public boolean equals(Object ob) {
            if (ob == null) return false;
            if (!(ob instanceof RegistryKey)) return false;
            RegistryKey other = (RegistryKey)ob;
            if (this.name != other.name) return false;
            if (this.family != other.family) return false;
            return true;
        }
    }

    private static class LazyInitialization {

        static final Map<RegistryKey,OptionKey> options = options();

        private static Map<RegistryKey,OptionKey> options() {
            Map<RegistryKey,OptionKey> map =
                new HashMap<RegistryKey,OptionKey>();
            map.put(new RegistryKey(StandardSocketOptions.SO_BROADCAST,
                Net.UNSPEC), new OptionKey(0xffff, 0x0020));
            map.put(new RegistryKey(StandardSocketOptions.SO_KEEPALIVE,
                Net.UNSPEC), new OptionKey(0xffff, 0x0008));
            map.put(new RegistryKey(StandardSocketOptions.SO_LINGER,
                Net.UNSPEC), new OptionKey(0xffff, 0x0080));
            map.put(new RegistryKey(StandardSocketOptions.SO_SNDBUF,
                Net.UNSPEC), new OptionKey(0xffff, 0x1001));
            map.put(new RegistryKey(StandardSocketOptions.SO_RCVBUF,
                Net.UNSPEC), new OptionKey(0xffff, 0x1002));
            map.put(new RegistryKey(StandardSocketOptions.SO_REUSEADDR,
                Net.UNSPEC), new OptionKey(0xffff, 0x0004));

            map.put(new RegistryKey(StandardSocketOptions.TCP_NODELAY,
                Net.UNSPEC), new OptionKey(6, 0x01));


            map.put(new RegistryKey(StandardSocketOptions.IP_TOS,
                StandardProtocolFamily.INET), new OptionKey(0, 3));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_IF,
                StandardProtocolFamily.INET), new OptionKey(0, 9));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_TTL,
                StandardProtocolFamily.INET), new OptionKey(0, 10));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_LOOP,
                StandardProtocolFamily.INET), new OptionKey(0, 11));



            map.put(new RegistryKey(StandardSocketOptions.IP_TOS,
                StandardProtocolFamily.INET6), new OptionKey(41, 36));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_IF,
                StandardProtocolFamily.INET6), new OptionKey(41, 9));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_TTL,
                StandardProtocolFamily.INET6), new OptionKey(41, 10));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_LOOP,
                StandardProtocolFamily.INET6), new OptionKey(41, 11));


            map.put(new RegistryKey(ExtendedSocketOption.SO_OOBINLINE,
                Net.UNSPEC), new OptionKey(0xffff, 0x0100));
            return map;
        }
    }

    public static OptionKey findOption(SocketOption<?> name, ProtocolFamily family) {
        RegistryKey key = new RegistryKey(name, family);
        return LazyInitialization.options.get(key);
    }
}
