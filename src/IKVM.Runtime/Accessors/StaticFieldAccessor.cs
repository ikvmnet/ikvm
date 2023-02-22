using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class fields.
    /// </summary>
    internal abstract class StaticFieldAccessor
    {

        readonly Type type;
        readonly string name;
        FieldInfo field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected StaticFieldAccessor(Type type, string name)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the type which contains the field being accessed.
        /// </summary>
        public Type Type => type;

        /// <summary>
        /// Gets the name of the field being accessed.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        public FieldInfo Field => AccessorUtil.LazyGet(ref field, () => type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));

    }

    /// <summary>
    /// Provides fast access to a field of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class StaticFieldAccessor<T> : StaticFieldAccessor
    {

        /// <summary>
        /// Gets a <see cref="FieldAccessor"/> for the given field on the given type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static StaticFieldAccessor<T> LazyGet(ref StaticFieldAccessor<T> location, Type type, string fieldName)
        {
            return AccessorUtil.LazyGet(ref location, () => new StaticFieldAccessor<T>(type, fieldName));
        }

        Func<T> getter;
        Action<T> setter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public StaticFieldAccessor(Type type, string name) :
            base(type, name)
        {

        }

        /// <summary>
        /// Gets the getter for the field.
        /// </summary>
        Func<T> Getter => AccessorUtil.LazyGet(ref getter, MakeGetter);

        /// <summary>
        /// Gets the setter for the field.
        /// </summary>
        Action<T> Setter => AccessorUtil.LazyGet(ref setter, MakeSetter);

        /// <summary>
        /// Emits the appropriate ldind opcode for the given type.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="t"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static void EmitLdind(ILGenerator il, Type t)
        {
            if (t == typeof(bool))
                il.Emit(OpCodes.Ldind_U1);
            else if (t == typeof(byte))
                il.Emit(OpCodes.Ldind_U1);
            else if (t == typeof(char))
                il.Emit(OpCodes.Ldind_U2);
            else if (t == typeof(short))
                il.Emit(OpCodes.Ldind_I2);
            else if (t == typeof(int))
                il.Emit(OpCodes.Ldind_I4);
            else if (t == typeof(long))
                il.Emit(OpCodes.Ldind_I8);
            else if (t == typeof(float))
                il.Emit(OpCodes.Ldind_R4);
            else if (t == typeof(double))
                il.Emit(OpCodes.Ldind_R8);
            else
                il.Emit(OpCodes.Ldind_Ref);
        }

        /// <summary>
        /// Creates a new getter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Func<T> MakeGetter()
        {
            var dm = new DynamicMethod($"__<StaticFieldAccessorGet>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", typeof(T), Array.Empty<Type>(), Field.DeclaringType.Module, true);
            var il = dm.GetILGenerator();

            if (Field.IsInitOnly)
            {
                // we obtain a reference to the field and do an indirect load here to avoid JIT optimizations that inline static readonly fields
                il.Emit(OpCodes.Ldsflda, Field);
                EmitLdind(il, Field.FieldType);
            }
            else
            {
                il.Emit(OpCodes.Ldsfld, Field);
            }

            il.Emit(OpCodes.Ret);
            return (Func<T>)dm.CreateDelegate(typeof(Func<T>));
        }

        /// <summary>
        /// Creates a new setter.
        /// </summary>
        /// <returns></returns>
        Action<T> MakeSetter()
        {
            var dm = new DynamicMethod($"__<StaticFieldAccessorSet>__{Field.DeclaringType.Name.Replace(".", "_")}__{Field.Name}", typeof(void), new[] { typeof(T) }, Field.DeclaringType.Module, true);
            var il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stsfld, Field);

            il.Emit(OpCodes.Ret);
            return (Action<T>)dm.CreateDelegate(typeof(Action<T>));
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public T GetValue() => Getter();

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetValue(T value) => Setter(value);

    }

}
