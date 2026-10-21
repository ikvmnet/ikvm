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

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Runtime;

namespace IKVM.Runtime
{

#if IMPORTER
    abstract partial class RuntimeByteCodeJavaType : RuntimeJavaType
#else
#pragma warning disable 628 // don't complain about protected members in sealed type
    sealed partial class RuntimeByteCodeJavaType
#endif
    {

        sealed class Metadata
        {

            internal static Metadata Create(ClassFile classFile)
            {
                if (classFile.MajorVersion < 49)
                    return null;

                string[][] genericMetaData = null;
                object[][] annotations = null;
                MethodParametersEntry[][] methodParameters = null;
                TypeAnnotationTable runtimeVisibleTypeAnnotations = TypeAnnotationTable.Empty;
                TypeAnnotationTable[] fieldRuntimeVisibleTypeAnnotations = null;
                TypeAnnotationTable[] methodRuntimeVisibleTypeAnnotations = null;

                if (classFile.EnclosingMethod != null)
                {
                    genericMetaData ??= new string[4][];
                    genericMetaData[2] = classFile.EnclosingMethod;
                }

                if (classFile.GenericSignature != null)
                {
                    genericMetaData ??= new string[4][];
                    genericMetaData[3] = new string[] { classFile.GenericSignature };
                }

                if (classFile.Annotations != null)
                {
                    annotations ??= new object[5][];
                    annotations[0] = classFile.Annotations;
                }

                if (classFile.RuntimeVisibleTypeAnnotations.Count > 0)
                {
                    runtimeVisibleTypeAnnotations = classFile.RuntimeVisibleTypeAnnotations;
                }

                for (int i = 0; i < classFile.Methods.Length; i++)
                {
                    if (classFile.Methods[i].GenericSignature != null)
                    {
                        genericMetaData ??= new string[4][];
                        genericMetaData[0] ??= new string[classFile.Methods.Length];
                        genericMetaData[0][i] = classFile.Methods[i].GenericSignature;
                    }

                    if (classFile.Methods[i].Annotations != null)
                    {
                        annotations ??= new object[5][];
                        annotations[1] ??= new object[classFile.Methods.Length];
                        annotations[1][i] = classFile.Methods[i].Annotations;
                    }

                    if (classFile.Methods[i].ParameterAnnotations != null)
                    {
                        annotations ??= new object[5][];
                        annotations[2] ??= new object[classFile.Methods.Length];
                        annotations[2][i] = classFile.Methods[i].ParameterAnnotations;
                    }

                    if (classFile.Methods[i].AnnotationDefault != null)
                    {
                        annotations ??= new object[5][];
                        annotations[3] ??= new object[classFile.Methods.Length];
                        annotations[3][i] = classFile.Methods[i].AnnotationDefault;
                    }

                    if (classFile.Methods[i].MethodParameters != null)
                    {
                        methodParameters ??= new MethodParametersEntry[classFile.Methods.Length][];
                        methodParameters[i] = classFile.Methods[i].MethodParameters;
                    }

                    if (classFile.Methods[i].RuntimeVisibleTypeAnnotations.Count > 0)
                    {
                        methodRuntimeVisibleTypeAnnotations ??= new TypeAnnotationTable[classFile.Methods.Length];
                        methodRuntimeVisibleTypeAnnotations[i] = classFile.Methods[i].RuntimeVisibleTypeAnnotations;
                    }
                }

                for (int i = 0; i < classFile.Fields.Length; i++)
                {
                    if (classFile.Fields[i].GenericSignature != null)
                    {
                        genericMetaData ??= new string[4][];
                        genericMetaData[1] ??= new string[classFile.Fields.Length];
                        genericMetaData[1][i] = classFile.Fields[i].GenericSignature;
                    }

                    if (classFile.Fields[i].Annotations != null)
                    {
                        annotations ??= new object[5][];
                        annotations[4] ??= new object[classFile.Fields.Length][];
                        annotations[4][i] = classFile.Fields[i].Annotations;
                    }

                    if (classFile.Fields[i].RuntimeVisibleTypeAnnotations.Count > 0)
                    {
                        fieldRuntimeVisibleTypeAnnotations ??= new TypeAnnotationTable[classFile.Fields.Length];
                        fieldRuntimeVisibleTypeAnnotations[i] = classFile.Fields[i].RuntimeVisibleTypeAnnotations;
                    }
                }

                if (genericMetaData != null ||
                    annotations != null ||
                    methodParameters != null ||
                    runtimeVisibleTypeAnnotations.Count > 0 ||
                    fieldRuntimeVisibleTypeAnnotations != null ||
                    methodRuntimeVisibleTypeAnnotations != null)
                {
                    var constantPool = runtimeVisibleTypeAnnotations.Count > 0 || fieldRuntimeVisibleTypeAnnotations != null || methodRuntimeVisibleTypeAnnotations != null ? classFile.GetConstantPool() : null;
                    return new Metadata(genericMetaData, annotations, methodParameters, runtimeVisibleTypeAnnotations, fieldRuntimeVisibleTypeAnnotations, methodRuntimeVisibleTypeAnnotations, constantPool);
                }

                return null;
            }

            internal static string GetGenericSignature(Metadata m)
            {
                if (m != null && m.genericMetaData != null && m.genericMetaData[3] != null)
                    return m.genericMetaData[3][0];

                return null;
            }

            internal static string[] GetEnclosingMethod(Metadata m)
            {
                if (m != null && m.genericMetaData != null)
                    return m.genericMetaData[2];

                return null;
            }

            internal static string GetGenericMethodSignature(Metadata m, int index)
            {
                if (m != null && m.genericMetaData != null && m.genericMetaData[0] != null)
                    return m.genericMetaData[0][index];

                return null;
            }

            internal static string GetGenericFieldSignature(Metadata m, int index)
            {
                if (m != null && m.genericMetaData != null && m.genericMetaData[1] != null)
                    return m.genericMetaData[1][index];

                return null;
            }

            internal static object[] GetAnnotations(Metadata m)
            {
                if (m != null && m.annotations != null)
                    return m.annotations[0];

                return null;
            }

            internal static object[] GetMethodAnnotations(Metadata m, int index)
            {
                if (m != null && m.annotations != null && m.annotations[1] != null)
                    return (object[])m.annotations[1][index];

                return null;
            }

            internal static object[][] GetMethodParameterAnnotations(Metadata m, int index)
            {
                if (m != null && m.annotations != null && m.annotations[2] != null)
                    return (object[][])m.annotations[2][index];

                return null;
            }

            internal static MethodParametersEntry[] GetMethodParameters(Metadata m, int index)
            {
                if (m != null && m.methodParameters != null)
                    return m.methodParameters[index];

                return null;
            }

            internal static object GetMethodDefaultValue(Metadata m, int index)
            {
                if (m != null && m.annotations != null && m.annotations[3] != null)
                    return m.annotations[3][index];

                return null;
            }

            // note that unlike GetGenericFieldSignature, the index is simply the field index 
            internal static object[] GetFieldAnnotations(Metadata m, int index)
            {
                if (m != null && m.annotations != null && m.annotations[4] != null)
                    return (object[])m.annotations[4][index];

                return null;
            }

            internal static object[] GetConstantPool(Metadata m)
            {
                return m.constantPool;
            }

            internal static byte[] GetRawTypeAnnotations(Metadata m)
            {
                if (m != null)
                    return SerializeTypeAnnotations(in m.runtimeVisibleTypeAnnotations);

                return null;
            }

            internal static byte[] GetMethodRawTypeAnnotations(Metadata m, int index)
            {
                if (m != null && m.methodRuntimeVisibleTypeAnnotations != null)
                    return SerializeTypeAnnotations(in m.methodRuntimeVisibleTypeAnnotations[index]);

                return null;
            }

            internal static byte[] GetFieldRawTypeAnnotations(Metadata m, int index)
            {
                if (m != null && m.fieldRuntimeVisibleTypeAnnotations != null)
                    return SerializeTypeAnnotations(in m.fieldRuntimeVisibleTypeAnnotations[index]);

                return null;
            }

            static byte[] SerializeTypeAnnotations(ref readonly TypeAnnotationTable annotations)
            {
                if (annotations.Count == 0)
                    return null;

                var builder = new BlobBuilder();
                var encoder = new TypeAnnotationTableEncoder(builder);
                annotations.WriteTo(ref encoder);
                return builder.ToArray();
            }

            readonly string[][] genericMetaData;
            readonly object[][] annotations;
            readonly MethodParametersEntry[][] methodParameters;
            readonly TypeAnnotationTable runtimeVisibleTypeAnnotations;
            readonly TypeAnnotationTable[] fieldRuntimeVisibleTypeAnnotations;
            readonly TypeAnnotationTable[] methodRuntimeVisibleTypeAnnotations;
            readonly object[] constantPool;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericMetaData"></param>
            /// <param name="annotations"></param>
            /// <param name="methodParameters"></param>
            /// <param name="runtimeVisibleTypeAnnotations"></param>
            /// <param name="constantPool"></param>
            Metadata(string[][] genericMetaData, object[][] annotations, MethodParametersEntry[][] methodParameters, TypeAnnotationTable runtimeVisibleTypeAnnotations, TypeAnnotationTable[] fieldRuntimeVisibleTypeAnnotations, TypeAnnotationTable[] methodRuntimeVisibleTypeAnnotations, object[] constantPool)
            {
                this.genericMetaData = genericMetaData;
                this.annotations = annotations;
                this.methodParameters = methodParameters;
                this.runtimeVisibleTypeAnnotations = runtimeVisibleTypeAnnotations;
                this.fieldRuntimeVisibleTypeAnnotations = fieldRuntimeVisibleTypeAnnotations;
                this.methodRuntimeVisibleTypeAnnotations = methodRuntimeVisibleTypeAnnotations;
                this.constantPool = constantPool;
            }

        }

    }

}
