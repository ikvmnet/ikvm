/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
#if !NO_REF_EMIT
using System.Reflection.Emit;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;
using IKVM.Internal;

static class Java_java_io_Console
{
	public static string encoding()
	{
		int cp = 437;
		try
		{
			cp = Console.InputEncoding.CodePage;
		}
		catch
		{
		}
		if (cp >= 874 && cp <= 950)
		{
			return "ms" + cp;
		}
		return "cp" + cp;
	}

	private const int STD_INPUT_HANDLE = -10;
	private const int ENABLE_ECHO_INPUT = 0x0004;

	[DllImport("kernel32")]
	private static extern IntPtr GetStdHandle(int nStdHandle);

	[DllImport("kernel32")]
	private static extern int GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

	[DllImport("kernel32")]
	private static extern int SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

	public static bool echo(bool on)
	{
#if !FIRST_PASS
		// HACK the only way to get this to work is by p/invoking the Win32 APIs
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			IntPtr hStdIn = GetStdHandle(STD_INPUT_HANDLE);
			if (hStdIn.ToInt64() == 0 || hStdIn.ToInt64() == -1)
			{
				throw new java.io.IOException("The handle is invalid");
			}
			int fdwMode;
			if (GetConsoleMode(hStdIn, out fdwMode) == 0)
			{
				throw new java.io.IOException("GetConsoleMode failed");
			}
			bool old = (fdwMode & ENABLE_ECHO_INPUT) != 0;
			if (on)
			{
				fdwMode |= ENABLE_ECHO_INPUT;
			}
			else
			{
				fdwMode &= ~ENABLE_ECHO_INPUT;
			}
			if (SetConsoleMode(hStdIn, fdwMode) == 0)
			{
				throw new java.io.IOException("SetConsoleMode failed");
			}
			return old;
		}
#endif
		return true;
	}

	public static bool istty()
	{
		// The JDK returns false here if stdin or stdout (not stderr) is redirected to a file
		// or if there is no console associated with the current process.
		// The best we can do is to look at the KeyAvailable property, which
		// will throw an InvalidOperationException if stdin is redirected or not available
		try
		{
			return Console.KeyAvailable || true;
		}
		catch (InvalidOperationException)
		{
			return false;
		}
	}
}

static class Java_java_io_FileDescriptor
{
	private static Converter<int, int> fsync;

	public static Stream open(string name, FileMode fileMode, FileAccess fileAccess)
	{
		if (VirtualFileSystem.IsVirtualFS(name))
		{
			return VirtualFileSystem.Open(name, fileMode, fileAccess);
		}
		else if (fileMode == FileMode.Append)
		{
			// this is the way to get atomic append behavior for all writes
			return new FileStream(name, fileMode, FileSystemRights.AppendData, FileShare.ReadWrite, 1, FileOptions.None);
		}
		else
		{
			return new FileStream(name, fileMode, fileAccess, FileShare.ReadWrite, 1, false);
		}
	}

