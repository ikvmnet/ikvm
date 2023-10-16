using System.IO;

using IKVM.Runtime;

namespace IKVM.Java.Externs.sun.net
{

    /// <summary>
    /// Implements the native methods for 'sun.net.PortConfig'.
    /// </summary>
    static class PortConfig
    {

        const int WindowsDefaultLowerPortRange = 49152;
        const int WindowsDefaultUpperPortRange = 65535;
        const string LinuxIpLocalPortRangeFile = "/proc/sys/net/ipv4/ip_local_port_range";
        const int LinuxDefaultLowerPortRange = 32768;
        const int LinuxDefaultUpperPortRange = 60999;
        const int OSXDefaultLowerPortRange = 49152;
        const int OSXDefaultUpperPortRange = 65535;

        /// <summary>
        /// Implements the native method 'getLower0'.
        /// </summary>
        /// <returns></returns>
        public static int getLower0()
        {
            if (RuntimeUtil.IsWindows)
            {
                return WindowsDefaultLowerPortRange;
            }
            else if (RuntimeUtil.IsLinux)
            {
                if (File.Exists(LinuxIpLocalPortRangeFile) == false)
                    return LinuxDefaultLowerPortRange;

                var f = File.ReadAllText(LinuxIpLocalPortRangeFile);
                var a = f.Split('\t');
                if (a.Length < 2)
                    return LinuxDefaultLowerPortRange;

                if (int.TryParse(a[0], out var i))
                    return i;

                return LinuxDefaultLowerPortRange;
            }
            else if (RuntimeUtil.IsOSX)
            {
                return OSXDefaultLowerPortRange;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Implements the native method 'getUpper0'.
        /// </summary>
        /// <returns></returns>
        public static int getUpper0()
        {
            if (RuntimeUtil.IsWindows)
            {
                return WindowsDefaultUpperPortRange;
            }
            else if (RuntimeUtil.IsLinux)
            {
                if (File.Exists(LinuxIpLocalPortRangeFile) == false)
                    return LinuxDefaultUpperPortRange;

                var f = File.ReadAllText(LinuxIpLocalPortRangeFile);
                var a = f.Split('\t');
                if (a.Length < 2)
                    return LinuxDefaultUpperPortRange;

                if (int.TryParse(a[1], out var i))
                    return i;

                return LinuxDefaultUpperPortRange;
            }
            else if (RuntimeUtil.IsOSX)
            {
                return OSXDefaultUpperPortRange;
            }
            else
            {
                return -1;
            }
        }

    }

}