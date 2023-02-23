using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal abstract partial class ConstructorAccessor
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ConstructorAccessor(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type which contains the method being accessed.
        /// </summary>
        public Type Type => type;

        /// <summary>
        /// Gets the constructor being accessed.
        /// </summary>
        public abstract ConstructorInfo Constructor { get; }

    }

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal sealed class ConstructorAccessor<TDelegate> : ConstructorAccessor
        where TDelegate : Delegate
    {

        ConstructorInfo ctor;
        TDelegate invoker;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructorAccessor(Type type) :
            base(type)
        {

        }

        /// <summary>
        /// Gets the constructor being accessed.
        /// </summary>
        public override ConstructorInfo Constructor => AccessorUtil.LazyGet(ref ctor, FindConstructor);

        /// <summary>
        /// Locates the method.
        /// </summary>
        /// <returns></returns>
        ConstructorInfo FindConstructor() => Type
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .Where(i => i.GetParameters().Length == typeof(TDelegate).GetMethod("Invoke").GetParameters().Length)
            .First();

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
            var objectType = GetDelegateReturnType(typeof(TDelegate));

            // create expressions for each parameter of the delegate
            var p = new List<ParameterExpression>();
            foreach (var i in parameterTypes)
                p.Add(Expression.Parameter(i));

            // create expressions to convert parameters of the delegate to the target constructor type
            var c = new List<Expression>();
            var l = Constructor.GetParameters();
            for (int i = 0; i < l.Length; i++)
                c.Add(Expression.Convert(p[i], l[i].ParameterType));

            // invocation of method with converted parameters
            var e = (Expression)Expression.New(Constructor, c);

            // convert return type
            e = Expression.Convert(e, objectType);

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
