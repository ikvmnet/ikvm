using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    abstract class ReflectionMemberSymbol : ReflectionSymbol, IReflectionMemberSymbol
    {

        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol? _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="member"></param>
        public ReflectionMemberSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType) :
            base(context)
        {
            _module = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _type = resolvingType;
        }

        /// <inheritdoc />
        public abstract MemberInfo UnderlyingMember { get; }

        /// <summary>
        /// Gets the <see cref="IReflectionModuleSymbol" /> which contains the metadata of this member.
        /// </summary>
        public IReflectionModuleSymbol ResolvingModule => _module;

        /// <summary>
        /// Gets the <see cref="IReflectionTypeSymbol" /> which contains the metadata of this member, or null if the member is not a member of a type.
        /// </summary>
        public IReflectionTypeSymbol? ResolvingType => _type;

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public override IReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type is null)
                return null;

            if (type == UnderlyingMember)
                return (IReflectionTypeSymbol)this;

            if (_type != null && type == _type.UnderlyingType)
                return _type;

            if (type.Module == _module.UnderlyingModule)
                return _module.GetOrCreateTypeSymbol(type);

            return base.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public override IReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
        {
            if (ctor is null)
                return null;

            if (ctor == UnderlyingMember)
                return (IReflectionConstructorSymbol)this;

            if (ctor.Module == _module.UnderlyingModule)
                return _module.GetOrCreateConstructorSymbol(ctor);

            return base.ResolveConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public override IReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method)
        {
            if (method is null)
                return null;

            if (method == UnderlyingMember)
                return (IReflectionMethodSymbol)this;

            if (method.Module == _module.UnderlyingModule)
                return _module.GetOrCreateMethodSymbol(method);

            return base.ResolveMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public override IReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field)
        {
            if (field is null)
                return null;

            if (field == UnderlyingMember)
                return (IReflectionFieldSymbol)this;

            if (field.Module == _module.UnderlyingModule)
                return _module.GetOrCreateFieldSymbol(field);

            return base.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public override IReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property)
        {
            if (property is null)
                return null;

            if (property == UnderlyingMember)
                return (IReflectionPropertySymbol)this;

            if (property.Module == _module.UnderlyingModule)
                return _module.GetOrCreatePropertySymbol(property);

            return base.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public override IReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event)
        {
            if (@event is null)
                return null;

            if (@event == UnderlyingMember)
                return (IReflectionEventSymbol)this;

            if (@event.Module == _module.UnderlyingModule)
                return _module.GetOrCreateEventSymbol(@event);

            return base.ResolveEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            if (parameter.Member.Module == _module.UnderlyingModule)
                return _module.GetOrCreateParameterSymbol(parameter);

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
            if (inherit == true)
                throw new NotSupportedException();

            return ResolveCustomAttributes(UnderlyingMember.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            if (inherit == true)
                throw new NotSupportedException();

            var _attribyteType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingMember.GetCustomAttributesData().Where(i => i.AttributeType == _attribyteType).ToArray());
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            if (inherit == true)
                throw new NotSupportedException();

            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttribute(UnderlyingMember.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).FirstOrDefault());
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingMember.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        public override string? ToString() => UnderlyingMember.ToString();

    }

}
