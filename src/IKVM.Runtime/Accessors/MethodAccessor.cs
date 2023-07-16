using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal abstract partial class MethodAccessor
    {

        readonly Type type;
        readonly string name;
        readonly Type returnType;
        readonly Type[] parameterTypes;
        MethodBase method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MethodAccessor(Type type, string name, Type returnType, Type[] parameterTypes)
        {

            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.returnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
            this.parameterTypes = parameterTypes ?? throw new ArgumentNullException(nameof(parameterTypes));
        }

        /// <summary>
        /// Gets the type which contains the method being accessed.
        /// </summary>
        protected Type Type => type;

        /// <summary>
        /// Gets the name of the method being accessed.
        /// </summary>
        protected string Name => name;

        /// <summary>
        /// Gets the return type of the method being accessed.
        /// </summary>
        protected Type ReturnType => returnType;

        /// <summary>
        /// Gets the paremeter types of the method being accessed.
        /// </summary>
        protected Type[] ParameterTypes => parameterTypes;

        /// <summary>
        /// Gets the method being accessed.
        /// </summary>
        protected MethodBase Method => AccessorUtil.LazyGet(ref method, FindMethod) ?? throw new InternalException($"Could not locate method {Name}.");

        /// <summary>
        /// Finds the appropriate method.
        /// </summary>
        /// <returns></returns>
        MethodBase FindMethod() => type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .OfType<MethodBase>()
            .Where(i => i.Name == Name)
            .Where(i => i.IsConstructor && ReturnType == typeof(void) || i is MethodInfo m && m.ReturnType == ReturnType)
            .Where(i => i.GetParameters().Select(i => i.ParameterType).SequenceEqual(ParameterTypes))
            .FirstOrDefault();

    }

    /// <summary>
    /// Base class for accessors of class methods.
    /// </summary>
    internal sealed class MethodAccessor<TDelegate> : MethodAccessor
        where TDelegate : Delegate
    {

        public static MethodAccessor<TDelegate> LazyGet(ref MethodAccessor<TDelegate> location, Type type, string name, Type returnType, params Type[] parameterTypes)
        {
            return AccessorUtil.LazyGet(ref location, () => new MethodAccessor<TDelegate>(type, name, returnType, parameterTypes));
        }

        TDelegate invoker;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MethodAccessor(Type type, string name, Type returnType, Type[] parameters) :
            base(type, name, returnType, parameters)
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
            // validate return type
            var delegateReturnType = GetDelegateReturnType(typeof(TDelegate));
            if (Method.IsConstructor)
            {
                if (delegateReturnType == typeof(void))
                    throw new InternalException("Delegate has no return type for constructor.");
            }
            else if (Method is MethodInfo mi)
            {
                if (delegateReturnType == typeof(void) && mi.ReturnType != typeof(void))
                    throw new InternalException("Delegate has no return type for method with return type.");
                if (delegateReturnType != typeof(void) && mi.ReturnType == typeof(void))
                    throw new InternalException("Delegate has return type for method without return type.");
            }

            var parameters = Method.GetParameters();

            // validate parameter counts
            var delegateParameterTypes = GetDelegateParameterTypes(typeof(TDelegate));
            if ((Method.IsConstructor || Method.IsStatic) && delegateParameterTypes.Length != parameters.Length)
                throw new InternalException("Delegate has wrong number of parameters for constructor or static method.");
            else if (Method.IsConstructor == false && Method.IsStatic == false && delegateParameterTypes.Length != parameters.Length + 1)
                throw new InternalException("Delegate has wrong number of parameters for instance method.");

            // generate new dynamic method
            var dm = DynamicMethodUtil.Create($"__<MethodAccessor>__{Type.Name.Replace(".", "_")}__{Method.Name}", Type, false, delegateReturnType, delegateParameterTypes);
            var il = CodeEmitter.Create(dm);

            // advance through each argument
            var n = 0;

            // load first argument, which is the instance if non-static
            if (Method.IsStatic == false && Method.IsConstructor == false)
            {
                il.EmitLdarg(n++);
                if (delegateParameterTypes[0] != Type)
                    il.EmitCastclass(Type);
            }

            // emit conversion code for the remainder of the arguments
            for (var i = 0; i < parameters.Length; i++)
            {
                var delegateParameterType = delegateParameterTypes[n];
                il.EmitLdarg(n++);
                if (parameters[i].ParameterType != delegateParameterType)
                    il.EmitCastclass(parameters[i].ParameterType);
            }

            if (Method.IsConstructor)
                il.Emit(OpCodes.Newobj, Method);
            else if (Method.IsStatic || Method.IsVirtual == false)
                il.Emit(OpCodes.Call, Method);
            else
                il.Emit(OpCodes.Callvirt, Method);

            // convert to delegate value
            if (delegateReturnType != typeof(void))
                if (Method.IsConstructor && delegateReturnType != Type || Method.IsConstructor == false && delegateReturnType != ((MethodInfo)Method).ReturnType)
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
