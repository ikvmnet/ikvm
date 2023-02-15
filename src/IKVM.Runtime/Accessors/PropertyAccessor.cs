using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

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
        public Type Type => type;

        /// <summary>
        /// Gets the name of the property being accessed.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the property being accessed.
        /// </summary>
        public PropertyInfo Property => AccessorUtil.LazyGet(ref property, () => type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

    }

    /// <summary>
    /// Provides fast access to a field of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class PropertyAccessor<T> : PropertyAccessor
    {

        /// <summary>
        /// Gets a <see cref="PropertyAccessor"/> for the given property on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyAccessor<T> LazyGet(ref PropertyAccessor<T> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new PropertyAccessor<T>(type, name));
        }

        Func<object, T> getter;
        Action<object, T> setter;

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
        Func<object, T> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the property.
        /// </summary>
        Action<object, T> Setter => AccessorUtil.LazyGet(ref setter, MakeSetter);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Func<object, T> MakeGetter()
        {
            var p = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object, T>>(Expression.Convert(Expression.Property(Expression.Convert(p, Type), Property), Property.PropertyType), p).Compile();
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Action<object, T> MakeSetter()
        {
            var p = Expression.Parameter(typeof(object));
            var v = Expression.Parameter(typeof(T));
            return Expression.Lambda<Action<object, T>>(Expression.Assign(Expression.Property(Expression.Convert(p, Type), Property), Expression.Convert(v, Property.PropertyType)), p, v).Compile();
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public T GetValue(object self) => Getter(self);

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetValue(object self, T value) => Setter(self, value);

    }

}
