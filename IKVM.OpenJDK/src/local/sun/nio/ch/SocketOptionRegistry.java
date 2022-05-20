/*
 * Copyright (c) 2008, 2012, Oracle and/or its affiliates. All rights reserved.
 *
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 *
 */
// AUTOMATICALLY GENERATED FILE - DO NOT EDIT                                  
package sun.nio.ch;                                                            
import java.net.SocketOption;                                                  
import java.net.StandardSocketOptions;                                         
import java.net.ProtocolFamily;                                                
import java.net.StandardProtocolFamily;                                        
import java.util.Map;                                                          
import java.util.HashMap;                                                      
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketOptionName;
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
            map.put(new RegistryKey(StandardSocketOptions.IP_TOS, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, ikvm.internal.Winsock.IPV6_TCLASS));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_IF, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_TTL, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.IpTimeToLive));
            map.put(new RegistryKey(StandardSocketOptions.IP_MULTICAST_LOOP, StandardProtocolFamily.INET6), new OptionKey(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback));
            map.put(new RegistryKey(ExtendedSocketOption.SO_OOBINLINE, Net.UNSPEC), new OptionKey(SocketOptionLevel.Socket, SocketOptionName.OutOfBandInline));
            return map;                                                        
        }                                                                      
    }                                                                          
    public static OptionKey findOption(SocketOption<?> name, ProtocolFamily family) { 
        RegistryKey key = new RegistryKey(name, family);                       
        return LazyInitialization.options.get(key);                            
    }                                                                          
}                                                                              
