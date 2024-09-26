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
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

using IKVM.CoreLib.Symbols;
using IKVM.Runtime;

namespace IKVM.Tools.Importer.MapXml
{

    [Instruction("call")]
    public class Call : Instruction
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Call"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static new Call Read(XElement element)
        {
            var inst = new Call();
            Load(inst, element);
            return inst;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="element"></param>
        public static void Load(Call inst, XElement element)
        {
            inst.Class = (string)element.Attribute("class");
            inst.Type = (string)element.Attribute("type");
            inst.Name = (string)element.Attribute("name");
            inst.Sig = (string)element.Attribute("sig");
        }

        readonly OpCode opcode;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Call() : this(OpCodes.Call)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        protected Call(OpCode opcode)
        {
            this.opcode = opcode;
        }

        public string Class { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Sig { get; set; }

        internal sealed override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (Name == null)
                throw new MapXmlException("Missing name.");

            if (Name == ".ctor")
            {
                Debug.Assert(Class == null && Type != null);

                var argTypes = context.ClassLoader.ArgTypeListFromSig(Sig);
                var ci = context.ClassLoader.Context.Resolver.ResolveType(Type).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, argTypes);
                if (ci == null)
                    throw new InvalidOperationException("Missing .ctor: " + Type + "..ctor" + Sig);

                ilgen.Emit(opcode, ci);
            }
            else
            {
                Debug.Assert(Class == null ^ Type == null);
                if (Class != null)
                {
                    Debug.Assert(Sig != null);
                    var method = context.ClassLoader.LoadClassByName(Class).GetMethodWrapper(Name, Sig, false);
                    if (method == null)
                        throw new InvalidOperationException("method not found: " + Class + "." + Name + Sig);

                    method.Link();
                    // TODO this code is part of what Compiler.CastInterfaceArgs (in compiler.cs) does,
                    // it would be nice if we could avoid this duplication...
                    var argTypeWrappers = method.GetParameters();
                    for (int i = 0; i < argTypeWrappers.Length; i++)
                    {
                        if (argTypeWrappers[i].IsGhost)
                        {
                            var temps = new CodeEmitterLocal[argTypeWrappers.Length + (method.IsStatic ? 0 : 1)];
                            for (int j = temps.Length - 1; j >= 0; j--)
                            {
                                RuntimeJavaType tw;
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
                        ilgen.Emit(opcode, (IMethodSymbol)method.GetMethod());
                    }
                }
                else
                {
                    ITypeSymbol[] argTypes;
                    if (Sig.StartsWith("("))
                    {
                        argTypes = context.ClassLoader.ArgTypeListFromSig(Sig);
                    }
                    else if (Sig == "")
                    {
                        argTypes = [];
                    }
                    else
                    {
                        var types = Sig.Split(';');
                        argTypes = new ITypeSymbol[types.Length];
                        for (int i = 0; i < types.Length; i++)
                        {
                            argTypes[i] = context.ClassLoader.Context.Resolver.ResolveType(types[i]);
                        }
                    }

                    var ti = context.ClassLoader.Context.Resolver.ResolveType(Type);
                    if (ti == null)
                        throw new InvalidOperationException("Missing type: " + Type);

                    var mi = ti.GetMethod(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, argTypes);
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
