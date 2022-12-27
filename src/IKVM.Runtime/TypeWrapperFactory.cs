﻿/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

#if NETCOREAPP
using System.Runtime.Loader;
#endif

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;

using ProtectionDomain = java.security.ProtectionDomain;
#endif

namespace IKVM.Internal
{

#if !EXPORTER

    abstract class TypeWrapperFactory
    {

        internal abstract ModuleBuilder ModuleBuilder { get; }

        internal abstract TypeWrapper DefineClassImpl(Dictionary<string, TypeWrapper> types, TypeWrapper host, ClassFile f, ClassLoaderWrapper classLoader, ProtectionDomain protectionDomain);

        internal abstract bool ReserveName(string name);

        internal abstract string AllocMangledName(DynamicTypeWrapper tw);

        internal abstract Type DefineUnloadable(string name);

        internal abstract Type DefineDelegate(int parameterCount, bool returnVoid);

        internal abstract bool HasInternalAccess { get; }

#if CLASSGC

        internal abstract void AddInternalsVisibleTo(Assembly friend);

#endif
    }

#endif

}
