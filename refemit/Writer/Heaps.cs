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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace IKVM.Reflection.Emit.Writer
{
	abstract class Heap
	{
		private bool frozen;
		private int unalignedlength;

		internal void Write(MetadataWriter mw)
		{
			int pos = mw.Position;
			WriteImpl(mw);
			Debug.Assert(mw.Position == pos + unalignedlength);
			int align = Length - unalignedlength;
			for (int i = 0; i < align; i++)
			{
				mw.Write((byte)0);
			}
		}

		internal void Freeze(ModuleBuilder md)
		{
			if (frozen)
				throw new InvalidOperationException();
			frozen = true;
			unalignedlength = GetLength(md);
		}

		internal bool IsBig
		{
			get { return Length > 65535; }
		}

		internal int Length
		{
			get
			{
				if (!frozen)
					throw new InvalidOperationException();
				return (unalignedlength + 3) & ~3;
			}
		}

		protected abstract void WriteImpl(MetadataWriter mw);
		protected abstract int GetLength(ModuleBuilder md);
	}

	sealed class TableHeap : Heap
	{
		internal readonly ModuleTable Module = new ModuleTable();
		internal readonly TypeRefTable TypeRef = new TypeRefTable();
		internal readonly TypeDefTable TypeDef = new TypeDefTable();
		internal readonly FieldTable Field = new FieldTable();
		internal readonly MemberRefTable MemberRef = new MemberRefTable();
		internal readonly ConstantTable Constant = new ConstantTable();
		internal readonly CustomAttributeTable CustomAttribute = new CustomAttributeTable();
		internal readonly FieldMarshalTable FieldMarshal = new FieldMarshalTable();
		internal readonly DeclSecurityTable DeclSecurity = new DeclSecurityTable();
		internal readonly ClassLayoutTable ClassLayout = new ClassLayoutTable();
		internal readonly FieldLayoutTable FieldLayout = new FieldLayoutTable();
		internal readonly ParamTable Param = new ParamTable();
		internal readonly InterfaceImplTable InterfaceImpl = new InterfaceImplTable();
		internal readonly StandAloneSigTable StandAloneSig = new StandAloneSigTable();
		internal readonly EventMapTable EventMap = new EventMapTable();
		internal readonly EventTable Event = new EventTable();
		internal readonly PropertyMapTable PropertyMap = new PropertyMapTable();
		internal readonly PropertyTable Property = new PropertyTable();
		internal readonly MethodSemanticsTable MethodSemantics = new MethodSemanticsTable();
		internal readonly MethodImplTable MethodImpl = new MethodImplTable();
		internal readonly ModuleRefTable ModuleRef = new ModuleRefTable();
		internal readonly TypeSpecTable TypeSpec = new TypeSpecTable();
		internal readonly ImplMapTable ImplMap = new ImplMapTable();
		internal readonly FieldRVATable FieldRVA = new FieldRVATable();
		internal readonly AssemblyTable Assembly = new AssemblyTable();
		internal readonly AssemblyRefTable AssemblyRef = new AssemblyRefTable();
		internal readonly MethodDefTable MethodDef = new MethodDefTable();
		internal readonly NestedClassTable NestedClass = new NestedClassTable();
		internal readonly FileTable File = new FileTable();
		internal readonly ExportedTypeTable ExportedType = new ExportedTypeTable();
		internal readonly ManifestResourceTable ManifestResource = new ManifestResourceTable();
		internal readonly GenericParamTable GenericParam = new GenericParamTable();
		internal readonly MethodSpecTable MethodSpec = new MethodSpecTable();
		internal readonly GenericParamConstraintTable GenericParamConstraint = new GenericParamConstraintTable();
		private readonly Table[] tables = new Table[64];

		internal TableHeap(ModuleBuilder moduleBuilder)
		{
			tables[ModuleTable.Index] = Module;
			tables[TypeRefTable.Index] = TypeRef;
			tables[TypeDefTable.Index] = TypeDef;
			tables[FieldTable.Index] = Field;
			tables[MemberRefTable.Index] = MemberRef;
			tables[ConstantTable.Index] = Constant;
			tables[CustomAttributeTable.Index] = CustomAttribute;
			tables[FieldMarshalTable.Index] = FieldMarshal;
			tables[DeclSecurityTable.Index] = DeclSecurity;
			tables[ClassLayoutTable.Index] = ClassLayout;
			tables[FieldLayoutTable.Index] = FieldLayout;
			tables[ParamTable.Index] = Param;
			tables[InterfaceImplTable.Index] = InterfaceImpl;
			tables[StandAloneSigTable.Index] = StandAloneSig;
			tables[EventMapTable.Index] = EventMap;
			tables[EventTable.Index] = Event;
			tables[PropertyMapTable.Index] = PropertyMap;
			tables[PropertyTable.Index] = Property;
			tables[MethodSemanticsTable.Index] = MethodSemantics;
			tables[MethodImplTable.Index] = MethodImpl;
			tables[ModuleRefTable.Index] = ModuleRef;
			tables[TypeSpecTable.Index] = TypeSpec;
			tables[ImplMapTable.Index] = ImplMap;
			tables[FieldRVATable.Index] = FieldRVA;
			tables[AssemblyTable.Index] = Assembly;
			tables[AssemblyRefTable.Index] = AssemblyRef;
			tables[MethodDefTable.Index] = MethodDef;
			tables[NestedClassTable.Index] = NestedClass;
			tables[FileTable.Index] = File;
			tables[ExportedTypeTable.Index] = ExportedType;
			tables[ManifestResourceTable.Index] = ManifestResource;
			tables[GenericParamTable.Index] = GenericParam;
			tables[MethodSpecTable.Index] = MethodSpec;
			tables[GenericParamConstraintTable.Index] = GenericParamConstraint;
		}

		internal abstract class Table
		{
			internal bool IsBig
			{
				get { return RowCount > 65535; }
			}

			internal abstract int RowCount { get; }

			internal abstract void Write(MetadataWriter mw);

			internal int GetLength(ModuleBuilder md)
			{
				return RowCount * GetRowSize(new RowSizeCalc(md));
			}

			protected abstract int GetRowSize(RowSizeCalc rsc);

			protected sealed class RowSizeCalc
			{
				private readonly ModuleBuilder md;
				private int size;

				internal RowSizeCalc(ModuleBuilder md)
				{
					this.md = md;
					this.size = 0;
				}

				internal RowSizeCalc AddFixed(int size)
				{
					this.size += size;
					return this;
				}

				internal RowSizeCalc WriteStringIndex()
				{
					if (md.bigStrings)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteGuidIndex()
				{
					if (md.bigGuids)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteBlobIndex()
				{
					if (md.bigBlobs)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteTypeDefOrRef()
				{
					if (md.bigTypeDefOrRef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteField()
				{
					if (md.bigField)
					{
						size += 4;
					}
					else
					{
						size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteMethodDef()
				{
					if (md.bigMethodDef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteParam()
				{
					if (md.bigParam)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteResolutionScope()
				{
					if (md.bigResolutionScope)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteMemberRefParent()
				{
					if (md.bigMemberRefParent)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteHasCustomAttribute()
				{
					if (md.bigHasCustomAttribute)
					{
						size += 4;
					}
					else
					{
						size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteCustomAttributeType()
				{
					if (md.bigCustomAttributeType)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteHasConstant()
				{
					if (md.bigHasConstant)
					{
						size += 4;
					}
					else
					{
						size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteTypeDef()
				{
					if (md.bigTypeDef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteMethodDefOrRef()
				{
					if (md.bigMethodDefOrRef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteEvent()
				{
					if (md.bigEvent)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteProperty()
				{
					if (md.bigProperty)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteHasSemantics()
				{
					if (md.bigHasSemantics)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteImplementation()
				{
					if (md.bigImplementation)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteTypeOrMethodDef()
				{
					if (md.bigTypeOrMethodDef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteGenericParam()
				{
					if (md.bigGenericParam)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteHasDeclSecurity()
				{
					if (md.bigHasDeclSecurity)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteMemberForwarded()
				{
					if (md.bigMemberForwarded)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteModuleRef()
				{
					if (md.bigModuleRef)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal RowSizeCalc WriteHasFieldMarshal()
				{
					if (md.bigHasFieldMarshal)
					{
						this.size += 4;
					}
					else
					{
						this.size += 2;
					}
					return this;
				}

				internal int Value
				{
					get { return size; }
				}
			}
		}

		internal abstract class Table<T> : Table
		{
			protected T[] records = new T[1];
			protected int rowCount;

			internal override int RowCount
			{
				get { return rowCount; }
			}

			internal int AddRecord(T newRecord)
			{
				if (rowCount == records.Length)
				{
					T[] newarr = new T[records.Length * 2];
					Array.Copy(records, newarr, records.Length);
					records = newarr;
				}
				records[rowCount++] = newRecord;
				return rowCount;
			}
		}

		internal abstract class VirtualTable : Table
		{
			protected int rowCount;

			internal sealed override int RowCount
			{
				get { return rowCount; }
			}

			internal int AddRow()
			{
				return ++rowCount;
			}
		}

		internal sealed class ModuleTable : Table<ModuleTable.Record>
		{
			internal const int Index = 0x00;

			internal struct Record
			{
				internal short Generation;
				internal int Name; // -> StringHeap
				internal int Mvid; // -> GuidHeap
				internal int EncId; // -> GuidHeap
				internal int EncBaseId; // -> GuidHeap
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Generation);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteGuidIndex(records[i].Mvid);
					mw.WriteGuidIndex(records[i].EncId);
					mw.WriteGuidIndex(records[i].EncBaseId);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteStringIndex()
					.WriteGuidIndex()
					.WriteGuidIndex()
					.WriteGuidIndex()
					.Value;
			}

			internal void Add(short generation, int name, int mvid, int encid, int encbaseid)
			{
				Record record = new Record();
				record.Generation = generation;
				record.Name = name;
				record.Mvid = mvid;
				record.EncId = encid;
				record.EncBaseId = encbaseid;
				AddRecord(record);
			}
		}

		internal sealed class AssemblyTable : Table<AssemblyTable.Record>
		{
			internal const int Index = 0x20;

			internal struct Record
			{
				internal int HashAlgId;
				internal short MajorVersion;
				internal short MinorVersion;
				internal short BuildNumber;
				internal short RevisionNumber;
				internal int Flags;
				internal int PublicKey;
				internal int Name;
				internal int Culture;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].HashAlgId);
					mw.Write(records[i].MajorVersion);
					mw.Write(records[i].MinorVersion);
					mw.Write(records[i].BuildNumber);
					mw.Write(records[i].RevisionNumber);
					mw.Write(records[i].Flags);
					mw.WriteBlobIndex(records[i].PublicKey);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteStringIndex(records[i].Culture);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(16)
					.WriteBlobIndex()
					.WriteStringIndex()
					.WriteStringIndex()
					.Value;
			}
		}

		internal sealed class AssemblyRefTable : Table<AssemblyRefTable.Record>
		{
			internal const int Index = 0x23;

			internal struct Record
			{
				internal short MajorVersion;
				internal short MinorVersion;
				internal short BuildNumber;
				internal short RevisionNumber;
				internal int Flags;
				internal int PublicKeyOrToken;
				internal int Name;
				internal int Culture;
				internal int HashValue;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].MajorVersion);
					mw.Write(records[i].MinorVersion);
					mw.Write(records[i].BuildNumber);
					mw.Write(records[i].RevisionNumber);
					mw.Write(records[i].Flags);
					mw.WriteBlobIndex(records[i].PublicKeyOrToken);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteStringIndex(records[i].Culture);
					mw.WriteBlobIndex(records[i].HashValue);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(12)
					.WriteBlobIndex()
					.WriteStringIndex()
					.WriteStringIndex()
					.WriteBlobIndex()
					.Value;
			}
		}

		internal sealed class TypeRefTable : Table<TypeRefTable.Record>
		{
			internal const int Index = 0x01;

			internal struct Record
			{
				internal int ResolutionScope;
				internal int TypeName;
				internal int TypeNameSpace;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteResolutionScope(records[i].ResolutionScope);
					mw.WriteStringIndex(records[i].TypeName);
					mw.WriteStringIndex(records[i].TypeNameSpace);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteResolutionScope()
					.WriteStringIndex()
					.WriteStringIndex()
					.Value;
			}
		}

		internal sealed class TypeDefTable : VirtualTable
		{
			internal const int Index = 0x02;

			internal int AllocToken()
			{
				return 0x02000000 + AddRow();
			}

			internal override void Write(MetadataWriter mw)
			{
				mw.ModuleBuilder.WriteTypeDefTable(mw);
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteStringIndex()
					.WriteStringIndex()
					.WriteTypeDefOrRef()
					.WriteField()
					.WriteMethodDef()
					.Value;
			}
		}

		internal sealed class FieldTable : VirtualTable
		{
			internal const int Index = 0x04;

			internal override void Write(MetadataWriter mw)
			{
				mw.ModuleBuilder.WriteFieldTable(mw);
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteStringIndex()
					.WriteBlobIndex()
					.Value;
			}
		}

		internal sealed class MethodDefTable : VirtualTable
		{
			internal const int Index = 0x06;
			private int baseRVA;

			internal override void Write(MetadataWriter mw)
			{
				mw.ModuleBuilder.WriteMethodDefTable(baseRVA, mw);
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(8)
					.WriteStringIndex()
					.WriteBlobIndex()
					.WriteParam()
					.Value;
			}

			internal void Fixup(TextSection code)
			{
				baseRVA = (int)code.MethodBodiesRVA;
			}
		}

		internal sealed class ParamTable : VirtualTable
		{
			internal const int Index = 0x08;

			internal override void Write(MetadataWriter mw)
			{
				mw.ModuleBuilder.WriteParamTable(mw);
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteStringIndex()
					.Value;
			}
		}

		internal sealed class InterfaceImplTable : Table<InterfaceImplTable.Record>, IComparer<InterfaceImplTable.Record>
		{
			internal const int Index = 0x09;

			internal struct Record
			{
				internal int Class;
				internal int Interface;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteTypeDef(records[i].Class);
					mw.WriteEncodedTypeDefOrRef(records[i].Interface);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteTypeDef()
					.WriteTypeDefOrRef()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					int token = records[i].Interface;
					switch (token >> 24)
					{
						case 0:
							break;
						case TableHeap.TypeDefTable.Index:
							token = (token & 0xFFFFFF) << 2 | 0;
							break;
						case TableHeap.TypeRefTable.Index:
							token = (token & 0xFFFFFF) << 2 | 1;
							break;
						case TableHeap.TypeSpecTable.Index:
							token = (token & 0xFFFFFF) << 2 | 2;
							break;
						default:
							throw new InvalidOperationException();
					}
					records[i].Interface = token;
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				if (x.Class == y.Class)
				{
					return x.Interface == y.Interface ? 0 : (x.Interface > y.Interface ? 1 : -1);
				}
				return x.Class > y.Class ? 1 : -1;
			}
		}

		internal sealed class MemberRefTable : Table<MemberRefTable.Record>
		{
			internal const int Index = 0x0A;

			internal struct Record
			{
				internal int Class;
				internal int Name;
				internal int Signature;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteMemberRefParent(records[i].Class);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteBlobIndex(records[i].Signature);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteMemberRefParent()
					.WriteStringIndex()
					.WriteBlobIndex()
					.Value;
			}
		}

		internal sealed class ConstantTable : Table<ConstantTable.Record>, IComparer<ConstantTable.Record>
		{
			internal const int Index = 0x0B;

			internal struct Record
			{
				internal short Type;
				internal int Parent;
				internal int Value;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Type);
					mw.WriteHasConstant(records[i].Parent);
					mw.WriteBlobIndex(records[i].Value);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteHasConstant()
					.WriteBlobIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					int token = records[i].Parent;
					if (moduleBuilder.IsPseudoToken(token))
					{
						token = moduleBuilder.ResolvePseudoToken(token);
					}
					// do the HasConstant encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.FieldTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 2 | 0;
							break;
						case TableHeap.ParamTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 2 | 1;
							break;
						case TableHeap.PropertyTable.Index:
						    records[i].Parent = (token & 0xFFFFFF) << 2 | 2;
							break;
						default:
							throw new InvalidOperationException();
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Parent == y.Parent ? 0 : (x.Parent > y.Parent ? 1 : -1);
			}
		}

		internal sealed class CustomAttributeTable : Table<CustomAttributeTable.Record>, IComparer<CustomAttributeTable.Record>
		{
			internal const int Index = 0x0C;

			internal struct Record
			{
				internal int Parent;
				internal int Type;
				internal int Value;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteHasCustomAttribute(records[i].Parent);
					mw.WriteCustomAttributeType(records[i].Type);
					mw.WriteBlobIndex(records[i].Value);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteHasCustomAttribute()
					.WriteCustomAttributeType()
					.WriteBlobIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (moduleBuilder.IsPseudoToken(records[i].Type))
					{
						records[i].Type = moduleBuilder.ResolvePseudoToken(records[i].Type);
					}
					int token = records[i].Parent;
					if (moduleBuilder.IsPseudoToken(token))
					{
						token = moduleBuilder.ResolvePseudoToken(token);
					}
					// do the HasCustomAttribute encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.MethodDefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 0;
							break;
						case TableHeap.FieldTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 1;
							break;
						case TableHeap.TypeRefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 2;
							break;
						case TableHeap.TypeDefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 3;
							break;
						case TableHeap.ParamTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 4;
							break;
						case TableHeap.InterfaceImplTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 5;
							break;
						case TableHeap.MemberRefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 6;
							break;
						case TableHeap.ModuleTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 7;
							break;
						// Permission (8) table doesn't exist in the spec
						case TableHeap.PropertyTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 9;
							break;
						case TableHeap.EventTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 10;
							break;
						case TableHeap.StandAloneSigTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 11;
							break;
						case TableHeap.ModuleRefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 12;
							break;
						case TableHeap.TypeSpecTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 13;
							break;
						case TableHeap.AssemblyTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 14;
							break;
						case TableHeap.AssemblyRefTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 15;
							break;
						case TableHeap.FileTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 16;
							break;
						case TableHeap.ExportedTypeTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 17;
							break;
						case TableHeap.ManifestResourceTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 5 | 18;
							break;
						default:
							throw new InvalidOperationException();
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Parent == y.Parent ? 0 : (x.Parent > y.Parent ? 1 : -1);
			}
		}

		internal sealed class FieldMarshalTable : Table<FieldMarshalTable.Record>, IComparer<FieldMarshalTable.Record>
		{
			internal const int Index = 0x0D;

			internal struct Record
			{
				internal int Parent;
				internal int NativeType;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteHasFieldMarshal(records[i].Parent);
					mw.WriteBlobIndex(records[i].NativeType);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteHasFieldMarshal()
					.WriteBlobIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					int token = moduleBuilder.ResolvePseudoToken(records[i].Parent);
					// do the HasFieldMarshal encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.FieldTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 1 | 0;
							break;
						case TableHeap.ParamTable.Index:
							records[i].Parent = (token & 0xFFFFFF) << 1 | 1;
							break;
						default:
							throw new InvalidOperationException();
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Parent == y.Parent ? 0 : (x.Parent > y.Parent ? 1 : -1);
			}
		}

		internal sealed class DeclSecurityTable : Table<DeclSecurityTable.Record>, IComparer<DeclSecurityTable.Record>
		{
			internal const int Index = 0x0E;

			internal struct Record
			{
				internal short Action;
				internal int Parent;
				internal int PermissionSet;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Action);
					mw.WriteHasDeclSecurity(records[i].Parent);
					mw.WriteBlobIndex(records[i].PermissionSet);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteHasDeclSecurity()
					.WriteBlobIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					int token = records[i].Parent;
					if (moduleBuilder.IsPseudoToken(token))
					{
						token = moduleBuilder.ResolvePseudoToken(token);
					}
					// do the HasDeclSecurity encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.TypeDefTable.Index:
							token = (token & 0xFFFFFF) << 2 | 0;
							break;
						case TableHeap.MethodDefTable.Index:
							token = (token & 0xFFFFFF) << 2 | 1;
							break;
						case TableHeap.AssemblyTable.Index:
							token = (token & 0xFFFFFF) << 2 | 2;
							break;
						default:
							throw new InvalidOperationException();
					}
					records[i].Parent = token;
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Parent == y.Parent ? 0 : (x.Parent > y.Parent ? 1 : -1);
			}
		}

		internal sealed class ClassLayoutTable : Table<ClassLayoutTable.Record>, IComparer<ClassLayoutTable.Record>
		{
			internal const int Index = 0x0f;

			internal struct Record
			{
				internal short PackingSize;
				internal int ClassSize;
				internal int Parent;
			}

			internal override void Write(MetadataWriter mw)
			{
				Array.Sort(records, 0, rowCount, this);
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].PackingSize);
					mw.Write(records[i].ClassSize);
					mw.WriteTypeDef(records[i].Parent);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(6)
					.WriteTypeDef()
					.Value;
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Parent == y.Parent ? 0 : (x.Parent > y.Parent ? 1 : -1);
			}

			internal void GetLayout(int token, ref int pack, ref int size)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i].Parent == token)
					{
						pack = records[i].PackingSize;
						size = records[i].ClassSize;
						break;
					}
				}
			}
		}

		internal sealed class FieldLayoutTable : Table<FieldLayoutTable.Record>, IComparer<FieldLayoutTable.Record>
		{
			internal const int Index = 0x10;

			internal struct Record
			{
				internal int Offset;
				internal int Field;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Offset);
					mw.WriteField(records[i].Field);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteField()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					records[i].Field = moduleBuilder.ResolvePseudoToken(records[i].Field);
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Field == y.Field ? 0 : (x.Field > y.Field ? 1 : -1);
			}
		}

		internal sealed class StandAloneSigTable : Table<int>
		{
			internal const int Index = 0x11;

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteBlobIndex(records[i]);
				}
			}

			protected override int GetRowSize(Table.RowSizeCalc rsc)
			{
				return rsc.WriteBlobIndex().Value;
			}

			internal int Add(int blob)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i] == blob)
					{
						return i + 1;
					}
				}
				return AddRecord(blob);
			}
		}

		internal sealed class EventMapTable : Table<EventMapTable.Record>
		{
			internal const int Index = 0x12;

			internal struct Record
			{
				internal int Parent;
				internal int EventList;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteTypeDef(records[i].Parent);
					mw.WriteEvent(records[i].EventList);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteTypeDef()
					.WriteEvent()
					.Value;
			}
		}

		internal sealed class EventTable : Table<EventTable.Record>
		{
			internal const int Index = 0x14;

			internal struct Record
			{
				internal short EventFlags;
				internal int Name;
				internal int EventType;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].EventFlags);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteTypeDefOrRef(records[i].EventType);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteStringIndex()
					.WriteTypeDefOrRef()
					.Value;
			}
		}

		internal sealed class PropertyMapTable : Table<PropertyMapTable.Record>
		{
			internal const int Index = 0x15;

			internal struct Record
			{
				internal int Parent;
				internal int PropertyList;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteTypeDef(records[i].Parent);
					mw.WriteProperty(records[i].PropertyList);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteTypeDef()
					.WriteProperty()
					.Value;
			}
		}

		internal sealed class PropertyTable : Table<PropertyTable.Record>
		{
			internal const int Index = 0x17;

			internal struct Record
			{
				internal short Flags;
				internal int Name;
				internal int Type;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Flags);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteBlobIndex(records[i].Type);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteStringIndex()
					.WriteBlobIndex()
					.Value;
			}
		}

		internal sealed class MethodSemanticsTable : Table<MethodSemanticsTable.Record>, IComparer<MethodSemanticsTable.Record>
		{
			internal const int Index = 0x18;

			internal struct Record
			{
				internal short Semantics;
				internal int Method;
				internal int Association;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Semantics);
					mw.WriteMethodDef(records[i].Method);
					mw.WriteHasSemantics(records[i].Association);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteMethodDef()
					.WriteHasSemantics()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (moduleBuilder.IsPseudoToken(records[i].Method))
					{
						records[i].Method = moduleBuilder.ResolvePseudoToken(records[i].Method);
					}
					int token = records[i].Association;
					// do the HasSemantics encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.EventTable.Index:
							token = (token & 0xFFFFFF) << 1 | 0;
							break;
						case TableHeap.PropertyTable.Index:
							token = (token & 0xFFFFFF) << 1 | 1;
							break;
						default:
							throw new InvalidOperationException();
					}
					records[i].Association = token;
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Association == y.Association ? 0 : (x.Association > y.Association ? 1 : -1);
			}
		}

		internal sealed class MethodImplTable : Table<MethodImplTable.Record>, IComparer<MethodImplTable.Record>
		{
			internal const int Index = 0x19;

			internal struct Record
			{
				internal int Class;
				internal int MethodBody;
				internal int MethodDeclaration;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteTypeDef(records[i].Class);
					mw.WriteMethodDefOrRef(records[i].MethodBody);
					mw.WriteMethodDefOrRef(records[i].MethodDeclaration);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteTypeDef()
					.WriteMethodDefOrRef()
					.WriteMethodDefOrRef()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (moduleBuilder.IsPseudoToken(records[i].MethodBody))
					{
						records[i].MethodBody = moduleBuilder.ResolvePseudoToken(records[i].MethodBody);
					}
					if (moduleBuilder.IsPseudoToken(records[i].MethodDeclaration))
					{
						records[i].MethodDeclaration = moduleBuilder.ResolvePseudoToken(records[i].MethodDeclaration);
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Class == y.Class ? 0 : (x.Class > y.Class ? 1 : -1);
			}
		}

		internal sealed class ModuleRefTable : Table<int>
		{
			internal const int Index = 0x1A;

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteStringIndex(records[i]);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteStringIndex()
					.Value;
			}

			internal int Add(int str)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i] == str)
					{
						return i + 1;
					}
				}
				return AddRecord(str);
			}
		}

		internal sealed class TypeSpecTable : Table<int>
		{
			internal const int Index = 0x1B;

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteBlobIndex(records[i]);
				}
			}

			protected override int GetRowSize(Table.RowSizeCalc rsc)
			{
				return rsc.WriteBlobIndex().Value;
			}
		}

		internal sealed class ImplMapTable : Table<ImplMapTable.Record>, IComparer<ImplMapTable.Record>
		{
			internal const int Index = 0x1C;

			internal struct Record
			{
				internal short MappingFlags;
				internal int MemberForwarded;
				internal int ImportName;
				internal int ImportScope;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].MappingFlags);
					mw.WriteMemberForwarded(records[i].MemberForwarded);
					mw.WriteStringIndex(records[i].ImportName);
					mw.WriteModuleRef(records[i].ImportScope);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(2)
					.WriteMemberForwarded()
					.WriteStringIndex()
					.WriteModuleRef()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (moduleBuilder.IsPseudoToken(records[i].MemberForwarded))
					{
						records[i].MemberForwarded = moduleBuilder.ResolvePseudoToken(records[i].MemberForwarded);
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.MemberForwarded == y.MemberForwarded ? 0 : (x.MemberForwarded > y.MemberForwarded ? 1 : -1);
			}
		}

		internal sealed class FieldRVATable : Table<FieldRVATable.Record>, IComparer<FieldRVATable.Record>
		{
			internal const int Index = 0x1D;

			internal struct Record
			{
				internal int RVA;
				internal int Field;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].RVA);
					mw.WriteField(records[i].Field);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteField()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder, int sdataRVA)
			{
				for (int i = 0; i < rowCount; i++)
				{
					records[i].RVA += sdataRVA;
					if (moduleBuilder.IsPseudoToken(records[i].Field))
					{
						records[i].Field = moduleBuilder.ResolvePseudoToken(records[i].Field);
					}
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Field == y.Field ? 0 : (x.Field > y.Field ? 1 : -1);
			}
		}

		internal sealed class FileTable : Table<FileTable.Record>
		{
			internal const int Index = 0x26;

			internal struct Record
			{
				internal int Flags;
				internal int Name;
				internal int HashValue;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Flags);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteBlobIndex(records[i].HashValue);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteStringIndex()
					.WriteBlobIndex()
					.Value;
			}
		}

		internal sealed class ExportedTypeTable : Table<ExportedTypeTable.Record>
		{
			internal const int Index = 0x27;

			internal struct Record
			{
				internal int Flags;
				internal int TypeDefId;
				internal int TypeName;
				internal int TypeNamespace;
				internal int Implementation;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Flags);
					mw.Write(records[i].TypeDefId);
					mw.WriteStringIndex(records[i].TypeName);
					mw.WriteStringIndex(records[i].TypeNamespace);
					mw.WriteImplementation(records[i].Implementation);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(8)
					.WriteStringIndex()
					.WriteStringIndex()
					.WriteImplementation()
					.Value;
			}

			internal int FindOrAddRecord(Record rec)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i].Implementation == rec.Implementation
						&& records[i].TypeName == rec.TypeName
						&& records[i].TypeNamespace == rec.TypeNamespace)
					{
						return i + 1;
					}
				}
				return AddRecord(rec);
			}
		}

		internal sealed class ManifestResourceTable : Table<ManifestResourceTable.Record>
		{
			internal const int Index = 0x28;

			internal struct Record
			{
				internal int Offset;
				internal int Flags;
				internal int Name;
				internal int Implementation;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Offset);
					mw.Write(records[i].Flags);
					mw.WriteStringIndex(records[i].Name);
					mw.WriteImplementation(records[i].Implementation);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(8)
					.WriteStringIndex()
					.WriteImplementation()
					.Value;
			}
		}

		internal sealed class NestedClassTable : Table<NestedClassTable.Record>
		{
			internal const int Index = 0x29;

			internal struct Record
			{
				internal int NestedClass;
				internal int EnclosingClass;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteTypeDef(records[i].NestedClass);
					mw.WriteTypeDef(records[i].EnclosingClass);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteTypeDef()
					.WriteTypeDef()
					.Value;
			}

			internal List<int> GetNestedClasses(int enclosingClass)
			{
				List<int> nestedClasses = new List<int>();
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i].EnclosingClass == enclosingClass)
					{
						nestedClasses.Add(records[i].NestedClass);
					}
				}
				return nestedClasses;
			}
		}

		internal sealed class GenericParamTable : Table<GenericParamTable.Record>, IComparer<GenericParamTable.Record>
		{
			internal const int Index = 0x2A;

			internal struct Record
			{
				internal short Number;
				internal short Flags;
				internal int Owner;
				internal int Name;
				// not part of the table, we use it to be able to fixup the GenericParamConstraint table
				internal int unsortedIndex;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.Write(records[i].Number);
					mw.Write(records[i].Flags);
					mw.WriteTypeOrMethodDef(records[i].Owner);
					mw.WriteStringIndex(records[i].Name);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.AddFixed(4)
					.WriteTypeOrMethodDef()
					.WriteStringIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					int token = records[i].Owner;
					if (moduleBuilder.IsPseudoToken(token))
					{
						token = moduleBuilder.ResolvePseudoToken(token);
					}
					// do the TypeOrMethodDef encoding, so that we can sort the table
					switch (token >> 24)
					{
						case TableHeap.TypeDefTable.Index:
							records[i].Owner = (token & 0xFFFFFF) << 1 | 0;
							break;
						case TableHeap.MethodDefTable.Index:
							records[i].Owner = (token & 0xFFFFFF) << 1 | 1;
							break;
						default:
							throw new InvalidOperationException();
					}
					records[i].unsortedIndex = i;
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				if (x.Owner == y.Owner)
				{
					return x.Number == y.Number ? 0 : (x.Number > y.Number ? 1 : -1);
				}
				return x.Owner > y.Owner ? 1 : -1;
			}

			internal void PatchAttribute(int token, System.Reflection.GenericParameterAttributes genericParameterAttributes)
			{
				records[(token & 0xFFFFFF) - 1].Flags = (short)genericParameterAttributes;
			}

			internal int[] GetIndexFixup()
			{
				int[] array = new int[rowCount];
				for (int i = 0; i < rowCount; i++)
				{
					array[records[i].unsortedIndex] = i;
				}
				return array;
			}
		}

		internal sealed class MethodSpecTable : Table<MethodSpecTable.Record>
		{
			internal const int Index = 0x2B;

			internal struct Record
			{
				internal int Method;
				internal int Instantiation;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteMethodDefOrRef(records[i].Method);
					mw.WriteBlobIndex(records[i].Instantiation);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteMethodDefOrRef()
					.WriteBlobIndex()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (moduleBuilder.IsPseudoToken(records[i].Method))
					{
						records[i].Method = moduleBuilder.ResolvePseudoToken(records[i].Method);
					}
				}
			}

			internal int FindOrAddRecord(Record rec)
			{
				for (int i = 0; i < rowCount; i++)
				{
					if (records[i].Method == rec.Method &&
						records[i].Instantiation == rec.Instantiation)
					{
						return i + 1;
					}
				}
				return AddRecord(rec);
			}
		}

		internal sealed class GenericParamConstraintTable : Table<GenericParamConstraintTable.Record>, IComparer<GenericParamConstraintTable.Record>
		{
			internal const int Index = 0x2C;

			internal struct Record
			{
				internal int Owner;
				internal int Constraint;
			}

			internal override void Write(MetadataWriter mw)
			{
				for (int i = 0; i < rowCount; i++)
				{
					mw.WriteGenericParam(records[i].Owner);
					mw.WriteTypeDefOrRef(records[i].Constraint);
				}
			}

			protected override int GetRowSize(RowSizeCalc rsc)
			{
				return rsc
					.WriteGenericParam()
					.WriteTypeDefOrRef()
					.Value;
			}

			internal void Fixup(ModuleBuilder moduleBuilder)
			{
				int[] fixups = moduleBuilder.Tables.GenericParam.GetIndexFixup();
				for (int i = 0; i < rowCount; i++)
				{
					records[i].Owner = fixups[records[i].Owner - 1] + 1;
				}
				Array.Sort(records, 0, rowCount, this);
			}

			int IComparer<Record>.Compare(Record x, Record y)
			{
				return x.Owner == y.Owner ? 0 : (x.Owner > y.Owner ? 1 : -1);
			}
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			// Header
			mw.Write(0);		// Reserved
			mw.Write((byte)2);	// MajorVersion
			mw.Write((byte)0);	// MinorVersion
			byte heapSizes = 0;
			if (mw.ModuleBuilder.Strings.IsBig)
			{
				heapSizes |= 0x01;
			}
			if (mw.ModuleBuilder.Guids.IsBig)
			{
				heapSizes |= 0x02;
			}
			if (mw.ModuleBuilder.Blobs.IsBig)
			{
				heapSizes |= 0x04;
			}
			mw.Write(heapSizes);// HeapSizes
			// LAMESPEC spec says reserved, but .NET 2.0 Ref.Emit sets it to 0x10
			mw.Write((byte)0x10);	// Reserved
			long bit = 1;
			long valid = 0;
			foreach (Table table in tables)
			{
				if (table != null && table.RowCount > 0)
				{
					valid |= bit;
				}
				bit <<= 1;
			}
			mw.Write(valid);	// Valid
			mw.Write(0x0016003301FA00L);	// Sorted
			// Rows
			foreach (Table table in tables)
			{
				if (table != null && table.RowCount > 0)
				{
					mw.Write(table.RowCount);
				}
			}
			// Tables
			foreach (Table table in tables)
			{
				if (table != null && table.RowCount > 0)
				{
					int pos = mw.Position;
					table.Write(mw);
					Debug.Assert(mw.Position - pos == table.GetLength(mw.ModuleBuilder));
				}
			}
			// unexplained extra padding
			mw.Write((byte)0);
		}

		protected override int GetLength(ModuleBuilder md)
		{
			int len = 4 + 4 + 8 + 8;
			foreach (Table table in tables)
			{
				if (table != null && table.RowCount > 0)
				{
					len += 4;	// row count
					len += table.GetLength(md);
				}
			}
			// note that we pad one extra (unexplained) byte
			return len + 1;
		}
	}

	sealed class StringHeap : Heap
	{
		private List<string> list = new List<string>();
		private Dictionary<string, int> strings = new Dictionary<string, int>();
		private int nextOffset;

		internal StringHeap()
		{
			Add("");
		}

		internal int Add(string str)
		{
			int offset;
			if (!strings.TryGetValue(str, out offset))
			{
				offset = nextOffset;
				nextOffset += System.Text.Encoding.UTF8.GetByteCount(str) + 1;
				list.Add(str);
				strings.Add(str, offset);
			}
			return offset;
		}

		protected override int GetLength(ModuleBuilder md)
		{
			return nextOffset;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			foreach (string str in list)
			{
				mw.Write(System.Text.Encoding.UTF8.GetBytes(str));
				mw.Write((byte)0);
			}
		}
	}

	sealed class UserStringHeap : Heap
	{
		private List<string> list = new List<string>();
		private Dictionary<string, int> strings = new Dictionary<string, int>();
		private int nextOffset;

		internal UserStringHeap()
		{
			nextOffset = 1;
		}

		internal bool IsEmpty
		{
			get { return nextOffset == 1; }
		}

		internal int Add(string str)
		{
			int offset;
			if (!strings.TryGetValue(str, out offset))
			{
				offset = nextOffset;
				nextOffset += str.Length * 2 + 1 + MetadataWriter.GetCompressedIntLength(str.Length * 2 + 1);
				list.Add(str);
				strings.Add(str, offset);
			}
			return offset;
		}

		protected override int GetLength(ModuleBuilder md)
		{
			return nextOffset;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			mw.Write((byte)0);
			foreach (string str in list)
			{
				mw.WriteCompressedInt(str.Length * 2 + 1);
				byte hasSpecialChars = 0;
				foreach (char ch in str)
				{
					mw.Write((ushort)ch);
					if (hasSpecialChars == 0 && (ch < 0x20 || ch > 0x7E))
					{
						if (ch > 0x7E
							|| (ch >= 0x01 && ch <= 0x08)
							|| (ch >= 0x0E && ch <= 0x1F)
							|| ch == 0x27
							|| ch == 0x2D)
						{
							hasSpecialChars = 1;
						}
					}
				}
				mw.Write(hasSpecialChars);
			}
		}
	}

	sealed class GuidHeap : Heap
	{
		private List<Guid> list = new List<Guid>();

		internal GuidHeap()
		{
		}

		internal int Add(Guid guid)
		{
			list.Add(guid);
			return list.Count;
		}

		protected override int GetLength(ModuleBuilder md)
		{
			return list.Count * 16;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			foreach (Guid guid in list)
			{
				mw.Write(guid.ToByteArray());
			}
		}
	}

	sealed class BlobHeap : Heap
	{
		private Key[] map = new Key[8179];
		private readonly ByteBuffer buf = new ByteBuffer(32);

		private struct Key
		{
			internal Key[] next;
			internal int len;
			internal int hash;
			internal int offset;
		}

		internal BlobHeap()
		{
			buf.Write((byte)0);
		}

		internal int Add(ByteBuffer bb)
		{
			if (bb.Length == 0)
			{
				return 0;
			}
			int lenlen = MetadataWriter.GetCompressedIntLength(bb.Length);
			int hash = bb.Hash();
			int index = (hash & 0x7FFFFFFF) % map.Length;
			Key[] keys = map;
			int last = index;
			while (keys[index].offset != 0)
			{
				if (keys[index].hash == hash
					&& keys[index].len == bb.Length
					&& buf.Match(keys[index].offset + lenlen, bb, 0, bb.Length))
				{
					return keys[index].offset;
				}
				if (index == last)
				{
					if (keys[index].next == null)
					{
						keys[index].next = new Key[4];
						keys = keys[index].next;
						index = 0;
						break;
					}
					keys = keys[index].next;
					index = -1;
					last = keys.Length - 1;
				}
				index++;
			}
			int offset = buf.Position;
			buf.WriteCompressedInt(bb.Length);
			buf.Write(bb);
			keys[index].len = bb.Length;
			keys[index].hash = hash;
			keys[index].offset = offset;
			return offset;
		}

		protected override int GetLength(ModuleBuilder md)
		{
			return buf.Position;
		}

		protected override void WriteImpl(MetadataWriter mw)
		{
			mw.Write(buf);
		}

		internal bool IsEmpty
		{
			get { return buf.Position == 1; }
		}
	}
}
