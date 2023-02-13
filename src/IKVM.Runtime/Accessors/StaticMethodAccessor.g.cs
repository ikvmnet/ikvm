using System;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Defines a method accessor for a static method.
    /// </summary>
    internal abstract partial class StaticMethodAccessor
    {

        public static StaticMethodAccessor<Action> LazyGetVoid(ref StaticMethodAccessor<Action> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1>> LazyGetVoid<TArg1>(ref StaticMethodAccessor<Action<TArg1>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2>> LazyGetVoid<TArg1, TArg2>(ref StaticMethodAccessor<Action<TArg1, TArg2>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3>> LazyGetVoid<TArg1, TArg2, TArg3>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>>(type, name));
        }

        public static StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> LazyGetVoid<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(ref StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>>(type, name));
        }

        public static StaticMethodAccessor<Func<TResult>> LazyGet<TResult>(ref StaticMethodAccessor<Func<TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TResult>> LazyGet<TArg1, TResult>(ref StaticMethodAccessor<Func<TArg1, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TResult>> LazyGet<TArg1, TArg2, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TResult>> LazyGet<TArg1, TArg2, TArg3, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>>(type, name));
        }

        public static StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> LazyGet<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(ref StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticMethodAccessor<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>>(type, name));
        }

    }

}
