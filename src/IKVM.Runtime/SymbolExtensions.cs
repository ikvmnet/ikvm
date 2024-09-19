using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
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
			return ((IIkvmReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#else
            return ((IReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#endif
        }

        public static Module AsReflection(this IModuleSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionModuleSymbol)symbol).UnderlyingModule;
#else
            return ((IReflectionModuleSymbol)symbol).UnderlyingModule;
#endif
        }

        public static Type AsReflection(this ITypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionTypeSymbol)symbol).UnderlyingType;
#else
            return ((IReflectionTypeSymbol)symbol).UnderlyingType;
#endif
        }

        public static Type[] AsReflection(this ITypeSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new Type[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = AsReflection(symbols[i]);

            return a;
        }

        public static MethodBase AsReflection(this IMethodBaseSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionMethodBaseSymbol)symbol).UnderlyingMethodBase;
#else
            return ((IReflectionMethodBaseSymbol)symbol).UnderlyingMethodBase;
#endif
        }

        public static ConstructorInfo AsReflection(this IConstructorSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#else
            return ((IReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#endif
        }

        public static MethodInfo AsReflection(this IMethodSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionMethodSymbol)symbol).UnderlyingMethod;
#else
            return ((IReflectionMethodSymbol)symbol).UnderlyingMethod;
#endif
        }

        public static FieldInfo AsReflection(this IFieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionFieldSymbol)symbol).UnderlyingField;
#else
            return ((IReflectionFieldSymbol)symbol).UnderlyingField;
#endif
        }

        public static FieldInfo[] AsReflection(this IFieldSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new FieldInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = AsReflection(symbols[i]);

            return a;
        }

        public static PropertyInfo AsReflection(this IPropertySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionPropertySymbol)symbol).UnderlyingProperty;
#else
            return ((IReflectionPropertySymbol)symbol).UnderlyingProperty;
#endif
        }

        public static PropertyInfo[] AsReflection(this IPropertySymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new PropertyInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = AsReflection(symbols[i]);

            return a;
        }

        public static AssemblyBuilder AsReflection(this IAssemblySymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionAssemblySymbolBuilder)builder).UnderlyingAssemblyBuilder;
#else
            return ((IReflectionAssemblySymbolBuilder)builder).UnderlyingAssemblyBuilder;
#endif
        }

        public static ModuleBuilder AsReflection(this IModuleSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionModuleSymbolBuilder)builder).UnderlyingModuleBuilder;
#else
            return ((IReflectionModuleSymbolBuilder)builder).UnderlyingModuleBuilder;
#endif
        }

        public static TypeBuilder AsReflection(this ITypeSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionTypeSymbolBuilder)builder).UnderlyingTypeBuilder;
#else
            return ((IReflectionTypeSymbolBuilder)builder).UnderlyingTypeBuilder;
#endif
        }

        public static ConstructorBuilder AsReflection(this IConstructorSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionConstructorSymbolBuilder)builder).UnderlyingConstructorBuilder;
#else
            return ((IReflectionConstructorSymbolBuilder)builder).UnderlyingConstructorBuilder;
#endif
        }

        public static MethodBuilder AsReflection(this IMethodSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionMethodSymbolBuilder)builder).UnderlyingMethodBuilder;
#else
            return ((IReflectionMethodSymbolBuilder)builder).UnderlyingMethodBuilder;
#endif
        }

        public static FieldBuilder AsReflection(this IFieldSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionFieldSymbolBuilder)builder).UnderlyingFieldBuilder;
#else
            return ((IReflectionFieldSymbolBuilder)builder).UnderlyingFieldBuilder;
#endif
        }

    }

}
