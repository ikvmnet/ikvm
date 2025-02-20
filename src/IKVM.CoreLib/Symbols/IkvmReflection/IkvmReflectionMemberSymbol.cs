using System;
using System.Collections.Immutable;
using System.Linq;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MemberInfo = IKVM.Reflection.MemberInfo;
using MethodInfo = IKVM.Reflection.MethodInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    abstract class IkvmReflectionMemberSymbol : IkvmReflectionSymbol, IMemberSymbol
    {

        readonly IkvmReflectionModuleSymbol _module;
        readonly IkvmReflectionTypeSymbol? _type;
        readonly MemberInfo _underlyingMember;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMember"></param>
        public IkvmReflectionMemberSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? type, MemberInfo underlyingMember) :
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
        protected internal override IkvmReflectionTypeSymbol ResolveTypeSymbol(Type type)
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
        protected internal override IkvmReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
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
        protected internal override IkvmReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
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
        protected internal override IkvmReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
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
        protected internal override IkvmReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
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
        protected internal override IkvmReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
        {
            if (_type != null && @event.DeclaringType == _type.UnderlyingType)
                return _type.GetOrCreateEventSymbol(@event);
            else if (@event.DeclaringType != null && @event.Module == _underlyingMember.Module)
                return _module.GetOrCreateTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
            else
                return base.ResolveEventSymbol(@event);
        }

        internal MemberInfo UnderlyingMember => _underlyingMember;

        internal IkvmReflectionModuleSymbol ContainingModule => _module;

        internal IkvmReflectionTypeSymbol? ContainingType => _type;

        public virtual ITypeSymbol? DeclaringType => _underlyingMember.DeclaringType is Type t ? Context.GetOrCreateTypeSymbol(t) : null;

        public virtual System.Reflection.MemberTypes MemberType => (System.Reflection.MemberTypes)_underlyingMember.MemberType;

        public virtual int MetadataToken => _underlyingMember.MetadataToken;

        public virtual IModuleSymbol Module => Context.GetOrCreateModuleSymbol(_underlyingMember.Module);

        public virtual string Name => _underlyingMember.Name;

        public override bool IsMissing => _underlyingMember.__IsMissing;

        public virtual ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return ResolveCustomAttributes(_underlyingMember.GetCustomAttributesData());
        }

        public virtual ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_underlyingMember.GetCustomAttributesData()).Where(i => i.AttributeType == attributeType).ToImmutableArray();
        }

        public virtual bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingMember.IsDefined(((IkvmReflectionTypeSymbol)attributeType).UnderlyingType, false);
        }

    }

}