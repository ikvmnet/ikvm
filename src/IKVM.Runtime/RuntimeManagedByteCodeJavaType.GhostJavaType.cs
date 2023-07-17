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
using System;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeManagedByteCodeJavaType
    {

        sealed class GhostJavaType : RuntimeManagedByteCodeJavaType
        {

            volatile FieldInfo ghostRefField;
            volatile Type typeAsBaseType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="type"></param>
            internal GhostJavaType(string name, Type type) :
                base(name, type)
            {

            }

            internal override Type TypeAsBaseType
            {
                get
                {
                    if (typeAsBaseType == null)
                        typeAsBaseType = type.GetNestedType("__Interface");

                    return typeAsBaseType;
                }
            }

            internal override FieldInfo GhostRefField
            {
                get
                {
                    if (ghostRefField == null)
                        ghostRefField = type.GetField("__<ref>");

                    return ghostRefField;
                }
            }

            internal override bool IsGhost => true;

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object GhostWrap(object obj)
            {
                return type.GetMethod("Cast").Invoke(null, new object[] { obj });
            }

            internal override object GhostUnwrap(object obj)
            {
                return type.GetMethod("ToObject").Invoke(obj, new object[0]);
            }

#endif

        }

    }

}
