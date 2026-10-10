/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    sealed class CustomAttributeBuilder
    {

        readonly ConstructorInfo _con;
        readonly byte[] _blob;
        readonly object[] _constructorArgs;
        readonly PropertyInfo[] _namedProperties;
        readonly object[] _propertyValues;
        readonly FieldInfo[] _namedFields;
        readonly object[] _fieldValues;

        /// <summary>
        /// Initialiezes a new instance.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="blob"></param>
        internal CustomAttributeBuilder(ConstructorInfo con, byte[] blob)
        {
            _con = con;
            _blob = blob;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs) :
            this(con, constructorArgs, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues) :
            this(con, constructorArgs, null, null, namedFields, fieldValues)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues) :
            this(con, constructorArgs, namedProperties, propertyValues, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
        {
            _con = con;
            _constructorArgs = constructorArgs;
            _namedProperties = namedProperties;
            _propertyValues = propertyValues;
            _namedFields = namedFields;
            _fieldValues = fieldValues;
        }

        public static CustomAttributeBuilder __FromBlob(ConstructorInfo con, byte[] blob) => new CustomAttributeBuilder(con, blob);

        public static CustomAttributeTypedArgument __MakeTypedArgument(Type type, object value) => new CustomAttributeTypedArgument(type, value);

        sealed class BlobWriter
        {

            readonly Assembly _assembly;
            readonly CustomAttributeBuilder _cab;
            readonly BlobBuilder _bb;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assembly"></param>
            /// <param name="cab"></param>
            /// <param name="bb"></param>
            internal BlobWriter(Assembly assembly, CustomAttributeBuilder cab, BlobBuilder bb)
            {
                _assembly = assembly;
                _cab = cab;
                _bb = bb;
            }

            internal void WriteCustomAttributeBlob()
            {
                // prolog
                WriteUInt16(1);

                var parameterTypes = _cab._con.GetParameterTypes();
                for (var i = 0; i < parameterTypes.Length; i++)
                    WriteFixedArg(parameterTypes[i], _cab._constructorArgs[i]);

                WriteNamedArguments(false);
            }

            internal void WriteNamedArguments(bool forDeclSecurity)
            {
                // NumNamed
                var named = 0;
                if (_cab._namedFields != null)
                {
                    named += _cab._namedFields.Length;
                }
                if (_cab._namedProperties != null)
                {
                    named += _cab._namedProperties.Length;
                }
                if (forDeclSecurity)
                {
                    WritePackedLen(named);
                }
                else
                {
                    WriteUInt16((ushort)named);
                }
                if (_cab._namedFields != null)
                {
                    for (var i = 0; i < _cab._namedFields.Length; i++)
                    {
                        WriteNamedArg(0x53, _cab._namedFields[i].FieldType, _cab._namedFields[i].Name, _cab._fieldValues[i]);
                    }
                }
                if (_cab._namedProperties != null)
                {
                    for (var i = 0; i < _cab._namedProperties.Length; i++)
                    {
                        WriteNamedArg(0x54, _cab._namedProperties[i].PropertyType, _cab._namedProperties[i].Name, _cab._propertyValues[i]);
                    }
                }
            }

            void WriteNamedArg(byte fieldOrProperty, Type type, string name, object value)
            {
                WriteByte(fieldOrProperty);
                WriteFieldOrPropType(type);
                WriteString(name);
                WriteFixedArg(type, value);
            }

            void WriteByte(byte value) => _bb.WriteByte(value);

            void WriteUInt16(ushort value) => _bb.WriteUInt16(value);

            void WriteInt32(int value) => _bb.WriteInt32(value);

            void WriteFixedArg(Type type, object value)
            {
                var u = _assembly.Universe;
                if (type == u.System_String)
                {
                    WriteString((string)value);
                }
                else if (type == u.System_Boolean)
                {
                    WriteByte((bool)value ? (byte)1 : (byte)0);
                }
                else if (type == u.System_Char)
                {
                    WriteUInt16((char)value);
                }
                else if (type == u.System_SByte)
                {
                    WriteByte((byte)(sbyte)value);
                }
                else if (type == u.System_Byte)
                {
                    WriteByte((byte)value);
                }
                else if (type == u.System_Int16)
                {
                    WriteUInt16((ushort)(short)value);
                }
                else if (type == u.System_UInt16)
                {
                    WriteUInt16((ushort)value);
                }
                else if (type == u.System_Int32)
                {
                    WriteInt32((int)value);
                }
                else if (type == u.System_UInt32)
                {
                    WriteInt32((int)(uint)value);
                }
                else if (type == u.System_Int64)
                {
                    WriteInt64((long)value);
                }
                else if (type == u.System_UInt64)
                {
                    WriteInt64((long)(ulong)value);
                }
                else if (type == u.System_Single)
                {
                    WriteSingle((float)value);
                }
                else if (type == u.System_Double)
                {
                    WriteDouble((double)value);
                }
                else if (type == u.System_Type)
                {
                    WriteTypeName((Type)value);
                }
                else if (type == u.System_Object)
                {
                    if (value == null)
                    {
                        type = u.System_String;
                    }
                    else if (value is Type)
                    {
                        // value.GetType() would return a subclass of Type, but we don't want to deal with that
                        type = u.System_Type;
                    }
                    else if (value is CustomAttributeTypedArgument cta)
                    {
                        value = cta.Value;
                        type = cta.ArgumentType;
                    }
                    else
                    {
                        type = u.Import(value.GetType());
                    }
                    WriteFieldOrPropType(type);
                    WriteFixedArg(type, value);
                }
                else if (type.IsArray)
                {
                    if (value == null)
                    {
                        WriteInt32(-1);
                    }
                    else
                    {
                        Array array = (Array)value;
                        Type elemType = type.GetElementType();
                        WriteInt32(array.Length);
                        foreach (var val in array)
                        {
                            WriteFixedArg(elemType, val);
                        }
                    }
                }
                else if (type.IsEnum)
                {
                    WriteFixedArg(type.GetEnumUnderlyingTypeImpl(), value);
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            void WriteInt64(long value) => _bb.WriteInt64(value);

            void WriteSingle(float value) => _bb.WriteSingle(value);

            void WriteDouble(double value) => _bb.WriteDouble(value);

            void WriteTypeName(Type type)
            {
                string name = null;
                if (type != null)
                {
                    StringBuilder sb = new StringBuilder();
                    GetTypeName(sb, type, false);
                    name = sb.ToString();
                }
                WriteString(name);
            }

            void GetTypeName(StringBuilder sb, Type type, bool isTypeParam)
            {
                var v1 = !_assembly.ManifestModule.__IsMissing && _assembly.ManifestModule.MDStreamVersion < 0x20000;
                var includeAssemblyName = type.Assembly != _assembly && (!v1 || type.Assembly != type.Module.Universe.CoreLib);
                if (isTypeParam && includeAssemblyName)
                {
                    sb.Append('[');
                }
                GetTypeNameImpl(sb, type);
                if (includeAssemblyName)
                {
                    if (v1)
                    {
                        sb.Append(',');
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                    if (isTypeParam)
                    {
                        sb.Append(type.Assembly.FullName.Replace("]", "\\]")).Append(']');
                    }
                    else
                    {
                        sb.Append(type.Assembly.FullName);
                    }
                }
            }

            void GetTypeNameImpl(StringBuilder sb, Type type)
            {
                if (type.HasElementType)
                {
                    GetTypeNameImpl(sb, type.GetElementType());
                    sb.Append(((ElementHolderType)type).GetSuffix());
                }
                else if (type.IsConstructedGenericType)
                {
                    sb.Append(type.GetGenericTypeDefinition().FullName);
                    sb.Append('[');
                    var sep = "";
                    foreach (Type typeParam in type.GetGenericArguments())
                    {
                        sb.Append(sep);
                        GetTypeName(sb, typeParam, true);
                        sep = ",";
                    }
                    sb.Append(']');
                }
                else
                {
                    sb.Append(type.FullName);
                }
            }

            void WriteString(string val) => _bb.WriteSerializedString(val);

            void WritePackedLen(int len) => _bb.WriteCompressedInteger(len);

            void WriteFieldOrPropType(Type type)
            {
                var u = type.Module.Universe;
                if (type == u.System_Type)
                {
                    WriteByte(0x50);
                }
                else if (type == u.System_Object)
                {
                    WriteByte(0x51);
                }
                else if (type == u.System_Boolean)
                {
                    WriteByte(0x02);
                }
                else if (type == u.System_Char)
                {
                    WriteByte(0x03);
                }
                else if (type == u.System_SByte)
                {
                    WriteByte(0x04);
                }
                else if (type == u.System_Byte)
                {
                    WriteByte(0x05);
                }
                else if (type == u.System_Int16)
                {
                    WriteByte(0x06);
                }
                else if (type == u.System_UInt16)
                {
                    WriteByte(0x07);
                }
                else if (type == u.System_Int32)
                {
                    WriteByte(0x08);
                }
                else if (type == u.System_UInt32)
                {
                    WriteByte(0x09);
                }
                else if (type == u.System_Int64)
                {
                    WriteByte(0x0A);
                }
                else if (type == u.System_UInt64)
                {
                    WriteByte(0x0B);
                }
                else if (type == u.System_Single)
                {
                    WriteByte(0x0C);
                }
                else if (type == u.System_Double)
                {
                    WriteByte(0x0D);
                }
                else if (type == u.System_String)
                {
                    WriteByte(0x0E);
                }
                else if (type.IsArray)
                {
                    WriteByte(0x1D);
                    WriteFieldOrPropType(type.GetElementType());
                }
                else if (type.IsEnum)
                {
                    WriteByte(0x55);
                    WriteTypeName(type);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        internal ConstructorInfo Constructor => _con;

        internal BlobHandle WriteBlob(ModuleBuilder moduleBuilder)
        {
            if (_blob != null)
                return moduleBuilder.GetOrAddBlob(_blob);

            var bb = new BlobBuilder(100);
            new BlobWriter(moduleBuilder.Assembly, this, bb).WriteCustomAttributeBlob();
            return moduleBuilder.GetOrAddBlob(bb);
        }

        internal object GetConstructorArgument(int pos) => _constructorArgs[pos];

        internal T? GetFieldValue<T>(string name) where T : struct
        {
            var val = GetFieldValue(name);
            if (val is T t)
            {
                return t;
            }
            else if (val != null)
            {
                if (TypeUtil.IsEnum(typeof(T)))
                {
                    Debug.Assert(Enum.GetUnderlyingType(typeof(T)) == val.GetType());
                    return (T)Enum.ToObject(typeof(T), val);
                }
                else
                {
                    Debug.Assert(Enum.GetUnderlyingType(val.GetType()) == typeof(T));
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
            else
            {
                return null;
            }
        }

        internal object GetFieldValue(string name)
        {
            if (_namedFields != null)
            {
                for (var i = 0; i < _namedFields.Length; i++)
                {
                    if (_namedFields[i].Name == name)
                    {
                        return _fieldValues[i];
                    }
                }
            }
            return null;
        }

        internal CustomAttributeData ToData(Assembly asm)
        {
            if (_blob != null)
            {
                if (_constructorArgs != null)
                    return new CustomAttributeData(asm, _con, (int)_constructorArgs[0], _blob, -1);

                unsafe
                {
                    fixed (byte* p = _blob)
                        return new CustomAttributeData(asm, _con, new System.Reflection.Metadata.BlobReader(p, _blob.Length));
                }
            }
            else
            {
                var namedArgs = new List<CustomAttributeNamedArgument>();

                if (_namedProperties != null)
                    for (var i = 0; i < _namedProperties.Length; i++)
                        namedArgs.Add(new CustomAttributeNamedArgument(_namedProperties[i], RewrapValue(_namedProperties[i].PropertyType, _propertyValues[i])));

                if (_namedFields != null)
                    for (var i = 0; i < _namedFields.Length; i++)
                        namedArgs.Add(new CustomAttributeNamedArgument(_namedFields[i], RewrapValue(_namedFields[i].FieldType, _fieldValues[i])));

                var args = new List<CustomAttributeTypedArgument>(_constructorArgs.Length);
                var parameterTypes = Constructor.GetParameterTypes();
                for (var i = 0; i < _constructorArgs.Length; i++)
                    args.Add(RewrapValue(parameterTypes[i], _constructorArgs[i]));

                return new CustomAttributeData(asm.ManifestModule, _con, args, namedArgs);
            }
        }

        static CustomAttributeTypedArgument RewrapValue(Type type, object value)
        {
            if (value is Array array)
            {
                var arrayType = type.Module.Universe.Import(array.GetType());
                return RewrapArray(arrayType, array);
            }
            else if (value is CustomAttributeTypedArgument arg)
            {
                if (arg.Value is Array)
                    return RewrapArray(arg.ArgumentType, (Array)arg.Value);

                return arg;
            }
            else
            {
                return new CustomAttributeTypedArgument(type, value);
            }
        }

        static CustomAttributeTypedArgument RewrapArray(Type arrayType, Array array)
        {
            var elementType = arrayType.GetElementType();
            var newArray = new CustomAttributeTypedArgument[array.Length];
            for (var i = 0; i < newArray.Length; i++)
                newArray[i] = RewrapValue(elementType, array.GetValue(i));

            return new CustomAttributeTypedArgument(arrayType, newArray);
        }

        internal bool HasBlob => _blob != null;

        internal CustomAttributeBuilder DecodeBlob(Assembly asm)
        {
            if (_blob == null)
            {
                return this;
            }
            else
            {
                return ToData(asm).__ToBuilder();
            }
        }

        internal KnownCA KnownCA
        {
            get
            {
                var attributeType = _con.DeclaringType;
                if (attributeType.IsConstructedGenericType)
                    return KnownCA.Unknown; // a constructed generic type doesn't have a TypeName and we already know it's not a Known CA

                var typeName = attributeType.TypeName;
                switch (typeName.Namespace)
                {
                    case "System":
                        switch (typeName.Name)
                        {
                            case "SerializableAttribute":
                                return KnownCA.SerializableAttribute;
                            case "NonSerializedAttribute":
                                return KnownCA.NonSerializedAttribute;
                        }
                        break;
                    case "System.Runtime.CompilerServices":
                        switch (typeName.Name)
                        {
                            case "MethodImplAttribute":
                                return KnownCA.MethodImplAttribute;
                            case "SpecialNameAttribute":
                                return KnownCA.SpecialNameAttribute;
                        }
                        break;
                    case "System.Runtime.InteropServices":
                        switch (typeName.Name)
                        {
                            case "DllImportAttribute":
                                return KnownCA.DllImportAttribute;
                            case "ComImportAttribute":
                                return KnownCA.ComImportAttribute;
                            case "MarshalAsAttribute":
                                return KnownCA.MarshalAsAttribute;
                            case "PreserveSigAttribute":
                                return KnownCA.PreserveSigAttribute;
                            case "InAttribute":
                                return KnownCA.InAttribute;
                            case "OutAttribute":
                                return KnownCA.OutAttribute;
                            case "OptionalAttribute":
                                return KnownCA.OptionalAttribute;
                            case "StructLayoutAttribute":
                                return KnownCA.StructLayoutAttribute;
                            case "FieldOffsetAttribute":
                                return KnownCA.FieldOffsetAttribute;
                        }
                        break;
                }

                if (typeName.Matches("System.Security.SuppressUnmanagedCodeSecurityAttribute"))
                    return KnownCA.SuppressUnmanagedCodeSecurityAttribute;

                return KnownCA.Unknown;
            }
        }

    }

}
