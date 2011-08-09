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
import sun.invoke.util.Wrapper;
import sun.invoke.util.ValueConversions;
import java.util.Arrays;
import java.util.ArrayList;
import java.util.Collections;
import static java.lang.invoke.MethodHandleStatics.*;
import ikvm.internal.NotYetImplementedError;

/**
 * This method handle performs simple conversion or checking of a single argument.
 * @author jrose
 */
class AdapterMethodHandle extends BoundMethodHandle {

    AdapterMethodHandle(MethodType type) {
        super(type, null, -1);
    }
    AdapterMethodHandle(MethodType type, Object vmtarget) {
        super(type, null, -1);
        this.vmtarget = vmtarget;
    }

    /** Can a JVM-level adapter directly implement the proposed
     *  argument conversion, as if by fixed-arity MethodHandle.asType?
     */
    static boolean canConvertArgument(Class<?> src, Class<?> dst, int level) {
        return true;
    }

    /**
     * Create a JVM-level adapter method handle to conform the given method
     * handle to the similar newType, using only pairwise argument conversions.
     * For each argument, convert incoming argument to the exact type needed.
     * The argument conversions allowed are casting, boxing and unboxing,
     * integral widening or narrowing, and floating point widening or narrowing.
     * @param newType required call type
     * @param target original method handle
     * @param level which strength of conversion is allowed
     * @return an adapter to the original handle with the desired new type,
     *          or the original target if the types are already identical
     *          or null if the adaptation cannot be made
     */
    static native MethodHandle makePairwiseConvert(MethodType newType, MethodHandle target, int level);

    /* Return one plus the position of the first non-trivial difference
     * between the given types.  This is not a symmetric operation;
     * we are considering adapting the targetType to adapterType.
     * Trivial differences are those which could be ignored by the JVM
     * without subverting the verifier.  Otherwise, adaptable differences
     * are ones for which we could create an adapter to make the type change.
     * Return zero if there are no differences (other than trivial ones).
     * Return 1+N if N is the only adaptable argument difference.
     * Return the -2-N where N is the first of several adaptable
     * argument differences.
     * Return -1 if there there are differences which are not adaptable.
     */
    private static int diffTypes(MethodType adapterType,
                                 MethodType targetType,
                                 boolean raw) {
        int diff;
        diff = diffReturnTypes(adapterType, targetType, raw);
        if (diff != 0)  return diff;
        int nargs = adapterType.parameterCount();
        if (nargs != targetType.parameterCount())
            return -1;
        diff = diffParamTypes(adapterType, 0, targetType, 0, nargs, raw);
        //System.out.println("diff "+adapterType);
        //System.out.println("  "+diff+" "+targetType);
        return diff;
    }
    private static int diffReturnTypes(MethodType adapterType,
                                       MethodType targetType,
                                       boolean raw) {
        Class<?> src = targetType.returnType();
        Class<?> dst = adapterType.returnType();
        if ((!raw
             ? VerifyType.canPassUnchecked(src, dst)
             : VerifyType.canPassRaw(src, dst)
             ) > 0)
            return 0;  // no significant difference
        if (raw && !src.isPrimitive() && !dst.isPrimitive())
            return 0;  // can force a reference return (very carefully!)
        //if (false)  return 1;  // never adaptable!
        return -1;  // some significant difference
    }
    private static int diffParamTypes(MethodType adapterType, int astart,
                                      MethodType targetType, int tstart,
                                      int nargs, boolean raw) {
        assert(nargs >= 0);
        int res = 0;
        for (int i = 0; i < nargs; i++) {
            Class<?> src  = adapterType.parameterType(astart+i);
            Class<?> dest = targetType.parameterType(tstart+i);
            if ((!raw
                 ? VerifyType.canPassUnchecked(src, dest)
                 : VerifyType.canPassRaw(src, dest)
                ) <= 0) {
                // found a difference; is it the only one so far?
                if (res != 0)
                    return -1-res; // return -2-i for prev. i
                res = 1+i;
            }
        }
        return res;
    }

    /** Can a retyping adapter (alone) validly convert the target to newType? */
    static boolean canRetypeOnly(MethodType newType, MethodType targetType) {
        return canRetype(newType, targetType, false);
    }
    /** Can a retyping adapter (alone) convert the target to newType?
     *  It is allowed to widen subword types and void to int, to make bitwise
     *  conversions between float/int and double/long, and to perform unchecked
     *  reference conversions on return.  This last feature requires that the
     *  caller be trusted, and perform explicit cast conversions on return values.
     */
    static boolean canRetypeRaw(MethodType newType, MethodType targetType) {
        return canRetype(newType, targetType, true);
    }
    static boolean canRetype(MethodType newType, MethodType targetType, boolean raw) {
        int diff = diffTypes(newType, targetType, raw);
        // %%% This assert is too strong.  Factor diff into VerifyType and reconcile.
        assert(raw || (diff == 0) == VerifyType.isNullConversion(newType, targetType));
        return diff == 0;
    }

