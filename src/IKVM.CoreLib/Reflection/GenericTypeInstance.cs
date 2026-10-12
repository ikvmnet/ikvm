/*
  Copyright (C) 2009-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Text;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a constructed generic type: a generic type definition together with its type arguments and the
    /// custom modifiers on those arguments. Members are obtained from the definition and bound to the type arguments.
    /// </summary>
    sealed class GenericTypeInstance : TypeInfo
    {

        readonly Type _type;
        readonly Type[] _args;
        readonly CustomModifiers[] _mods;
        Type _baseType;
        int _token;

        /// <summary>
        /// Returns the generic type instance for the specified definition and type arguments. When the type arguments
        /// are the definition's own type parameters, without custom modifiers, the definition itself is returned,
        /// except for type builders, baked types and missing types, which are always instantiated.
        /// </summary>
        /// <param name="type">The generic type definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <param name="mods">The custom modifiers on each type argument, or <see langword="null"/> if there are none.</param>
        /// <returns>The canonical generic type instance, or <paramref name="type"/> for the identity instantiation.</returns>
        internal static Type Make(Type type, Type[] typeArguments, CustomModifiers[] mods)
        {
            var identity = true;
            if (type is TypeBuilder || type is BakedType || type.__IsMissing)
            {
                // a TypeBuilder identity must be instantiated
                identity = false;
            }
            else
            {
                // we must not instantiate the identity instance, because typeof(Foo<>).MakeGenericType(typeof(Foo<>).GetGenericArguments()) == typeof(Foo<>)
                for (var i = 0; i < typeArguments.Length; i++)
                {
                    if (typeArguments[i] != type.GetGenericTypeArgument(i)
                        || !IsEmpty(mods, i))
                    {
                        identity = false;
                        break;
                    }
                }
            }
            if (identity)
            {
                return type;
            }
            else
            {
                return type.Universe.CanonicalizeType(new GenericTypeInstance(type, typeArguments, mods));
            }
        }

        /// <summary>
        /// Returns whether the type argument at the specified index has no custom modifiers.
        /// </summary>
        /// <param name="mods">The custom modifiers of the type arguments, or <see langword="null"/>.</param>
        /// <param name="i">The index of the type argument.</param>
        /// <returns><see langword="true"/> if the type argument has no custom modifiers; otherwise <see langword="false"/>.</returns>
        // we need to be extra careful, because mods does not need to be in canonical format
        // (Signature.ReadGenericInst() calls Make() directly, without copying the modifier arrays)
        static bool IsEmpty(CustomModifiers[] mods, int i) =>
            mods == null || mods[i].IsEmpty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type">The generic type definition.</param>
        /// <param name="args">The type arguments.</param>
        /// <param name="mods">The custom modifiers on each type argument, or <see langword="null"/>.</param>
        GenericTypeInstance(Type type, Type[] args, CustomModifiers[] mods)
        {
            _type = type;
            _args = args;
            _mods = mods;
        }

        /// <summary>
        /// Returns whether the specified object is a generic type instance with the same definition, type arguments
        /// and custom modifiers.
        /// </summary>
        /// <param name="o">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="o"/> is an equal generic type instance; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object o)
        {
            GenericTypeInstance gt = o as GenericTypeInstance;
            return gt != null && gt._type.Equals(_type) && Util.ArrayEquals(gt._args, _args)
                && Util.ArrayEquals(gt._mods, _mods);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _type.GetHashCode() * 3 ^ Util.GetHashCode(_args);

        /// <summary>
        /// Gets the assembly-qualified name of the type, or <see langword="null"/> if it has no full name because it
        /// contains generic parameters.
        /// </summary>
        public override string AssemblyQualifiedName
        {
            get
            {
                var fn = FullName;
                return fn == null ? null : fn + ", " + _type.Assembly.FullName;
            }
        }

        /// <summary>
        /// Gets the base type of the generic type definition, bound to this instance's type arguments.
        /// </summary>
        public override Type BaseType
        {
            get
            {
                if (_baseType == null)
                {
                    Type rawBaseType = _type.BaseType;
                    if (rawBaseType == null)
                    {
                        _baseType = rawBaseType;
                    }
                    else
                    {
                        _baseType = rawBaseType.BindTypeParameters(this);
                    }
                }
                return _baseType;
            }
        }

        /// <inheritdoc />
        protected override bool IsValueTypeImpl => _type.IsValueType;

        /// <summary>
        /// Gets a value indicating whether the type can be accessed by code outside the assembly, which requires the
        /// definition and all type arguments to be visible.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                if (base.IsVisible)
                {
                    foreach (Type arg in _args)
                    {
                        if (!arg.IsVisible)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the type that declares the generic type definition.
        /// </summary>
        public override Type DeclaringType => _type.DeclaringType;

        /// <summary>
        /// Gets the attributes of the generic type definition.
        /// </summary>
        public override TypeAttributes Attributes => _type.Attributes;

        /// <inheritdoc />
        internal override void CheckBaked() => _type.CheckBaked();

        /// <summary>
        /// Gets the fields declared by the generic type definition, bound to this instance's type arguments.
        /// </summary>
        /// <returns>The declared fields.</returns>
        public override FieldInfo[] __GetDeclaredFields()
        {
            FieldInfo[] fields = _type.__GetDeclaredFields();
            for (var i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].BindTypeParameters(this);
            }
            return fields;
        }

        /// <summary>
        /// Gets the interfaces declared by the generic type definition, bound to this instance's type arguments.
        /// </summary>
        /// <returns>The declared interfaces.</returns>
        public override Type[] __GetDeclaredInterfaces()
        {
            Type[] interfaces = _type.__GetDeclaredInterfaces();
            for (var i = 0; i < interfaces.Length; i++)
            {
                interfaces[i] = interfaces[i].BindTypeParameters(this);
            }
            return interfaces;
        }

        /// <summary>
        /// Gets the methods and constructors declared by the generic type definition, bound to this instance's type
        /// arguments.
        /// </summary>
        /// <returns>The declared methods and constructors.</returns>
        public override MethodBase[] __GetDeclaredMethods()
        {
            MethodBase[] methods = _type.__GetDeclaredMethods();
            for (var i = 0; i < methods.Length; i++)
            {
                methods[i] = methods[i].BindTypeParameters(this);
            }
            return methods;
        }

        /// <summary>
        /// Gets the nested types declared by the generic type definition. These are not bound to the type arguments.
        /// </summary>
        /// <returns>The declared nested types.</returns>
        public override Type[] __GetDeclaredTypes() => _type.__GetDeclaredTypes();

        /// <summary>
        /// Gets the events declared by the generic type definition, bound to this instance's type arguments.
        /// </summary>
        /// <returns>The declared events.</returns>
        public override EventInfo[] __GetDeclaredEvents()
        {
            EventInfo[] events = _type.__GetDeclaredEvents();
            for (var i = 0; i < events.Length; i++)
            {
                events[i] = events[i].BindTypeParameters(this);
            }
            return events;
        }

        /// <summary>
        /// Gets the properties declared by the generic type definition, bound to this instance's type arguments.
        /// </summary>
        /// <returns>The declared properties.</returns>
        public override PropertyInfo[] __GetDeclaredProperties()
        {
            PropertyInfo[] properties = _type.__GetDeclaredProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                properties[i] = properties[i].BindTypeParameters(this);
            }
            return properties;
        }

        /// <summary>
        /// Gets the method implementation map of the generic type definition, with the method bodies and the
        /// declarations on generic interfaces bound to this instance's type arguments.
        /// </summary>
        /// <returns>The method implementation map.</returns>
        public override __MethodImplMap __GetMethodImplMap()
        {
            __MethodImplMap map = _type.__GetMethodImplMap();
            map.TargetType = this;
            for (var i = 0; i < map.MethodBodies.Length; i++)
            {
                map.MethodBodies[i] = (MethodInfo)map.MethodBodies[i].BindTypeParameters(this);
                for (var j = 0; j < map.MethodDeclarations[i].Length; j++)
                {
                    Type interfaceType = map.MethodDeclarations[i][j].DeclaringType;
                    if (interfaceType.IsGenericType)
                    {
                        map.MethodDeclarations[i][j] = (MethodInfo)map.MethodDeclarations[i][j].BindTypeParameters(this);
                    }
                }
            }
            return map;
        }

        /// <summary>
        /// Gets the namespace of the generic type definition.
        /// </summary>
        public override string Namespace => _type.Namespace;

        /// <summary>
        /// Gets the name of the generic type definition.
        /// </summary>
        public override string Name => _type.Name;

        /// <summary>
        /// Gets the full name of the type, with each type argument written as an assembly-qualified name in
        /// brackets, or <see langword="null"/> if the type contains generic parameters and no missing types.
        /// </summary>
        public override string FullName
        {
            get
            {
                if (!__ContainsMissingType && ContainsGenericParameters)
                {
                    return null;
                }
                StringBuilder sb = new StringBuilder(_type.FullName);
                sb.Append('[');
                var sep = "";
                foreach (Type type in _args)
                {
                    sb.Append(sep).Append('[').Append(type.FullName).Append(", ").Append(type.Assembly.FullName.Replace("]", "\\]")).Append(']');
                    sep = ",";
                }
                sb.Append(']');
                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns the full name of the generic type definition followed by the type arguments in brackets.
        /// </summary>
        /// <returns>A string that represents the type.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_type.FullName);
            sb.Append('[');
            var sep = "";
            foreach (Type arg in _args)
            {
                sb.Append(sep);
                sb.Append(arg);
                sep = ",";
            }
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// Gets the module of the generic type definition.
        /// </summary>
        public override Module Module => _type.Module;

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type, which is always the case.
        /// </summary>
        public override bool IsGenericType => true;

        /// <summary>
        /// Gets a value that indicates whether this object represents a constructed generic type, which is always the
        /// case.
        /// </summary>
        public override bool IsConstructedGenericType => true;

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a generic type definition from which the current generic
        /// type can be constructed.
        /// </summary>
        /// <returns>The generic type definition.</returns>
        public override Type GetGenericTypeDefinition() => _type;

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of this closed generic type.
        /// </summary>
        /// <returns>A copy of the type arguments.</returns>
        public override Type[] GetGenericArguments() => Util.Copy(_args);

        /// <summary>
        /// Gets the custom modifiers on each type argument.
        /// </summary>
        /// <returns>A copy of the custom modifiers, one entry per type argument.</returns>
        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() =>
            _mods != null ? (CustomModifiers[])_mods.Clone() : new CustomModifiers[_args.Length];

        /// <inheritdoc />
        internal override Type GetGenericTypeArgument(int index) => _args[index];

        /// <summary>
        /// Gets a value indicating whether any of the type arguments contains generic parameters.
        /// </summary>
        public override bool ContainsGenericParameters
        {
            get
            {
                foreach (Type type in _args)
                {
                    if (type.ContainsGenericParameters)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets whether the generic type definition or any of the type arguments refers to a missing type.
        /// </summary>
        protected override bool ContainsMissingTypeImpl => _type.__ContainsMissingType || ContainsMissingType(_args);

        /// <summary>
        /// Gets the TypeSpec token of this instance in the module builder that defines the generic type definition,
        /// importing it on first use.
        /// </summary>
        /// <returns>The TypeSpec token.</returns>
        internal override int GetModuleBuilderToken()
        {
            if (_token == 0)
            {
                _token = ((ModuleBuilder)_type.Module).ImportType(this);
            }
            return _token;
        }

        /// <summary>
        /// Returns this type with any generic parameters in the type arguments replaced using the specified binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the type arguments.</param>
        /// <returns>The bound type, or this instance if no type argument changed.</returns>
        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            for (var i = 0; i < _args.Length; i++)
            {
                Type xarg = _args[i].BindTypeParameters(binder);
                if (!ReferenceEquals(xarg, _args[i]))
                {
                    Type[] xargs = new Type[_args.Length];
                    Array.Copy(_args, xargs, i);
                    xargs[i++] = xarg;
                    for (; i < _args.Length; i++)
                    {
                        xargs[i] = _args[i].BindTypeParameters(binder);
                    }
                    return Make(_type, xargs, _mods);
                }
            }
            return this;
        }

        /// <summary>
        /// Gets the current token of the generic type definition.
        /// </summary>
        /// <returns>The current token of the generic type definition.</returns>
        internal override int GetCurrentToken() => _type.GetCurrentToken();

        /// <summary>
        /// Gets whether the generic type definition is complete.
        /// </summary>
        internal override bool IsBaked => _type.IsBaked;

    }

}
