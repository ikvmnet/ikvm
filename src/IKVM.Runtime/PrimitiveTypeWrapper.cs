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

using IKVM.Attributes;

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
    sealed class PrimitiveTypeWrapper : TypeWrapper
    {

        internal static readonly PrimitiveTypeWrapper BYTE = new PrimitiveTypeWrapper(Types.Byte, "B");
        internal static readonly PrimitiveTypeWrapper CHAR = new PrimitiveTypeWrapper(Types.Char, "C");
        internal static readonly PrimitiveTypeWrapper DOUBLE = new PrimitiveTypeWrapper(Types.Double, "D");
        internal static readonly PrimitiveTypeWrapper FLOAT = new PrimitiveTypeWrapper(Types.Single, "F");
        internal static readonly PrimitiveTypeWrapper INT = new PrimitiveTypeWrapper(Types.Int32, "I");
        internal static readonly PrimitiveTypeWrapper LONG = new PrimitiveTypeWrapper(Types.Int64, "J");
        internal static readonly PrimitiveTypeWrapper SHORT = new PrimitiveTypeWrapper(Types.Int16, "S");
        internal static readonly PrimitiveTypeWrapper BOOLEAN = new PrimitiveTypeWrapper(Types.Boolean, "Z");
        internal static readonly PrimitiveTypeWrapper VOID = new PrimitiveTypeWrapper(Types.Void, "V");

        readonly Type type;
        readonly string sigName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sigName"></param>
        private PrimitiveTypeWrapper(Type type, string sigName) :
            base(TypeFlags.None, Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null)
        {
            this.type = type;
            this.sigName = sigName;
        }

        internal override TypeWrapper BaseTypeWrapper => null;

        internal static bool IsPrimitiveType(Type type)
        {
            return type == BYTE.type
                || type == CHAR.type
                || type == DOUBLE.type
                || type == FLOAT.type
                || type == INT.type
                || type == LONG.type
                || type == SHORT.type
                || type == BOOLEAN.type
                || type == VOID.type;
        }

        internal override string SigName => sigName;

        internal override ClassLoaderWrapper GetClassLoader() => ClassLoaderWrapper.GetBootstrapClassLoader();

        internal override Type TypeAsTBD => type;

        public override string ToString() => "PrimitiveTypeWrapper[" + sigName + "]";

    }

}
