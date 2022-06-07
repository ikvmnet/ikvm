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

using System.Xml.Serialization;

using Type = IKVM.Reflection.Type;

namespace IKVM.Internal.MapXml
{
    [XmlType("exceptionBlock")]
    public sealed class ExceptionBlock : Instruction
    {
        public InstructionList @try;
        public CatchBlock @catch;
        public InstructionList @finally;

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            ilgen.BeginExceptionBlock();
            @try.Generate(context, ilgen);
            if (@catch != null)
            {
                Type type;
                if (@catch.type != null)
                {
                    type = StaticCompiler.GetTypeForMapXml(context.ClassLoader, @catch.type);
                }
                else
                {
                    type = context.ClassLoader.LoadClassByDottedName(@catch.Class).TypeAsExceptionType;
                }
                ilgen.BeginCatchBlock(type);
                @catch.Generate(context, ilgen);
            }
            if (@finally != null)
            {
                ilgen.BeginFinallyBlock();
                @finally.Generate(context, ilgen);
            }
            ilgen.EndExceptionBlock();
        }
    }
}
