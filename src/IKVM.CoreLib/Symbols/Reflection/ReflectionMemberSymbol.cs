using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    abstract class ReflectionMemberSymbol : ReflectionSymbol, IMemberSymbol
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

        readonly ReflectionModuleSymbol _containingModule;
        readonly ReflectionTypeSymbol? _containingType;
        readonly MemberInfo _member;

        CustomAttributeSymbol[]? _customAttributes;
        CustomAttributeSymbol[]? _inheritedCustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModule"></param>
        /// <param name="containingType"></param>
        /// <param name="member"></param>
        public ReflectionMemberSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol containingModule, ReflectionTypeSymbol? containingType, MemberInfo member) :
            base(context)
        {
            _containingModule = containingModule ?? throw new ArgumentNullException(nameof(containingModule));
            _containingType = containingType;
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal override ReflectionTypeSymbol ResolveTypeSymbol(Type type)
        {
            if (_containingType != null && type == _containingType.ReflectionObject)
                return _containingType;
            else if (type.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(type);
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
            if (_containingType != null && ctor.DeclaringType == _containingType.ReflectionObject)
                return _containingType.GetOrCreateConstructorSymbol(ctor);
            else if (ctor.DeclaringType != null && ctor.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(ctor.DeclaringType).GetOrCreateConstructorSymbol(ctor);
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
            if (_containingType != null && method.DeclaringType == _containingType.ReflectionObject)
                return _containingType.GetOrCreateMethodSymbol(method);
            else if (method.DeclaringType != null && method.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(method.DeclaringType).GetOrCreateMethodSymbol(method);
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
            if (_containingType != null && field.DeclaringType == _containingType.ReflectionObject)
                return _containingType.GetOrCreateFieldSymbol(field);
            else if (field.DeclaringType != null && field.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(field.DeclaringType).GetOrCreateFieldSymbol(field);
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

            if (_containingType != null && property.DeclaringType == _containingType.ReflectionObject)
                return _containingType.GetOrCreatePropertySymbol(property);
            else if (property.DeclaringType != null && property.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(property.DeclaringType).GetOrCreatePropertySymbol(property);
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
            if (_containingType != null && @event.DeclaringType == _containingType.ReflectionObject)
                return _containingType.GetOrCreateEventSymbol(@event);
            else if (@event.DeclaringType != null && @event.Module == _member.Module)
                return _containingModule.GetOrCreateTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
            else
                return base.ResolveEventSymbol(@event);
        }

        /// <summary>
        /// Gets the underlying <see cref="MemberInfo"/> wrapped by this symbol.
        /// </summary>
        internal MemberInfo ReflectionObject => _member;

        /// <summary>
        /// Gets the <see cref="ReflectionModuleSymbol" /> which contains the metadata of this member.
        /// </summary>
        internal ReflectionModuleSymbol ContainingModule => _containingModule;

        /// <summary>
        /// Gets the <see cref="ReflectionTypeSymbol" /> which contains the metadata of this member, or null if the member is not a member of a type.
        /// </summary>
        internal ReflectionTypeSymbol? ContainingType => _containingType;

        /// <inheritdoc />
        public virtual IModuleSymbol Module => Context.GetOrCreateModuleSymbol(_member.Module);

        /// <inheritdoc />
        public virtual ITypeSymbol? DeclaringType => _member.DeclaringType is Type t ? Context.GetOrCreateTypeSymbol(t) : null;

        /// <inheritdoc />
        public virtual MemberTypes MemberType => _member.MemberType;

        /// <inheritdoc />
        public virtual int MetadataToken => _member.MetadataToken;

        /// <inheritdoc />
        public virtual string Name => _member.Name;

        /// <inheritdoc />
        public virtual CustomAttributeSymbol[] GetCustomAttributes(bool inherit = false)
        {
            if (inherit)
            {
                // check that we've already processed this
                if (_inheritedCustomAttributes != null)
                    return _inheritedCustomAttributes;

                var self = _member;
                var list = default(List<CustomAttributeSymbol[]>?);
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
        public virtual CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        /// <inheritdoc />
        public virtual CustomAttributeSymbol? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _member.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject, inherit);
        }

    }

}