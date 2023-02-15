using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class static properties.
    /// </summary>
    internal abstract class StaticPropertyAccessor
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
        protected StaticPropertyAccessor(Type type, string name)
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
        public PropertyInfo Property => AccessorUtil.LazyGet(ref property, () => type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));

    }

    /// <summary>
    /// Provides fast access to a field of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class StaticPropertyAccessor<T> : StaticPropertyAccessor
    {

        /// <summary>
        /// Gets a <see cref="StaticPropertyAccessor"/> for the given property on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static StaticPropertyAccessor<T> LazyGet(ref StaticPropertyAccessor<T> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticPropertyAccessor<T>(type, name));
        }

        Func<T> getter;
        Action<T> setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public StaticPropertyAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the property.
        /// </summary>
        Func<T> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the property.
        /// </summary>
        Action<T> Setter => AccessorUtil.LazyGet(ref setter, MakeSetter);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        Func<T> MakeGetter()
        {
            return Expression.Lambda<Func<T>>(Expression.Convert(Expression.Property(null, Property), Property.PropertyType)).Compile();
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        Action<T> MakeSetter()
        {
            var v = Expression.Parameter(typeof(T));
            return Expression.Lambda<Action<T>>(Expression.Assign(Expression.Property(null, Property), Expression.Convert(v, Property.PropertyType)), v).Compile();
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns></returns>
        public T GetValue() => Getter();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(T value) => Setter(value);

    }

}
