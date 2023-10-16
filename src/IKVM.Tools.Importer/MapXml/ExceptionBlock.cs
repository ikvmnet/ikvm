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

using System.Linq;
using System.Xml.Linq;

using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer.MapXml
{

    [Instruction("exceptionBlock")]
    public sealed class ExceptionBlock : Instruction
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="ExceptionBlock"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static new ExceptionBlock Read(XElement element)
        {
            var inst = new ExceptionBlock();
            Load(inst, element);
            return inst;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="element"></param>
        public static void Load(ExceptionBlock inst, XElement element)
        {
            Load((Instruction)inst, element);
            inst.Try = element.Elements(MapXmlSerializer.NS + "try").Select(InstructionList.Read).FirstOrDefault();
            inst.Catch = element.Elements(MapXmlSerializer.NS + "catch").Select(CatchBlock.Read).FirstOrDefault();
            inst.Finally = element.Elements(MapXmlSerializer.NS + "finally").Select(InstructionList.Read).FirstOrDefault();
        }

        public InstructionList Try { get; set; }

        public CatchBlock Catch { get; set; }

        public InstructionList Finally { get; set; }

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            ilgen.BeginExceptionBlock();

            Try.Generate(context, ilgen);

            if (Catch != null)
            {
                Type type;
                if (Catch.Type != null)
                {
                    type = context.ClassLoader.Context.StaticCompiler.GetTypeForMapXml(context.ClassLoader, Catch.Type);
                }
                else
                {
                    type = context.ClassLoader.LoadClassByName(Catch.Class).TypeAsExceptionType;
                }

                ilgen.BeginCatchBlock(type);
                Catch.Generate(context, ilgen);
            }

            if (Finally != null)
            {
                ilgen.BeginFinallyBlock();
                Finally.Generate(context, ilgen);
            }

            ilgen.EndExceptionBlock();
        }

    }

}
