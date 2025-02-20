﻿using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class ReflectionModuleSymbol : ReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> is a TypeDef. That is, not a modified or substituted or generic parameter type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsTypeDefinition(Type type)
        {
#if NET
			return type.IsTypeDefinition;
#else
            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
#endif
        }

        readonly Module _underlyingModule;

        Type[]? _typesSource;
        int _typesBaseRow;
        ReflectionTypeSymbol?[]? _types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingModule"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleSymbol(ReflectionSymbolContext context, Module underlyingModule) :
            base(context)
        {
            _underlyingModule = underlyingModule ?? throw new ArgumentNullException(nameof(underlyingModule));
        }

        /// <summary>
        /// Gets the wrapped <see cref="Module"/>.
        /// </summary>
        internal Module UnderlyingModule => _underlyingModule;

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _underlyingModule);

            // type is not a definition, but is substituted
            if (IsTypeDefinition(type) == false)
                return GetOrCreateTypeSymbolForSpecification(type);

            // look up handle and row
            var hnd = MetadataTokens.TypeDefinitionHandle(type.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source table
            if (_typesSource == null)
            {
                Interlocked.CompareExchange(ref _typesSource, _underlyingModule.GetTypes().OrderBy(i => i.MetadataToken).ToArray(), null);
                _typesBaseRow = _typesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_typesSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            if (_types == null)
                Interlocked.CompareExchange(ref _types, new ReflectionTypeSymbol?[_typesSource.Length], null);

            // index of current record is specified row - base
            var idx = row - _typesBaseRow;
            if (idx < 0)
                throw new Exception();

            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _typesSource.Length);

            // check that our type list is long enough to contain the entire table
            if (_types.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            if (_types[idx] == null)
                Interlocked.CompareExchange(ref _types[idx], new ReflectionTypeSymbol(Context, this, type), null);

            // this should never happen
            if (_types[idx] is not ReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// For a given 
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="NotImplementedException"></exception>
        ReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _underlyingModule);

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = GetOrCreateTypeSymbol(elementType);

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

                throw new InvalidOperationException();
            }

            if (type.IsGenericType)
            {
                var definitionType = type.GetGenericTypeDefinition();
                var definitionTypeSymbol = GetOrCreateTypeSymbol(definitionType);
                return definitionTypeSymbol.GetOrCreateGenericTypeSymbol(type.GetGenericArguments());
            }

            // generic type parameter
            if (type.IsGenericParameter && type.DeclaringMethod is null && type.DeclaringType is not null)
            {
                var declaringType = GetOrCreateTypeSymbol(type.DeclaringType);
                return declaringType.GetOrCreateGenericParameterSymbol(type);
            }

            // generic method parameter
            if (type.IsGenericParameter && type.DeclaringMethod is not null && type.DeclaringMethod.DeclaringType is not null)
            {
                var declaringMethod = GetOrCreateTypeSymbol(type.DeclaringMethod.DeclaringType);
                return declaringMethod.GetOrCreateGenericParameterSymbol(type);
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_underlyingModule.Assembly);

        /// <inheritdoc />
        public string FullyQualifiedName => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public int MetadataToken => _underlyingModule.MetadataToken;

        /// <inheritdoc />
        public Guid ModuleVersionId => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public string Name => _underlyingModule.Name;

        /// <inheritdoc />
        public string ScopeName => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _underlyingModule.GetField(name) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return _underlyingModule.GetField(name, bindingAttr) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(_underlyingModule.GetFields(bindingFlags));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(_underlyingModule.GetFields());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _underlyingModule.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _underlyingModule.GetMethod(name, UnpackTypeSymbols(types)) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _underlyingModule.GetMethod(name, bindingAttr, null, callConvention, UnpackTypeSymbols(types), modifiers) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(_underlyingModule.GetMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(_underlyingModule.GetMethods(bindingFlags));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className)
        {
            return _underlyingModule.GetType(className) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool ignoreCase)
        {
            return _underlyingModule.GetType(className, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
        {
            return _underlyingModule.GetType(className, throwOnError, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_underlyingModule.GetTypes());
        }

        /// <inheritdoc />
        public bool IsResource()
        {
            return _underlyingModule.IsResource();
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken)
        {
            return _underlyingModule.ResolveField(metadataToken) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _underlyingModule.ResolveField(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken)
        {
            return _underlyingModule.ResolveMember(metadataToken) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _underlyingModule.ResolveMember(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _underlyingModule.ResolveMethod(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken)
        {
            return _underlyingModule.ResolveMethod(metadataToken) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public byte[] ResolveSignature(int metadataToken)
        {
            return _underlyingModule.ResolveSignature(metadataToken);
        }

        /// <inheritdoc />
        public string ResolveString(int metadataToken)
        {
            return _underlyingModule.ResolveString(metadataToken);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken)
        {
            return ResolveTypeSymbol(_underlyingModule.ResolveType(metadataToken));
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return ResolveTypeSymbol(_underlyingModule.ResolveType(metadataToken, _genericTypeArguments, _genericMethodArguments));
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return ResolveCustomAttributes(_underlyingModule.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            var underlyingAttributeType = ((ReflectionTypeSymbol)attributeType).UnderlyingType;
            return ResolveCustomAttributes(_underlyingModule.GetCustomAttributesData(), i => underlyingAttributeType.IsAssignableFrom(i.AttributeType));
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingModule.IsDefined(((ReflectionTypeSymbol)attributeType).UnderlyingType);
        }

    }

}
