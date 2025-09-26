/*
  Copyright (C) 2009 Jeroen Frijters

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
namespace IKVM.Reflection.Metadata
{

    /// <summary>
	/// Base class for MetadataReader and MetadataWriter
	/// </summary>
    abstract class MetadataRW
    {

        internal readonly bool bigStrings;
        internal readonly bool bigGuids;
        internal readonly bool bigBlobs;
        internal readonly bool bigResolutionScope;
        internal readonly bool bigTypeDefOrRef;
        internal readonly bool bigMemberRefParent;
        internal readonly bool bigHasCustomAttribute;
        internal readonly bool bigCustomAttributeType;
        internal readonly bool bigMethodDefOrRef;
        internal readonly bool bigHasConstant;
        internal readonly bool bigHasSemantics;
        internal readonly bool bigHasFieldMarshal;
        internal readonly bool bigHasDeclSecurity;
        internal readonly bool bigTypeOrMethodDef;
        internal readonly bool bigMemberForwarded;
        internal readonly bool bigImplementation;
        internal readonly bool bigField;
        internal readonly bool bigMethodDef;
        internal readonly bool bigParam;
        internal readonly bool bigTypeDef;
        internal readonly bool bigProperty;
        internal readonly bool bigEvent;
        internal readonly bool bigGenericParam;
        internal readonly bool bigModuleRef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="bigStrings"></param>
        /// <param name="bigGuids"></param>
        /// <param name="bigBlobs"></param>
        protected MetadataRW(Module module, bool bigStrings, bool bigGuids, bool bigBlobs)
        {
            this.bigStrings = bigStrings;
            this.bigGuids = bigGuids;
            this.bigBlobs = bigBlobs;
            this.bigField = module.FieldTable.IsBig;
            this.bigMethodDef = module.MethodDefTable.IsBig;
            this.bigParam = module.ParamTable.IsBig;
            this.bigTypeDef = module.TypeDefTable.IsBig;
            this.bigProperty = module.PropertyTable.IsBig;
            this.bigEvent = module.EventTable.IsBig;
            this.bigGenericParam = module.GenericParamTable.IsBig;
            this.bigModuleRef = module.ModuleRefTable.IsBig;
            this.bigResolutionScope = IsBig(2, module.ModuleTable, module.ModuleRefTable, module.AssemblyRefTable, module.TypeRefTable);
            this.bigTypeDefOrRef = IsBig(2, module.TypeDefTable, module.TypeRefTable, module.TypeSpecTable);
            this.bigMemberRefParent = IsBig(3, module.TypeDefTable, module.TypeRefTable, module.ModuleRefTable, module.MethodDefTable, module.TypeSpecTable);
            this.bigMethodDefOrRef = IsBig(1, module.MethodDefTable, module.MemberRefTable);
            this.bigHasCustomAttribute = IsBig(5, module.MethodDefTable, module.FieldTable, module.TypeRefTable, module.TypeDefTable, module.ParamTable, module.InterfaceImplTable, module.MemberRefTable, module.ModuleTable, module.DeclSecurityTable, module.PropertyTable, module.EventTable, module.StandAloneSigTable, module.ModuleRefTable, module.TypeSpecTable, module.AssemblyTable, module.AssemblyRefTable, module.FileTable, module.ExportedTypeTable, module.ManifestResourceTable, module.GenericParamTable, module.GenericParamConstraint, module.MethodSpecTable);
            this.bigCustomAttributeType = IsBig(3, module.MethodDefTable, module.MemberRefTable);
            this.bigHasConstant = IsBig(2, module.FieldTable, module.ParamTable, module.PropertyTable);
            this.bigHasSemantics = IsBig(1, module.EventTable, module.PropertyTable);
            this.bigHasFieldMarshal = IsBig(1, module.FieldTable, module.ParamTable);
            this.bigHasDeclSecurity = IsBig(2, module.TypeDefTable, module.MethodDefTable, module.AssemblyTable);
            this.bigTypeOrMethodDef = IsBig(1, module.TypeDefTable, module.MethodDefTable);
            this.bigMemberForwarded = IsBig(1, module.FieldTable, module.MethodDefTable);
            this.bigImplementation = IsBig(2, module.FileTable, module.AssemblyRefTable, module.ExportedTypeTable);
        }

        static bool IsBig(int bitsUsed, params Table[] tables)
        {
            var limit = 1 << (16 - bitsUsed);
            foreach (var table in tables)
                if (table.RowCount >= limit)
                    return true;

            return false;
        }

    }

}
