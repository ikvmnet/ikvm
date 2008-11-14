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
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;

namespace IKVM.Reflection.Emit.Impl
{
	static class PdbSupport
	{
		[Guid("809c652e-7396-11d2-9771-00a0c9b4d50c")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CoClass(typeof(MetaDataDispenserClass))]
		[ComImport]
		private interface IMetaDataDispenser
		{
			void DefineScope(
				[In]  ref Guid rclsid,
				[In]  int dwCreateFlags,
				[In]  ref Guid riid,
				[Out, MarshalAs(UnmanagedType.IUnknown)] out object punk);
		}

		[Guid("e5cb7a31-7512-11d2-89ce-0080c792e5d8")]
		[ComImport]
		private class MetaDataDispenserClass { }

		[Guid("7dac8207-d3ae-4c75-9b67-92801a497d44")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IMetadataImport { }

		[Guid("ba3fee4c-ecb9-4e41-83b7-183fa41cd859")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IMetaDataEmit { }

		[Guid("ed14aa72-78e2-4884-84e2-334293ae5214")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		[CoClass(typeof(CorSymWriterClass))]
		private interface ISymUnmanagedWriter
		{
			void PlaceHolder_DefineDocument();
			void PlaceHolder_SetUserEntryPoint();
			void PlaceHolder_OpenMethod();
			void PlaceHolder_CloseMethod();
			void PlaceHolder_OpenScope();
			void PlaceHolder_CloseScope();
			void PlaceHolder_SetScopeRange();
			void PlaceHolder_DefineLocalVariable();
			void PlaceHolder_DefineParameter();
			void PlaceHolder_DefineField();
			void PlaceHolder_DefineGlobalVariable();
			void PlaceHolder_Close();
			void PlaceHolder_SetSymAttribute();
			void PlaceHolder_OpenNamespace();
			void PlaceHolder_CloseNamespace();
			void PlaceHolder_UsingNamespace();
			void PlaceHolder_SetMethodSourceRange();
			void PlaceHolder_Initialize();

			void GetDebugInfo(
				[In, Out] ref IMAGE_DEBUG_DIRECTORY pIDD,
				[In]  uint cData,
				[Out] out uint pcData,
				[Out, MarshalAs(UnmanagedType.LPArray)] byte[] data);

			void PlaceHolder_DefineSequencePoints();

			void RemapToken(
				[In] int oldToken,
				[In] int newToken);
		}

		[Guid("108296c1-281e-11d3-bd22-0000f80849bd")]
		[ComImport]
		private class CorSymWriterClass { }

		[StructLayout(LayoutKind.Sequential)]
		internal struct IMAGE_DEBUG_DIRECTORY
		{
			internal uint Characteristics;
			internal uint TimeDateStamp;
			internal ushort MajorVersion;
			internal ushort MinorVersion;
			internal uint Type;
			internal uint SizeOfData;
			internal uint AddressOfRawData;
			internal uint PointerToRawData;
		}

		private sealed class MySymWriter : SymWriter
		{
			private readonly IntPtr ppISymUnmanagedWriter = Marshal.AllocHGlobal(IntPtr.Size);
			private readonly ISymUnmanagedWriter symUnmanagedWriter = new ISymUnmanagedWriter();
			private readonly IntPtr pISymUnmanagedWriter;

			internal MySymWriter(string fileName)
			{
				pISymUnmanagedWriter = Marshal.GetComInterfaceForObject(symUnmanagedWriter, typeof(ISymUnmanagedWriter));
				Marshal.WriteIntPtr(ppISymUnmanagedWriter, pISymUnmanagedWriter);
				SetUnderlyingWriter(ppISymUnmanagedWriter);
				IMetaDataDispenser disp = new IMetaDataDispenser();
				object emitter;
				Guid CLSID_CorMetaDataRuntime = new Guid("005023ca-72b1-11d3-9fc4-00c04f79a0a3");
				Guid IID_IMetaDataEmit = typeof(IMetaDataEmit).GUID;
				disp.DefineScope(ref CLSID_CorMetaDataRuntime, 0, ref IID_IMetaDataEmit, out emitter);
				IntPtr emitterPtr = Marshal.GetComInterfaceForObject(emitter, typeof(IMetaDataEmit));
				try
				{
					Initialize(emitterPtr, fileName, true);
				}
				finally
				{
					Marshal.Release(emitterPtr);
				}
				Marshal.ReleaseComObject(disp);
				Marshal.ReleaseComObject(emitter);
			}

			~MySymWriter()
			{
				Marshal.Release(pISymUnmanagedWriter);
				Marshal.FreeHGlobal(ppISymUnmanagedWriter);
			}

			internal byte[] GetDebugInfo(ref IMAGE_DEBUG_DIRECTORY idd)
			{
				uint cData;
				symUnmanagedWriter.GetDebugInfo(ref idd, 0, out cData, null);
				byte[] buf = new byte[cData];
				symUnmanagedWriter.GetDebugInfo(ref idd, (uint)buf.Length, out cData, buf);
				return buf;
			}

			internal void RemapToken(int oldToken, int newToken)
			{
				symUnmanagedWriter.RemapToken(oldToken, newToken);
			}
		}

		internal static ISymbolWriter CreateSymbolWriter(string fileName)
		{
			return new MySymWriter(fileName);
		}

		internal static byte[] GetDebugInfo(ISymbolWriter writer, ref IMAGE_DEBUG_DIRECTORY idd)
		{
			return ((MySymWriter)writer).GetDebugInfo(ref idd);
		}

		internal static void RemapToken(ISymbolWriter writer, int oldToken, int newToken)
		{
			((MySymWriter)writer).RemapToken(oldToken, newToken);
		}
	}
}
