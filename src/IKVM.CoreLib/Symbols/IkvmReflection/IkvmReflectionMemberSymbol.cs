using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    abstract class IkvmReflectionMemberSymbol : IkvmReflectionSymbol, IIkvmReflectionMemberSymbol
    {

        readonly IIkvmReflectionModuleSymbol _resolvingModule;
        readonly IIkvmReflectionTypeSymbol? _resolvingType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="member"></param>
        public IkvmReflectionMemberSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionTypeSymbol? resolvingType) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingType = resolvingType;
        }

        /// <inheritdoc />
        public abstract MemberInfo UnderlyingMember { get; }

        /// <summary>
        /// Gets the <see cref="IIkvmReflectionModuleSymbol" /> which contains the metadata of this member.
        /// </summary>
        public IIkvmReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <summary>
        /// Gets the <see cref="IIkvmReflectionTypeSymbol" /> which contains the metadata of this member, or null if the member is not a member of a type.
        /// </summary>
        public IIkvmReflectionTypeSymbol? ResolvingType => _resolvingType;

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public override IIkvmReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type is null)
                return null;

            if (type == UnderlyingMember)
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

            if (ctor == UnderlyingMember)
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

            if (method == UnderlyingMember)
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

            if (field == UnderlyingMember)
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

            if (property == UnderlyingMember)
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

            if (@event == UnderlyingMember)
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
        public virtual CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingMember.__GetCustomAttributes(null, inherit))!;
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingMember.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttribute(UnderlyingMember.__GetCustomAttributes(attributeType.Unpack(), inherit).FirstOrDefault());
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingMember.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

    }

}
