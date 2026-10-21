/*
  Copyright (C) 2007-2013 Jeroen Frijters

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

using IKVM.CoreLib.Runtime;
using IKVM.Runtime;

namespace IKVM.Java.Externs.java.lang.reflect
{

    static class Executable
    {

        public static object[] getParameters0(global::java.lang.reflect.Executable _this)
        {
#if FIRST_PASS
		    throw new NotImplementedException();
#else
            var mw = RuntimeJavaMethod.FromExecutable(_this);
            var methodParameters = mw.DeclaringType.GetMethodParameters(mw);
            if (methodParameters == null)
                return null;

            if (methodParameters == MethodParametersEntry.Malformed)
                throw new global::java.lang.reflect.MalformedParametersException("Invalid constant pool index");

            var parameters = new global::java.lang.reflect.Parameter[methodParameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new global::java.lang.reflect.Parameter(methodParameters[i].name ?? "", (int)(ushort)methodParameters[i].accessFlags, _this, i);

            return parameters;
#endif
        }

        public static byte[] getTypeAnnotationBytes0(global::java.lang.reflect.Executable _this)
        {
            var mw = RuntimeJavaMethod.FromExecutable(_this);
            return mw.DeclaringType.GetMethodRawTypeAnnotations(mw);
        }

        public static object declaredAnnotationsImpl(global::java.lang.reflect.Executable executable)
        {
            var mw = RuntimeJavaMethod.FromExecutable(executable);
            return IKVM.Java.Externs.java.lang.Class.AnnotationsToMap(mw.DeclaringType.ClassLoader, mw.DeclaringType.GetMethodAnnotations(mw));
        }

        public static object[][] sharedGetParameterAnnotationsImpl(global::java.lang.reflect.Executable executable)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var mw = RuntimeJavaMethod.FromExecutable(executable);
            var objAnn = mw.DeclaringType.GetParameterAnnotations(mw);
            if (objAnn == null)
                return null;

            var ann = new global::java.lang.annotation.Annotation[objAnn.Length][];
            for (int i = 0; i < ann.Length; i++)
            {
                var list = new List<global::java.lang.annotation.Annotation>();
                foreach (var obj in objAnn[i])
                {
                    var a = obj as global::java.lang.annotation.Annotation;
                    if (a != null)
                    {
                        list.Add(IKVM.Java.Externs.java.lang.Class.FreezeOrWrapAttribute(a));
                    }
                    else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
                    {
                        a = (global::java.lang.annotation.Annotation)JVM.NewAnnotation(mw.DeclaringType.ClassLoader.GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition);
                        if (a != null)
                            list.Add(a);
                    }
                }

                ann[i] = list.ToArray();
            }

            return ann;
#endif
        }

    }

}
