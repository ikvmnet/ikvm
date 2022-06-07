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

namespace IKVM.Runtime.JniExport.java.lang
{

    static class Runtime
    {

        public static int availableProcessors(object thisRuntime)
        {
            return Environment.ProcessorCount;
        }

        public static long freeMemory(object thisRuntime)
        {
            // TODO figure out if there is anything meaningful we can return here
            return 10 * 1024 * 1024;
        }

        public static long totalMemory(object thisRuntime)
        {
            // NOTE this really is a bogus number, but we have to return something
            return GC.GetTotalMemory(false) + freeMemory(thisRuntime);
        }

        public static long maxMemory(object thisRuntime)
        {
            // spec says: If there is no inherent limit then the value Long.MAX_VALUE will be returned.
            return Int64.MaxValue;
        }

        public static void gc(object thisRuntime)
        {
            GC.Collect();
        }

        public static void traceInstructions(object thisRuntime, bool on)
        {
        }

        public static void traceMethodCalls(object thisRuntime, bool on)
        {
        }

        public static void runFinalization0()
        {
            GC.WaitForPendingFinalizers();
        }

    }

}
