using System;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Defines a method accessor for a static method.
    /// </summary>
    internal abstract partial class ConstructorAccessor
    {

        public static ConstructorAccessor<Action> LazyGetVoid(ref ConstructorAccessor<Action> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Action>(type));
        }

        public static ConstructorAccessor<Func<TResult>> LazyGet<TResult>(ref ConstructorAccessor<Func<TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TResult>> LazyGet<TArg1, TResult>(ref ConstructorAccessor<Func<TArg1, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TResult>> LazyGet<TArg1, TArg2, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TResult>> LazyGet<TArg1, TArg2, TArg3, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>>(type));
        }

        public static ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(ref ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> location, Type type)
        {
            return AccessorUtil.LazyGet(ref location, () => new ConstructorAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>>(type));
        }

    }

}
