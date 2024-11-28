/*
  Copyright (C) 2009 Jeroen Frijters

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

using IKVM.CoreLib.Symbols;

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides various cached system types.
    /// </summary>
    class Types
    {

        readonly RuntimeContext context;

        TypeSymbol typeOfObject;
        TypeSymbol typeOfValueType;
        TypeSymbol typeOfEnum;
        TypeSymbol typeOfType;
        TypeSymbol typeOfString;
        TypeSymbol typeOfException;
        TypeSymbol typeOfArray;
        TypeSymbol typeOfAttribute;
        TypeSymbol typeOfDelegate;
        TypeSymbol typeOfMulticastDelegate;
        TypeSymbol typeOfRuntimeTypeHandle;

        TypeSymbol typeOfIntPtr;
        TypeSymbol typeOfVoid;
        TypeSymbol typeOfBoolean;
        TypeSymbol typeOfByte;
        TypeSymbol typeOfSByte;
        TypeSymbol typeOfChar;
        TypeSymbol typeOfInt16;
        TypeSymbol typeOfUInt16;
        TypeSymbol typeOfInt32;
        TypeSymbol typeOfUInt32;
        TypeSymbol typeOfInt64;
        TypeSymbol typeOfUInt64;
        TypeSymbol typeOfSingle;
        TypeSymbol typeOfDouble;

        TypeSymbol typeOfIsVolatile;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Types(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Imports the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeSymbol Import(System.Type type)
        {
            return context.Resolver.ResolveCoreType(type.FullName);
        }

        public TypeSymbol Object => typeOfObject ??= Import(typeof(System.Object));

        public TypeSymbol ValueType => typeOfValueType ??= Import(typeof(System.ValueType));

        public TypeSymbol Enum => typeOfEnum ??= Import(typeof(System.Enum));

        public TypeSymbol Type => typeOfType ??= Import(typeof(System.Type));

        public TypeSymbol String => typeOfString ??= Import(typeof(System.String));

        public TypeSymbol Exception => typeOfException ??= Import(typeof(System.Exception));

        public TypeSymbol Array => typeOfArray ??= Import(typeof(System.Array));

        public TypeSymbol Attribute => typeOfAttribute ??= Import(typeof(System.Attribute));

        public TypeSymbol Delegate => typeOfDelegate ??= Import(typeof(System.Delegate));

        public TypeSymbol MulticastDelegate => typeOfMulticastDelegate ??= Import(typeof(System.MulticastDelegate));

        public TypeSymbol RuntimeTypeHandle => typeOfRuntimeTypeHandle ??= Import(typeof(System.RuntimeTypeHandle));

        public TypeSymbol IntPtr => typeOfIntPtr ??= Import(typeof(IntPtr));

        public TypeSymbol Void => typeOfVoid ??= Import(typeof(void));

        public TypeSymbol Boolean => typeOfBoolean ??= Import(typeof(bool));

        public TypeSymbol Byte => typeOfByte ??= Import(typeof(byte));

        public TypeSymbol SByte => typeOfSByte ??= Import(typeof(sbyte));

        public TypeSymbol Char => typeOfChar ??= Import(typeof(char));

        public TypeSymbol Int16 => typeOfInt16 ??= Import(typeof(short));

        public TypeSymbol UInt16 => typeOfUInt16 ??= Import(typeof(ushort));

        public TypeSymbol Int32 => typeOfInt32 ??= Import(typeof(int));

        public TypeSymbol UInt32 => typeOfUInt32 ??= Import(typeof(uint));

        public TypeSymbol Int64 => typeOfInt64 ??= Import(typeof(long));

        public TypeSymbol UInt64 => typeOfUInt64 ??= Import(typeof(ulong));

        public TypeSymbol Single => typeOfSingle ??= Import(typeof(float));

        public TypeSymbol Double => typeOfDouble ??= Import(typeof(double));

        public TypeSymbol IsVolatile => typeOfIsVolatile ??= Import(typeof(System.Runtime.CompilerServices.IsVolatile));

    }

}
