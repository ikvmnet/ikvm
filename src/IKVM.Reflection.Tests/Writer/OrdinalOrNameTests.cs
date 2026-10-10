using FluentAssertions;

using IKVM.Reflection.Writer;

using Xunit;

namespace IKVM.Reflection.Tests.Writer
{

    /// <summary>
    /// Tests for <see cref="OrdinalOrName"/>.
    /// </summary>
    public class OrdinalOrNameTests
    {

        /// <summary>
        /// An ordinal has no name and hashes without throwing, and names that differ only in case hash alike.
        /// </summary>
        [Fact]
        public void HashCodeHandlesOrdinalsAndIgnoresCase()
        {
            new OrdinalOrName(16).GetHashCode().Should().Be(new OrdinalOrName(16).GetHashCode());
            new OrdinalOrName("ICON").GetHashCode().Should().Be(new OrdinalOrName("icon").GetHashCode());
        }

    }

}
