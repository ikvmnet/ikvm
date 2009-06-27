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
using System;
using System.Reflection;
using IKVM.Reflection.Emit.Writer;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Emit
{
	public sealed class EventBuilder : EventInfo
	{
		private readonly ModuleBuilder moduleBuilder;
		private readonly int name;
		private EventAttributes attributes;
		private readonly int eventtype;
		private int addOnMethodToken;
		private int removeOnMethodToken;
		private int fireMethodToken;
		private int pseudoToken;

		internal EventBuilder(ModuleBuilder moduleBuilder, string name, EventAttributes attributes, Type eventtype)
		{
			this.moduleBuilder = moduleBuilder;
			this.name = moduleBuilder.Strings.Add(name);
			this.attributes = attributes;
			this.eventtype = moduleBuilder.GetTypeToken(eventtype).Token;
		}

		public void SetAddOnMethod(MethodBuilder mdBuilder)
		{
			addOnMethodToken = mdBuilder.MetadataToken;
		}

		public void SetRemoveOnMethod(MethodBuilder mdBuilder)
		{
			removeOnMethodToken = mdBuilder.MetadataToken;
		}

		public void SetRaiseMethod(MethodBuilder mdBuilder)
		{
			fireMethodToken = mdBuilder.MetadataToken;
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Constructor.DeclaringType == typeof(SpecialNameAttribute))
			{
				attributes |= EventAttributes.SpecialName;
			}
			else
			{
				if (pseudoToken == 0)
				{
					pseudoToken = moduleBuilder.AllocPseudoToken();
				}
				moduleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
			}
		}

		public override EventAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodInfo GetAddMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo GetRaiseMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo GetRemoveMethod(bool nonPublic)
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
			TableHeap.EventTable.Record rec = new TableHeap.EventTable.Record();
			rec.EventFlags = (short)attributes;
			rec.Name = name;
			rec.EventType = eventtype;
			int token = 0x14000000 | moduleBuilder.Tables.Event.AddRecord(rec);

			if (pseudoToken != 0)
			{
				moduleBuilder.RegisterTokenFixup(pseudoToken, token);
			}

			if (addOnMethodToken != 0)
			{
				const short AddOn = 0x0008;
				AddMethodSemantics(AddOn, addOnMethodToken, token);
			}
			if (removeOnMethodToken != 0)
			{
				const short RemoveOn = 0x0010;
				AddMethodSemantics(RemoveOn, removeOnMethodToken, token);
			}
			if (fireMethodToken != 0)
			{
				const short Fire = 0x0020;
				AddMethodSemantics(Fire, fireMethodToken, token);
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
