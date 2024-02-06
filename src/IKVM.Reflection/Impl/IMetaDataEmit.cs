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

    [Guid("ba3fee4c-ecb9-4e41-83b7-183fa41cd859")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    interface IMetaDataEmit
    {
        void PlaceHolder_SetModuleProps();
        void PlaceHolder_Save();
        void PlaceHolder_SaveToStream();
        void PlaceHolder_GetSaveSize();
        void PlaceHolder_DefineTypeDef();
        void PlaceHolder_DefineNestedType();
        void PlaceHolder_SetHandler();
        void PlaceHolder_DefineMethod();
        void PlaceHolder_DefineMethodImpl();
        void PlaceHolder_DefineTypeRefByName();
        void PlaceHolder_DefineImportType();
        void PlaceHolder_DefineMemberRef();
        void PlaceHolder_DefineImportMember();
        void PlaceHolder_DefineEvent();
        void PlaceHolder_SetClassLayout();
        void PlaceHolder_DeleteClassLayout();
        void PlaceHolder_SetFieldMarshal();
        void PlaceHolder_DeleteFieldMarshal();
        void PlaceHolder_DefinePermissionSet();
        void PlaceHolder_SetRVA();
        void PlaceHolder_GetTokenFromSig();
        void PlaceHolder_DefineModuleRef();
        void PlaceHolder_SetParent();
        void PlaceHolder_GetTokenFromTypeSpec();
        void PlaceHolder_SaveToMemory();
        void PlaceHolder_DefineUserString();
        void PlaceHolder_DeleteToken();
        void PlaceHolder_SetMethodProps();
        void PlaceHolder_SetTypeDefProps();
        void PlaceHolder_SetEventProps();
        void PlaceHolder_SetPermissionSetProps();
        void PlaceHolder_DefinePinvokeMap();
        void PlaceHolder_SetPinvokeMap();
        void PlaceHolder_DeletePinvokeMap();
        void PlaceHolder_DefineCustomAttribute();
        void PlaceHolder_SetCustomAttributeValue();
        void PlaceHolder_DefineField();
        void PlaceHolder_DefineProperty();
        void PlaceHolder_DefineParam();
        void PlaceHolder_SetFieldProps();
        void PlaceHolder_SetPropertyProps();
        void PlaceHolder_SetParamProps();
        void PlaceHolder_DefineSecurityAttributeSet();
        void PlaceHolder_ApplyEditAndContinue();
        void PlaceHolder_TranslateSigWithScope();
        void PlaceHolder_SetMethodImplFlags();
        void PlaceHolder_SetFieldRVA();
        void PlaceHolder_Merge();
        void PlaceHolder_MergeEnd();
    }

}

#endif
