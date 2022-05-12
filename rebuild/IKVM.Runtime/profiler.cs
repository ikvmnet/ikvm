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
using System.Collections.Generic;
using System.Diagnostics;

sealed class Profiler
{
	private static Profiler instance = new Profiler();
	private static Dictionary<string, Entry> counters = new Dictionary<string, Entry>();
	[ThreadStatic]
	private static Stack<Entry> stack;

	private sealed class Entry
	{
		internal long Time;
		internal long Count;
	}

	~Profiler()
	{
		// get rid off the warning that "instance" is unused
		instance.Equals(null);
		Console.Error.WriteLine("{0,-40}{1,10}{2,12}", "Event", "Count", "Time (ms)");
		Console.Error.WriteLine("{0,-40}{1,10}{2,12}", "-----", "-----", "---------");
		long totalTime = 0;
		foreach(KeyValuePair<string, Entry> e in counters)
		{
			Entry entry = e.Value;
			if(entry.Time == 0)
			{
				Console.Error.WriteLine("{0,-40}{1,10}", e.Key, entry.Count);
			}
			else
			{
				totalTime += entry.Time / 10000;
				Console.Error.WriteLine("{0,-40}{1,10}{2,12}", e.Key, entry.Count, entry.Time / 10000);
			}
		}
		Console.Error.WriteLine("{0,-40}{1,10}{2,12}", "", "", "---------");
		Console.Error.WriteLine("{0,-40}{1,10}{2,12}", "", "", totalTime);
	}

	[Conditional("PROFILE")]
	internal static void Enter(string name)
	{
		long ticks = DateTime.Now.Ticks;
		lock(counters)
		{
			if(stack == null)
			{
				stack = new Stack<Entry>();
			}
			if(stack.Count > 0)
			{
				stack.Peek().Time += ticks;
			}
			Entry e;
			if(!counters.TryGetValue(name, out e))
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
			Entry e = counters[name];
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
		lock(counters)
		{
			Entry e;
			if(!counters.TryGetValue(name, out e))
			{
				e = new Entry();
				counters[name] = e;
			}
			e.Count++;
		}
	}
}
