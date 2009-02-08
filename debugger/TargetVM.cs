/*
  Copyright (C) 2009 Volker Berlin (vberlin@inetsoftware.de)

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
using System.Text;
using Debugger;
using System.Collections;

namespace ikvm.debugger
{
    /// <summary>
    /// This class represent the target IKVM that should be debugged. The current implementation 
    /// based on Debugger.Core.dll from the SharpDevelop project.
    /// </summary>
    class TargetVM
    {
        private readonly NDebugger debugger;

        private readonly Process process;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid">Process ID of the IKVM</param>
        internal TargetVM(int pid)
        {
            debugger = new NDebugger();
            System.Diagnostics.Process sysProcess = System.Diagnostics.Process.GetProcessById(pid);
            debugger.Attach(sysProcess);
        }

        /// <summary>
        /// Return a list of thread IDs 
        /// </summary>
        /// <returns>never null</returns>
        internal int[] GetThreadIDs()
        {
            IList<Thread> list = process.Threads;
            int[] ids = new int[list.Count];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = (int)list[0].ID;
            }
            return ids;
        }
    }
}
