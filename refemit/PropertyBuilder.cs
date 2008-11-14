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
using IKVM.Reflection.Emit.Writer;

namespace IKVM.Reflection.Emit
{
	public class PropertyBuilder : PropertyInfo
	{
		private readonly ModuleBuilder moduleBuilder;
		private readonly int name;
		private readonly PropertyAttributes attributes;
		private readonly ByteBuffer signature;
		private int getMethodToken;
		private int setMethodToken;
		private int pseudoToken;

		internal PropertyBuilder(ModuleBuilder moduleBuilder, string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			this.moduleBuilder = moduleBuilder;
			this.name = moduleBuilder.Strings.Add(name);
			this.attributes = attributes;
			signature = new ByteBuffer(16);
			// later on we'll patch this byte, if it turns out that it is an instance property
			signature.Write(SignatureHelper.PROPERTY);
			signature.WriteCompressedInt(parameterTypes == null ? 0 : parameterTypes.Length);
			SignatureHelper.WriteType(moduleBuilder, signature, returnType);
			if (parameterTypes != null)
			{
				foreach (Type type in parameterTypes)
				{
					SignatureHelper.WriteType(moduleBuilder, signature, type);
				}
			}
		}

		public void SetGetMethod(MethodBuilder mdBuilder)
		{
			if (!mdBuilder.IsStatic)
			{
				signature.Position = 0;
				signature.Write((byte)(SignatureHelper.PROPERTY | SignatureHelper.HASTHIS));
			}
			getMethodToken = mdBuilder.MetadataToken;
		}

		public void SetSetMethod(MethodBuilder mdBuilder)
		{
			if (!mdBuilder.IsStatic)
			{
				signature.Position = 0;
				signature.Write((byte)(SignatureHelper.PROPERTY | SignatureHelper.HASTHIS));
			}
			setMethodToken = mdBuilder.MetadataToken;
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (pseudoToken == 0)
			{
				pseudoToken = moduleBuilder.AllocPseudoToken();
			}
			moduleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
		}

		public override PropertyAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}

		public override bool CanRead
		{
			get { throw new NotImplementedException(); }
		}

		public override bool CanWrite
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public override ParameterInfo[] GetIndexParameters()
		{
			throw new NotImplementedException();
		}

		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override Type PropertyType
		{
			get { throw new NotImplementedException(); }
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override Type DeclaringType
		{
			get { throw new NotImplementedException(); }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override Type ReflectedType
		{
			get { throw new NotImplementedException(); }
		}

		internal void Bake()
		{
			TableHeap.PropertyTable.Record rec = new TableHeap.PropertyTable.Record();
			rec.Flags = (short)attributes;
			rec.Name = name;
			rec.Type = moduleBuilder.Blobs.Add(signature);
			int token = 0x17000000 | moduleBuilder.Tables.Property.AddRecord(rec);

			if (pseudoToken != 0)
			{
				moduleBuilder.RegisterTokenFixup(pseudoToken, token);
			}

			if (getMethodToken != 0)
			{
				const short Getter = 0x0002;
				AddMethodSemantics(Getter, getMethodToken, token);
			}
			if (setMethodToken != 0)
			{
				const short Setter = 0x0001;
				AddMethodSemantics(Setter, setMethodToken, token);
			}
		}

		private void AddMethodSemantics(short semantics, int methodToken, int propertyToken)
		{
			TableHeap.MethodSemanticsTable.Record rec = new TableHeap.MethodSemanticsTable.Record();
			rec.Semantics = semantics;
			rec.Method = methodToken;
			rec.Association = propertyToken;
			moduleBuilder.Tables.MethodSemantics.AddRecord(rec);
		}
	}
}
