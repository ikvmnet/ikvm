/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Collections;
using System.Diagnostics;

class Profiler
{
	private static Profiler instance = new Profiler();
	private static Hashtable counters = new Hashtable();
	[ThreadStatic]
	private static Stack stack;

	private class Entry
	{
		internal long Time;
		internal long Count;
	}

	~Profiler()
	{
		foreach(DictionaryEntry e in counters)
		{
			Entry entry = (Entry)e.Value;
			if(entry.Time == 0)
			{
				Console.WriteLine("{0} occurred {1} times", e.Key, entry.Count);
			}
			else
			{
				Console.WriteLine("{0} was executed {1} times for a total of {2} ms", e.Key, entry.Count, entry.Time / 10000);
			}
		}
	}

	[Conditional("PROFILE")]
	internal static void Enter(string name)
	{
		long ticks = DateTime.Now.Ticks;
		lock(counters)
		{
			if(stack == null)
			{
				stack = new Stack();
			}
			if(stack.Count > 0)
			{
				((Entry)stack.Peek()).Time += ticks;
			}
			Entry e = (Entry)counters[name];
			if(e == null)
			{
				e = new Entry();
				counters[name] = e;
			}
			e.Count++;
			e.Time -= ticks;
			stack.Push(e);
		}
	}

	[Conditional("PROFILE")]
	internal static void Leave(string name)
	{
		long ticks = DateTime.Now.Ticks;
		stack.Pop();
		lock(counters)
		{
			Entry e = (Entry)counters[name];
			e.Time += ticks;
			if(stack.Count > 0)
			{
				((Entry)stack.Peek()).Time -= ticks;
			}
		}
	}

	[Conditional("PROFILE")]
	internal static void Count(string name)
	{
		Entry e = (Entry)counters[name];
		if(e == null)
		{
			e = new Entry();
			counters[name] = e;
		}
		e.Count++;
	}
}
