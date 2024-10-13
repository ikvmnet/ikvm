﻿/*
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

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

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

#if EXPORTER
using IKVM.Tools.Exporter;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed partial class AttributeAnnotationJavaType : AttributeAnnotationJavaTypeBase
        {

            readonly ITypeSymbol fakeType;
            readonly ITypeSymbol attributeType;
            volatile RuntimeJavaType[] innerClasses;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="attributeType"></param>
            internal AttributeAnnotationJavaType(RuntimeContext context, string name, ITypeSymbol attributeType) :
                base(context, name)
            {
#if IMPORTER || EXPORTER
                this.fakeType = context.FakeTypes.GetAttributeType(attributeType);
#elif !FIRST_PASS
                this.fakeType = context.Resolver.GetSymbol(typeof(ikvm.@internal.AttributeAnnotation<>)).MakeGenericType(attributeType);
#endif
                this.attributeType = attributeType;
            }

            static bool IsSupportedType(RuntimeContext context, ITypeSymbol type)
            {
                // Java annotations only support one-dimensional arrays
                if (type.IsSZArray)
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

            internal static void GetConstructors(RuntimeContext context, ITypeSymbol type, out IConstructorSymbol defCtor, out IConstructorSymbol singleOneArgCtor)
            {
                defCtor = null;
                int oneArgCtorCount = 0;
                IConstructorSymbol oneArgCtor = null;
                var constructors = type.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                // HACK we have a special rule to make some additional custom attributes from mscorlib usable:
                // Attributes that have two constructors, one an enum and another one taking a byte, short or int,
                // we only expose the enum constructor.
                if (constructors.Length == 2 && type.Assembly == context.Types.Object.Assembly)
                {
                    var p0 = constructors[0].GetParameters();
                    var p1 = constructors[1].GetParameters();
                    if (p0.Length == 1 && p1.Length == 1)
                    {
                        var t0 = p0[0].ParameterType;
                        var t1 = p1[0].ParameterType;
                        bool swapped = false;
                        if (t1.IsEnum)
                        {
                            var tmp = t0;
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
                        foreach (var ci in constructors)
                        {
                            var p = ci.GetParameters();
                            if (p.Length == 1 && p[0].ParameterType.IsEnum)
                            {
                                singleOneArgCtor = ci;
                                return;
                            }
                        }
                    }
                }

                foreach (var ci in constructors)
                {
                    var args = ci.GetParameters();
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
                internal AttributeAnnotationJavaMethod(AttributeAnnotationJavaType tw, string name, ITypeSymbol type, bool optional) :
                    this(tw, name, MapType(tw.Context, type, false), optional)
                {

                }

                static RuntimeJavaType MapType(RuntimeContext context, ITypeSymbol type, bool isArray)
                {
                    if (type == context.Types.String)
                    {
                        return context.JavaBase.TypeOfJavaLangString;
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
                        return context.JavaBase.TypeOfJavaLangClass;
                    }
                    else if (type.IsEnum)
                    {
                        foreach (var tw in context.ClassLoaderFactory.GetJavaTypeFromType(type).InnerClasses)
                        {
                            if (tw is EnumEnumJavaType)
                            {
                                if (!isArray && type.IsDefined(context.Resolver.ResolveCoreType(typeof(FlagsAttribute).FullName), false))
                                    return tw.MakeArrayType(1);

                                return tw;
                            }
                        }

                        throw new InvalidOperationException();
                    }
                    else if (isArray == false && type.IsSZArray)
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

                foreach (var pi in attributeType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    // the getter and setter methods both need to be public
                    // the getter signature must be: <PropertyType> Getter()
                    // the setter signature must be: void Setter(<PropertyType>)
                    // the property type needs to be a supported type
                    var getter = pi.GetGetMethod();
                    var setter = pi.GetSetMethod();
                    IParameterSymbol[] parameters;
                    if (getter != null && getter.GetParameters().Length == 0 && getter.ReturnType == pi.PropertyType && setter != null && (parameters = setter.GetParameters()).Length == 1 && parameters[0].ParameterType == pi.PropertyType && setter.ReturnType == Context.Types.Void && IsSupportedType(Context, pi.PropertyType))
                        AddMethodIfUnique(methods, new AttributeAnnotationJavaMethod(this, pi.Name, pi.PropertyType, true));
                }

                foreach (var fi in attributeType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
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
                    else if (mw.ReturnType == Context.JavaBase.TypeOfJavaLangString)
                    {
                        return "";
                    }
                    else if (mw.ReturnType == Context.JavaBase.TypeOfJavaLangClass)
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
                        return Array.CreateInstance(mw.ReturnType.TypeAsArrayType.GetUnderlyingType(), 0);
                    }
                }

                return null;
            }

#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override RuntimeJavaType DeclaringTypeWrapper => Context.ClassLoaderFactory.GetJavaTypeFromType(attributeType);

            internal override ITypeSymbol TypeAsTBD => fakeType;

            internal override RuntimeJavaType[] InnerClasses => innerClasses ??= GetInnerClasses();

            RuntimeJavaType[] GetInnerClasses()
            {
                List<RuntimeJavaType> list = new List<RuntimeJavaType>();
                AttributeUsageAttribute attr = GetAttributeUsage();
                if ((attr.ValidOn & AttributeTargets.ReturnValue) != 0)
                {
                    list.Add(ClassLoader.RegisterInitiatingLoader(new ReturnValueAnnotationJavaType(Context, this)));
                }
                if (attr.AllowMultiple)
                {
                    list.Add(ClassLoader.RegisterInitiatingLoader(new MultipleAnnotationJavaType(Context, this)));
                }
                return list.ToArray();
            }

            internal override bool IsFakeTypeContainer => true;

            AttributeUsageAttribute GetAttributeUsage()
            {
                AttributeTargets validOn = AttributeTargets.All;
                bool allowMultiple = false;
                bool inherited = true;
                foreach (var cad in attributeType.GetCustomAttributes())
                {
                    if (cad.Constructor.DeclaringType == Context.Resolver.ResolveCoreType(typeof(AttributeUsageAttribute).FullName))
                    {
                        if (cad.ConstructorArguments.Length == 1 && cad.ConstructorArguments[0].ArgumentType == Context.Resolver.ResolveCoreType(typeof(AttributeTargets).FullName))
                        {
                            validOn = (AttributeTargets)cad.ConstructorArguments[0].Value;
                        }

                        foreach (var cana in cad.NamedArguments)
                        {
                            if (cana.MemberInfo.Name == "AllowMultiple")
                                allowMultiple = (bool)cana.TypedValue.Value;
                            else if (cana.MemberInfo.Name == "Inherited")
                                inherited = (bool)cana.TypedValue.Value;
                        }
                    }
                }

                var attr = new AttributeUsageAttribute(validOn);
                attr.AllowMultiple = allowMultiple;
                attr.Inherited = inherited;
                return attr;
            }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override object[] GetDeclaredAnnotations()
            {
                // note that AttributeUsageAttribute.Inherited does not map to java.lang.annotation.Inherited
                var validOn = GetAttributeUsage().ValidOn;
                var targets = new List<java.lang.annotation.ElementType>();

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
                return [
                    java.lang.reflect.Proxy.newProxyInstance(null, [typeof(java.lang.annotation.Target)], new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
                    java.lang.reflect.Proxy.newProxyInstance(null, [typeof(java.lang.annotation.Retention)], new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
                ];
            }
#endif

            sealed class AttributeAnnotation : Annotation
            {

                readonly ITypeSymbol type;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="type"></param>
                internal AttributeAnnotation(ITypeSymbol type)
                {
                    this.type = type;
                }

                CustomAttribute MakeCustomAttributeBuilder(RuntimeClassLoader loader, object annotation)
                {
                    object[] arr = (object[])annotation;
                    object ctorArg = null;

                    GetConstructors(loader.Context, type, out var defCtor, out var singleOneArgCtor);
                    var properties = new List<IPropertySymbol>();
                    var propertyValues = new List<object>();
                    var fields = new List<IFieldSymbol>();
                    var fieldValues = new List<object>();

                    for (int i = 2; i < arr.Length; i += 2)
                    {
                        string name = (string)arr[i];
                        if (name == "value" && singleOneArgCtor != null)
                        {
                            ctorArg = ConvertValue(loader, singleOneArgCtor.GetParameters()[0].ParameterType, arr[i + 1]);
                        }
                        else
                        {
                            var pi = type.GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            if (pi != null)
                            {
                                properties.Add(pi);
                                propertyValues.Add(ConvertValue(loader, pi.PropertyType, arr[i + 1]));
                            }
                            else
                            {
                                var fi = type.GetField(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
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

                    return CustomAttribute.Create(
                        ctorArg == null ? defCtor : singleOneArgCtor,
                        ctorArg == null ? [] : new object[] { ctorArg },
                        properties.ToArray(),
                        propertyValues.ToArray(),
                        fields.ToArray(),
                        fieldValues.ToArray());
                }

                internal override void Apply(RuntimeClassLoader loader, ITypeSymbolBuilder tb, object annotation)
                {
                    if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Runtime.InteropServices.StructLayoutAttribute).FullName) && tb.BaseType != loader.Context.Types.Object)
                    {
                        // we have to handle this explicitly, because if we apply an illegal StructLayoutAttribute,
                        // TypeBuilder.CreateType() will later on throw an exception.
#if IMPORTER
                        loader.Diagnostics.IgnoredCustomAttribute(type.FullName, $"Type '{tb.FullName}' does not extend cli.System.Object");
#else
                        loader.Diagnostics.GenericRuntimeError($"StructLayoutAttribute cannot be applied to {tb.FullName}, because it does not directly extend cli.System.Object");
#endif
                        return;
                    }

                    tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, IMethodBaseSymbolBuilder mb, object annotation)
                {
                    mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, IFieldSymbolBuilder fb, object annotation)
                {
                    fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, IParameterSymbolBuilder pb, object annotation)
                {
                    // TODO with the current custom attribute annotation restrictions it is impossible to use this CA,
                    // but if we make it possible, we should also implement it here
                    if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Runtime.InteropServices.DefaultParameterValueAttribute).FullName))
                        throw new NotImplementedException();
                    else
                        pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, IAssemblySymbolBuilder assemblyBuilder, object annotation)
                {
#if IMPORTER
                    var ab = assemblyBuilder.GetUnderlyingAssemblyBuilder();

                    if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute).FullName))
                    {
                        assemblyBuilder.AddTypeForwarder((ITypeSymbol)ConvertValue(loader, loader.Context.Types.Type, ((object[])annotation)[3]));
                    }
                    else if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyVersionAttribute).FullName))
                    {
                        string str = (string)ConvertValue(loader, loader.Context.Types.String, ((object[])annotation)[3]);
                        Version version;
                        if (ImportContextFactory.TryParseVersion(str, out version))
                        {
                            ab.__SetAssemblyVersion(version);
                        }
                        else
                        {
                            loader.Diagnostics.InvalidCustomAttribute(type.FullName, "The version '" + str + "' is invalid.");
                        }
                    }
                    else if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyCultureAttribute).FullName))
                    {
                        string str = (string)ConvertValue(loader, loader.Context.Types.String, ((object[])annotation)[3]);
                        if (str != "")
                        {
                            ab.__SetAssemblyCulture(str);
                        }
                    }
                    else if (
                        type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyDelaySignAttribute).FullName) ||
                        type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyKeyFileAttribute).FullName) ||
                        type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyKeyNameAttribute).FullName))
                    {
                        loader.Diagnostics.IgnoredCustomAttribute(type.FullName, "Please use the corresponding compiler switch.");
                    }
                    else if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyAlgorithmIdAttribute).FullName))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else if (type == loader.Context.Resolver.ResolveCoreType(typeof(System.Reflection.AssemblyFlagsAttribute).FullName))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else
                    {
                        assemblyBuilder.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                    }
#else
                    assemblyBuilder.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
#endif
                }

                internal override void Apply(RuntimeClassLoader loader, IPropertySymbolBuilder pb, object annotation)
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
