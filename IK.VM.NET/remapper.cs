using System;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

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
		[XmlAttribute("name")]
		public string Name;
		[XmlAttribute("sig")]
		public string Sig;

		private MethodBase method;
		private OpCode opcode;

		internal sealed override void Generate(Hashtable context, ILGenerator ilgen)
		{
			if(method == null)
			{
				string name = Name;
				if(name == ".ctor")
				{
					string sig = Sig;
					Type[] argTypes = ClassLoaderWrapper.GetBootstrapClassLoader().ArgTypeListFromSig(sig);
					method = Type.GetType(Class, true).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, argTypes, null);
				}
				else
				{
					method = Type.GetType(Class, true).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				}
				if(method == null)
				{
					throw new InvalidOperationException("method not found: " + name);
				}
			}
			if(method.IsConstructor)
			{
				ilgen.Emit(opcode, (ConstructorInfo)method);
			}
			else
			{
				ilgen.Emit(opcode, (MethodInfo)method);
			}
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

	[XmlType("isinst")]
	public sealed class IsInst : Instruction
	{
		[XmlAttribute("class")]
		public string Class;

		private Type type;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			if(type == null)
			{
				type = Type.GetType(Class, true);
			}
			ilgen.Emit(OpCodes.Isinst, type);
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
	
		private Type type;

		internal override void Generate(Hashtable context, ILGenerator ilgen)
		{
			LocalBuilder lb = (LocalBuilder)context[Name];
			if(lb == null)
			{
				if(type == null)
				{
					type = Type.GetType(Class, true);
				}
				lb = ilgen.DeclareLocal(type);
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

	[XmlType("ret")]
	public sealed class Ret : Simple
	{
		public Ret() : base(OpCodes.Ret)
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
		[XmlElement(typeof(BrFalse))]
		[XmlElement(typeof(Br))]
		[XmlElement(typeof(BrLabel))]
		[XmlElement(typeof(NewObj))]
		[XmlElement(typeof(StLoc))]
		[XmlElement(typeof(LdLoc))]
		[XmlElement(typeof(LdArg_0))]
		[XmlElement(typeof(Ret))]
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
