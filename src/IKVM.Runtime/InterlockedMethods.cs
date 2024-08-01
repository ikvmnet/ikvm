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

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif


namespace IKVM.Runtime
{
    class InterlockedMethods
    {

        readonly RuntimeContext context;

        internal readonly MethodInfo AddInt32;
        internal readonly MethodInfo CompareExchangeInt32;
        internal readonly MethodInfo CompareExchangeInt64;
        internal readonly MethodInfo CompareExchangeOfT;
        internal readonly MethodInfo ExchangeOfT;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public InterlockedMethods(RuntimeContext context)
        {
            this.context = context;

            var type = context.Resolver.ResolveCoreType(typeof(System.Threading.Interlocked).FullName);
            AddInt32 = type.GetMethod("Add", new Type[] { context.Types.Int32.MakeByRefType(), context.Types.Int32 });
            CompareExchangeInt32 = type.GetMethod("CompareExchange", new Type[] { context.Types.Int32.MakeByRefType(), context.Types.Int32, context.Types.Int32 });
            CompareExchangeInt64 = type.GetMethod("CompareExchange", new Type[] { context.Types.Int64.MakeByRefType(), context.Types.Int64, context.Types.Int64 });
            foreach (MethodInfo m in type.GetMethods())
            {
                if (m.IsGenericMethodDefinition)
                {
                    switch (m.Name)
                    {
                        case "CompareExchange":
                            CompareExchangeOfT = m;
                            break;
                        case "Exchange":
                            ExchangeOfT = m;
                            break;
                    }
                }
            }

        }
    }

}
