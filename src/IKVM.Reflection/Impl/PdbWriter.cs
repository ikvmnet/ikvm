#if NETFRAMEWORK

/*
  Copyright (C) 2008-2010 Jeroen Frijters

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
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Impl
{

    sealed class PdbWriter : AbstractPdbWriter, ISymbolWriterImpl, IMetaDataEmit, IMetaDataImport
    {
        private ISymUnmanagedWriter2 symUnmanagedWriter;

        internal PdbWriter(ModuleBuilder moduleBuilder) : base(moduleBuilder)
        {
        }


        private ISymUnmanagedDocumentWriter GetUnmanagedDocument(Document document)
        {
            if (document.unmanagedDocument == null)
            {
                document.unmanagedDocument = symUnmanagedWriter.DefineDocument(document.url, ref document.language, ref document.languageVendor, ref document.documentType);
            }
            return document.unmanagedDocument as ISymUnmanagedDocumentWriter;
        }

        private void ReleaseDocument(Document document)
        {
            if (document.unmanagedDocument != null)
            {
                Marshal.ReleaseComObject(document.unmanagedDocument);
                document.unmanagedDocument = null;
            }
        }

        private void WriteScope(Scope scope)
        {
            symUnmanagedWriter.OpenScope(scope.startOffset);
            foreach (KeyValuePair<string, LocalVar> kv in scope.locals)
            {
                symUnmanagedWriter.DefineLocalVariable2(kv.Key, (int)kv.Value.attributes, kv.Value.signature, (int)kv.Value.addrKind, kv.Value.addr1, kv.Value.addr2, kv.Value.addr3, kv.Value.startOffset, kv.Value.endOffset);
            }
            foreach (Scope child in scope.scopes)
            {
                WriteScope(child);
            }
            symUnmanagedWriter.CloseScope(scope.endOffset);
        }

        private void InitWriter()
        {
            if (symUnmanagedWriter == null)
            {
                string fileName = System.IO.Path.ChangeExtension(moduleBuilder.FullyQualifiedName, ".pdb");
                // pro-actively delete the .pdb to get a meaningful IOException, instead of COMInteropException if the file can't be overwritten (or is corrupt, or who knows what)
                System.IO.File.Delete(fileName);
                symUnmanagedWriter = new ISymUnmanagedWriter2();
                symUnmanagedWriter.Initialize(this, fileName, null, true);
            }
        }

        public override byte[] GetDebugInfo(ref IMAGE_DEBUG_DIRECTORY idd)
        {
            InitWriter();
            uint cData;
            symUnmanagedWriter.GetDebugInfo(ref idd, 0, out cData, null);
            byte[] buf = new byte[cData];
            symUnmanagedWriter.GetDebugInfo(ref idd, (uint)buf.Length, out cData, buf);
            return buf;
        }

        public override void Close()
        {
            InitWriter();

            foreach (Method method in methods)
            {
                int remappedToken = method.token;
                remap.TryGetValue(remappedToken, out remappedToken);
                symUnmanagedWriter.OpenMethod(remappedToken);
                if (method.document != null)
                {
                    ISymUnmanagedDocumentWriter doc = GetUnmanagedDocument(method.document);
                    symUnmanagedWriter.DefineSequencePoints(doc, method.offsets.Length, method.offsets, method.lines, method.columns, method.endLines, method.endColumns);
                }
                foreach (Scope scope in method.scopes)
                {
                    WriteScope(scope);
                }
                symUnmanagedWriter.CloseMethod();
            }

            foreach (Document doc in documents.Values)
            {
                ReleaseDocument(doc);
            }

            symUnmanagedWriter.Close();
            Marshal.ReleaseComObject(symUnmanagedWriter);
            symUnmanagedWriter = null;
            documents.Clear();
            methods.Clear();
            remap.Clear();
            reversemap.Clear();
        }
    }

}

#endif
