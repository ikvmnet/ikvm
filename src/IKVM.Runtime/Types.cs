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
using System.Threading;

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

        Type typeOfObject;
        Type typeOfValueType;
        Type typeOfEnum;
        Type typeOfType;
        Type typeOfString;
        Type typeOfException;
        Type typeOfArray;
        Type typeOfAttribute;
        Type typeOfDelegate;
        Type typeOfMulticastDelegate;
        Type typeOfRuntimeTypeHandle;

        Type typeOfIntPtr;
        Type typeOfVoid;
        Type typeOfBoolean;
        Type typeOfByte;
        Type typeOfSByte;
        Type typeOfChar;
        Type typeOfInt16;
        Type typeOfUInt16;
        Type typeOfInt32;
        Type typeOfUInt32;
        Type typeOfInt64;
        Type typeOfUInt64;
        Type typeOfSingle;
        Type typeOfDouble;

        Type typeOfIsVolatile;

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
        Type Import(System.Type type)
        {
            return context.Resolver.ResolveCoreType(type.FullName);
        }

        public Type Object => typeOfObject ??= Import(typeof(System.Object));

        public Type ValueType => typeOfValueType ??= Import(typeof(System.ValueType));

        public Type Enum => typeOfEnum ??= Import(typeof(System.Enum));

        public Type Type => typeOfType ??= Import(typeof(System.Type));

        public Type String => typeOfString ??= Import(typeof(System.String));

        public Type Exception => typeOfException ??= Import(typeof(System.Exception));

        public Type Array => typeOfArray ??= Import(typeof(System.Array));

        public Type Attribute => typeOfAttribute ??= Import(typeof(System.Attribute));

        public Type Delegate => typeOfDelegate ??= Import(typeof(System.Delegate));

        public Type MulticastDelegate => typeOfMulticastDelegate ??= Import(typeof(System.MulticastDelegate));

        public Type RuntimeTypeHandle => typeOfRuntimeTypeHandle ??= Import(typeof(System.RuntimeTypeHandle));

        public Type IntPtr => typeOfIntPtr ??= Import(typeof(IntPtr));

        public Type Void => typeOfVoid ??= Import(typeof(void));

        public Type Boolean => typeOfBoolean ??= Import(typeof(bool));

        public Type Byte => typeOfByte ??= Import(typeof(byte));

        public Type SByte => typeOfSByte ??= Import(typeof(sbyte));

        public Type Char => typeOfChar ??= Import(typeof(char));

        public Type Int16 => typeOfInt16 ??= Import(typeof(short));

        public Type UInt16 => typeOfUInt16 ??= Import(typeof(ushort));

        public Type Int32 => typeOfInt32 ??= Import(typeof(int));

        public Type UInt32 => typeOfUInt32 ??= Import(typeof(uint));

        public Type Int64 => typeOfInt64 ??= Import(typeof(long));

        public Type UInt64 => typeOfUInt64 ??= Import(typeof(ulong));

        public Type Single => typeOfSingle ??= Import(typeof(float));

        public Type Double => typeOfDouble ??= Import(typeof(double));

        public Type IsVolatile => typeOfIsVolatile ??= Import(typeof(System.Runtime.CompilerServices.IsVolatile));

    }

}
