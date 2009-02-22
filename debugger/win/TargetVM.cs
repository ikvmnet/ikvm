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
using Debugger.Wrappers.MetaData;
using ikvm.debugger.requests;


namespace ikvm.debugger.win
{
    /// <summary>
    /// This class represent the target IKVM that should be debugged. The current implementation 
    /// based on Debugger.Core.dll from the SharpDevelop project.
    /// </summary>
    class TargetVM
    {
        private readonly NDebugger debugger;

        private readonly Process process;

        private readonly Dictionary<String, IList<TargetType>> nameTypeMap = new Dictionary<String, IList<TargetType>>();

        private readonly JdwpEventHandler jdwpEventHandler = null;


        private EventRequest threadStartEventRequest;

        /// <summary>
        /// Create a new target VM for the giveb process id.
        /// </summary>
        /// <param name="pid">Process ID of the IKVM</param>
        internal TargetVM(int pid, JdwpEventHandler jdwpEventHandler)
        {
            this.jdwpEventHandler = jdwpEventHandler;
            debugger = new NDebugger();
            System.Diagnostics.Process sysProcess = System.Diagnostics.Process.GetProcessById(pid);
            process = debugger.Attach(sysProcess);
            process.Exited += new EventHandler(ProcessExited);

            process.ModuleLoaded += new EventHandler<ModuleEventArgs>(ModuleLoaded);
            process.Paused += new EventHandler<ProcessEventArgs>(Paused);
            process.Resumed += new EventHandler<ProcessEventArgs>(Resumed);
            process.ThreadStarted += new EventHandler<ThreadEventArgs>(ThreadStarted);
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

        /// <summary>
        /// Find all types with the given names that can currently loaded.
        /// If the module or depending modules was not loaded then a empty list return.
        /// </summary>
        /// <param name="jniClassName">a class name with JNI syntax.</param>
        /// <returns>the list, never null</returns>
        internal IList<TargetType> FindTypes(String jniClassName)
        {
            String className = DebuggerUtils.ConvertJniClassName(jniClassName);
            IList<TargetType> result;
            try
            {
                result = nameTypeMap[className];
            }
            catch (KeyNotFoundException)
            {
                result = new List<TargetType>();
                nameTypeMap.Add(className, result);
            }
            IList<DebugType> types = FindTypesInModules(className);
            foreach (DebugType type in types)
            {
                bool isFound = false;
                foreach (TargetType tgType in result)
                {
                    if (tgType.Identical(type))
                    {
                        isFound = true;
                        break; 
                    }
                }
                if (!isFound)
                {
                    result.Add(new TargetType(type));
                }
            }

            return result;
        }

        internal TargetType FindType( int typeId )
        {
            return TargetType.GetTargetType( typeId );
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
                Console.Error.WriteLine("ModuleLoaded:" + module.FullPath + "|");
                /*List<DebugType> types = module.GetDefinedTypes();
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
                }*/
            }
            catch (System.Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private void Paused(object sender, ProcessEventArgs ev)
        {
            Console.Error.WriteLine("Paused:" + ev);
        }

        private void Resumed(object sender, ProcessEventArgs ev)
        {
            Console.Error.WriteLine("Resumed:" + ev);
        }

        private void ThreadStarted(object sender, ThreadEventArgs ev)
        {
            EventRequest eventRequest = threadStartEventRequest;
            if (eventRequest != null)
            {
                Thread th = ev.Thread;
                jdwpEventHandler.Send(SuspendPolicy.EVENT_THREAD, EventKind.THREAD_START, eventRequest.RequestId, (int)th.ID);
                ev.Thread.Exited += new EventHandler<ThreadEventArgs>(ThreadExited);
            }
            Console.Error.WriteLine("ThreadStarted:" + ev.Thread.ID+ " " + eventRequest);
        }

        private void ThreadExited(object sender, ThreadEventArgs ev)
        {
            Console.Error.WriteLine("ThreadExited:" + ev.Thread.ID);
        }



        private IList<DebugType> FindTypesInModules(String name)
        {
            List<DebugType> result = new List<DebugType>();
            List<Module> modules = new List<Module>(process.Modules);
            foreach (Module module in modules)
            {
                MetaDataImport metaData = module.MetaData;
                foreach (TypeDefProps typeDef in module.MetaData.EnumTypeDefProps())
                {
                    uint token = typeDef.Token;
                    if (metaData.GetGenericParamCount(token) == 0)
                    {
                        if (name.Equals(typeDef.Name))
                        {
                            try
                            {
                                result.Add(DebugType.Create(module, token));
                            }
                            catch { }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Set an EventRequest received from debugger. 
        /// </summary>
        /// <param name="eventRequest">the new EventRequest</param>
        internal void AddEventRequest(ikvm.debugger.requests.EventRequest eventRequest)
        {
            switch (eventRequest.EventKind)
            {
                case EventKind.THREAD_START:
                    threadStartEventRequest = eventRequest;
                    break;
            }
        }
    }
}
