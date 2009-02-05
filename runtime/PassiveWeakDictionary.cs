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
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IKVM.Internal
{
	/*
	 * This is a special purpose dictionary. It only keeps weak references to the keys,
	 * thereby making them eligable for GC at any time.
	 * 
	 * However, it does not make any promises about cleaning up the values (hence "passive").
	 * This class should only be used if memory consumption and lifetime of the values is of no consequence.
	 * 
	 * Furthermore, the type also assumes that it needs to continue to work during AppDomain unload,
	 * meaning that it uses GC.SuppressFinalize() on the WeakReference objects it uses to prevent
	 * them from being finalized during AppDomain unload.
	 * As a consequence, if you want to dispose on object of this type prior to AppDomain unload, you *must*
	 * call Dispose() or you will leak GC handles. After calling Dispose() you should not call any other
	 * methods.
	 * 
	 * This class is thread safe and TryGetValue is lock free.
	 * 
	 */
	sealed class PassiveWeakDictionary<TKey, TValue>
		where TKey : class
		where TValue : class
	{
		private volatile Bucket[] table = new Bucket[37];
		private int count;
		private FreeNode freeList;
		private int freeCount;

		internal void Dispose()
		{
			Bucket[] table = this.table;
			for (int i = 0; i < table.Length; i++)
			{
				for (Bucket b = table[i]; b != null; b = b.Next)
				{
					GC.ReRegisterForFinalize(b.WeakRef);
				}
			}
			for (FreeNode b = freeList; b != null; b = b.Next)
			{
				GC.ReRegisterForFinalize(b.WeakRef);
			}
		}

		internal void Add(TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			lock (this)
			{
				if (count > table.Length)
				{
					Rehash();
				}
				int index = GetHashCode(key) % table.Length;
				for (Bucket b = table[index]; b != null; b = b.Next)
				{
					if (b.Key == key)
					{
						throw new ArgumentException("Key already exists");
					}
				}
				table[index] = new Bucket(AllocWeakRef(key), value, table[index]);
				count++;
			}
		}

		private WeakReference AllocWeakRef(TKey key)
		{
			WeakReference weakRef;
			if (freeList != null)
			{
				weakRef = freeList.WeakRef;
				freeList = freeList.Next;
				freeCount--;
				weakRef.Target = key;
			}
			else
			{
				weakRef = new WeakReference(key, true);
				GC.SuppressFinalize(weakRef);
			}
			return weakRef;
		}

		private static readonly int[] primes = {
			37, 67, 131, 257, 521, 1031, 2053, 4099, 8209, 16411, 32771, 65537, 131101, 262147, 524309, 1048583, 2097169,
			4194319, 8388617, 16777259, 33554467, 67108879, 134217757, 268435459, 536870923, 1073741827, 2147483647 };

		private static int GetPrime(int count)
		{
			foreach (int p in primes)
			{
				if (p > count)
					return p;
			}
			return count;
		}

		private static int GetHashCode(TKey key)
		{
			return RuntimeHelpers.GetHashCode(key) & 0x7FFFFFFF;
		}

		private void Rehash()
		{
			count = 0;
			for (int i = 0; i < table.Length; i++)
			{
				for (Bucket b = table[i]; b != null; b = b.Next)
				{
					if (b.Key != null)
					{
						count++;
					}
				}
			}
			Bucket[] newTable = new Bucket[GetPrime(count * 2)];
			for (int i = 0; i < table.Length; i++)
			{
				for (Bucket b = table[i]; b != null; b = b.Next)
				{
					TKey key = b.Key;
					if (key != null)
					{
						int index = GetHashCode(key) % newTable.Length;
						WeakReference weakRef = AllocWeakRef(key);
						newTable[index] = new Bucket(weakRef, b.Value, newTable[index]);
					}
					else
					{
						// don't cache an infinite amount,
						// busy loop garbage allocation benchmarking shows that 8K is enough with current gen0 size
						if (freeCount < 8192)
						{
							WeakReference weakRef = b.WeakRef;
							b.Clear();
							freeList = new FreeNode(weakRef, freeList);
							freeCount++;
						}
						else
						{
							GC.ReRegisterForFinalize(b.WeakRef);
							b.Clear();
						}
					}
				}
			}
			this.table = newTable;
		}

		internal bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			Bucket[] table = this.table;
			int index = GetHashCode(key) % table.Length;
			for (Bucket b = table[index]; b != null; b = b.Next)
			{
				if (b.Key == key)
				{
					value = b.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		private sealed class Bucket
		{
			private volatile WeakReference weakRef;
			internal TValue Value;
			internal readonly Bucket Next;

			internal Bucket(WeakReference weakRef, TValue value, Bucket next)
			{
				this.weakRef = weakRef;
				this.Value = value;
				this.Next = next;
			}

			internal WeakReference WeakRef
			{
				get
				{
					return weakRef;
				}
			}

			internal TKey Key
			{
				get
				{
					WeakReference weakRef = this.weakRef;
					if (weakRef != null)
					{
						TKey key = (TKey)weakRef.Target;
						// this extra null check is to guard against the case where there was a GC and a Rehash and the weak ref was reused for another slot
						// between the initial read of weakRef and accessing its Target property
						if (this.weakRef != null)
						{
							return key;
						}
					}
					return null;
				}
			}

			internal void Clear()
			{
				weakRef = null;
			}
		}

		private sealed class FreeNode
		{
			internal readonly WeakReference WeakRef;
			internal readonly FreeNode Next;

			internal FreeNode(WeakReference weakRef, FreeNode next)
			{
				this.WeakRef = weakRef;
				this.Next = next;
			}
		}
	}
}
