/*
  Copyright (C) 2008 Jeroen Frijters

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

namespace IKVM.Reflection.Emit
{

    public enum StackBehaviour
	{

		Pop0 = 0,
		Pop1 = 1,
		Pop1_pop1 = 2,
		Popi = 3,
		Popi_pop1 = 4,
		Popi_popi = 5,
		Popi_popi8 = 6,
		Popi_popi_popi = 7,
		Popi_popr4 = 8,
		Popi_popr8 = 9,
		Popref = 10,
		Popref_pop1 = 11,
		Popref_popi = 12,
		Popref_popi_popi = 13,
		Popref_popi_popi8 = 14,
		Popref_popi_popr4 = 15,
		Popref_popi_popr8 = 16,
		Popref_popi_popref = 17,
		Push0 = 18,
		Push1 = 19,
		Push1_push1 = 20,
		Pushi = 21,
		Pushi8 = 22,
		Pushr4 = 23,
		Pushr8 = 24,
		Pushref = 25,
		Varpop = 26,
		Varpush = 27,
		Popref_popi_pop1 = 28,

	}

}
