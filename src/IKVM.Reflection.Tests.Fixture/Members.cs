using System;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Tests.Fixture
{

    public class Properties
    {

        public int ReadWrite { get; set; }

        public int ReadOnly { get; }

        public int WriteOnly { set { } }

        public int PrivateSetter { get; private set; }

        public static int Static { get; set; }

        public virtual int Virtual { get; set; }

        public int this[int index] { get => 0; set { } }

        public string this[string key, int index] => key;

        public ref int RefReturning => throw null;

        public ref readonly int RefReadOnlyReturning => throw null;

        public int InitOnly { get; init; }

    }

    public class DerivedProperties : Properties
    {

        public override int Virtual { get => 1; }

        public new int ReadWrite { get; set; }

    }

    public class Events
    {

        public event EventHandler FieldLike;

        public event EventHandler<EventArgs> Generic;

        public static event Action Static;

        public event Action Custom { add { } remove { } }

        public virtual event EventHandler Virtual;

    }

    public class DerivedEvents : Events
    {

        public override event EventHandler Virtual;

    }

    public class Fields
    {

        public int Instance;

        public static int Static;

        public readonly int ReadOnly;

        public static readonly int StaticReadOnly = 1;

        public volatile int Volatile;

        public const int Const = 1;

        private int Private;

        protected int Protected;

        internal int Internal;

        protected internal int ProtectedInternal;

        private protected int PrivateProtected;

        public int[] Array;

        public int[,] MultiDimensionalArray;

        public int[][] JaggedArray;

        public unsafe int* Pointer;

        public unsafe int** PointerToPointer;

        public unsafe void* VoidPointer;

        public object Object;

        public dynamic Dynamic;

        public (int, string) Tuple;

        public (int First, string Second) NamedTuple;

        public int? Nullable;

    }

    public class Methods
    {

        public void NoParameters() { }

        public int Parameters(int a, string b, object c) => 0;

        public void RefParameters(ref int a, out int b, in int c) { b = 0; }

        public ref int RefReturn(ref int a) => ref a;

        public ref readonly int RefReadOnlyReturn(in int a) => ref a;

        public void Params(params object[] values) { }

        public void Optional(int a = 1, string b = "b", object c = null, PlainEnum d = PlainEnum.B) { }

        public void OptionalNullable(int? a = null, int? b = 1) { }

        public void OptionalDecimal(decimal a = 1.5m) { }

        public void OptionalDateTime([System.Runtime.InteropServices.Optional, DateTimeConstant(630822816000000000)] DateTime a) { }

        public void ArrayParameters(int[] a, int[,] b, int[][] c, int[,,] d) { }

        public unsafe void PointerParameters(int* a, void* b, int** c) { }

        public void VarArgs(int a, __arglist) { }

        public static void StaticMethod() { }

        public virtual void VirtualMethod() { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void NoInlining() { }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Synchronized() { }

#if NET8_0_OR_GREATER

        // only the .NET 8 runtime reports function pointer types; earlier runtimes report System.IntPtr

        public unsafe delegate*<int, int> ManagedFunctionPointer(delegate*<int, void> a) => null;

        public unsafe delegate* unmanaged[Cdecl]<int, int> UnmanagedFunctionPointer(delegate* unmanaged[Stdcall]<void> a) => null;

#endif

    }

    public class Operators
    {

        public static Operators operator +(Operators a, Operators b) => a;

        public static bool operator ==(Operators a, Operators b) => true;

        public static bool operator !=(Operators a, Operators b) => false;

        public static implicit operator int(Operators a) => 0;

        public static explicit operator Operators(int a) => null;

        public override bool Equals(object obj) => false;

        public override int GetHashCode() => 0;

    }

}