	[SecuritySafeCritical]
	public static bool flushPosix(FileStream fs)
	{
		if (fsync == null)
		{
			ResolveFSync();
		}
		bool success = false;
		SafeFileHandle handle = fs.SafeFileHandle;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			handle.DangerousAddRef(ref success);
			return fsync(handle.DangerousGetHandle().ToInt32()) == 0;
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[SecurityCritical]
	private static void ResolveFSync()
	{
		// we don't want a build time dependency on this Mono assembly, so we use reflection
		Type type = Type.GetType("Mono.Unix.Native.Syscall, Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
		if (type != null)
		{
			fsync = (Converter<int, int>)Delegate.CreateDelegate(typeof(Converter<int, int>), type, "fsync", false, false);
		}
		if (fsync == null)
		{
			fsync = DummyFSync;
		}
	}

	private static int DummyFSync(int fd)
	{
		return 0;
	}
}

static class Java_java_io_FileInputStream
{
	public static void open0(object _this, string name, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.openReadOnly(name);
#endif
	}

	public static int read0(object _this, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.read();
#endif
	}

	public static int readBytes(object _this, byte[] b, int off, int len, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.readBytes(b, off, len);
#endif
	}

	public static long skip(object _this, long n, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.skip(n);
#endif
	}

	public static int available(object _this, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.available();
#endif
	}

	public static void close0(object _this, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.close();
#endif
	}

	public static void initIDs()
	{
	}
}

static class Java_java_io_FileOutputStream
{
	public static void open0(object _this, string name, bool append, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		if (append)
		{
			fd.openAppend(name);
		}
		else
		{
			fd.openWriteOnly(name);
		}
#endif
	}

	public static void write(object _this, int b, bool append, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.write(b);
#endif
	}

	public static void writeBytes(object _this, byte[] b, int off, int len, bool append, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.writeBytes(b, off, len);
#endif
	}

	public static void close0(object _this, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.close();
#endif
	}

	public static void initIDs()
	{
	}
}

static class Java_java_io_ObjectInputStream
{
	public static void bytesToFloats(byte[] src, int srcpos, float[] dst, int dstpos, int nfloats)
	{
		IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
		for (int i = 0; i < nfloats; i++)
		{
			int v = src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			dst[dstpos++] = IKVM.Runtime.FloatConverter.ToFloat(v, ref converter);
		}
	}

	public static void bytesToDoubles(byte[] src, int srcpos, double[] dst, int dstpos, int ndoubles)
	{
		IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
		for (int i = 0; i < ndoubles; i++)
		{
			long v = src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			v = (v << 8) | src[srcpos++];
			dst[dstpos++] = IKVM.Runtime.DoubleConverter.ToDouble(v, ref converter);
		}
	}
}

static class Java_java_io_ObjectOutputStream
{
	public static void floatsToBytes(float[] src, int srcpos, byte[] dst, int dstpos, int nfloats)
	{
		IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
		for (int i = 0; i < nfloats; i++)
		{
			int v = IKVM.Runtime.FloatConverter.ToInt(src[srcpos++], ref converter);
			dst[dstpos++] = (byte)(v >> 24);
			dst[dstpos++] = (byte)(v >> 16);
			dst[dstpos++] = (byte)(v >> 8);
			dst[dstpos++] = (byte)(v >> 0);
		}
	}

	public static void doublesToBytes(double[] src, int srcpos, byte[] dst, int dstpos, int ndoubles)
	{
		IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
		for (int i = 0; i < ndoubles; i++)
		{
			long v = IKVM.Runtime.DoubleConverter.ToLong(src[srcpos++], ref converter);
			dst[dstpos++] = (byte)(v >> 56);
			dst[dstpos++] = (byte)(v >> 48);
			dst[dstpos++] = (byte)(v >> 40);
			dst[dstpos++] = (byte)(v >> 32);
			dst[dstpos++] = (byte)(v >> 24);
			dst[dstpos++] = (byte)(v >> 16);
			dst[dstpos++] = (byte)(v >> 8);
			dst[dstpos++] = (byte)(v >> 0);
		}
	}
}

namespace IKVM.Internal
{
	public static class IOHelpers
	{
		public static void WriteByte(byte[] buf, int offset, byte value)
		{
			buf[offset] = value;
		}

		public static void WriteBoolean(byte[] buf, int offset, bool value)
		{
			buf[offset] = value ? (byte)1 : (byte)0;
		}

		public static void WriteChar(byte[] buf, int offset, char value)
		{
			buf[offset + 0] = (byte)(value >> 8);
			buf[offset + 1] = (byte)(value >> 0);
		}

		public static void WriteShort(byte[] buf, int offset, short value)
		{
			buf[offset + 0] = (byte)(value >> 8);
			buf[offset + 1] = (byte)(value >> 0);
		}

		public static void WriteInt(byte[] buf, int offset, int value)
		{
			buf[offset + 0] = (byte)(value >> 24);
			buf[offset + 1] = (byte)(value >> 16);
			buf[offset + 2] = (byte)(value >> 8);
			buf[offset + 3] = (byte)(value >> 0);
		}

		public static void WriteFloat(byte[] buf, int offset, float value)
		{
#if !FIRST_PASS
			java.io.Bits.putFloat(buf, offset, value);
#endif
		}

		public static void WriteLong(byte[] buf, int offset, long value)
		{
			WriteInt(buf, offset, (int)(value >> 32));
			WriteInt(buf, offset + 4, (int)value);
		}

		public static void WriteDouble(byte[] buf, int offset, double value)
		{
#if !FIRST_PASS
			java.io.Bits.putDouble(buf, offset, value);
#endif
		}

		public static byte ReadByte(byte[] buf, int offset)
		{
			return buf[offset];
		}

		public static bool ReadBoolean(byte[] buf, int offset)
		{
			return buf[offset] != 0;
		}

		public static char ReadChar(byte[] buf, int offset)
		{
			return (char)((buf[offset] << 8) + buf[offset + 1]);
		}

		public static short ReadShort(byte[] buf, int offset)
		{
			return (short)((buf[offset] << 8) + buf[offset + 1]);
		}

		public static int ReadInt(byte[] buf, int offset)
		{
			return (buf[offset + 0] << 24)
				 + (buf[offset + 1] << 16)
				 + (buf[offset + 2] << 8)
				 + (buf[offset + 3] << 0);
		}

		public static float ReadFloat(byte[] buf, int offset)
		{
#if FIRST_PASS
			return 0;
#else
			return java.lang.Float.intBitsToFloat(ReadInt(buf, offset));
#endif
		}

		public static long ReadLong(byte[] buf, int offset)
		{
			long hi = (uint)ReadInt(buf, offset);
			long lo = (uint)ReadInt(buf, offset + 4);
			return lo + (hi << 32);
		}

		public static double ReadDouble(byte[] buf, int offset)
		{
#if FIRST_PASS
			return 0;
#else
			return java.lang.Double.longBitsToDouble(ReadLong(buf, offset));
#endif
		}
	}
}

static class Java_java_io_ObjectStreamClass
{
	public static void initNative()
	{
	}

	public static bool isDynamicTypeWrapper(java.lang.Class cl)
	{
		TypeWrapper wrapper = TypeWrapper.FromClass(cl);
		return !wrapper.IsFastClassLiteralSafe;
	}

	public static bool hasStaticInitializer(java.lang.Class cl)
	{
		TypeWrapper wrapper = TypeWrapper.FromClass(cl);
		try
		{
			wrapper.Finish();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		Type type = wrapper.TypeAsTBD;
		if (!type.IsArray && type.TypeInitializer != null)
		{
			wrapper.RunClassInit();
			return !AttributeHelper.IsHideFromJava(type.TypeInitializer);
		}
		return false;
	}

#if !FIRST_PASS && !NO_REF_EMIT
	private sealed class FastFieldReflector : ikvm.@internal.FieldReflectorBase
	{
		private static readonly MethodInfo ReadByteMethod = typeof(IOHelpers).GetMethod("ReadByte");
		private static readonly MethodInfo ReadBooleanMethod = typeof(IOHelpers).GetMethod("ReadBoolean");
		private static readonly MethodInfo ReadCharMethod = typeof(IOHelpers).GetMethod("ReadChar");
		private static readonly MethodInfo ReadShortMethod = typeof(IOHelpers).GetMethod("ReadShort");
		private static readonly MethodInfo ReadIntMethod = typeof(IOHelpers).GetMethod("ReadInt");
		private static readonly MethodInfo ReadFloatMethod = typeof(IOHelpers).GetMethod("ReadFloat");
		private static readonly MethodInfo ReadLongMethod = typeof(IOHelpers).GetMethod("ReadLong");
		private static readonly MethodInfo ReadDoubleMethod = typeof(IOHelpers).GetMethod("ReadDouble");
		private static readonly MethodInfo WriteByteMethod = typeof(IOHelpers).GetMethod("WriteByte");
		private static readonly MethodInfo WriteBooleanMethod = typeof(IOHelpers).GetMethod("WriteBoolean");
		private static readonly MethodInfo WriteCharMethod = typeof(IOHelpers).GetMethod("WriteChar");
		private static readonly MethodInfo WriteShortMethod = typeof(IOHelpers).GetMethod("WriteShort");
		private static readonly MethodInfo WriteIntMethod = typeof(IOHelpers).GetMethod("WriteInt");
		private static readonly MethodInfo WriteFloatMethod = typeof(IOHelpers).GetMethod("WriteFloat");
		private static readonly MethodInfo WriteLongMethod = typeof(IOHelpers).GetMethod("WriteLong");
		private static readonly MethodInfo WriteDoubleMethod = typeof(IOHelpers).GetMethod("WriteDouble");
		private delegate void ObjFieldGetterSetter(object obj, object[] objarr);
		private delegate void PrimFieldGetterSetter(object obj, byte[] objarr);
		private static readonly ObjFieldGetterSetter objDummy = new ObjFieldGetterSetter(Dummy);
		private static readonly PrimFieldGetterSetter primDummy = new PrimFieldGetterSetter(Dummy);
		private java.io.ObjectStreamField[] fields;
		private ObjFieldGetterSetter objFieldGetter;
		private PrimFieldGetterSetter primFieldGetter;
		private ObjFieldGetterSetter objFieldSetter;
		private PrimFieldGetterSetter primFieldSetter;

		private static void Dummy(object obj, object[] objarr)
		{
		}

		private static void Dummy(object obj, byte[] barr)
		{
		}

		internal FastFieldReflector(java.io.ObjectStreamField[] fields)
		{
			this.fields = fields;
			TypeWrapper tw = null;
			foreach (java.io.ObjectStreamField field in fields)
			{
				FieldWrapper fw = GetFieldWrapper(field);
				if (fw != null)
				{
					if (tw == null)
					{
						tw = fw.DeclaringType;
					}
					else if (tw != fw.DeclaringType)
					{
						// pre-condition is that all fields are from the same Type!
						throw new java.lang.InternalError();
					}
				}
			}
			if (tw == null)
			{
				objFieldGetter = objFieldSetter = objDummy;
				primFieldGetter = primFieldSetter = primDummy;
			}
			else
			{
				try
				{
					tw.Finish();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				DynamicMethod dmObjGetter = DynamicMethodUtils.Create("__<ObjFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
				DynamicMethod dmPrimGetter = DynamicMethodUtils.Create("__<PrimFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
				DynamicMethod dmObjSetter = DynamicMethodUtils.Create("__<ObjFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
				DynamicMethod dmPrimSetter = DynamicMethodUtils.Create("__<PrimFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
				CodeEmitter ilgenObjGetter = CodeEmitter.Create(dmObjGetter);
				CodeEmitter ilgenPrimGetter = CodeEmitter.Create(dmPrimGetter);
				CodeEmitter ilgenObjSetter = CodeEmitter.Create(dmObjSetter);
				CodeEmitter ilgenPrimSetter = CodeEmitter.Create(dmPrimSetter);

				// we want the getters to be verifiable (because writeObject can be used from partial trust),
				// so we create a local to hold the properly typed object reference
				CodeEmitterLocal objGetterThis = ilgenObjGetter.DeclareLocal(tw.TypeAsBaseType);
				CodeEmitterLocal primGetterThis = ilgenPrimGetter.DeclareLocal(tw.TypeAsBaseType);
				ilgenObjGetter.Emit(OpCodes.Ldarg_0);
				ilgenObjGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
				ilgenObjGetter.Emit(OpCodes.Stloc, objGetterThis);
				ilgenPrimGetter.Emit(OpCodes.Ldarg_0);
				ilgenPrimGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
				ilgenPrimGetter.Emit(OpCodes.Stloc, primGetterThis);

				foreach (java.io.ObjectStreamField field in fields)
				{
					FieldWrapper fw = GetFieldWrapper(field);
					if (fw == null)
					{
						continue;
					}
					fw.ResolveField();
					TypeWrapper fieldType = fw.FieldTypeWrapper;
					try
					{
						fieldType = fieldType.EnsureLoadable(tw.GetClassLoader());
						fieldType.Finish();
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
					if (fieldType.IsPrimitive)
					{
						// Getter
						ilgenPrimGetter.Emit(OpCodes.Ldarg_1);
						ilgenPrimGetter.EmitLdc_I4(field.getOffset());
						ilgenPrimGetter.Emit(OpCodes.Ldloc, primGetterThis);
						fw.EmitGet(ilgenPrimGetter);
						if (fieldType == PrimitiveTypeWrapper.BYTE)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteByteMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.BOOLEAN)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteBooleanMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.CHAR)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteCharMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.SHORT)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteShortMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.INT)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteIntMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.FLOAT)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteFloatMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.LONG)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteLongMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.DOUBLE)
						{
							ilgenPrimGetter.Emit(OpCodes.Call, WriteDoubleMethod);
						}
						else
						{
							throw new java.lang.InternalError();
						}

						// Setter
						ilgenPrimSetter.Emit(OpCodes.Ldarg_0);
						ilgenPrimSetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
						ilgenPrimSetter.Emit(OpCodes.Ldarg_1);
						ilgenPrimSetter.EmitLdc_I4(field.getOffset());
						if (fieldType == PrimitiveTypeWrapper.BYTE)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadByteMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.BOOLEAN)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadBooleanMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.CHAR)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadCharMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.SHORT)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadShortMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.INT)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadIntMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.FLOAT)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadFloatMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.LONG)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadLongMethod);
						}
						else if (fieldType == PrimitiveTypeWrapper.DOUBLE)
						{
							ilgenPrimSetter.Emit(OpCodes.Call, ReadDoubleMethod);
						}
						else
						{
							throw new java.lang.InternalError();
						}
						fw.EmitSet(ilgenPrimSetter);
					}
					else
					{
						// Getter
						ilgenObjGetter.Emit(OpCodes.Ldarg_1);
						ilgenObjGetter.EmitLdc_I4(field.getOffset());
						ilgenObjGetter.Emit(OpCodes.Ldloc, objGetterThis);
						fw.EmitGet(ilgenObjGetter);
						fieldType.EmitConvSignatureTypeToStackType(ilgenObjGetter);
						ilgenObjGetter.Emit(OpCodes.Stelem_Ref);

						// Setter
						ilgenObjSetter.Emit(OpCodes.Ldarg_0);
						ilgenObjSetter.Emit(OpCodes.Ldarg_1);
						ilgenObjSetter.EmitLdc_I4(field.getOffset());
						ilgenObjSetter.Emit(OpCodes.Ldelem_Ref);
						fieldType.EmitCheckcast(ilgenObjSetter);
						fieldType.EmitConvStackTypeToSignatureType(ilgenObjSetter, null);
						fw.EmitSet(ilgenObjSetter);
					}
				}
				ilgenObjGetter.Emit(OpCodes.Ret);
				ilgenPrimGetter.Emit(OpCodes.Ret);
				ilgenObjSetter.Emit(OpCodes.Ret);
				ilgenPrimSetter.Emit(OpCodes.Ret);
				ilgenObjGetter.DoEmit();
				ilgenPrimGetter.DoEmit();
				ilgenObjSetter.DoEmit();
				ilgenPrimSetter.DoEmit();
				objFieldGetter = (ObjFieldGetterSetter)dmObjGetter.CreateDelegate(typeof(ObjFieldGetterSetter));
				primFieldGetter = (PrimFieldGetterSetter)dmPrimGetter.CreateDelegate(typeof(PrimFieldGetterSetter));
				objFieldSetter = (ObjFieldGetterSetter)dmObjSetter.CreateDelegate(typeof(ObjFieldGetterSetter));
				primFieldSetter = (PrimFieldGetterSetter)dmPrimSetter.CreateDelegate(typeof(PrimFieldGetterSetter));
			}
		}

		private static FieldWrapper GetFieldWrapper(java.io.ObjectStreamField field)
		{
			java.lang.reflect.Field f = field.getField();
			return f == null ? null : FieldWrapper.FromField(f);
		}

		public override java.io.ObjectStreamField[] getFields()
		{
			return fields;
		}

		public override void getObjFieldValues(object obj, object[] objarr)
		{
			objFieldGetter(obj, objarr);
		}

		public override void setObjFieldValues(object obj, object[] objarr)
		{
			objFieldSetter(obj, objarr);
		}

		public override void getPrimFieldValues(object obj, byte[] barr)
		{
			primFieldGetter(obj, barr);
		}

		public override void setPrimFieldValues(object obj, byte[] barr)
		{
			primFieldSetter(obj, barr);
		}
	}
