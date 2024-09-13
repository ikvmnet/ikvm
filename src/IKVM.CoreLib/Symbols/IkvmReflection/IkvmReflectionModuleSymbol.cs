using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Reflection;

using Module = IKVM.Reflection.Module;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class IkvmReflectionModuleSymbol : IkvmReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> is a TypeDef. That is, not a modified or substituted or generic parameter type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsTypeDefinition(Type type)
        {
            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
        }

        readonly Module _module;

        Type[]? _typesSource;
        int _typesBaseRow;
        IkvmReflectionTypeSymbol?[]? _types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleSymbol(IkvmReflectionSymbolContext context, Module module) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Gets the wrapped <see cref="Module"/>.
        /// </summary>
        internal Module ReflectionObject => _module;

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            // type is not a definition, but is substituted
            if (IsTypeDefinition(type) == false)
                return GetOrCreateTypeSymbolForSpecification(type);

            // look up handle and row
            var hnd = MetadataTokens.TypeDefinitionHandle(type.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source table
            if (_typesSource == null)
            {
                _typesSource = _module.GetTypes().OrderBy(i => i.MetadataToken).ToArray();
                _typesBaseRow = _typesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_typesSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            _types ??= new IkvmReflectionTypeSymbol?[_typesSource.Length];

            // index of current record is specified row - base
            var idx = row - _typesBaseRow - 1;
            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _typesSource.Length);

            // check that our type list is long enough to contain the entire table
            if (_types.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            if (_types[idx] == null)
                Interlocked.CompareExchange(ref _types[idx], new IkvmReflectionTypeSymbol(Context, this, type), null);

            // this should never happen
            if (_types[idx] is not IkvmReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// For a given 
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="NotImplementedException"></exception>
        IkvmReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = GetOrCreateTypeSymbol(elementType);

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

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
        public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_module.Assembly);

        /// <inheritdoc />
        public string FullyQualifiedName => _module.FullyQualifiedName;

        /// <inheritdoc />
        public int MetadataToken => _module.MetadataToken;

        /// <inheritdoc />
        public Guid ModuleVersionId => _module.ModuleVersionId;

        /// <inheritdoc />
        public string Name => _module.Name;

        /// <inheritdoc />
        public string ScopeName => _module.ScopeName;

        /// <inheritdoc />
        public override bool IsMissing => _module.__IsMissing;

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _module.GetField(name) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _module.GetField(name, (BindingFlags)bindingAttr) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(_module.GetFields((BindingFlags)bindingFlags));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(_module.GetFields());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _module.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _module.GetMethod(name, UnpackTypeSymbols(types)) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            if (modifiers != null)
                throw new NotImplementedException();

            return _module.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, UnpackTypeSymbols(types), null) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(_module.GetMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(_module.GetMethods((BindingFlags)bindingFlags));
        }
        /// <inheritdoc />

        public ITypeSymbol? GetType(string className)
        {
            return _module.GetType(className) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool ignoreCase)
        {
            return _module.GetType(className, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
        {
            return _module.GetType(className, throwOnError, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_module.GetTypes());
        }

        /// <inheritdoc />
        public bool IsResource()
        {
            return _module.IsResource();
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken)
        {
            return _module.ResolveField(metadataToken) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _module.ResolveField(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken)
        {
            return _module.ResolveMember(metadataToken) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _module.ResolveMember(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return _module.ResolveMethod(metadataToken, _genericTypeArguments, _genericMethodArguments) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken)
        {
            return _module.ResolveMethod(metadataToken) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public byte[] ResolveSignature(int metadataToken)
        {
            return _module.ResolveSignature(metadataToken);
        }

        /// <inheritdoc />
        public string ResolveString(int metadataToken)
        {
            return _module.ResolveString(metadataToken);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken)
        {
            return ResolveTypeSymbol(_module.ResolveType(metadataToken));
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            var _genericTypeArguments = genericTypeArguments != null ? UnpackTypeSymbols(genericTypeArguments) : null;
            var _genericMethodArguments = genericMethodArguments != null ? UnpackTypeSymbols(genericMethodArguments) : null;
            return ResolveTypeSymbol(_module.ResolveType(metadataToken, _genericTypeArguments, _genericMethodArguments));
        }

        /// <inheritdoc />
        public CustomAttributeSymbol[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(_module.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_module.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false));
        }

        /// <inheritdoc />
        public CustomAttributeSymbol? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _module.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

    }

}
