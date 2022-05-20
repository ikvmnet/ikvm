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
using System.Xml.Serialization;

using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.Internal.MapXml
{
    [XmlType("ldtoken")]
    public sealed class Ldtoken : Instruction
    {
        [XmlAttribute("type")]
        public string type;
        [XmlAttribute("class")]
        public string Class;
        [XmlAttribute("method")]
        public string Method;
        [XmlAttribute("field")]
        public string Field;
        [XmlAttribute("sig")]
        public string Sig;

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
            if (type != null && Class == null)
            {
                if (Method != null || Field != null || Sig != null)
                {
                    StaticCompiler.IssueMessage(Message.MapXmlError, "not implemented: cannot use 'type' attribute with 'method' or 'field' attribute for ldtoken");
                    return false;
                }
                return true;
            }
            else if (Class != null && type == null)
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
            if (type != null)
            {
                if (Class != null || Method != null || Field != null || Sig != null)
                {
                    throw new NotImplementedException();
                }
                return StaticCompiler.GetTypeForMapXml(context.ClassLoader, type);
            }
            else if (Class != null)
            {
                TypeWrapper tw = context.ClassLoader.LoadClassByDottedNameFast(Class);
                if (tw == null)
                {
                    return null;
                }
                else if (Method != null)
                {
                    MethodWrapper mw = tw.GetMethodWrapper(Method, Sig, false);
                    if (mw == null)
                    {
                        return null;
                    }
                    return mw.GetMethod();
                }
                else if (Field != null)
                {
                    FieldWrapper fw = tw.GetFieldWrapper(Field, Sig);
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
