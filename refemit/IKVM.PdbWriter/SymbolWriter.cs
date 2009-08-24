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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;
using IKVM.Reflection.Emit.Impl;

namespace IKVM.Reflection.Emit.Impl
{
	[Guid("809c652e-7396-11d2-9771-00a0c9b4d50c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CoClass(typeof(MetaDataDispenserClass))]
	[ComImport]
	interface IMetaDataDispenser
	{
		void DefineScope(
			[In]  ref Guid rclsid,
			[In]  int dwCreateFlags,
			[In]  ref Guid riid,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object punk);
	}

	[Guid("e5cb7a31-7512-11d2-89ce-0080c792e5d8")]
	[ComImport]
	class MetaDataDispenserClass { }

	[Guid("7dac8207-d3ae-4c75-9b67-92801a497d44")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	interface IMetadataImport { }

	[Guid("ba3fee4c-ecb9-4e41-83b7-183fa41cd859")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	interface IMetaDataEmit { }

	[Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedDocumentWriter { }

	[Guid("ed14aa72-78e2-4884-84e2-334293ae5214")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	[CoClass(typeof(CorSymWriterClass))]
	interface ISymUnmanagedWriter
	{
		ISymUnmanagedDocumentWriter DefineDocument(string url, ref Guid language, ref Guid languageVendor, ref Guid documentType);
		void PlaceHolder_SetUserEntryPoint();
		void OpenMethod(int method);
		void CloseMethod();
		int OpenScope(int startOffset);
		void CloseScope(int endOffset);
		void PlaceHolder_SetScopeRange();
		void DefineLocalVariable(string name, int attributes, int cSig, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] signature, int addrKind, int addr1, int addr2, int startOffset, int endOffset);
		void PlaceHolder_DefineParameter();
		void PlaceHolder_DefineField();
		void PlaceHolder_DefineGlobalVariable();
		void Close();
		void PlaceHolder_SetSymAttribute();
		void PlaceHolder_OpenNamespace();
		void PlaceHolder_CloseNamespace();
		void PlaceHolder_UsingNamespace();
		void PlaceHolder_SetMethodSourceRange();
		void Initialize([MarshalAs(UnmanagedType.IUnknown)] object emitter, string filename, [MarshalAs(UnmanagedType.IUnknown)] object pIStream, bool fFullBuild);

		void GetDebugInfo(
			[In, Out] ref IMAGE_DEBUG_DIRECTORY pIDD,
			[In]  uint cData,
			[Out] out uint pcData,
			[Out, MarshalAs(UnmanagedType.LPArray)] byte[] data);

		void DefineSequencePoints(ISymUnmanagedDocumentWriter document, int spCount,
		  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] offsets,
		  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] lines,
		  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] columns,
		  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endLines,
		  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endColumns);

		void RemapToken(
			[In] int oldToken,
			[In] int newToken);
	}

	[Guid("108296c1-281e-11d3-bd22-0000f80849bd")]
	[ComImport]
	class CorSymWriterClass { }

	public sealed class SymbolWriter : ISymbolWriterImpl
	{
		private readonly ModuleBuilder moduleBuilder;
		private ISymUnmanagedWriter symUnmanagedWriter;
		private readonly Dictionary<string, Document> documents = new Dictionary<string, Document>();
		private readonly List<Method> methods = new List<Method>();
		private readonly Dictionary<int, int> remap = new Dictionary<int, int>();
		private Method currentMethod;

		public SymbolWriter(ModuleBuilder moduleBuilder)
		{
			this.moduleBuilder = moduleBuilder;
		}

		private sealed class Document : ISymbolDocumentWriter
		{
			internal readonly string url;
			private Guid language;
			private Guid languageVendor;
			private Guid documentType;
			private ISymUnmanagedDocumentWriter unmanagedDocument;

			internal Document(string url, Guid language, Guid languageVendor, Guid documentType)
			{
				this.url = url;
				this.language = language;
				this.languageVendor = languageVendor;
				this.documentType = documentType;
			}

			public void SetCheckSum(Guid algorithmId, byte[] checkSum)
			{
				throw new NotImplementedException();
			}

			public void SetSource(byte[] source)
			{
				throw new NotImplementedException();
			}

			internal ISymUnmanagedDocumentWriter GetUnmanagedDocument(ISymUnmanagedWriter symUnmanagedWriter)
			{
				if (unmanagedDocument == null)
				{
					unmanagedDocument = symUnmanagedWriter.DefineDocument(url, ref language, ref languageVendor, ref documentType);
				}
				return unmanagedDocument;
			}

			internal void Release()
			{
				if (unmanagedDocument != null)
				{
					Marshal.ReleaseComObject(unmanagedDocument);
					unmanagedDocument = null;
				}
			}
		}

		private sealed class LocalVar
		{
			internal readonly System.Reflection.FieldAttributes attributes;
			internal readonly byte[] signature;
			internal readonly SymAddressKind addrKind;
			internal readonly int addr1;
			internal readonly int addr2;
			internal readonly int startOffset;
			internal readonly int endOffset;

			internal LocalVar(System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int startOffset, int endOffset)
			{
				this.attributes = attributes;
				this.signature = signature;
				this.addrKind = addrKind;
				this.addr1 = addr1;
				this.addr2 = addr2;
				this.startOffset = startOffset;
				this.endOffset = endOffset;
			}
		}

		private sealed class Scope
		{
			internal readonly int startOffset;
			internal int endOffset;
			internal readonly List<Scope> scopes = new List<Scope>();
			internal readonly Dictionary<string, LocalVar> locals = new Dictionary<string, LocalVar>();

			internal Scope(int startOffset)
			{
				this.startOffset = startOffset;
			}

			internal void Do(ISymUnmanagedWriter symUnmanagedWriter)
			{
				symUnmanagedWriter.OpenScope(startOffset);
				foreach (KeyValuePair<string, LocalVar> kv in locals)
				{
					symUnmanagedWriter.DefineLocalVariable(kv.Key, (int)kv.Value.attributes, kv.Value.signature.Length, kv.Value.signature, (int)kv.Value.addrKind, kv.Value.addr1, kv.Value.addr2, kv.Value.startOffset, kv.Value.endOffset);
				}
				foreach (Scope scope in scopes)
				{
					scope.Do(symUnmanagedWriter);
				}
				symUnmanagedWriter.CloseScope(endOffset);
			}
		}

		private sealed class Method
		{
			internal readonly int token;
			internal Document document;
			internal int[] offsets;
			internal int[] lines;
			internal int[] columns;
			internal int[] endLines;
			internal int[] endColumns;
			internal readonly List<Scope> scopes = new List<Scope>();
			internal readonly Stack<Scope> scopeStack = new Stack<Scope>();

			internal Method(int token)
			{
				this.token = token;
			}
		}

		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			Document doc;
			if (!documents.TryGetValue(url, out doc))
			{
				doc = new Document(url, language, languageVendor, documentType);
				documents.Add(url, doc);
			}
			return doc;
		}

		public void OpenMethod(SymbolToken method)
		{
			currentMethod = new Method(method.GetToken());
		}

		public void CloseMethod()
		{
			methods.Add(currentMethod);
			currentMethod = null;
		}

		public void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			currentMethod.document = (Document)document;
			currentMethod.offsets = offsets;
			currentMethod.lines = lines;
			currentMethod.columns = columns;
			currentMethod.endLines = endLines;
			currentMethod.endColumns = endColumns;
		}

		public int OpenScope(int startOffset)
		{
			Scope scope = new Scope(startOffset);
			if (currentMethod.scopeStack.Count == 0)
			{
				currentMethod.scopes.Add(scope);
			}
			else
			{
				currentMethod.scopeStack.Peek().scopes.Add(scope);
			}
			currentMethod.scopeStack.Push(scope);
			return 0;
		}

		public void CloseScope(int endOffset)
		{
			currentMethod.scopeStack.Pop().endOffset = endOffset;
		}

		public void DefineLocalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			currentMethod.scopeStack.Peek().locals[name] = new LocalVar(attributes, signature, addrKind, addr1, addr2, startOffset, endOffset);
		}

