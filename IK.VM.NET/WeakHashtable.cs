using System;
using System.Runtime.InteropServices;
using System.Collections;

// TODO implement this properly, instead of this quick hack
public class WeakHashtable : IDictionary
{
	private GCHandle[] keys = new GCHandle[101];
	private object[] values = new object[101];

	public WeakHashtable()
	{
	}

	~WeakHashtable()
	{
		foreach(GCHandle h in keys)
		{
			if(h.IsAllocated)
			{
				h.Free();
			}
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
			return index != -1 && keys[index].IsAllocated && keys[index].Target != null;
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
			if(index != -1 && keys[index].IsAllocated && keys[index].Target != null)
			{
				throw new ArgumentException();
			}
			int newSize = keys.Length;
			while(index == -1)
			{
				Rehash(newSize);
				newSize = keys.Length * 2 - 1;
				index = FindKey(key);
			}
			if(keys[index].IsAllocated)
			{
				keys[index].Free();
			}
			keys[index] = GCHandle.Alloc(key, GCHandleType.Weak);
			values[index] = value;
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
		int start = key.GetHashCode() % keys.Length;
		int end = (start + 5) % keys.Length;
		for(int index = start; ; index = (index + 1) % keys.Length)
		{
			if(!keys[index].IsAllocated)
			{
				return index;
			}
			if(key.Equals(keys[index].Target))
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
		GCHandle[] currKeys = keys;
		object[] currValues = values;
	restart:
		keys = new GCHandle[newSize];
		values = new object[newSize];
		for(int i = 0; i < currKeys.Length; i++)
		{
			if(currKeys[i].IsAllocated)
			{
				object key = currKeys[i].Target;
				if(key != null)
				{
					int index = FindKey(key);
					if(index == -1)
					{
						newSize = newSize * 2 - 1;
						goto restart;
					}
					keys[index] = currKeys[i];
					values[index] = currValues[i];
				}
				else
				{
					currKeys[i].Free();
				}
			}
		}
	}

	public object this[object key]
	{
		get
		{
			lock(this)
			{
				int index = FindKey(key);
				if(index >= 0)
				{
					return values[index];
				}
				return null;
			}
		}
		set
		{
			lock(this)
			{
				int index = FindKey(key);
				int newSize = keys.Length;
				while(index == -1)
				{
					Rehash(newSize);
					newSize = keys.Length * 2 - 1;
					index = FindKey(key);
				}
				if(keys[index].IsAllocated)
				{
					keys[index].Free();
				}
				keys[index] = GCHandle.Alloc(key, GCHandleType.Weak);
				values[index] = value;
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
