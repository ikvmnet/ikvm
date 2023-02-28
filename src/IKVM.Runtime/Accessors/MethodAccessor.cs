using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.Internal;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal abstract partial class MethodAccessor
    {

        readonly TypeWrapper type;
        readonly string name;
        readonly string signature;
        MethodWrapper method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MethodAccessor(TypeWrapper type, string name, string signature)
        {

            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        /// <summary>
        /// Gets the type which contains the method being accessed.
        /// </summary>
        protected TypeWrapper Type => type;

        /// <summary>
        /// Gets the name of the method being accessed.
        /// </summary>
        protected string Name => name;

        /// <summary>
        /// Gets the signature of the method being accessed.
        /// </summary>
        protected string Signature => signature;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        protected MethodWrapper Method => AccessorUtil.LazyGet(ref method, () => type.GetMethodWrapper(name, signature, false)) ?? throw new InvalidOperationException();

    }

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal sealed class MethodAccessor<TDelegate> : MethodAccessor
        where TDelegate : Delegate
    {

        public static MethodAccessor<TDelegate> LazyGet(ref MethodAccessor<TDelegate> location, TypeWrapper type, string name, string signature)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<TDelegate>(type, name, signature));
        }

        TDelegate invoker;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MethodAccessor(TypeWrapper type, string name, string signature) :
            base(type, name, signature)
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
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            Type.Finish();
            var parameters = Method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].EnsureLoadable(Type.GetClassLoader());
                parameters[i].Finish();
            }

            // resolve the runtime method info
            Method.ResolveMethod();

            // validate return type
            var delegateReturnType = GetDelegateReturnType(typeof(TDelegate));
            if (Method.IsConstructor)
            {
                if (delegateReturnType == typeof(void))
                    throw new InternalException("Delegate has no return type for constructor.");
            }
            else
            {
                if (delegateReturnType == typeof(void) && Method.ReturnType != PrimitiveTypeWrapper.VOID)
                    throw new InternalException("Delegate has no return type for method with return type.");
                if (delegateReturnType != typeof(void) && Method.ReturnType == PrimitiveTypeWrapper.VOID)
                    throw new InternalException("Delegate has return type for method without return type.");
            }

            // validate parameter counts
            var delegateParameterTypes = GetDelegateParameterTypes(typeof(TDelegate));
            if ((Method.IsConstructor || Method.IsStatic) && delegateParameterTypes.Length != parameters.Length)
                throw new InternalException("Delegate has wrong number of parameters for constructor or static method.");
            else if (Method.IsConstructor == false && Method.IsStatic == false && delegateParameterTypes.Length != parameters.Length + 1)
                throw new InternalException("Delegate has wrong number of parameters for instance method.");

            // generate new dynamic method
            var dm = DynamicMethodUtil.Create($"__<MethodAccessor>__{Type.Name.Replace(".", "_")}__{Method.Name}", Type.TypeAsTBD, false, delegateReturnType, delegateParameterTypes);
            var il = CodeEmitter.Create(dm);

            // advance through each argument
            var n = 0;

            // load first argument, which is the instance if non-static
            if (Method.IsStatic == false && Method.IsConstructor == false)
            {
                il.EmitLdarg(n++);
                if (delegateParameterTypes[0] != Type.TypeAsTBD)
                    il.EmitCastclass(Type.TypeAsTBD);
            }

            // emit conversion code for the remainder of the arguments
            for (var i = 0; i < parameters.Length; i++)
            {
                var delegateParameterType = delegateParameterTypes[n];
                il.EmitLdarg(n++);
                if (parameters[i].TypeAsTBD != delegateParameterType)
                    il.EmitCastclass(parameters[i].TypeAsTBD);
            }

            if (Method.IsConstructor)
                il.Emit(OpCodes.Newobj, Method.GetMethod());
            else if (Method.IsStatic || Method.IsVirtual == false)
                il.Emit(OpCodes.Call, Method.GetMethod());
            else
                il.Emit(OpCodes.Callvirt, Method.GetMethod());

            // convert to delegate value
            if (delegateReturnType != typeof(void))
                if (Method.IsConstructor && delegateReturnType != Type.TypeAsTBD || Method.IsConstructor == false && delegateReturnType != Method.ReturnType.TypeAsTBD)
                    il.EmitCastclass(delegateReturnType);

            il.Emit(OpCodes.Ret);
            il.DoEmit();

            return (TDelegate)dm.CreateDelegate(typeof(TDelegate));
#endif
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
