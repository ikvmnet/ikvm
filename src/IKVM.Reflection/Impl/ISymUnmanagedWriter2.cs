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
using System.Runtime.InteropServices;

namespace IKVM.Reflection.Impl
{

    [Guid("0b97726e-9e6d-4f05-9a26-424022093caa")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [CoClass(typeof(CorSymWriterClass))]
    interface ISymUnmanagedWriter2
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
            [In] uint cData,
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

        void PlaceHolder_Initialize2();
        void PlaceHolder_DefineConstant();
        void PlaceHolder_Abort();

        void DefineLocalVariable2(string name, int attributes, int token, int addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset);

        void PlaceHolder_DefineGlobalVariable2();
        void PlaceHolder_DefineConstant2();
    }

}

#endif
