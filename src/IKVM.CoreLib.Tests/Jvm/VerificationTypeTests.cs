using System;
using System.Collections.Generic;

using FluentAssertions;

using IKVM.CoreLib.Jvm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jvm
{

    [TestClass]
    public class VerificationTypeTests
    {

        sealed class T
        {

            readonly string name;

            public T(string name)
            {
                this.name = name;
            }

            public override string ToString() => name;

        }

        static readonly T String = new("java.lang.String");
        static readonly T Object = new("java.lang.Object");
        static readonly T Throwable = new("java.lang.Throwable");

        /// <summary>
        /// One of every distinct shape of verification type, none of which are equal to each other.
        /// </summary>
        static IEnumerable<VerificationType<T>> Distinct()
        {
            yield return VerificationType<T>.Top;
            yield return VerificationType<T>.Int;
            yield return VerificationType<T>.Float;
            yield return VerificationType<T>.ExtendedFloat;
            yield return VerificationType<T>.Long;
            yield return VerificationType<T>.Double;
            yield return VerificationType<T>.ExtendedDouble;
            yield return VerificationType<T>.Null;
            yield return VerificationType<T>.UninitializedThis;
            yield return VerificationType<T>.Uninitialized(String, 0);
            yield return VerificationType<T>.Uninitialized(String, 1);
            yield return VerificationType<T>.Uninitialized(Object, 0);
            yield return VerificationType<T>.Reference(String);
            yield return VerificationType<T>.Reference(Object);
            yield return VerificationType<T>.This(String);
            yield return VerificationType<T>.This(Object);
            yield return VerificationType<T>.FaultBlockException(Throwable, 3);
            yield return VerificationType<T>.FaultBlockException(Throwable, 4);
            yield return VerificationType<T>.Reference(Throwable);
        }

        [TestMethod]
        public void DefaultIsTop()
        {
            default(VerificationType<T>).IsTop.Should().BeTrue();
            default(VerificationType<T>).Should().Be(VerificationType<T>.Top);
            new VerificationType<T>[3].Should().AllSatisfy(i => i.IsTop.Should().BeTrue());
        }

        [TestMethod]
        public void DistinctTypesAreNotEqual()
        {
            var all = new List<VerificationType<T>>(Distinct());
            for (int i = 0; i < all.Count; i++)
            {
                for (int j = 0; j < all.Count; j++)
                {
                    (all[i] == all[j]).Should().Be(i == j, "{0} and {1}", all[i], all[j]);
                    (all[i] != all[j]).Should().Be(i != j, "{0} and {1}", all[i], all[j]);
                    all[i].Equals(all[j]).Should().Be(i == j, "{0} and {1}", all[i], all[j]);
                    all[i].Equals((object)all[j]).Should().Be(i == j, "{0} and {1}", all[i], all[j]);
                }
            }
        }

        [TestMethod]
        public void EqualTypesHaveEqualHashCodes()
        {
            foreach (var a in Distinct())
                foreach (var b in Distinct())
                    if (a == b)
                        a.GetHashCode().Should().Be(b.GetHashCode());

            VerificationType<T>.Reference(String).GetHashCode().Should().Be(VerificationType<T>.Reference(String).GetHashCode());
            VerificationType<T>.Uninitialized(String, 7).GetHashCode().Should().Be(VerificationType<T>.Uninitialized(String, 7).GetHashCode());
        }

        [TestMethod]
        public void TypesAreEqualByValue()
        {
            VerificationType<T>.Reference(String).Should().Be(VerificationType<T>.Reference(String));
            VerificationType<T>.This(String).Should().Be(VerificationType<T>.This(String));
            VerificationType<T>.Uninitialized(String, 5).Should().Be(VerificationType<T>.Uninitialized(String, 5));
            VerificationType<T>.FaultBlockException(Throwable, 5).Should().Be(VerificationType<T>.FaultBlockException(Throwable, 5));
            VerificationType<T>.ExtendedDouble.Should().Be(VerificationType<T>.ExtendedDouble);
        }

        [TestMethod]
        public void ReferencesCompareTypesByIdentity()
        {
            var a = new T("same");
            var b = new T("same");
            VerificationType<T>.Reference(a).Should().NotBe(VerificationType<T>.Reference(b));
        }

        [TestMethod]
        public void NotEqualToOtherObjects()
        {
            VerificationType<T>.Int.Equals(null).Should().BeFalse();
            VerificationType<T>.Int.Equals(1).Should().BeFalse();
            VerificationType<T>.Reference(String).Equals(String).Should().BeFalse();
        }

        [TestMethod]
        public void KindsAndFlags()
        {
            VerificationType<T>.Top.Kind.Should().Be(VerificationTypeKind.Top);
            VerificationType<T>.Int.Kind.Should().Be(VerificationTypeKind.Int);
            VerificationType<T>.Float.Kind.Should().Be(VerificationTypeKind.Float);
            VerificationType<T>.ExtendedFloat.Kind.Should().Be(VerificationTypeKind.Float);
            VerificationType<T>.Long.Kind.Should().Be(VerificationTypeKind.Long);
            VerificationType<T>.Double.Kind.Should().Be(VerificationTypeKind.Double);
            VerificationType<T>.ExtendedDouble.Kind.Should().Be(VerificationTypeKind.Double);
            VerificationType<T>.Null.Kind.Should().Be(VerificationTypeKind.Null);
            VerificationType<T>.UninitializedThis.Kind.Should().Be(VerificationTypeKind.Uninitialized);
            VerificationType<T>.Uninitialized(String, 0).Kind.Should().Be(VerificationTypeKind.Uninitialized);
            VerificationType<T>.Reference(String).Kind.Should().Be(VerificationTypeKind.Reference);
            VerificationType<T>.This(String).Kind.Should().Be(VerificationTypeKind.Reference);
            VerificationType<T>.FaultBlockException(Throwable, 0).Kind.Should().Be(VerificationTypeKind.Reference);

            VerificationType<T>.Float.Flags.Should().Be(VerificationTypeFlags.None);
            VerificationType<T>.ExtendedFloat.Flags.Should().Be(VerificationTypeFlags.Extended);
            VerificationType<T>.ExtendedDouble.Flags.Should().Be(VerificationTypeFlags.Extended);
            VerificationType<T>.Reference(String).Flags.Should().Be(VerificationTypeFlags.None);
            VerificationType<T>.This(String).Flags.Should().Be(VerificationTypeFlags.This);
            VerificationType<T>.FaultBlockException(Throwable, 0).Flags.Should().Be(VerificationTypeFlags.FaultBlock);
        }

        [TestMethod]
        public void Predicates()
        {
            foreach (var t in Distinct())
            {
                t.IsTop.Should().Be(t.Kind == VerificationTypeKind.Top);
                t.IsInt.Should().Be(t.Kind == VerificationTypeKind.Int);
                t.IsFloat.Should().Be(t.Kind == VerificationTypeKind.Float);
                t.IsLong.Should().Be(t.Kind == VerificationTypeKind.Long);
                t.IsDouble.Should().Be(t.Kind == VerificationTypeKind.Double);
                t.IsNull.Should().Be(t.Kind == VerificationTypeKind.Null);
                t.IsReference.Should().Be(t.Kind == VerificationTypeKind.Reference);
                t.IsUninitialized.Should().Be(t.Kind == VerificationTypeKind.Uninitialized);
                t.IsUninitialized.Should().Be(t.IsUninitializedThis || t.IsUninitializedNew);
                (t.IsUninitializedThis && t.IsUninitializedNew).Should().BeFalse();
                t.IsPrimitive.Should().Be(t.IsInt || t.IsFloat || t.IsLong || t.IsDouble);
                t.IsCategory2.Should().Be(t.IsLong || t.IsDouble);
                t.IsExtended.Should().Be((t.Flags & VerificationTypeFlags.Extended) != 0);
                t.IsThis.Should().Be((t.Flags & VerificationTypeFlags.This) != 0);
                t.IsFaultBlockException.Should().Be((t.Flags & VerificationTypeFlags.FaultBlock) != 0);
            }
        }

        [TestMethod]
        public void UninitializedThisAndNewAreDistinguished()
        {
            VerificationType<T>.UninitializedThis.IsUninitializedThis.Should().BeTrue();
            VerificationType<T>.UninitializedThis.IsUninitializedNew.Should().BeFalse();
            VerificationType<T>.UninitializedThis.Type.Should().BeNull();
            VerificationType<T>.Uninitialized(String, 0).IsUninitializedNew.Should().BeTrue();
            VerificationType<T>.Uninitialized(String, 0).IsUninitializedThis.Should().BeFalse();
        }

        [TestMethod]
        public void TypeAndIndex()
        {
            VerificationType<T>.Int.Type.Should().BeNull();
            VerificationType<T>.Null.Type.Should().BeNull();
            VerificationType<T>.Reference(String).Type.Should().BeSameAs(String);
            VerificationType<T>.This(String).Type.Should().BeSameAs(String);
            VerificationType<T>.Uninitialized(String, 9).Type.Should().BeSameAs(String);
            VerificationType<T>.Uninitialized(String, 9).Index.Should().Be(9);
            VerificationType<T>.FaultBlockException(Throwable, 12).Type.Should().BeSameAs(Throwable);
            VerificationType<T>.FaultBlockException(Throwable, 12).Index.Should().Be(12);
        }

        [TestMethod]
        public void WithoutFlagsLeavesTheCoreType()
        {
            VerificationType<T>.ExtendedFloat.WithoutFlags().Should().Be(VerificationType<T>.Float);
            VerificationType<T>.ExtendedDouble.WithoutFlags().Should().Be(VerificationType<T>.Double);
            VerificationType<T>.This(String).WithoutFlags().Should().Be(VerificationType<T>.Reference(String));
            VerificationType<T>.FaultBlockException(Throwable, 4).WithoutFlags().Should().Be(VerificationType<T>.Reference(Throwable));
        }

        [TestMethod]
        public void WithoutFlagsKeepsTypesWithoutFlags()
        {
            foreach (var t in Distinct())
                if (t.Flags == VerificationTypeFlags.None)
                    t.WithoutFlags().Should().Be(t);
        }

        [TestMethod]
        public void ArgumentsAreChecked()
        {
            FluentActions.Invoking(() => VerificationType<T>.Reference(null!)).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => VerificationType<T>.This(null!)).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => VerificationType<T>.Uninitialized(null!, 0)).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => VerificationType<T>.Uninitialized(String, -1)).Should().Throw<ArgumentOutOfRangeException>();
            FluentActions.Invoking(() => VerificationType<T>.FaultBlockException(null!, 0)).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => VerificationType<T>.FaultBlockException(Throwable, -1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToStringDescribesTheType()
        {
            VerificationType<T>.Top.ToString().Should().Be("top");
            VerificationType<T>.Int.ToString().Should().Be("I");
            VerificationType<T>.Float.ToString().Should().Be("F");
            VerificationType<T>.ExtendedFloat.ToString().Should().Be("F!");
            VerificationType<T>.Long.ToString().Should().Be("J");
            VerificationType<T>.Double.ToString().Should().Be("D");
            VerificationType<T>.ExtendedDouble.ToString().Should().Be("D!");
            VerificationType<T>.Null.ToString().Should().Be("null");
            VerificationType<T>.UninitializedThis.ToString().Should().Be("uninitThis");
            VerificationType<T>.Uninitialized(Object, 3).ToString().Should().Be("new@3:java.lang.Object");
            VerificationType<T>.Reference(String).ToString().Should().Be("java.lang.String");
            VerificationType<T>.This(String).ToString().Should().Be("this:java.lang.String");
            VerificationType<T>.FaultBlockException(Throwable, 6).ToString().Should().Be("fault@6");
        }

    }

}