    /** Factory method:  Performs no conversions; simply retypes the adapter.
     *  Allows unchecked argument conversions pairwise, if they are safe.
     *  Returns null if not possible.
     */
    static MethodHandle makeRetypeOnly(MethodType newType, MethodHandle target) {
        return makeRetype(newType, target, false);
    }
    static MethodHandle makeRetypeRaw(MethodType newType, MethodHandle target) {
        return makeRetype(newType, target, true);
    }
    static native MethodHandle makeRetype(MethodType newType, MethodHandle target, boolean raw);

    static MethodHandle makeVarargsCollector(MethodHandle target, Class<?> arrayType) {
        MethodType type = target.type();
        int last = type.parameterCount() - 1;
        if (type.parameterType(last) != arrayType)
            target = target.asType(type.changeParameterType(last, arrayType));
        target = target.asFixedArity();  // make sure this attribute is turned off
        return new AsVarargsCollector(target, arrayType);
    }

    static class AsVarargsCollector extends AdapterMethodHandle {
        final MethodHandle target;
        final Class<?> arrayType;
        MethodHandle cache;

        AsVarargsCollector(MethodHandle target, Class<?> arrayType) {
            super(target.type());
            this.vmtarget = target.vmtarget;
            this.target = target;
            this.arrayType = arrayType;
            this.cache = target.asCollector(arrayType, 0);
        }

        @Override
        public boolean isVarargsCollector() {
            return true;
        }

        @Override
        public MethodHandle asFixedArity() {
            return target;
        }

        @Override
        public MethodHandle asType(MethodType newType) {
            MethodType type = this.type();
            int collectArg = type.parameterCount() - 1;
            int newArity = newType.parameterCount();
            if (newArity == collectArg+1 &&
                type.parameterType(collectArg).isAssignableFrom(newType.parameterType(collectArg))) {
                // if arity and trailing parameter are compatible, do normal thing
                return super.asType(newType);
            }
            // check cache
            if (cache.type().parameterCount() == newArity)
                return cache.asType(newType);
            // build and cache a collector
            int arrayLength = newArity - collectArg;
            MethodHandle collector;
            try {
                collector = target.asCollector(arrayType, arrayLength);
            } catch (IllegalArgumentException ex) {
                throw new WrongMethodTypeException("cannot build collector");
            }
            cache = collector;
            return collector.asType(newType);
        }
    }

    /** Can a checkcast adapter validly convert the target to newType?
     *  The JVM supports all kind of reference casts, even silly ones.
     */
    static boolean canCheckCast(MethodType newType, MethodType targetType,
                int arg, Class<?> castType) {
        Class<?> src = newType.parameterType(arg);
        Class<?> dst = targetType.parameterType(arg);
        if (!canCheckCast(src, castType)
                || !VerifyType.isNullConversion(castType, dst))
            return false;
        int diff = diffTypes(newType, targetType, false);
        return (diff == arg+1) || (diff == 0);  // arg is sole non-trivial diff
    }
    /** Can an primitive conversion adapter validly convert src to dst? */
    static boolean canCheckCast(Class<?> src, Class<?> dst) {
        return (!src.isPrimitive() && !dst.isPrimitive());
    }

    /** Factory method:  Forces a cast at the given argument.
     *  The castType is the target of the cast, and can be any type
     *  with a null conversion to the corresponding target parameter.
     *  Return null if this cannot be done.
     */
    static MethodHandle makeCheckCast(MethodType newType, MethodHandle target,
                int arg, Class<?> castType) {
        if (!canCheckCast(newType, target.type(), arg, castType))
            return null;
        throw new NotYetImplementedError();
    }

    /** Can an adapter simply drop arguments to convert the target to newType? */
    static boolean canDropArguments(MethodType newType, MethodType targetType,
                int dropArgPos, int dropArgCount) {
        if (dropArgCount == 0)
            return canRetypeOnly(newType, targetType);
        if (diffReturnTypes(newType, targetType, false) != 0)
            return false;
        int nptypes = newType.parameterCount();
        // parameter types must be the same up to the drop point
        if (dropArgPos != 0 && diffParamTypes(newType, 0, targetType, 0, dropArgPos, false) != 0)
            return false;
        int afterPos = dropArgPos + dropArgCount;
        int afterCount = nptypes - afterPos;
        if (dropArgPos < 0 || dropArgPos >= nptypes ||
            dropArgCount < 1 || afterPos > nptypes ||
            targetType.parameterCount() != nptypes - dropArgCount)
            return false;
        // parameter types after the drop point must also be the same
        if (afterCount != 0 && diffParamTypes(newType, afterPos, targetType, dropArgPos, afterCount, false) != 0)
            return false;
        return true;
    }

