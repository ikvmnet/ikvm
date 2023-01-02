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
using System.Diagnostics;

using IKVM.Attributes;
using IKVM.Runtime;

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

namespace IKVM.Internal
{

    abstract class Annotation
    {

#if IMPORTER

        internal static Annotation LoadAssemblyCustomAttribute(ClassLoaderWrapper loader, object[] def)
        {
            if (def.Length == 0)
                throw new ArgumentException("LoadAssemblyCustomAttribute did not receive any definitions.");
            if (object.Equals(def[0], AnnotationDefaultAttribute.TAG_ANNOTATION) == false)
                throw new InternalException("LoadAssemblyCustomAttribute did not receive AnnotationDefaultAttribute.TAG_ANNOTATION.");

            string annotationClass = (string)def[1];
            if (ClassFile.IsValidFieldSig(annotationClass))
            {
                try
                {
                    return loader.RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.LoadOrThrow).Annotation;
                }
                catch (RetargetableJavaException)
                {

                }
            }
            return null;
        }

#endif

#if !EXPORTER
        // NOTE this method returns null if the type could not be found
        // or if the type is not a Custom Attribute and we're not in the static compiler
        internal static Annotation Load(TypeWrapper owner, object[] def)
        {
            Debug.Assert(def[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION));
            string annotationClass = (string)def[1];
#if !IMPORTER
            if (!annotationClass.EndsWith("$Annotation;")
                && !annotationClass.EndsWith("$Annotation$__ReturnValue;")
                && !annotationClass.EndsWith("$Annotation$__Multiple;"))
            {
                // we don't want to try to load an annotation in dynamic mode,
                // unless it is a .NET custom attribute (which can affect runtime behavior)
                return null;
            }
#endif
            if (ClassFile.IsValidFieldSig(annotationClass))
            {
                TypeWrapper tw = owner.GetClassLoader().RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.Link);
                // Java allows inaccessible annotations to be used, so when the annotation isn't visible
                // we fall back to using the DynamicAnnotationAttribute.
                if (!tw.IsUnloadable && tw.IsAccessibleFrom(owner))
                {
                    return tw.Annotation;
                }
            }
            Tracer.Warning(Tracer.Compiler, "Unable to load annotation class {0}", annotationClass);
#if IMPORTER
            return new CompiledTypeWrapper.CompiledAnnotation(StaticCompiler.GetRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute"));
#else
            return null;
#endif
        }
#endif

        private static object LookupEnumValue(Type enumType, string value)
        {
            FieldInfo field = enumType.GetField(value, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                return field.GetRawConstantValue();
            }
            // both __unspecified and missing values end up here
            return EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(enumType), 0);
        }

        protected static object ConvertValue(ClassLoaderWrapper loader, Type targetType, object obj)
        {
            if (targetType.IsEnum)
            {
                // TODO check the obj descriptor matches the type we expect
                if (((object[])obj)[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
                {
                    object[] arr = (object[])obj;
                    object value = null;
                    for (int i = 1; i < arr.Length; i++)
                    {
                        // TODO check the obj descriptor matches the type we expect
                        string s = ((object[])arr[i])[2].ToString();
                        object newval = LookupEnumValue(targetType, s);
                        if (value == null)
                        {
                            value = newval;
                        }
                        else
                        {
                            value = EnumHelper.OrBoxedIntegrals(value, newval);
                        }
                    }
                    return value;
                }
                else
                {
                    string s = ((object[])obj)[2].ToString();
                    if (s == "__unspecified")
                    {
                        // TODO we should probably return null and handle that
                    }
                    return LookupEnumValue(targetType, s);
                }
            }
            else if (targetType == Types.Type)
            {
                // TODO check the obj descriptor matches the type we expect
                return loader.FieldTypeWrapperFromSig(((string)((object[])obj)[1]).Replace('/', '.'), LoadMode.LoadOrThrow).TypeAsTBD;
            }
            else if (targetType.IsArray)
            {
                // TODO check the obj descriptor matches the type we expect
                object[] arr = (object[])obj;
                Type elementType = targetType.GetElementType();
                object[] targetArray = new object[arr.Length - 1];
                for (int i = 1; i < arr.Length; i++)
                {
                    targetArray[i - 1] = ConvertValue(loader, elementType, arr[i]);
                }
                return targetArray;
            }
            else
            {
                return obj;
            }
        }

        internal static bool HasRetentionPolicyRuntime(object[] annotations)
        {
            if (annotations != null)
            {
                foreach (object[] def in annotations)
                {
                    if (def[1].Equals("Ljava/lang/annotation/Retention;"))
                    {
                        for (int i = 2; i < def.Length; i += 2)
                        {
                            if (def[i].Equals("value"))
                            {
                                object[] val = def[i + 1] as object[];
                                if (val != null
                                    && val.Length == 3
                                    && val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
                                    && val[1].Equals("Ljava/lang/annotation/RetentionPolicy;")
                                    && val[2].Equals("RUNTIME"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        internal static bool HasObsoleteAttribute(object[] annotations)
        {
            if (annotations != null)
            {
                foreach (object[] def in annotations)
                {
                    if (def[1].Equals("Lcli/System/ObsoleteAttribute$Annotation;"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected static object QualifyClassNames(ClassLoaderWrapper loader, object annotation)
        {
            bool copy = false;
            object[] def = (object[])annotation;
            for (int i = 3; i < def.Length; i += 2)
            {
                object[] val = def[i] as object[];
                if (val != null)
                {
                    object[] newval = ValueQualifyClassNames(loader, val);
                    if (newval != val)
                    {
                        if (!copy)
                        {
                            copy = true;
                            object[] newdef = new object[def.Length];
                            Array.Copy(def, newdef, def.Length);
                            def = newdef;
                        }
                        def[i] = newval;
                    }
                }
            }
            return def;
        }

        private static object[] ValueQualifyClassNames(ClassLoaderWrapper loader, object[] val)
        {
            if (val[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
            {
                return (object[])QualifyClassNames(loader, val);
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_CLASS))
            {
                string sig = (string)val[1];
                if (sig.StartsWith("L"))
                {
                    TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
                    if (tw != null)
                    {
                        return new object[] { AnnotationDefaultAttribute.TAG_CLASS, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";" };
                    }
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM))
            {
                string sig = (string)val[1];
                TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
                if (tw != null)
                {
                    return new object[] { AnnotationDefaultAttribute.TAG_ENUM, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";", val[2] };
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
            {
                bool copy = false;
                for (int i = 1; i < val.Length; i++)
                {
                    object[] nval = val[i] as object[];
                    if (nval != null)
                    {
                        object newnval = ValueQualifyClassNames(loader, nval);
                        if (newnval != nval)
                        {
                            if (!copy)
                            {
                                copy = true;
                                object[] newval = new object[val.Length];
                                Array.Copy(val, newval, val.Length);
                                val = newval;
                            }
                            val[i] = newnval;
                        }
                    }
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ERROR))
            {
                return val;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        internal abstract void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation);

        internal virtual void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
        {

        }

        internal abstract bool IsCustomAttribute { get; }

    }

}
