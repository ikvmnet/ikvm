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
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    internal struct FieldMarshal
    {

        const UnmanagedType UnmanagedType_CustomMarshaler = (UnmanagedType)0x2c;
        const UnmanagedType NATIVE_TYPE_MAX = (UnmanagedType)0x50;

        public UnmanagedType UnmanagedType;
        public UnmanagedType? ArraySubType;
        public short? SizeParamIndex;
        public int? SizeConst;
        public VarEnum? SafeArraySubType;
        public Type SafeArrayUserDefinedSubType;
        public int? IidParameterIndex;
        public string MarshalType;
        public string MarshalCookie;
        public Type MarshalTypeRef;

        internal static bool ReadFieldMarshal(Module module, int token, out FieldMarshal fm)
        {
            fm = new FieldMarshal();

            if (module is ModuleBuilder builder)
                return builder.TryGetFieldMarshal(token, out var nativeType) && Decode(module, new ByteReader(nativeType, 0, nativeType.Length), out fm);

            if (module is not ModuleReader reader || (token & 0xFFFFFF) == 0)
                return false;

            var descriptor = (token >> 24) switch
            {
                FieldTable.Index => reader.Metadata.GetFieldDefinition(MetadataTokens.FieldDefinitionHandle(token & 0xFFFFFF)).GetMarshallingDescriptor(),
                ParamTable.Index => reader.Metadata.GetParameter(MetadataTokens.ParameterHandle(token & 0xFFFFFF)).GetMarshallingDescriptor(),
                _ => default,
            };

            return descriptor.IsNil == false && Decode(module, module.GetBlobReader(descriptor), out fm);
        }

        static bool Decode(Module module, ByteReader blob, out FieldMarshal fm)
        {
            fm = new FieldMarshal();

            fm.UnmanagedType = (UnmanagedType)blob.ReadCompressedUInt();
            switch (fm.UnmanagedType)
            {
                case UnmanagedType.LPArray:
                    fm.ArraySubType = (UnmanagedType)blob.ReadCompressedUInt();
                    if (fm.ArraySubType == NATIVE_TYPE_MAX)
                        fm.ArraySubType = null;

                    if (blob.Length != 0)
                    {
                        fm.SizeParamIndex = (short)blob.ReadCompressedUInt();
                        if (blob.Length != 0)
                        {
                            fm.SizeConst = blob.ReadCompressedUInt();
                            if (blob.Length != 0 && blob.ReadCompressedUInt() == 0)
                                fm.SizeParamIndex = null;
                        }
                    }
                    break;
                case UnmanagedType.SafeArray:
                    if (blob.Length != 0)
                    {
                        fm.SafeArraySubType = (VarEnum)blob.ReadCompressedUInt();
                        if (blob.Length != 0)
                            fm.SafeArrayUserDefinedSubType = ReadType(module, blob);
                    }
                    break;
                case UnmanagedType.ByValArray:
                    fm.SizeConst = blob.ReadCompressedUInt();
                    if (blob.Length != 0)
                        fm.ArraySubType = (UnmanagedType)blob.ReadCompressedUInt();
                    break;
                case UnmanagedType.ByValTStr:
                    fm.SizeConst = blob.ReadCompressedUInt();
                    break;
                case UnmanagedType.Interface:
                case UnmanagedType.IDispatch:
                case UnmanagedType.IUnknown:
                    if (blob.Length != 0)
                        fm.IidParameterIndex = blob.ReadCompressedUInt();
                    break;
                case UnmanagedType_CustomMarshaler:
                    {
                        blob.ReadCompressedUInt();
                        blob.ReadCompressedUInt();
                        fm.MarshalType = ReadString(blob);
                        fm.MarshalCookie = ReadString(blob);

                        var parser = TypeNameParser.Parse(fm.MarshalType, false);
                        if (!parser.Error)
                            fm.MarshalTypeRef = parser.GetType(module.Universe, module, false, fm.MarshalType, false, false);
                        break;
                    }
            }

            return true;
        }

        internal static void SetMarshalAsAttribute(ModuleBuilder module, int token, CustomAttributeBuilder attribute)
        {
            attribute = attribute.DecodeBlob(module.Assembly);
            module.AddFieldMarshal(token, WriteMarshallingDescriptor(module, attribute));
        }

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
                                bb.WriteCompressedInteger(1); // flag that says that SizeParamIndex was specified
                            }
                        }
                        else if (sizeConst != null)
                        {
                            bb.WriteCompressedInteger(0); // SizeParamIndex
                            bb.WriteCompressedInteger(sizeConst.Value);
                            bb.WriteCompressedInteger(0); // flag that says that SizeParamIndex was not specified
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

        static Type ReadType(Module module, ByteReader br)
        {
            var str = ReadString(br);
            if (str == "")
                return null;

            return module.Assembly.GetType(str) ?? module.Universe.GetType(str, true);
        }

        static void WriteType(Module module, BlobBuilder bb, Type type)
        {
            WriteString(bb, type.Assembly == module.Assembly ? type.FullName : type.AssemblyQualifiedName);
        }

        static string ReadString(ByteReader br)
        {
            return Encoding.UTF8.GetString(br.ReadBytes(br.ReadCompressedUInt()));
        }

        static void WriteString(BlobBuilder bb, string str)
        {
            var buf = Encoding.UTF8.GetBytes(str);
            bb.WriteCompressedInteger(buf.Length);
            bb.WriteBytes(buf);
        }

    }

}
