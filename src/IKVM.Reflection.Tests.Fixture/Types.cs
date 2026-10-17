using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Reflection.Tests.Fixture
{

    /// <summary>
    /// A class with instance and static constructors and a finalizer.
    /// </summary>
    public class PlainClass
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public PlainClass() { }

        /// <summary>
        /// Initializes a new instance with a value.
        /// </summary>
        /// <param name="value">The value, not used.</param>
        public PlainClass(int value) { }

        /// <summary>
        /// Initializes the type.
        /// </summary>
        static PlainClass() { }

        /// <summary>
        /// Finalizes an instance.
        /// </summary>
        ~PlainClass() { }

    }

    /// <summary>
    /// A static class, which is abstract and sealed.
    /// </summary>
    public static class StaticClass
    {

        /// <summary>
        /// A static field.
        /// </summary>
        public static int Value;

        /// <summary>
        /// A static method.
        /// </summary>
        public static void Method() { }

    }

    /// <summary>
    /// An abstract class with methods of each kind of virtualness and visibility, which derived classes override, seal
    /// and hide.
    /// </summary>
    public abstract class AbstractClass
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected AbstractClass() { }

        /// <summary>
        /// An abstract method.
        /// </summary>
        public abstract void AbstractMethod();

        /// <summary>
        /// A virtual method that derived classes override.
        /// </summary>
        public virtual void VirtualMethod() { }

        /// <summary>
        /// A virtual method that a derived class seals.
        /// </summary>
        public virtual void VirtualMethodToSeal() { }

        /// <summary>
        /// A virtual method that a derived class hides.
        /// </summary>
        public virtual void VirtualMethodToHide() { }

        /// <summary>
        /// A method that is not virtual.
        /// </summary>
        public void NonVirtualMethod() { }

        /// <summary>
        /// A virtual overload that a derived class overrides.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns><paramref name="value"/>.</returns>
        public virtual int OverloadedMethod(int value) => value;

        /// <summary>
        /// A virtual overload that derived classes do not override.
        /// </summary>
        /// <param name="value">A value, not used.</param>
        /// <returns>Zero.</returns>
        public virtual int OverloadedMethod(string value) => 0;

        /// <summary>
        /// A protected internal virtual method.
        /// </summary>
        protected internal virtual void ProtectedInternalMethod() { }

        /// <summary>
        /// An internal virtual method.
        /// </summary>
        internal virtual void InternalMethod() { }

        /// <summary>
        /// A private method.
        /// </summary>
        private void PrivateMethod() { }

    }

    /// <summary>
    /// A class that overrides, seals and hides methods of its base class.
    /// </summary>
    public class DerivedClass : AbstractClass
    {

        /// <summary>
        /// Overrides an abstract method.
        /// </summary>
        public override void AbstractMethod() { }

        /// <summary>
        /// Overrides a virtual method.
        /// </summary>
        public override void VirtualMethod() { }

        /// <summary>
        /// Overrides and seals a virtual method.
        /// </summary>
        public sealed override void VirtualMethodToSeal() { }

        /// <summary>
        /// Hides a virtual method with a new virtual method.
        /// </summary>
        public new virtual void VirtualMethodToHide() { }

        /// <summary>
        /// Overrides one overload of a virtual method.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns><paramref name="value"/>.</returns>
        public override int OverloadedMethod(int value) => value;

        /// <summary>
        /// Overrides a protected internal virtual method.
        /// </summary>
        protected internal override void ProtectedInternalMethod() { }

    }

    /// <summary>
    /// A sealed class that overrides an overridden method and the new virtual method that hides a base method.
    /// </summary>
    public sealed class MostDerivedClass : DerivedClass
    {

        /// <summary>
        /// Overrides a method that is itself an override.
        /// </summary>
        public override void VirtualMethod() { }

        /// <summary>
        /// Overrides the new virtual method of the base class, not the one it hides.
        /// </summary>
        public override void VirtualMethodToHide() { }

    }

    /// <summary>
    /// A struct with a field, a constructor and an override.
    /// </summary>
    public struct PlainStruct
    {

        /// <summary>
        /// A field.
        /// </summary>
        public int Field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="field">The value of <see cref="Field"/>.</param>
        public PlainStruct(int field) { Field = field; }

        /// <summary>
        /// Overrides <see cref="ValueType.ToString"/>.
        /// </summary>
        /// <returns>An empty string.</returns>
        public override string ToString() => "";

    }

    /// <summary>
    /// A read-only struct, which carries a custom attribute.
    /// </summary>
    public readonly struct ReadOnlyStruct
    {

        /// <summary>
        /// A read-only field.
        /// </summary>
        public readonly int Field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="field">The value of <see cref="Field"/>.</param>
        public ReadOnlyStruct(int field) { Field = field; }

    }

    /// <summary>
    /// A ref struct, which carries custom attributes.
    /// </summary>
    public ref struct RefStruct
    {

        /// <summary>
        /// A field.
        /// </summary>
        public int Field;

    }

    /// <summary>
    /// An interface with a method, a property and an event.
    /// </summary>
    public interface IPlainInterface
    {

        /// <summary>
        /// A method.
        /// </summary>
        void Method();

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        int Property { get; set; }

        /// <summary>
        /// An event.
        /// </summary>
        event EventHandler Event;

    }

    /// <summary>
    /// An interface that derives from two interfaces.
    /// </summary>
    public interface IDerivedInterface : IPlainInterface, IEnumerable
    {

        /// <summary>
        /// A method of the derived interface.
        /// </summary>
        void DerivedMethod();

    }

    /// <summary>
    /// A class that implements an interface and its base interfaces with public members.
    /// </summary>
    public class ImplicitImplementation : IDerivedInterface
    {

        /// <summary>
        /// Implements <see cref="IPlainInterface.Method"/>.
        /// </summary>
        public void Method() { }

        /// <summary>
        /// Implements <see cref="IPlainInterface.Property"/>.
        /// </summary>
        public int Property { get; set; }

        /// <summary>
        /// Implements <see cref="IPlainInterface.Event"/>.
        /// </summary>
        public event EventHandler Event;

        /// <summary>
        /// Implements <see cref="IDerivedInterface.DerivedMethod"/>.
        /// </summary>
        public void DerivedMethod() { }

        /// <summary>
        /// Implements <see cref="IEnumerable.GetEnumerator"/>.
        /// </summary>
        /// <returns><see langword="null"/>.</returns>
        public IEnumerator GetEnumerator() => null;

    }

    /// <summary>
    /// A class that implements interfaces with explicit implementations, which are private methods with overrides.
    /// </summary>
    public class ExplicitImplementation : IPlainInterface, IDisposable
    {

        /// <summary>
        /// Explicitly implements <see cref="IPlainInterface.Method"/>.
        /// </summary>
        void IPlainInterface.Method() { }

        /// <summary>
        /// Explicitly implements <see cref="IPlainInterface.Property"/>.
        /// </summary>
        int IPlainInterface.Property { get; set; }

        /// <summary>
        /// Explicitly implements <see cref="IPlainInterface.Event"/>.
        /// </summary>
        event EventHandler IPlainInterface.Event { add { } remove { } }

        /// <summary>
        /// Explicitly implements <see cref="IDisposable.Dispose"/>.
        /// </summary>
        void IDisposable.Dispose() { }

    }

    /// <summary>
    /// A class that re-implements an interface its base class already implements, with a new method.
    /// </summary>
    public class ReimplementingClass : ImplicitImplementation, IPlainInterface
    {

        /// <summary>
        /// Hides the method of the base class and implements <see cref="IPlainInterface.Method"/> again.
        /// </summary>
        public new void Method() { }

    }

    /// <summary>
    /// A class that implements one generic interface over two type arguments, once implicitly and once explicitly.
    /// </summary>
    public class MultipleGenericInterfaceImplementation : IEquatable<int>, IEquatable<string>, IComparable<MultipleGenericInterfaceImplementation>
    {

        /// <summary>
        /// Implicitly implements <see cref="IEquatable{T}"/> of <see cref="int"/>.
        /// </summary>
        /// <param name="other">The value to compare with, not used.</param>
        /// <returns><see langword="false"/>.</returns>
        public bool Equals(int other) => false;

        /// <summary>
        /// Explicitly implements <see cref="IEquatable{T}"/> of <see cref="string"/>.
        /// </summary>
        /// <param name="other">The value to compare with, not used.</param>
        /// <returns><see langword="false"/>.</returns>
        bool IEquatable<string>.Equals(string other) => false;

        /// <summary>
        /// Implements <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="other">The value to compare with, not used.</param>
        /// <returns>Zero.</returns>
        public int CompareTo(MultipleGenericInterfaceImplementation other) => 0;

    }

    /// <summary>
    /// A delegate without parameters.
    /// </summary>
    public delegate void PlainDelegate();

    /// <summary>
    /// A generic delegate with variant generic parameters.
    /// </summary>
    /// <typeparam name="T">The contravariant argument type.</typeparam>
    /// <typeparam name="TResult">The covariant result type.</typeparam>
    /// <param name="arg">The argument.</param>
    /// <returns>The result.</returns>
    public delegate TResult GenericDelegate<in T, out TResult>(T arg);

    /// <summary>
    /// A delegate with ref and out parameters.
    /// </summary>
    /// <param name="value">A ref parameter.</param>
    /// <param name="result">An out parameter.</param>
    /// <returns>A value.</returns>
    public delegate int DelegateWithRef(ref int value, out string result);

    /// <summary>
    /// A class with nested types of each kind and visibility.
    /// </summary>
    public class Outer
    {

        /// <summary>
        /// A nested class that itself contains a nested class.
        /// </summary>
        public class Nested
        {

            /// <summary>
            /// A class nested two levels deep.
            /// </summary>
            public class DeeplyNested
            {

            }

        }

        /// <summary>
        /// A protected nested class.
        /// </summary>
        protected class ProtectedNested { }

        /// <summary>
        /// An internal nested class.
        /// </summary>
        internal class InternalNested { }

        /// <summary>
        /// A private nested class.
        /// </summary>
        private class PrivateNested { }

        /// <summary>
        /// A nested struct.
        /// </summary>
        public struct NestedStruct { }

        /// <summary>
        /// A nested interface.
        /// </summary>
        public interface INestedInterface { }

        /// <summary>
        /// A nested enum.
        /// </summary>
        public enum NestedEnum { A }

        /// <summary>
        /// A nested delegate.
        /// </summary>
        public delegate void NestedDelegate();

    }

}
