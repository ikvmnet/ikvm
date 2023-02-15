using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal abstract partial class StaticMethodAccessor
    {

        readonly Type type;
        readonly string name;

        MethodInfo method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected StaticMethodAccessor(Type type, string name)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the type which contains the method being accessed.
        /// </summary>
        public Type Type => type;

        /// <summary>
        /// Gets the name of the method being accessed.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        public MethodInfo Method => AccessorUtil.LazyGet(ref method, () => type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));

    }

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal sealed class StaticMethodAccessor<TDelegate> : StaticMethodAccessor
        where TDelegate : Delegate
    {

        TDelegate invoker;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StaticMethodAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the setter for the field.
        /// </summary>
        public TDelegate Invoker => AccessorUtil.LazyGet(ref invoker, MakeInvoker);

        /// <summary>
        /// Creates a new invoker.
        /// </summary>
        /// <returns></returns>
        TDelegate MakeInvoker()
        {
            // find the types of the delegate
            var parameterTypes = GetDelegateParameterTypes(typeof(TDelegate));
            var returnType = GetDelegateReturnType(typeof(TDelegate));

            // create expressions for each parameter of the delegate
            var p = new List<ParameterExpression>();
            foreach (var i in parameterTypes)
                p.Add(Expression.Parameter(i));

            // create expressions to convert parameters of the delegate to the target method type
            var c = new List<Expression>();
            var l = Method.GetParameters();
            for (int i = 0; i < l.Length; i++)
                c.Add(Expression.Convert(p[i], l[i].ParameterType));

            // invocation of method with converted parameters
            var e = (Expression)Expression.Call(Method, c);

            // optionally convert return type
            if (returnType != null)
                e = Expression.Convert(e, returnType);

            // compile invoker
            return Expression.Lambda<TDelegate>(e, p).Compile();
        }

        /// <summary>
        /// Gets the parameter types for a delegate.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Type[] GetDelegateParameterTypes(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
                throw new ArgumentException("Not a delegate.", nameof(d));

            var invoke = d.GetMethod("Invoke");
            if (invoke == null)
                throw new ArgumentException("Not a delegate.", nameof(d));

            var parameters = invoke.GetParameters();
            var typeParameters = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                typeParameters[i] = parameters[i].ParameterType;

            return typeParameters;
        }

        /// <summary>
        /// Gets the return type for a delegate.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Type GetDelegateReturnType(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
                throw new ArgumentException("Not a delegate.", nameof(d));

            var invoke = d.GetMethod("Invoke");
            if (invoke == null)
                throw new ArgumentException("Not a delegate.", nameof(d));

            return invoke.ReturnType;
        }

    }

}
