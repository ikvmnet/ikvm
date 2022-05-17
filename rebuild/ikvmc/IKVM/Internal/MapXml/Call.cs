/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.Internal.MapXml
{
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

        private OpCode opcode;

        internal sealed override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            Debug.Assert(Name != null);
            if (Name == ".ctor")
            {
                Debug.Assert(Class == null && type != null);
                Type[] argTypes = context.ClassLoader.ArgTypeListFromSig(Sig);
                ConstructorInfo ci = StaticCompiler.GetTypeForMapXml(context.ClassLoader, type).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, argTypes, null);
                if (ci == null)
                {
                    throw new InvalidOperationException("Missing .ctor: " + type + "..ctor" + Sig);
                }
                ilgen.Emit(opcode, ci);
            }
            else
            {
                Debug.Assert(Class == null ^ type == null);
                if (Class != null)
                {
                    Debug.Assert(Sig != null);
                    MethodWrapper method = context.ClassLoader.LoadClassByDottedName(Class).GetMethodWrapper(Name, Sig, false);
                    if (method == null)
                    {
                        throw new InvalidOperationException("method not found: " + Class + "." + Name + Sig);
                    }
                    method.Link();
                    // TODO this code is part of what Compiler.CastInterfaceArgs (in compiler.cs) does,
                    // it would be nice if we could avoid this duplication...
                    TypeWrapper[] argTypeWrappers = method.GetParameters();
                    for (int i = 0; i < argTypeWrappers.Length; i++)
                    {
                        if (argTypeWrappers[i].IsGhost)
                        {
                            CodeEmitterLocal[] temps = new CodeEmitterLocal[argTypeWrappers.Length + (method.IsStatic ? 0 : 1)];
                            for (int j = temps.Length - 1; j >= 0; j--)
                            {
                                TypeWrapper tw;
                                if (method.IsStatic)
                                {
                                    tw = argTypeWrappers[j];
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        tw = method.DeclaringType;
                                    }
                                    else
                                    {
                                        tw = argTypeWrappers[j - 1];
                                    }
                                }
                                if (tw.IsGhost)
                                {
                                    tw.EmitConvStackTypeToSignatureType(ilgen, null);
                                }
                                temps[j] = ilgen.DeclareLocal(tw.TypeAsSignatureType);
                                ilgen.Emit(OpCodes.Stloc, temps[j]);
                            }
                            for (int j = 0; j < temps.Length; j++)
                            {
                                ilgen.Emit(OpCodes.Ldloc, temps[j]);
                            }
                            break;
                        }
                    }
                    if (opcode.Value == OpCodes.Call.Value)
                    {
                        method.EmitCall(ilgen);
                    }
                    else if (opcode.Value == OpCodes.Callvirt.Value)
                    {
                        method.EmitCallvirt(ilgen);
                    }
                    else if (opcode.Value == OpCodes.Newobj.Value)
                    {
                        method.EmitNewobj(ilgen);
                    }
                    else
                    {
                        // ldftn or ldvirtftn
                        ilgen.Emit(opcode, (MethodInfo)method.GetMethod());
                    }
                }
                else
                {
                    Type[] argTypes;
                    if (Sig.StartsWith("("))
                    {
                        argTypes = context.ClassLoader.ArgTypeListFromSig(Sig);
                    }
                    else if (Sig == "")
                    {
                        argTypes = Type.EmptyTypes;
                    }
                    else
                    {
                        string[] types = Sig.Split(';');
                        argTypes = new Type[types.Length];
                        for (int i = 0; i < types.Length; i++)
                        {
                            argTypes[i] = StaticCompiler.GetTypeForMapXml(context.ClassLoader, types[i]);
                        }
                    }

                    Type ti = StaticCompiler.GetTypeForMapXml(context.ClassLoader, type);
                    if (ti == null)
                    {
                        throw new InvalidOperationException("Missing type: " + type);
                    }

                    MethodInfo mi = ti.GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, argTypes, null);
                    if (mi == null)
                    {
                        var ta = argTypes.Select(i => i.AssemblyQualifiedName).ToArray();
                        var m = ti.GetMethods().FirstOrDefault(i => i.Name == Name);
                        throw new InvalidOperationException("Missing method: " + ti.FullName + "." + Name + Sig + $" -> ({string.Join("; ", ta)}). {(m != null ? string.Join(";", m.GetParameters().Select(i => i.ParameterType.AssemblyQualifiedName)) : null)}");
                    }
                    ilgen.Emit(opcode, mi);
                }
            }
        }
    }
}
