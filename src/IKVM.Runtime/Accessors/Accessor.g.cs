using System;

namespace IKVM.Runtime.Accessors
{

    internal abstract partial class Accessor
    {

        protected MethodAccessor<Action<object>> GetVoidMethod(ref MethodAccessor<Action<object>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1>> GetVoidMethod<TArg1>(ref MethodAccessor<Action<object, TArg1>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2>> GetVoidMethod<TArg1, TArg2>(ref MethodAccessor<Action<object, TArg1, TArg2>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3>> GetVoidMethod<TArg1, TArg2, TArg3>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4>> GetVoidMethod<TArg1, TArg2, TArg3, TArg4>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5>> GetVoidMethod<TArg1, TArg2, TArg3, TArg4, TArg5>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> GetVoidMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> GetVoidMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> GetVoidMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(ref MethodAccessor<Action<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> location, string name)
        {
            return MethodAccessor.LazyGetVoid(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TResult>> GetMethod<TResult>(ref MethodAccessor<Func<object, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TResult>> GetMethod<TArg1, TResult>(ref MethodAccessor<Func<object, TArg1, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TResult>> GetMethod<TArg1, TArg2, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TResult>> GetMethod<TArg1, TArg2, TArg3, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TResult>> GetMethod<TArg1, TArg2, TArg3, TArg4, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> GetMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> GetMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> GetMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

        protected MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> GetMethod<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(ref MethodAccessor<Func<object, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> location, string name)
        {
            return MethodAccessor.LazyGet(ref location, type, name);
        }

    }

}
