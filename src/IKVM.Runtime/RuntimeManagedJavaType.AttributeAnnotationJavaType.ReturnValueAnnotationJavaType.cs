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

        sealed partial class AttributeAnnotationJavaType
        {

            sealed class ReturnValueAnnotationJavaType : AttributeAnnotationJavaTypeBase
            {

                readonly Type fakeType;
                readonly AttributeAnnotationJavaType declaringType;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal ReturnValueAnnotationJavaType(RuntimeContext context, AttributeAnnotationJavaType declaringType) :
                    base(context, declaringType.Name + AttributeAnnotationReturnValueSuffix)
                {
#if IMPORTER || EXPORTER
                    this.fakeType = context.FakeTypes.GetAttributeReturnValueType(declaringType.attributeType);
#elif !FIRST_PASS
                    this.fakeType = typeof(ikvm.@internal.AttributeAnnotationReturnValue<>).MakeGenericType(declaringType.attributeType);
#endif
                    this.declaringType = declaringType;
                }

                protected override void LazyPublishMembers()
                {
                    var tw = (RuntimeJavaType)declaringType;
                    if (declaringType.GetAttributeUsage().AllowMultiple)
                        tw = tw.MakeArrayType(1);

                    SetMethods(new RuntimeJavaMethod[] { new DynamicOnlyJavaMethod(this, "value", "()" + tw.SignatureName, tw, Array.Empty<RuntimeJavaType>(), MemberFlags.None) });
                    SetFields(Array.Empty<RuntimeJavaField>());
                }

                internal override RuntimeJavaType DeclaringTypeWrapper => declaringType;

                internal override Type TypeAsTBD => fakeType;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

                internal override object[] GetDeclaredAnnotations()
                {
                    java.util.HashMap targetMap = new java.util.HashMap();
                    targetMap.put("value", new java.lang.annotation.ElementType[] { java.lang.annotation.ElementType.METHOD });
                    java.util.HashMap retentionMap = new java.util.HashMap();
                    retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
                    return new object[] {
                        java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
                        java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
                    };
                }

#endif

                sealed class ReturnValueAnnotation : Annotation
                {

                    readonly AttributeAnnotationJavaType type;

                    /// <summary>
                    /// Initializes a new instance.
                    /// </summary>
                    /// <param name="type"></param>
                    internal ReturnValueAnnotation(AttributeAnnotationJavaType type)
                    {
                        this.type = type;
                    }

                    internal override void ApplyReturnValue(RuntimeClassLoader loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
                    {
                        // TODO make sure the descriptor is correct
                        var ann = type.Annotation;
                        var arr = (object[])annotation;
                        for (int i = 2; i < arr.Length; i += 2)
                        {
                            if ("value".Equals(arr[i]))
                            {
                                pb ??= mb.DefineParameter(0, ParameterAttributes.None, null);

                                var value = (object[])arr[i + 1];
                                if (value[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
                                    ann.Apply(loader, pb, value);
                                else
                                    for (int j = 1; j < value.Length; j++)
                                        ann.Apply(loader, pb, value[j]);

                                break;
                            }
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, MethodBuilder mb, object annotation)
                    {

                    }

                    internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
                    {

                    }

                    internal override void Apply(RuntimeClassLoader loader, FieldBuilder fb, object annotation)
                    {

                    }

                    internal override void Apply(RuntimeClassLoader loader, ParameterBuilder pb, object annotation)
                    {

                    }

                    internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
                    {

                    }

                    internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
                    {

                    }

                    internal override bool IsCustomAttribute => type.Annotation.IsCustomAttribute;

                }

                internal override Annotation Annotation => new ReturnValueAnnotation(declaringType);

                internal override AttributeTargets AttributeTargets => AttributeTargets.ReturnValue;

            }

        }

    }

}
