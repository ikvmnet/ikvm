using System;
using System.Runtime.InteropServices;
using System.Collections;

// TODO implement this properly, instead of this quick hack
public class WeakHashtable : IDictionary
{
	private struct Pair
	{
		private GCHandle keyHandle;
		internal object Value;

		internal object Key
		{
			get
			{
				return keyHandle.IsAllocated ? keyHandle.Target : null;
			}
			set
			{
				if(!keyHandle.IsAllocated)
				{
					keyHandle = GCHandle.Alloc(value, GCHandleType.Weak);
				}
				else
				{
					keyHandle.Target = value;
				}
			}
		}

		internal bool IsAllocated
		{
			get
			{
				return keyHandle.IsAllocated;
			}
		}

		internal void Free()
		{
			if(keyHandle.IsAllocated)
			{
				keyHandle.Free();
			}
		}
	}

	private Pair[] data = new Pair[101];

	public WeakHashtable()
	{
	}

	~WeakHashtable()
	{
		foreach(Pair p in data)
		{
			p.Free();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		throw new NotImplementedException();
	}

	public void Remove(object key)
	{
		throw new NotImplementedException();	
	}

	public bool Contains(object key)
	{
		return ContainsKey(key);
	}

	public bool ContainsKey(object key)
	{
		lock(this)
		{
			int index = FindKey(key);
			return index != -1 && data[index].Key != null;
		}
	}

	public void Clear()
	{
		throw new NotImplementedException();	
	}

	public void Add(object key, object value)
	{
		lock(this)
		{
			int index = FindKey(key);
			if(index != -1 && data[index].Key != null)
			{
				throw new ArgumentException();
			}
			int newSize = data.Length;
			while(index == -1)
			{
				Rehash(newSize);
				newSize = data.Length * 2 - 1;
				index = FindKey(key);
			}
			data[index].Key = key;
			data[index].Value = value;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// this returns the slot containing the key,
	// the empty slot that should contain the key, or -1 if the
	// table is too full to contain the key
	private int FindKey(object key)
	{
		int start = key.GetHashCode() % data.Length;
		int end = (start + 5) % data.Length;
		for(int index = start; ; index = (index + 1) % data.Length)
		{
			if(!data[index].IsAllocated)
			{
				return index;
			}
			if(key.Equals(data[index].Key))
			{
				return index;
			}
			if(index == end)
			{
				return -1;
			}
		}
	}

	private void Rehash(int newSize)
	{
		Profiler.Enter("WeakHashtable.Rehash");
		try
		{
			Pair[] curr = data;
		restart:
			data = new Pair[newSize];
			for(int i = 0; i < curr.Length; i++)
			{
				if(curr[i].IsAllocated)
				{
					object key = curr[i].Key;
					if(key != null)
					{
						int index = FindKey(key);
						if(index == -1)
						{
							newSize = newSize * 2 - 1;
							goto restart;
						}
						data[index] = curr[i];
					}
					else
					{
						curr[i].Free();
					}
				}
			}
		}
		finally
		{
			Profiler.Leave("WeakHashtable.Rehash");
		}
	}

	public object this[object key]
	{
		get
		{
			lock(this)
			{
				int index = FindKey(key);
				return index == -1 ? null : data[index].Value;
			}
		}
		set
		{
			lock(this)
			{
				int index = FindKey(key);
				int newSize = data.Length;
				while(index == -1)
				{
					Rehash(newSize);
					newSize = data.Length * 2 - 1;
					index = FindKey(key);
				}
				data[index].Key = key;
				data[index].Value = value;
			}
		}
	}

	public ICollection Values
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ICollection Keys
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	public void CopyTo(Array array, int index)
	{
		throw new NotImplementedException();	
	}

	public bool IsSynchronized
	{
		get
		{
			return true;
		}
	}

	public int Count
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public object SyncRoot
	{
		get
		{
			return this;
		}
	}
}
