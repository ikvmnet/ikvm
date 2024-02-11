/*
  Copyright (C) 2008, 2009 Jeroen Frijters

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
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Impl
{

    static class SymbolSupport
    {

        internal static ISymbolWriterImpl CreateSymbolWriterFor(ModuleBuilder moduleBuilder)
        {
#if NETFRAMEWORK
            return new PdbWriter(moduleBuilder);
#else
            throw new NotSupportedException("IKVM.Reflection must be compiled with MONO defined to support writing Mono debugging symbols.");
#endif
        }

        internal static byte[] GetDebugInfo(ISymbolWriterImpl writer, ref IMAGE_DEBUG_DIRECTORY idd)
        {
            return writer.GetDebugInfo(ref idd);
        }

        internal static void RemapToken(ISymbolWriterImpl writer, int oldToken, int newToken)
        {
            writer.RemapToken(oldToken, newToken);
        }

    }

}
