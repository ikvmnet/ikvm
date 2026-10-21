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

using IKVM.Attributes;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        private sealed partial class JavaTypeImpl
        {

            sealed class DelegateConstructorMethodWrapper : RuntimeJavaMethod
            {

                MethodBuilder constructor;
                MethodInfo invoke;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="m"></param>
                internal DelegateConstructorMethodWrapper(RuntimeByteCodeJavaType tw, Method m) :
                    base(tw, m.Name, m.Signature, null, null, null, (Modifiers)m.AccessFlags, MemberFlags.None)
                {

                }

                internal void DoLink(TypeBuilder typeBuilder)
                {
                    var attribs = MethodAttributes.HideBySig | MethodAttributes.Public;
                    constructor = ReflectUtil.DefineConstructor(typeBuilder, attribs, new Type[] { DeclaringType.Context.Types.Object, DeclaringType.Context.Types.IntPtr });
                    constructor.SetImplementationFlags(MethodImplAttributes.Runtime);
                    var mw = GetParameters()[0].GetMethods()[0];
                    mw.Link();
                    invoke = (MethodInfo)mw.GetMethod();
                }

                internal override void EmitNewobj(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.Emit(OpCodes.Ldvirtftn, invoke);
                    ilgen.Emit(OpCodes.Newobj, constructor);
                }

            }

        }

    }

}