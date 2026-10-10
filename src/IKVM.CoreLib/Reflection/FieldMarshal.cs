/*
  Copyright (C) 2008-2012 Jeroen Frijters

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

using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    /// <summary>
    /// Describes the marshalling information of a field or parameter, as recorded in the FieldMarshal table. This is
    /// the decoded form of a marshalling descriptor, with the same members as
    /// <see cref="System.Runtime.InteropServices.MarshalAsAttribute"/>.
    /// </summary>
    struct FieldMarshal
    {

        /// <summary>
        /// The NATIVE_TYPE_CUSTOMMARSHALER value, which identifies a descriptor for a custom marshaler.
        /// </summary>
        const UnmanagedType UnmanagedType_CustomMarshaler = (UnmanagedType)0x2c;

        /// <summary>
        /// The NATIVE_TYPE_MAX value, which a descriptor uses to say that an array's element type is not specified.
        /// </summary>
        const UnmanagedType NATIVE_TYPE_MAX = (UnmanagedType)0x50;

        /// <summary>
        /// The unmanaged type the field or parameter is marshalled as.
        /// </summary>
        public UnmanagedType UnmanagedType;

        /// <summary>
        /// The unmanaged type of the elements of an <c>LPArray</c> or <c>ByValArray</c>, or <see langword="null"/> if
        /// not specified.
        /// </summary>
        public UnmanagedType? ArraySubType;

        /// <summary>
        /// The zero-based index of the parameter that holds the number of elements of an <c>LPArray</c>, or
        /// <see langword="null"/> if not specified.
        /// </summary>
        public short? SizeParamIndex;

        /// <summary>
        /// The number of elements of a fixed-length array, or the number of characters of a fixed-length string, or
        /// <see langword="null"/> if not specified.
        /// </summary>
        public int? SizeConst;

        /// <summary>
        /// The element type of a <c>SafeArray</c>, or <see langword="null"/> if not specified.
        /// </summary>
        public VarEnum? SafeArraySubType;

        /// <summary>
        /// The user-defined element type of a <c>SafeArray</c>, or <see langword="null"/> if not specified.
        /// </summary>
        public Type SafeArrayUserDefinedSubType;

        /// <summary>
        /// The zero-based index of the parameter that holds the interface identifier of an interface pointer, or
        /// <see langword="null"/> if not specified.
        /// </summary>
        public int? IidParameterIndex;

        /// <summary>
        /// The name of the custom marshaler type, as recorded in the descriptor.
        /// </summary>
        public string MarshalType;

        /// <summary>
        /// The additional information passed to the custom marshaler.
        /// </summary>
        public string MarshalCookie;

        /// <summary>
        /// The custom marshaler type resolved from <see cref="MarshalType"/>, or <see langword="null"/> if the name
        /// could not be parsed or resolved.
        /// </summary>
        public Type MarshalTypeRef;

        /// <summary>
        /// Reads the marshalling information of the field or parameter identified by a metadata token. For a module
        /// being built, the descriptor recorded by the builder is decoded; for a module read from a file, the
        /// descriptor is read from the FieldDef or Param row.
        /// </summary>
        /// <param name="module">The module that defines the field or parameter.</param>
        /// <param name="token">The FieldDef or Param metadata token of the field or parameter.</param>
        /// <param name="fm">When this method returns, contains the marshalling information if there is any; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the field or parameter has marshalling information; otherwise, <see langword="false"/>.</returns>
        internal static bool ReadFieldMarshal(Module module, int token, out FieldMarshal fm)
        {
            fm = new FieldMarshal();

            if (module is ModuleBuilder builder)
            {
                if (builder.TryGetFieldMarshal(token, out var nativeType) == false)
                    return false;

                unsafe
                {
                    fixed (byte* p = nativeType)
                        return Decode(module, new BlobReader(p, nativeType.Length), out fm);
                }
            }

            if (module is not ModuleReader reader || (token & 0xFFFFFF) == 0)
                return false;

            var descriptor = (token >> 24) switch
            {
                (int)TableIndex.Field => reader.Metadata.GetFieldDefinition(MetadataTokens.FieldDefinitionHandle(token & 0xFFFFFF)).GetMarshallingDescriptor(),
                (int)TableIndex.Param => reader.Metadata.GetParameter(MetadataTokens.ParameterHandle(token & 0xFFFFFF)).GetMarshallingDescriptor(),
                _ => default,
            };

            return descriptor.IsNil == false && Decode(module, module.GetBlobReader(descriptor), out fm);
        }

        /// <summary>
        /// Decodes a marshalling descriptor blob.
        /// </summary>
        /// <param name="module">The module used to resolve type names in the descriptor.</param>
        /// <param name="blob">The reader positioned at the start of the descriptor.</param>
        /// <param name="fm">When this method returns, contains the decoded marshalling information.</param>
        /// <returns>Always <see langword="true"/>.</returns>
        static bool Decode(Module module, BlobReader blob, out FieldMarshal fm)
        {
            fm = new FieldMarshal
            {
                UnmanagedType = (UnmanagedType)blob.ReadCompressedInteger()
            };
            switch (fm.UnmanagedType)
            {
                case UnmanagedType.LPArray:
                    fm.ArraySubType = (UnmanagedType)blob.ReadCompressedInteger();
                    if (fm.ArraySubType == NATIVE_TYPE_MAX)
                        fm.ArraySubType = null;

                    if (blob.RemainingBytes != 0)
                    {
                        fm.SizeParamIndex = (short)blob.ReadCompressedInteger();
                        if (blob.RemainingBytes != 0)
                        {
                            fm.SizeConst = blob.ReadCompressedInteger();
                            if (blob.RemainingBytes != 0 && blob.ReadCompressedInteger() == 0)
                                fm.SizeParamIndex = null;
                        }
                    }
                    break;
                case UnmanagedType.SafeArray:
                    if (blob.RemainingBytes != 0)
                    {
                        fm.SafeArraySubType = (VarEnum)blob.ReadCompressedInteger();
                        if (blob.RemainingBytes != 0)
                            fm.SafeArrayUserDefinedSubType = ReadType(module, ref blob);
                    }
                    break;
                case UnmanagedType.ByValArray:
                    fm.SizeConst = blob.ReadCompressedInteger();
                    if (blob.RemainingBytes != 0)
                        fm.ArraySubType = (UnmanagedType)blob.ReadCompressedInteger();
                    break;
                case UnmanagedType.ByValTStr:
                    fm.SizeConst = blob.ReadCompressedInteger();
                    break;
                case UnmanagedType.Interface:
                case UnmanagedType.IDispatch:
                case UnmanagedType.IUnknown:
                    if (blob.RemainingBytes != 0)
                        fm.IidParameterIndex = blob.ReadCompressedInteger();
                    break;
                case UnmanagedType_CustomMarshaler:
                    {
                        blob.ReadCompressedInteger();
                        blob.ReadCompressedInteger();
                        fm.MarshalType = ReadString(ref blob);
                        fm.MarshalCookie = ReadString(ref blob);

                        var parser = TypeNameParser.Parse(fm.MarshalType, false);
                        if (!parser.Error)
                            fm.MarshalTypeRef = parser.GetType(module.Universe, module, false, fm.MarshalType, false, false);
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Encodes a <c>MarshalAsAttribute</c> as a marshalling descriptor and records it for the field or parameter
        /// identified by a metadata token, instead of emitting it as a custom attribute.
        /// </summary>
        /// <param name="module">The module being built that defines the field or parameter.</param>
        /// <param name="token">The metadata token of the field or parameter.</param>
        /// <param name="attribute">The <c>MarshalAsAttribute</c> to encode.</param>
        internal static void SetMarshalAsAttribute(ModuleBuilder module, int token, CustomAttributeBuilder attribute)
        {
            attribute = attribute.DecodeBlob(module.Assembly);
            module.AddFieldMarshal(token, WriteMarshallingDescriptor(module, attribute));
        }

        /// <summary>
        /// Encodes the constructor argument and named fields of a <c>MarshalAsAttribute</c> as a marshalling
        /// descriptor blob.
        /// </summary>
        /// <param name="module">The module being built, used to decide how to write type names.</param>
        /// <param name="attribute">The decoded <c>MarshalAsAttribute</c>.</param>
        /// <returns>The marshalling descriptor.</returns>
        static byte[] WriteMarshallingDescriptor(ModuleBuilder module, CustomAttributeBuilder attribute)
        {
            var val = attribute.GetConstructorArgument(0);
            var unmanagedType = val switch
            {
                short s => (UnmanagedType)s,
                int i => (UnmanagedType)i,
                _ => (UnmanagedType)val,
            };

            var bb = new BlobBuilder(5);
            bb.WriteCompressedInteger((int)unmanagedType);

            switch (unmanagedType)
            {
                case UnmanagedType.LPArray:
                    {
                        var arraySubType = attribute.GetFieldValue<UnmanagedType>("ArraySubType") ?? NATIVE_TYPE_MAX;
                        bb.WriteCompressedInteger((int)arraySubType);

                        var sizeParamIndex = attribute.GetFieldValue<short>("SizeParamIndex");
                        var sizeConst = attribute.GetFieldValue<int>("SizeConst");
                        if (sizeParamIndex != null)
                        {
                            bb.WriteCompressedInteger(sizeParamIndex.Value);
                            if (sizeConst != null)
                            {
                                bb.WriteCompressedInteger(sizeConst.Value);

                                // flag that says that SizeParamIndex was specified
                                bb.WriteCompressedInteger(1);
                            }
                        }
                        else if (sizeConst != null)
                        {
                            // SizeParamIndex
                            bb.WriteCompressedInteger(0);
                            bb.WriteCompressedInteger(sizeConst.Value);

                            // flag that says that SizeParamIndex was not specified
                            bb.WriteCompressedInteger(0);
                        }

                        break;
                    }
                case UnmanagedType.SafeArray:
                    {
                        var safeArraySubType = attribute.GetFieldValue<VarEnum>("SafeArraySubType");
                        if (safeArraySubType != null)
                        {
                            bb.WriteCompressedInteger((int)safeArraySubType);
                            var safeArrayUserDefinedSubType = (Type)attribute.GetFieldValue("SafeArrayUserDefinedSubType");
                            if (safeArrayUserDefinedSubType != null)
                                WriteType(module, bb, safeArrayUserDefinedSubType);
                        }

                        break;
                    }
                case UnmanagedType.ByValArray:
                    {
                        bb.WriteCompressedInteger(attribute.GetFieldValue<int>("SizeConst") ?? 1);
                        var arraySubType = attribute.GetFieldValue<UnmanagedType>("ArraySubType");
                        if (arraySubType != null)
                            bb.WriteCompressedInteger((int)arraySubType);

                        break;
                    }
                case UnmanagedType.ByValTStr:
                    bb.WriteCompressedInteger(attribute.GetFieldValue<int>("SizeConst").Value);
                    break;
                case UnmanagedType.Interface:
                case UnmanagedType.IDispatch:
                case UnmanagedType.IUnknown:
                    {
                        var iidParameterIndex = attribute.GetFieldValue<int>("IidParameterIndex");
                        if (iidParameterIndex != null)
                            bb.WriteCompressedInteger(iidParameterIndex.Value);

                        break;
                    }
                case UnmanagedType_CustomMarshaler:
                    {
                        bb.WriteCompressedInteger(0);
                        bb.WriteCompressedInteger(0);
                        var marshalType = (string)attribute.GetFieldValue("MarshalType");
                        if (marshalType != null)
                            WriteString(bb, marshalType);
                        else
                            WriteType(module, bb, (Type)attribute.GetFieldValue("MarshalTypeRef"));

                        WriteString(bb, (string)attribute.GetFieldValue("MarshalCookie") ?? "");
                        break;
                    }
            }

            return bb.ToArray();
        }

        /// <summary>
        /// Reads a type name from a marshalling descriptor and resolves it, first in the module's assembly and then in
        /// the universe.
        /// </summary>
        /// <param name="module">The module that contains the descriptor.</param>
        /// <param name="br">The reader positioned at the length-prefixed type name.</param>
        /// <returns>The resolved type, or <see langword="null"/> if the name is empty.</returns>
        static Type ReadType(Module module, ref BlobReader br)
        {
            var str = ReadString(ref br);
            if (str == "")
                return null;

            return module.Assembly.GetType(str) ?? module.Universe.GetType(str, true);
        }

        /// <summary>
        /// Writes a type name to a marshalling descriptor: the full name for a type in the module's own assembly, and
        /// the assembly-qualified name otherwise.
        /// </summary>
        /// <param name="module">The module being built.</param>
        /// <param name="bb">The builder that receives the type name.</param>
        /// <param name="type">The type to write.</param>
        static void WriteType(Module module, BlobBuilder bb, Type type) =>
            WriteString(bb, type.Assembly == module.Assembly ? type.FullName : type.AssemblyQualifiedName);

        /// <summary>
        /// Reads a UTF-8 string prefixed by its compressed length.
        /// </summary>
        /// <param name="br">The reader positioned at the string.</param>
        /// <returns>The string that was read.</returns>
        static string ReadString(ref BlobReader br) => Encoding.UTF8.GetString(br.ReadBytes(br.ReadCompressedInteger()));

        /// <summary>
        /// Writes a UTF-8 string prefixed by its compressed length.
        /// </summary>
        /// <param name="bb">The builder that receives the string.</param>
        /// <param name="str">The string to write.</param>
        static void WriteString(BlobBuilder bb, string str)
        {
            var buf = Encoding.UTF8.GetBytes(str);
            bb.WriteCompressedInteger(buf.Length);
            bb.WriteBytes(buf);
        }

    }

}
