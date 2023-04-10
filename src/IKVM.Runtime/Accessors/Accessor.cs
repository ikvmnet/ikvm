using System;

using IKVM.Internal;

namespace IKVM.Runtime.Accessors
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides a way to access classes at runtime.
    /// </summary>
    internal abstract partial class Accessor
    {

        readonly TypeWrapper type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Accessor(TypeWrapper type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type being accessed.
        /// </summary>
        protected TypeWrapper Type => type;

    }

    /// <summary>
    /// Provides a way to access classes at runtime.
    /// </summary>
    internal abstract partial class Accessor<TObject> : Accessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public Accessor(TypeWrapper type) :
             base(type)
        {

        }

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected FieldAccessor<TField> GetField<TField>(ref FieldAccessor<TField> accessor, string name) => FieldAccessor<TField>.LazyGet(ref accessor, Type, name);

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected FieldAccessor<TObject, TField> GetField<TField>(ref FieldAccessor<TObject, TField> accessor, string name) => FieldAccessor<TObject, TField>.LazyGet(ref accessor, Type, name);

        /// <summary>
        /// Initializes a property accessor.
        /// </summary>  
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected PropertyAccessor<TField> GetProperty<TField>(ref PropertyAccessor<TField> accessor, string name) => PropertyAccessor<TField>.LazyGet(ref accessor, Type, name);

        /// <summary>
        /// Initializes a property accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected PropertyAccessor<TObject, TField> GetProperty<TField>(ref PropertyAccessor<TObject, TField> accessor, string name) => PropertyAccessor<TObject, TField>.LazyGet(ref accessor, Type, name);

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        protected MethodAccessor<TDelegate> GetConstructor<TDelegate>(ref MethodAccessor<TDelegate> accessor, string signature) where TDelegate : Delegate => MethodAccessor<TDelegate>.LazyGet(ref accessor, Type, "<init>", signature);

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        protected MethodAccessor<TDelegate> GetMethod<TDelegate>(ref MethodAccessor<TDelegate> accessor, string name, string signature) where TDelegate : Delegate => MethodAccessor<TDelegate>.LazyGet(ref accessor, Type, name, signature);

    }

#endif

}