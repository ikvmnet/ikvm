using System;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using OpenSystem.Java;

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
					emitter = CodeEmitter.Create(opcode, ClassLoaderWrapper.GetType(type).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, argTypes, null));
				}
				else
				{
					Debug.Assert(Class == null ^ type == null);
					if(Class != null)
					{
						Debug.Assert(Sig != null);
						MethodWrapper method = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Class).GetMethodWrapper(new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), Name, Sig), false);
						if(method != null)
						{
							emitter = CodeEmitter.Create(opcode, method.GetMethod());
						}
					}
					else
					{
						if(Sig != null)
						{
							Type[] argTypes = ClassLoaderWrapper.GetBootstrapClassLoader().ArgTypeListFromSig(Sig);
							emitter = CodeEmitter.Create(opcode, ClassLoaderWrapper.GetType(type).GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, argTypes, null));
						}
						else
						{
							emitter = CodeEmitter.Create(opcode, ClassLoaderWrapper.GetType(type).GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
						}
					}
				}
				if(emitter == null)
				{
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

	public abstract class TypeInstruction : Instruction
	{
		[XmlAttribute("class")]
		public string Class;
		[XmlAttribute("type")]
		public string type;

		private OpCode opcode;
		private TypeWrapper typeWrapper;
		private Type typeType;

		internal TypeInstruction(OpCode opcode)
		{
			this.opcode = opcode;
		}

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
					typeType = ClassLoaderWrapper.GetType(type);
				}
			}
			ilgen.Emit(OpCodes.Isinst, typeType != null ? typeType : typeWrapper.Type);
		}
	}

	[XmlType("isinst")]
	public sealed class IsInst : TypeInstruction
	{
		public IsInst() : base(OpCodes.Isinst)
		{
		}
	}

	[XmlType("castclass")]
	public sealed class Castclass : TypeInstruction
	{
		public Castclass() : base(OpCodes.Castclass)
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
						typeType = ClassLoaderWrapper.GetType(type);
					}
					else
					{
						typeWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Class);
					}
				}
				lb = ilgen.DeclareLocal(typeType != null ? typeType : typeWrapper.Type);
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

	public class InstructionList : CodeEmitter
	{
		[XmlElement(typeof(Ldstr))]
		[XmlElement(typeof(Call))]
		[XmlElement(typeof(Callvirt))]
		[XmlElement(typeof(Dup))]
		[XmlElement(typeof(Pop))]
		[XmlElement(typeof(IsInst))]
		[XmlElement(typeof(Castclass))]
		[XmlElement(typeof(BrFalse))]
		[XmlElement(typeof(BrTrue))]
		[XmlElement(typeof(Br))]
		[XmlElement(typeof(BrLabel))]
		[XmlElement(typeof(NewObj))]
		[XmlElement(typeof(StLoc))]
		[XmlElement(typeof(LdLoc))]
		[XmlElement(typeof(LdArg_0))]
		[XmlElement(typeof(LdArg_1))]
		[XmlElement(typeof(LdArg_2))]
		[XmlElement(typeof(LdArg_3))]
		[XmlElement(typeof(Ret))]
		[XmlElement(typeof(Throw))]
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

	public class Constructor
	{
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		public InstructionList invokespecial;
		public InstructionList newobj;
		public Redirect redirect;
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

	public class Method : InstructionList
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		[XmlAttribute("type")]
		public string Type;
		public InstructionList invokevirtual;
		public InstructionList invokespecial;
		public InstructionList invokestatic;
		public Redirect redirect;
		public Override @override;
	}

	public class Field
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
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
		[XmlEnum("final")]
		Final = Modifiers.Final,
		[XmlEnum("interface")]
		Interface = Modifiers.Interface,
		[XmlEnum("static")]
		Static = Modifiers.Static
	}

	[XmlType("class")]
	public class Class
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("type")]
		public string Type;
		[XmlAttribute("modifiers")]
		public MapModifiers Modifiers;
		[XmlElement("constructor")]
		public Constructor[] Constructors;
		[XmlElement("method")]
		public Method[] Methods;
		[XmlElement("field")]
		public Field[] Fields;
		[XmlElement("implements")]
		public Interface[] Interfaces;
	}

	[XmlRoot("root")]
	public class Root
	{
		public Class[] remappings;
		public Class[] nativeMethods;
	}
}
