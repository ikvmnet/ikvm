/*
  Copyright (C) 2002-2012 Jeroen Frijters

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
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.Runtime
{

    sealed class CodeEmitterLocal
	{

		private TypeSymbol type;
		private string name;
		private IKVM.CoreLib.Symbols.Emit.LocalBuilder local;

		internal CodeEmitterLocal(TypeSymbol type)
		{
			this.type = type;
		}

		internal TypeSymbol LocalType
		{
			get { return type; }
		}

		internal void SetLocalSymInfo(string name)
		{
			this.name = name;
		}

		internal int __LocalIndex
		{
			get { return local == null ? 0xFFFF : local.LocalIndex; }
		}

		internal void Emit(IKVM.CoreLib.Symbols.Emit.ILGenerator ilgen, OpCode opcode)
        {
            // it's a temporary local that is only allocated on-demand
            local ??= ilgen.DeclareLocal(type);
			ilgen.Emit(opcode, local);
		}

		internal void Declare(IKVM.CoreLib.Symbols.Emit.ILGenerator ilgen)
		{
			local = ilgen.DeclareLocal(type);

            if (name != null)
				local.SetLocalSymInfo(name);
        }

    }

}
