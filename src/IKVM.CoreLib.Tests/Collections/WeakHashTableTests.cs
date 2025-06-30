using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

using FluentAssertions;

using IKVM.CoreLib.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Collections
{

    [TestClass]
    public class WeakHashTableTests
    {

        class ArrayComparer : EqualityComparer<object[]>
        {

            public override bool Equals(object[]? x, object[]? y)
            {
                return Enumerable.SequenceEqual(x ?? [], y ?? []);
            }

            public override int GetHashCode([DisallowNull] object[] obj)
            {
                var hc = new HashCode();
                foreach (var i in obj)
                    hc.Add(i);

                return hc.ToHashCode();
            }

        }

        [TestMethod]
        public void CanGetOrCreateValueAtPath()
        {
            var a = new object();
            var b = new object();
            var c = new object();
            var r = new object();

            var t = new WeakHashTable<object[], object>(new ArrayComparer());
            var v1 = t.GetOrCreateValue([a, b, c], k => r);
            v1.Should().BeSameAs(r);
            var v2 = t.GetOrCreateValue([a, b, c], k => new object());
            v2.Should().BeSameAs(r);
        }

        [TestMethod]
        public void CachedValueDoesNotExpireWhenReferenced()
        {
            var t = new WeakHashTable<object[], object>(new ArrayComparer());

            var a = new object();
            var b = new object();
            var c = new object();
            var r = new object();

            void Test(WeakHashTable<object[], object> t)
            {
                var v1 = t.GetOrCreateValue([a, b, c], k => r);
                v1.Should().BeSameAs(r);
                t.Should().HaveCount(1);
            }

            Test(t);

            GC.Collect();
            GC.Collect();
            GC.Collect();
            Thread.Sleep(10);

            t.Should().HaveCount(1);
            GC.KeepAlive(r);
        }

        [TestMethod]
        public void CachedValueExpiresWhenUnreferenced()
        {
            var t = new WeakHashTable<object[], object>(new ArrayComparer());

            var a = new object();
            var b = new object();
            var c = new object();

            void Test(WeakHashTable<object[], object> t)
            {
                var r = new object();

                var v1 = t.GetOrCreateValue([a, b, c], k => r);
                v1.Should().BeSameAs(r);
                t.Should().HaveCount(1);
            }

            Test(t);

            GC.Collect();
            GC.Collect();
            GC.Collect();
            Thread.Sleep(10);

            t.Should().HaveCount(0);
        }

    }

}
