using System;
using System.Collections.Generic;

namespace IKVM.Reflection.Tests.Fixture
{

    public class GenericClass<T>
    {

        public T Field;

        public T[] ArrayField;

        public List<T> ListField;

        public GenericClass() { }

        public GenericClass(T value) { }

        public T Property { get; set; }

        public T Method(T value) => value;

        public TMethod GenericMethod<TMethod>(T value, TMethod other) => other;

        public void GenericMethodWithConstraint<TMethod>() where TMethod : T { }

        public class NestedInGeneric
        {

            public T Field;

        }

        public class GenericNestedInGeneric<TNested>
        {

            public T OuterField;

            public TNested InnerField;

            public Dictionary<T, TNested> Map;

        }

    }

    public class GenericClassWithTwoParameters<TKey, TValue> : Dictionary<TKey, TValue>
    {

    }

    public class GenericConstraints<TClass, TStruct, TNew, TInterface, TBase, TDerived, TUnmanaged, TNotNull>
        where TClass : class
        where TStruct : struct
        where TNew : new()
        where TInterface : IDisposable, IComparable<TInterface>
        where TBase : PlainClass
        where TDerived : TBase
        where TUnmanaged : unmanaged
        where TNotNull : notnull
    {

    }

    public interface IVariant<in TIn, out TOut>
    {

        TOut Convert(TIn value);

    }

    public class ClosedGenericBase : GenericClass<string>
    {

    }

    public class OpenGenericBase<T> : GenericClass<List<T>>, IEquatable<T>, IVariant<T, T>
    {

        public bool Equals(T other) => false;

        public T Convert(T value) => value;

    }

    public class SelfReferencingGeneric<T> : IComparable<SelfReferencingGeneric<T>> where T : SelfReferencingGeneric<T>
    {

        public int CompareTo(SelfReferencingGeneric<T> other) => 0;

    }

    public static class GenericMethods
    {

        public static T Identity<T>(T value) => value;

        public static TOut Convert<TIn, TOut>(TIn value, Func<TIn, TOut> converter) => converter(value);

        public static void ByRef<T>(ref T value, out T result, in T input) { result = value; }

        public static T[] MakeArray<T>(int length) => new T[length];

        public static IEnumerable<KeyValuePair<TKey, List<TValue>>> Nested<TKey, TValue>() => null;

        public static void Constrained<T>(T value) where T : struct, IComparable<T> { }

        public static int Overloaded(int value) => 0;

        public static int Overloaded<T>(T value) => 0;

        public static int Overloaded<T1, T2>(T1 value1, T2 value2) => 0;

    }

    public abstract class GenericVirtualBase<T>
    {

        public abstract T Get();

        public virtual void Set(T value) { }

        public abstract TMethod Generic<TMethod>(T value);

    }

    public class GenericVirtualDerived : GenericVirtualBase<int>
    {

        public override int Get() => 0;

        public override void Set(int value) { }

        public override TMethod Generic<TMethod>(int value) => default;

    }

    public class GenericVirtualDerivedOpen<T> : GenericVirtualBase<T[]>
    {

        public override T[] Get() => null;

        public override TMethod Generic<TMethod>(T[] value) => default;

    }

}
