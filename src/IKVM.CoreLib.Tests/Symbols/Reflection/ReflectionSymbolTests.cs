using System;

using FluentAssertions;

using IKVM.CoreLib.Symbols.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

	[TestClass]
	public class ReflectionSymbolTests
	{

		[TestMethod]
		public void SameTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(object));
			var s2 = c.GetOrCreateTypeSymbol(typeof(object));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void SubstitutedTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(Action<>));
			var s2 = c.GetOrCreateTypeSymbol(typeof(Action<object>));
		}

	}

}
