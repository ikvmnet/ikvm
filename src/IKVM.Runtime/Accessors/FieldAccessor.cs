using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class fields.
    /// </summary>
    internal abstract class FieldAccessor
    {

        readonly Type type;
        readonly string name;
        FieldInfo field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected FieldAccessor(Type type, string name)
        {

            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type which contains the field being accessed.
        /// </summary>
        public Type Type => type;

        /// <summary>
        /// Gets the name of the field being accessed.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        public FieldInfo Field => AccessorUtil.LazyGet(ref field, () => type.GetField(name));

    }

    /// <summary>
    /// Provides fast access to a field of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class FieldAccessor<T> : FieldAccessor
    {

        /// <summary>
        /// Gets a <see cref="FieldAccessor"/> for the given field on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static FieldAccessor<T> LazyGet(ref FieldAccessor<T> location, Type type, string fieldName)
        {
            return AccessorUtil.LazyGet(ref location, () => new FieldAccessor<T>(type, fieldName));
        }

        Func<object, T> getter;
        Action<object, T> setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public FieldAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the field.
        /// </summary>
        Func<object, T> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the field.
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
            return Expression.Lambda<Func<object, T>>(Expression.Convert(Expression.Field(Expression.Convert(p, Type), Field), Field.FieldType), p).Compile();
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
            return Expression.Lambda<Action<object, T>>(Expression.Assign(Expression.Field(Expression.Convert(p, Type), Field), Expression.Convert(v, Field.FieldType)), p, v).Compile();
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public T GetValue(object self) => Getter(self);

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetValue(object self, T value) => Setter(self, value);

    }

}
