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

    sealed class GenericTypeInstance : TypeInfo
    {

        readonly Type _type;
        readonly Type[] _args;
        readonly CustomModifiers[] _mods;
        Type _baseType;
        int _token;

        internal static Type Make(Type type, Type[] typeArguments, CustomModifiers[] mods)
        {
            var identity = true;
            if (type is TypeBuilder || type is BakedType || type.__IsMissing)
            {
                // a TypeBuiler identity must be instantiated
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

        static bool IsEmpty(CustomModifiers[] mods, int i) =>
            // we need to be extra careful, because mods doesn't not need to be in canonical format
            // (Signature.ReadGenericInst() calls Make() directly, without copying the modifier arrays)
            mods == null || mods[i].IsEmpty;

        GenericTypeInstance(Type type, Type[] args, CustomModifiers[] mods)
        {
            _type = type;
            _args = args;
            _mods = mods;
        }

        public override bool Equals(object o)
        {
            GenericTypeInstance gt = o as GenericTypeInstance;
            return gt != null && gt._type.Equals(_type) && Util.ArrayEquals(gt._args, _args)
                && Util.ArrayEquals(gt._mods, _mods);
        }

        public override int GetHashCode() => _type.GetHashCode() * 3 ^ Util.GetHashCode(_args);

        public override string AssemblyQualifiedName
        {
            get
            {
                var fn = FullName;
                return fn == null ? null : fn + ", " + _type.Assembly.FullName;
            }
        }

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

        protected override bool IsValueTypeImpl => _type.IsValueType;

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

        public override Type DeclaringType => _type.DeclaringType;

        public override TypeAttributes Attributes => _type.Attributes;

        internal override void CheckBaked() => _type.CheckBaked();

        public override FieldInfo[] __GetDeclaredFields()
        {
            FieldInfo[] fields = _type.__GetDeclaredFields();
            for (var i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].BindTypeParameters(this);
            }
            return fields;
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            Type[] interfaces = _type.__GetDeclaredInterfaces();
            for (var i = 0; i < interfaces.Length; i++)
            {
                interfaces[i] = interfaces[i].BindTypeParameters(this);
            }
            return interfaces;
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            MethodBase[] methods = _type.__GetDeclaredMethods();
            for (var i = 0; i < methods.Length; i++)
            {
                methods[i] = methods[i].BindTypeParameters(this);
            }
            return methods;
        }

        public override Type[] __GetDeclaredTypes() => _type.__GetDeclaredTypes();

        public override EventInfo[] __GetDeclaredEvents()
        {
            EventInfo[] events = _type.__GetDeclaredEvents();
            for (var i = 0; i < events.Length; i++)
            {
                events[i] = events[i].BindTypeParameters(this);
            }
            return events;
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            PropertyInfo[] properties = _type.__GetDeclaredProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                properties[i] = properties[i].BindTypeParameters(this);
            }
            return properties;
        }

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

        public override string Namespace => _type.Namespace;

        public override string Name => _type.Name;

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

        public override Module Module => _type.Module;

        public override bool IsGenericType => true;

        public override bool IsConstructedGenericType => true;

        public override Type GetGenericTypeDefinition() => _type;

        public override Type[] GetGenericArguments() => Util.Copy(_args);

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() =>
            _mods != null ? (CustomModifiers[])_mods.Clone() : new CustomModifiers[_args.Length];

        internal override Type GetGenericTypeArgument(int index) => _args[index];

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

        protected override bool ContainsMissingTypeImpl => _type.__ContainsMissingType || ContainsMissingType(_args);

        internal override int GetModuleBuilderToken()
        {
            if (_token == 0)
            {
                _token = ((ModuleBuilder)_type.Module).ImportType(this);
            }
            return _token;
        }

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
                    return Make(_type, xargs, null);
                }
            }
            return this;
        }

        internal override int GetCurrentToken() => _type.GetCurrentToken();

        internal override bool IsBaked => _type.IsBaked;

    }

}
