/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Metadata
{

    sealed class MethodSemanticsTable : SortedTable<MethodSemanticsTable.Record>
    {

        internal struct Record : IRecord
        {

            internal short Semantics;
            internal int Method;
            internal int Association;

            readonly int IRecord.SortKey => EncodeHasSemantics(Association);

            readonly int IRecord.FilterKey => Association;

        }

        internal const int Index = 0x18;

        // semantics
        internal const short Setter = 0x0001;
        internal const short Getter = 0x0002;
        internal const short Other = 0x0004;
        internal const short AddOn = 0x0008;
        internal const short RemoveOn = 0x0010;
        internal const short Fire = 0x0020;

        internal override void Read(Reader.MetadataReader mr)
        {
            for (int i = 0; i < records.Length; i++)
            {
                records[i].Semantics = mr.ReadInt16();
                records[i].Method = mr.ReadMethodDef();
                records[i].Association = mr.ReadHasSemantics();
            }
        }

        internal override void Write(ModuleBuilder module)
        {
            for (int i = 0; i < rowCount; i++)
                module.Metadata.AddMethodSemantics(
                    MetadataTokens.EntityHandle(records[i].Association),
                    (System.Reflection.MethodSemanticsAttributes)records[i].Semantics,
                    MetadataTokens.MethodDefinitionHandle(records[i].Method));
        }

        static internal int EncodeHasSemantics(int token) => (token >> 24) switch
        {
            EventTable.Index => (token & 0xFFFFFF) << 1 | 0,
            PropertyTable.Index => (token & 0xFFFFFF) << 1 | 1,
            _ => throw new InvalidOperationException(),
        };

        internal void Fixup(ModuleBuilder moduleBuilder)
        {
            for (int i = 0; i < rowCount; i++)
                moduleBuilder.FixupPseudoToken(ref records[i].Method);

            Sort();
        }

        internal MethodInfo GetMethod(Module module, int token, bool nonPublic, short semantics)
        {
            foreach (int i in Filter(token))
            {
                if ((records[i].Semantics & semantics) != 0)
                {
                    var method = module.ResolveMethod((MethodDefTable.Index << 24) + records[i].Method);
                    if (nonPublic || method.IsPublic)
                        return (MethodInfo)method;
                }
            }

            return null;
        }

        internal MethodInfo[] GetMethods(Module module, int token, bool nonPublic, short semantics)
        {
            var methods = new List<MethodInfo>();
            foreach (int i in Filter(token))
            {
                if ((records[i].Semantics & semantics) != 0)
                {
                    var method = (MethodInfo)module.ResolveMethod((MethodDefTable.Index << 24) + records[i].Method);
                    if (nonPublic || method.IsPublic)
                        methods.Add(method);
                }
            }

            return methods.ToArray();
        }

        internal void ComputeFlags(Module module, int token, out bool isPublic, out bool isNonPrivate, out bool isStatic)
        {
            isPublic = false;
            isNonPrivate = false;
            isStatic = false;

            foreach (int i in Filter(token))
            {
                var method = module.ResolveMethod((MethodDefTable.Index << 24) + records[i].Method);
                isPublic |= method.IsPublic;
                isNonPrivate |= (method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private;
                isStatic |= method.IsStatic;
            }
        }

    }

}