#endif // !FIRST_PASS && !NO_REF_EMIT

	public static object getFastFieldReflector(java.io.ObjectStreamField[] fieldsObj)
	{
#if FIRST_PASS || NO_REF_EMIT
		return null;
#else
		return new FastFieldReflector(fieldsObj);
#endif
	}
}

static class Java_java_io_RandomAccessFile
{
	public static void open0(object _this, string name, int mode, [In] java.io.FileDescriptor fd, [In] int O_RDWR)
	{
#if !FIRST_PASS
		if ((mode & O_RDWR) == O_RDWR)
		{
			fd.openReadWrite(name);
		}
		else
		{
			fd.openReadOnly(name);
		}
#endif
	}

	public static int read0(object _this, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.read();
#endif
	}

	public static int readBytes(object _this, byte[] b, int off, int len, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.readBytes(b, off, len);
#endif
	}

	public static void write0(object _this, int b, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.write(b);
#endif
	}

	public static void writeBytes(object _this, byte[] b, int off, int len, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.writeBytes(b, off, len);
#endif
	}

	public static long getFilePointer(object _this, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.getFilePointer();
#endif
	}

	public static void seek0(object _this, long pos, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.seek(pos);
#endif
	}

	public static long length(object _this, [In] java.io.FileDescriptor fd)
	{
#if FIRST_PASS
		return 0;
#else
		return fd.length();
#endif
	}

