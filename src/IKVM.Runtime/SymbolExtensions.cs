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

        public static Assembly GetUnderlyingAssembly(this AssemblySymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveAssembly(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveAssembly(symbol);
#endif
        }

        public static Module GetUnderlyingModule(this ModuleSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveModule(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveModule(symbol);
#endif
        }

        public static Type GetUnderlyingType(this TypeSymbol symbol)
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
                a[i] = GetUnderlyingType(symbols[i]);

            return a;
        }

        public static Type GetUnderlyingRuntimeType(this TypeSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            throw new NotSupportedException();
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveType(symbol, ReflectionSymbolState.Completed);
#endif
        }

        public static Type[] GetUnderlyingRuntimeTypes(this TypeSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new Type[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingRuntimeType(symbols[i]);

            return a;
        }

        public static MethodBase GetUnderlyingMethod(this MethodSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveMethod(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveMethod(symbol);
#endif
        }

        public static ParameterInfo GetUnderlyingParameter(this ParameterSymbol symbol)
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
        public static FieldInfo GetUnderlyingField(this FieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            return ((IkvmReflectionSymbolContext)symbol.Context).ResolveField(symbol);
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveField(symbol);
#endif
        }

        /// <summary>
        /// Gets the underlying runtime <see cref="FieldInfo"/> of the specified symbol. May result in a reification of type symbols.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static FieldInfo GetUnderlyingRuntimeField(this FieldSymbol symbol)
        {
            if (symbol == null)
                return null;

#if IMPORTER || EXPORTER
            throw new NotSupportedException();
#else
            return ((ReflectionSymbolContext)symbol.Context).ResolveField(symbol, ReflectionSymbolState.Completed);
#endif
        }

        public static FieldInfo[] GetUnderlyingFields(this FieldSymbol[] symbols)
        {
            if (symbols == null)
                return null;

            var a = new FieldInfo[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
                a[i] = GetUnderlyingField(symbols[i]);

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
