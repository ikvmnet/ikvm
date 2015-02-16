/*
  Copyright (C) 2015 Jeroen Frijters

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

#if CORECLR
namespace System.Diagnostics
{
	static class Debug
	{
		[Conditional("DEBUG")]
		internal static void Assert(bool cond)
		{
			if (!cond)
			{
				Environment.FailFast("assertion failed");
			}
		}
	}
}

namespace System.Collections.Generic
{
	sealed class Stack<T>
	{
		private readonly List<T> items = new List<T>();

		internal void Push(T value)
		{
			items.Add(value);
		}

		internal T Peek()
		{
			if (items.Count == 0)
			{
				throw new InvalidOperationException();
			}
			return items[items.Count - 1];
		}

		internal T Pop()
		{
			T value = Peek();
			items.RemoveAt(items.Count - 1);
			return value;
		}
	}
}
#endif
