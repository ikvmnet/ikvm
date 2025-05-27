using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;

namespace IKVM.CoreLib.Runtime
{

    /// <summary>
    /// Private type-safe implementation of DependentHandle. On .NET Core this serves as a wrapper for the built-in
    /// type. On Framework this uses reflection.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TDependent"></typeparam>
    struct DependentHandle<TTarget, TDependent> : IDisposable
        where TTarget : class?
        where TDependent : class?
    {

#if NETFRAMEWORK

        static readonly Type DependentHandleType =  typeof(object).Assembly.GetType("System.Runtime.CompilerServices.DependentHandle") ?? throw new Exception();
        static readonly ConstructorInfo DependentHandleCtor = DependentHandleType.GetConstructor([typeof(object), typeof(object)]) ?? throw new Exception();
        static readonly PropertyInfo IsAllocatedProperty = DependentHandleType.GetProperty("IsAllocated") ?? throw new Exception();
        static readonly MethodInfo GetPrimaryMethod = DependentHandleType.GetMethod("GetPrimary") ?? throw new Exception();
        static readonly MethodInfo GetPrimaryAndSecondaryMethod = DependentHandleType.GetMethod("GetPrimaryAndSecondary") ?? throw new Exception();
        static readonly MethodInfo FreeMethod = DependentHandleType.GetMethod("Free") ?? throw new Exception();

        static readonly ParameterExpression ThisExpr = Expression.Parameter(typeof(object));

        static readonly Func<object, bool> GetIsAllocatedFunc = Expression.Lambda<Func<object, bool>>(
                Expression.Property(
                    Expression.ConvertChecked(ThisExpr, DependentHandleType),
                    IsAllocatedProperty),
                ThisExpr)
            .Compile();

        static readonly Func<object, object?> GetPrimaryFunc = Expression.Lambda<Func<object, object?>>(
                Expression.Call(
                    Expression.ConvertChecked(ThisExpr, DependentHandleType),
                    GetPrimaryMethod),
                ThisExpr)
            .Compile();

        delegate void GetPrimaryAndSecondaryDelegate(object self, out object primary, out object secondary);

        static readonly ParameterExpression Ref1 = Expression.Parameter(typeof(object).MakeByRefType());
        static readonly ParameterExpression Ref2 = Expression.Parameter(typeof(object).MakeByRefType());

        static readonly GetPrimaryAndSecondaryDelegate GetPrimaryAndSecondaryFunc = Expression.Lambda<GetPrimaryAndSecondaryDelegate>(
                Expression.Call(
                    Expression.ConvertChecked(ThisExpr, DependentHandleType),
                    GetPrimaryAndSecondaryMethod,
                    Ref1,
                    Ref2),
                ThisExpr,
                Ref1,
                Ref2)
            .Compile();

        static readonly Action<object> FreeFunc = Expression.Lambda<Action<object>>(
                Expression.Call(
                    Expression.ConvertChecked(ThisExpr, DependentHandleType),
                    FreeMethod),
                ThisExpr)
            .Compile();

#endif

#if NET
        DependentHandle _hnd;
#else
        object _hnd;
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dependent"></param>
        public DependentHandle(TTarget? target, TDependent? dependent)
        {
#if NET
            _hnd = new DependentHandle(target, dependent);
#else
            _hnd = DependentHandleCtor.Invoke([target, dependent]);
#endif
        }

        /// <summary>
        /// Gets a value indicating whether this instance was constructed and has not yet been disposed.
        /// </summary>
        public readonly bool IsAllocated
        {
            get
            {
#if NET
                return _hnd.IsAllocated;
#else
                return GetIsAllocatedFunc(_hnd);
#endif
            }
        }

        /// <summary>
        /// Gets or sets the target object instance for the current handle. 
        /// </summary>
        public TTarget? Target
        {
            readonly get
            {
#if NET
                return (TTarget?)_hnd.Target;
#else
                return (TTarget?)GetPrimaryFunc(_hnd);
#endif
            }
            set
            {
#if NET
                _hnd.Target = value;
#else
                GetPrimaryAndSecondaryFunc(_hnd, out var primary, out var secondary);
                FreeFunc(_hnd);
                _hnd = DependentHandleCtor.Invoke([value, secondary]);
                GC.KeepAlive(primary);
                GC.KeepAlive(secondary);
#endif
            }
        }

        /// <summary>
        /// Gets or sets the dependent object instance for the current handle.


        /// </summary>
        public TDependent? Dependent
        {
            readonly get
            {
#if NET
                return (TDependent?)_hnd.Dependent;
#else
                GetPrimaryAndSecondaryFunc(_hnd, out var primary, out var secondary);
                return (TDependent?)secondary;
#endif
            }
            set
            {
#if NET
                _hnd.Dependent = value;
#else
                GetPrimaryAndSecondaryFunc(_hnd, out var primary, out var secondary);
                FreeFunc(_hnd);
                _hnd = DependentHandleCtor.Invoke([primary, value]);
                GC.KeepAlive(primary);
                GC.KeepAlive(secondary);
#endif
            }
        }

        /// <summary>
        /// Gets the values of both Target and Dependent (if available) as an atomic operation. 
        /// </summary>
        public readonly (TTarget? Target, TDependent? Dependent) TargetAndDependent
        {
            get
            {
#if NET
                var (Target, Dependent) = _hnd.TargetAndDependent;
                return ((TTarget?)Target, (TDependent?)Dependent);
#else
                GetPrimaryAndSecondaryFunc(_hnd, out var primary, out var secondary);
                return ((TTarget?)primary, (TDependent?)secondary);
#endif
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
#if NET
            _hnd.Dispose();
#else
            FreeFunc(_hnd);
#endif
        }

    }

}
