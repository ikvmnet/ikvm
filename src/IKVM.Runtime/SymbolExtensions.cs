using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;

using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    static class SymbolExtensions
    {

        public static Assembly AsReflection(this AssemblySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveAssembly(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveAssembly(symbol);
#endif
        }

        public static Module AsReflection(this ModuleSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveModule(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveModule(symbol);
#endif
        }

        public static Type AsReflection(this TypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveType(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveType(symbol);
#endif
        }

        public static Type[] GetUnderlyingTypes(this TypeSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new Type[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = AsReflection(symbols[i]);

            return a;
        }

        public static MethodBase AsReflection(this MethodSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveMethod(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveMethod(symbol);
#endif
        }

        public static ParameterInfo AsReflection(this ParameterSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveParameter(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveParameter(symbol);
#endif
        }

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/> of the specified symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static FieldInfo AsReflection(this FieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveField(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveField(symbol);
#endif
        }

        public static FieldInfo[] GetUnderlyingFields(this FieldSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new FieldInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = AsReflection(symbols[i]);

            return a;
        }

        public static PropertyInfo GetUnderlyingProperty(this PropertySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveProperty(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveProperty(symbol);
#endif
        }

        public static PropertyInfo[] GetUnderlyingProperties(this PropertySymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new PropertyInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingProperty(symbols[i]);

            return a;
        }

    }

}
