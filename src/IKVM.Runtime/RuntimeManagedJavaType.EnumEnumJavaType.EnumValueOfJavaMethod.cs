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
using IKVM.Attributes;

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed partial class EnumEnumJavaType
        {

            sealed class EnumValueOfJavaMethod : RuntimeJavaMethod
            {

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal EnumValueOfJavaMethod(RuntimeJavaType declaringType) :
                    base(declaringType, "valueOf", "(Ljava.lang.String;)" + declaringType.SignatureName, null, declaringType, [declaringType.Context.JavaBase.TypeOfJavaLangString], Modifiers.Public | Modifiers.Static, MemberFlags.None)
                {

                }

                internal override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

                internal override object Invoke(object obj, object[] args)
                {
                    var values = DeclaringType.GetFields();
                    for (int i = 0; i < values.Length; i++)
                        if (values[i].Name.Equals(args[0]))
                            return values[i].GetValue(null);

                    throw new java.lang.IllegalArgumentException("" + args[0]);
                }

#endif

            }

        }

    }

}
