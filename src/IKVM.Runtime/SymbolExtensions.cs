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
			return ((IkvmReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#else
            return ((ReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#endif
        }

        public static Type AsReflection(this ITypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionTypeSymbol)symbol).UnderlyingType;
#else
            return ((ReflectionTypeSymbol)symbol).UnderlyingType;
#endif
        }

        public static ConstructorInfo AsReflection(this IConstructorSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#else
            return ((ReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#endif
        }

        public static MethodInfo AsReflection(this IMethodSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionMethodSymbol)symbol).UnderlyingMethod;
#else
            return ((ReflectionMethodSymbol)symbol).UnderlyingMethod;
#endif
        }

        public static FieldInfo AsReflection(this IFieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionFieldSymbol)symbol).UnderlyingField;
#else
            return ((ReflectionFieldSymbol)symbol).UnderlyingField;
#endif
        }

        public static PropertyInfo AsReflection(this IPropertySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionPropertySymbol)symbol).UnderlyingProperty;
#else
            return ((ReflectionPropertySymbol)symbol).UnderlyingProperty;
#endif
        }

    }

}
