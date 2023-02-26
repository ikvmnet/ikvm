using System;
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

            // create new method
            var delegateReturnType = GetDelegateReturnType(typeof(TDelegate));
            var delegateParameterTypes = GetDelegateParameterTypes(typeof(TDelegate));
            var dm = DynamicMethodUtil.Create($"__<MethodAccessor>__{Type.Name.Replace(".", "_")}__{Method.Name}", Type.TypeAsBaseType, false, delegateReturnType, delegateParameterTypes);
            var il = CodeEmitter.Create(dm);

            // advance through each argument
            var n = 0;

            // load first argument, which is the instance if non-static
            if (Method.IsStatic == false && Method.IsConstructor == false)
            {
                il.EmitLdarg(n++);
                il.EmitCastclass(Type.TypeAsTBD);
            }

            // emit conversion code for the remainder of the arguments
            for (var i = 0; i < parameters.Length; i++)
            {
                il.EmitLdarg(n++);
                var parameterType = parameters[i];
                il.EmitCastclass(parameterType.TypeAsTBD);
            }

            if (Method.IsConstructor)
            {
                Method.EmitNewobj(il);

                // convert to delegate value
                if (delegateReturnType != null)
                    il.EmitCastclass(delegateReturnType);
                else
                    il.Emit(OpCodes.Pop);
            }
            else if (Method.IsStatic)
            {
                Method.EmitCall(il);

                // handle the return value if it exists
                if (Method.ReturnType != PrimitiveTypeWrapper.VOID)
                {
                    // convert to delegate value
                    if (delegateReturnType != null)
                        il.EmitCastclass(delegateReturnType);
                    else
                        il.Emit(OpCodes.Pop);
                }
            }
            else
            {
                Method.EmitCallvirt(il);

                // handle the return value if it exists
                if (Method.ReturnType != PrimitiveTypeWrapper.VOID)
                {
                    // convert to delegate value
                    if (delegateReturnType != null)
                        il.EmitCastclass(delegateReturnType);
                    else
                        il.Emit(OpCodes.Pop);
                }
            }

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
