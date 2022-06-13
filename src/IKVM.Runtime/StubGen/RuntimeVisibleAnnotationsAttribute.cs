/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.IO;

using IKVM.Attributes;

namespace IKVM.StubGen
{
    sealed class RuntimeVisibleAnnotationsAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private MemoryStream mem;
		private BigEndianStream bes;
		private ushort count;

		internal RuntimeVisibleAnnotationsAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("RuntimeVisibleAnnotations"))
		{
			this.classFile = classFile;
			mem = new MemoryStream();
			bes = new BigEndianStream(mem);
		}

		internal void Add(object[] annot)
		{
			count++;
			bes.WriteUInt16(classFile.AddUtf8((string)annot[1]));
			bes.WriteUInt16((ushort)((annot.Length - 2) / 2));
			for (int i = 2; i < annot.Length; i += 2)
			{
				bes.WriteUInt16(classFile.AddUtf8((string)annot[i]));
				WriteElementValue(bes, annot[i + 1]);
			}
		}

		private static string DecodeTypeName(string typeName)
		{
#if !FIRST_PASS && !STUB_GENERATOR
			int index = typeName.IndexOf(',');
			if (index > 0)
			{
				// HACK if we have an assembly qualified type name we have to resolve it to a Java class name
				// (at the very least we should use the right class loader here)
				try
				{
					typeName = "L" + java.lang.Class.forName(typeName.Substring(1, typeName.Length - 2).Replace('/', '.')).getName().Replace('.', '/') + ";";
				}
				catch { }
			}
#endif
			return typeName;
		}

		private void WriteElementValue(BigEndianStream bes, object val)
		{
			if (val is object[])
			{
				object[] arr = (object[])val;
				if (AnnotationDefaultAttribute.TAG_ENUM.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ENUM);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
					bes.WriteUInt16(classFile.AddUtf8((string)arr[2]));
				}
				else if (AnnotationDefaultAttribute.TAG_ARRAY.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ARRAY);
					bes.WriteUInt16((ushort)(arr.Length - 1));
					for (int i = 1; i < arr.Length; i++)
					{
						WriteElementValue(bes, arr[i]);
					}
				}
				else if (AnnotationDefaultAttribute.TAG_CLASS.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_CLASS);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
				}
				else if (AnnotationDefaultAttribute.TAG_ANNOTATION.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ANNOTATION);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
					bes.WriteUInt16((ushort)((arr.Length - 2) / 2));
					for (int i = 2; i < arr.Length; i += 2)
					{
						bes.WriteUInt16(classFile.AddUtf8((string)arr[i]));
						WriteElementValue(bes, arr[i + 1]);
					}
				}
			}
			else if (val is bool)
			{
				bes.WriteByte((byte)'Z');
				bes.WriteUInt16(classFile.AddInt((bool)val ? 1 : 0));
			}
			else if (val is byte)
			{
				bes.WriteByte((byte)'B');
				bes.WriteUInt16(classFile.AddInt((byte)val));
			}
			else if (val is char)
			{
				bes.WriteByte((byte)'C');
				bes.WriteUInt16(classFile.AddInt((char)val));
			}
			else if (val is short)
			{
				bes.WriteByte((byte)'S');
				bes.WriteUInt16(classFile.AddInt((short)val));
			}
			else if (val is int)
			{
				bes.WriteByte((byte)'I');
				bes.WriteUInt16(classFile.AddInt((int)val));
			}
			else if (val is long)
			{
				bes.WriteByte((byte)'J');
				bes.WriteUInt16(classFile.AddLong((long)val));
			}
			else if (val is float)
			{
				bes.WriteByte((byte)'F');
				bes.WriteUInt16(classFile.AddFloat((float)val));
			}
			else if (val is double)
			{
				bes.WriteByte((byte)'D');
				bes.WriteUInt16(classFile.AddDouble((double)val));
			}
			else if (val is string)
			{
				bes.WriteByte((byte)'s');
				bes.WriteUInt16(classFile.AddUtf8((string)val));
			}
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32(Length);
			WriteImpl(bes);
		}

		internal void WriteImpl(BigEndianStream bes)
		{
			bes.WriteUInt16(count);
			foreach (byte b in mem.ToArray())
			{
				bes.WriteByte(b);
			}
		}

		internal uint Length
		{
			get { return (uint)mem.Length + 2; }
		}
	}
}
