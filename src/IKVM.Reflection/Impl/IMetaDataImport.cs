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

    [Guid("7dac8207-d3ae-4c75-9b67-92801a497d44")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    interface IMetaDataImport
    {
        void PlaceHolder_CloseEnum();
        void PlaceHolder_CountEnum();
        void PlaceHolder_ResetEnum();
        void PlaceHolder_EnumTypeDefs();
        void PlaceHolder_EnumInterfaceImpls();
        void PlaceHolder_EnumTypeRefs();
        void PlaceHolder_FindTypeDefByName();
        void PlaceHolder_GetScopeProps();
        void PlaceHolder_GetModuleFromScope();

        void GetTypeDefProps(
            int td,                     // [IN] TypeDef token for inquiry.
            IntPtr szTypeDef,              // [OUT] Put name here.
            int cchTypeDef,             // [IN] size of name buffer in wide chars.
            IntPtr pchTypeDef,              // [OUT] put size of name (wide chars) here.
            IntPtr pdwTypeDefFlags,     // [OUT] Put flags here.
            IntPtr ptkExtends);         // [OUT] Put base class TypeDef/TypeRef here.

        void PlaceHolder_GetInterfaceImplProps();
        void PlaceHolder_GetTypeRefProps();
        void PlaceHolder_ResolveTypeRef();
        void PlaceHolder_EnumMembers();
        void PlaceHolder_EnumMembersWithName();
        void PlaceHolder_EnumMethods();
        void PlaceHolder_EnumMethodsWithName();
        void PlaceHolder_EnumFields();
        void PlaceHolder_EnumFieldsWithName();
        void PlaceHolder_EnumParams();
        void PlaceHolder_EnumMemberRefs();
        void PlaceHolder_EnumMethodImpls();
        void PlaceHolder_EnumPermissionSets();
        void PlaceHolder_FindMember();
        void PlaceHolder_FindMethod();
        void PlaceHolder_FindField();
        void PlaceHolder_FindMemberRef();

        void GetMethodProps(
            int mb,                     // The method for which to get props.   
            IntPtr pClass,                  // Put method's class here. 
            IntPtr szMethod,               // Put method's name here.  
            int cchMethod,              // Size of szMethod buffer in wide chars.   
            IntPtr pchMethod,               // Put actual size here 
            IntPtr pdwAttr,             // Put flags here.  
            IntPtr ppvSigBlob,              // [OUT] point to the blob value of meta data   
            IntPtr pcbSigBlob,              // [OUT] actual size of signature blob  
            IntPtr pulCodeRVA,              // [OUT] codeRVA    
            IntPtr pdwImplFlags);           // [OUT] Impl. Flags    

        void PlaceHolder_GetMemberRefProps();
        void PlaceHolder_EnumProperties();
        void PlaceHolder_EnumEvents();
        void PlaceHolder_GetEventProps();
        void PlaceHolder_EnumMethodSemantics();
        void PlaceHolder_GetMethodSemantics();
        void PlaceHolder_GetClassLayout();
        void PlaceHolder_GetFieldMarshal();
        void PlaceHolder_GetRVA();
        void PlaceHolder_GetPermissionSetProps();
        void PlaceHolder_GetSigFromToken();
        void PlaceHolder_GetModuleRefProps();
        void PlaceHolder_EnumModuleRefs();
        void PlaceHolder_GetTypeSpecFromToken();
        void PlaceHolder_GetNameFromToken();
        void PlaceHolder_EnumUnresolvedMethods();
        void PlaceHolder_GetUserString();
        void PlaceHolder_GetPinvokeMap();
        void PlaceHolder_EnumSignatures();
        void PlaceHolder_EnumTypeSpecs();
        void PlaceHolder_EnumUserStrings();
        void PlaceHolder_GetParamForMethodIndex();
        void PlaceHolder_EnumCustomAttributes();
        void PlaceHolder_GetCustomAttributeProps();
        void PlaceHolder_FindTypeRef();
        void PlaceHolder_GetMemberProps();
        void PlaceHolder_GetFieldProps();
        void PlaceHolder_GetPropertyProps();
        void PlaceHolder_GetParamProps();
        void PlaceHolder_GetCustomAttributeByName();
        void PlaceHolder_IsValidToken();

        void GetNestedClassProps(
            int tdNestedClass,          // [IN] NestedClass token.
            IntPtr ptdEnclosingClass);      // [OUT] EnclosingClass token.

        void PlaceHolder_GetNativeCallConvFromSig();
        void PlaceHolder_IsGlobal();
    }

}

#endif
