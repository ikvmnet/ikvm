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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
using System.Net.Mime;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

#if EXPORTER
using IKVM.Tools.Exporter;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed partial class AttributeAnnotationJavaType : AttributeAnnotationJavaTypeBase
        {

            readonly Type fakeType;
            readonly Type attributeType;
            volatile RuntimeJavaType[] innerClasses;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="attributeType"></param>
            internal AttributeAnnotationJavaType(RuntimeContext context, string name, Type attributeType) :
                base(context, name)
            {
#if IMPORTER || EXPORTER
                this.fakeType = context.FakeTypes.GetAttributeType(attributeType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.AttributeAnnotation<>).MakeGenericType(attributeType);
#endif
                this.attributeType = attributeType;
            }

            static bool IsSupportedType(RuntimeContext context, Type type)
            {
                // Java annotations only support one-dimensional arrays
                if (ReflectUtil.IsVector(type))
                    type = type.GetElementType();

                return type == context.Types.String
                    || type == context.Types.Boolean
                    || type == context.Types.Byte
                    || type == context.Types.Char
                    || type == context.Types.Int16
                    || type == context.Types.Int32
                    || type == context.Types.Single
                    || type == context.Types.Int64
                    || type == context.Types.Double
                    || type == context.Types.Type
                    || type.IsEnum;
            }

            internal static void GetConstructors(RuntimeContext context, Type type, out ConstructorInfo defCtor, out ConstructorInfo singleOneArgCtor)
            {
                defCtor = null;
                int oneArgCtorCount = 0;
                ConstructorInfo oneArgCtor = null;
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                // HACK we have a special rule to make some additional custom attributes from mscorlib usable:
                // Attributes that have two constructors, one an enum and another one taking a byte, short or int,
                // we only expose the enum constructor.
                if (constructors.Length == 2 && type.Assembly == context.Types.Object.Assembly)
                {
                    ParameterInfo[] p0 = constructors[0].GetParameters();
                    ParameterInfo[] p1 = constructors[1].GetParameters();
                    if (p0.Length == 1 && p1.Length == 1)
                    {
                        Type t0 = p0[0].ParameterType;
                        Type t1 = p1[0].ParameterType;
                        bool swapped = false;
                        if (t1.IsEnum)
                        {
                            Type tmp = t0;
                            t0 = t1;
                            t1 = tmp;
                            swapped = true;
                        }
                        if (t0.IsEnum && (t1 == context.Types.Byte || t1 == context.Types.Int16 || t1 == context.Types.Int32))
                        {
                            if (swapped)
                            {
                                singleOneArgCtor = constructors[1];
                            }
                            else
                            {
                                singleOneArgCtor = constructors[0];
                            }
                            return;
                        }
                    }
                }

                if (type.Assembly == context.Types.Object.Assembly)
                {
                    if (type.FullName == "System.Runtime.CompilerServices.MethodImplAttribute")
                    {
                        foreach (ConstructorInfo ci in constructors)
                        {
                            ParameterInfo[] p = ci.GetParameters();
                            if (p.Length == 1 && p[0].ParameterType.IsEnum)
                            {
                                singleOneArgCtor = ci;
                                return;
                            }
                        }
                    }
                }

                foreach (ConstructorInfo ci in constructors)
                {
                    ParameterInfo[] args = ci.GetParameters();
                    if (args.Length == 0)
                    {
                        defCtor = ci;
                    }
                    else if (args.Length == 1)
                    {
                        if (IsSupportedType(context, args[0].ParameterType))
                        {
                            oneArgCtor = ci;
                            oneArgCtorCount++;
                        }
                        else
                        {
                            // set to two to make sure we don't see the oneArgCtor as viable
                            oneArgCtorCount = 2;
                        }
                    }
                }
                singleOneArgCtor = oneArgCtorCount == 1 ? oneArgCtor : null;
            }

            sealed class AttributeAnnotationJavaMethod : DynamicOnlyJavaMethod
            {

                readonly bool optional;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="type"></param>
                /// <param name="optional"></param>
                internal AttributeAnnotationJavaMethod(AttributeAnnotationJavaType tw, string name, Type type, bool optional) :
                    this(tw, name, MapType(tw.Context, type, false), optional)
                {

                }

                static RuntimeJavaType MapType(RuntimeContext context, Type type, bool isArray)
                {
                    if (type == context.Types.String)
                    {
                        return context.JavaBase.javaLangString;
                    }
                    else if (type == context.Types.Boolean)
                    {
                        return context.PrimitiveJavaTypeFactory.BOOLEAN;
                    }
                    else if (type == context.Types.Byte)
                    {
                        return context.PrimitiveJavaTypeFactory.BYTE;
                    }
                    else if (type == context.Types.Char)
                    {
                        return context.PrimitiveJavaTypeFactory.CHAR;
                    }
                    else if (type == context.Types.Int16)
                    {
                        return context.PrimitiveJavaTypeFactory.SHORT;
                    }
                    else if (type == context.Types.Int32)
                    {
                        return context.PrimitiveJavaTypeFactory.INT;
                    }
                    else if (type == context.Types.Single)
                    {
                        return context.PrimitiveJavaTypeFactory.FLOAT;
                    }
                    else if (type == context.Types.Int64)
                    {
                        return context.PrimitiveJavaTypeFactory.LONG;
                    }
                    else if (type == context.Types.Double)
                    {
                        return context.PrimitiveJavaTypeFactory.DOUBLE;
                    }
                    else if (type == context.Types.Type)
                    {
                        return context.JavaBase.javaLangClass;
                    }
                    else if (type.IsEnum)
                    {
                        foreach (RuntimeJavaType tw in context.ClassLoaderFactory.GetJavaTypeFromType(type).InnerClasses)
                        {
                            if (tw is EnumEnumJavaType)
                            {
                                if (!isArray && type.IsDefined(context.Resolver.ResolveType(typeof(FlagsAttribute).FullName), false))
                                {
                                    return tw.MakeArrayType(1);
                                }
                                return tw;
                            }
                        }
                        throw new InvalidOperationException();
                    }
                    else if (!isArray && ReflectUtil.IsVector(type))
                    {
                        return MapType(context, type.GetElementType(), true).MakeArrayType(1);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="returnType"></param>
                /// <param name="optional"></param>
                private AttributeAnnotationJavaMethod(AttributeAnnotationJavaType tw, string name, RuntimeJavaType returnType, bool optional) :
                    base(tw, name, "()" + returnType.SigName, returnType, Array.Empty<RuntimeJavaType>(), MemberFlags.None)
                {
                    this.optional = optional;
                }

                internal override bool IsOptionalAttributeAnnotationValue
                {
                    get { return optional; }
                }
            }

            protected override void LazyPublishMembers()
            {
                var methods = new List<RuntimeJavaMethod>();
                GetConstructors(Context, attributeType, out var defCtor, out var singleOneArgCtor);

                if (singleOneArgCtor != null)
                    methods.Add(new AttributeAnnotationJavaMethod(this, "value", singleOneArgCtor.GetParameters()[0].ParameterType, defCtor != null));

                foreach (var pi in attributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    // the getter and setter methods both need to be public
                    // the getter signature must be: <PropertyType> Getter()
                    // the setter signature must be: void Setter(<PropertyType>)
                    // the property type needs to be a supported type
                    var getter = pi.GetGetMethod();
                    var setter = pi.GetSetMethod();
                    ParameterInfo[] parameters;
                    if (getter != null && getter.GetParameters().Length == 0 && getter.ReturnType == pi.PropertyType && setter != null && (parameters = setter.GetParameters()).Length == 1 && parameters[0].ParameterType == pi.PropertyType && setter.ReturnType == Context.Types.Void && IsSupportedType(Context, pi.PropertyType))
                        AddMethodIfUnique(methods, new AttributeAnnotationJavaMethod(this, pi.Name, pi.PropertyType, true));
                }

                foreach (var fi in attributeType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    if (!fi.IsInitOnly && IsSupportedType(Context, fi.FieldType))
                        AddMethodIfUnique(methods, new AttributeAnnotationJavaMethod(this, fi.Name, fi.FieldType, true));

                SetMethods(methods.ToArray());

                base.LazyPublishMembers();
            }

            static void AddMethodIfUnique(List<RuntimeJavaMethod> methods, RuntimeJavaMethod method)
            {
                foreach (var mw in methods)
                {
                    if (mw.Name == method.Name && mw.Signature == method.Signature)
                    {
                        // ignore duplicate
                        return;
                    }
                }

                methods.Add(method);
            }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override object GetAnnotationDefault(RuntimeJavaMethod mw)
            {
                if (mw.IsOptionalAttributeAnnotationValue)
                {
                    if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.BOOLEAN)
                    {
                        return java.lang.Boolean.FALSE;
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.BYTE)
                    {
                        return java.lang.Byte.valueOf((byte)0);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.CHAR)
                    {
                        return java.lang.Character.valueOf((char)0);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.SHORT)
                    {
                        return java.lang.Short.valueOf((short)0);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.INT)
                    {
                        return java.lang.Integer.valueOf(0);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.FLOAT)
                    {
                        return java.lang.Float.valueOf(0F);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.LONG)
                    {
                        return java.lang.Long.valueOf(0L);
                    }
                    else if (mw.ReturnType == Context.PrimitiveJavaTypeFactory.DOUBLE)
                    {
                        return java.lang.Double.valueOf(0D);
                    }
                    else if (mw.ReturnType == Context.JavaBase.javaLangString)
                    {
                        return "";
                    }
                    else if (mw.ReturnType == Context.JavaBase.javaLangClass)
                    {
                        return (java.lang.Class)typeof(ikvm.@internal.__unspecified);
                    }
                    else if (mw.ReturnType is EnumEnumJavaType)
                    {
                        EnumEnumJavaType eetw = (EnumEnumJavaType)mw.ReturnType;
                        return eetw.GetUnspecifiedValue();
                    }
                    else if (mw.ReturnType.IsArray)
                    {
                        return Array.CreateInstance(mw.ReturnType.TypeAsArrayType, 0);
                    }
                }

                return null;
            }

#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override RuntimeJavaType DeclaringTypeWrapper => Context.ClassLoaderFactory.GetJavaTypeFromType(attributeType);

            internal override Type TypeAsTBD => fakeType;

            internal override RuntimeJavaType[] InnerClasses => innerClasses ??= GetInnerClasses();

            RuntimeJavaType[] GetInnerClasses()
            {
                List<RuntimeJavaType> list = new List<RuntimeJavaType>();
                AttributeUsageAttribute attr = GetAttributeUsage();
                if ((attr.ValidOn & AttributeTargets.ReturnValue) != 0)
                {
                    list.Add(GetClassLoader().RegisterInitiatingLoader(new ReturnValueAnnotationJavaType(Context, this)));
                }
                if (attr.AllowMultiple)
                {
                    list.Add(GetClassLoader().RegisterInitiatingLoader(new MultipleAnnotationJavaType(Context, this)));
                }
                return list.ToArray();
            }

            internal override bool IsFakeTypeContainer => true;

            AttributeUsageAttribute GetAttributeUsage()
            {
                AttributeTargets validOn = AttributeTargets.All;
                bool allowMultiple = false;
                bool inherited = true;
                foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(attributeType))
                {
                    if (cad.Constructor.DeclaringType == Context.Resolver.ResolveType(typeof(AttributeUsageAttribute).FullName))
                    {
                        if (cad.ConstructorArguments.Count == 1 && cad.ConstructorArguments[0].ArgumentType == Context.Resolver.ResolveType(typeof(AttributeTargets).FullName))
                        {
                            validOn = (AttributeTargets)cad.ConstructorArguments[0].Value;
                        }
                        foreach (CustomAttributeNamedArgument cana in cad.NamedArguments)
                        {
                            if (cana.MemberInfo.Name == "AllowMultiple")
                            {
                                allowMultiple = (bool)cana.TypedValue.Value;
                            }
                            else if (cana.MemberInfo.Name == "Inherited")
                            {
                                inherited = (bool)cana.TypedValue.Value;
                            }
                        }
                    }
                }
                AttributeUsageAttribute attr = new AttributeUsageAttribute(validOn);
                attr.AllowMultiple = allowMultiple;
                attr.Inherited = inherited;
                return attr;
            }

#if !IMPORTER && !FIRST_PASS && !EXPORTER
            internal override object[] GetDeclaredAnnotations()
            {
                // note that AttributeUsageAttribute.Inherited does not map to java.lang.annotation.Inherited
                AttributeTargets validOn = GetAttributeUsage().ValidOn;
                List<java.lang.annotation.ElementType> targets = new List<java.lang.annotation.ElementType>();
                if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.TYPE);
                }
                if ((validOn & AttributeTargets.Constructor) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.CONSTRUCTOR);
                }
                if ((validOn & AttributeTargets.Field) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.FIELD);
                }
                if ((validOn & AttributeTargets.Method) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.METHOD);
                }
                if ((validOn & AttributeTargets.Parameter) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.PARAMETER);
                }
                java.util.HashMap targetMap = new java.util.HashMap();
                targetMap.put("value", targets.ToArray());
                java.util.HashMap retentionMap = new java.util.HashMap();
                retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
                return new object[] {
                    java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
                    java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
                };
            }
