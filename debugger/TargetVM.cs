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
using Debugger.MetaData;
using System.Windows.Forms;


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

        private readonly Dictionary<String, DebugType> typeMap = new Dictionary<String, DebugType>();

        /// <summary>
        /// Create a new target VM for the giveb process id.
        /// </summary>
        /// <param name="pid">Process ID of the IKVM</param>
        internal TargetVM(int pid)
        {
            debugger = new NDebugger();
            System.Diagnostics.Process sysProcess = System.Diagnostics.Process.GetProcessById(pid);
            process = debugger.Attach(sysProcess);
            process.Exited += new EventHandler(ProcessExited);

            process.ModuleLoaded += new EventHandler<ModuleEventArgs>(ModuleLoaded);
            process.Paused += new EventHandler<ProcessEventArgs>(Paused);
            process.Resumed += new EventHandler<ProcessEventArgs>(Resumed);

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

        internal void Suspend()
        {
            debugger.MTA2STA.Call(delegate { process.Break(); });
        }

        internal void Resume()
        {
            debugger.MTA2STA.Call(delegate { process.Continue(); });
        }

        internal void Exit(int exitCode)
        {
            debugger.MTA2STA.Call(delegate { process.Terminate(); });
        }

        internal int GetTypeID(String jniClassName)
        {
            String className = DebuggerUtils.ConvertJniClassName(jniClassName);
            int id;
            try
            {
                DebugType type = typeMap[className];
                id = (int)type.Token;
            }
            catch (KeyNotFoundException)
            {
                id = 0;
            }
            Console.Error.WriteLine("GetTypeID:" + jniClassName + " " + className + " " + id);
            return id;
        }

        void ProcessExited(object sender, EventArgs ev)
        {
            Environment.Exit(0);
        }

        void ModuleLoaded(object sender, ModuleEventArgs ev)
        {
            try
            {
                Module module = ev.Module;
                Console.Error.WriteLine("ModuleLoaded:" + module.Filename + "|");
                List<DebugType> types = module.GetDefinedTypes();
                Console.Error.WriteLine("ModuleLoaded:" + types.Count);
                for (int t = 0; t < types.Count; t++)
                {
                    try
                    {
                        DebugType type = types[t];
                        //Console.Error.WriteLine(type + " " + type.FullName);
                        typeMap.Add(type.FullName, type);
                    }
                    catch (ArgumentException)
                    {
                        //TODO add support for duplicate values
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            Console.Error.WriteLine("ModuleLoaded:" + typeMap.Count);
        }

        void Paused(object sender, ProcessEventArgs ev)
        {
            Console.Error.WriteLine("Paused:" + ev);
        }

        void Resumed(object sender, ProcessEventArgs ev)
        {
            Console.Error.WriteLine("Resumed:" + ev);
        }
    }
}
