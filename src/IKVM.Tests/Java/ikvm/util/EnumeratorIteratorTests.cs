using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.ikvm.util
{

    [TestClass]
    public class EnumeratorIteratorTests
    {

        [TestMethod]
        public void CanIterate()
        {
            var a = new[] { 1, 2, 3, 4 };
            var i = new global::ikvm.util.EnumeratorIterator(a.GetEnumerator());

            var t = 0;
            var c = 0;
            while (i.hasNext())
            {
                c++;
                t += (int)i.next();
            }

            c.Should().Be(4);
            t.Should().Be(1 + 2 + 3 + 4);
        }

    }

}
