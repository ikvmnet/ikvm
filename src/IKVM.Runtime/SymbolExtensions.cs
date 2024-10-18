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

        public static Assembly GetUnderlyingAssembly(this IAssemblySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#else
            return ((IReflectionAssemblySymbol)symbol).UnderlyingAssembly;
#endif
        }

        public static Module GetUnderlyingModule(this IModuleSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionModuleSymbol)symbol).UnderlyingModule;
#else
            return ((IReflectionModuleSymbol)symbol).UnderlyingModule;
#endif
        }

        public static Type GetUnderlyingType(this ITypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionTypeSymbol)symbol).UnderlyingType;
#else
            return ((IReflectionTypeSymbol)symbol).UnderlyingType;
#endif
        }

        public static Type GetUnderlyingRuntimeType(this ITypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            throw new NotSupportedException();
#else
            return ((IReflectionTypeSymbol)symbol).UnderlyingRuntimeType;
#endif
        }

        public static Type[] GetUnderlyingTypes(this ITypeSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new Type[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingType(symbols[i]);

            return a;
        }

        public static MethodBase GetUnderlyingMethodBase(this IMethodBaseSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionMethodBaseSymbol)symbol).UnderlyingMethodBase;
#else
            return ((IReflectionMethodBaseSymbol)symbol).UnderlyingMethodBase;
#endif
        }

        public static ConstructorInfo GetUnderlyingConstructor(this IConstructorSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#else
            return ((IReflectionConstructorSymbol)symbol).UnderlyingConstructor;
#endif
        }

        public static MethodInfo GetUnderlyingMethod(this IMethodSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionMethodSymbol)symbol).UnderlyingMethod;
#else
            return ((IReflectionMethodSymbol)symbol).UnderlyingMethod;
#endif
        }

        public static ParameterInfo GetUnderlyingParameter(this IParameterSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionParameterSymbol)symbol).UnderlyingParameter;
#else
            return ((IReflectionParameterSymbol)symbol).UnderlyingParameter;
#endif
        }

        public static FieldInfo GetUnderlyingField(this IFieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionFieldSymbol)symbol).UnderlyingField;
#else
            return ((IReflectionFieldSymbol)symbol).UnderlyingField;
#endif
        }

        public static FieldInfo GetUnderlyingRuntimeField(this IFieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            throw new NotSupportedException();
#else
            return ((IReflectionFieldSymbol)symbol).UnderlyingRuntimeField;
#endif
        }

        public static FieldInfo[] GetUnderlyingFields(this IFieldSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new FieldInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingField(symbols[i]);

            return a;
        }

        public static PropertyInfo GetUnderlyingProperty(this IPropertySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IIkvmReflectionPropertySymbol)symbol).UnderlyingProperty;
#else
            return ((IReflectionPropertySymbol)symbol).UnderlyingProperty;
#endif
        }

        public static PropertyInfo[] GetUnderlyingProperties(this IPropertySymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new PropertyInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingProperty(symbols[i]);

            return a;
        }

        public static AssemblyBuilder GetUnderlyingAssemblyBuilder(this IAssemblySymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionAssemblySymbolBuilder)builder).UnderlyingAssemblyBuilder;
#else
            return ((IReflectionAssemblySymbolBuilder)builder).UnderlyingAssemblyBuilder;
#endif
        }

        public static ModuleBuilder GetUnderlyingModuleBuilder(this IModuleSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionModuleSymbolBuilder)builder).UnderlyingModuleBuilder;
#else
            return ((IReflectionModuleSymbolBuilder)builder).UnderlyingModuleBuilder;
#endif
        }

        public static TypeBuilder GetUnderlyingTypeBuilder(this ITypeSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionTypeSymbolBuilder)builder).UnderlyingTypeBuilder;
#else
            return ((IReflectionTypeSymbolBuilder)builder).UnderlyingTypeBuilder;
#endif
        }

        public static ConstructorBuilder GetUnderlyingConstructorBuilder(this IConstructorSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionConstructorSymbolBuilder)builder).UnderlyingConstructorBuilder;
#else
            return ((IReflectionConstructorSymbolBuilder)builder).UnderlyingConstructorBuilder;
#endif
        }

        public static MethodBuilder GetUnderlyingMethodBuilder(this IMethodSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IIkvmReflectionMethodSymbolBuilder)builder).UnderlyingMethodBuilder;
#else
            return ((IReflectionMethodSymbolBuilder)builder).UnderlyingMethodBuilder;
#endif
        }

        public static FieldBuilder GetUnderlyingFieldBuilder(this IFieldSymbolBuilder builder)
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