    /** Factory method:  Drop selected arguments.
     *  Allow unchecked retyping of remaining arguments, pairwise.
     *  Return null if this is not possible.
     */
    static MethodHandle makeDropArguments(MethodType newType, MethodHandle target,
                int dropArgPos, int dropArgCount) {
        if (dropArgCount == 0)
            return makeRetypeOnly(newType, target);
        if (!canDropArguments(newType, target.type(), dropArgPos, dropArgCount))
            return null;
        int[] permute = new int[target.type().parameterCount()];
        for (int i = 0, arg = 0; i < permute.length; i++) {
            if (arg == dropArgPos)
                arg += dropArgCount;
            permute[i] = arg++;
        }
        return MethodHandleImpl.permuteArguments(target, newType, target.type(), permute);
    }

    /** Can an adapter duplicate an argument to convert the target to newType? */
    static boolean canDupArguments(MethodType newType, MethodType targetType,
                int dupArgPos, int dupArgCount) {
        if (diffReturnTypes(newType, targetType, false) != 0)
            return false;
        int nptypes = newType.parameterCount();
        if (dupArgCount < 0 || dupArgPos + dupArgCount > nptypes)
            return false;
        if (targetType.parameterCount() != nptypes + dupArgCount)
            return false;
        // parameter types must be the same up to the duplicated arguments
        if (diffParamTypes(newType, 0, targetType, 0, nptypes, false) != 0)
            return false;
        // duplicated types must be, well, duplicates
        if (diffParamTypes(newType, dupArgPos, targetType, nptypes, dupArgCount, false) != 0)
            return false;
        return true;
    }

    /** Factory method:  Duplicate the selected argument.
     *  Return null if this is not possible.
     */
    static MethodHandle makeDupArguments(MethodType newType, MethodHandle target,
                int dupArgPos, int dupArgCount) {
        if (!canDupArguments(newType, target.type(), dupArgPos, dupArgCount))
            return null;
        if (dupArgCount == 0)
            return target;
        // in  arglist: [0: ...keep1 | dpos: dup... | dpos+dcount: keep2... ]
        // out arglist: [0: ...keep1 | dpos: dup... | dpos+dcount: keep2... | dup... ]
        throw new NotYetImplementedError();
    }

    /** Can an adapter swap two arguments to convert the target to newType? */
    static boolean canSwapArguments(MethodType newType, MethodType targetType,
                int swapArg1, int swapArg2) {
        if (diffReturnTypes(newType, targetType, false) != 0)
            return false;
        if (swapArg1 >= swapArg2)  return false;  // caller resp
        int nptypes = newType.parameterCount();
        if (targetType.parameterCount() != nptypes)
            return false;
        if (swapArg1 < 0 || swapArg2 >= nptypes)
            return false;
        if (diffParamTypes(newType, 0, targetType, 0, swapArg1, false) != 0)
            return false;
        if (diffParamTypes(newType, swapArg1, targetType, swapArg2, 1, false) != 0)
            return false;
        if (diffParamTypes(newType, swapArg1+1, targetType, swapArg1+1, swapArg2-swapArg1-1, false) != 0)
            return false;
        if (diffParamTypes(newType, swapArg2, targetType, swapArg1, 1, false) != 0)
            return false;
        if (diffParamTypes(newType, swapArg2+1, targetType, swapArg2+1, nptypes-swapArg2-1, false) != 0)
            return false;
        return true;
    }

    /** Factory method:  Swap the selected arguments.
     *  Return null if this is not possible.
     */
    static MethodHandle makeSwapArguments(MethodType newType, MethodHandle target,
                int swapArg1, int swapArg2) {
        if (swapArg1 == swapArg2)
            return target;
        if (swapArg1 > swapArg2) { int t = swapArg1; swapArg1 = swapArg2; swapArg2 = t; }
        if (!canSwapArguments(newType, target.type(), swapArg1, swapArg2))
            return null;
        // in  arglist: [0: ...keep1 | pos1: a1 | pos1+1: keep2... | pos2: a2 | pos2+1: keep3... ]
        // out arglist: [0: ...keep1 | pos1: a2 | pos1+1: keep2... | pos2: a1 | pos2+1: keep3... ]
        throw new NotYetImplementedError();
    }

    static int positiveRotation(int argCount, int rotateBy) {
        assert(argCount > 0);
        if (rotateBy >= 0) {
            if (rotateBy < argCount)
                return rotateBy;
            return rotateBy % argCount;
        } else if (rotateBy >= -argCount) {
            return rotateBy + argCount;
        } else {
            return (-1-((-1-rotateBy) % argCount)) + argCount;
        }
    }

