using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    abstract class IkvmReflectionMemberSymbolBuilder : IkvmReflectionSymbolBuilder, IIkvmReflectionMemberSymbolBuilder
    {

        readonly IIkvmReflectionModuleSymbolBuilder _resolvingModule;
        readonly IIkvmReflectionTypeSymbolBuilder? _resolvingType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public IkvmReflectionMemberSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionTypeSymbolBuilder? resolvingType) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingType = resolvingType;
        }

        #region IIkvmReflectionMemberSymbol

        /// <inheritdoc />
        public abstract MemberInfo UnderlyingMember { get; }

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbolBuilder ResolvingModuleBuilder => _resolvingModule;

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol? ResolvingType => _resolvingType;

        #endregion

        #region IIkvmReflectionSymbolBuilder

        /// <inheritdoc />
        [return: NotNullIfNotNull("type")]
        public override IIkvmReflectionTypeSymbolBuilder ResolveTypeSymbol(TypeBuilder type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (UnderlyingMember == type)
                return (IIkvmReflectionTypeSymbolBuilder)this;

            if (_resolvingType != null && type == _resolvingType.UnderlyingType)
                return (IIkvmReflectionTypeSymbolBuilder)_resolvingType;

            if (type.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateTypeSymbol(type);

            return base.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public override IIkvmReflectionConstructorSymbolBuilder ResolveConstructorSymbol(ConstructorBuilder ctor)
        {
            if (ctor is null)
                throw new ArgumentNullException(nameof(ctor));

            if (UnderlyingMember == ctor)
                return (IIkvmReflectionConstructorSymbolBuilder)this;

            if (ctor.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateConstructorSymbol(ctor);

            return base.ResolveConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public override IIkvmReflectionMethodSymbolBuilder ResolveMethodSymbol(MethodBuilder method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            if (UnderlyingMember == method)
                return (IIkvmReflectionMethodSymbolBuilder)this;

            if (method.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateMethodSymbol(method);

            return base.ResolveMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public override IIkvmReflectionFieldSymbolBuilder ResolveFieldSymbol(FieldBuilder field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            if (UnderlyingMember == field)
                return (IIkvmReflectionFieldSymbolBuilder)this;

            if (field.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateFieldSymbol(field);

            return base.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public override IIkvmReflectionPropertySymbolBuilder ResolvePropertySymbol(PropertyBuilder property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (UnderlyingMember == property)
                return (IIkvmReflectionPropertySymbolBuilder)this;

            if (property.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreatePropertySymbol(property);

            return base.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public override IIkvmReflectionEventSymbolBuilder ResolveEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (@event.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateEventSymbol(@event);

            return base.ResolveEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IIkvmReflectionParameterSymbolBuilder? ResolveParameterSymbol(IIkvmReflectionMethodBaseSymbolBuilder method, ParameterBuilder parameter)
        {
            if (parameter is null)
                return null;

            if (parameter.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateParameterSymbol(method, parameter);

            return base.ResolveParameterSymbol(method, parameter);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IIkvmReflectionParameterSymbolBuilder? ResolveParameterSymbol(IIkvmReflectionPropertySymbolBuilder property, ParameterBuilder parameter)
        {
            if (parameter is null)
                return null;

            if (parameter.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateParameterSymbol(property, parameter);

            return base.ResolveParameterSymbol(property, parameter);
        }

        #endregion

        #region IIkvmReflectionSymbol

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public override IIkvmReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type is null)
                return null;

            if (UnderlyingMember == type)
                return (IIkvmReflectionTypeSymbol)this;

            if (_resolvingType != null && type == _resolvingType.UnderlyingType)
                return _resolvingType;

            if (type.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateTypeSymbol(type);

            return base.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public override IIkvmReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
        {
            if (ctor is null)
                return null;

            if (UnderlyingMember == ctor)
                return (IIkvmReflectionConstructorSymbol)this;

            if (ctor.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateConstructorSymbol(ctor);

            return base.ResolveConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public override IIkvmReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method)
        {
            if (method is null)
                return null;

            if (UnderlyingMember == method)
                return (IIkvmReflectionMethodSymbol)this;

            if (method.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateMethodSymbol(method);

            return base.ResolveMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public override IIkvmReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field)
        {
            if (field is null)
                return null;

            if (UnderlyingMember == field)
                return (IIkvmReflectionFieldSymbol)this;

            if (field.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateFieldSymbol(field);

            return base.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public override IIkvmReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property)
        {
            if (property is null)
                return null;

            if (UnderlyingMember == property)
                return (IIkvmReflectionPropertySymbol)this;

            if (property.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreatePropertySymbol(property);

            return base.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public override IIkvmReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event)
        {
            if (@event is null)
                return null;

            if (UnderlyingMember == @event)
                return (IIkvmReflectionEventSymbol)this;

            if (@event.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateEventSymbol(@event);

            return base.ResolveEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IIkvmReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            if (parameter.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateParameterSymbol(parameter);

            return base.ResolveParameterSymbol(parameter);
        }

        #endregion

        #region IMemberSymbol

        /// <inheritdoc />
        public virtual IModuleSymbol Module => ResolveModuleSymbol(UnderlyingMember.Module)!;

        /// <inheritdoc />
        public virtual ITypeSymbol? DeclaringType => ResolveTypeSymbol(UnderlyingMember.DeclaringType)!;

        /// <inheritdoc />
        public virtual System.Reflection.MemberTypes MemberType => (System.Reflection.MemberTypes)UnderlyingMember.MemberType;

        /// <inheritdoc />
        public virtual int MetadataToken => UnderlyingMember.MetadataToken;

        /// <inheritdoc />
        public virtual string Name => UnderlyingMember.Name;

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingMember.__GetCustomAttributes(null, inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingMember.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            var a = UnderlyingMember.__GetCustomAttributes(_attributeType, inherit);
            if (a.Count > 0)
                return ResolveCustomAttribute(a[0]);

            return null;
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingMember.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public abstract void SetCustomAttribute(CustomAttribute attribute);

        #endregion

        /// <inheritdoc />
        public virtual void OnComplete()
        {

        }

        /// <inheritdoc />
        public override string ToString() => UnderlyingMember.ToString()!;
    }

}
