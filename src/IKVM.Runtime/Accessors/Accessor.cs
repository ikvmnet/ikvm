using System;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Provides a way to access classes at runtime.
    /// </summary>
    internal abstract partial class Accessor
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Accessor(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type being accessed.
        /// </summary>
        public Type Type => type;

        /// <summary>
        /// Initializes a static field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected StaticFieldAccessor<TField> GetStaticField<TField>(ref StaticFieldAccessor<TField> accessor, string name) => StaticFieldAccessor<TField>.LazyGet(ref accessor, type, name);

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected FieldAccessor<TField> GetField<TField>(ref FieldAccessor<TField> accessor, string name) => FieldAccessor<TField>.LazyGet(ref accessor, type, name);

        /// <summary>
        /// Initializes a static property accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected StaticPropertyAccessor<TProperty> GetStaticProperty<TProperty>(ref StaticPropertyAccessor<TProperty> accessor, string name) => StaticPropertyAccessor<TProperty>.LazyGet(ref accessor, type, name);

        /// <summary>
        /// Initializes a property accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected PropertyAccessor<TProperty> GetProperty<TProperty>(ref PropertyAccessor<TProperty> accessor, string name) => PropertyAccessor<TProperty>.LazyGet(ref accessor, type, name);

    }

}