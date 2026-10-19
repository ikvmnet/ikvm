using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace IKVM.Runtime.Accessors
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Base class for accessors of class fields.
    /// </summary>
    internal abstract class FieldAccessor
    {

        readonly Type _type;
        readonly string _name;
        FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected FieldAccessor(Type type, string name)
        {
            this._type = type ?? throw new ArgumentNullException(nameof(type));
            this._name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the type which contains the field being accessed.
        /// </summary>
        protected Type Type => _type;

        /// <summary>
        /// Gets the name of the field being accessed.
        /// </summary>
        protected string Name => _name;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        protected FieldInfo Field => AccessorUtil.LazyGet(ref _field, () => _type.GetField(_name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) ?? throw new InvalidOperationException();

    }

    /// <summary>
    /// Provides fast access to a field of a given type.
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    internal sealed class FieldAccessor<TField> : FieldAccessor
    {

        /// <summary>
        /// Gets a <see cref="FieldAccessor"/> for the given field on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FieldAccessor<TField> LazyGet(ref FieldAccessor<TField> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new FieldAccessor<TField>(type, name));
        }

        Func<TField> _getter;
        Action<TField> _setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public FieldAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the field.
        /// </summary>
        Func<TField> Getter => AccessorUtil.LazyGet(ref _getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the field.
        /// </summary>
        Action<TField> Setter => AccessorUtil.LazyGet(ref _setter, MakeSetter);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Func<TField> MakeGetter()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorGet>__{Type.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(TField), Array.Empty<Type>());
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldsfld, Field);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Func<TField>)dm.CreateDelegate(typeof(Func<TField>));
#endif
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Action<TField> MakeSetter()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorSet>__{Type.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(void), new[] { typeof(TField) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stsfld, Field);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (Action<TField>)dm.CreateDelegate(typeof(Action<TField>));
#endif
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <returns></returns>
        public TField GetValue() => Getter();

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(TField value) => Setter(value);

    }

    /// <summary>
    /// Provides fast access to a field of a given type on the given object type.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TField"></typeparam>
    internal sealed class FieldAccessor<TObject, TField> : FieldAccessor
    {

        delegate TField GetterDelegate(TObject self);
        delegate void SetterDelegate(TObject self, TField value);
        delegate TField ExchangeDelegate(TObject self, TField value);
        delegate TField CompareExchangeDelegate(TObject self, TField value, TField comparand);

        static readonly MethodInfo ExchangeOfInt32 = typeof(Interlocked).GetMethod(nameof(Interlocked.Exchange), new[] { typeof(int).MakeByRefType(), typeof(int) });
        static readonly MethodInfo ExchangeOfInt64 = typeof(Interlocked).GetMethod(nameof(Interlocked.Exchange), new[] { typeof(long).MakeByRefType(), typeof(long) });
        static readonly MethodInfo ExchangeOfSingle = typeof(Interlocked).GetMethod(nameof(Interlocked.Exchange), new[] { typeof(float).MakeByRefType(), typeof(float) });
        static readonly MethodInfo ExchangeOfDouble = typeof(Interlocked).GetMethod(nameof(Interlocked.Exchange), new[] { typeof(double).MakeByRefType(), typeof(double) });
        static readonly MethodInfo ExchangeOfT = typeof(Interlocked).GetMethods().First(i => i.Name == nameof(Interlocked.Exchange) && i.GetGenericArguments().Length == 1);

        static readonly MethodInfo CompareExchangeOfInt32 = typeof(Interlocked).GetMethod(nameof(Interlocked.CompareExchange), new[] { typeof(int).MakeByRefType(), typeof(int), typeof(int) });
        static readonly MethodInfo CompareExchangeOfInt64 = typeof(Interlocked).GetMethod(nameof(Interlocked.CompareExchange), new[] { typeof(long).MakeByRefType(), typeof(long), typeof(long) });
        static readonly MethodInfo CompareExchangeOfSingle = typeof(Interlocked).GetMethod(nameof(Interlocked.CompareExchange), new[] { typeof(float).MakeByRefType(), typeof(float), typeof(float) });
        static readonly MethodInfo CompareExchangeOfDouble = typeof(Interlocked).GetMethod(nameof(Interlocked.CompareExchange), new[] { typeof(double).MakeByRefType(), typeof(double), typeof(double) });
        static readonly MethodInfo CompareExchangeOfT = typeof(Interlocked).GetMethods().First(i => i.Name == nameof(Interlocked.CompareExchange) && i.GetGenericArguments().Length == 1);

        /// <summary>
        /// Gets a <see cref="FieldAccessor"/> for the given field on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FieldAccessor<TObject, TField> LazyGet(ref FieldAccessor<TObject, TField> location, Type type, string name)
        {
            return AccessorUtil.LazyGet(ref location, () => new FieldAccessor<TObject, TField>(type, name));
        }

        GetterDelegate _getter;
        SetterDelegate _setter;
        ExchangeDelegate _exchange;
        CompareExchangeDelegate _compareExchange;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public FieldAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the field.
        /// </summary>
        GetterDelegate Getter => AccessorUtil.LazyGet(ref _getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the field.
        /// </summary>
        SetterDelegate Setter => AccessorUtil.LazyGet(ref _setter, MakeSetter);

        /// <summary>
        /// Gets the exchange operation for the field.
        /// </summary>
        ExchangeDelegate Exchange => AccessorUtil.LazyGet(ref _exchange, MakeExchange);

        /// <summary>
        /// Gets the compare and exchange operation for the field.
        /// </summary>
        CompareExchangeDelegate CompareExchange => AccessorUtil.LazyGet(ref _compareExchange, MakeCompareExchange);

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        GetterDelegate MakeGetter()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorGet>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(TField), new[] { typeof(TObject) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(OpCodes.Ldfld, Field);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (GetterDelegate)dm.CreateDelegate(typeof(GetterDelegate));
#endif
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        SetterDelegate MakeSetter()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorSet>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(void), new[] { typeof(TObject), typeof(TField) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, Field);
            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (SetterDelegate)dm.CreateDelegate(typeof(SetterDelegate));
#endif
        }

        /// <summary>
        /// Creates a new compare and exchange delegate.
        /// </summary>
        /// <returns></returns>
        ExchangeDelegate MakeExchange()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorExchange>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(TField), new[] { typeof(TObject), typeof(TField) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.EmitLdarg(0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(Field.IsStatic ? OpCodes.Ldsflda: OpCodes.Ldflda, Field);
            il.EmitLdarg(1);

            switch (typeof(TField))
            {
                case Type t when t == typeof(int):
                    il.Emit(OpCodes.Call, ExchangeOfInt32);
                    break;
                case Type t when t == typeof(long):
                    il.Emit(OpCodes.Call, ExchangeOfInt64);
                    break;
                case Type t when t == typeof(float):
                    il.Emit(OpCodes.Call, ExchangeOfSingle);
                    break;
                case Type t when t == typeof(double):
                    il.Emit(OpCodes.Call, ExchangeOfDouble);
                    break;
                case Type t when t.IsValueType == false:
                    il.Emit(OpCodes.Call, ExchangeOfT.MakeGenericMethod(Field.FieldType));
                    break;
                default:
                    throw new InternalException("No Interlocked.Exchange implementation for type.");
            }

            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (ExchangeDelegate)dm.CreateDelegate(typeof(ExchangeDelegate));
#endif
        }

        /// <summary>
        /// Creates a new compare and exchange delegate.
        /// </summary>
        /// <returns></returns>
        CompareExchangeDelegate MakeCompareExchange()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var dm = DynamicMethodUtil.Create($"__<FieldAccessorCompareExchange>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", Type, false, typeof(TField), new[] { typeof(TObject), typeof(TField), typeof(TField) });
            var il = JVM.Context.CodeEmitterFactory.Create(dm);

            il.EmitLdarg(0);
            il.Emit(OpCodes.Castclass, Type);
            il.Emit(Field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, Field);
            il.EmitLdarg(1);
            il.EmitLdarg(2);

            switch (typeof(TField))
            {
                case Type t when t == typeof(int):
                    il.Emit(OpCodes.Call, CompareExchangeOfInt32);
                    break;
                case Type t when t == typeof(long):
                    il.Emit(OpCodes.Call, CompareExchangeOfInt64);
                    break;
                case Type t when t == typeof(float):
                    il.Emit(OpCodes.Call, CompareExchangeOfSingle);
                    break;
                case Type t when t == typeof(double):
                    il.Emit(OpCodes.Call, CompareExchangeOfDouble);
                    break;
                case Type t when t.IsValueType == false:
                    il.Emit(OpCodes.Call, CompareExchangeOfT.MakeGenericMethod(Field.FieldType));
                    break;
                default:
                    throw new InternalException("No Interlocked.CompareExchange implementation for type.");
            }

            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (CompareExchangeDelegate)dm.CreateDelegate(typeof(CompareExchangeDelegate));
#endif
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public TField GetValue(TObject self) => Getter(self);

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetValue(TObject self, TField value) => Setter(self, value);

        /// <summary>
        /// Exchanges the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TField ExchangeValue(TObject self, TField value) => Exchange(self, value);
        
        /// <summary>
        /// Compares and exchanges the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <param name="comparand"></param>
        /// <returns></returns>
        public TField CompareExchangeValue(TObject self, TField value, TField comparand) => CompareExchange(self, value, comparand);

    }

#endif

}
