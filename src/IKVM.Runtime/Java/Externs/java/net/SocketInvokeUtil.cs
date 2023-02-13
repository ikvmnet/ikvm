using System;
using System.Net.Sockets;

using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.java.net
{

    /// <summary>
    /// Provides some static methods for working with socket implementations.
    /// </summary>
    static class SocketImplUtil
    {

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="action"></param>
        public static void InvokeAction<TArg1>(object arg1, Action<TArg1> action)
        {
            InvokeAction(() => action((TArg1)arg1));
        }

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="action"></param>
        public static void InvokeAction<TArg1, TArg2>(object arg1, object arg2, Action<TArg1, TArg2> action)
        {
            InvokeAction(() => action((TArg1)arg1, (TArg2)arg2));
        }

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="action"></param>
        public static void InvokeAction<TArg1, TArg2, TArg3>(object arg1, object arg2, object arg3, Action<TArg1, TArg2, TArg3> action)
        {
            InvokeAction(() => action((TArg1)arg1, (TArg2)arg2, (TArg3)arg3));
        }

        /// <summary>
        /// Invokes the given action, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void InvokeAction(Action action)
        {
            try
            {
                action();
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
        }

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult InvokeFunc<TArg1, TResult>(object arg1, Func<TArg1, TResult> action)
        {
            return InvokeFunc(() => action((TArg1)arg1));
        }

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult InvokeFunc<TArg1, TArg2, TResult>(object arg1, object arg2, Func<TArg1, TArg2, TResult> action)
        {
            return InvokeFunc(() => action((TArg1)arg1, (TArg2)arg2));
        }

        /// <summary>
        /// Invokes the given delegate mapping exception to their appropriate Java type.
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult InvokeFunc<TArg1, TArg2, TArg3, TResult>(object arg1, object arg2, object arg3, Func<TArg1, TArg2, TArg3, TResult> action)
        {
            return InvokeFunc(() => action((TArg1)arg1, (TArg2)arg2, (TArg3)arg3));
        }

        /// <summary>
        /// Invokes the given action, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static TResult InvokeFunc<TResult>(Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
        }

#endif

    }

}
