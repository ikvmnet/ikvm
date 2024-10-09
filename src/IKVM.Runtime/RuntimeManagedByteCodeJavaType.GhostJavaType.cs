/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class RuntimeManagedByteCodeJavaType
    {

        public sealed class GhostJavaType : RuntimeManagedByteCodeJavaType
        {

            volatile IFieldSymbol ghostRefField;
            volatile ITypeSymbol typeAsBaseType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="type"></param>
            internal GhostJavaType(RuntimeContext context, string name, ITypeSymbol type) :
                base(context, name, type)
            {

            }

            internal override ITypeSymbol TypeAsBaseType => typeAsBaseType ??= type.GetNestedType("__Interface");

            internal override IFieldSymbol GhostRefField => ghostRefField ??= type.GetField("__<ref>");

            internal override bool IsGhost => true;

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object GhostWrap(object obj)
            {
                return type.GetMethod("Cast").AsReflection().Invoke(null, [obj]);
            }

            internal override object GhostUnwrap(object obj)
            {
                return type.GetMethod("ToObject").AsReflection().Invoke(obj, []);
            }

#endif

        }

    }

}
