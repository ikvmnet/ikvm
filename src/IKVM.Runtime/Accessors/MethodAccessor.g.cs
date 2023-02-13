using System;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Defines a method accessor for a method.
    /// </summary>
    internal abstract partial class MethodAccessor
    {

        public static MethodAccessor<Action<object>> LazyGetVoid(ref MethodAccessor<Action<object>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1>> LazyGetVoid<TArg1>(ref MethodAccessor<Action<object, TArg1>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2>> LazyGetVoid<TArg1, TArg2>(ref MethodAccessor<Action<object, TArg1, TArg2>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3>> LazyGetVoid<TArg1, TArg2, TArg3>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>>(type, name));
        }

        public static MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>>(type, name));
        }

        public static MethodAccessor<Func<object, TResult>> LazyGet<TResult>(ref MethodAccessor<Func<object, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TResult>> LazyGet<TArg1, TResult>(ref MethodAccessor<Func<object, TArg1, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TResult>> LazyGet<TArg1, TArg2, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TResult>> LazyGet<TArg1, TArg2, TArg3, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>>(type, name));
        }

        public static MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>>(type, name));
        }

    }

}