	public static void setLength(object _this, long newLength, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.setLength(newLength);
#endif
	}

	public static void close0(object _this, [In] java.io.FileDescriptor fd)
	{
#if !FIRST_PASS
		fd.close();
#endif
	}

	public static void initIDs()
	{
	}
}

static class Java_java_io_WinNTFileSystem
{
	internal const int ACCESS_READ = 0x04;
	const int ACCESS_WRITE = 0x02;
	const int ACCESS_EXECUTE = 0x01;

	public static string getDriveDirectory(object _this, int drive)
	{
		try
		{
			string path = ((char)('A' + (drive - 1))) + ":";
			return Path.GetFullPath(path).Substring(2);
		}
		catch (ArgumentException)
		{
		}
		catch (SecurityException)
		{
		}
		catch (PathTooLongException)
		{
		}
		return "\\";
	}

	private static string CanonicalizePath(string path)
	{
		try
		{
			FileInfo fi = new FileInfo(path);
			if (fi.DirectoryName == null)
			{
				return path.Length > 1 && path[1] == ':'
					? (Char.ToUpper(path[0]) + ":" + Path.DirectorySeparatorChar)
					: path;
			}
			string dir = CanonicalizePath(fi.DirectoryName);
			string name = fi.Name;
			try
			{
				if (!VirtualFileSystem.IsVirtualFS(path))
				{
					string[] arr = Directory.GetFileSystemEntries(dir, name);
					if (arr.Length == 1)
					{
						name = arr[0];
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return Path.Combine(dir, name);
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (SecurityException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return path;
	}

	public static string canonicalize0(object _this, string path)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			// TODO there is still a known bug here. A dotted path component right after the root component
			// are not removed as they should be. E.g. "c:\..." => "C:\..." or "\\server\..." => IOException
			// Another know issue is that when running under Mono on Windows, the case names aren't converted
			// to the correct (on file system) casing.
			//
			// FXBUG we're appending the directory separator to work around an apparent .NET bug.
			// If we don't do this, "c:\j\." would be canonicalized to "C:\"
			int colon = path.IndexOf(':', 2);
			if (colon != -1)
			{
				return CanonicalizePath(path.Substring(0, colon) + Path.DirectorySeparatorChar) + path.Substring(colon);
			}
			return CanonicalizePath(path + Path.DirectorySeparatorChar);
		}
		catch (ArgumentException x)
		{
			throw new java.io.IOException(x.Message);
		}
#endif
	}

	public static string canonicalizeWithPrefix0(object _this, string canonicalPrefix, string pathWithCanonicalPrefix)
	{
		return canonicalize0(_this, pathWithCanonicalPrefix);
	}

	private static string GetPathFromFile(java.io.File file)
	{
#if FIRST_PASS
		return null;
#else
		return file.getPath();
#endif
	}

	public static int getBooleanAttributes(object _this, java.io.File f)
	{
		try
		{
			string path = GetPathFromFile(f);
			if (VirtualFileSystem.IsVirtualFS(path))
			{
				return VirtualFileSystem.GetBooleanAttributes(path);
			}
			FileAttributes attr = File.GetAttributes(path);
			const int BA_EXISTS = 0x01;
			const int BA_REGULAR = 0x02;
			const int BA_DIRECTORY = 0x04;
			const int BA_HIDDEN = 0x08;
			int rv = BA_EXISTS;
			if ((attr & FileAttributes.Directory) != 0)
			{
				rv |= BA_DIRECTORY;
			}
			else
			{
				rv |= BA_REGULAR;
			}
			if ((attr & FileAttributes.Hidden) != 0)
			{
				rv |= BA_HIDDEN;
			}
			return rv;
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (SecurityException)
		{
		}
		catch (NotSupportedException)
		{
		}
		catch (IOException)
		{
		}
		return 0;
	}

	public static bool checkAccess(object _this, java.io.File f, int access)
	{
		string path = GetPathFromFile(f);
		if (VirtualFileSystem.IsVirtualFS(path))
		{
			return VirtualFileSystem.CheckAccess(path, access);
		}
		bool ok = true;
		if ((access & (ACCESS_READ | ACCESS_EXECUTE)) != 0)
		{
			ok = false;
			try
			{
				// HACK if path refers to a directory, we always return true
				if (!Directory.Exists(path))
				{
					new FileInfo(path).Open(
						FileMode.Open,
						FileAccess.Read,
						FileShare.ReadWrite).Close();
				}
				ok = true;
			}
			catch (SecurityException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}
		if (ok && ((access & ACCESS_WRITE) != 0))
		{
			ok = false;
			try
			{
				// HACK if path refers to a directory, we always return true
				if (Directory.Exists(path))
				{
					ok = true;
				}
				else
				{
					FileInfo fileInfo = new FileInfo(path);
					// Like the JDK we'll only look at the read-only attribute and not
					// the security permissions associated with the file or directory.
					ok = (fileInfo.Attributes & FileAttributes.ReadOnly) == 0;
				}
			}
			catch (SecurityException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}
		return ok;
	}

	private static long DateTimeToJavaLongTime(DateTime datetime)
	{
		return (TimeZone.CurrentTimeZone.ToUniversalTime(datetime) - new DateTime(1970, 1, 1)).Ticks / 10000L;
	}

	private static DateTime JavaLongTimeToDateTime(long datetime)
	{
		return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(new DateTime(1970, 1, 1).Ticks + datetime * 10000L));
	}

	public static long getLastModifiedTime(object _this, java.io.File f)
	{
		try
		{
			DateTime dt = File.GetLastWriteTime(GetPathFromFile(f));
			if (dt.ToFileTime() == 0)
			{
				return 0;
			}
			else
			{
				return DateTimeToJavaLongTime(dt);
			}
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return 0;
	}

	public static long getLength(object _this, java.io.File f)
	{
		try
		{
			string path = GetPathFromFile(f);
			if (VirtualFileSystem.IsVirtualFS(path))
			{
				return VirtualFileSystem.GetLength(path);
			}
			return new FileInfo(path).Length;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return 0;
	}

	public static bool setPermission(object _this, java.io.File f, int access, bool enable, bool owneronly)
	{
		if ((access & ACCESS_WRITE) != 0)
		{
			try
			{
				FileInfo file = new FileInfo(GetPathFromFile(f));
				if (enable)
				{
					file.Attributes &= ~FileAttributes.ReadOnly;
				}
				else
				{
					file.Attributes |= FileAttributes.ReadOnly;
				}
				return true;
			}
			catch (SecurityException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			catch (NotSupportedException)
			{
			}
			return false;
		}
		return enable;
	}

	public static bool createFileExclusively(object _this, string path)
	{
#if !FIRST_PASS
		try
		{
			File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None).Close();
			return true;
		}
		catch (ArgumentException x)
		{
			throw new java.io.IOException(x.Message);
		}
		catch (IOException x)
		{
			if (!File.Exists(path) && !Directory.Exists(path))
			{
				throw new java.io.IOException(x.Message);
			}
		}
		catch (UnauthorizedAccessException x)
		{
			if (!File.Exists(path) && !Directory.Exists(path))
			{
				throw new java.io.IOException(x.Message);
			}
		}
		catch (NotSupportedException x)
		{
			throw new java.io.IOException(x.Message);
		}
#endif
		return false;
	}

	public static bool delete0(object _this, java.io.File f)
	{
		FileSystemInfo fileInfo = null;
		try
		{
			string path = GetPathFromFile(f);
			if (Directory.Exists(path))
			{
				fileInfo = new DirectoryInfo(path);
			}
			else if (File.Exists(path))
			{
				fileInfo = new FileInfo(path);
			}
			else
			{
				return false;
			}
			// We need to be able to delete read-only files/dirs too, so we clear
			// the read-only attribute, if set.
			if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
			{
				fileInfo.Attributes &= ~FileAttributes.ReadOnly;
			}
			fileInfo.Delete();
			return true;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return false;
	}

	public static string[] list(object _this, java.io.File f)
	{
		try
		{
			string path = GetPathFromFile(f);
			if (VirtualFileSystem.IsVirtualFS(path))
			{
				return VirtualFileSystem.List(path);
			}
			string[] l = Directory.GetFileSystemEntries(path);
			for (int i = 0; i < l.Length; i++)
			{
				int pos = l[i].LastIndexOf(Path.DirectorySeparatorChar);
				if (pos >= 0)
				{
					l[i] = l[i].Substring(pos + 1);
				}
			}
			return l;
		}
		catch (ArgumentException)
		{
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return null;
	}

	public static bool createDirectory(object _this, java.io.File f)
	{
		try
		{
			string path = GetPathFromFile(f);
			DirectoryInfo parent = Directory.GetParent(path);
			if (parent == null ||
				!Directory.Exists(parent.FullName) ||
				Directory.Exists(path))
			{
				return false;
			}
			return Directory.CreateDirectory(path) != null;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return false;
	}

	public static bool rename0(object _this, java.io.File f1, java.io.File f2)
	{
		try
		{
			new FileInfo(GetPathFromFile(f1)).MoveTo(GetPathFromFile(f2));
			return true;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return false;
	}

	public static bool setLastModifiedTime(object _this, java.io.File f, long time)
	{
		try
		{
			new FileInfo(GetPathFromFile(f)).LastWriteTime = JavaLongTimeToDateTime(time);
			return true;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return false;
	}

	public static bool setReadOnly(object _this, java.io.File f)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(GetPathFromFile(f));
			fileInfo.Attributes |= FileAttributes.ReadOnly;
			return true;
		}
		catch (SecurityException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return false;
	}

	public static int listRoots0()
	{
		try
		{
			int drives = 0;
			foreach (string drive in Environment.GetLogicalDrives())
			{
				char c = Char.ToUpper(drive[0]);
				drives |= 1 << (c - 'A');
			}
			return drives;
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (SecurityException)
		{
		}
		return 0;
	}

	[SecuritySafeCritical]
	public static long getSpace0(object _this, java.io.File f, int t)
	{
#if !FIRST_PASS
		long freeAvailable;
		long total;
		long totalFree;
		StringBuilder volname = new StringBuilder(256);
		if (GetVolumePathName(GetPathFromFile(f), volname, volname.Capacity) != 0
			&& GetDiskFreeSpaceEx(volname.ToString(), out freeAvailable, out total, out totalFree) != 0)
		{
			switch (t)
			{
				case java.io.FileSystem.SPACE_TOTAL:
					return total;
				case java.io.FileSystem.SPACE_FREE:
					return totalFree;
				case java.io.FileSystem.SPACE_USABLE:
					return freeAvailable;
			}
		}
#endif
		return 0;
	}

	[DllImport("kernel32")]
	private static extern int GetDiskFreeSpaceEx(string directory, out long freeAvailable, out long total, out long totalFree);

	[DllImport("kernel32")]
	private static extern int GetVolumePathName(string lpszFileName, [In, Out] StringBuilder lpszVolumePathName, int cchBufferLength);

	public static void initIDs()
	{
	}
}

static class Java_java_io_UnixFileSystem
{
	public static int getBooleanAttributes0(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.getBooleanAttributes(_this, f);
	}

	public static long getSpace(object _this, java.io.File f, int t)
	{
		// TODO
		return 0;
	}

	public static string canonicalize0(object _this, string path)
	{
		return Java_java_io_WinNTFileSystem.canonicalize0(_this, path);
	}

	public static bool checkAccess(object _this, java.io.File f, int access)
	{
		return Java_java_io_WinNTFileSystem.checkAccess(_this, f, access);
	}

	public static long getLastModifiedTime(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.getLastModifiedTime(_this, f);
	}

	public static long getLength(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.getLength(_this, f);
	}

	public static bool setPermission(object _this, java.io.File f, int access, bool enable, bool owneronly)
	{
		// TODO consider using Mono.Posix
		return Java_java_io_WinNTFileSystem.setPermission(_this, f, access, enable, owneronly);
	}

	public static bool createFileExclusively(object _this, string path)
	{
		return Java_java_io_WinNTFileSystem.createFileExclusively(_this, path);
	}

	public static bool delete0(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.delete0(_this, f);
	}

	public static string[] list(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.list(_this, f);
	}

	public static bool createDirectory(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.createDirectory(_this, f);
	}

	public static bool rename0(object _this, java.io.File f1, java.io.File f2)
	{
		return Java_java_io_WinNTFileSystem.rename0(_this, f1, f2);
	}

	public static bool setLastModifiedTime(object _this, java.io.File f, long time)
	{
		return Java_java_io_WinNTFileSystem.setLastModifiedTime(_this, f, time);
	}

	public static bool setReadOnly(object _this, java.io.File f)
	{
		return Java_java_io_WinNTFileSystem.setReadOnly(_this, f);
	}

	public static void initIDs()
	{
	}
}
