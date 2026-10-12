using System;
using System.Runtime.InteropServices;

// assembly and module attributes, so the rendering covers custom attributes on both
[assembly: IKVM.Reflection.Tests.Fixture.Plain]
[assembly: IKVM.Reflection.Tests.Fixture.WithArguments(1, "assembly")]
[module: IKVM.Reflection.Tests.Fixture.Plain]

namespace IKVM.Reflection.Tests.Fixture
{

    /// <summary>
    /// An attribute without arguments that may be applied more than once and is inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class PlainAttribute : Attribute
    {

    }

    /// <summary>
    /// An attribute that may be applied once and is not inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class NotInheritedAttribute : Attribute
    {

    }

    /// <summary>
    /// An attribute with constructors and named arguments of every kind of custom attribute value, so the rendering
    /// covers how each kind is encoded.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class WithArgumentsAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance without arguments.
        /// </summary>
        public WithArgumentsAttribute() { }

        /// <summary>
        /// Initializes a new instance with an integer and a string.
        /// </summary>
        /// <param name="value">An integer argument.</param>
        /// <param name="text">A string argument.</param>
        public WithArgumentsAttribute(int value, string text) { }

        /// <summary>
        /// Initializes a new instance with one argument of each primitive type and a string.
        /// </summary>
        /// <param name="a">An argument of the attribute.</param>
        /// <param name="b">An argument of the attribute.</param>
        /// <param name="c">An argument of the attribute.</param>
        /// <param name="d">An argument of the attribute.</param>
        /// <param name="e">An argument of the attribute.</param>
        /// <param name="f">An argument of the attribute.</param>
        /// <param name="g">An argument of the attribute.</param>
        /// <param name="h">An argument of the attribute.</param>
        /// <param name="i">An argument of the attribute.</param>
        /// <param name="j">An argument of the attribute.</param>
        /// <param name="k">An argument of the attribute.</param>
        /// <param name="l">An argument of the attribute.</param>
        /// <param name="m">An argument of the attribute.</param>
        public WithArgumentsAttribute(bool a, byte b, sbyte c, char d, short e, ushort f, int g, uint h, long i, ulong j, float k, double l, string m) { }

        /// <summary>
        /// Initializes a new instance with a type argument.
        /// </summary>
        /// <param name="type">A type argument, possibly <see langword="null"/>.</param>
        public WithArgumentsAttribute(Type type) { }

        /// <summary>
        /// Initializes a new instance with an enum argument.
        /// </summary>
        /// <param name="value">An enum argument.</param>
        public WithArgumentsAttribute(PlainEnum value) { }

        /// <summary>
        /// Initializes a new instance with a flags enum argument.
        /// </summary>
        /// <param name="value">A flags enum argument.</param>
        public WithArgumentsAttribute(FlagsEnum value) { }

        /// <summary>
        /// Initializes a new instance with an argument of type <see cref="object"/>, which is encoded with its actual
        /// type.
        /// </summary>
        /// <param name="value">A boxed argument.</param>
        public WithArgumentsAttribute(object value) { }

        /// <summary>
        /// Initializes a new instance with an array of integers.
        /// </summary>
        /// <param name="values">An integer array argument.</param>
        public WithArgumentsAttribute(int[] values) { }

        /// <summary>
        /// Initializes a new instance with an array of objects, whose elements are encoded with their actual types.
        /// </summary>
        /// <param name="values">An object array argument.</param>
        public WithArgumentsAttribute(object[] values) { }

        /// <summary>
        /// Initializes a new instance with an array of types.
        /// </summary>
        /// <param name="values">A type array argument.</param>
        public WithArgumentsAttribute(Type[] values) { }

        /// <summary>
        /// Initializes a new instance with a parameter array of strings.
        /// </summary>
        /// <param name="values">A string array argument.</param>
        public WithArgumentsAttribute(params string[] values) { }

        /// <summary>
        /// An integer field set as a named argument.
        /// </summary>
        public int IntField;

        /// <summary>
        /// An object field set as a named argument.
        /// </summary>
        public object ObjectField;

        /// <summary>
        /// Gets or sets a type, set as a named argument.
        /// </summary>
        public Type TypeProperty { get; set; }

        /// <summary>
        /// Gets or sets an enum value, set as a named argument.
        /// </summary>
        public PlainEnum EnumProperty { get; set; }

