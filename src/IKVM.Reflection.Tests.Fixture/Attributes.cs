using System;
using System.Runtime.InteropServices;

[assembly: IKVM.Reflection.Tests.Fixture.Plain]
[assembly: IKVM.Reflection.Tests.Fixture.WithArguments(1, "assembly")]
[module: IKVM.Reflection.Tests.Fixture.Plain]

namespace IKVM.Reflection.Tests.Fixture
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class PlainAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class NotInheritedAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class WithArgumentsAttribute : Attribute
    {

        public WithArgumentsAttribute() { }

        public WithArgumentsAttribute(int value, string text) { }

        public WithArgumentsAttribute(bool a, byte b, sbyte c, char d, short e, ushort f, int g, uint h, long i, ulong j, float k, double l, string m) { }

        public WithArgumentsAttribute(Type type) { }

        public WithArgumentsAttribute(PlainEnum value) { }

        public WithArgumentsAttribute(FlagsEnum value) { }

        public WithArgumentsAttribute(object value) { }

        public WithArgumentsAttribute(int[] values) { }

        public WithArgumentsAttribute(object[] values) { }

        public WithArgumentsAttribute(Type[] values) { }

        public WithArgumentsAttribute(params string[] values) { }

        public int IntField;

        public object ObjectField;

        public Type TypeProperty { get; set; }

        public PlainEnum EnumProperty { get; set; }

        public string[] ArrayProperty { get; set; }

    }

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

        [Plain]
        public int Field;

        [Plain]
        public int Property { [Plain] get; [Plain] set; }

        [Plain]
        public event EventHandler Event;

        [Plain]
        [return: Plain]
        public int Method([Plain] int parameter) => 0;

        public void GenericMethod<[Plain] T>() { }

        [Plain]
        public AttributedClass() { }

    }

    public class InheritedAttributes : AttributedClass
    {

    }

    public class GenericAttributed<[Plain] T>
    {

    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 24, Pack = 8, CharSet = CharSet.Unicode)]
    public struct PseudoAttributes
    {

        [FieldOffset(0)]
        public int A;

        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPWStr)]
        public string B;

        [FieldOffset(16)]
        [NonSerialized]
        public int C;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SequentialPacked
    {

        public byte A;

        public int B;

    }

    [ComImport]
    [Guid("00000000-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComInterface
    {

        [PreserveSig]
        int Method([In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] values, [MarshalAs(UnmanagedType.Interface)] object value);

    }

    public static class PInvoke
    {

        [DllImport("kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern uint TickCount();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MessageBeep([MarshalAs(UnmanagedType.U4)] uint type, [Optional] int ignored);

    }

    [Obsolete("obsolete", true)]
    public class ObsoleteClass
    {

    }

}
