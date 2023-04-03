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

using IKVM.Internal;

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

    static class ByteCodeHelperMethods
    {

        internal static readonly MethodInfo multianewarray;
        internal static readonly MethodInfo multianewarray_ghost;
        internal static readonly MethodInfo anewarray_ghost;
        internal static readonly MethodInfo f2i;
        internal static readonly MethodInfo d2i;
        internal static readonly MethodInfo f2l;
        internal static readonly MethodInfo d2l;
        internal static readonly MethodInfo arraycopy_fast;
        internal static readonly MethodInfo arraycopy_primitive_8;
        internal static readonly MethodInfo arraycopy_primitive_4;
        internal static readonly MethodInfo arraycopy_primitive_2;
        internal static readonly MethodInfo arraycopy_primitive_1;
        internal static readonly MethodInfo arraycopy;
        internal static readonly MethodInfo DynamicCast;
        internal static readonly MethodInfo DynamicAaload;
        internal static readonly MethodInfo DynamicAastore;
        internal static readonly MethodInfo DynamicClassLiteral;
        internal static readonly MethodInfo DynamicMultianewarray;
        internal static readonly MethodInfo DynamicNewarray;
        internal static readonly MethodInfo DynamicNewCheckOnly;
        internal static readonly MethodInfo DynamicCreateDelegate;
        internal static readonly MethodInfo DynamicLoadMethodType;
        internal static readonly MethodInfo DynamicLoadMethodHandle;
        internal static readonly MethodInfo DynamicBinderMemberLookup;
        internal static readonly MethodInfo DynamicMapException;
        internal static readonly MethodInfo DynamicCallerID;
        internal static readonly MethodInfo DynamicLinkIndyCallSite;
        internal static readonly MethodInfo DynamicEraseInvokeExact;
        internal static readonly MethodInfo VerboseCastFailure;
        internal static readonly MethodInfo SkipFinalizer;
        internal static readonly MethodInfo DynamicInstanceOf;
        internal static readonly MethodInfo VolatileReadLong;
        internal static readonly MethodInfo VolatileReadDouble;
        internal static readonly MethodInfo VolatileWriteLong;
        internal static readonly MethodInfo VolatileWriteDouble;
        internal static readonly MethodInfo CompareAndSwapObject;
        internal static readonly MethodInfo CompareAndSwapInt;
        internal static readonly MethodInfo CompareAndSwapLong;
        internal static readonly MethodInfo CompareAndSwapDouble;
        internal static readonly MethodInfo mapException;
        internal static readonly MethodInfo GetDelegateForInvokeExact;
        internal static readonly MethodInfo GetDelegateForInvoke;
        internal static readonly MethodInfo GetDelegateForInvokeBasic;
        internal static readonly MethodInfo LoadMethodType;
        internal static readonly MethodInfo LinkIndyCallSite;

        static ByteCodeHelperMethods()
        {
#if IMPORTER
		    var typeofByteCodeHelper = StaticCompiler.GetRuntimeType("IKVM.Runtime.ByteCodeHelper");
#else
            var typeofByteCodeHelper = typeof(IKVM.Runtime.ByteCodeHelper);
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
            SkipFinalizer = GetHelper(typeofByteCodeHelper, "SkipFinalizer");
            DynamicInstanceOf = GetHelper(typeofByteCodeHelper, "DynamicInstanceOf");
            VolatileReadLong = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { Types.Int64.MakeByRefType() });
            VolatileWriteLong = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { Types.Int64.MakeByRefType(), Types.Int64 });
            VolatileReadDouble = GetHelper(typeofByteCodeHelper, "VolatileRead", new Type[] { Types.Double.MakeByRefType() });
            VolatileWriteDouble = GetHelper(typeofByteCodeHelper, "VolatileWrite", new Type[] { Types.Double.MakeByRefType(), Types.Double });
            CompareAndSwapObject = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { Types.Object.MakeByRefType(), Types.Object, Types.Object });
            CompareAndSwapInt = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { Types.Int32.MakeByRefType(), Types.Int32, Types.Int32 });
            CompareAndSwapLong = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { Types.Int64.MakeByRefType(), Types.Int64, Types.Int64 });
            CompareAndSwapDouble = GetHelper(typeofByteCodeHelper, "CompareAndSwap", new[] { Types.Double.MakeByRefType(), Types.Double, Types.Double });
            mapException = GetHelper(typeofByteCodeHelper, "MapException");
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
#if IMPORTER
		    if (mi == null)
			    throw new FatalCompilerErrorException(Message.RuntimeMethodMissing, method);
#endif
            return mi;
        }

    }

#endif

}
