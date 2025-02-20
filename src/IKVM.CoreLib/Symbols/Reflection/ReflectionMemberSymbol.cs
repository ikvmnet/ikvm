﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    abstract class ReflectionMemberSymbol : ReflectionSymbol, IMemberSymbol
    {

        readonly ReflectionModuleSymbol _module;
        readonly ReflectionTypeSymbol? _type;
        readonly MemberInfo _underlyingMember;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMember"></param>
        public ReflectionMemberSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? type, MemberInfo underlyingMember) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
            _underlyingMember = underlyingMember ?? throw new ArgumentNullException(nameof(underlyingMember));
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal override ReflectionTypeSymbol ResolveTypeSymbol(Type type)
        {
            if (_type != null && type == _type.UnderlyingType)
                return _type;
            else if (type.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(type);
            else
                return base.ResolveTypeSymbol(type);
        }

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        protected internal override ReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
        {
            if (_type != null && ctor.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreateConstructorSymbol(ctor);
            else if (ctor.DeclaringType != null && ctor.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(ctor.DeclaringType).GetOrCreateConstructorSymbol(ctor);
            else
                return base.ResolveConstructorSymbol(ctor);
        }

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected internal override ReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
        {
            if (_type != null && method.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreateMethodSymbol(method);
            else if (method.DeclaringType != null && method.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(method.DeclaringType).GetOrCreateMethodSymbol(method);
            else
                return base.ResolveMethodSymbol(method);
        }

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected internal override ReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
        {
            if (_type != null && field.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreateFieldSymbol(field);
            else if (field.DeclaringType != null && field.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(field.DeclaringType).GetOrCreateFieldSymbol(field);
            else
                return base.ResolveFieldSymbol(field);
        }


        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected internal override ReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
        {

            if (_type != null && property.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreatePropertySymbol(property);
            else if (property.DeclaringType != null && property.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(property.DeclaringType).GetOrCreatePropertySymbol(property);
            else
                return base.ResolvePropertySymbol(property);
        }

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected internal override ReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
        {
            if (_type != null && @event.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreateEventSymbol(@event);
            else if (@event.DeclaringType != null && @event.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
            else
                return base.ResolveEventSymbol(@event);
        }

        /// <summary>
        /// Gets the underlying <see cref="MemberInfo"/> wrapped by this symbol.
        /// </summary>
        internal MemberInfo UnderlyingMember => _underlyingMember;

        /// <summary>
        /// Gets the <see cref="ReflectionModuleSymbol" /> which contains the metadata of this member.
        /// </summary>
        internal ReflectionModuleSymbol ContainingModule => _module;

        /// <summary>
        /// Gets the <see cref="ReflectionTypeSymbol" /> which contains the metadata of this member, or null if the member is not a member of a type.
        /// </summary>
        internal ReflectionTypeSymbol? ContainingType => _type;

        /// <inheritdoc />
        public virtual IModuleSymbol Module => Context.GetOrCreateModuleSymbol(_underlyingMember.Module);

        /// <inheritdoc />
        public virtual ITypeSymbol? DeclaringType => _underlyingMember.DeclaringType is Type t ? Context.GetOrCreateTypeSymbol(t) : null;

        /// <inheritdoc />
        public virtual MemberTypes MemberType => _underlyingMember.MemberType;

        /// <inheritdoc />
        public virtual int MetadataToken => _underlyingMember.MetadataToken;

        /// <inheritdoc />
        public virtual string Name => _underlyingMember.Name;

        /// <inheritdoc />
        public virtual ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return ResolveCustomAttributes(_underlyingMember.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_underlyingMember.GetCustomAttributesData()).Where(i => i.AttributeType == attributeType).ToImmutableArray();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingMember.IsDefined(((ReflectionTypeSymbol)attributeType).UnderlyingType);
        }

    }

}