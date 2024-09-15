using System;
using System.Collections.Generic;
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

        readonly IkvmReflectionModuleSymbol _module;
        readonly IkvmReflectionTypeSymbol? _type;
        readonly MemberInfo _member;

        CustomAttribute[]? _customAttributes;
        CustomAttribute[]? _inheritedCustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="member"></param>
        public IkvmReflectionMemberSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? type, MemberInfo member) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionTypeSymbol ResolveTypeSymbol(Type type)
        {
            if (_type != null && type == _type.ReflectionObject)
                return _type;
            else if (type.Module == _member.Module)
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
            if (_type != null && ctor.DeclaringType == _type.ReflectionObject)
                return _type.GetOrCreateConstructorSymbol(ctor);
            else if (ctor.DeclaringType != null && ctor.Module == _member.Module)
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
            if (_type != null && method.DeclaringType == _type.ReflectionObject)
                return _type.GetOrCreateMethodSymbol(method);
            else if (method.DeclaringType != null && method.Module == _member.Module)
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
            if (_type != null && field.DeclaringType == _type.ReflectionObject)
                return _type.GetOrCreateFieldSymbol(field);
            else if (field.DeclaringType != null && field.Module == _member.Module)
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

            if (_type != null && property.DeclaringType == _type.ReflectionObject)
                return _type.GetOrCreatePropertySymbol(property);
            else if (property.DeclaringType != null && property.Module == _member.Module)
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
            if (_type != null && @event.DeclaringType == _type.ReflectionObject)
                return _type.GetOrCreateEventSymbol(@event);
            else if (@event.DeclaringType != null && @event.Module == _member.Module)
                return _module.GetOrCreateTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
            else
                return base.ResolveEventSymbol(@event);
        }

        /// <summary>
        /// Gets the wrapped <see cref="MemberInfo"/>.
        /// </summary>
        internal MemberInfo ReflectionObject => _member;

        /// <inheritdoc />
        internal IkvmReflectionModuleSymbol ContainingModule => _module;

        /// <inheritdoc />
        internal IkvmReflectionTypeSymbol? ContainingType => _type;

        /// <inheritdoc />
        public virtual ITypeSymbol? DeclaringType => _member.DeclaringType is Type t ? Context.GetOrCreateTypeSymbol(t) : null;

        /// <inheritdoc />
        public virtual System.Reflection.MemberTypes MemberType => (System.Reflection.MemberTypes)_member.MemberType;

        /// <inheritdoc />
        public virtual int MetadataToken => _member.MetadataToken;

        /// <inheritdoc />
        public virtual IModuleSymbol Module => Context.GetOrCreateModuleSymbol(_member.Module);

        /// <inheritdoc />
        public virtual string Name => _member.Name;

        /// <inheritdoc />
        public override bool IsMissing => _member.__IsMissing;

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (inherit)
            {
                // check that we've already processed this
                if (_inheritedCustomAttributes != null)
                    return _inheritedCustomAttributes;

                var self = _member;
                var list = default(List<CustomAttribute[]>?);
                for (; ; )
                {
                    // get attribute for current member and append to list
                    var attr = ResolveMemberSymbol(self).GetCustomAttributes(false);
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

            return _customAttributes ??= ResolveCustomAttributes(_member.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_member.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _member.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, inherit);
        }

    }

}