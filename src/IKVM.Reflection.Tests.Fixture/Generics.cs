using System;
using System.Collections.Generic;

namespace IKVM.Reflection.Tests.Fixture
{

    /// <summary>
    /// A generic class with fields, constructors, a property, methods and nested types that use its generic parameter.
    /// </summary>
    /// <typeparam name="T">The generic parameter.</typeparam>
    public class GenericClass<T>
    {

        /// <summary>
        /// A field of the generic parameter type.
        /// </summary>
        public T Field;

        /// <summary>
        /// A field of an array of the generic parameter type.
        /// </summary>
        public T[] ArrayField;

        /// <summary>
        /// A field of a generic instance over the generic parameter.
        /// </summary>
        public List<T> ListField;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public GenericClass() { }

        /// <summary>
        /// Initializes a new instance with a value of the generic parameter type.
        /// </summary>
        /// <param name="value">The value, not used.</param>
        public GenericClass(T value) { }

        /// <summary>
        /// Gets or sets a value of the generic parameter type.
        /// </summary>
        public T Property { get; set; }

        /// <summary>
        /// A method that takes and returns the generic parameter type.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns><paramref name="value"/>.</returns>
        public T Method(T value) => value;

        /// <summary>
        /// A generic method in a generic class.
        /// </summary>
        /// <typeparam name="TMethod">The method generic parameter.</typeparam>
        /// <param name="value">A value of the class generic parameter type, not used.</param>
        /// <param name="other">The value to return.</param>
        /// <returns><paramref name="other"/>.</returns>
        public TMethod GenericMethod<TMethod>(T value, TMethod other) => other;

        /// <summary>
        /// A generic method whose generic parameter is constrained to the class generic parameter.
        /// </summary>
        /// <typeparam name="TMethod">The method generic parameter.</typeparam>
        public void GenericMethodWithConstraint<TMethod>() where TMethod : T { }

        /// <summary>
        /// A class nested in a generic class, which inherits its generic parameter.
        /// </summary>
        public class NestedInGeneric
        {

            /// <summary>
            /// A field of the inherited generic parameter type.
            /// </summary>
            public T Field;

        }

        /// <summary>
        /// A generic class nested in a generic class, with a generic parameter of its own.
        /// </summary>
        /// <typeparam name="TNested">The generic parameter of the nested class.</typeparam>
        public class GenericNestedInGeneric<TNested>
        {

            /// <summary>
            /// A field of the enclosing class generic parameter type.
            /// </summary>
            public T OuterField;

            /// <summary>
            /// A field of the nested class generic parameter type.
            /// </summary>
            public TNested InnerField;

            /// <summary>
            /// A field of a generic instance over both generic parameters.
            /// </summary>
            public Dictionary<T, TNested> Map;

        }

    }

    /// <summary>
    /// A generic class that derives from a generic instance over its generic parameters.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class GenericClassWithTwoParameters<TKey, TValue> : Dictionary<TKey, TValue>
    {

    }

    /// <summary>
    /// A generic class with one generic parameter for each kind of constraint.
    /// </summary>
    /// <typeparam name="TClass">A parameter constrained to reference types.</typeparam>
    /// <typeparam name="TStruct">A parameter constrained to non-nullable value types.</typeparam>
    /// <typeparam name="TNew">A parameter constrained to types with a public parameterless constructor.</typeparam>
    /// <typeparam name="TInterface">A parameter constrained to interfaces, one of which refers to the parameter itself.</typeparam>
    /// <typeparam name="TBase">A parameter constrained to a base class.</typeparam>
    /// <typeparam name="TDerived">A parameter constrained to another generic parameter.</typeparam>
    /// <typeparam name="TUnmanaged">A parameter constrained to unmanaged types.</typeparam>
    /// <typeparam name="TNotNull">A parameter constrained to non-nullable types.</typeparam>
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

    /// <summary>
    /// A generic interface with a contravariant and a covariant generic parameter.
    /// </summary>
    /// <typeparam name="TIn">The contravariant generic parameter.</typeparam>
    /// <typeparam name="TOut">The covariant generic parameter.</typeparam>
    public interface IVariant<in TIn, out TOut>
    {

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        TOut Convert(TIn value);

    }

    /// <summary>
    /// A class that derives from a closed generic instance.
    /// </summary>
    public class ClosedGenericBase : GenericClass<string>
    {

    }

    /// <summary>
    /// A generic class that derives from and implements generic instances over its generic parameter.
    /// </summary>
    /// <typeparam name="T">The generic parameter.</typeparam>
    public class OpenGenericBase<T> : GenericClass<List<T>>, IEquatable<T>, IVariant<T, T>
    {

        /// <summary>
        /// Implements <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <param name="other">The value to compare with, not used.</param>
        /// <returns><see langword="false"/>.</returns>
        public bool Equals(T other) => false;

        /// <summary>
        /// Implements <see cref="IVariant{TIn, TOut}"/>.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns><paramref name="value"/>.</returns>
        public T Convert(T value) => value;

    }

    /// <summary>
    /// A generic class whose generic parameter is constrained to the class itself, and which implements an interface
    /// over itself.
    /// </summary>
    /// <typeparam name="T">The generic parameter, constrained to the class itself.</typeparam>
    public class SelfReferencingGeneric<T> : IComparable<SelfReferencingGeneric<T>> where T : SelfReferencingGeneric<T>
    {

