/*
 * Copyright (c) 2008, 2011, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

/*
 * Extensively modified for IKVM.NET by Jeroen Frijters
 * Copyright (C) 2011 Jeroen Frijters
 */

package java.lang.invoke;

import sun.invoke.util.VerifyType;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import sun.invoke.empty.Empty;
import sun.invoke.util.ValueConversions;
import sun.invoke.util.Wrapper;
import sun.misc.Unsafe;
import static java.lang.invoke.MethodHandleStatics.*;
import static java.lang.invoke.MethodHandles.Lookup.IMPL_LOOKUP;

/**
 * Trusted implementation code for MethodHandle.
 * @author jrose
 */
/*non-public*/ abstract class MethodHandleImpl {
    /// Factory methods to create method handles:

    private static final MemberName.Factory LOOKUP = MemberName.Factory.INSTANCE;

    static void initStatics() {
        // Trigger preceding sequence.
    }

    /** Look up a given method.
     * Callable only from sun.invoke and related packages.
     * <p>
     * The resulting method handle type will be of the given type,
     * with a receiver type {@code rcvc} prepended if the member is not static.
     * <p>
     * Access checks are made as of the given lookup class.
     * In particular, if the method is protected and {@code defc} is in a
     * different package from the lookup class, then {@code rcvc} must be
     * the lookup class or a subclass.
     * @param token Proof that the lookup class has access to this package.
     * @param member Resolved method or constructor to call.
     * @param name Name of the desired method.
     * @param rcvc Receiver type of desired non-static method (else null)
     * @param doDispatch whether the method handle will test the receiver type
     * @param lookupClass access-check relative to this class
     * @return a direct handle to the matching method
     * @throws IllegalAccessException if the given method cannot be accessed by the lookup class
     */
    static
    MethodHandle findMethod(MemberName method,
                            boolean doDispatch, Class<?> lookupClass) throws IllegalAccessException {
        MethodType mtype = method.getMethodType();
        if (!method.isStatic()) {
            // adjust the advertised receiver type to be exactly the one requested
            // (in the case of invokespecial, this will be the calling class)
            Class<?> recvType = method.getDeclaringClass();
            mtype = mtype.insertParameterTypes(0, recvType);
        }
        DirectMethodHandle mh = new DirectMethodHandle(mtype, method, doDispatch, lookupClass);
        if (!mh.isValid())
            throw method.makeAccessException("no direct method handle", lookupClass);
        assert(mh.type() == mtype);
        if (!method.isVarargs())
            return mh;
        int argc = mtype.parameterCount();
        if (argc != 0) {
            Class<?> arrayType = mtype.parameterType(argc-1);
            if (arrayType.isArray())
                return AdapterMethodHandle.makeVarargsCollector(mh, arrayType);
        }
        throw method.makeAccessException("cannot make variable arity", null);
    }

    static
    MethodHandle makeAllocator(MethodHandle rawConstructor) {
        MethodType rawConType = rawConstructor.type();
        Class<?> allocateClass = rawConType.parameterType(0);
        // Wrap the raw (unsafe) constructor with the allocation of a suitable object.
        // allocator(arg...)
        // [fold]=> cookedConstructor(obj=allocate(C), arg...)
        // [dup,collect]=> identity(obj, void=rawConstructor(obj, arg...))
        MethodHandle returner = MethodHandles.identity(allocateClass);
        MethodType ctype = rawConType.insertParameterTypes(0, allocateClass).changeReturnType(allocateClass);
        MethodHandle  cookedConstructor = AdapterMethodHandle.makeCollectArguments(returner, rawConstructor, 1, false);
        assert(cookedConstructor.type().equals(ctype));
        ctype = ctype.dropParameterTypes(0, 1);
        cookedConstructor = AdapterMethodHandle.makeCollectArguments(cookedConstructor, returner, 0, true);
        MethodHandle allocator = new AllocateObject(allocateClass);
        // allocate() => new C(void)
        assert(allocator.type().equals(MethodType.methodType(allocateClass)));
        ctype = ctype.dropParameterTypes(0, 1);
        MethodHandle fold = foldArguments(cookedConstructor, ctype, 0, allocator);
        return fold;
    }

    static final class AllocateObject<C> extends BoundMethodHandle {
        private static final Unsafe unsafe = Unsafe.getUnsafe();

        private final Class<C> allocateClass;

        // for allocation only:
        private AllocateObject(Class<C> allocateClass) {
            super(ALLOCATE.asType(MethodType.methodType(allocateClass, AllocateObject.class)));
            this.allocateClass = allocateClass;
        }
        @SuppressWarnings("unchecked")
        private C allocate() throws InstantiationException {
            return (C) unsafe.allocateInstance(allocateClass);
        }
        static final MethodHandle ALLOCATE;
        static {
            try {
                ALLOCATE = IMPL_LOOKUP.findVirtual(AllocateObject.class, "allocate", MethodType.genericMethodType(0));
            } catch (ReflectiveOperationException ex) {
                throw uncaughtException(ex);
            }
        }
    }

    static
    MethodHandle accessField(MemberName member, boolean isSetter,
                             Class<?> lookupClass) {
        // Use sun. misc.Unsafe to dig up the dirt on the field.
        MethodHandle mh = new FieldAccessor(member, isSetter);
        return mh;
    }

    static
    MethodHandle accessArrayElement(Class<?> arrayClass, boolean isSetter) {
        if (!arrayClass.isArray())
            throw newIllegalArgumentException("not an array: "+arrayClass);
        Class<?> elemClass = arrayClass.getComponentType();
        MethodHandle[] mhs = FieldAccessor.ARRAY_CACHE.get(elemClass);
        if (mhs == null) {
            if (!FieldAccessor.doCache(elemClass))
                return FieldAccessor.ahandle(arrayClass, isSetter);
            mhs = new MethodHandle[] {
                FieldAccessor.ahandle(arrayClass, false),
                FieldAccessor.ahandle(arrayClass, true)
            };
            if (mhs[0].type().parameterType(0) == Class.class) {
                mhs[0] = mhs[0].bindTo(elemClass);
                mhs[1] = mhs[1].bindTo(elemClass);
            }
            synchronized (FieldAccessor.ARRAY_CACHE) {}  // memory barrier
            FieldAccessor.ARRAY_CACHE.put(elemClass, mhs);
        }
        return mhs[isSetter ? 1 : 0];
    }

    static final class FieldAccessor<C,V> extends BoundMethodHandle {
        private static final Unsafe unsafe = Unsafe.getUnsafe();
        final Object base;  // for static refs only
        final long offset;
        final String name;

        FieldAccessor(MemberName field, boolean isSetter) {
            super(fhandle(field.getDeclaringClass(), field.getFieldType(), isSetter, field.isStatic()));
            this.offset = fieldOffset(field);
            this.name = field.getName();
            this.base = null;
        }
        @Override
        String debugString() { return addTypeString(name, this); }

        int getFieldI(C obj) { return unsafe.getInt(obj, offset); }
        void setFieldI(C obj, int x) { unsafe.putInt(obj, offset, x); }
        long getFieldJ(C obj) { return unsafe.getLong(obj, offset); }
        void setFieldJ(C obj, long x) { unsafe.putLong(obj, offset, x); }
        float getFieldF(C obj) { return unsafe.getFloat(obj, offset); }
        void setFieldF(C obj, float x) { unsafe.putFloat(obj, offset, x); }
        double getFieldD(C obj) { return unsafe.getDouble(obj, offset); }
        void setFieldD(C obj, double x) { unsafe.putDouble(obj, offset, x); }
        boolean getFieldZ(C obj) { return unsafe.getBoolean(obj, offset); }
        void setFieldZ(C obj, boolean x) { unsafe.putBoolean(obj, offset, x); }
        byte getFieldB(C obj) { return unsafe.getByte(obj, offset); }
        void setFieldB(C obj, byte x) { unsafe.putByte(obj, offset, x); }
        short getFieldS(C obj) { return unsafe.getShort(obj, offset); }
        void setFieldS(C obj, short x) { unsafe.putShort(obj, offset, x); }
        char getFieldC(C obj) { return unsafe.getChar(obj, offset); }
        void setFieldC(C obj, char x) { unsafe.putChar(obj, offset, x); }
        @SuppressWarnings("unchecked")
        V getFieldL(C obj) { return (V) unsafe.getObject(obj, offset); }
        @SuppressWarnings("unchecked")
        void setFieldL(C obj, V x) { unsafe.putObject(obj, offset, x); }
        // cast (V) is OK here, since we wrap convertArguments around the MH.

        static Integer fieldOffset(final MemberName field) {
            return AccessController.doPrivileged(new PrivilegedAction<Integer>() {
                    public Integer run() {
                        try {
                            Class c = field.getDeclaringClass();
                            // FIXME:  Should not have to create 'f' to get this value.
                            java.lang.reflect.Field f = c.getDeclaredField(field.getName());
                            return unsafe.fieldOffset(f);
                        } catch (NoSuchFieldException ee) {
                            throw uncaughtException(ee);
                        }
                    }
                });
        }

        int getStaticI() { return unsafe.getInt(base, offset); }
        void setStaticI(int x) { unsafe.putInt(base, offset, x); }
        long getStaticJ() { return unsafe.getLong(base, offset); }
        void setStaticJ(long x) { unsafe.putLong(base, offset, x); }
        float getStaticF() { return unsafe.getFloat(base, offset); }
        void setStaticF(float x) { unsafe.putFloat(base, offset, x); }
        double getStaticD() { return unsafe.getDouble(base, offset); }
        void setStaticD(double x) { unsafe.putDouble(base, offset, x); }
        boolean getStaticZ() { return unsafe.getBoolean(base, offset); }
        void setStaticZ(boolean x) { unsafe.putBoolean(base, offset, x); }
        byte getStaticB() { return unsafe.getByte(base, offset); }
        void setStaticB(byte x) { unsafe.putByte(base, offset, x); }
        short getStaticS() { return unsafe.getShort(base, offset); }
        void setStaticS(short x) { unsafe.putShort(base, offset, x); }
        char getStaticC() { return unsafe.getChar(base, offset); }
        void setStaticC(char x) { unsafe.putChar(base, offset, x); }
        V getStaticL() { return (V) unsafe.getObject(base, offset); }
        void setStaticL(V x) { unsafe.putObject(base, offset, x); }

        static String fname(Class<?> vclass, boolean isSetter, boolean isStatic) {
            String stem;
            if (!isStatic)
                stem = (!isSetter ? "getField" : "setField");
            else
                stem = (!isSetter ? "getStatic" : "setStatic");
            return stem + Wrapper.basicTypeChar(vclass);
        }
        static MethodType ftype(Class<?> cclass, Class<?> vclass, boolean isSetter, boolean isStatic) {
            MethodType type;
            if (!isStatic) {
                if (!isSetter)
                    return MethodType.methodType(vclass, cclass);
                else
                    return MethodType.methodType(void.class, cclass, vclass);
            } else {
                if (!isSetter)
                    return MethodType.methodType(vclass);
                else
                    return MethodType.methodType(void.class, vclass);
            }
        }
        static MethodHandle fhandle(Class<?> cclass, Class<?> vclass, boolean isSetter, boolean isStatic) {
            String name = FieldAccessor.fname(vclass, isSetter, isStatic);
            if (cclass.isPrimitive())  throw newIllegalArgumentException("primitive "+cclass);
            Class<?> ecclass = Object.class;  //erase this type
            Class<?> evclass = vclass;
            if (!evclass.isPrimitive())  evclass = Object.class;
            MethodType type = FieldAccessor.ftype(ecclass, evclass, isSetter, isStatic);
            MethodHandle mh;
            try {
                mh = IMPL_LOOKUP.findVirtual(FieldAccessor.class, name, type);
            } catch (ReflectiveOperationException ex) {
                throw uncaughtException(ex);
            }
            if (evclass != vclass || (!isStatic && ecclass != cclass)) {
                MethodType strongType = FieldAccessor.ftype(cclass, vclass, isSetter, isStatic);
                strongType = strongType.insertParameterTypes(0, FieldAccessor.class);
                mh = convertArguments(mh, strongType, 0);
            }
            return mh;
        }

        /// Support for array element access
        static final HashMap<Class<?>, MethodHandle[]> ARRAY_CACHE =
                new HashMap<Class<?>, MethodHandle[]>();
        // FIXME: Cache on the classes themselves, not here.
        static boolean doCache(Class<?> elemClass) {
            if (elemClass.isPrimitive())  return true;
            ClassLoader cl = elemClass.getClassLoader();
            return cl == null || cl == ClassLoader.getSystemClassLoader();
        }
        static int getElementI(int[] a, int i) { return a[i]; }
        static void setElementI(int[] a, int i, int x) { a[i] = x; }
        static long getElementJ(long[] a, int i) { return a[i]; }
        static void setElementJ(long[] a, int i, long x) { a[i] = x; }
        static float getElementF(float[] a, int i) { return a[i]; }
        static void setElementF(float[] a, int i, float x) { a[i] = x; }
        static double getElementD(double[] a, int i) { return a[i]; }
        static void setElementD(double[] a, int i, double x) { a[i] = x; }
        static boolean getElementZ(boolean[] a, int i) { return a[i]; }
        static void setElementZ(boolean[] a, int i, boolean x) { a[i] = x; }
        static byte getElementB(byte[] a, int i) { return a[i]; }
        static void setElementB(byte[] a, int i, byte x) { a[i] = x; }
        static short getElementS(short[] a, int i) { return a[i]; }
        static void setElementS(short[] a, int i, short x) { a[i] = x; }
        static char getElementC(char[] a, int i) { return a[i]; }
        static void setElementC(char[] a, int i, char x) { a[i] = x; }
        static Object getElementL(Object[] a, int i) { return a[i]; }
        static void setElementL(Object[] a, int i, Object x) { a[i] = x; }
        static <V> V getElementL(Class<V[]> aclass, V[] a, int i) { return aclass.cast(a)[i]; }
        static <V> void setElementL(Class<V[]> aclass, V[] a, int i, V x) { aclass.cast(a)[i] = x; }

        static String aname(Class<?> aclass, boolean isSetter) {
            Class<?> vclass = aclass.getComponentType();
            if (vclass == null)  throw new IllegalArgumentException();
            return (!isSetter ? "getElement" : "setElement") + Wrapper.basicTypeChar(vclass);
        }
        static MethodType atype(Class<?> aclass, boolean isSetter) {
            Class<?> vclass = aclass.getComponentType();
            if (!isSetter)
                return MethodType.methodType(vclass, aclass, int.class);
            else
                return MethodType.methodType(void.class, aclass, int.class, vclass);
        }
        static MethodHandle ahandle(Class<?> aclass, boolean isSetter) {
            Class<?> vclass = aclass.getComponentType();
            String name = FieldAccessor.aname(aclass, isSetter);
            Class<?> caclass = null;
            if (!vclass.isPrimitive() && vclass != Object.class) {
                caclass = aclass;
                aclass = Object[].class;
                vclass = Object.class;
            }
            MethodType type = FieldAccessor.atype(aclass, isSetter);
            if (caclass != null)
                type = type.insertParameterTypes(0, Class.class);
            MethodHandle mh;
            try {
                mh = IMPL_LOOKUP.findStatic(FieldAccessor.class, name, type);
            } catch (ReflectiveOperationException ex) {
                throw uncaughtException(ex);
            }
            if (caclass != null) {
                MethodType strongType = FieldAccessor.atype(caclass, isSetter);
                mh = mh.bindTo(caclass);
                mh = convertArguments(mh, strongType, 0);
            }
            return mh;
        }
    }

    /** Bind a predetermined first argument to the given direct method handle.
     * Callable only from MethodHandles.
     * @param token Proof that the caller has access to this package.
     * @param target Any direct method handle.
     * @param receiver Receiver (or first static method argument) to pre-bind.
     * @return a BoundMethodHandle for the given DirectMethodHandle, or null if it does not exist
     */
    static
    MethodHandle bindReceiver(MethodHandle target, Object receiver) {
        if (receiver == null)  return null;
        return new BoundMethodHandle(target, receiver, 0);
    }

    /** Bind a predetermined argument to the given arbitrary method handle.
     * Callable only from MethodHandles.
     * @param token Proof that the caller has access to this package.
     * @param target Any method handle.
     * @param receiver Argument (which can be a boxed primitive) to pre-bind.
     * @return a suitable BoundMethodHandle
     */
    static
    MethodHandle bindArgument(MethodHandle target, int argnum, Object receiver) {
        return new BoundMethodHandle(target, receiver, argnum);
    }

    static native MethodHandle permuteArguments(MethodHandle target,
                                                MethodType newType,
                                                MethodType oldType,
                                                int[] permutationOrNull);

    /*non-public*/ static
    MethodHandle convertArguments(MethodHandle target, MethodType newType, int level) {
        MethodType oldType = target.type();
        if (oldType.equals(newType))
            return target;
        assert(level > 1 || oldType.isConvertibleTo(newType));
        MethodHandle retFilter = null;
        Class<?> oldRT = oldType.returnType();
        Class<?> newRT = newType.returnType();
        if (!VerifyType.isNullConversion(oldRT, newRT)) {
            if (oldRT == void.class) {
                Wrapper wrap = newRT.isPrimitive() ? Wrapper.forPrimitiveType(newRT) : Wrapper.OBJECT;
                retFilter = ValueConversions.zeroConstantFunction(wrap);
            } else {
                retFilter = MethodHandles.identity(newRT);
                retFilter = convertArguments(retFilter, retFilter.type().changeParameterType(0, oldRT), level);
            }
            newType = newType.changeReturnType(oldRT);
        }
        MethodHandle res = null;
        Exception ex = null;
        try {
            res = convertArguments(target, newType, oldType, level);
        } catch (IllegalArgumentException ex1) {
            ex = ex1;
        }
        if (res == null) {
            WrongMethodTypeException wmt = new WrongMethodTypeException("cannot convert to "+newType+": "+target);
            wmt.initCause(ex);
            throw wmt;
        }
        if (retFilter != null)
            res = MethodHandles.filterReturnValue(res, retFilter);
        return res;
    }

    static MethodHandle convertArguments(MethodHandle target,
                                                MethodType newType,
                                                MethodType oldType,
                                                int level) {
        assert(oldType.parameterCount() == target.type().parameterCount());
        if (newType == oldType)
            return target;
        if (oldType.parameterCount() != newType.parameterCount())
            throw newIllegalArgumentException("mismatched parameter count", oldType, newType);
        return AdapterMethodHandle.makePairwiseConvert(newType, target, level);
    }

    static MethodHandle spreadArguments(MethodHandle target, Class<?> arrayType, int arrayLength) {
        MethodType oldType = target.type();
        int nargs = oldType.parameterCount();
        int keepPosArgs = nargs - arrayLength;
        MethodType newType = oldType
                .dropParameterTypes(keepPosArgs, nargs)
                .insertParameterTypes(keepPosArgs, arrayType);
        return spreadArguments(target, newType, keepPosArgs, arrayType, arrayLength);
    }
    // called internally only
    static MethodHandle spreadArgumentsFromPos(MethodHandle target, MethodType newType, int spreadArgPos) {
        int arrayLength = target.type().parameterCount() - spreadArgPos;
        return spreadArguments(target, newType, spreadArgPos, Object[].class, arrayLength);
    }
    static MethodHandle spreadArguments(MethodHandle target,
                                               MethodType newType,
                                               int spreadArgPos,
                                               Class<?> arrayType,
                                               int arrayLength) {
        // TO DO: maybe allow the restarg to be Object and implicitly cast to Object[]
        MethodType oldType = target.type();
        // spread the last argument of newType to oldType
        assert(arrayLength == oldType.parameterCount() - spreadArgPos);
        assert(newType.parameterType(spreadArgPos) == arrayType);
        return AdapterMethodHandle.makeSpreadArguments(newType, target, arrayType, spreadArgPos, arrayLength);
    }

    static MethodHandle collectArguments(MethodHandle target,
                                                int collectArg,
                                                MethodHandle collector) {
        MethodType type = target.type();
        Class<?> collectType = collector.type().returnType();
        assert(collectType != void.class);  // else use foldArguments
        if (collectType != type.parameterType(collectArg))
            target = target.asType(type.changeParameterType(collectArg, collectType));
        MethodType newType = type
                .dropParameterTypes(collectArg, collectArg+1)
                .insertParameterTypes(collectArg, collector.type().parameterArray());
        return collectArguments(target, newType, collectArg, collector);
    }
    static MethodHandle collectArguments(MethodHandle target,
                                                MethodType newType,
                                                int collectArg,
                                                MethodHandle collector) {
        MethodType oldType = target.type();     // (a...,c)=>r
        //         newType                      // (a..., b...)=>r
        MethodType colType = collector.type();  // (b...)=>c
        //         oldType                      // (a..., b...)=>r
        assert(newType.parameterCount() == collectArg + colType.parameterCount());
        assert(oldType.parameterCount() == collectArg + 1);
        return AdapterMethodHandle.makeCollectArguments(target, collector, collectArg, false);
    }

    static MethodHandle filterArgument(MethodHandle target,
                                       int pos,
                                       MethodHandle filter) {
        MethodType ttype = target.type();
        MethodType ftype = filter.type();
        assert(ftype.parameterCount() == 1);
        return AdapterMethodHandle.makeCollectArguments(target, filter, pos, false);
    }

    static MethodHandle foldArguments(MethodHandle target,
                                      MethodType newType,
                                      int foldPos,
                                      MethodHandle combiner) {
        MethodType oldType = target.type();
        MethodType ctype = combiner.type();
        return AdapterMethodHandle.makeCollectArguments(target, combiner, foldPos, true);
    }

    static
    MethodHandle dropArguments(MethodHandle target,
                               MethodType newType, int argnum) {
        int drops = newType.parameterCount() - target.type().parameterCount();
        return AdapterMethodHandle.makeDropArguments(newType, target, argnum, drops);
    }

    static
    MethodHandle selectAlternative(boolean testResult, MethodHandle target, MethodHandle fallback) {
        return testResult ? target : fallback;
    }

    static MethodHandle SELECT_ALTERNATIVE;
    static MethodHandle selectAlternative() {
        if (SELECT_ALTERNATIVE != null)  return SELECT_ALTERNATIVE;
        try {
            SELECT_ALTERNATIVE
            = IMPL_LOOKUP.findStatic(MethodHandleImpl.class, "selectAlternative",
                    MethodType.methodType(MethodHandle.class, boolean.class, MethodHandle.class, MethodHandle.class));
        } catch (ReflectiveOperationException ex) {
            throw new RuntimeException(ex);
        }
        return SELECT_ALTERNATIVE;
    }

    static
    MethodHandle makeGuardWithTest(MethodHandle test,
                                   MethodHandle target,
                                   MethodHandle fallback) {
        // gwt(arg...)
        // [fold]=> continueAfterTest(z=test(arg...), arg...)
        // [filter]=> (tf=select(z))(arg...)
        //    where select(z) = select(z, t, f).bindTo(t, f) => z ? t f
        // [tailcall]=> tf(arg...)
        assert(test.type().returnType() == boolean.class);
        MethodType targetType = target.type();
        MethodType foldTargetType = targetType.insertParameterTypes(0, boolean.class);
        // working backwards, as usual:
        assert(target.type().equals(fallback.type()));
        MethodHandle tailcall = MethodHandles.exactInvoker(target.type());
        MethodHandle select = selectAlternative();
        select = bindArgument(select, 2, fallback);
        select = bindArgument(select, 1, target);
        // select(z: boolean) => (z ? target : fallback)
        MethodHandle filter = filterArgument(tailcall, 0, select);
        assert(filter.type().parameterType(0) == boolean.class);
        MethodHandle fold = foldArguments(filter, filter.type().dropParameterTypes(0, 1), 0, test);
        return fold;
    }

    private static class GuardWithCatch extends BoundMethodHandle {
        private final MethodHandle target;
        private final Class<? extends Throwable> exType;
        private final MethodHandle catcher;
        GuardWithCatch(MethodHandle target, Class<? extends Throwable> exType, MethodHandle catcher) {
            this(INVOKES[target.type().parameterCount()], target, exType, catcher);
        }
        // FIXME: Build the control flow out of foldArguments.
        GuardWithCatch(MethodHandle invoker,
                       MethodHandle target, Class<? extends Throwable> exType, MethodHandle catcher) {
            super(invoker);
            this.target = target;
            this.exType = exType;
            this.catcher = catcher;
        }
        @Override
        String debugString() {
            return addTypeString(target, this);
        }
        private Object invoke_V(Object... av) throws Throwable {
            try {
                return target.invokeExact(av);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, av);
            }
        }
        private Object invoke_L0() throws Throwable {
            try {
                return target.invokeExact();
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t);
            }
        }
        private Object invoke_L1(Object a0) throws Throwable {
            try {
                return target.invokeExact(a0);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0);
            }
        }
        private Object invoke_L2(Object a0, Object a1) throws Throwable {
            try {
                return target.invokeExact(a0, a1);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1);
            }
        }
        private Object invoke_L3(Object a0, Object a1, Object a2) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2);
            }
        }
        private Object invoke_L4(Object a0, Object a1, Object a2, Object a3) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2, a3);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2, a3);
            }
        }
        private Object invoke_L5(Object a0, Object a1, Object a2, Object a3, Object a4) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2, a3, a4);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2, a3, a4);
            }
        }
        private Object invoke_L6(Object a0, Object a1, Object a2, Object a3, Object a4, Object a5) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2, a3, a4, a5);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2, a3, a4, a5);
            }
        }
        private Object invoke_L7(Object a0, Object a1, Object a2, Object a3, Object a4, Object a5, Object a6) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2, a3, a4, a5, a6);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2, a3, a4, a5, a6);
            }
        }
        private Object invoke_L8(Object a0, Object a1, Object a2, Object a3, Object a4, Object a5, Object a6, Object a7) throws Throwable {
            try {
                return target.invokeExact(a0, a1, a2, a3, a4, a5, a6, a7);
            } catch (Throwable t) {
                if (!exType.isInstance(t))  throw t;
                return catcher.invokeExact(t, a0, a1, a2, a3, a4, a5, a6, a7);
            }
        }
        static MethodHandle[] makeInvokes() {
            ArrayList<MethodHandle> invokes = new ArrayList<MethodHandle>();
            MethodHandles.Lookup lookup = IMPL_LOOKUP;
            for (;;) {
                int nargs = invokes.size();
                String name = "invoke_L"+nargs;
                MethodHandle invoke = null;
                try {
                    invoke = lookup.findVirtual(GuardWithCatch.class, name, MethodType.genericMethodType(nargs));
                } catch (ReflectiveOperationException ex) {
                }
                if (invoke == null)  break;
                invokes.add(invoke);
            }
            assert(invokes.size() == 9);  // current number of methods
            return invokes.toArray(new MethodHandle[0]);
        };
        static final MethodHandle[] INVOKES = makeInvokes();
        // For testing use this:
        //static final MethodHandle[] INVOKES = Arrays.copyOf(makeInvokes(), 2);
        static final MethodHandle VARARGS_INVOKE;
        static {
            try {
                VARARGS_INVOKE = IMPL_LOOKUP.findVirtual(GuardWithCatch.class, "invoke_V", MethodType.genericMethodType(0, true));
            } catch (ReflectiveOperationException ex) {
                throw uncaughtException(ex);
            }
        }
    }


    static
    MethodHandle makeGuardWithCatch(MethodHandle target,
                                    Class<? extends Throwable> exType,
                                    MethodHandle catcher) {
        MethodType type = target.type();
        MethodType ctype = catcher.type();
        int nargs = type.parameterCount();
        if (nargs < GuardWithCatch.INVOKES.length) {
            MethodType gtype = type.generic();
            MethodType gcatchType = gtype.insertParameterTypes(0, Throwable.class);
            // Note: convertArguments(...2) avoids interface casts present in convertArguments(...0)
            MethodHandle gtarget = convertArguments(target, gtype, type, 2);
            MethodHandle gcatcher = convertArguments(catcher, gcatchType, ctype, 2);
            MethodHandle gguard = new GuardWithCatch(gtarget, exType, gcatcher);
            if (gtarget == null || gcatcher == null || gguard == null)  return null;
            return convertArguments(gguard, type, gtype, 2);
        } else {
            MethodType gtype = MethodType.genericMethodType(0, true);
            MethodType gcatchType = gtype.insertParameterTypes(0, Throwable.class);
            MethodHandle gtarget = spreadArgumentsFromPos(target, gtype, 0);
            catcher = catcher.asType(ctype.changeParameterType(0, Throwable.class));
            MethodHandle gcatcher = spreadArgumentsFromPos(catcher, gcatchType, 1);
            MethodHandle gguard = new GuardWithCatch(GuardWithCatch.VARARGS_INVOKE, gtarget, exType, gcatcher);
            if (gtarget == null || gcatcher == null || gguard == null)  return null;
            return collectArguments(gguard, type, 0, ValueConversions.varargsArray(nargs)).asType(type);
        }
    }

    static
    MethodHandle throwException(MethodType type) {
        return AdapterMethodHandle.makeRetypeRaw(type, throwException());
    }

    static MethodHandle THROW_EXCEPTION;
    static MethodHandle throwException() {
        if (THROW_EXCEPTION != null)  return THROW_EXCEPTION;
        try {
            THROW_EXCEPTION
            = IMPL_LOOKUP.findStatic(MethodHandleImpl.class, "throwException",
                    MethodType.methodType(Empty.class, Throwable.class));
        } catch (ReflectiveOperationException ex) {
            throw new RuntimeException(ex);
        }
        return THROW_EXCEPTION;
    }
    static <T extends Throwable> Empty throwException(T t) throws T { throw t; }
}
