using System;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    public static partial class ManagedSignatureExtensions
    {
        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList1<ManagedSignatureData> ToDataList1(this in ReadOnlyFixedValueList1<ManagedSignature> sigs)
        {
            var l = new FixedValueList1<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList1<ManagedSignatureData> ToDataList1(this ReadOnlySpan<ManagedSignature> sigs)
        {
            var l = new FixedValueList1<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList1<ManagedSignatureData> ToDataList1(this IReadOnlyList<ManagedSignature> sigs)
        {
            var l = new FixedValueList1<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList1<ManagedSignatureData> ToDataList1(this ManagedSignature[] sigs)
        {
            var l = new FixedValueList1<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }
        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList2<ManagedSignatureData> ToDataList2(this in ReadOnlyFixedValueList2<ManagedSignature> sigs)
        {
            var l = new FixedValueList2<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList2<ManagedSignatureData> ToDataList2(this ReadOnlySpan<ManagedSignature> sigs)
        {
            var l = new FixedValueList2<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList2<ManagedSignatureData> ToDataList2(this IReadOnlyList<ManagedSignature> sigs)
        {
            var l = new FixedValueList2<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList2<ManagedSignatureData> ToDataList2(this ManagedSignature[] sigs)
        {
            var l = new FixedValueList2<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }
        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList3<ManagedSignatureData> ToDataList3(this in ReadOnlyFixedValueList3<ManagedSignature> sigs)
        {
            var l = new FixedValueList3<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList3<ManagedSignatureData> ToDataList3(this ReadOnlySpan<ManagedSignature> sigs)
        {
            var l = new FixedValueList3<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList3<ManagedSignatureData> ToDataList3(this IReadOnlyList<ManagedSignature> sigs)
        {
            var l = new FixedValueList3<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList3<ManagedSignatureData> ToDataList3(this ManagedSignature[] sigs)
        {
            var l = new FixedValueList3<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }
        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList4<ManagedSignatureData> ToDataList4(this in ReadOnlyFixedValueList4<ManagedSignature> sigs)
        {
            var l = new FixedValueList4<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList4<ManagedSignatureData> ToDataList4(this ReadOnlySpan<ManagedSignature> sigs)
        {
            var l = new FixedValueList4<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList4<ManagedSignatureData> ToDataList4(this IReadOnlyList<ManagedSignature> sigs)
        {
            var l = new FixedValueList4<ManagedSignatureData>(sigs.Count);
            for (int i = 0; i < sigs.Count; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }

        /// <summary>
        /// Unpacks the data structures from multiple signatures.
        /// </summary>
        /// <param name="sigs"></param>
        /// <returns></returns>
        internal static ReadOnlyFixedValueList4<ManagedSignatureData> ToDataList4(this ManagedSignature[] sigs)
        {
            var l = new FixedValueList4<ManagedSignatureData>(sigs.Length);
            for (int i = 0; i < sigs.Length; i++)
                l[i] = sigs[i].data;

            return l.AsReadOnly();
        }


        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedTypeSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedTypeSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedTypeSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedTypeSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedTypeSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedTypeSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPrimitiveTypeSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedPrimitiveTypeSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPrimitiveTypeSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedPrimitiveTypeSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedPrimitiveTypeSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedSZArraySignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedSZArraySignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedSZArraySignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedSZArraySignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedSZArraySignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedSZArraySignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedArraySignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedArraySignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedArraySignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedArraySignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedArraySignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedArraySignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedByRefSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedByRefSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedByRefSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedByRefSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedByRefSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedByRefSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericConstraintSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericConstraintSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericConstraintSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericConstraintSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericConstraintSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericConstraintSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericTypeParameterSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericTypeParameterSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericTypeParameterSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericTypeParameterSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericTypeParameterSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedGenericMethodParameterSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedGenericMethodParameterSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedGenericMethodParameterSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedGenericMethodParameterSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedGenericMethodParameterSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedModifiedSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedModifiedSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedModifiedSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedModifiedSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedModifiedSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedModifiedSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedPointerSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedPointerSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedPointerSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedPointerSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedPointerSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedPointerSignature self) => ManagedPointerSignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <returns></returns>
        public static ManagedSZArraySignature CreateArray(this in ManagedFunctionPointerSignature self) => ManagedSZArraySignature.Create(self.data);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, ReadOnlyFixedValueList2<int> sizes, ReadOnlyFixedValueList2<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, sizes, lowerBounds);

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, IReadOnlyList<int> sizes, IReadOnlyList<int> lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new array type with this type as the element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public static ManagedArraySignature CreateArray(this in ManagedFunctionPointerSignature self, int rank, int[] sizes, int[] lowerBounds) => ManagedArraySignature.Create(self.data, rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)));

        /// <summary>
        /// Creates a new by-ref type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedByRefSignature CreateByRef(this in ManagedFunctionPointerSignature self) => ManagedByRefSignature.Create(self.data);

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, ReadOnlyFixedValueList4<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, IReadOnlyList<ManagedSignature> genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new generic type with this type as the base type.
        /// </summary>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        public static ManagedGenericSignature CreateGeneric(this in ManagedFunctionPointerSignature self, params ManagedSignature[] genericParameters) => ManagedGenericSignature.Create(self.data, ToDataList4(genericParameters));

        /// <summary>
        /// Creates a new modified type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedModifiedSignature CreateModified(this in ManagedFunctionPointerSignature self, in ManagedSignature modifier, bool required) => ManagedModifiedSignature.Create(self.data, modifier.data, required);

        /// <summary>
        /// Creates a new pointer type with this type as the base type.
        /// </summary>
        /// <returns></returns>
        public static ManagedPointerSignature CreatePointer(this in ManagedFunctionPointerSignature self) => ManagedPointerSignature.Create(self.data);

    }

}