        /// <summary>
        /// Gets or sets a string array, set as a named argument.
        /// </summary>
        public string[] ArrayProperty { get; set; }

    }

    /// <summary>
    /// A class with one custom attribute of each argument kind, and members of each kind that carry a custom attribute.
    /// </summary>
    [Plain]
    [NotInherited]
    [WithArguments(true, 1, -1, 'c', -2, 2, -3, 3, -4, 4, 1.5f, 2.5, "s")]
    [WithArguments(typeof(PlainClass))]
    [WithArguments(typeof(GenericClass<>))]
    [WithArguments(typeof(GenericClass<int>))]
    [WithArguments(typeof(GenericClass<string>.GenericNestedInGeneric<PlainEnum>))]
    [WithArguments(typeof(Outer.Nested.DeeplyNested))]
    [WithArguments(typeof(int[]))]
    [WithArguments(typeof(int[,]))]
    [WithArguments((Type)null)]
    [WithArguments(PlainEnum.C)]
    [WithArguments(FlagsEnum.One | FlagsEnum.Two)]
    [WithArguments((object)1)]
    [WithArguments((object)"boxed")]
    [WithArguments((object)PlainEnum.B)]
    [WithArguments((object)typeof(string))]
    [WithArguments((object)null)]
    [WithArguments(new[] { 1, 2, 3 })]
    [WithArguments(new object[] { 1, "two", typeof(int), PlainEnum.A, null, new[] { 1 } })]
    [WithArguments(new[] { typeof(int), typeof(string) })]
    [WithArguments("a", "b", "c")]
    [WithArguments(IntField = 1, ObjectField = "object", TypeProperty = typeof(PlainStruct), EnumProperty = PlainEnum.B, ArrayProperty = new[] { "x", null })]
    public class AttributedClass
    {

        /// <summary>
        /// A field with a custom attribute.
        /// </summary>
        [Plain]
        public int Field;

        /// <summary>
        /// Gets or sets a value; the property and both accessors carry a custom attribute.
        /// </summary>
        [Plain]
        public int Property { [Plain] get; [Plain] set; }

        /// <summary>
        /// An event with a custom attribute.
        /// </summary>
        [Plain]
        public event EventHandler Event;

        /// <summary>
        /// A method with custom attributes on itself, its return value and its parameter.
        /// </summary>
        /// <param name="parameter">A parameter with a custom attribute.</param>
        /// <returns>Zero.</returns>
        [Plain]
        [return: Plain]
        public int Method([Plain] int parameter) => 0;

        /// <summary>
        /// A generic method whose generic parameter carries a custom attribute.
        /// </summary>
        /// <typeparam name="T">A generic parameter with a custom attribute.</typeparam>
        public void GenericMethod<[Plain] T>() { }

        /// <summary>
        /// Initializes a new instance; the constructor carries a custom attribute.
        /// </summary>
        [Plain]
        public AttributedClass() { }

    }

    /// <summary>
    /// A class that inherits the custom attributes of <see cref="AttributedClass"/>.
    /// </summary>
    public class InheritedAttributes : AttributedClass
    {

    }

    /// <summary>
    /// A generic class whose generic parameter carries a custom attribute.
    /// </summary>
    /// <typeparam name="T">A generic parameter with a custom attribute.</typeparam>
    public class GenericAttributed<[Plain] T>
    {

    }

    /// <summary>
    /// A struct whose layout, serialization, field offsets and marshalling come from pseudo custom attributes, which
    /// are stored as metadata rather than as custom attributes.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 24, Pack = 8, CharSet = CharSet.Unicode)]
    public struct PseudoAttributes
    {

        /// <summary>
        /// A field at offset 0.
        /// </summary>
        [FieldOffset(0)]
        public int A;

        /// <summary>
        /// A field at offset 8 with a marshalling descriptor.
        /// </summary>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPWStr)]
        public string B;

        /// <summary>
        /// A field at offset 16 that is not serialized.
        /// </summary>
        [FieldOffset(16)]
        [NonSerialized]
        public int C;

    }

    /// <summary>
    /// A sequential struct packed to one byte.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SequentialPacked
    {

        /// <summary>
        /// The first field.
        /// </summary>
        public byte A;

        /// <summary>
        /// The second field, unaligned because of the packing.
        /// </summary>
        public int B;

    }

    /// <summary>
    /// A COM interface, imported with a GUID.
    /// </summary>
    [ComImport]
    [Guid("00000000-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComInterface
    {

        /// <summary>
        /// A method whose signature is preserved, with marshalled and in/out parameters.
        /// </summary>
        /// <param name="values">An in/out array marshalled as a fixed-size C array.</param>
        /// <param name="value">An object marshalled as an interface pointer.</param>
        /// <returns>An HRESULT.</returns>
        [PreserveSig]
        int Method([In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] values, [MarshalAs(UnmanagedType.Interface)] object value);

    }

    /// <summary>
    /// P/Invoke methods, whose imports are stored as metadata rather than as custom attributes.
    /// </summary>
    public static class PInvoke
    {

        /// <summary>
        /// An import that sets the entry point, character set, last error, calling convention and exact spelling options.
        /// </summary>
        /// <returns>The number of milliseconds since the system started.</returns>
        [DllImport("kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern uint TickCount();

        /// <summary>
        /// An import with default options and marshalled return value and parameters.
        /// </summary>
        /// <param name="type">A parameter marshalled as an unsigned 32-bit integer.</param>
        /// <param name="ignored">An optional parameter.</param>
        /// <returns>The result, marshalled as a Win32 BOOL.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MessageBeep([MarshalAs(UnmanagedType.U4)] uint type, [Optional] int ignored);

    }

    /// <summary>
    /// A class marked obsolete, whose use is an error.
    /// </summary>
    [Obsolete("obsolete", true)]
    public class ObsoleteClass
    {

    }

}
