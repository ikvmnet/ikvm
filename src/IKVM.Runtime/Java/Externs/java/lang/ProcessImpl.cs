/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IKVM.Java.Externs.java.lang
{

    static class ProcessImpl
    {

        public static int parseCommandString(string cmdstr)
        {
            int pos = cmdstr.IndexOf(' ');
            if (pos == -1)
            {
                return cmdstr.Length;
            }
            if (cmdstr[0] == '"')
            {
                int close = cmdstr.IndexOf('"', 1);
                return close == -1 ? cmdstr.Length : close + 1;
            }
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return pos;
            }
            IList<string> path = null;
            for (; ; )
            {
                string str = cmdstr.Substring(0, pos);
                if (IsPathRooted(str))
                {
                    if (Exists(str))
                    {
                        return pos;
                    }
                }
                else
                {
                    if (path == null)
                    {
                        path = GetSearchPath();
                    }
                    foreach (string p in path)
                    {
                        if (Exists(Path.Combine(p, str)))
                        {
                            return pos;
                        }
                    }
                }
                if (pos == cmdstr.Length)
                {
                    return cmdstr.IndexOf(' ');
                }
                pos = cmdstr.IndexOf(' ', pos + 1);
                if (pos == -1)
                {
                    pos = cmdstr.Length;
                }
            }
        }

        private static List<string> GetSearchPath()
        {
            List<string> list = new List<string>();
            list.Add(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
            list.Add(Environment.CurrentDirectory);
            list.Add(Environment.SystemDirectory);
            string windir = Path.GetDirectoryName(Environment.SystemDirectory);
            list.Add(Path.Combine(windir, "System"));
            list.Add(windir);
            string path = Environment.GetEnvironmentVariable("PATH");
            if (path != null)
            {
                foreach (string p in path.Split(Path.PathSeparator))
                {
                    list.Add(p);
                }
            }
            return list;
        }

        private static bool IsPathRooted(string path)
        {
            try
            {
                return Path.IsPathRooted(path);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static bool Exists(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    return true;
                }
                else if (Directory.Exists(file))
                {
                    return false;
                }
                else if (file.IndexOf('.') == -1 && File.Exists(file + ".exe"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }

}
