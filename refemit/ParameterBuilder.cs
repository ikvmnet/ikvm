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
using System.Runtime.InteropServices;

namespace IKVM.Reflection.Emit
{
	public sealed class ParameterBuilder
	{
		private readonly ModuleBuilder moduleBuilder;
		private short flags;
		private readonly short sequence;
		private readonly int name;
		private int lazyPseudoToken;

		internal ParameterBuilder(ModuleBuilder moduleBuilder, int sequence, ParameterAttributes attribs, string name)
		{
			this.moduleBuilder = moduleBuilder;
			this.flags = (short)attribs;
			this.sequence = (short)sequence;
			this.name = name == null ? 0 : moduleBuilder.Strings.Add(name);
		}

		public void SetCustomAttribute(CustomAttributeBuilder customAttributeBuilder)
		{
			if (customAttributeBuilder.Constructor.DeclaringType == typeof(InAttribute))
			{
				flags |= (short)ParameterAttributes.In;
			}
			else if (customAttributeBuilder.Constructor.DeclaringType == typeof(OutAttribute))
			{
				flags |= (short)ParameterAttributes.Out;
			}
			else if (customAttributeBuilder.Constructor.DeclaringType == typeof(OptionalAttribute))
			{
				flags |= (short)ParameterAttributes.Optional;
			}
			else if (customAttributeBuilder.Constructor.DeclaringType == typeof(MarshalAsAttribute))
			{
				flags |= (short)ParameterAttributes.HasFieldMarshal;
				if (lazyPseudoToken == 0)
				{
					lazyPseudoToken = moduleBuilder.AllocPseudoToken();
				}
				TableHeap.FieldMarshalTable.Record rec = new TableHeap.FieldMarshalTable.Record();
				rec.Parent = lazyPseudoToken;
				rec.NativeType = FieldBuilder.WriteMarshallingDescriptor(moduleBuilder, customAttributeBuilder);
				moduleBuilder.Tables.FieldMarshal.AddRecord(rec);
			}
			else if (customAttributeBuilder.Constructor.DeclaringType == typeof(DefaultParameterValueAttribute))
			{
				flags |= (short)ParameterAttributes.HasDefault;
				if (lazyPseudoToken == 0)
				{
					lazyPseudoToken = moduleBuilder.AllocPseudoToken();
				}
				moduleBuilder.AddConstant(lazyPseudoToken, customAttributeBuilder.GetConstructorArgument(0));
			}
			else
			{
				if (lazyPseudoToken == 0)
				{
					lazyPseudoToken = moduleBuilder.AllocPseudoToken();
				}
				moduleBuilder.SetCustomAttribute(lazyPseudoToken, customAttributeBuilder);
			}
		}

		internal void WriteParamRecord(MetadataWriter mw)
		{
			mw.Write(flags);
			mw.Write(sequence);
			mw.WriteStringIndex(name);
		}

		internal void FixupToken(int parameterToken)
		{
			if (lazyPseudoToken != 0)
			{
				moduleBuilder.RegisterTokenFixup(lazyPseudoToken, parameterToken);
			}
		}
	}
}
