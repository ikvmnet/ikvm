using System;

using FluentAssertions;

using IKVM.CoreLib.Symbols.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

	[TestClass]
	public class ReflectionSymbolTests
	{

		class Foo<T>
		{
			T? field;
		}

		[TestMethod]
		public void SameTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(object));
			var s2 = c.GetOrCreateTypeSymbol(typeof(object));
			s1.Should().BeOfType<ReflectionTypeSymbol>();
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void GenericTypeDefinitionShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(Foo<>));
			var s2 = c.GetOrCreateTypeSymbol(typeof(Foo<>));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void GenericTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
			var s2 = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void ArrayTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(object[,]));
			var s2 = c.GetOrCreateTypeSymbol(typeof(object[,]));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void SZArrayTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(object[]));
			var s2 = c.GetOrCreateTypeSymbol(typeof(object[]));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public unsafe void PointerTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(int*));
			var s2 = c.GetOrCreateTypeSymbol(typeof(int*));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public unsafe void ByRefTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(int).MakeByRefType());
			var s2 = c.GetOrCreateTypeSymbol(typeof(int).MakeByRefType());
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void EnumTypeShouldBeSame()
		{
			var c = new ReflectionSymbolContext();
			var s1 = c.GetOrCreateTypeSymbol(typeof(AttributeTargets));
			var s2 = c.GetOrCreateTypeSymbol(typeof(AttributeTargets));
			s1.Should().BeSameAs(s2);
		}

		[TestMethod]
		public void CanGetType()
		{
			var c = new ReflectionSymbolContext();
			var s = c.GetOrCreateTypeSymbol(typeof(object));
			s.Name.Should().Be("Object");
			s.FullName.Should().Be("System.Object");
		}

		[TestMethod]
		public void CanGetFieldOfGenericTypeDefinition()
		{
			var c = new ReflectionSymbolContext();
			var s = c.GetOrCreateTypeSymbol(typeof(Foo<>));
			s.IsGenericType.Should().BeTrue();
			s.IsGenericTypeDefinition.Should().BeTrue();
			var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			f.Name.Should().Be("field");
			f.FieldType.IsGenericType.Should().BeFalse();
			f.FieldType.IsGenericParameter.Should().BeTrue();
		}

		[TestMethod]
		public void CanGetFieldOfGenericType()
		{
			var c = new ReflectionSymbolContext();
			var s = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
			s.IsGenericType.Should().BeTrue();
			s.IsGenericTypeDefinition.Should().BeFalse();
			var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			f.Name.Should().Be("field");
			f.FieldType.IsGenericType.Should().BeFalse();
			f.FieldType.IsGenericParameter.Should().BeFalse();
			f.FieldType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(int)));
		}

		[TestMethod]
		public void CanGetMethod()
		{
			var c = new ReflectionSymbolContext();
			var s = c.GetOrCreateTypeSymbol(typeof(object));
			var m = s.GetMethod("ToString");
			m.Name.Should().Be("ToString");
			m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(string)));
			m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(string)));
			m.IsGenericMethod.Should().BeFalse();
			m.IsGenericMethodDefinition.Should().BeFalse();
			m.IsPublic.Should().BeTrue();
			m.IsPrivate.Should().BeFalse();
		}

	}

}
