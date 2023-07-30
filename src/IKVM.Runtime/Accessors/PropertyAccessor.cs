using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.Runtime.Accessors
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Base class for accessors of class properties.
    /// </summary>
    internal abstract class PropertyAccessor
    {

        readonly Type type;
        readonly string name;
        PropertyInfo property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected PropertyAccessor(Type type, string name)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the type which contains the property being accessed.
        /// </summary>
        protected Type Type => type;

        /// <summary>
        /// Gets the name of the property being accessed.
        /// </summary>
        protected string Name => name;

        /// <summary>
        /// Gets the property being accessed.
        /// </summary>
        protected PropertyInfo Property => AccessorUtil.LazyGet(ref property, () => type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) ?? throw new InvalidOperationException();

    }

    /// <summary>
    /// Provides fast access to a property of a given type.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    internal sealed class PropertyAccessor<TProperty> : PropertyAccessor
    {

        /// <summary>
        /// Gets a <see cref="FieldAccessor"/> for the given property on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static PropertyAccessor<TProperty> LazyGet(ref PropertyAccessor<TProperty> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new PropertyAccessor<TProperty>(type, name));
        }

        Func<TProperty> getter;
        Action<TProperty> setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public PropertyAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the property.
        /// </summary>
        Func<TProperty> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the property.
        /// </summary>
        Action<TProperty> Setter => AccessorUtil.LazyGet(ref setter, MakeSetter);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Func<TProperty> MakeGetter()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<PropertyAccessorGet>__{Type.Name.Replace(".", "_")}__{Property.Name}", Type, false, typeof(TProperty), Array.Empty<Type>());
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(Property.GetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, Property.GetMethod);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Func<TProperty>)dm.CreateDelegate(typeof(Func<TProperty>));
#endif
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Action<TProperty> MakeSetter()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<PropertyAccessorSet>__{Type.Name.Replace(".", "_")}__{Property.Name}", Type, false, typeof(void), new[] { typeof(TProperty) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(Property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, Property.SetMethod);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Action<TProperty>)dm.CreateDelegate(typeof(Action<TProperty>));
#endif
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns></returns>
        public TProperty GetValue() => Getter();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(TProperty value) => Setter(value);

    }

    /// <summary>
    /// Provides fast access to a property of a given type on the given object type.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal sealed class PropertyAccessor<TObject, TProperty> : PropertyAccessor
    {

        /// <summary>
        /// Gets a <see cref="PropertyAccessor"/> for the given property on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyAccessor<TObject, TProperty> LazyGet(ref PropertyAccessor<TObject, TProperty> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new PropertyAccessor<TObject, TProperty>(type, name));
        }

        Func<TObject, TProperty> getter;
        Action<TObject, TProperty> setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public PropertyAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the property.
        /// </summary>
        Func<TObject, TProperty> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the property.
        /// </summary>
        Action<TObject, TProperty> Setter => AccessorUtil.LazyGet(ref setter, MakeSetter);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        Func<TObject, TProperty> MakeGetter()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            if (Property.CanRead == false)
                throw new InternalException($"Property {Property.Name} cannot be read.");

            var dm = DynamicMethodUtil.Create($"__<PropertyAccessorGet>__{Property.DeclaringType.Name.Replace(".", "_")}__{Property.Name}", Type, false, typeof(TProperty), new[] { typeof(TObject) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(Property.GetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, Property.GetMethod);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Func<TObject, TProperty>)dm.CreateDelegate(typeof(Func<TObject, TProperty>));
#endif
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        Action<TObject, TProperty> MakeSetter()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            if (Property.CanWrite == false)
                throw new InternalException($"Property {Property.Name} cannot be written.");

            var dm = DynamicMethodUtil.Create($"__<PropertyAccessorSet>__{Property.DeclaringType.Name.Replace(".", "_")}__{Property.Name}", Type, false, typeof(void), new[] { typeof(TObject), typeof(TProperty) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(Property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, Property.SetMethod);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Action<TObject, TProperty>)dm.CreateDelegate(typeof(Action<TObject, TProperty>));
#endif
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public TProperty GetValue(TObject self) => Getter(self);

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetValue(TObject self, TProperty value) => Setter(self, value);

    }

#endif

}
