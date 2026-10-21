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
using System.Collections.Generic;

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed partial class EnumEnumJavaType : FakeJavaType
        {

            readonly Type fakeType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="enumType"></param>
            internal EnumEnumJavaType(RuntimeContext context, string name, Type enumType) :
                base(context, Modifiers.Public | Modifiers.Enum | Modifiers.Final, name, context.ClassLoaderFactory.LoadClassCritical("java.lang.Enum"))
            {
#if IMPORTER || EXPORTER
                this.fakeType = context.FakeTypes.GetEnumType(enumType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.EnumEnum<>).MakeGenericType(enumType);
#endif
            }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal object GetUnspecifiedValue()
            {
                return GetFieldWrapper("__unspecified", this.SignatureName).GetValue(null);
            }

#endif

            sealed class EnumJavaField : RuntimeJavaField
            {

#if !IMPORTER && !EXPORTER

                readonly int ordinal;
                object val;

#endif

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="ordinal"></param>
                internal EnumJavaField(RuntimeJavaType tw, string name, int ordinal) :
                    base(tw, tw, name, tw.SignatureName, Modifiers.Public | Modifiers.Static | Modifiers.Final | Modifiers.Enum, null, MemberFlags.None)
                {
#if !IMPORTER && !EXPORTER
                    this.ordinal = ordinal;
#endif
                }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

                internal override object GetValue(object obj)
                {
                    if (val == null)
                        System.Threading.Interlocked.CompareExchange(ref val, Activator.CreateInstance(this.DeclaringType.TypeAsTBD, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { this.Name, ordinal }, null), null);

                    return val;
                }

                internal override void SetValue(object obj, object value)
                {

                }

#endif

#if EMITTERS
                protected override void EmitGetImpl(CodeEmitter ilgen)
                {
                    var typeofByteCodeHelper = DeclaringType.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.ByteCodeHelper").AsReflection();
                    ilgen.Emit(OpCodes.Ldstr, Name);
                    ilgen.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("GetDotNetEnumField").MakeGenericMethod(DeclaringType.TypeAsBaseType));
                }

                protected override void EmitSetImpl(CodeEmitter ilgen)
                {
                    throw new NotImplementedException();
                }

#endif

            }

            sealed class EnumValuesJavaMethod : RuntimeJavaMethod
            {

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal EnumValuesJavaMethod(RuntimeJavaType declaringType) :
                    base(declaringType, "values", "()[" + declaringType.SignatureName, null, declaringType.MakeArrayType(1), Array.Empty<RuntimeJavaType>(), Modifiers.Public | Modifiers.Static, MemberFlags.None)
                {

                }

                internal override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

                internal override object Invoke(object obj, object[] args)
                {
                    RuntimeJavaField[] values = this.DeclaringType.GetFields();
                    object[] array = (object[])Array.CreateInstance(this.DeclaringType.TypeAsArrayType, values.Length);
                    for (int i = 0; i < values.Length; i++)
                    {
                        array[i] = values[i].GetValue(null);
                    }
                    return array;
                }

#endif

            }

            protected override void LazyPublishMembers()
            {
                var fields = new List<RuntimeJavaField>();
                int ordinal = 0;
                foreach (var field in DeclaringTypeWrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.Public))
                    if (field.IsLiteral)
                        fields.Add(new EnumJavaField(this, field.Name, ordinal++));

                // TODO if the enum already has an __unspecified value, rename this one
                fields.Add(new EnumJavaField(this, "__unspecified", ordinal++));
                SetFields(fields.ToArray());
                SetMethods(new RuntimeJavaMethod[] { new EnumValuesJavaMethod(this), new EnumValueOfJavaMethod(this) });
                base.LazyPublishMembers();
            }

            internal override RuntimeJavaType DeclaringTypeWrapper => Context.ClassLoaderFactory.GetJavaTypeFromType(fakeType.GetGenericArguments()[0]);

            internal override RuntimeClassLoader ClassLoader => DeclaringTypeWrapper.ClassLoader;

            internal override Type TypeAsTBD => fakeType;

            internal override bool IsFastClassLiteralSafe => true;

        }

    }

}
