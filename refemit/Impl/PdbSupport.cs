/*
  Copyright (C) 2008, 2009 Jeroen Frijters

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
	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_DEBUG_DIRECTORY
	{
		public uint Characteristics;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public uint Type;
		public uint SizeOfData;
		public uint AddressOfRawData;
		public uint PointerToRawData;
	}

	public interface ISymbolWriterImpl : ISymbolWriter
	{
		byte[] GetDebugInfo(ref IMAGE_DEBUG_DIRECTORY idd);
		void RemapToken(int oldToken, int newToken);
		void DefineLocalVariable2(string name, System.Reflection.FieldAttributes attributes, int signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset);
	}

	static class PdbSupport
	{
		private static readonly Type symbolWriterType = GetSymbolWriterType();

		private static Type GetSymbolWriterType()
		{
			string symbolAssembly = Environment.GetEnvironmentVariable("IKVM_REFEMIT_SYMBOLWRITER_ASSEMBLY");
			if (symbolAssembly == null)
			{
				string baseAssembly = typeof(PdbSupport).Assembly.FullName;
				string suffix = RunningOnMono ? ".MdbWriter" : ".PdbWriter";
				symbolAssembly = baseAssembly.Insert(baseAssembly.IndexOf(','), suffix);
			}
			return Type.GetType("IKVM.Reflection.Emit.Impl.SymbolWriter, " + symbolAssembly, true);
		}

		private static bool RunningOnMono { get { return Type.GetType("Mono.Runtime") != null; } }

		internal static ISymbolWriterImpl CreateSymbolWriterFor(ModuleBuilder moduleBuilder)
		{
			return (ISymbolWriterImpl)Activator.CreateInstance(symbolWriterType, moduleBuilder);
		}

		internal static byte[] GetDebugInfo(ISymbolWriter writer, ref IMAGE_DEBUG_DIRECTORY idd)
		{
			return ((ISymbolWriterImpl)writer).GetDebugInfo(ref idd);
		}

		internal static void RemapToken(ISymbolWriter writer, int oldToken, int newToken)
		{
			((ISymbolWriterImpl)writer).RemapToken(oldToken, newToken);
		}
	}
}
