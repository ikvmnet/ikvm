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

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        /// <summary>
        /// We allow open generic types to be visible to Java code as very limited classes (or interfaces).
        /// They are always package private and have the abstract and final modifiers set, this makes them
        /// inaccessible and invalid from a Java point of view. The intent is to avoid any usage of these
        /// classes. They exist solely for the purpose of stack walking, because the .NET runtime will report
        /// open generic types when walking the stack (as a performance optimization). We cannot (reliably) map
        /// these classes to their instantiations, so we report the open generic type class instead.
        /// Note also that these classes can only be used as a "handle" to the type, they expose no members,
        /// don't implement any interfaces and the base class is always object.
        /// </summary>
        sealed class OpenGenericJavaType : RuntimeJavaType
        {

            readonly Type type;

            static Modifiers GetModifiers(Type type)
            {
                var modifiers = Modifiers.Abstract | Modifiers.Final;
                if (type.IsInterface)
                    modifiers |= Modifiers.Interface;

                return modifiers;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="type"></param>
            /// <param name="name"></param>
            internal OpenGenericJavaType(RuntimeContext context, Type type, string name) :
                base(context, TypeFlags.None, GetModifiers(type), name)
            {
                this.type = type;
            }

            internal override RuntimeJavaType BaseTypeWrapper => type.IsInterface ? null : Context.JavaBase.TypeOfJavaLangObject;

            internal override Type TypeAsTBD => type;

            internal override RuntimeClassLoader ClassLoader => Context.AssemblyClassLoaderFactory.FromAssembly(type.Assembly);

            protected override void LazyPublishMembers()
            {
                SetFields([]);
                SetMethods([]);
            }

        }

    }

}
