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
using System.Xml.Linq;

using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer.MapXml
{

    [Instruction("ldtoken")]
    public sealed class Ldtoken : Instruction
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Ldtoken"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static new Ldtoken Read(XElement element)
        {
            var inst = new Ldtoken();
            Load(inst, element);
            return inst;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="element"></param>
        public static void Load(Ldtoken inst, XElement element)
        {
            Load((Instruction)inst, element);
            inst.Type = (string)element.Attribute("type");
            inst.Class = (string)element.Attribute("class");
            inst.Method = (string)element.Attribute("method");
            inst.Field = (string)element.Attribute("field");
            inst.Sig = (string)element.Attribute("sig");
        }

        public string Type { get; set; }

        public string Class { get; set; }

        public string Method { get; set; }

        public string Field { get; set; }

        public string Sig { get; set; }

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (!Validate())
            {
                return;
            }

            MemberInfo member = Resolve(context);
            Type type = member as Type;
            MethodInfo method = member as MethodInfo;
            ConstructorInfo constructor = member as ConstructorInfo;
            FieldInfo field = member as FieldInfo;

            if (type != null)
            {
                ilgen.Emit(OpCodes.Ldtoken, type);
            }
            else if (method != null)
            {
                ilgen.Emit(OpCodes.Ldtoken, method);
            }
            else if (constructor != null)
            {
                ilgen.Emit(OpCodes.Ldtoken, constructor);
            }
            else if (field != null)
            {
                ilgen.Emit(OpCodes.Ldtoken, field);
            }
            else
            {
                StaticCompiler.IssueMessage(Message.MapXmlUnableToResolveOpCode, ToString());
            }
        }

        private bool Validate()
        {
            if (Type != null && Class == null)
            {
                if (Method != null || Field != null || Sig != null)
                {
                    StaticCompiler.IssueMessage(Message.MapXmlError, "not implemented: cannot use 'type' attribute with 'method' or 'field' attribute for ldtoken");
                    return false;
                }
                return true;
            }
            else if (Class != null && Type == null)
            {
                if (Method == null && Field == null)
                {
                    if (Sig != null)
                    {
                        StaticCompiler.IssueMessage(Message.MapXmlError, "cannot specify 'sig' attribute without either 'method' or 'field' attribute for ldtoken");
                    }
                    return true;
                }
                if (Method != null && Field != null)
                {
                    StaticCompiler.IssueMessage(Message.MapXmlError, "cannot specify both 'method' and 'field' attribute for ldtoken");
                    return false;
                }
                return true;
            }
            else
            {
                StaticCompiler.IssueMessage(Message.MapXmlError, "must specify either 'type' or 'class' attribute for ldtoken");
                return false;
            }
        }

        private MemberInfo Resolve(CodeGenContext context)
        {
            if (Type != null)
            {
                if (Class != null || Method != null || Field != null || Sig != null)
                {
                    throw new NotImplementedException();
                }
                return StaticCompiler.GetTypeForMapXml(context.ClassLoader, Type);
            }
            else if (Class != null)
            {
                var tw = context.ClassLoader.TryLoadClassByName(Class);
                if (tw == null)
                {
                    return null;
                }
                else if (Method != null)
                {
                    var mw = tw.GetMethodWrapper(Method, Sig, false);
                    if (mw == null)
                    {
                        return null;
                    }
                    return mw.GetMethod();
                }
                else if (Field != null)
                {
                    var fw = tw.GetFieldWrapper(Field, Sig);
                    if (fw == null)
                    {
                        return null;
                    }
                    return fw.GetField();
                }
                else
                {
                    return tw.TypeAsBaseType;
                }
            }
            else
            {
                return null;
            }
        }

    }

}
