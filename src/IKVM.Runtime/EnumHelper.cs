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

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    static class EnumHelper
    {

        internal static object Parse(RuntimeContext context, ITypeSymbol type, string value)
        {
            object retval = null;

            foreach (string str in value.Split(','))
            {
                var field = type.GetField(str.Trim(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
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
                    retval = OrBoxedIntegrals(context, retval, field.GetRawConstantValue());
                }
            }

            return retval;
        }

        // note that we only support the integer types that C# supports
        // (the CLI also supports bool, char, IntPtr & UIntPtr)
        internal static object OrBoxedIntegrals(RuntimeContext context, object v1, object v2)
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
                switch (context.Resolver.ResolveCoreType(v1.GetType().FullName).TypeCode)
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
        internal static object GetPrimitiveValue(RuntimeContext context, ITypeSymbol underlyingType, object obj)
        {
            // Note that this method doesn't trust that obj is of the correct type,
            // because it turns out there exist assemblies (e.g. gtk-sharp.dll) that
            // have incorrectly typed enum constant values (e.g. int32 instead of uint32).
            long value;
            if (obj is ulong || (obj is Enum && underlyingType == context.Types.UInt64))
            {
                value = unchecked((long)((IConvertible)obj).ToUInt64(null));
            }
            else
            {
                value = ((IConvertible)obj).ToInt64(null);
            }
            if (underlyingType == context.Types.SByte || underlyingType == context.Types.Byte)
            {
                return unchecked((byte)value);
            }
            else if (underlyingType == context.Types.Int16 || underlyingType == context.Types.UInt16)
            {
                return unchecked((short)value);
            }
            else if (underlyingType == context.Types.Int32 || underlyingType == context.Types.UInt32)
            {
                return unchecked((int)value);
            }
            else if (underlyingType == context.Types.Int64 || underlyingType == context.Types.UInt64)
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
