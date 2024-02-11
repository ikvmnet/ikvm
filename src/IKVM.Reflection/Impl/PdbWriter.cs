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

    sealed class PdbWriter : ISymbolWriterImpl, IMetaDataEmit, IMetaDataImport
    {

        private readonly ModuleBuilder moduleBuilder;
        private ISymUnmanagedWriter2 symUnmanagedWriter;
        private readonly Dictionary<string, Document> documents = new Dictionary<string, Document>();
        private readonly List<Method> methods = new List<Method>();
        private readonly Dictionary<int, int> remap = new Dictionary<int, int>();
        private readonly Dictionary<int, int> reversemap = new Dictionary<int, int>();
        private readonly Dictionary<int, MethodBase> methodMap = new Dictionary<int, MethodBase>();
        private Method currentMethod;

        internal PdbWriter(ModuleBuilder moduleBuilder)
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

            internal ISymUnmanagedDocumentWriter GetUnmanagedDocument(ISymUnmanagedWriter2 symUnmanagedWriter)
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
            internal readonly FieldAttributes attributes;
            internal readonly int signature;
            internal readonly SymAddressKind addrKind;
            internal readonly int addr1;
            internal readonly int addr2;
            internal readonly int addr3;
            internal readonly int startOffset;
            internal readonly int endOffset;

            internal LocalVar(FieldAttributes attributes, int signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
            {
                this.attributes = attributes;
                this.signature = signature;
                this.addrKind = addrKind;
                this.addr1 = addr1;
                this.addr2 = addr2;
                this.addr3 = addr3;
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

            internal void Do(ISymUnmanagedWriter2 symUnmanagedWriter)
            {
                symUnmanagedWriter.OpenScope(startOffset);
                foreach (KeyValuePair<string, LocalVar> kv in locals)
                {
                    symUnmanagedWriter.DefineLocalVariable2(kv.Key, (int)kv.Value.attributes, kv.Value.signature, (int)kv.Value.addrKind, kv.Value.addr1, kv.Value.addr2, kv.Value.addr3, kv.Value.startOffset, kv.Value.endOffset);
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
            throw new NotImplementedException();
        }

        public void OpenMethod(SymbolToken method, MethodBase mb)
        {
            int token = method.GetToken();
            currentMethod = new Method(token);
            methodMap.Add(token, mb);
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

        public void DefineLocalVariable2(string name, FieldAttributes attributes, int signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
        {
            currentMethod.scopeStack.Peek().locals[name] = new LocalVar(attributes, signature, addrKind, addr1, addr2, addr3, startOffset, endOffset);
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
            reversemap.Add(newToken, oldToken);
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
            reversemap.Clear();
        }

        public void DefineLocalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
        {
            throw new NotImplementedException();
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

        public void PlaceHolder_CloseEnum()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_CountEnum()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_ResetEnum()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumTypeDefs()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumInterfaceImpls()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumTypeRefs()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindTypeDefByName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetScopeProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetModuleFromScope()
        {
            throw new NotImplementedException();
        }

        private static void WriteString(IntPtr ptrString, IntPtr ptrLength, string str, int length)
        {
            if (ptrString != IntPtr.Zero)
            {
                for (int i = 0; i < Math.Min(length, str.Length); i++)
                {
                    Marshal.WriteInt16(ptrString, i, str[i]);
                }
            }
            if (ptrLength != IntPtr.Zero)
            {
                Marshal.WriteInt32(ptrLength, str.Length);
            }
        }

        private static void WriteToken(IntPtr ptr, MemberInfo member)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.WriteInt32(ptr, member == null ? 0 : member.MetadataToken);
            }
        }

        private static void WriteInt32(IntPtr ptr, int value)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.WriteInt32(ptr, value);
            }
        }

        public void GetTypeDefProps(
            int td,                     // [IN] TypeDef token for inquiry.
            IntPtr szTypeDef,           // [OUT] Put name here.
            int cchTypeDef,             // [IN] size of name buffer in wide chars.
            IntPtr pchTypeDef,          // [OUT] put size of name (wide chars) here.
            IntPtr pdwTypeDefFlags,     // [OUT] Put flags here.
            IntPtr ptkExtends)          // [OUT] Put base class TypeDef/TypeRef here.
        {
            if (td == 0)
            {
                // why are we being called with an invalid token?
                WriteString(szTypeDef, pchTypeDef, "", cchTypeDef);
                WriteInt32(pdwTypeDefFlags, 0);
                WriteToken(ptkExtends, null);
            }
            else
            {
                Type type = moduleBuilder.ResolveType(td);
                WriteString(szTypeDef, pchTypeDef, type.FullName, cchTypeDef);
                WriteInt32(pdwTypeDefFlags, (int)type.Attributes);
                WriteToken(ptkExtends, type.BaseType);
            }
        }

        public void PlaceHolder_GetInterfaceImplProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetTypeRefProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_ResolveTypeRef()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMembers()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMembersWithName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMethods()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMethodsWithName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumFields()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumFieldsWithName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumParams()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMemberRefs()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMethodImpls()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumPermissionSets()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindMember()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindMethod()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindField()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindMemberRef()
        {
            throw new NotImplementedException();
        }

        public void GetMethodProps(
            int mb,                     // The method for which to get props.   
            IntPtr pClass,              // [OUT] Put method's class here. 
            IntPtr szMethod,            // [OUT] Put method's name here.  
            int cchMethod,              // Size of szMethod buffer in wide chars.   
            IntPtr pchMethod,           // [OUT] Put actual size here 
            IntPtr pdwAttr,             // [OUT] Put flags here.  
            IntPtr ppvSigBlob,          // [OUT] point to the blob value of meta data   
            IntPtr pcbSigBlob,          // [OUT] actual size of signature blob  
            IntPtr pulCodeRVA,          // [OUT] codeRVA    
            IntPtr pdwImplFlags)        // [OUT] Impl. Flags    
        {
            if (pdwAttr != IntPtr.Zero || ppvSigBlob != IntPtr.Zero || pcbSigBlob != IntPtr.Zero || pulCodeRVA != IntPtr.Zero || pdwImplFlags != IntPtr.Zero)
            {
                throw new NotImplementedException();
            }
            MethodBase method = methodMap[reversemap[mb]];
            WriteToken(pClass, method.DeclaringType);
            WriteString(szMethod, pchMethod, method.Name, cchMethod);
        }

        public void PlaceHolder_GetMemberRefProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumProperties()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumEvents()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetEventProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumMethodSemantics()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetMethodSemantics()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetClassLayout()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetFieldMarshal()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetRVA()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetPermissionSetProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetSigFromToken()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetModuleRefProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumModuleRefs()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetTypeSpecFromToken()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetNameFromToken()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumUnresolvedMethods()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetUserString()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetPinvokeMap()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumSignatures()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumTypeSpecs()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumUserStrings()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetParamForMethodIndex()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_EnumCustomAttributes()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetCustomAttributeProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_FindTypeRef()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetMemberProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetFieldProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetPropertyProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetParamProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetCustomAttributeByName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_IsValidToken()
        {
            throw new NotImplementedException();
        }

        public void GetNestedClassProps(
            int tdNestedClass,              // [IN] NestedClass token.
            IntPtr ptdEnclosingClass)       // [OUT] EnclosingClass token.
        {
            Type type = moduleBuilder.ResolveType(tdNestedClass);
            WriteToken(ptdEnclosingClass, type.DeclaringType);
        }

        public void PlaceHolder_GetNativeCallConvFromSig()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_IsGlobal()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetModuleProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_Save()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SaveToStream()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetSaveSize()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineTypeDef()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineNestedType()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetHandler()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineMethod()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineMethodImpl()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineTypeRefByName()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineImportType()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineMemberRef()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineImportMember()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineEvent()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetClassLayout()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DeleteClassLayout()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetFieldMarshal()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DeleteFieldMarshal()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefinePermissionSet()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetRVA()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetTokenFromSig()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineModuleRef()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetParent()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_GetTokenFromTypeSpec()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SaveToMemory()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineUserString()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DeleteToken()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetMethodProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetTypeDefProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetEventProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetPermissionSetProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefinePinvokeMap()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetPinvokeMap()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DeletePinvokeMap()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineCustomAttribute()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetCustomAttributeValue()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineField()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineProperty()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineParam()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetFieldProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetPropertyProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetParamProps()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_DefineSecurityAttributeSet()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_ApplyEditAndContinue()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_TranslateSigWithScope()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetMethodImplFlags()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_SetFieldRVA()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_Merge()
        {
            throw new NotImplementedException();
        }

        public void PlaceHolder_MergeEnd()
        {
            throw new NotImplementedException();
        }

        public bool IsDeterministic
        {
            get { return false; }
        }
    }

}

#endif
