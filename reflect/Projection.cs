/*
  Copyright (C) 2013 Jeroen Frijters

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
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{
	sealed class WindowsRuntimeProjection
	{
		private static readonly Dictionary<TypeName, Mapping> projections = new Dictionary<TypeName, Mapping>();
		private readonly ModuleReader module;
		private readonly Dictionary<int, string> strings;
		private readonly Dictionary<string, int> added = new Dictionary<string,int>();
		private readonly int[] assemblyRefTokens = new int[(int)ProjectionAssembly.Count];
		private int typeofSystemAttribute = -1;
		private int typeofSystemAttributeUsageAttribute = -1;
		private int typeofSystemEnum = -1;
		private int typeofSystemIDisposable = -1;
		private int typeofSystemMulticastDelegate = -1;
		private int typeofWindowsFoundationMetadataAllowMultipleAttribute = -1;
		private bool[] projectedTypeRefs;

		enum ProjectionAssembly
		{
			System_Runtime,
			System_Runtime_InteropServices_WindowsRuntime,
			System_ObjectModel,
			System_Runtime_WindowsRuntime,
			System_Runtime_WindowsRuntime_UI_Xaml,

			Count
		}

		sealed class Mapping
		{
			internal readonly ProjectionAssembly Assembly;
			internal readonly string TypeNamespace;
			internal readonly string TypeName;

			internal Mapping(ProjectionAssembly assembly, string typeNamespace, string typeName)
			{
				this.Assembly = assembly;
				this.TypeNamespace = typeNamespace;
				this.TypeName = typeName;
			}
		}

		static WindowsRuntimeProjection()
		{
			projections.Add(new TypeName("System", "Attribute"), new Mapping(ProjectionAssembly.System_Runtime, "System", "Attribute"));
			projections.Add(new TypeName("System", "MulticastDelegate"), new Mapping(ProjectionAssembly.System_Runtime, "System", "MulticastDelegate"));
			projections.Add(new TypeName("Windows.Foundation", "DateTime"), new Mapping(ProjectionAssembly.System_Runtime, "System", "DateTimeOffset"));
			projections.Add(new TypeName("Windows.Foundation", "EventHandler`1"), new Mapping(ProjectionAssembly.System_Runtime, "System", "EventHandler`1"));
			projections.Add(new TypeName("Windows.Foundation", "EventRegistrationToken"), new Mapping(ProjectionAssembly.System_Runtime_InteropServices_WindowsRuntime, "System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationToken"));
			projections.Add(new TypeName("Windows.Foundation", "HResult"), new Mapping(ProjectionAssembly.System_Runtime, "System", "Exception"));
			projections.Add(new TypeName("Windows.Foundation", "IClosable"), new Mapping(ProjectionAssembly.System_Runtime, "System", "IDisposable"));
			projections.Add(new TypeName("Windows.Foundation", "IReference`1"), new Mapping(ProjectionAssembly.System_Runtime, "System", "Nullable`1"));
			projections.Add(new TypeName("Windows.Foundation", "Point"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime, "Windows.Foundation", "Point"));
			projections.Add(new TypeName("Windows.Foundation", "Rect"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime, "Windows.Foundation", "Rect"));
			projections.Add(new TypeName("Windows.Foundation", "Size"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime, "Windows.Foundation", "Size"));
			projections.Add(new TypeName("Windows.Foundation", "TimeSpan"), new Mapping(ProjectionAssembly.System_Runtime, "System", "TimeSpan"));
			projections.Add(new TypeName("Windows.Foundation", "Uri"), new Mapping(ProjectionAssembly.System_Runtime, "System", "Uri"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IIterable`1"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "IEnumerable`1"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IKeyValuePair`2"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "KeyValuePair`2"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IMap`2"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "IDictionary`2"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IMapView`2"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "IReadOnlyDictionary`2"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IVector`1"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "IList`1"));
			projections.Add(new TypeName("Windows.Foundation.Collections", "IVectorView`1"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections.Generic", "IReadOnlyList`1"));
			projections.Add(new TypeName("Windows.Foundation.Metadata", "AttributeTargets"), new Mapping(ProjectionAssembly.System_Runtime, "System", "AttributeTargets"));
			projections.Add(new TypeName("Windows.Foundation.Metadata", "AttributeUsageAttribute"), new Mapping(ProjectionAssembly.System_Runtime, "System", "AttributeUsageAttribute"));
			projections.Add(new TypeName("Windows.UI", "Color"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime, "Windows.UI", "Color"));
			projections.Add(new TypeName("Windows.UI.Xaml", "CornerRadius"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "CornerRadius"));
			projections.Add(new TypeName("Windows.UI.Xaml", "Duration"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "Duration"));
			projections.Add(new TypeName("Windows.UI.Xaml", "DurationType"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "DurationType"));
			projections.Add(new TypeName("Windows.UI.Xaml", "GridLength"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "GridLength"));
			projections.Add(new TypeName("Windows.UI.Xaml", "GridUnitType"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "GridUnitType"));
			projections.Add(new TypeName("Windows.UI.Xaml", "Thickness"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml", "Thickness"));
			projections.Add(new TypeName("Windows.UI.Xaml.Controls.Primitives", "GeneratorPosition"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Controls.Primitives", "GeneratorPosition"));
			projections.Add(new TypeName("Windows.UI.Xaml.Data", "INotifyPropertyChanged"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.ComponentModel", "INotifyPropertyChanged"));
			projections.Add(new TypeName("Windows.UI.Xaml.Data", "PropertyChangedEventArgs"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.ComponentModel", "PropertyChangedEventArgs"));
			projections.Add(new TypeName("Windows.UI.Xaml.Data", "PropertyChangedEventHandler"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.ComponentModel", "PropertyChangedEventHandler"));
			projections.Add(new TypeName("Windows.UI.Xaml.Input", "ICommand"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.Windows.Input", "ICommand"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "IBindableIterable"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections", "IEnumerable"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "IBindableVector"), new Mapping(ProjectionAssembly.System_Runtime, "System.Collections", "IList"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "NotifyCollectionChangedAction"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.Collections.Specialized", "NotifyCollectionChangedAction"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "NotifyCollectionChangedEventArgs"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.Collections.Specialized", "NotifyCollectionChangedEventArgs"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "NotifyCollectionChangedEventHandler"), new Mapping(ProjectionAssembly.System_ObjectModel, "System.Collections.Specialized", "NotifyCollectionChangedEventHandler"));
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "TypeName"), new Mapping(ProjectionAssembly.System_Runtime, "System", "Type"));
			projections.Add(new TypeName("Windows.UI.Xaml.Media", "Matrix"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Media", "Matrix"));
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Animation", "KeyTime"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Media.Animation", "KeyTime"));
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Animation", "RepeatBehavior"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Media.Animation", "RepeatBehavior"));
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Animation", "RepeatBehaviorType"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Media.Animation", "RepeatBehaviorType"));
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Media3D", "Matrix3D"), new Mapping(ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml, "Windows.UI.Xaml.Media.Media3D", "Matrix3D"));

			// hidden types
			projections.Add(new TypeName("Windows.Foundation", "IPropertyValue"), null);
			projections.Add(new TypeName("Windows.Foundation", "IReferenceArray`1"), null);
			projections.Add(new TypeName("Windows.Foundation.Metadata", "GCPressureAmount"), null);
			projections.Add(new TypeName("Windows.Foundation.Metadata", "GCPressureAttribute"), null);
			projections.Add(new TypeName("Windows.UI.Xaml", "CornerRadiusHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml", "DurationHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml", "GridLengthHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml", "ThicknessHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Controls.Primitives", "GeneratorPositionHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Interop", "INotifyCollectionChanged"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Media", "MatrixHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Animation", "KeyTimeHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Animation", "RepeatBehaviorHelper"), null);
			projections.Add(new TypeName("Windows.UI.Xaml.Media.Media3D", "Matrix3DHelper"), null);
		}

		private WindowsRuntimeProjection(ModuleReader module, Dictionary<int, string> strings)
		{
			this.module = module;
			this.strings = strings;
		}

		internal static void Patch(ModuleReader module, Dictionary<int, string> strings, ref string imageRuntimeVersion, ref byte[] blobHeap)
		{
			if (!module.CustomAttribute.Sorted)
			{
				// HasAllowMultipleAttribute requires this
				throw new NotImplementedException("CustomAttribute table must be sorted");
			}

			bool clr = imageRuntimeVersion.Contains(";");
			if (clr)
			{
				imageRuntimeVersion = imageRuntimeVersion.Substring(imageRuntimeVersion.IndexOf(';') + 1);
				if (imageRuntimeVersion.StartsWith("CLR", StringComparison.OrdinalIgnoreCase))
				{
					imageRuntimeVersion = imageRuntimeVersion.Substring(3);
				}
				imageRuntimeVersion = imageRuntimeVersion.TrimStart(' ');
			}
			else
			{
				Assembly mscorlib = module.universe.Mscorlib;
				imageRuntimeVersion = mscorlib.__IsMissing ? "v4.0.30319" : mscorlib.ImageRuntimeVersion;
			}

			WindowsRuntimeProjection obj = new WindowsRuntimeProjection(module, strings);
			obj.PatchAssemblyRef(ref blobHeap);
			obj.PatchTypeRef();
			obj.PatchTypes(clr);
			obj.PatchMethodImpl();
			obj.PatchCustomAttribute(ref blobHeap);
		}

		private void PatchAssemblyRef(ref byte[] blobHeap)
		{
			AssemblyRefTable assemblyRefs = module.AssemblyRef;
			for (int i = 0; i < assemblyRefs.records.Length; i++)
			{
				if (module.GetString(assemblyRefs.records[i].Name) == "mscorlib")
				{
					Version ver = GetMscorlibVersion();
					assemblyRefs.records[i].MajorVersion = (ushort)ver.Major;
					assemblyRefs.records[i].MinorVersion = (ushort)ver.Minor;
					assemblyRefs.records[i].BuildNumber = (ushort)ver.Build;
					assemblyRefs.records[i].RevisionNumber = (ushort)ver.Revision;
					break;
				}
			}

			int publicKeyTokenMicrosoft = AddBlob(ref blobHeap, new byte[] { 0xB0, 0x3F, 0x5F, 0x7F, 0x11, 0xD5, 0x0A, 0x3A });
			int publicKeyTokenEcma = AddBlob(ref blobHeap, new byte[] { 0xB7, 0x7A, 0x5C, 0x56, 0x19, 0x34, 0xE0, 0x89 });
			assemblyRefTokens[(int)ProjectionAssembly.System_Runtime] = AddAssemblyReference("System.Runtime", publicKeyTokenMicrosoft);
			assemblyRefTokens[(int)ProjectionAssembly.System_Runtime_InteropServices_WindowsRuntime] = AddAssemblyReference("System.Runtime.InteropServices.WindowsRuntime", publicKeyTokenMicrosoft);
			assemblyRefTokens[(int)ProjectionAssembly.System_ObjectModel] = AddAssemblyReference("System.ObjectModel", publicKeyTokenMicrosoft);
			assemblyRefTokens[(int)ProjectionAssembly.System_Runtime_WindowsRuntime] = AddAssemblyReference("System.Runtime.WindowsRuntime", publicKeyTokenEcma);
			assemblyRefTokens[(int)ProjectionAssembly.System_Runtime_WindowsRuntime_UI_Xaml] = AddAssemblyReference("System.Runtime.WindowsRuntime.UI.Xaml", publicKeyTokenEcma);
		}

		private void PatchTypeRef()
		{
			TypeRefTable.Record[] typeRefs = module.TypeRef.records;
			projectedTypeRefs = new bool[typeRefs.Length];
			for (int i = 0; i < typeRefs.Length; i++)
			{
				Mapping mapping;
				TypeName typeName = GetTypeRefName(i);
				projections.TryGetValue(typeName, out mapping);
				if (mapping != null)
				{
					typeRefs[i].ResolutionScope = assemblyRefTokens[(int)mapping.Assembly];
					typeRefs[i].TypeNamespace = GetString(mapping.TypeNamespace);
					typeRefs[i].TypeName = GetString(mapping.TypeName);
					projectedTypeRefs[i] = true;
				}
				switch (typeName.Namespace)
				{
					case "System":
						switch (typeName.Name)
						{
							case "Attribute":
								typeofSystemAttribute = (TypeRefTable.Index << 24) + i + 1;
								break;
							case "Enum":
								typeofSystemEnum = (TypeRefTable.Index << 24) + i + 1;
								break;
							case "MulticastDelegate":
								typeofSystemMulticastDelegate = (TypeRefTable.Index << 24) + i + 1;
								break;
						}
						break;
					case "Windows.Foundation":
						switch (typeName.Name)
						{
							case "IClosable":
								typeofSystemIDisposable = (TypeRefTable.Index << 24) + i + 1;
								break;
						}
						break;
					case "Windows.Foundation.Metadata":
						switch (typeName.Name)
						{
							case "AllowMultipleAttribute":
								typeofWindowsFoundationMetadataAllowMultipleAttribute = (TypeRefTable.Index << 24) + i + 1;
								break;
							case "AttributeUsageAttribute":
								typeofSystemAttributeUsageAttribute = (TypeRefTable.Index << 24) + i + 1;
								break;
						}
						break;
				}
			}
		}

		private void PatchTypes(bool clr)
		{
			TypeDefTable.Record[] types = module.TypeDef.records;
			MethodDefTable.Record[] methods = module.MethodDef.records;
			FieldTable.Record[] fields = module.Field.records;
			for (int i = 0; i < types.Length; i++)
			{
				TypeAttributes attr = (TypeAttributes)types[i].Flags;
				if ((attr & TypeAttributes.WindowsRuntime) != 0)
				{
					if (clr && (attr & (TypeAttributes.VisibilityMask | TypeAttributes.WindowsRuntime | TypeAttributes.Interface)) == (TypeAttributes.Public | TypeAttributes.WindowsRuntime))
					{
						types[i].TypeName = GetString("<WinRT>" + module.GetString(types[i].TypeName));
						types[i].Flags &= (int)~TypeAttributes.Public;
					}

					if (types[i].Extends != typeofSystemAttribute && (!clr || (attr & TypeAttributes.Interface) == 0))
					{
						types[i].Flags |= (int)TypeAttributes.Import;
					}
					if (projections.ContainsKey(GetTypeDefName(i)))
					{
						types[i].Flags &= (int)~TypeAttributes.Public;
					}

					int endOfMethodList = i == types.Length - 1 ? methods.Length : types[i + 1].MethodList - 1;
					for (int j = types[i].MethodList - 1; j < endOfMethodList; j++)
					{
						if (types[i].Extends == typeofSystemMulticastDelegate)
						{
							if (module.GetString(methods[j].Name) == ".ctor")
							{
								methods[j].Flags &= (int)~MethodAttributes.MemberAccessMask;
								methods[j].Flags |= (int)MethodAttributes.Public;
							}
						}
						else if (methods[j].RVA == 0)
						{
							methods[j].ImplFlags = (int)(MethodImplAttributes.Runtime | MethodImplAttributes.Managed | MethodImplAttributes.InternalCall);
						}
					}

					if (types[i].Extends == typeofSystemEnum)
					{
						int endOfFieldList = i == types.Length - 1 ? fields.Length : types[i + 1].FieldList - 1;
						for (int j = types[i].FieldList - 1; j < endOfFieldList; j++)
						{
							fields[j].Flags &= (int)~FieldAttributes.FieldAccessMask;
							fields[j].Flags |= (int)FieldAttributes.Public;
						}
					}
				}
				else if (clr && (attr & (TypeAttributes.VisibilityMask | TypeAttributes.SpecialName)) == (TypeAttributes.NotPublic | TypeAttributes.SpecialName))
				{
					string name = module.GetString(types[i].TypeName);
					if (name.StartsWith("<CLR>", StringComparison.Ordinal))
					{
						types[i].TypeName = GetString(name.Substring(5));
						types[i].Flags |= (int)TypeAttributes.Public;
						types[i].Flags &= (int)~TypeAttributes.SpecialName;
					}
				}
			}
		}

		private void PatchMethodImpl()
		{
			MethodImplTable.Record[] methodImpls = module.MethodImpl.records;
			MemberRefTable.Record[] memberRefs = module.MemberRef.records;
			MethodDefTable.Record[] methods = module.MethodDef.records;
			int[] typeSpecs = module.TypeSpec.records;
			for (int i = 0; i < methodImpls.Length; i++)
			{
				int methodDefOrMemberRef = methodImpls[i].MethodDeclaration;
				if ((methodDefOrMemberRef >> 24) == MemberRefTable.Index)
				{
					int typeDefOrRef = memberRefs[(methodDefOrMemberRef & 0xFFFFFF) - 1].Class;
					if ((typeDefOrRef >> 24) == TypeSpecTable.Index)
					{
						typeDefOrRef = ReadTypeSpec(module.GetBlob(typeSpecs[(typeDefOrRef & 0xFFFFFF) - 1]));
					}
					if ((typeDefOrRef >> 24) == TypeRefTable.Index)
					{
						if (typeDefOrRef == typeofSystemIDisposable)
						{
							int dispose = GetString("Dispose");
							methods[(methodImpls[i].MethodBody & 0xFFFFFF) - 1].Name = dispose;
							memberRefs[(methodImpls[i].MethodDeclaration & 0xFFFFFF) - 1].Name = dispose;
						}
						else if (projectedTypeRefs[(typeDefOrRef & 0xFFFFFF) - 1])
						{
							methods[(methodImpls[i].MethodBody & 0xFFFFFF) - 1].Flags &= (int)~MethodAttributes.MemberAccessMask;
							methods[(methodImpls[i].MethodBody & 0xFFFFFF) - 1].Flags |= (int)MethodAttributes.Private;
							methodImpls[i].MethodBody = 0;
							methodImpls[i].MethodDeclaration = 0;
						}
					}
					else if ((typeDefOrRef >> 24) == TypeDefTable.Index)
					{
					}
					else if ((typeDefOrRef >> 24) == TypeSpecTable.Index)
					{
						throw new NotImplementedException();
					}
					else
					{
						throw new BadImageFormatException();
					}
				}
			}
		}

		private void PatchCustomAttribute(ref byte[] blobHeap)
		{
			MemberRefTable.Record[] memberRefs = module.MemberRef.records;
			int ctorSystemAttributeUsageAttribute = -1;
			int ctorWindowsFoundationMetadataAllowMultipleAttribute = -1;
			for (int i = 0; i < memberRefs.Length; i++)
			{
				if (memberRefs[i].Class == typeofSystemAttributeUsageAttribute
					&& module.GetString(memberRefs[i].Name) == ".ctor")
				{
					ctorSystemAttributeUsageAttribute = (MemberRefTable.Index << 24) + i + 1;
				}
				else if (memberRefs[i].Class == typeofWindowsFoundationMetadataAllowMultipleAttribute
					&& module.GetString(memberRefs[i].Name) == ".ctor")
				{
					ctorWindowsFoundationMetadataAllowMultipleAttribute = (MemberRefTable.Index << 24) + i + 1;
				}
			}

			if (ctorSystemAttributeUsageAttribute != -1)
			{
				CustomAttributeTable.Record[] customAttributes = module.CustomAttribute.records;
				Dictionary<int, int> map = new Dictionary<int, int>();
				for (int i = 0; i < customAttributes.Length; i++)
				{
					if (customAttributes[i].Type == ctorSystemAttributeUsageAttribute)
					{
						ByteReader br = module.GetBlob(customAttributes[i].Value);
						br.ReadInt16();
						AttributeTargets targets = MapAttributeTargets(br.ReadInt32());
						if ((targets & AttributeTargets.Method) != 0)
						{
							// apart from the two types special cased below, Method implies Constructor
							targets |= AttributeTargets.Constructor;
							if (customAttributes[i].Parent >> 24 == TypeDefTable.Index)
							{
								TypeName typeName = GetTypeDefName((customAttributes[i].Parent & 0xFFFFFF) - 1);
								if (typeName.Namespace == "Windows.Foundation.Metadata" && (typeName.Name == "OverloadAttribute" || typeName.Name == "DefaultOverloadAttribute"))
								{
									targets &= ~AttributeTargets.Constructor;
								}
							}
						}
						customAttributes[i].Value = GetAttributeUsageAttributeBlob(ref blobHeap, map, targets, HasAllowMultipleAttribute(customAttributes, i, ctorWindowsFoundationMetadataAllowMultipleAttribute));
					}
				}
			}
		}

		private int AddAssemblyReference(string name, int publicKeyToken)
		{
			AssemblyRefTable.Record rec;
			Version ver = GetMscorlibVersion();
			rec.MajorVersion = (ushort)ver.Major;
			rec.MinorVersion = (ushort)ver.Minor;
			rec.BuildNumber = (ushort)ver.Build;
			rec.RevisionNumber = (ushort)ver.Revision;
			rec.Flags = 0;
			rec.PublicKeyOrToken = publicKeyToken;
			rec.Name = GetString(name);
			rec.Culture = 0;
			rec.HashValue = 0;
			int token = 0x23000000 | module.AssemblyRef.FindOrAddRecord(rec);
			Array.Resize(ref module.AssemblyRef.records, module.AssemblyRef.RowCount);
			return token;
		}

		private TypeName GetTypeRefName(int index)
		{
			return new TypeName(module.GetString(module.TypeRef.records[index].TypeNamespace), module.GetString(module.TypeRef.records[index].TypeName));
		}

		private TypeName GetTypeDefName(int index)
		{
			return new TypeName(module.GetString(module.TypeDef.records[index].TypeNamespace), module.GetString(module.TypeDef.records[index].TypeName));
		}

		private int GetString(string str)
		{
			int index;
			if (!added.TryGetValue(str, out index))
			{
				index = -(added.Count + 1);
				added.Add(str, index);
				strings.Add(index, str);
			}
			return index;
		}

		private Version GetMscorlibVersion()
		{
			Assembly mscorlib = module.universe.Mscorlib;
			return mscorlib.__IsMissing ? new Version(4, 0, 0, 0) : mscorlib.GetName().Version;
		}

		private static bool HasAllowMultipleAttribute(CustomAttributeTable.Record[] customAttributes, int i, int ctorWindowsFoundationMetadataAllowMultipleAttribute)
		{
			// we can assume that the CustomAttribute table is sorted, because we've checked the Sorted flag earlier
			int owner = customAttributes[i].Parent;
			while (i > 0 && customAttributes[i - 1].Parent == owner)
			{
				i--;
			}
			while (i < customAttributes.Length && customAttributes[i].Parent == owner)
			{
				if (customAttributes[i].Type == ctorWindowsFoundationMetadataAllowMultipleAttribute)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		private static AttributeTargets MapAttributeTargets(int targets)
		{
			if (targets == -1)
			{
				return AttributeTargets.All;
			}
			AttributeTargets result = 0;
			if ((targets & 1) != 0)
			{
				result |= AttributeTargets.Delegate;
			}
			if ((targets & 2) != 0)
			{
				result |= AttributeTargets.Enum;
			}
			if ((targets & 4) != 0)
			{
				result |= AttributeTargets.Event;
			}
			if ((targets & 8) != 0)
			{
				result |= AttributeTargets.Field;
			}
			if ((targets & 16) != 0)
			{
				result |= AttributeTargets.Interface;
			}
			if ((targets & 64) != 0)
			{
				result |= AttributeTargets.Method;
			}
			if ((targets & 128) != 0)
			{
				result |= AttributeTargets.Parameter;
			}
			if ((targets & 256) != 0)
			{
				result |= AttributeTargets.Property;
			}
			if ((targets & 512) != 0)
			{
				result |= AttributeTargets.Class;
			}
			if ((targets & 1024) != 0)
			{
				result |= AttributeTargets.Struct;
			}
			return result;
		}

		private static int GetAttributeUsageAttributeBlob(ref byte[] blobHeap, Dictionary<int, int> map, AttributeTargets targets, bool allowMultiple)
		{
			int key = (int)targets;
			if (allowMultiple)
			{
				key |= unchecked((int)0x80000000);
			}
			int blob;
			if (!map.TryGetValue(key, out blob))
			{
				blob = AddBlob(ref blobHeap, new byte[] { 0x01, 0x00, (byte)targets, (byte)((int)targets >> 8), (byte)((int)targets >> 16), (byte)((int)targets >> 24),
					0x01, 0x00, 0x54, 0x02, 0x0D, 0x41, 0x6C, 0x6C, 0x6F, 0x77, 0x4D, 0x75, 0x6C, 0x74, 0x69, 0x70, 0x6C, 0x65, allowMultiple ? (byte)0x01 : (byte)0x00 });
				map.Add(key, blob);
			}
			return blob;
		}

		private static int ReadTypeSpec(ByteReader br)
		{
			if (br.ReadByte() != Signature.ELEMENT_TYPE_GENERICINST)
			{
				throw new NotImplementedException("Expected ELEMENT_TYPE_GENERICINST");
			}
			switch (br.ReadByte())
			{
				case Signature.ELEMENT_TYPE_CLASS:
				case Signature.ELEMENT_TYPE_VALUETYPE:
					break;
				default:
					throw new NotImplementedException("Expected ELEMENT_TYPE_CLASS or ELEMENT_TYPE_VALUETYPE");
			}
			int encoded = br.ReadCompressedUInt();
			switch (encoded & 3)
			{
				case 0:
					return (TypeDefTable.Index << 24) + (encoded >> 2);
				case 1:
					return (TypeRefTable.Index << 24) + (encoded >> 2);
				case 2:
					return (TypeSpecTable.Index << 24) + (encoded >> 2);
				default:
					throw new BadImageFormatException();
			}
		}

		private static int AddBlob(ref byte[] blobHeap, byte[] blob)
		{
			if (blob.Length > 127)
			{
				throw new NotImplementedException();
			}
			int offset = blobHeap.Length;
			Array.Resize(ref blobHeap, offset + blob.Length + 1);
			blobHeap[offset] = (byte)blob.Length;
			Buffer.BlockCopy(blob, 0, blobHeap, offset + 1, blob.Length);
			return offset;
		}

		internal static bool IsProjectedValueType(string ns, string name, Module module)
		{
			return ((ns == "System.Collections.Generic" && name == "KeyValuePair`2")
					|| (ns == "System" && name == "Nullable`1"))
				&& module.Assembly.GetName().Name == "System.Runtime";
		}

		internal static bool IsProjectedReferenceType(string ns, string name, Module module)
		{
			return ((ns == "System" && name == "Exception")
					|| (ns == "System" && name == "Type"))
				&& module.Assembly.GetName().Name == "System.Runtime";
		}
	}
}