        /// <summary>
        /// Implements <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="other">The value to compare with, not used.</param>
        /// <returns>Zero.</returns>
        public int CompareTo(SelfReferencingGeneric<T> other) => 0;

    }

    /// <summary>
    /// Generic methods with signatures of each kind.
    /// </summary>
    public static class GenericMethods
    {

        /// <summary>
        /// A generic method that returns its argument.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns><paramref name="value"/>.</returns>
        public static T Identity<T>(T value) => value;

        /// <summary>
        /// A generic method with two generic parameters and a generic delegate parameter.
        /// </summary>
        /// <typeparam name="TIn">The type of the value.</typeparam>
        /// <typeparam name="TOut">The type of the result.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="converter">The conversion.</param>
        /// <returns>The converted value.</returns>
        public static TOut Convert<TIn, TOut>(TIn value, Func<TIn, TOut> converter) => converter(value);

        /// <summary>
        /// A generic method with ref, out and in parameters of its generic parameter type.
        /// </summary>
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <param name="value">A ref parameter.</param>
        /// <param name="result">An out parameter, set to <paramref name="value"/>.</param>
        /// <param name="input">An in parameter.</param>
        public static void ByRef<T>(ref T value, out T result, in T input) { result = value; }

        /// <summary>
        /// A generic method that returns an array of its generic parameter type.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="length">The length of the array.</param>
        /// <returns>A new array.</returns>
        public static T[] MakeArray<T>(int length) => new T[length];

        /// <summary>
        /// A generic method that returns nested generic instances over its generic parameters.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The element type of the values.</typeparam>
        /// <returns><see langword="null"/>.</returns>
        public static IEnumerable<KeyValuePair<TKey, List<TValue>>> Nested<TKey, TValue>() => null;

        /// <summary>
        /// A generic method whose generic parameter is constrained to value types and to an interface over itself.
        /// </summary>
        /// <typeparam name="T">The constrained generic parameter.</typeparam>
        /// <param name="value">A value, not used.</param>
        public static void Constrained<T>(T value) where T : struct, IComparable<T> { }

        /// <summary>
        /// A non-generic overload.
        /// </summary>
        /// <param name="value">A value, not used.</param>
        /// <returns>Zero.</returns>
        public static int Overloaded(int value) => 0;

        /// <summary>
        /// A generic overload with one generic parameter.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">A value, not used.</param>
        /// <returns>Zero.</returns>
        public static int Overloaded<T>(T value) => 0;

        /// <summary>
        /// A generic overload with two generic parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <param name="value1">A value, not used.</param>
        /// <param name="value2">A value, not used.</param>
        /// <returns>Zero.</returns>
        public static int Overloaded<T1, T2>(T1 value1, T2 value2) => 0;

    }

    /// <summary>
    /// A generic class with abstract and virtual methods that use its generic parameter, for base definitions through
    /// generic instances.
    /// </summary>
    /// <typeparam name="T">The generic parameter.</typeparam>
    public abstract class GenericVirtualBase<T>
    {

        /// <summary>
        /// An abstract method that returns the generic parameter type.
        /// </summary>
        /// <returns>A value.</returns>
        public abstract T Get();

        /// <summary>
        /// A virtual method that takes the generic parameter type.
        /// </summary>
        /// <param name="value">A value, not used.</param>
        public virtual void Set(T value) { }

        /// <summary>
        /// An abstract generic method in a generic class.
        /// </summary>
        /// <typeparam name="TMethod">The method generic parameter.</typeparam>
        /// <param name="value">A value.</param>
        /// <returns>A value.</returns>
        public abstract TMethod Generic<TMethod>(T value);

    }

    /// <summary>
    /// A class that overrides the methods of a closed generic instance of <see cref="GenericVirtualBase{T}"/>.
    /// </summary>
    public class GenericVirtualDerived : GenericVirtualBase<int>
    {

        /// <summary>
        /// Overrides an abstract method of a closed generic base.
        /// </summary>
        /// <returns>Zero.</returns>
        public override int Get() => 0;

        /// <summary>
        /// Overrides a virtual method of a closed generic base.
        /// </summary>
        /// <param name="value">A value, not used.</param>
        public override void Set(int value) { }

        /// <summary>
        /// Overrides an abstract generic method of a closed generic base.
        /// </summary>
        /// <typeparam name="TMethod">The method generic parameter.</typeparam>
        /// <param name="value">A value, not used.</param>
        /// <returns>The default value of <typeparamref name="TMethod"/>.</returns>
        public override TMethod Generic<TMethod>(int value) => default;

    }

    /// <summary>
    /// A generic class that overrides the methods of an open generic instance of <see cref="GenericVirtualBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the base generic argument.</typeparam>
    public class GenericVirtualDerivedOpen<T> : GenericVirtualBase<T[]>
    {

        /// <summary>
        /// Overrides an abstract method of an open generic base.
        /// </summary>
        /// <returns><see langword="null"/>.</returns>
        public override T[] Get() => null;

        /// <summary>
        /// Overrides an abstract generic method of an open generic base.
        /// </summary>
        /// <typeparam name="TMethod">The method generic parameter.</typeparam>
        /// <param name="value">A value, not used.</param>
        /// <returns>The default value of <typeparamref name="TMethod"/>.</returns>
        public override TMethod Generic<TMethod>(T[] value) => default;

    }

}
