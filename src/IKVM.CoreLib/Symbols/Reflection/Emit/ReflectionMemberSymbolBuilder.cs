using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    abstract class ReflectionMemberSymbolBuilder : ReflectionSymbolBuilder, IReflectionMemberSymbolBuilder
    {

        /// <summary>
        /// Concatinates the list of arrays.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static T[] Concat<T>(List<T[]>? list)
        {
            if (list == null)
                return [];

            var c = 0;
            foreach (var i in list)
                c += i.Length;

            if (c == 0)
                return [];

            var t = 0;
            var a = new T[c];
            foreach (var i in list)
            {
                Array.Copy(i, 0, a, t, i.Length);
                t += i.Length;
            }

            return a;
        }

        readonly IReflectionModuleSymbol _resolvingModule;
        readonly IReflectionTypeSymbol? _resolvingType;

        CustomAttribute[]? _customAttributes;
        CustomAttribute[]? _inheritedCustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public ReflectionMemberSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingType = resolvingType;
        }

        #region IMemberSymbol

        /// <inheritdoc />
        public abstract MemberInfo UnderlyingMember { get; }

        /// <inheritdoc />
        public IReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <inheritdoc />
        public IReflectionTypeSymbol? ResolvingType => ResolvingType;

        /// <inheritdoc />
        public virtual IModuleSymbol Module => ResolveModuleSymbol(UnderlyingMember.Module)!;

        /// <inheritdoc />
        public virtual ITypeSymbol? DeclaringType => ResolveTypeSymbol(UnderlyingMember.DeclaringType)!;

        /// <inheritdoc />
        public virtual MemberTypes MemberType => UnderlyingMember.MemberType;

        /// <inheritdoc />
        public virtual int MetadataToken => UnderlyingMember.GetMetadataTokenSafe();

        /// <inheritdoc />
        public virtual string Name => UnderlyingMember.Name;

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (inherit)
            {
                // check that we've already processed this
                if (_inheritedCustomAttributes != null)
                    return _inheritedCustomAttributes;

                var self = UnderlyingMember;
                var list = default(List<CustomAttribute[]>?);
                for (; ; )
                {
                    if (self == null)
                        throw new InvalidOperationException();

                    // get attribute for current member and append to list
                    var attr = ResolveMemberSymbol(self)!.GetCustomAttributes(false);
                    if (attr.Length > 0)
                    {
                        list ??= [];
                        list.Add(attr);
                    }

                    var type = self as Type;
                    if (type != null)
                    {
                        type = type.BaseType;
                        if (type == null)
                            return _inheritedCustomAttributes ??= Concat(list);

                        self = type;
                        continue;
                    }

                    var method = self as MethodInfo;
                    if (method != null)
                    {
                        var prev = self;
                        method = method.GetBaseDefinition();
                        if (method == null || method == prev)
                            return _inheritedCustomAttributes ??= Concat(list);

                        self = method;
                        continue;
                    }

                    return _inheritedCustomAttributes ??= Concat(list);
                }
            }

            return _customAttributes ??= ResolveCustomAttributes(UnderlyingMember.GetCustomAttributesData())!;
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingMember.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public override IReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type is null)
                return null;

            if (UnderlyingMember == type)
                return (IReflectionTypeSymbol)this;

            if (_resolvingType != null && type == _resolvingType.UnderlyingType)
                return _resolvingType;

            if (type.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateTypeSymbol(type);

            return base.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull("type")]
        public override IReflectionTypeSymbolBuilder ResolveTypeSymbol(TypeBuilder type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (UnderlyingMember == type)
                return (IReflectionTypeSymbolBuilder)this;

            if (_resolvingType != null && type == _resolvingType.UnderlyingType)
                return (IReflectionTypeSymbolBuilder)_resolvingType;

            if (type.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateTypeSymbol(type);

            return base.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public override IReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
        {
            if (ctor is null)
                return null;

            if (UnderlyingMember == ctor)
                return (IReflectionConstructorSymbol)this;

            if (ctor.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateConstructorSymbol(ctor);

            return base.ResolveConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public override IReflectionConstructorSymbolBuilder ResolveConstructorSymbol(ConstructorBuilder ctor)
        {
            if (ctor is null)
                throw new ArgumentNullException(nameof(ctor));

            if (UnderlyingMember == ctor)
                return (IReflectionConstructorSymbolBuilder)this;

            if (ctor.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateConstructorSymbol(ctor);

            return base.ResolveConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public override IReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method)
        {
            if (method is null)
                return null;

            if (UnderlyingMember == method)
                return (IReflectionMethodSymbol)this;

            if (method.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateMethodSymbol(method);

            return base.ResolveMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public override IReflectionMethodSymbolBuilder ResolveMethodSymbol(MethodBuilder method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            if (UnderlyingMember == method)
                return (IReflectionMethodSymbolBuilder)this;

            if (method.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateMethodSymbol(method);

            return base.ResolveMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public override IReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field)
        {
            if (field is null)
                return null;

            if (UnderlyingMember == field)
                return (IReflectionFieldSymbol)this;

            if (field.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateFieldSymbol(field);

            return base.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public override IReflectionFieldSymbolBuilder ResolveFieldSymbol(FieldBuilder field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            if (UnderlyingMember == field)
                return (IReflectionFieldSymbolBuilder)this;

            if (field.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateFieldSymbol(field);

            return base.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public override IReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property)
        {
            if (property is null)
                return null;

            if (UnderlyingMember == property)
                return (IReflectionPropertySymbol)this;

            if (property.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreatePropertySymbol(property);

            return base.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public override IReflectionPropertySymbolBuilder ResolvePropertySymbol(PropertyBuilder property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (UnderlyingMember == property)
                return (IReflectionPropertySymbolBuilder)this;

            if (property.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreatePropertySymbol(property);

            return base.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public override IReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event)
        {
            if (@event is null)
                return null;

            if (UnderlyingMember == @event)
                return (IReflectionEventSymbol)this;

            if (@event.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateEventSymbol(@event);

            return base.ResolveEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public override IReflectionEventSymbolBuilder ResolveEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (@event.GetModuleBuilder() == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateEventSymbol(@event);

            return base.ResolveEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            if (parameter.Member.Module == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateParameterSymbol(parameter);

            return base.ResolveParameterSymbol(parameter);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public override IReflectionParameterSymbolBuilder ResolveParameterSymbol(ParameterBuilder parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            if (parameter.GetModuleBuilder() == _resolvingModule.UnderlyingModule)
                return _resolvingModule.GetOrCreateParameterSymbol(parameter);

            return base.ResolveParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public virtual void OnComplete()
        {

        }

    }

}
