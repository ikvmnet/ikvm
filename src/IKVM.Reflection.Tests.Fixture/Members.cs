using System;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Tests.Fixture
{

    /// <summary>
    /// Properties of each kind.
    /// </summary>
    public class Properties
    {

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public int ReadWrite { get; set; }

        /// <summary>
        /// Gets a value, with no setter.
        /// </summary>
        public int ReadOnly { get; }

        /// <summary>
        /// Sets a value, with no getter.
        /// </summary>
        public int WriteOnly { set { } }

        /// <summary>
        /// Gets a value, with a private setter.
        /// </summary>
        public int PrivateSetter { get; private set; }

        /// <summary>
        /// Gets or sets a static value.
        /// </summary>
        public static int Static { get; set; }

        /// <summary>
        /// Gets or sets a value, with virtual accessors.
        /// </summary>
        public virtual int Virtual { get; set; }

        /// <summary>
        /// Gets or sets a value by index.
        /// </summary>
        /// <param name="index">The index, not used.</param>
        public int this[int index] { get => 0; set { } }

        /// <summary>
        /// Gets a value by two indexes.
        /// </summary>
        /// <param name="key">The value to return.</param>
        /// <param name="index">The index, not used.</param>
        public string this[string key, int index] => key;

        /// <summary>
        /// Gets a value by reference.
        /// </summary>
        public ref int RefReturning => throw null;

        /// <summary>
        /// Gets a value by read-only reference.
        /// </summary>
        public ref readonly int RefReadOnlyReturning => throw null;

        /// <summary>
        /// Gets a value that can be set only during initialization, whose setter carries a required custom modifier.
        /// </summary>
        public int InitOnly { get; init; }

    }

    /// <summary>
    /// A class that overrides and hides properties of its base class.
    /// </summary>
    public class DerivedProperties : Properties
    {

        /// <summary>
        /// Overrides only the getter of a virtual property.
        /// </summary>
        public override int Virtual { get => 1; }

        /// <summary>
        /// Gets or sets a value, hiding the property of the same name in the base class.
        /// </summary>
        public new int ReadWrite { get; set; }

    }

    /// <summary>
    /// Events of each kind.
    /// </summary>
    public class Events
    {

        /// <summary>
        /// A field-like event.
        /// </summary>
        public event EventHandler FieldLike;

        /// <summary>
        /// A field-like event of a generic delegate type.
        /// </summary>
        public event EventHandler<EventArgs> Generic;

        /// <summary>
        /// A static event.
        /// </summary>
        public static event Action Static;

        /// <summary>
        /// An event with custom accessors.
        /// </summary>
        public event Action Custom { add { } remove { } }

        /// <summary>
        /// A virtual event.
        /// </summary>
        public virtual event EventHandler Virtual;

    }

    /// <summary>
    /// A class that overrides an event of its base class.
    /// </summary>
    public class DerivedEvents : Events
    {

        /// <summary>
        /// Overrides a virtual event.
        /// </summary>
        public override event EventHandler Virtual;

    }

    /// <summary>
    /// Fields of each kind, visibility and type.
    /// </summary>
    public class Fields
    {

        /// <summary>
        /// An instance field.
        /// </summary>
        public int Instance;

        /// <summary>
        /// A static field.
        /// </summary>
        public static int Static;

        /// <summary>
        /// A read-only field.
        /// </summary>
        public readonly int ReadOnly;

        /// <summary>
        /// A static read-only field.
        /// </summary>
        public static readonly int StaticReadOnly = 1;

        /// <summary>
        /// A volatile field, which carries a required custom modifier.
        /// </summary>
        public volatile int Volatile;

        /// <summary>
        /// A constant field.
        /// </summary>
        public const int Const = 1;

        /// <summary>
        /// A private field.
        /// </summary>
        private int Private;

        /// <summary>
        /// A protected field.
        /// </summary>
        protected int Protected;

        /// <summary>
        /// An internal field.
        /// </summary>
        internal int Internal;

        /// <summary>
        /// A protected internal field.
        /// </summary>
        protected internal int ProtectedInternal;

        /// <summary>
        /// A private protected field.
        /// </summary>
        private protected int PrivateProtected;

        /// <summary>
        /// A field of a single-dimensional array type.
        /// </summary>
        public int[] Array;

        /// <summary>
        /// A field of a multidimensional array type.
        /// </summary>
        public int[,] MultiDimensionalArray;

        /// <summary>
        /// A field of a jagged array type.
        /// </summary>
        public int[][] JaggedArray;

        /// <summary>
        /// A field of a pointer type.
        /// </summary>
        public unsafe int* Pointer;

        /// <summary>
        /// A field of a pointer to pointer type.
        /// </summary>
        public unsafe int** PointerToPointer;

        /// <summary>
        /// A field of a void pointer type.
        /// </summary>
        public unsafe void* VoidPointer;

        /// <summary>
        /// A field of type <see cref="object"/>.
        /// </summary>
        public object Object;

        /// <summary>
        /// A field of type <see langword="dynamic"/>, which is <see cref="object"/> with a custom attribute.
        /// </summary>
        public dynamic Dynamic;

        /// <summary>
        /// A field of a tuple type.
        /// </summary>
        public (int, string) Tuple;

        /// <summary>
        /// A field of a tuple type with element names, which are stored in a custom attribute.
        /// </summary>
        public (int First, string Second) NamedTuple;

        /// <summary>
        /// A field of a nullable value type.
        /// </summary>
        public int? Nullable;

    }

    /// <summary>
    /// Methods with signatures and implementation flags of each kind.
    /// </summary>
    public class Methods
    {

        /// <summary>
        /// A method without parameters.
        /// </summary>
        public void NoParameters() { }

        /// <summary>
        /// A method with parameters of several types.
        /// </summary>
        /// <param name="a">An integer parameter.</param>
        /// <param name="b">A string parameter.</param>
        /// <param name="c">An object parameter.</param>
        /// <returns>Zero.</returns>
        public int Parameters(int a, string b, object c) => 0;

        /// <summary>
        /// A method with ref, out and in parameters.
        /// </summary>
        /// <param name="a">A ref parameter.</param>
        /// <param name="b">An out parameter, set to zero.</param>
        /// <param name="c">An in parameter.</param>
        public void RefParameters(ref int a, out int b, in int c) { b = 0; }

        /// <summary>
        /// A method that returns by reference.
        /// </summary>
        /// <param name="a">The variable to return a reference to.</param>
        /// <returns>A reference to <paramref name="a"/>.</returns>
        public ref int RefReturn(ref int a) => ref a;

        /// <summary>
        /// A method that returns by read-only reference.
        /// </summary>
        /// <param name="a">The variable to return a reference to.</param>
        /// <returns>A read-only reference to <paramref name="a"/>.</returns>
        public ref readonly int RefReadOnlyReturn(in int a) => ref a;

        /// <summary>
        /// A method with a parameter array.
        /// </summary>
        /// <param name="values">The parameter array.</param>
        public void Params(params object[] values) { }

        /// <summary>
        /// A method with optional parameters that have default values of several types.
        /// </summary>
        /// <param name="a">An integer with a default value.</param>
        /// <param name="b">A string with a default value.</param>
        /// <param name="c">An object with a <see langword="null"/> default value.</param>
        /// <param name="d">An enum with a default value.</param>
        public void Optional(int a = 1, string b = "b", object c = null, PlainEnum d = PlainEnum.B) { }

        /// <summary>
        /// A method with optional nullable parameters.
        /// </summary>
        /// <param name="a">A nullable integer with a <see langword="null"/> default value.</param>
        /// <param name="b">A nullable integer with a default value.</param>
        public void OptionalNullable(int? a = null, int? b = 1) { }

        /// <summary>
        /// A method with an optional decimal parameter, whose default value is stored in a custom attribute.
        /// </summary>
        /// <param name="a">A decimal with a default value.</param>
        public void OptionalDecimal(decimal a = 1.5m) { }

        /// <summary>
        /// A method with an optional date and time parameter, whose default value is stored in a custom attribute.
        /// </summary>
        /// <param name="a">A date and time with a default value.</param>
        public void OptionalDateTime([System.Runtime.InteropServices.Optional, DateTimeConstant(630822816000000000)] DateTime a) { }

        /// <summary>
        /// A method with parameters of each kind of array type.
        /// </summary>
        /// <param name="a">A single-dimensional array.</param>
        /// <param name="b">A two-dimensional array.</param>
        /// <param name="c">A jagged array.</param>
        /// <param name="d">A three-dimensional array.</param>
        public void ArrayParameters(int[] a, int[,] b, int[][] c, int[,,] d) { }

        /// <summary>
        /// A method with parameters of pointer types.
        /// </summary>
        /// <param name="a">A pointer.</param>
        /// <param name="b">A void pointer.</param>
        /// <param name="c">A pointer to pointer.</param>
        public unsafe void PointerParameters(int* a, void* b, int** c) { }

        /// <summary>
        /// A method with a variable argument list.
        /// </summary>
        /// <param name="a">A fixed parameter.</param>
        public void VarArgs(int a, __arglist) { }

        /// <summary>
        /// A static method.
        /// </summary>
        public static void StaticMethod() { }

        /// <summary>
        /// A virtual method.
        /// </summary>
        public virtual void VirtualMethod() { }

        /// <summary>
        /// A method that must not be inlined.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void NoInlining() { }

        /// <summary>
        /// A synchronized method.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Synchronized() { }

#if NET8_0_OR_GREATER

        // only the .NET 8 runtime reports function pointer types; earlier runtimes report System.IntPtr

        /// <summary>
        /// A method that takes and returns managed function pointers.
        /// </summary>
        /// <param name="a">A managed function pointer.</param>
        /// <returns><see langword="null"/>.</returns>
        public unsafe delegate*<int, int> ManagedFunctionPointer(delegate*<int, void> a) => null;

        /// <summary>
        /// A method that takes and returns unmanaged function pointers with calling conventions.
        /// </summary>
        /// <param name="a">An unmanaged function pointer.</param>
        /// <returns><see langword="null"/>.</returns>
        public unsafe delegate* unmanaged[Cdecl]<int, int> UnmanagedFunctionPointer(delegate* unmanaged[Stdcall]<void> a) => null;

#endif

    }

    /// <summary>
    /// A class with operators and conversions, which are special-name static methods.
    /// </summary>
    public class Operators
    {

        /// <summary>
        /// An addition operator.
        /// </summary>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand, not used.</param>
        /// <returns><paramref name="a"/>.</returns>
        public static Operators operator +(Operators a, Operators b) => a;

        /// <summary>
        /// An equality operator.
        /// </summary>
        /// <param name="a">The left operand, not used.</param>
        /// <param name="b">The right operand, not used.</param>
        /// <returns><see langword="true"/>.</returns>
        public static bool operator ==(Operators a, Operators b) => true;

        /// <summary>
        /// An inequality operator.
        /// </summary>
        /// <param name="a">The left operand, not used.</param>
        /// <param name="b">The right operand, not used.</param>
        /// <returns><see langword="false"/>.</returns>
        public static bool operator !=(Operators a, Operators b) => false;

        /// <summary>
        /// An implicit conversion.
        /// </summary>
        /// <param name="a">The value to convert, not used.</param>
        /// <returns>Zero.</returns>
        public static implicit operator int(Operators a) => 0;

        /// <summary>
        /// An explicit conversion.
        /// </summary>
        /// <param name="a">The value to convert, not used.</param>
        /// <returns><see langword="null"/>.</returns>
        public static explicit operator Operators(int a) => null;

        /// <summary>
        /// Overrides <see cref="object.Equals(object)"/>, as the equality operators require.
        /// </summary>
        /// <param name="obj">The object to compare with, not used.</param>
        /// <returns><see langword="false"/>.</returns>
        public override bool Equals(object obj) => false;

        /// <summary>
        /// Overrides <see cref="object.GetHashCode"/>, as the equality operators require.
        /// </summary>
        /// <returns>Zero.</returns>
        public override int GetHashCode() => 0;

    }

}
