using System;
using System.Collections;

// TODO implement this properly, instead of this quick hack
public class WeakHashtable : IDictionary
{
	private struct KeyValue
	{
		public WeakReference Key;
		public object Value;
	}
	private KeyValue[] items = new KeyValue[101];

	public WeakHashtable()
	{
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
			return index != -1 && items[index].Key != null && items[index].Key.Target != null;
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
			if(index != -1 && items[index].Key != null && items[index].Key.Target != null)
			{
				throw new ArgumentException();
			}
			int newSize = items.Length;
			while(index == -1)
			{
				Rehash(newSize);
				newSize = items.Length * 2 - 1;
				index = FindKey(key);
			}
			items[index].Key = new WeakReference(key);
			items[index].Value = value;
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
		int start = key.GetHashCode() % items.Length;
		int end = (start + 5) % items.Length;
		for(int index = start; ; index = (index + 1) % items.Length)
		{
			if(items[index].Key == null)
			{
				return index;
			}
			if(key.Equals(items[index].Key.Target))
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
		KeyValue[] curr = items;
	restart:
		items = new KeyValue[newSize];
		for(int i = 0; i < curr.Length; i++)
		{
			if(curr[i].Key != null)
			{
				object key = curr[i].Key.Target;
				if(key != null)
				{
					int index = FindKey(key);
					if(index == -1)
					{
						newSize = newSize * 2 - 1;
						goto restart;
					}
					items[index].Key = new WeakReference(key);
					items[index].Value = curr[i].Value;
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
					return items[index].Value;
				}
				return null;
			}
		}
		set
		{
			lock(this)
			{
				int index = FindKey(key);
				int newSize = items.Length;
				while(index == -1)
				{
					Rehash(newSize);
					newSize = items.Length * 2 - 1;
					index = FindKey(key);
				}
				items[index].Key = new WeakReference(key);
				items[index].Value = value;
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
