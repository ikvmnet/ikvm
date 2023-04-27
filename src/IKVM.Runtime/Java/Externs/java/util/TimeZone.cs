using System;
using System.IO;

namespace IKVM.Java.Externs.java.util
{

    /// <summary>
    /// Implements the native methods for 'java.util.TimeZone'.
    /// </summary>
    static class TimeZone
    {

        /// <summary>
        /// Gets the current local .NET time zone ID.
        /// </summary>
        /// <returns></returns>
        static string GetCurrentTimeZoneID()
        {
            return TimeZoneInfo.Local.Id;
        }

        /// <summary>
        /// Searches the tzmappings file for the specified time zone name.
        /// </summary>
        /// <param name="javaHome"></param>
        /// <param name="tzName"></param>
        /// <returns></returns>
        static string MatchJavaTZ(string javaHome, string tzName)
        {
            try
            {
                var mapFileName = Path.Combine(javaHome, "lib", "tzmappings");
                if (File.Exists(mapFileName) == false)
                    return null;

                using var mapFile = File.OpenText(mapFileName);
                string mapLine = null;
                while ((mapLine = mapFile.ReadLine()) != null)
                {
                    mapLine = mapLine.Trim();
                    if (mapLine.Length < 4)
                        continue;
                    if (mapLine[0] == '#')
                        continue;

                    var l = mapLine.Split(':');
                    if (l.Length < 4)
                        continue;

                    if (l[0] == tzName)
                        return l[3];
                }

                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>
        /// Implements the native method 'getSystemTimeZoneID'.
        /// </summary>
        /// <param name="javaHome"></param>
        /// <returns></returns>
        public static string getSystemTimeZoneID(string javaHome)
        {
            return MatchJavaTZ(javaHome, GetCurrentTimeZoneID()) ?? getSystemGMTOffsetID();
        }

        /// <summary>
        /// Implements the native method 'getSystemGMTOffsetID'.
        /// </summary>
        /// <returns></returns>
        public static string getSystemGMTOffsetID()
        {
            var sp = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var h = sp.Hours;
            var m = sp.Minutes;
            if (h == 0 && m == 0)
                return "GMT";
            else if (h >= 0 && m >= 0)
                return $"GMT+{h:D2}:{m:D2}";
            else
                return $"GMT-{-h:D2}:{-m:D2}";
        }

    }

}
