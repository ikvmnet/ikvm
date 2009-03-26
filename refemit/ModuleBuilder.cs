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
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using IKVM.Reflection.Emit.Impl;
using IKVM.Reflection.Emit.Writer;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Emit
{
	public sealed class ModuleBuilder : ITypeOwner
	{
		private readonly AssemblyBuilder asm;
		internal readonly string moduleName;
		internal readonly string fileName;
		internal readonly ISymbolWriter symbolWriter;
		private readonly TypeBuilder moduleType;
		private readonly List<TypeBuilder> types = new List<TypeBuilder>();
		private readonly Dictionary<Type, int> typeTokens = new Dictionary<Type, int>();
		private readonly Dictionary<string, TypeBuilder> fullNameToType = new Dictionary<string, TypeBuilder>();
		internal readonly ByteBuffer methodBodies = new ByteBuffer(128 * 1024);
		internal readonly List<int> tokenFixupOffsets = new List<int>();
		internal readonly ByteBuffer initializedData = new ByteBuffer(512);
		internal readonly ByteBuffer manifestResources = new ByteBuffer(512);
		private readonly Dictionary<MemberInfo, int> importedMembers = new Dictionary<MemberInfo, int>();
		private readonly Dictionary<AssemblyName, int> referencedAssemblies = new Dictionary<AssemblyName, int>(new AssemblyNameEqualityComparer());
		private readonly Dictionary<Type, Type> canonicalizedTypes = new Dictionary<Type, Type>();
		private readonly Dictionary<MethodInfo, MethodInfo> canonicalizedGenericMethods = new Dictionary<MethodInfo, MethodInfo>(new GenericMethodComparer());
		private int nextPseudoToken = -1;
		private readonly List<int> resolvedTokens = new List<int>();
		internal readonly TableHeap Tables;
		internal readonly StringHeap Strings = new StringHeap();
		internal readonly UserStringHeap UserStrings = new UserStringHeap();
		internal readonly GuidHeap Guids = new GuidHeap();
		internal readonly BlobHeap Blobs = new BlobHeap();
		internal bool bigStrings;
		internal bool bigGuids;
		internal bool bigBlobs;
		internal bool bigField;
		internal bool bigMethodDef;
		internal bool bigParam;
		internal bool bigTypeDef;
		internal bool bigProperty;
		internal bool bigGenericParam;
		internal bool bigModuleRef;
		internal bool bigResolutionScope;
		internal bool bigMemberRefParent;
		internal bool bigMethodDefOrRef;
		internal bool bigTypeDefOrRef;
		internal bool bigHasCustomAttribute;
		internal bool bigCustomAttributeType;
		internal bool bigHasConstant;
		internal bool bigHasSemantics;
		internal bool bigImplementation;
		internal bool bigTypeOrMethodDef;
		internal bool bigHasDeclSecurity;
		internal bool bigMemberForwarded;
		internal bool bigHasFieldMarshal;

		// FXBUG AssemblyName doesn't have a working Equals (sigh)
		private sealed class AssemblyNameEqualityComparer : IEqualityComparer<AssemblyName>
		{
			public bool Equals(AssemblyName x, AssemblyName y)
			{
				return x.FullName == y.FullName;
			}

			public int GetHashCode(AssemblyName obj)
			{
				return obj.FullName.GetHashCode();
			}
		}

		// this class makes multiple instances of a generic method compare as equal,
		// however, it does not ensure that the underlying method definition is canonicalized
		private sealed class GenericMethodComparer : IEqualityComparer<MethodInfo>
		{
			public bool Equals(MethodInfo x, MethodInfo y)
			{
				if (x.GetGenericMethodDefinition() == y.GetGenericMethodDefinition())
				{
					Type[] xArgs = x.GetGenericArguments();
					Type[] yArgs = y.GetGenericArguments();
					for (int i = 0; i < xArgs.Length; i++)
					{
						if (xArgs[i] != yArgs[i])
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			public int GetHashCode(MethodInfo obj)
			{
				int hash = obj.GetGenericMethodDefinition().GetHashCode();
				foreach (Type arg in obj.GetGenericArguments())
				{
					hash *= 37;
					hash ^= arg.GetHashCode();
				}
				return hash;
			}
		}

		internal ModuleBuilder(AssemblyBuilder asm, string moduleName, string fileName, bool emitSymbolInfo)
		{
			this.Tables = new TableHeap(this);
			this.asm = asm;
			this.moduleName = moduleName;
			this.fileName = fileName;
			if (emitSymbolInfo)
			{
				symbolWriter = PdbSupport.CreateSymbolWriterFor(this);
			}
			// <Module> must be the first record in the TypeDef table
			moduleType = new TypeBuilder(this, "<Module>", null, 0);
			types.Add(moduleType);
		}

		internal void WriteTypeDefTable(MetadataWriter mw)
		{
			int fieldList = 1;
			int methodList = 1;
			foreach (TypeBuilder type in types)
			{
				type.WriteTypeDefRecord(mw, ref fieldList, ref methodList);
			}
		}

		internal void WriteMethodDefTable(int baseRVA, MetadataWriter mw)
		{
			int paramList = 1;
			foreach (TypeBuilder type in types)
			{
				type.WriteMethodDefRecords(baseRVA, mw, ref paramList);
			}
		}

		internal void WriteParamTable(MetadataWriter mw)
		{
			foreach (TypeBuilder type in types)
			{
				type.WriteParamRecords(mw);
			}
		}

		internal void WriteFieldTable(MetadataWriter mw)
		{
			foreach (TypeBuilder type in types)
			{
				type.WriteFieldRecords(mw);
			}
		}

		internal int GetTypeCount()
		{
			return types.Count;
		}

		internal int AllocPseudoToken()
		{
			return nextPseudoToken--;
		}

		public TypeBuilder DefineType(string name)
		{
			return DefineType(name, TypeAttributes.Class);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attribs)
		{
			return DefineType(name, attribs, null);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attribs, Type baseType)
		{
			return DefineType(name, attribs, baseType, PackingSize.Unspecified, 0);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			if (parent == null && (attr & TypeAttributes.Interface) == 0)
			{
				parent = typeof(object);
			}
			TypeBuilder typeBuilder = new TypeBuilder(this, name, parent, attr);
			PostDefineType(typeBuilder, packingSize, typesize);
			return typeBuilder;
		}

		internal TypeBuilder DefineNestedTypeHelper(TypeBuilder enclosingType, string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			if (parent == null && (attr & TypeAttributes.Interface) == 0)
			{
				parent = typeof(object);
			}
			TypeBuilder typeBuilder = new TypeBuilder(enclosingType, name, parent, attr);
			PostDefineType(typeBuilder, packingSize, typesize);
			if (enclosingType != null)
			{
				TableHeap.NestedClassTable.Record rec = new TableHeap.NestedClassTable.Record();
				rec.NestedClass = typeBuilder.MetadataToken;
				rec.EnclosingClass = enclosingType.MetadataToken;
				this.Tables.NestedClass.AddRecord(rec);
			}
			return typeBuilder;
		}

		private void PostDefineType(TypeBuilder typeBuilder, PackingSize packingSize, int typesize)
		{
			types.Add(typeBuilder);
			fullNameToType.Add(typeBuilder.FullName, typeBuilder);
			if (packingSize != PackingSize.Unspecified || typesize != 0)
			{
				TableHeap.ClassLayoutTable.Record rec = new TableHeap.ClassLayoutTable.Record();
				rec.PackingSize = (short)packingSize;
				rec.ClassSize = typesize;
				rec.Parent = typeBuilder.MetadataToken;
				this.Tables.ClassLayout.AddRecord(rec);
			}
		}

		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			Type fieldType = GetType("$ArrayType$" + data.Length);
			if (fieldType == null)
			{
				fieldType = DefineType("$ArrayType$" + data.Length, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.ExplicitLayout, typeof(ValueType), PackingSize.Size1, data.Length);
			}
			FieldBuilder fb = moduleType.DefineField(name, fieldType, attributes | FieldAttributes.Static | FieldAttributes.HasFieldRVA);
			TableHeap.FieldRVATable.Record rec = new TableHeap.FieldRVATable.Record();
			rec.RVA = initializedData.Position;
			rec.Field = fb.MetadataToken;
			this.Tables.FieldRVA.AddRecord(rec);
			initializedData.Write(data);
			return fb;
		}

		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return moduleType.DefineMethod(name, attributes, returnType, parameterTypes);
		}

		public void CreateGlobalFunctions()
		{
			moduleType.CreateType();
		}

		private void AddTypeForwarder(Type type)
		{
			ExportType(type);
			foreach (Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
			{
				// we export all nested types (i.e. even the private ones)
				// (this behavior is the same as the C# compiler)
				AddTypeForwarder(nested);
			}
		}

		private int ExportType(Type type)
		{
			TableHeap.ExportedTypeTable.Record rec = new TableHeap.ExportedTypeTable.Record();
			rec.TypeDefId = type.MetadataToken;
			rec.TypeName = this.Strings.Add(type.Name);
			if (type.IsNested)
			{
				rec.Flags = 0;
				rec.TypeNamespace = 0;
				rec.Implementation = ExportType(type.DeclaringType);
			}
			else
			{
				rec.Flags = 0x00200000;	// CorTypeAttr.tdForwarder
				string ns = type.Namespace;
				rec.TypeNamespace = ns == null ? 0 : this.Strings.Add(ns);
				rec.Implementation = ImportAssemblyRef(GetAssemblyName(type));
			}
			return 0x27000000 | this.Tables.ExportedType.FindOrAddRecord(rec);
		}

		internal void SetAssemblyCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Constructor.DeclaringType == typeof(TypeForwardedToAttribute))
			{
				AddTypeForwarder((Type)customBuilder.GetConstructorArgument(0));
			}
			else
			{
				SetCustomAttribute(0x20000001, customBuilder);
			}
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			SetCustomAttribute(0x00000001, customBuilder);
		}

		internal void SetCustomAttribute(int token, CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.IsPseudoCustomAttribute)
			{
				throw new NotImplementedException("Pseudo custom attribute " + customBuilder.Constructor.DeclaringType.FullName + " is not implemented");
			}
			TableHeap.CustomAttributeTable.Record rec = new TableHeap.CustomAttributeTable.Record();
			rec.Parent = token;
			rec.Type = this.GetConstructorToken(customBuilder.Constructor).Token;
			rec.Value = customBuilder.WriteBlob(this);
			this.Tables.CustomAttribute.AddRecord(rec);
		}

		public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
		{
			TableHeap.ManifestResourceTable.Record rec = new TableHeap.ManifestResourceTable.Record();
			rec.Offset = manifestResources.Position;
			rec.Flags = (int)attribute;
			rec.Name = this.Strings.Add(name);
			rec.Implementation = 0;
			this.Tables.ManifestResource.AddRecord(rec);
			manifestResources.Write(0);	// placeholder for the length
			manifestResources.Write(stream);
			int savePosition = manifestResources.Position;
			manifestResources.Position = rec.Offset;
			manifestResources.Write(savePosition - (manifestResources.Position + 4));
			manifestResources.Position = savePosition;
		}

		public AssemblyBuilder Assembly
		{
			get { return asm; }
		}

		public Type GetType(string className)
		{
			return GetType(className, false, false);
		}

		public Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			if (ignoreCase)
			{
				throw new NotImplementedException();
			}
			TypeBuilder type;
			if (!fullNameToType.TryGetValue(className, out type) && throwOnError)
			{
				throw new TypeLoadException();
			}
			return type;
		}

		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			return symbolWriter.DefineDocument(url, language, languageVendor, documentType);
		}

		public TypeToken GetTypeToken(Type type)
		{
			TypeBase tb = type as TypeBase;
			if (tb != null && tb.ModuleBuilder == this)
			{
				return new TypeToken(tb.GetTypeToken());
			}
			else
			{
				return new TypeToken(ImportType(type));
			}
		}

		internal TypeToken GetTypeTokenForMemberRef(Type type)
		{
			if (type.IsGenericTypeDefinition)
			{
				return GetTypeToken(type.MakeGenericType(type.GetGenericArguments()));
			}
			else
			{
				return GetTypeToken(type);
			}
		}

		private static bool IsFromGenericTypeDefinition(MemberInfo member)
		{
			Type decl = member.DeclaringType;
			return decl != null && decl.IsGenericTypeDefinition;
		}

		public FieldToken GetFieldToken(FieldInfo field)
		{
			FieldBuilder fb = field as FieldBuilder;
			if (fb != null && fb.ModuleBuilder == this && !IsFromGenericTypeDefinition(fb))
			{
				return new FieldToken(fb.MetadataToken);
			}
			else
			{
				return new FieldToken(ImportMember(field));
			}
		}

		public MethodToken GetMethodToken(MethodInfo method)
		{
			MethodBuilder mb = method as MethodBuilder;
			if (mb != null && mb.ModuleBuilder == this && !IsFromGenericTypeDefinition(mb))
			{
				return new MethodToken(mb.MetadataToken);
			}
			else
			{
				return new MethodToken(ImportMember(method));
			}
		}

		public ConstructorToken GetConstructorToken(ConstructorInfo constructor)
		{
			ConstructorBuilder cb = constructor as ConstructorBuilder;
			if (cb != null && cb.ModuleBuilder == this && !IsFromGenericTypeDefinition(cb))
			{
				return new ConstructorToken(cb.MetadataToken);
			}
			else
			{
				return new ConstructorToken(ImportMember(constructor));
			}
		}

		private int ImportMember(MemberInfo member)
		{
			int token;
			if (!importedMembers.TryGetValue(member, out token))
			{
				if (member.DeclaringType == null)
				{
					throw new NotImplementedException();
				}
				if (member.ReflectedType != member.DeclaringType)
				{
					// look up the canonicalized member
					token = ImportMember(member.Module.ResolveMember(member.MetadataToken));
					importedMembers.Add(member, token);
					return token;
				}

				MethodInfo method = member as MethodInfo;
				if (method != null)
				{
					if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
					{
						// FXBUG generic MethodInfos don't have a working Equals/GetHashCode,
						// so we have to canonicalize them manually
						// (we don't have to recursively call ImportMember here (like above), because the first method we encounter will always become the canonical one)
						if (importedMembers.TryGetValue(CanonicalizeGenericMethod(method), out token))
						{
							importedMembers.Add(member, token);
							return token;
						}

						const byte GENERICINST = 0x0A;
						ByteBuffer spec = new ByteBuffer(10);
						spec.Write(GENERICINST);
						Type[] args = method.GetGenericArguments();
						spec.WriteCompressedInt(args.Length);
						foreach (Type arg in args)
						{
							SignatureHelper.WriteType(this, spec, arg);
						}
						TableHeap.MethodSpecTable.Record rec = new TableHeap.MethodSpecTable.Record();
						rec.Method = GetMethodToken(method.GetGenericMethodDefinition()).Token;
						rec.Instantiation = this.Blobs.Add(spec);
						token = 0x2B000000 | this.Tables.MethodSpec.AddRecord(rec);
					}
					else
					{
						token = ImportMethodOrConstructorRef(method);
					}
				}
				else
				{
					ConstructorInfo constructor = member as ConstructorInfo;
					if (constructor != null)
					{
						token = ImportMethodOrConstructorRef(constructor);
					}
					else
					{
						FieldInfo field = member as FieldInfo;
						if (field != null)
						{
							FieldBuilder fb = field as FieldBuilder;
							if (fb != null)
							{
								token = fb.ImportTo(this);
							}
							else
							{
								token = ImportField(field.DeclaringType, field.Name, field.FieldType, field.GetOptionalCustomModifiers(), field.GetRequiredCustomModifiers());
							}
						}
						else
						{
							throw new NotImplementedException();
						}
					}
				}
				importedMembers.Add(member, token);
			}
			return token;
		}

		private int ImportMethodOrConstructorRef(MethodBase method)
		{
			if (method.DeclaringType == null)
			{
				throw new NotImplementedException();
			}
			TableHeap.MemberRefTable.Record rec = new TableHeap.MemberRefTable.Record();
			rec.Class = GetTypeTokenForMemberRef(method.DeclaringType).Token;
			rec.Name = this.Strings.Add(method.Name);
			ByteBuffer bb = new ByteBuffer(16);
			SignatureHelper.WriteMethodSig(this, bb, method);
			rec.Signature = this.Blobs.Add(bb);
			return 0x0A000000 | this.Tables.MemberRef.AddRecord(rec);
		}

		internal int ImportField(Type declaringType, string name, Type fieldType, Type[] optionalCustomModifiers, Type[] requiredCustomModifiers)
		{
			if (declaringType == null)
			{
				throw new NotImplementedException();
			}
			TableHeap.MemberRefTable.Record rec = new TableHeap.MemberRefTable.Record();
			rec.Class = GetTypeTokenForMemberRef(declaringType).Token;
			rec.Name = this.Strings.Add(name);
			ByteBuffer bb = new ByteBuffer(16);
			bb.Write(SignatureHelper.FIELD);
			SignatureHelper.WriteCustomModifiers(this, bb, SignatureHelper.ELEMENT_TYPE_CMOD_OPT, optionalCustomModifiers);
			SignatureHelper.WriteCustomModifiers(this, bb, SignatureHelper.ELEMENT_TYPE_CMOD_REQD, requiredCustomModifiers);
			SignatureHelper.WriteType(this, bb, fieldType);
			rec.Signature = this.Blobs.Add(bb);
			return 0x0A000000 | this.Tables.MemberRef.AddRecord(rec);
		}

		private int ImportType(Type type)
		{
			int token;
			if (!typeTokens.TryGetValue(type, out token))
			{
				if (type.HasElementType || (type.IsGenericType && !type.IsGenericTypeDefinition))
				{
					ByteBuffer spec = new ByteBuffer(5);
					SignatureHelper.WriteType(this, spec, type);
					token = 0x1B000000 | this.Tables.TypeSpec.AddRecord(this.Blobs.Add(spec));
				}
				else
				{
					TableHeap.TypeRefTable.Record rec = new TableHeap.TypeRefTable.Record();
					if (type.IsNested)
					{
						rec.ResolutionScope = GetTypeToken(type.DeclaringType).Token;
						rec.TypeName = this.Strings.Add(type.Name);
						rec.TypeNameSpace = 0;
					}
					else
					{
						rec.ResolutionScope = ImportAssemblyRef(GetAssemblyName(type));
						rec.TypeName = this.Strings.Add(type.Name);
						string ns = type.Namespace;
						rec.TypeNameSpace = ns == null ? 0 : this.Strings.Add(ns);
					}
					token = 0x01000000 | this.Tables.TypeRef.AddRecord(rec);
				}
				typeTokens.Add(type, token);
			}
			return token;
		}

		private static AssemblyName GetAssemblyName(Type type)
		{
			TypeBase tb = type as TypeBase;
			if (tb != null)
			{
				return tb.ModuleBuilder.Assembly.GetName();
			}
			else
			{
				return type.Assembly.GetName();
			}
		}

		private int ImportAssemblyRef(AssemblyName asm)
		{
			int token;
			if (!referencedAssemblies.TryGetValue(asm, out token))
			{
				TableHeap.AssemblyRefTable.Record rec = new TableHeap.AssemblyRefTable.Record();
				Version ver = asm.Version;
				rec.MajorVersion = (short)ver.Major;
				rec.MinorVersion = (short)ver.Minor;
				rec.BuildNumber = (short)ver.Build;
				rec.RevisionNumber = (short)ver.Revision;
				rec.Flags = 0;
				rec.PublicKeyOrToken = this.Blobs.Add(ByteBuffer.Wrap(asm.GetPublicKeyToken()));
				rec.Name = this.Strings.Add(asm.Name);
				rec.Culture = 0;
				rec.HashValue = 0;
				token = 0x23000000 | this.Tables.AssemblyRef.AddRecord(rec);
				referencedAssemblies.Add(asm, token);
			}
			return token;
		}

		internal void WriteSymbolTokenMap()
		{
			for (int i = 0; i < resolvedTokens.Count; i++)
			{
				int newToken = resolvedTokens[i];
				// The symbol API doesn't support remapping arbitrary integers, the types have to be the same,
				// so we copy the type from the newToken, because our pseudo tokens don't have a type.
				// (see MethodToken.SymbolToken)
				int oldToken = (i + 1) | (newToken & ~0xFFFFFF);
				PdbSupport.RemapToken(symbolWriter, oldToken, newToken);
			}
		}

		internal void RegisterTokenFixup(int pseudoToken, int realToken)
		{
			int index = -(pseudoToken + 1);
			while (resolvedTokens.Count <= index)
			{
				resolvedTokens.Add(0);
			}
			resolvedTokens[index] = realToken;
		}

		internal bool IsPseudoToken(int token)
		{
			return token < 0;
		}

		internal int ResolvePseudoToken(int pseudoToken)
		{
			int index = -(pseudoToken + 1);
			return resolvedTokens[index];
		}

		internal void FixupMethodBodyTokens()
		{
			int methodToken = 0x06000001;
			int fieldToken = 0x04000001;
			int parameterToken = 0x08000001;
			foreach (TypeBuilder type in types)
			{
				type.ResolveMethodAndFieldTokens(ref methodToken, ref fieldToken, ref parameterToken);
			}
			foreach (int offset in tokenFixupOffsets)
			{
				methodBodies.Position = offset;
				int pseudoToken = methodBodies.GetInt32AtCurrentPosition();
				methodBodies.Write(ResolvePseudoToken(pseudoToken));
			}
		}

		internal int MetadataLength
		{
			get
			{
				return (Blobs.IsEmpty ? 92 : 108 + Blobs.Length) + Tables.Length + Strings.Length + UserStrings.Length + Guids.Length;
			}
		}

		internal void WriteMetadata(MetadataWriter mw)
		{
			mw.Write(0x424A5342);			// Signature ("BSJB")
			mw.Write((ushort)1);			// MajorVersion
			mw.Write((ushort)1);			// MinorVersion
			mw.Write(0);					// Reserved
			byte[] version = StringToPaddedUTF8("v2.0.50727");
			mw.Write(version.Length);		// Length
			mw.Write(version);
			mw.Write((ushort)0);			// Flags
			int offset;
			// #Blob is the only optional heap
			if (Blobs.IsEmpty)
			{
				mw.Write((ushort)4);		// Streams
				offset = 92;
			}
			else
			{
				mw.Write((ushort)5);		// Streams
				offset = 108;
			}

			// Streams
			mw.Write(offset);				// Offset
			mw.Write(Tables.Length);		// Size
			mw.Write(StringToPaddedUTF8("#~"));
			offset += Tables.Length;

			mw.Write(offset);				// Offset
			mw.Write(Strings.Length);		// Size
			mw.Write(StringToPaddedUTF8("#Strings"));
			offset += Strings.Length;

			mw.Write(offset);				// Offset
			mw.Write(UserStrings.Length);	// Size
			mw.Write(StringToPaddedUTF8("#US"));
			offset += UserStrings.Length;

			mw.Write(offset);				// Offset
			mw.Write(Guids.Length);			// Size
			mw.Write(StringToPaddedUTF8("#GUID"));
			offset += Guids.Length;

			if (!Blobs.IsEmpty)
			{
				mw.Write(offset);				// Offset
				mw.Write(Blobs.Length);			// Size
				mw.Write(StringToPaddedUTF8("#Blob"));
			}

			Tables.Write(mw);
			Strings.Write(mw);
			UserStrings.Write(mw);
			Guids.Write(mw);
			if (!Blobs.IsEmpty)
			{
				Blobs.Write(mw);
			}
		}

		private static byte[] StringToPaddedUTF8(string str)
		{
			byte[] buf = new byte[(System.Text.Encoding.UTF8.GetByteCount(str) + 4) & ~3];
			System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, buf, 0);
			return buf;
		}

		internal void Freeze()
		{
			Strings.Freeze(this);
			UserStrings.Freeze(this);
			Guids.Freeze(this);
			Blobs.Freeze(this);
			this.bigStrings = Strings.IsBig;
			this.bigGuids = Guids.IsBig;
			this.bigBlobs = Blobs.IsBig;
			this.bigField = Tables.Field.IsBig;
			this.bigMethodDef = Tables.MethodDef.IsBig;
			this.bigParam = Tables.Param.IsBig;
			this.bigTypeDef = Tables.TypeDef.IsBig;
			this.bigProperty = Tables.Property.IsBig;
			this.bigGenericParam = Tables.GenericParam.IsBig;
			this.bigModuleRef = Tables.ModuleRef.IsBig;
			this.bigResolutionScope = IsBig(2, Tables.Module, Tables.ModuleRef, Tables.AssemblyRef, Tables.TypeRef);
			this.bigMemberRefParent = IsBig(3, Tables.TypeDef, Tables.TypeRef, Tables.ModuleRef, Tables.MethodDef, Tables.TypeSpec);
			this.bigMethodDefOrRef = IsBig(1, Tables.MethodDef, Tables.MemberRef);
			this.bigTypeDefOrRef = IsBig(2, Tables.TypeDef, Tables.TypeRef, Tables.TypeSpec);
			this.bigHasCustomAttribute = IsBig(5, Tables.MethodDef, Tables.Field, Tables.TypeRef, Tables.TypeDef, Tables.Param, Tables.InterfaceImpl, Tables.MemberRef,
				Tables.Module, /*Tables.Permission,*/ Tables.Property, /*Tables.Event,*/ Tables.StandAloneSig, Tables.ModuleRef, Tables.TypeSpec, Tables.Assembly,
				Tables.AssemblyRef, Tables.File, Tables.ExportedType, Tables.ManifestResource);
			this.bigCustomAttributeType = IsBig(3, Tables.MethodDef, Tables.MemberRef);
			this.bigHasConstant = IsBig(2, Tables.Field, Tables.Param, Tables.Property);
			this.bigHasSemantics = IsBig(1, /*Tables.Event,*/ Tables.Property);
			this.bigImplementation = IsBig(2, Tables.File, Tables.AssemblyRef, Tables.ExportedType);
			this.bigTypeOrMethodDef = IsBig(1, Tables.TypeDef, Tables.MethodDef);
			this.bigHasDeclSecurity = IsBig(2, Tables.TypeDef, Tables.MethodDef, Tables.Assembly);
			this.bigMemberForwarded = IsBig(1, Tables.Field, Tables.MethodDef);
			this.bigHasFieldMarshal = IsBig(1, Tables.Field, Tables.Param);
			Tables.Freeze(this);
		}

		private bool IsBig(int bitsUsed, params TableHeap.Table[] tables)
		{
			int limit = 1 << (16 - bitsUsed);
			foreach (TableHeap.Table table in tables)
			{
				if (table.RowCount >= limit)
				{
					return true;
				}
			}
			return false;
		}

		internal Type CanonicalizeType(Type type)
		{
			Type canon;
			if (!canonicalizedTypes.TryGetValue(type, out canon))
			{
				canon = type;
				canonicalizedTypes.Add(canon, canon);
			}
			return canon;
		}

		private MethodInfo CanonicalizeGenericMethod(MethodInfo method)
		{
			MethodInfo canon;
			if (!canonicalizedGenericMethods.TryGetValue(method, out canon))
			{
				canonicalizedGenericMethods.Add(method, method);
				canon = method;
			}
			return canon;
		}

		internal void ExportTypes(int fileToken, ModuleBuilder manifestModule)
		{
			Dictionary<Type, int> declaringTypes = new Dictionary<Type, int>();
			foreach (TypeBuilder type in types)
			{
				if (type != moduleType && IsVisible(type))
				{
					TableHeap.ExportedTypeTable.Record rec = new TableHeap.ExportedTypeTable.Record();
					rec.Flags = (int)type.Attributes;
					rec.TypeDefId = type.MetadataToken & 0xFFFFFF;
					rec.TypeName = manifestModule.Strings.Add(type.Name);
					string ns = type.Namespace;
					rec.TypeNamespace = ns == null ? 0 : manifestModule.Strings.Add(ns);
					if (type.IsNested)
					{
						rec.Implementation = declaringTypes[type.DeclaringType];
					}
					else
					{
						rec.Implementation = fileToken;
					}
					int exportTypeToken = 0x27000000 | manifestModule.Tables.ExportedType.AddRecord(rec);
					declaringTypes.Add(type, exportTypeToken);
				}
			}
		}

		internal static bool IsVisible(Type type)
		{
			return type.IsPublic || ((type.IsNestedFamily || type.IsNestedFamORAssem || type.IsNestedPublic) && IsVisible(type.DeclaringType));
		}

		internal void AddConstant(int parentToken, object defaultValue)
		{
			TableHeap.ConstantTable.Record rec = new TableHeap.ConstantTable.Record();
			rec.Parent = parentToken;
			ByteBuffer val = new ByteBuffer(16);
			if (defaultValue == null)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_CLASS;
			}
			else if (defaultValue is bool)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_BOOLEAN;
				val.Write((bool)defaultValue ? (byte)1 : (byte)0);
			}
			else if (defaultValue is char)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_CHAR;
				val.Write((char)defaultValue);
			}
			else if (defaultValue is byte)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_U1;
				val.Write((byte)defaultValue);
			}
			else if (defaultValue is short)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_I2;
				val.Write((short)defaultValue);
			}
			else if (defaultValue is int)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_I4;
				val.Write((int)defaultValue);
			}
			else if (defaultValue is long)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_I8;
				val.Write((long)defaultValue);
			}
			else if (defaultValue is float)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_R4;
				val.Write((float)defaultValue);
			}
			else if (defaultValue is double)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_R8;
				val.Write((double)defaultValue);
			}
			else if (defaultValue is string)
			{
				rec.Type = SignatureHelper.ELEMENT_TYPE_STRING;
				foreach (char c in (string)defaultValue)
				{
					val.Write(c);
				}
			}
			else
			{
				throw new NotImplementedException(defaultValue.GetType().FullName);
			}
			rec.Value = this.Blobs.Add(val);
			this.Tables.Constant.AddRecord(rec);
		}

		internal bool IsModuleType(TypeBuilder type)
		{
			return type == moduleType;
		}

		ModuleBuilder ITypeOwner.ModuleBuilder
		{
			get { return this; }
		}

		public Type ResolveType(int metadataToken)
		{
			return types[(metadataToken & 0xFFFFFF) - 1];
		}

		public MethodBase ResolveMethod(int metadataToken)
		{
			// HACK if we're given a SymbolToken, we need to convert back
			if ((metadataToken & 0xFF000000) == 0x06000000)
			{
				metadataToken = -(metadataToken & 0x00FFFFFF);
			}
			// TODO this is very inefficient (but currently not used, it exists only because the symbol writer for Mono potentially needs it)
			foreach (Type type in types)
			{
				MethodBase method = ((TypeBuilder)type).LookupMethod(metadataToken);
				if (method != null)
				{
					return method;
				}
			}
			return ((TypeBuilder)moduleType).LookupMethod(metadataToken);
		}

		public string FullyQualifiedName
		{
			get
			{
				return Path.GetFullPath(fileName);
			}
		}
	}
}
