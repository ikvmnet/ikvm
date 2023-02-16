using System;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Runtime.Util.Java.Util.Concurrent;

#if FIRST_PASS == false

using java.nio.channels;
using java.security;

using sun.nio.ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class DotNetAsynchronousChannelExtensions
    {

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, object attachment, CompletionHandler handler, Func<TChannel, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg, object attachment, CompletionHandler handler, Func<TChannel, TArg1, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2, TArg3, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, TArg3 arg3, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, TArg3, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, arg3, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <typeparam name="TArg4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2, TArg3, TArg4, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, TArg3, TArg4, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, arg3, arg4, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <typeparam name="TArg4"></typeparam>
        /// <typeparam name="TArg5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, TArg3, TArg4, TArg5, AccessControlContext, CancellationToken, Task<TResult>> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, arg3, arg4, arg5, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { f.setResult(await a); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg arg, object attachment, CompletionHandler handler, Func<TChannel, TArg, AccessControlContext, CancellationToken, Task> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { await a; f.setResult(null); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, AccessControlContext, CancellationToken, Task> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { await a; f.setResult(null); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

        /// <summary>
        /// Executes the specified function on the channel group.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <typeparam name="TArg3"></typeparam>
        /// <typeparam name="TArg4"></typeparam>
        /// <typeparam name="TArg5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="channel"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PendingFuture Execute<TChannel, TArg1, TArg2, TArg3, TArg4, TArg5>(this DotNetAsynchronousChannelGroup self, TChannel channel, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object attachment, CompletionHandler handler, Func<TChannel, TArg1, TArg2, TArg3, TArg4, TArg5, AccessControlContext, CancellationToken, Task> func)
            where TChannel : AsynchronousChannel
        {
            var f = new PendingFuture(channel, handler, attachment, null);
            var c = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(self.executor().ToSynchronizationContext());
                var cts = new CancellationTokenSource();
                var t = func(channel, arg1, arg2, arg3, arg4, arg5, global::java.lang.System.getSecurityManager() != null ? AccessController.getContext() : null, cts.Token);
                f.setContext((t, cts));
                t.ContinueWith(async a => { try { await a; f.setResult(null); } catch (Exception e) { f.setFailure(e); } });
            }
            catch (Exception e)
            {
                if (handler != null)
                {
                    Invoker.invokeIndirectly(channel, handler, attachment, null, e);
                    return null;
                }

                f.setFailure(e);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(c);
            }

            return f;
        }

    }

}

#endif
