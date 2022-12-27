﻿/*
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

using IKVM.Internal;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer.MapXml
{

    public sealed class Redirect : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Redirect"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Redirect Read(XElement element)
        {
            var redirect = new Redirect();
            Load(redirect, element);
            return redirect;
        }

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="redirect"></param>
        /// <param name="element"></param>
        public static void Load(Redirect redirect, XElement element)
        {
            Load((MapXmlElement)redirect, element);
            redirect.Class = (string)element.Attribute("class");
            redirect.Name = (string)element.Attribute("name");
            redirect.Sig = (string)element.Attribute("sig");
            redirect.Type = (string)element.Attribute("type");
        }

        public string Class { get; set; }

        public string Name { get; set; }

        public string Sig { get; set; }

        public string Type { get; set; }

        internal void Emit(ClassLoaderWrapper loader, CodeEmitter ilgen)
        {
            if (Type != "static" || Class == null || Name == null || Sig == null)
                throw new NotImplementedException();

            Type[] redirParamTypes = loader.ArgTypeListFromSig(Sig);
            for (int i = 0; i < redirParamTypes.Length; i++)
            {
                ilgen.EmitLdarg(i);
            }
            // HACK if the class name contains a comma, we assume it is a .NET type
            if (Class.IndexOf(',') >= 0)
            {
#if NETCOREAPP
				Class = Class.Replace("mscorlib", Universe.CoreLibName);
#endif
                Type type = StaticCompiler.Universe.GetType(Class, true);
                MethodInfo mi = type.GetMethod(Name, redirParamTypes);
                if (mi == null)
                {
                    throw new InvalidOperationException();
                }
                ilgen.Emit(OpCodes.Call, mi);
            }
            else
            {
                TypeWrapper tw = loader.LoadClassByDottedName(Class);
                MethodWrapper mw = tw.GetMethodWrapper(Name, Sig, false);
                if (mw == null)
                {
                    throw new InvalidOperationException();
                }
                mw.Link();
                mw.EmitCall(ilgen);
            }
            // TODO we may need a cast here (or a stack to return type conversion)
            ilgen.Emit(OpCodes.Ret);
        }

    }

}
