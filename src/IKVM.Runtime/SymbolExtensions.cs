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
			return ((IkvmReflectionAssemblySymbol)symbol).ReflectionObject;
#else
            return ((ReflectionAssemblySymbol)symbol).ReflectionObject;
#endif
        }

        public static Module AsReflection(this IModuleSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
			return ((IkvmReflectionModuleSymbol)symbol).ReflectionObject;
#else
            return ((ReflectionModuleSymbol)symbol).ReflectionObject;
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
			return ((IkvmReflectionMethodBaseSymbol)symbol).ReflectionObject;
#else
            return ((ReflectionMethodBaseSymbol)symbol).ReflectionObject;
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
			return ((IkvmReflectionPropertySymbol)symbol).ReflectionObject;
#else
            return ((ReflectionPropertySymbol)symbol).ReflectionObject;
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
            return ((IkvmReflectionAssemblySymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionAssemblySymbolBuilder)builder).ReflectionBuilder;
#endif
        }

        public static ModuleBuilder AsReflection(this IModuleSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionModuleSymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionModuleSymbolBuilder)builder).ReflectionBuilder;
#endif
        }

        public static TypeBuilder AsReflection(this ITypeSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionTypeSymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionTypeSymbolBuilder)builder).ReflectionBuilder;
#endif
        }

        public static ConstructorBuilder AsReflection(this IConstructorSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionConstructorSymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionConstructorSymbolBuilder)builder).ReflectionBuilder;
#endif
        }

        public static MethodBuilder AsReflection(this IMethodSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionMethodSymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionMethodSymbolBuilder)builder).ReflectionBuilder;
#endif
        }

        public static FieldBuilder AsReflection(this IFieldSymbolBuilder builder)
        {
            if (builder == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionFieldSymbolBuilder)builder).ReflectionBuilder;
#else
            return ((ReflectionFieldSymbolBuilder)builder).ReflectionBuilder;
#endif
        }

    }

}