    final static int MAX_ARG_ROTATION = 1;

    /** Can an adapter rotate arguments to convert the target to newType? */
    static boolean canRotateArguments(MethodType newType, MethodType targetType,
                int firstArg, int argCount, int rotateBy) {
        rotateBy = positiveRotation(argCount, rotateBy);
        if (rotateBy == 0)  return false;  // no rotation
        if (rotateBy > MAX_ARG_ROTATION && rotateBy < argCount - MAX_ARG_ROTATION)
            return false;  // too many argument positions
        // Rotate incoming args right N to the out args, N in 1..(argCouunt-1).
        if (diffReturnTypes(newType, targetType, false) != 0)
            return false;
        int nptypes = newType.parameterCount();
        if (targetType.parameterCount() != nptypes)
            return false;
        if (firstArg < 0 || firstArg >= nptypes)  return false;
        int argLimit = firstArg + argCount;
        if (argLimit > nptypes)  return false;
        if (diffParamTypes(newType, 0, targetType, 0, firstArg, false) != 0)
            return false;
        int newChunk1 = argCount - rotateBy, newChunk2 = rotateBy;
        // swap new chunk1 with target chunk2
        if (diffParamTypes(newType, firstArg, targetType, argLimit-newChunk1, newChunk1, false) != 0)
            return false;
        // swap new chunk2 with target chunk1
        if (diffParamTypes(newType, firstArg+newChunk1, targetType, firstArg, newChunk2, false) != 0)
            return false;
        return true;
    }

    /** Factory method:  Rotate the selected argument range.
     *  Return null if this is not possible.
     */
    static MethodHandle makeRotateArguments(MethodType newType, MethodHandle target,
                int firstArg, int argCount, int rotateBy) {
        rotateBy = positiveRotation(argCount, rotateBy);
        if (!canRotateArguments(newType, target.type(), firstArg, argCount, rotateBy))
            return null;
        throw new NotYetImplementedError();
    }

    /** Can an adapter spread an argument to convert the target to newType? */
    static boolean canSpreadArguments(MethodType newType, MethodType targetType,
                Class<?> spreadArgType, int spreadArgPos, int spreadArgCount) {
        if (diffReturnTypes(newType, targetType, false) != 0)
            return false;
        int nptypes = newType.parameterCount();
        // parameter types must be the same up to the spread point
        if (spreadArgPos != 0 && diffParamTypes(newType, 0, targetType, 0, spreadArgPos, false) != 0)
            return false;
        int afterPos = spreadArgPos + spreadArgCount;
        int afterCount = nptypes - (spreadArgPos + 1);
        if (spreadArgPos < 0 || spreadArgPos >= nptypes ||
            spreadArgCount < 0 ||
            targetType.parameterCount() != afterPos + afterCount)
            return false;
        // parameter types after the spread point must also be the same
        if (afterCount != 0 && diffParamTypes(newType, spreadArgPos+1, targetType, afterPos, afterCount, false) != 0)
            return false;
        // match the array element type to the spread arg types
        Class<?> rawSpreadArgType = newType.parameterType(spreadArgPos);
        if (rawSpreadArgType != spreadArgType && !canCheckCast(rawSpreadArgType, spreadArgType))
            return false;
        for (int i = 0; i < spreadArgCount; i++) {
            Class<?> src = VerifyType.spreadArgElementType(spreadArgType, i);
            Class<?> dst = targetType.parameterType(spreadArgPos + i);
            if (src == null || !canConvertArgument(src, dst, 1))
                return false;
        }
        return true;
    }


    /** Factory method:  Spread selected argument. */
    static native MethodHandle makeSpreadArguments(MethodType newType, MethodHandle target, Class<?> spreadArgType, int spreadArgPos, int spreadArgCount);

    /** Can an adapter collect a series of arguments, replacing them by zero or one results? */
    static boolean canCollectArguments(MethodType targetType,
                MethodType collectorType, int collectArgPos, boolean retainOriginalArgs) {
        int collectArgCount = collectorType.parameterCount();
        Class<?> rtype = collectorType.returnType();
        assert(rtype == void.class || targetType.parameterType(collectArgPos) == rtype)
                // [(Object)Object[], (Object[])Object[], 0, 1]
                : Arrays.asList(targetType, collectorType, collectArgPos, collectArgCount)
                ;
        return true;
    }

    /** Factory method:  Collect or filter selected argument(s). */
    static native MethodHandle makeCollectArguments(MethodHandle target, MethodHandle collector, int collectArgPos, boolean retainOriginalArgs);
}
