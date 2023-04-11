using System;
using System.Threading;

namespace IKVM.Runtime.Accessors
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides a way to access classes at runtime.
    /// </summary>
    internal abstract partial class Accessor
    {

        readonly string typeName;
        readonly AccessorTypeResolver resolver;
        Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="typeName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Accessor(AccessorTypeResolver resolver, string typeName)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.typeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        }

        /// <summary>
        /// Gets the type being accessed.
        /// </summary>
        protected Type Type => AccessorUtil.LazyGet(ref type, () => Resolve(typeName));

        /// <summary>
        /// Resolves the given type name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected Type Resolve(string typeName) => typeName != null ? resolver(typeName) : null;

        /// <summary>
        /// Resolves the given type name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected Type Resolve(ref Type type, string typeName) => AccessorUtil.LazyGet(ref type, () => Resolve(typeName));

    }

    /// <summary>
    /// Provides a way to access classes at runtime.
    /// </summary>
    internal abstract partial class Accessor<TObject> : Accessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="typeName"></param>
        public Accessor(AccessorTypeResolver resolver, string typeName) :
             base(resolver, typeName)
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
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected MethodAccessor<TDelegate> GetConstructor<TDelegate>(ref MethodAccessor<TDelegate> accessor, params Type[] parameterTypes) where TDelegate : Delegate => MethodAccessor<TDelegate>.LazyGet(ref accessor, Type, ".ctor", typeof(void), parameterTypes);

        /// <summary>
        /// Initializes a field accessor.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected MethodAccessor<TDelegate> GetMethod<TDelegate>(ref MethodAccessor<TDelegate> accessor, string name, Type returnType, params Type[] parameterTypes) where TDelegate : Delegate => MethodAccessor<TDelegate>.LazyGet(ref accessor, Type, name, returnType, parameterTypes);

    }

#endif

}