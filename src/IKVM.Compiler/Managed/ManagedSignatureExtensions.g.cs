using System;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    internal static partial class ManagedSignatureExtensions
    {

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList1(this in FixedValueList1<ManagedSignature> sigs, out FixedValueList1<ManagedSignatureData> result)
        {
            result = new FixedValueList1<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList1(this ReadOnlySpan<ManagedSignature> sigs, out FixedValueList1<ManagedSignatureData> result)
        {
            result = new FixedValueList1<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList1(this IReadOnlyList<ManagedSignature> sigs, out FixedValueList1<ManagedSignatureData> result)
        {
            result = new FixedValueList1<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList1(this ManagedSignature[] sigs, out FixedValueList1<ManagedSignatureData> result)
        {
            result = new FixedValueList1<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList2(this in FixedValueList2<ManagedSignature> sigs, out FixedValueList2<ManagedSignatureData> result)
        {
            result = new FixedValueList2<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList2(this ReadOnlySpan<ManagedSignature> sigs, out FixedValueList2<ManagedSignatureData> result)
        {
            result = new FixedValueList2<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList2(this IReadOnlyList<ManagedSignature> sigs, out FixedValueList2<ManagedSignatureData> result)
        {
            result = new FixedValueList2<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList2(this ManagedSignature[] sigs, out FixedValueList2<ManagedSignatureData> result)
        {
            result = new FixedValueList2<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList3(this in FixedValueList3<ManagedSignature> sigs, out FixedValueList3<ManagedSignatureData> result)
        {
            result = new FixedValueList3<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList3(this ReadOnlySpan<ManagedSignature> sigs, out FixedValueList3<ManagedSignatureData> result)
        {
            result = new FixedValueList3<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList3(this IReadOnlyList<ManagedSignature> sigs, out FixedValueList3<ManagedSignatureData> result)
        {
            result = new FixedValueList3<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList3(this ManagedSignature[] sigs, out FixedValueList3<ManagedSignatureData> result)
        {
            result = new FixedValueList3<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList4(this in FixedValueList4<ManagedSignature> sigs, out FixedValueList4<ManagedSignatureData> result)
        {
            result = new FixedValueList4<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList4(this ReadOnlySpan<ManagedSignature> sigs, out FixedValueList4<ManagedSignatureData> result)
        {
            result = new FixedValueList4<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList4(this IReadOnlyList<ManagedSignature> sigs, out FixedValueList4<ManagedSignatureData> result)
        {
            result = new FixedValueList4<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                result[i] = sigs[i].data;
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static void ToDataList4(this ManagedSignature[] sigs, out FixedValueList4<ManagedSignatureData> result)
        {
            result = new FixedValueList4<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                result[i] = sigs[i].data;
        }


        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedTypeSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedTypeSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedTypeSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedTypeSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedPrimitiveTypeSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedPrimitiveTypeSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedPrimitiveTypeSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedSZArraySignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedSZArraySignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedSZArraySignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedSZArraySignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedArraySignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedArraySignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedArraySignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedArraySignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedByRefSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedByRefSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedByRefSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedByRefSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericConstraintSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericConstraintSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericConstraintSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericConstraintSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericTypeParameterSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericTypeParameterSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericTypeParameterSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericMethodParameterSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericMethodParameterSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericMethodParameterSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedModifiedSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedModifiedSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedModifiedSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedModifiedSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedPointerSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedPointerSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedPointerSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedPointerSignature self) => new ManagedPointerSignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedFunctionPointerSignature self) => new ManagedSZArraySignature(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, FixedValueList2<int> sizes, FixedValueList2<int> lowerBounds) => new ManagedArraySignature(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, int[] sizes, int[] lowerBounds) => new ManagedArraySignature(self.data, rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedFunctionPointerSignature self) => new ManagedByRefSignature(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, FixedValueList4<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, IReadOnlyList<ManagedSignature> genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, params ManagedSignature[] genericParameters)
        {
            ToDataList4(genericParameters, out var genericParameters_);
            return new ManagedGenericSignature(self.data, genericParameters_);
        }

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedFunctionPointerSignature self, in ManagedSignature modifier, bool required) => new ManagedModifiedSignature(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedFunctionPointerSignature self) => new ManagedPointerSignature(self.data);

    }

}