#endif

            sealed class AttributeAnnotation : Annotation
            {

                readonly Type type;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="type"></param>
                internal AttributeAnnotation(Type type)
                {
                    this.type = type;
                }

                CustomAttributeBuilder MakeCustomAttributeBuilder(RuntimeClassLoader loader, object annotation)
                {
                    object[] arr = (object[])annotation;
                    ConstructorInfo defCtor;
                    ConstructorInfo singleOneArgCtor;
                    object ctorArg = null;
                    GetConstructors(loader.Context, type, out defCtor, out singleOneArgCtor);
                    List<PropertyInfo> properties = new List<PropertyInfo>();
                    List<object> propertyValues = new List<object>();
                    List<FieldInfo> fields = new List<FieldInfo>();
                    List<object> fieldValues = new List<object>();
                    for (int i = 2; i < arr.Length; i += 2)
                    {
                        string name = (string)arr[i];
                        if (name == "value" && singleOneArgCtor != null)
                        {
                            ctorArg = ConvertValue(loader, singleOneArgCtor.GetParameters()[0].ParameterType, arr[i + 1]);
                        }
                        else
                        {
                            PropertyInfo pi = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                            if (pi != null)
                            {
                                properties.Add(pi);
                                propertyValues.Add(ConvertValue(loader, pi.PropertyType, arr[i + 1]));
                            }
                            else
                            {
                                FieldInfo fi = type.GetField(name, BindingFlags.Public | BindingFlags.Instance);
                                if (fi != null)
                                {
                                    fields.Add(fi);
                                    fieldValues.Add(ConvertValue(loader, fi.FieldType, arr[i + 1]));
                                }
                            }
                        }
                    }
                    if (ctorArg == null && defCtor == null)
                    {
                        // TODO required argument is missing
                    }
                    return new CustomAttributeBuilder(ctorArg == null ? defCtor : singleOneArgCtor,
                        ctorArg == null ? new object[0] : new object[] { ctorArg },
                        properties.ToArray(),
                        propertyValues.ToArray(),
                        fields.ToArray(),
                        fieldValues.ToArray());
                }

                internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
                {
                    if (type == loader.Context.Resolver.ResolveType(typeof(System.Runtime.InteropServices.StructLayoutAttribute).FullName) && tb.BaseType != loader.Context.Types.Object)
                    {
                        // we have to handle this explicitly, because if we apply an illegal StructLayoutAttribute,
                        // TypeBuilder.CreateType() will later on throw an exception.
#if IMPORTER
                        loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Type '" + tb.FullName + "' does not extend cli.System.Object");
#else
                        Tracer.Error(Tracer.Runtime, "StructLayoutAttribute cannot be applied to {0}, because it does not directly extend cli.System.Object", tb.FullName);
#endif
                        return;
                    }

                    tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, MethodBuilder mb, object annotation)
                {
                    mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, FieldBuilder fb, object annotation)
                {
                    fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, ParameterBuilder pb, object annotation)
                {
                    // TODO with the current custom attribute annotation restrictions it is impossible to use this CA,
                    // but if we make it possible, we should also implement it here
                    if (type == loader.Context.Resolver.ResolveType(typeof(System.Runtime.InteropServices.DefaultParameterValueAttribute).FullName))
                        throw new NotImplementedException();
                    else
                        pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
                {
#if IMPORTER
                    if (type == loader.Context.Resolver.ResolveType(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute).FullName))
                    {
                        ab.__AddTypeForwarder((Type)ConvertValue(loader, loader.Context.Types.Type, ((object[])annotation)[3]));
                    }
                    else if (type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyVersionAttribute).FullName))
                    {
                        string str = (string)ConvertValue(loader, loader.Context.Types.String, ((object[])annotation)[3]);
                        Version version;
                        if (IkvmImporterInternal.TryParseVersion(str, out version))
                        {
                            ab.__SetAssemblyVersion(version);
                        }
                        else
                        {
                            loader.IssueMessage(Message.InvalidCustomAttribute, type.FullName, "The version '" + str + "' is invalid.");
                        }
                    }
                    else if (type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyCultureAttribute).FullName))
                    {
                        string str = (string)ConvertValue(loader, loader.Context.Types.String, ((object[])annotation)[3]);
                        if (str != "")
                        {
                            ab.__SetAssemblyCulture(str);
                        }
                    }
                    else if (type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyDelaySignAttribute).FullName)
                        || type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyKeyFileAttribute).FullName)
                        || type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyKeyNameAttribute).FullName))
                    {
                        loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Please use the corresponding compiler switch.");
                    }
                    else if (type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyAlgorithmIdAttribute).FullName))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else if (type == loader.Context.Resolver.ResolveType(typeof(System.Reflection.AssemblyFlagsAttribute).FullName))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else
                    {
                        ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                    }
#else
                    ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
#endif
                }

                internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
                {
                    pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override bool IsCustomAttribute => true;

            }

            internal override Annotation Annotation => new AttributeAnnotation(attributeType);

            internal override AttributeTargets AttributeTargets => GetAttributeUsage().ValidOn;

        }

    }

}
