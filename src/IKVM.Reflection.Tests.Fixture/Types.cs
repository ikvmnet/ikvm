using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Reflection.Tests.Fixture
{

    public class PlainClass
    {

        public PlainClass() { }

        public PlainClass(int value) { }

        static PlainClass() { }

        ~PlainClass() { }

    }

    public static class StaticClass
    {

        public static int Value;

        public static void Method() { }

    }

    public abstract class AbstractClass
    {

        protected AbstractClass() { }

        public abstract void AbstractMethod();

        public virtual void VirtualMethod() { }

        public virtual void VirtualMethodToSeal() { }

        public virtual void VirtualMethodToHide() { }

        public void NonVirtualMethod() { }

        public virtual int OverloadedMethod(int value) => value;

        public virtual int OverloadedMethod(string value) => 0;

        protected internal virtual void ProtectedInternalMethod() { }

        internal virtual void InternalMethod() { }

        private void PrivateMethod() { }

    }

    public class DerivedClass : AbstractClass
    {

        public override void AbstractMethod() { }

        public override void VirtualMethod() { }

        public sealed override void VirtualMethodToSeal() { }

        public new virtual void VirtualMethodToHide() { }

        public override int OverloadedMethod(int value) => value;

        protected internal override void ProtectedInternalMethod() { }

    }

    public sealed class MostDerivedClass : DerivedClass
    {

        public override void VirtualMethod() { }

        public override void VirtualMethodToHide() { }

    }

    public struct PlainStruct
    {

        public int Field;

        public PlainStruct(int field) { Field = field; }

        public override string ToString() => "";

    }

    public readonly struct ReadOnlyStruct
    {

        public readonly int Field;

        public ReadOnlyStruct(int field) { Field = field; }

    }

    public ref struct RefStruct
    {

        public int Field;

    }

    public interface IPlainInterface
    {

        void Method();

        int Property { get; set; }

        event EventHandler Event;

    }

    public interface IDerivedInterface : IPlainInterface, IEnumerable
    {

        void DerivedMethod();

    }

    public class ImplicitImplementation : IDerivedInterface
    {

        public void Method() { }

        public int Property { get; set; }

        public event EventHandler Event;

        public void DerivedMethod() { }

        public IEnumerator GetEnumerator() => null;

    }

    public class ExplicitImplementation : IPlainInterface, IDisposable
    {

        void IPlainInterface.Method() { }

        int IPlainInterface.Property { get; set; }

        event EventHandler IPlainInterface.Event { add { } remove { } }

        void IDisposable.Dispose() { }

    }

    public class ReimplementingClass : ImplicitImplementation, IPlainInterface
    {

        public new void Method() { }

    }

    public class MultipleGenericInterfaceImplementation : IEquatable<int>, IEquatable<string>, IComparable<MultipleGenericInterfaceImplementation>
    {

        public bool Equals(int other) => false;

        bool IEquatable<string>.Equals(string other) => false;

        public int CompareTo(MultipleGenericInterfaceImplementation other) => 0;

    }

    public delegate void PlainDelegate();

    public delegate TResult GenericDelegate<in T, out TResult>(T arg);

    public delegate int DelegateWithRef(ref int value, out string result);

    public class Outer
    {

        public class Nested
        {

            public class DeeplyNested
            {

            }

        }

        protected class ProtectedNested { }

        internal class InternalNested { }

        private class PrivateNested { }

        public struct NestedStruct { }

        public interface INestedInterface { }

        public enum NestedEnum { A }

        public delegate void NestedDelegate();

    }

}
