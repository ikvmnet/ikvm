using System.Net.Sockets;

namespace IKVM.Java.Externs.java.net
{

    /// <summary>
    /// Provides a map of Java socket information to .NET socket information.
    /// </summary>
    static class SocketOptionUtil
    {

#if !FIRST_PASS

        public enum SocketOptionType
        {

            Boolean,
            Integer,
            Unknown,

        }

        /// <summary>
        /// Describes a mapping between a <see cref="global::java.net.SocketOptions"/> and it's associated .NET information.
        /// </summary>
        struct SocketOptionMapItem
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="option"></param>
            /// <param name="level"></param>
            /// <param name="name"></param>
            /// <param name="type"></param>
            public SocketOptionMapItem(int option, SocketOptionLevel level, SocketOptionName name, SocketOptionType type)
            {
                Option = option;
                Level = level;
                Name = name;
                Type = type;
            }

            /// <summary>
            /// Gets the option.
            /// </summary>
            public int Option { get; set; }

            /// <summary>
            /// Gets the <see cref="SocketOptionLevel"/>.
            /// </summary>
            public SocketOptionLevel Level { get; set; }

            /// <summary>
            /// Gets the <see cref="SocketOptionName"/>.
            /// </summary>
            public SocketOptionName Name { get; set; }

            /// <summary>
            /// Gets the type of the socket option when getting.
            /// </summary>
            public SocketOptionType Type { get; set; }

        }

        readonly static SocketOptionMapItem[] map = new[]
        {
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_OOBINLINE,        SocketOptionLevel.Socket,   SocketOptionName.OutOfBandInline,       SocketOptionType.Boolean),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_SNDBUF,           SocketOptionLevel.Socket,   SocketOptionName.SendBuffer,            SocketOptionType.Integer),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_RCVBUF,           SocketOptionLevel.Socket,   SocketOptionName.ReceiveBuffer,         SocketOptionType.Integer),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_KEEPALIVE,        SocketOptionLevel.Socket,   SocketOptionName.KeepAlive,             SocketOptionType.Boolean),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_REUSEADDR,        SocketOptionLevel.Socket,   SocketOptionName.ReuseAddress,          SocketOptionType.Boolean),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_BROADCAST,        SocketOptionLevel.Socket,   SocketOptionName.Broadcast,             SocketOptionType.Boolean),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_TIMEOUT,          SocketOptionLevel.Socket,   SocketOptionName.ReceiveTimeout,        SocketOptionType.Integer),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_MULTICAST_IF,     SocketOptionLevel.IP,       SocketOptionName.MulticastInterface,    SocketOptionType.Integer),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_MULTICAST_LOOP,   SocketOptionLevel.IP,       SocketOptionName.MulticastLoopback,     SocketOptionType.Boolean),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_TOS,              SocketOptionLevel.IP,       SocketOptionName.TypeOfService,         SocketOptionType.Integer),
            new SocketOptionMapItem(global::java.net.SocketOptions.TCP_NODELAY,         SocketOptionLevel.Tcp,      SocketOptionName.NoDelay,               SocketOptionType.Boolean),
        };

        /// <summary>
        /// Gets the socket option map item from the given option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool TryGetDotNetSocketOption(int option, out (SocketOptionLevel Level, SocketOptionName Name, SocketOptionType Type) options)
        {
            foreach (var o in map)
            {
                if (o.Option == option)
                {
                    options = (o.Level, o.Name, o.Type);
                    return true;
                }
            }

            options = default;
            return false;
        }

#endif

    }

}
