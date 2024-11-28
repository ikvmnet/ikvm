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
using System.Collections.Immutable;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

#if EXPORTER == FALSE

    class ByteCodeHelperMethods
    {

        internal readonly MethodSymbol multianewarray;
        internal readonly MethodSymbol multianewarray_ghost;
        internal readonly MethodSymbol anewarray_ghost;
        internal readonly MethodSymbol f2i;
        internal readonly MethodSymbol d2i;
        internal readonly MethodSymbol f2l;
        internal readonly MethodSymbol d2l;
        internal readonly MethodSymbol arraycopy_fast;
        internal readonly MethodSymbol arraycopy_primitive_8;
        internal readonly MethodSymbol arraycopy_primitive_4;
        internal readonly MethodSymbol arraycopy_primitive_2;
        internal readonly MethodSymbol arraycopy_primitive_1;
        internal readonly MethodSymbol arraycopy;
        internal readonly MethodSymbol DynamicCast;
        internal readonly MethodSymbol DynamicAaload;
        internal readonly MethodSymbol DynamicAastore;
        internal readonly MethodSymbol DynamicClassLiteral;
        internal readonly MethodSymbol DynamicMultianewarray;
        internal readonly MethodSymbol DynamicNewarray;
        internal readonly MethodSymbol DynamicNewCheckOnly;
        internal readonly MethodSymbol DynamicCreateDelegate;
        internal readonly MethodSymbol DynamicLoadMethodType;
        internal readonly MethodSymbol DynamicLoadMethodHandle;
        internal readonly MethodSymbol DynamicBinderMemberLookup;
        internal readonly MethodSymbol DynamicMapException;
        internal readonly MethodSymbol DynamicCallerID;
        internal readonly MethodSymbol DynamicLinkIndyCallSite;
        internal readonly MethodSymbol DynamicEraseInvokeExact;
        internal readonly MethodSymbol VerboseCastFailure;
        internal readonly MethodSymbol SkipFinalizer;
        internal readonly MethodSymbol SkipFinalizerOf;
        internal readonly MethodSymbol DynamicInstanceOf;
        internal readonly MethodSymbol VolatileReadBoolean;
        internal readonly MethodSymbol VolatileReadByte;
        internal readonly MethodSymbol VolatileReadChar;
        internal readonly MethodSymbol VolatileReadShort;
        internal readonly MethodSymbol VolatileReadInt;
        internal readonly MethodSymbol VolatileReadLong;
        internal readonly MethodSymbol VolatileReadFloat;
        internal readonly MethodSymbol VolatileReadDouble;
        internal readonly MethodSymbol VolatileWriteBoolean;
        internal readonly MethodSymbol VolatileWriteByte;
        internal readonly MethodSymbol VolatileWriteChar;
        internal readonly MethodSymbol VolatileWriteShort;
        internal readonly MethodSymbol VolatileWriteInt;
        internal readonly MethodSymbol VolatileWriteLong;
        internal readonly MethodSymbol VolatileWriteFloat;
        internal readonly MethodSymbol VolatileWriteDouble;
        internal readonly MethodSymbol CompareAndSwapObject;
        internal readonly MethodSymbol CompareAndSwapInt;
        internal readonly MethodSymbol CompareAndSwapLong;
        internal readonly MethodSymbol CompareAndSwapDouble;
        internal readonly MethodSymbol MapException;
        internal readonly MethodSymbol GetDelegateForInvokeExact;
        internal readonly MethodSymbol GetDelegateForInvoke;
        internal readonly MethodSymbol GetDelegateForInvokeBasic;
        internal readonly MethodSymbol LoadMethodType;
        internal readonly MethodSymbol LinkIndyCallSite;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ByteCodeHelperMethods(RuntimeContext context)
        {
            var typeofByteCodeHelper = context.Resolver.ResolveRuntimeType("IKVM.Runtime.ByteCodeHelper");
            multianewarray = GetHelper(typeofByteCodeHelper, "multianewarray");
            multianewarray_ghost = GetHelper(typeofByteCodeHelper, "multianewarray_ghost");
            anewarray_ghost = GetHelper(typeofByteCodeHelper, "anewarray_ghost");
            f2i = GetHelper(typeofByteCodeHelper, "f2i");
            d2i = GetHelper(typeofByteCodeHelper, "d2i");
            f2l = GetHelper(typeofByteCodeHelper, "f2l");
            d2l = GetHelper(typeofByteCodeHelper, "d2l");
            arraycopy_fast = GetHelper(typeofByteCodeHelper, "arraycopy_fast");
            arraycopy_primitive_8 = GetHelper(typeofByteCodeHelper, "arraycopy_primitive_8");
            arraycopy_primitive_4 = GetHelper(typeofByteCodeHelper, "arraycopy_primitive_4");
            arraycopy_primitive_2 = GetHelper(typeofByteCodeHelper, "arraycopy_primitive_2");
            arraycopy_primitive_1 = GetHelper(typeofByteCodeHelper, "arraycopy_primitive_1");
            arraycopy = GetHelper(typeofByteCodeHelper, "arraycopy");
            DynamicCast = GetHelper(typeofByteCodeHelper, "DynamicCast");
            DynamicAaload = GetHelper(typeofByteCodeHelper, "DynamicAaload");
            DynamicAastore = GetHelper(typeofByteCodeHelper, "DynamicAastore");
            DynamicClassLiteral = GetHelper(typeofByteCodeHelper, "DynamicClassLiteral");
            DynamicMultianewarray = GetHelper(typeofByteCodeHelper, "DynamicMultianewarray");
            DynamicNewarray = GetHelper(typeofByteCodeHelper, "DynamicNewarray");
            DynamicNewCheckOnly = GetHelper(typeofByteCodeHelper, "DynamicNewCheckOnly");
            DynamicCreateDelegate = GetHelper(typeofByteCodeHelper, "DynamicCreateDelegate");
            DynamicLoadMethodType = GetHelper(typeofByteCodeHelper, "DynamicLoadMethodType");
            DynamicLoadMethodHandle = GetHelper(typeofByteCodeHelper, "DynamicLoadMethodHandle");
            DynamicBinderMemberLookup = GetHelper(typeofByteCodeHelper, "DynamicBinderMemberLookup");
            DynamicMapException = GetHelper(typeofByteCodeHelper, "DynamicMapException");
            DynamicCallerID = GetHelper(typeofByteCodeHelper, "DynamicCallerID");
            DynamicLinkIndyCallSite = GetHelper(typeofByteCodeHelper, "DynamicLinkIndyCallSite");
            DynamicEraseInvokeExact = GetHelper(typeofByteCodeHelper, "DynamicEraseInvokeExact");
            VerboseCastFailure = GetHelper(typeofByteCodeHelper, "VerboseCastFailure");
            SkipFinalizer = GetHelper(typeofByteCodeHelper, "SkipFinalizer", []);
            SkipFinalizerOf = GetHelper(typeofByteCodeHelper, "SkipFinalizer", [context.Types.Object]);
            DynamicInstanceOf = GetHelper(typeofByteCodeHelper, "DynamicInstanceOf");
            VolatileReadBoolean = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Boolean.MakeByRefType()]);
            VolatileReadByte = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Byte.MakeByRefType()]);
            VolatileReadChar = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Char.MakeByRefType()]);
            VolatileReadShort = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Int16.MakeByRefType()]);
            VolatileReadInt = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Int32.MakeByRefType()]);
            VolatileReadLong = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Int64.MakeByRefType()]);
            VolatileReadFloat = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Single.MakeByRefType()]);
            VolatileReadDouble = GetHelper(typeofByteCodeHelper, "VolatileRead", [context.Types.Double.MakeByRefType()]);
            VolatileWriteBoolean = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Boolean.MakeByRefType(), context.Types.Boolean]);
            VolatileWriteByte = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Byte.MakeByRefType(), context.Types.Byte]);
            VolatileWriteChar = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Char.MakeByRefType(), context.Types.Char]);
            VolatileWriteShort = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Int16.MakeByRefType(), context.Types.Int16]);
            VolatileWriteInt = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Int32.MakeByRefType(), context.Types.Int32]);
            VolatileWriteLong = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Int64.MakeByRefType(), context.Types.Int64]);
            VolatileWriteFloat = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Single.MakeByRefType(), context.Types.Single]);
            VolatileWriteDouble = GetHelper(typeofByteCodeHelper, "VolatileWrite", [context.Types.Double.MakeByRefType(), context.Types.Double]);
            CompareAndSwapObject = GetHelper(typeofByteCodeHelper, "CompareAndSwap", [context.Types.Object.MakeByRefType(), context.Types.Object, context.Types.Object]);
            CompareAndSwapInt = GetHelper(typeofByteCodeHelper, "CompareAndSwap", [context.Types.Int32.MakeByRefType(), context.Types.Int32, context.Types.Int32]);
            CompareAndSwapLong = GetHelper(typeofByteCodeHelper, "CompareAndSwap", [context.Types.Int64.MakeByRefType(), context.Types.Int64, context.Types.Int64]);
            CompareAndSwapDouble = GetHelper(typeofByteCodeHelper, "CompareAndSwap", [context.Types.Double.MakeByRefType(), context.Types.Double, context.Types.Double]);
            MapException = GetHelper(typeofByteCodeHelper, "MapException");
            GetDelegateForInvokeExact = GetHelper(typeofByteCodeHelper, "GetDelegateForInvokeExact");
            GetDelegateForInvoke = GetHelper(typeofByteCodeHelper, "GetDelegateForInvoke");
            GetDelegateForInvokeBasic = GetHelper(typeofByteCodeHelper, "GetDelegateForInvokeBasic");
            LoadMethodType = GetHelper(typeofByteCodeHelper, "LoadMethodType");
            LinkIndyCallSite = GetHelper(typeofByteCodeHelper, "LinkIndyCallSite");
        }

        static MethodSymbol GetHelper(TypeSymbol type, string method)
        {
            return GetHelper(type, method, default);
        }

        static MethodSymbol GetHelper(TypeSymbol type, string method, ImmutableArray<TypeSymbol> parameters)
        {
            var mi = parameters.IsDefault ? type.GetMethod(method) : type.GetMethod(method, parameters);
            if (mi == null)
#if IMPORTER
			    throw new DiagnosticEventException(DiagnosticEvent.RuntimeMethodMissing(method));
#else
                throw new InternalException("Missing ByteCodeHelper method in runtime.");
#endif

            return mi;
        }

    }

#endif

}
