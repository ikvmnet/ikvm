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

        ITypeSymbol typeOfObject;
        ITypeSymbol typeOfValueType;
        ITypeSymbol typeOfEnum;
        ITypeSymbol typeOfType;
        ITypeSymbol typeOfString;
        ITypeSymbol typeOfException;
        ITypeSymbol typeOfArray;
        ITypeSymbol typeOfAttribute;
        ITypeSymbol typeOfDelegate;
        ITypeSymbol typeOfMulticastDelegate;
        ITypeSymbol typeOfRuntimeTypeHandle;

        ITypeSymbol typeOfIntPtr;
        ITypeSymbol typeOfVoid;
        ITypeSymbol typeOfBoolean;
        ITypeSymbol typeOfByte;
        ITypeSymbol typeOfSByte;
        ITypeSymbol typeOfChar;
        ITypeSymbol typeOfInt16;
        ITypeSymbol typeOfUInt16;
        ITypeSymbol typeOfInt32;
        ITypeSymbol typeOfUInt32;
        ITypeSymbol typeOfInt64;
        ITypeSymbol typeOfUInt64;
        ITypeSymbol typeOfSingle;
        ITypeSymbol typeOfDouble;

        ITypeSymbol typeOfIsVolatile;

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
        ITypeSymbol Import(System.Type type)
        {
            return context.Resolver.ResolveCoreType(type.FullName);
        }

        public ITypeSymbol Object => typeOfObject ??= Import(typeof(System.Object));

        public ITypeSymbol ValueType => typeOfValueType ??= Import(typeof(System.ValueType));

        public ITypeSymbol Enum => typeOfEnum ??= Import(typeof(System.Enum));

        public ITypeSymbol Type => typeOfType ??= Import(typeof(System.Type));

        public ITypeSymbol String => typeOfString ??= Import(typeof(System.String));

        public ITypeSymbol Exception => typeOfException ??= Import(typeof(System.Exception));

        public ITypeSymbol Array => typeOfArray ??= Import(typeof(System.Array));

        public ITypeSymbol Attribute => typeOfAttribute ??= Import(typeof(System.Attribute));

        public ITypeSymbol Delegate => typeOfDelegate ??= Import(typeof(System.Delegate));

        public ITypeSymbol MulticastDelegate => typeOfMulticastDelegate ??= Import(typeof(System.MulticastDelegate));

        public ITypeSymbol RuntimeTypeHandle => typeOfRuntimeTypeHandle ??= Import(typeof(System.RuntimeTypeHandle));

        public ITypeSymbol IntPtr => typeOfIntPtr ??= Import(typeof(IntPtr));

        public ITypeSymbol Void => typeOfVoid ??= Import(typeof(void));

        public ITypeSymbol Boolean => typeOfBoolean ??= Import(typeof(bool));

        public ITypeSymbol Byte => typeOfByte ??= Import(typeof(byte));

        public ITypeSymbol SByte => typeOfSByte ??= Import(typeof(sbyte));

        public ITypeSymbol Char => typeOfChar ??= Import(typeof(char));

        public ITypeSymbol Int16 => typeOfInt16 ??= Import(typeof(short));

        public ITypeSymbol UInt16 => typeOfUInt16 ??= Import(typeof(ushort));

        public ITypeSymbol Int32 => typeOfInt32 ??= Import(typeof(int));

        public ITypeSymbol UInt32 => typeOfUInt32 ??= Import(typeof(uint));

        public ITypeSymbol Int64 => typeOfInt64 ??= Import(typeof(long));

        public ITypeSymbol UInt64 => typeOfUInt64 ??= Import(typeof(ulong));

        public ITypeSymbol Single => typeOfSingle ??= Import(typeof(float));

        public ITypeSymbol Double => typeOfDouble ??= Import(typeof(double));

        public ITypeSymbol IsVolatile => typeOfIsVolatile ??= Import(typeof(System.Runtime.CompilerServices.IsVolatile));

    }

}
