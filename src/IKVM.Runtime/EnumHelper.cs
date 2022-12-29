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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{
    static class EnumHelper
    {
        internal static Type GetUnderlyingType(Type enumType)
        {
#if IMPORTER || EXPORTER
            return enumType.GetEnumUnderlyingType();
#else
            return Enum.GetUnderlyingType(enumType);
#endif
        }

#if IMPORTER
        internal static object Parse(Type type, string value)
        {
            object retval = null;
            foreach (string str in value.Split(','))
            {
                FieldInfo field = type.GetField(str.Trim(), BindingFlags.Public | BindingFlags.Static);
                if (field == null)
                {
                    throw new InvalidOperationException("Enum value '" + str + "' not found in " + type.FullName);
                }
                if (retval == null)
                {
                    retval = field.GetRawConstantValue();
                }
                else
                {
                    retval = OrBoxedIntegrals(retval, field.GetRawConstantValue());
                }
            }
            return retval;
        }
#endif

        // note that we only support the integer types that C# supports
        // (the CLI also supports bool, char, IntPtr & UIntPtr)
        internal static object OrBoxedIntegrals(object v1, object v2)
        {
            Debug.Assert(v1.GetType() == v2.GetType());
            if (v1 is ulong)
            {
                ulong l1 = (ulong)v1;
                ulong l2 = (ulong)v2;
                return l1 | l2;
            }
            else
            {
                long v = ((IConvertible)v1).ToInt64(null) | ((IConvertible)v2).ToInt64(null);
                switch (Type.GetTypeCode(JVM.Import(v1.GetType())))
                {
                    case TypeCode.SByte:
                        return (sbyte)v;
                    case TypeCode.Byte:
                        return (byte)v;
                    case TypeCode.Int16:
                        return (short)v;
                    case TypeCode.UInt16:
                        return (ushort)v;
                    case TypeCode.Int32:
                        return (int)v;
                    case TypeCode.UInt32:
                        return (uint)v;
                    case TypeCode.Int64:
                        return (long)v;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        // this method can be used to convert an enum value or its underlying value to a Java primitive
        internal static object GetPrimitiveValue(Type underlyingType, object obj)
        {
            // Note that this method doesn't trust that obj is of the correct type,
            // because it turns out there exist assemblies (e.g. gtk-sharp.dll) that
            // have incorrectly typed enum constant values (e.g. int32 instead of uint32).
            long value;
            if (obj is ulong || (obj is Enum && underlyingType == Types.UInt64))
            {
                value = unchecked((long)((IConvertible)obj).ToUInt64(null));
            }
            else
            {
                value = ((IConvertible)obj).ToInt64(null);
            }
            if (underlyingType == Types.SByte || underlyingType == Types.Byte)
            {
                return unchecked((byte)value);
            }
            else if (underlyingType == Types.Int16 || underlyingType == Types.UInt16)
            {
                return unchecked((short)value);
            }
            else if (underlyingType == Types.Int32 || underlyingType == Types.UInt32)
            {
                return unchecked((int)value);
            }
            else if (underlyingType == Types.Int64 || underlyingType == Types.UInt64)
            {
                return value;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

}
