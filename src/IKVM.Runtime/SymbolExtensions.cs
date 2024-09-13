using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Symbols.IkvmReflection;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

	static class SymbolExtensions
	{

		public static Assembly AsReflection(this IAssemblySymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionAssemblySymbol)symbol).ReflectionObject;
#else
			return ((ReflectionAssemblySymbol)symbol).ReflectionObject;
#endif
		}

		public static Type AsReflection(this ITypeSymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionTypeSymbol)symbol).ReflectionObject;
#else
			return ((ReflectionTypeSymbol)symbol).ReflectionObject;
#endif
		}

		public static ConstructorInfo AsReflection(this IConstructorSymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionConstructorSymbol)symbol).ReflectionObject;
#else
			return ((ReflectionConstructorSymbol)symbol).ReflectionObject;
#endif
		}

		public static MethodInfo AsReflection(this IMethodSymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionMethodSymbol)symbol).ReflectionObject;
#else
			return ((ReflectionMethodSymbol)symbol).ReflectionObject;
#endif
		}

		public static FieldInfo AsReflection(this IFieldSymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionFieldSymbol)symbol).ReflectionObject;
#else
			return ((ReflectionFieldSymbol)symbol).ReflectionObject;
#endif
		}

		public static PropertyInfo AsReflection(this IPropertySymbol symbol)
		{
			if (symbol == null)
				return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionPropertySymbol)symbol).ReflectionObject;
#else
			return ((ReflectionPropertySymbol)symbol).ReflectionObject;
#endif
		}

	}

}
