/*
  Copyright (C) 2007-2011 Jeroen Frijters

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
using System.IO;
using System.Reflection;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a fake path to the java executable.
    /// Research shows somebody trying to launch Minecraft in IKVM, and the Minecraft launcher attempting to relaunch
    /// the application using javac.exe directly. I don't know if this spawned the need for this fake path, but it
    /// seems like a good likelihood.
    /// </summary>
    sealed class VfsJavaExe : VfsExecutable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public VfsJavaExe(VfsContext context) :
            base(context)
        {

        }

        public override string GetLink()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "ikvm.exe");
            else
                throw new PlatformNotSupportedException("Cannot locate a compatible Java executable for this platform.");
        }

    }

}
