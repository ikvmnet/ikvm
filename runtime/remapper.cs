/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace MapXml
{
	public abstract class Instruction
	{
		internal abstract void Generate(Hashtable context, ILGenerator ilgen);
	}

	[XmlType("ldstr")]
	public sealed class Ldstr : Instruction
	{
		[XmlAttribute("value")]
		public string Value;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldstr, Value);
		}
	}

	[XmlType("ldnull")]
	public sealed class Ldnull : Simple
	{
		public Ldnull() : base(OpCodes.Ldnull)
		{
		}
	}

	[XmlType("call")]
	public class Call : Instruction
	{
		public Call() : this(OpCodes.Call)
		{
		}

		internal Call(OpCode opcode)
		{
			this.opcode = opcode;
		}

		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("type")]
		public string type;
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;

		private CodeEmitter emitter;
		private OpCode opcode;

		internal sealed override void Generate(Hashtable context, ILGenerator ilgen)
		{
			if(emitter == null)
			{
				Debug.Assert(Name != null);
				if(Name == ".ctor")
				{
					Debug.Assert(Class == null && type != null);
					Type[] argTypes = ClassLoaderWrapper.GetBootstrapClassLoader().ArgTypeListFromSig(Sig);
					emitter = CodeEmitter.Create(opcode, Type.GetType(type, true).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, argTypes, null));
				}
				else
				{
					Debug.Assert(Class == null ^ type == null);
					if(Class != null)
					{
						Debug.Assert(Sig != null);
						MethodWrapper method = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Class).GetMethodWrapper(MethodDescriptor.FromNameSig(ClassLoaderWrapper.GetBootstrapClassLoader(), Name, Sig), false);
						if(method == null)
						{
							throw new InvalidOperationException("method not found: " + Class + "." + Name + Sig);
						}
						if(opcode.Value == OpCodes.Call.Value)
						{
							emitter = method.EmitCall;
						}
						else if(opcode.Value == OpCodes.Callvirt.Value)
						{
							emitter = method.EmitCallvirt;
						}
						else if(opcode.Value == OpCodes.Newobj.Value)
						{
							emitter = method.EmitNewobj;
						}
						else
						{
							throw new InvalidOperationException();
						}
						// TODO this code is part of what Compiler.CastInterfaceArgs (in compiler.cs) does,
						// it would be nice if we could avoid this duplication...
						TypeWrapper[] argTypeWrappers = method.Descriptor.ArgTypeWrappers;
						for(int i = 0; i < argTypeWrappers.Length; i++)
						{
							if(argTypeWrappers[i].IsGhost)
							{
								LocalBuilder[] temps = new LocalBuilder[argTypeWrappers.Length + (method.IsStatic ? 0 : 1)];
								for(int j = temps.Length - 1; j >= 0; j--)
								{
									TypeWrapper tw;
									if(method.IsStatic)
									{
										tw = argTypeWrappers[j];
									}
									else
									{
										if(j == 0)
										{
											tw = method.DeclaringType;
										}
										else
										{
											tw = argTypeWrappers[j - 1];
										}
									}
									if(tw.IsGhost)
									{
										tw.EmitConvStackToParameterType(ilgen, tw);
									}
									temps[j] = ilgen.DeclareLocal(tw.TypeAsParameterType);
									ilgen.Emit(OpCodes.Stloc, temps[j]);
								}
								for(int j = 0; j < temps.Length; j++)
								{
									ilgen.Emit(OpCodes.Ldloc, temps[j]);
								}
								break;
							}
						}
					}
					else
					{
						if(Sig != null)
						{
							Type[] argTypes = ClassLoaderWrapper.GetBootstrapClassLoader().ArgTypeListFromSig(Sig);
							emitter = CodeEmitter.Create(opcode, Type.GetType(type, true).GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, argTypes, null));
						}
						else
						{
							emitter = CodeEmitter.Create(opcode, Type.GetType(type, true).GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
						}
					}
				}
				if(emitter == null)
				{
					// TODO better error handling
					throw new InvalidOperationException("method not found: " + Name);
				}
			}
			emitter.Emit(ilgen);
		}
	}

	[XmlType("callvirt")]
	public sealed class Callvirt : Call
	{
		public Callvirt() : base(OpCodes.Callvirt)
		{
		}
	}

	[XmlType("newobj")]
	public sealed class NewObj : Call
	{
		public NewObj() : base(OpCodes.Newobj)
		{
		}
	}

	public abstract class Simple : Instruction
	{
		private OpCode opcode;

		public Simple(OpCode opcode)
		{
			this.opcode = opcode;
		}

		internal sealed override void Generate(Hashtable context, ILGenerator ilgen)
		{
			ilgen.Emit(opcode);
		}
	}

	[XmlType("dup")]
	public sealed class Dup : Simple
	{
		public Dup() : base(OpCodes.Dup)
		{
		}
	}

	[XmlType("pop")]
	public sealed class Pop : Simple
	{
		public Pop() : base(OpCodes.Pop)
		{
		}
	}

	public abstract class TypeOrTypeWrapperInstruction : Instruction
	{
		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("type")]
		public string type;

		internal TypeWrapper typeWrapper;
		internal Type typeType;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			if(typeWrapper == null && typeType == null)
			{
				Debug.Assert(Class == null ^ type == null);
				if(Class != null)
				{
					typeWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Class);
				}
				else
				{
					typeType = Type.GetType(type, true);
				}
			}
		}
	}

	[XmlType("isinst")]
	public sealed class IsInst : TypeOrTypeWrapperInstruction
	{
		public IsInst()
		{
		}
	
		// TODO isinst is broken, because for Java types it returns true/false while for
		// .NET types it returns null/reference.
		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			base.Generate(context, ilgen);
			if(typeType != null)
			{
				ilgen.Emit(OpCodes.Isinst, typeType);
			}
			else
			{
				// NOTE we pass a null context, but that shouldn't be a problem, because
				// typeWrapper should never be an UnloadableTypeWrapper
				typeWrapper.EmitInstanceOf(null, ilgen);
			}
		}
	}

	[XmlType("castclass")]
	public sealed class Castclass : TypeOrTypeWrapperInstruction
	{
		public Castclass()
		{
		}

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			base.Generate(context, ilgen);
			if(typeType != null)
			{
				ilgen.Emit(OpCodes.Castclass, typeType);
			}
			else
			{
				// NOTE we pass a null context, but that shouldn't be a problem, because
				// typeWrapper should never be an UnloadableTypeWrapper
				typeWrapper.EmitCheckcast(null, ilgen);
			}
		}
	}

	public abstract class TypeInstruction : Instruction
	{
		[XmlAttribute("type")]
		public string type;

		private OpCode opcode;
		private Type typeType;

		internal TypeInstruction(OpCode opcode)
		{
			this.opcode = opcode;
		}

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			if(typeType == null)
			{
				Debug.Assert(type != null);
				typeType = Type.GetType(type, true);
			}
			ilgen.Emit(opcode, typeType);
		}
	}

	[XmlType("unbox")]
	public sealed class Unbox : TypeInstruction
	{
		public Unbox() : base(OpCodes.Unbox)
		{
		}
	}

	[XmlType("box")]
	public sealed class Box : TypeInstruction
	{
		public Box() : base(OpCodes.Box)
		{
		}
	}

	public abstract class Branch : Instruction
	{
		private OpCode opcode;

		public Branch(OpCode opcode)
		{
			this.opcode = opcode;
		}

		internal sealed override void Generate(Hashtable context, ILGenerator ilgen)
		{
			Label l;
			if(context[Name] == null)
			{
				l = ilgen.DefineLabel();
				context[Name] = l;
			}
			else
			{
				l = (Label)context[Name];
			}
			ilgen.Emit(opcode, l);
		}

		[XmlAttribute("name")]
		public string Name;
	}

	[XmlType("brfalse")]
	public sealed class BrFalse : Branch
	{
		public BrFalse() : base(OpCodes.Brfalse)
		{
		}
	}

	[XmlType("brtrue")]
	public sealed class BrTrue : Branch
	{
		public BrTrue() : base(OpCodes.Brtrue)
		{
		}
	}

	[XmlType("br")]
	public sealed class Br : Branch
	{
		public Br() : base(OpCodes.Br)
		{
		}
	}

	[XmlType("bge_un")]
	public sealed class Bge_Un : Branch
	{
		public Bge_Un() : base(OpCodes.Bge_Un)
		{
		}
	}

	[XmlType("ble_un")]
	public sealed class Ble_Un : Branch
	{
		public Ble_Un() : base(OpCodes.Ble_Un)
		{
		}
	}

	[XmlType("label")]
	public sealed class BrLabel : Instruction
	{
		[XmlAttribute("name")]
		public string Name;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			Label l;
			if(context[Name] == null)
			{
				l = ilgen.DefineLabel();
				context[Name] = l;
			}
			else
			{
				l = (Label)context[Name];
			}
			ilgen.MarkLabel(l);
		}
	}

	[XmlType("stloc")]
	public sealed class StLoc : Instruction
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("type")]
		public string type;

		private TypeWrapper typeWrapper;
		private Type typeType;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			LocalBuilder lb = (LocalBuilder)context[Name];
			if(lb == null)
			{
				if(typeWrapper == null && typeType == null)
				{
					Debug.Assert(Class == null ^ type == null);
					if(type != null)
					{
						typeType = Type.GetType(type, true);
					}
					else
					{
						typeWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Class);
					}
				}
				lb = ilgen.DeclareLocal(typeType != null ? typeType : typeWrapper.TypeAsTBD);
				context[Name] = lb;
			}
			ilgen.Emit(OpCodes.Stloc, lb);
		}
	}

	[XmlType("ldloc")]
	public sealed class LdLoc : Instruction
	{
		[XmlAttribute("name")]
		public string Name;
		
		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldloc, (LocalBuilder)context[Name]);
		}
	}

	[XmlType("ldarg_0")]
	public sealed class LdArg_0 : Simple
	{
		public LdArg_0() : base(OpCodes.Ldarg_0)
		{
		}
	}

	[XmlType("ldarg_1")]
	public sealed class LdArg_1 : Simple
	{
		public LdArg_1() : base(OpCodes.Ldarg_1)
		{
		}
	}

	[XmlType("ldarg_2")]
	public sealed class LdArg_2 : Simple
	{
		public LdArg_2() : base(OpCodes.Ldarg_2)
		{
		}
	}

	[XmlType("ldarg_3")]
	public sealed class LdArg_3 : Simple
	{
		public LdArg_3() : base(OpCodes.Ldarg_3)
		{
		}
	}

	[XmlType("ldind_i1")]
	public sealed class Ldind_i1 : Simple
	{
		public Ldind_i1() : base(OpCodes.Ldind_I1)
		{
		}
	}

	[XmlType("ldind_i2")]
	public sealed class Ldind_i2 : Simple
	{
		public Ldind_i2() : base(OpCodes.Ldind_I2)
		{
		}
	}

	[XmlType("ldind_i4")]
	public sealed class Ldind_i4 : Simple
	{
		public Ldind_i4() : base(OpCodes.Ldind_I4)
		{
		}
	}

	[XmlType("ldind_i8")]
	public sealed class Ldind_i8 : Simple
	{
		public Ldind_i8() : base(OpCodes.Ldind_I8)
		{
		}
	}

	[XmlType("ldind_r4")]
	public sealed class Ldind_r4 : Simple
	{
		public Ldind_r4() : base(OpCodes.Ldind_R4)
		{
		}
	}

	[XmlType("ldind_r8")]
	public sealed class Ldind_r8 : Simple
	{
		public Ldind_r8() : base(OpCodes.Ldind_R8)
		{
		}
	}

	[XmlType("ret")]
	public sealed class Ret : Simple
	{
		public Ret() : base(OpCodes.Ret)
		{
		}
	}

	[XmlType("throw")]
	public sealed class Throw : Simple
	{
		public Throw() : base(OpCodes.Throw)
		{
		}
	}

	[XmlType("stsfld")]
	public sealed class Stsfld : Instruction
	{
		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			ClassLoaderWrapper.LoadClassCritical(Class).GetFieldWrapper(Name, ClassLoaderWrapper.GetBootstrapClassLoader().FieldTypeWrapperFromSig(Sig)).EmitSet.Emit(ilgen);
		}
	}

	[XmlType("ldc_i4")]
	public sealed class Ldc_I4 : Instruction
	{
		[XmlAttribute("value")]
		public int val;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldc_I4, val);
		}
	}

	[XmlType("ldc_i4_0")]
	public sealed class Ldc_I4_0 : Simple
	{
		public Ldc_I4_0() : base(OpCodes.Ldc_I4_0)
		{
		}
	}

	[XmlType("ldc_i4_m1")]
	public sealed class Ldc_I4_M1 : Simple
	{
		public Ldc_I4_M1() : base(OpCodes.Ldc_I4_M1)
		{
		}
	}

	[XmlType("conv_u1")]
	public sealed class Conv_U1 : Simple
	{
		public Conv_U1() : base(OpCodes.Conv_U1)
		{
		}
	}

	[XmlType("conv_u2")]
	public sealed class Conv_U2 : Simple
	{
		public Conv_U2() : base(OpCodes.Conv_U2)
		{
		}
	}

	[XmlType("conv_u4")]
	public sealed class Conv_U4 : Simple
	{
		public Conv_U4() : base(OpCodes.Conv_U4)
		{
		}
	}

	[XmlType("conv_u8")]
	public sealed class Conv_U8 : Simple
	{
		public Conv_U8() : base(OpCodes.Conv_U8)
		{
		}
	}

	[XmlType("ldlen")]
	public sealed class Ldlen : Simple
	{
		public Ldlen() : base(OpCodes.Ldlen)
		{
		}
	}

	public class InstructionList : CodeEmitter
	{
		[XmlElement(typeof(Ldstr))]
		[XmlElement(typeof(Call))]
		[XmlElement(typeof(Callvirt))]
		[XmlElement(typeof(Dup))]
		[XmlElement(typeof(Pop))]
		[XmlElement(typeof(IsInst))]
		[XmlElement(typeof(Castclass))]
		[XmlElement(typeof(Unbox))]
		[XmlElement(typeof(Box))]
		[XmlElement(typeof(BrFalse))]
		[XmlElement(typeof(BrTrue))]
		[XmlElement(typeof(Br))]
		[XmlElement(typeof(Bge_Un))]
		[XmlElement(typeof(Ble_Un))]
		[XmlElement(typeof(BrLabel))]
		[XmlElement(typeof(NewObj))]
		[XmlElement(typeof(StLoc))]
		[XmlElement(typeof(LdLoc))]
		[XmlElement(typeof(LdArg_0))]
		[XmlElement(typeof(LdArg_1))]
		[XmlElement(typeof(LdArg_2))]
		[XmlElement(typeof(LdArg_3))]
		[XmlElement(typeof(Ldind_i1))]
		[XmlElement(typeof(Ldind_i2))]
		[XmlElement(typeof(Ldind_i4))]
		[XmlElement(typeof(Ldind_i8))]
		[XmlElement(typeof(Ldind_r4))]
		[XmlElement(typeof(Ldind_r8))]
		[XmlElement(typeof(Ret))]
		[XmlElement(typeof(Throw))]
		[XmlElement(typeof(Ldnull))]
		[XmlElement(typeof(Stsfld))]
		[XmlElement(typeof(Ldc_I4))]
		[XmlElement(typeof(Ldc_I4_0))]
		[XmlElement(typeof(Ldc_I4_M1))]
		[XmlElement(typeof(Conv_U1))]
		[XmlElement(typeof(Conv_U2))]
		[XmlElement(typeof(Conv_U4))]
		[XmlElement(typeof(Conv_U8))]
		[XmlElement(typeof(Ldlen))]
		public Instruction[] invoke;

		internal sealed override void Emit(ILGenerator ilgen)
		{
			Hashtable context = new Hashtable();
			for(int i = 0; i < invoke.Length; i++)
			{
				invoke[i].Generate(context, ilgen);
			}
		}
	}

	public class Throws
	{
		[XmlAttribute("class")]
		public string Class;
	}

	public class Constructor
	{
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		public InstructionList body;
		public InstructionList alternateBody;
		public Redirect redirect;
		[XmlElement("throws", typeof(Throws))]
		public Throws[] throws;
	}

	public class Redirect
	{
		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("type")]
		public string Type;
	}

	public class Override
	{
		[XmlAttribute("name")]
		public string Name;
	}

	public class Method
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		[XmlAttribute("type")]
		public string Type;
		public InstructionList body;
		public InstructionList alternateBody;
		public Redirect redirect;
		public Override @override;
		[XmlElement("throws", typeof(Throws))]
		public Throws[] throws;
	}

	public class Field
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		[XmlAttribute("constant")]
		public string Constant;
		public Redirect redirect;
	}

	public class Interface
	{
		[XmlAttribute("class")]
		public string Name;
	}

	[Flags]
	public enum MapModifiers
	{
		[XmlEnum("public")]
		Public = Modifiers.Public,
		[XmlEnum("protected")]
		Protected = Modifiers.Protected,
		[XmlEnum("private")]
		Private = Modifiers.Private,
		[XmlEnum("final")]
		Final = Modifiers.Final,
		[XmlEnum("interface")]
		Interface = Modifiers.Interface,
		[XmlEnum("static")]
		Static = Modifiers.Static,
		[XmlEnum("abstract")]
		Abstract = Modifiers.Abstract
	}

	public enum Scope
	{
		[XmlEnum("public")]
		Public = 0,
		[XmlEnum("private")]
		Private = 1
	}

	[XmlType("class")]
	public class Class
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("shadows")]
		public string Shadows;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		[XmlAttribute("scope")]
		public Scope scope;
		[XmlElement("constructor")]
		public Constructor[] Constructors;
		[XmlElement("method")]
		public Method[] Methods;
		[XmlElement("field")]
		public Field[] Fields;
		[XmlElement("implements")]
		public Interface[] Interfaces;
		[XmlElement("box")]
		public InstructionList Box;
		[XmlElement("clinit")]
		public Method Clinit;
	}

	[XmlType("exception")]
	public class ExceptionMapping
	{
		[XmlAttribute]
		public string src;
		[XmlAttribute]
		public string dst;
		public InstructionList code;
	}

	[XmlRoot("root")]
	public class Root
	{
		public Class[] assembly;
		public ExceptionMapping[] exceptionMappings;
	}
}