		private void InitWriter()
		{
			if (symUnmanagedWriter == null)
			{
				IMetaDataDispenser disp = new IMetaDataDispenser();
				symUnmanagedWriter = new ISymUnmanagedWriter();
				string fileName = System.IO.Path.ChangeExtension(moduleBuilder.FullyQualifiedName, ".pdb");
				object emitter;
				Guid CLSID_CorMetaDataRuntime = new Guid("005023ca-72b1-11d3-9fc4-00c04f79a0a3");
				Guid IID_IMetaDataEmit = typeof(IMetaDataEmit).GUID;
				disp.DefineScope(ref CLSID_CorMetaDataRuntime, 0, ref IID_IMetaDataEmit, out emitter);
				symUnmanagedWriter.Initialize(emitter, fileName, null, true);
				Marshal.ReleaseComObject(disp);
			}
		}

		public byte[] GetDebugInfo(ref IMAGE_DEBUG_DIRECTORY idd)
		{
			InitWriter();
			uint cData;
			symUnmanagedWriter.GetDebugInfo(ref idd, 0, out cData, null);
			byte[] buf = new byte[cData];
			symUnmanagedWriter.GetDebugInfo(ref idd, (uint)buf.Length, out cData, buf);
			return buf;
		}

		public void RemapToken(int oldToken, int newToken)
		{
			remap.Add(oldToken, newToken);
		}

		public void Close()
		{
			InitWriter();

			foreach (Method method in methods)
			{
				int remappedToken = method.token;
				remap.TryGetValue(remappedToken, out remappedToken);
				symUnmanagedWriter.OpenMethod(remappedToken);
				if (method.document != null)
				{
					ISymUnmanagedDocumentWriter doc = method.document.GetUnmanagedDocument(symUnmanagedWriter);
					symUnmanagedWriter.DefineSequencePoints(doc, method.offsets.Length, method.offsets, method.lines, method.columns, method.endLines, method.endColumns);
				}
				foreach (Scope scope in method.scopes)
				{
					scope.Do(symUnmanagedWriter);
				}
				symUnmanagedWriter.CloseMethod(); 
			}

			foreach (Document doc in documents.Values)
			{
				doc.Release();
			}

			symUnmanagedWriter.Close();
			Marshal.ReleaseComObject(symUnmanagedWriter);
			symUnmanagedWriter = null;
			documents.Clear();
			methods.Clear();
			remap.Clear();
		}

		public void CloseNamespace()
		{
			throw new NotImplementedException();
		}

		public void DefineField(SymbolToken parent, string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
			throw new NotImplementedException();
		}

		public void DefineGlobalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
			throw new NotImplementedException();
		}

		public void DefineParameter(string name, System.Reflection.ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
			throw new NotImplementedException();
		}

		public void Initialize(IntPtr emitter, string filename, bool fFullBuild)
		{
			throw new NotImplementedException();
		}

		public void OpenNamespace(string name)
		{
			throw new NotImplementedException();
		}

		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
			throw new NotImplementedException();
		}

		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
			throw new NotImplementedException();
		}

		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
			throw new NotImplementedException();
		}

		public void SetUnderlyingWriter(IntPtr underlyingWriter)
		{
			throw new NotImplementedException();
		}

		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
			throw new NotImplementedException();
		}

		public void UsingNamespace(string fullName)
		{
			throw new NotImplementedException();
		}
	}
}
