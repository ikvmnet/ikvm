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

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

#if IMPORTER || EXPORTER
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed partial class AttributeAnnotationJavaType
        {

            sealed class MultipleAnnotationJavaType : AttributeAnnotationJavaTypeBase
            {

                readonly ITypeSymbol annotationType;
                readonly AttributeAnnotationJavaType declaringType;

                internal MultipleAnnotationJavaType(RuntimeContext context, AttributeAnnotationJavaType declaringType) :
                    base(context, declaringType.Name + AttributeAnnotationMultipleSuffix)
                {
#if IMPORTER || EXPORTER
                    this.annotationType = context.FakeTypes.GetAttributeMultipleType(declaringType.attributeType);
#elif !FIRST_PASS
                    this.annotationType = context.Resolver.GetSymbol(typeof(ikvm.@internal.AttributeAnnotationMultiple<>)).MakeGenericType(declaringType.attributeType);
#endif
                    this.declaringType = declaringType;
                }

                protected override void LazyPublishMembers()
                {
                    var tw = declaringType.MakeArrayType(1);
                    SetMethods([new DynamicOnlyJavaMethod(this, "value", "()" + tw.SigName, tw, [], MemberFlags.None)]);
                    SetFields([]);
                }

                internal override RuntimeJavaType DeclaringTypeWrapper => declaringType;

                internal override ITypeSymbol TypeAsTBD => annotationType;

#if !IMPORTER && !EXPORTER

                internal override object[] GetDeclaredAnnotations()
                {
                    return declaringType.GetDeclaredAnnotations();
                }

#endif

                sealed class MultipleAnnotation : Annotation
                {

                    readonly AttributeAnnotationJavaType type;

                    /// <summary>
                    /// Initializes a new instance.
                    /// </summary>
                    /// <param name="type"></param>
                    internal MultipleAnnotation(AttributeAnnotationJavaType type)
                    {
                        this.type = type;
                    }

                    static object[] UnwrapArray(object annotation)
                    {
                        // TODO make sure the descriptor is correct
                        object[] arr = (object[])annotation;
                        for (int i = 2; i < arr.Length; i += 2)
                        {
                            if ("value".Equals(arr[i]))
                            {
                                object[] value = (object[])arr[i + 1];
                                object[] rc = new object[value.Length - 1];
                                Array.Copy(value, 1, rc, 0, rc.Length);
                                return rc;
                            }
                        }

                        return [];
                    }

                    internal override void Apply(RuntimeClassLoader loader, IMethodBaseSymbolBuilder mb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, mb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, IAssemblySymbolBuilder ab, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, ab, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, IFieldSymbolBuilder fb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, fb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, IParameterSymbolBuilder pb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, pb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, ITypeSymbolBuilder tb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, tb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, IPropertySymbolBuilder pb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, pb, ann);
                        }
                    }

                    internal override bool IsCustomAttribute
                    {
                        get { return type.Annotation.IsCustomAttribute; }
                    }

                }

                internal override Annotation Annotation => new MultipleAnnotation(declaringType);

                internal override AttributeTargets AttributeTargets => declaringType.AttributeTargets;

            }

        }

    }

}
