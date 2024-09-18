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

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Symbols.Reflection;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

#if EXPORTER == FALSE

    class ByteCodeHelperMethods
    {

        internal readonly MethodInfo multianewarray;
        internal readonly MethodInfo multianewarray_ghost;
        internal readonly MethodInfo anewarray_ghost;
        internal readonly MethodInfo f2i;
        internal readonly MethodInfo d2i;
        internal readonly MethodInfo f2l;
        internal readonly MethodInfo d2l;
        internal readonly MethodInfo arraycopy_fast;
        internal readonly MethodInfo arraycopy_primitive_8;
        internal readonly MethodInfo arraycopy_primitive_4;
        internal readonly MethodInfo arraycopy_primitive_2;
        internal readonly MethodInfo arraycopy_primitive_1;
        internal readonly MethodInfo arraycopy;
        internal readonly MethodInfo DynamicCast;
        internal readonly MethodInfo DynamicAaload;
        internal readonly MethodInfo DynamicAastore;
        internal readonly MethodInfo DynamicClassLiteral;
        internal readonly MethodInfo DynamicMultianewarray;
        internal readonly MethodInfo DynamicNewarray;
        internal readonly MethodInfo DynamicNewCheckOnly;
        internal readonly MethodInfo DynamicCreateDelegate;
        internal readonly MethodInfo DynamicLoadMethodType;
        internal readonly MethodInfo DynamicLoadMethodHandle;
        internal readonly MethodInfo DynamicBinderMemberLookup;
        internal readonly MethodInfo DynamicMapException;
        internal readonly MethodInfo DynamicCallerID;
        internal readonly MethodInfo DynamicLinkIndyCallSite;
        internal readonly MethodInfo DynamicEraseInvokeExact;
        internal readonly MethodInfo VerboseCastFailure;
        internal readonly MethodInfo SkipFinalizer;
        internal readonly MethodInfo SkipFinalizerOf;
        internal readonly MethodInfo DynamicInstanceOf;
        internal readonly MethodInfo VolatileReadBoolean;
        internal readonly MethodInfo VolatileReadByte;
        internal readonly MethodInfo VolatileReadChar;
        internal readonly MethodInfo VolatileReadShort;
        internal readonly MethodInfo VolatileReadInt;
        internal readonly MethodInfo VolatileReadLong;
        internal readonly MethodInfo VolatileReadFloat;
        internal readonly MethodInfo VolatileReadDouble;
        internal readonly MethodInfo VolatileWriteBoolean;
        internal readonly MethodInfo VolatileWriteByte;
        internal readonly MethodInfo VolatileWriteChar;
        internal readonly MethodInfo VolatileWriteShort;
        internal readonly MethodInfo VolatileWriteInt;
        internal readonly MethodInfo VolatileWriteLong;
        internal readonly MethodInfo VolatileWriteFloat;
        internal readonly MethodInfo VolatileWriteDouble;
        internal readonly MethodInfo CompareAndSwapObject;
        internal readonly MethodInfo CompareAndSwapInt;
        internal readonly MethodInfo CompareAndSwapLong;
        internal readonly MethodInfo CompareAndSwapDouble;
        internal readonly MethodInfo MapException;
        internal readonly MethodInfo GetDelegateForInvokeExact;
        internal readonly MethodInfo GetDelegateForInvoke;
        internal readonly MethodInfo GetDelegateForInvokeBasic;
        internal readonly MethodInfo LoadMethodType;
        internal readonly MethodInfo LinkIndyCallSite;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ByteCodeHelperMethods(RuntimeContext context)
        {
#if IMPORTER || EXPORTER
            var typeofByteCodeHelper = ((IkvmReflectionTypeSymbol)context.Resolver.ResolveRuntimeType("IKVM.Runtime.ByteCodeHelper")).ReflectionObject;
#else
            var typeofByteCodeHelper = ((ReflectionTypeSymbol)context.Resolver.ResolveRuntimeType("IKVM.Runtime.ByteCodeHelper")).ReflectionObject;
#endif
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
            SkipFinalizer = GetHelper(typeofByteCodeHelper, "SkipFinalizer", new Type[] { });
            SkipFinalizerOf = GetHelper(typeofByteCodeHelper, "SkipFinalizer", new Type[] { context.Types.Object });
            DynamicInstanceOf = GetHelper(typeofByteCodeHelper, "DynamicInstanceOf");
            VolatileReadBoolean = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Boolean.MakeByRefType() });
            VolatileReadByte = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Byte.MakeByRefType() });
            VolatileReadChar = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Char.MakeByRefType() });
            VolatileReadShort = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Int16.MakeByRefType() });
            VolatileReadInt = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Int32.MakeByRefType() });
            VolatileReadLong = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Int64.MakeByRefType() });
            VolatileReadFloat = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Single.MakeByRefType() });
            VolatileReadDouble = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { context.Types.Double.MakeByRefType() });
            VolatileWriteBoolean = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Boolean.MakeByRefType(), context.Types.Boolean });
            VolatileWriteByte = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Byte.MakeByRefType(), context.Types.Byte });
            VolatileWriteChar = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Char.MakeByRefType(), context.Types.Char });
            VolatileWriteShort = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Int16.MakeByRefType(), context.Types.Int16 });
            VolatileWriteInt = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Int32.MakeByRefType(), context.Types.Int32 });
            VolatileWriteLong = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Int64.MakeByRefType(), context.Types.Int64 });
            VolatileWriteFloat = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Single.MakeByRefType(), context.Types.Single });
            VolatileWriteDouble = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { context.Types.Double.MakeByRefType(), context.Types.Double });
            CompareAndSwapObject = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { context.Types.Object.MakeByRefType(), context.Types.Object, context.Types.Object });
            CompareAndSwapInt = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { context.Types.Int32.MakeByRefType(), context.Types.Int32, context.Types.Int32 });
            CompareAndSwapLong = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { context.Types.Int64.MakeByRefType(), context.Types.Int64, context.Types.Int64 });
            CompareAndSwapDouble = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { context.Types.Double.MakeByRefType(), context.Types.Double, context.Types.Double });
            MapException = GetHelper(typeofByteCodeHelper, "MapException");
            GetDelegateForInvokeExact = GetHelper(typeofByteCodeHelper, "GetDelegateForInvokeExact");
            GetDelegateForInvoke = GetHelper(typeofByteCodeHelper, "GetDelegateForInvoke");
            GetDelegateForInvokeBasic = GetHelper(typeofByteCodeHelper, "GetDelegateForInvokeBasic");
            LoadMethodType = GetHelper(typeofByteCodeHelper, "LoadMethodType");
            LinkIndyCallSite = GetHelper(typeofByteCodeHelper, "LinkIndyCallSite");
        }

        static MethodInfo GetHelper(Type type, string method)
        {
            return GetHelper(type, method, null);
        }

        static MethodInfo GetHelper(Type type, string method, Type[] parameters)
        {
            var mi = parameters == null ? type.GetMethod(method) : type.GetMethod(method, parameters);
            if (mi == null)
#if IMPORTER
			    throw new FatalCompilerErrorException(DiagnosticEvent.RuntimeMethodMissing(method));
#else
                throw new InternalException("Missing ByteCodeHelper method in runtime.");
#endif

            return mi;
        }

    }

#endif

}
