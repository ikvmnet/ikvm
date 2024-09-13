using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Base class for managed symbols.
    /// </summary>
    abstract class ReflectionSymbol : ISymbol
    {

        readonly ReflectionSymbolContext _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ReflectionSymbol(ReflectionSymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the associated <see cref="ReflectionSymbolContext"/>.
        /// </summary>
        protected ReflectionSymbolContext Context => _context;

        /// <inheritdoc />
        public virtual bool IsMissing => false;

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        protected virtual internal ReflectionModuleSymbol ResolveModuleSymbol(Module module)
        {
            return _context.GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        protected internal ReflectionModuleSymbol[] ResolveModuleSymbols(Module[] modules)
        {
            var a = new ReflectionModuleSymbol[modules.Length];
            for (int i = 0; i < modules.Length; i++)
                a[i] = ResolveModuleSymbol(modules[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        protected internal IEnumerable<ReflectionModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules)
        {
            foreach (var module in modules)
                yield return ResolveModuleSymbol(module);
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        protected internal Module[] UnpackModuleSymbols(IModuleSymbol[] modules)
        {
            var a = new Module[modules.Length];
            for (int i = 0; i < modules.Length; i++)
                a[i] = ((ReflectionModuleSymbol)modules[i]).ReflectionModule;

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual ReflectionMemberSymbol ResolveMemberSymbol(MemberInfo member)
        {
            return member.MemberType switch
            {
                MemberTypes.Constructor => ResolveConstructorSymbol((ConstructorInfo)member),
                MemberTypes.Event => ResolveEventSymbol((EventInfo)member),
                MemberTypes.Field => ResolveFieldSymbol((FieldInfo)member),
                MemberTypes.Method => ResolveMethodSymbol((MethodInfo)member),
                MemberTypes.Property => ResolvePropertySymbol((PropertyInfo)member),
                MemberTypes.TypeInfo => ResolveTypeSymbol((Type)member),
                MemberTypes.NestedType => ResolveTypeSymbol((Type)member),
                MemberTypes.Custom => throw new NotImplementedException(),
                MemberTypes.All => throw new NotImplementedException(),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected internal ReflectionMemberSymbol[] ResolveMemberSymbols(MemberInfo[] types)
        {
            var a = new ReflectionMemberSymbol[types.Length];
            for (int i = 0; i < types.Length; i++)
                a[i] = ResolveMemberSymbol(types[i]);

            return a;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        protected internal MemberInfo[] UnpackMemberSymbols(IMemberSymbol[] members)
        {
            var a = new MemberInfo[members.Length];
            for (int i = 0; i < members.Length; i++)
                a[i] = ((ReflectionMemberSymbol)members[i]).ReflectionObject;

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual internal ReflectionTypeSymbol ResolveTypeSymbol(Type type)
        {
            return _context.GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected internal ReflectionTypeSymbol[] ResolveTypeSymbols(Type[] types)
        {
            var a = new ReflectionTypeSymbol[types.Length];
            for (int i = 0; i < types.Length; i++)
                a[i] = ResolveTypeSymbol(types[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected internal IEnumerable<ReflectionTypeSymbol> ResolveTypeSymbols(IEnumerable<Type> types)
        {
            foreach (var type in types)
                yield return ResolveTypeSymbol(type);
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected internal Type[] UnpackTypeSymbols(ITypeSymbol[] types)
        {
            var a = new Type[types.Length];
            for (int i = 0; i < types.Length; i++)
                a[i] = ((ReflectionTypeSymbol)types[i]).ReflectionObject;

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected virtual internal ReflectionMethodBaseSymbol ResolveMethodBaseSymbol(MethodBase method)
        {
            if (method.IsConstructor)
                return ResolveConstructorSymbol((ConstructorInfo)method);
            else
                return ResolveMethodSymbol((MethodInfo)method);
        }

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        protected virtual internal ReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
        {
            return _context.GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Resolves the symbols for the specified constructors.
        /// </summary>
        /// <param name="ctors"></param>
        /// <returns></returns>
        protected internal ReflectionConstructorSymbol[] ResolveConstructorSymbols(ConstructorInfo[] ctors)
        {
            var a = new ReflectionConstructorSymbol[ctors.Length];
            for (int i = 0; i < ctors.Length; i++)
                a[i] = ResolveConstructorSymbol(ctors[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected virtual internal ReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
        {
            return _context.GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Resolves the symbols for the specified methods.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        protected internal ReflectionMethodSymbol[] ResolveMethodSymbols(MethodInfo[] methods)
        {
            var a = new ReflectionMethodSymbol[methods.Length];
            for (int i = 0; i < methods.Length; i++)
                a[i] = ResolveMethodSymbol(methods[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected virtual internal ReflectionParameterSymbol ResolveParameterSymbol(ParameterInfo parameter)
        {
            return _context.GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Resolves the symbols for the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected internal ReflectionParameterSymbol[] ResolveParameterSymbols(ParameterInfo[] parameters)
        {
            var a = new ReflectionParameterSymbol[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                a[i] = ResolveParameterSymbol(parameters[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual internal ReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
        {
            return _context.GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Resolves the symbols for the specified fields.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected internal ReflectionFieldSymbol[] ResolveFieldSymbols(FieldInfo[] fields)
        {
            var a = new ReflectionFieldSymbol[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                a[i] = ResolveFieldSymbol(fields[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual internal ReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
        {
            return _context.GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Resolves the symbols for the specified properties.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected internal ReflectionPropertySymbol[] ResolvePropertySymbols(PropertyInfo[] properties)
        {
            var a = new ReflectionPropertySymbol[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                a[i] = ResolvePropertySymbol(properties[i]);

            return a;
        }

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected virtual internal ReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
        {
            return _context.GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Resolves the symbols for the specified events.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        protected internal ReflectionEventSymbol[] ResolveEventSymbols(EventInfo[] events)
        {
            var a = new ReflectionEventSymbol[events.Length];
            for (int i = 0; i < events.Length; i++)
                a[i] = ResolveEventSymbol(events[i]);

            return a;
        }

        /// <summary>
        /// Transforms a custom set of custom attribute data records to a symbol record.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected internal CustomAttributeSymbol[] ResolveCustomAttributes(IList<CustomAttributeData> attributes)
        {
            var a = new CustomAttributeSymbol[attributes.Count];
            for (int i = 0; i < attributes.Count; i++)
                a[i] = ResolveCustomAttribute(attributes[i]);

            return a;
        }

        /// <summary>
        /// Transforms a custom set of custom attribute data records to a symbol record.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected internal CustomAttributeSymbol[] ResolveCustomAttributes(IEnumerable<CustomAttributeData> attributes)
        {
            var a = new List<CustomAttributeSymbol>();
            foreach (var i in attributes)
                a.Add(ResolveCustomAttribute(i));

            return a.ToArray();
        }

        /// <summary>
        /// Transforms a custom attribute data record to a symbol record.
        /// </summary>
        /// <param name="customAttributeData"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected internal CustomAttributeSymbol ResolveCustomAttribute(CustomAttributeData customAttributeData)
        {
            return new CustomAttributeSymbol(
                ResolveTypeSymbol(customAttributeData.AttributeType),
                ResolveConstructorSymbol(customAttributeData.Constructor),
                ResolveCustomAttributeTypedArguments(customAttributeData.ConstructorArguments),
                ResolveCustomAttributeNamedArguments(customAttributeData.NamedArguments));
        }

        /// <summary>
        /// Transforms a list of <see cref="CustomAttributeTypedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeSymbolTypedArgument> ResolveCustomAttributeTypedArguments(IList<CustomAttributeTypedArgument> args)
        {
            var a = new CustomAttributeSymbolTypedArgument[args.Count];
            for (int i = 0; i < args.Count; i++)
                a[i] = ResolveCustomAttributeTypedArgument(args[i]);

            return a.ToImmutableArray();
        }

        /// <summary>
        /// Transforms a <see cref="CustomAttributeTypedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeSymbolTypedArgument ResolveCustomAttributeTypedArgument(CustomAttributeTypedArgument arg)
        {
            return new CustomAttributeSymbolTypedArgument(ResolveTypeSymbol(arg.ArgumentType), arg.Value);
        }

        /// <summary>
        /// Transforms a list of <see cref="CustomAttributeNamedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeSymbolNamedArgument> ResolveCustomAttributeNamedArguments(IList<CustomAttributeNamedArgument> args)
        {
            var a = new CustomAttributeSymbolNamedArgument[args.Count];
            for (int i = 0; i < args.Count; i++)
                a[i] = ResolveCustomAttributeNamedArgument(args[i]);

            return ImmutableArray.Create(a);
        }

        /// <summary>
        /// Transforms a <see cref="CustomAttributeNamedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeSymbolNamedArgument ResolveCustomAttributeNamedArgument(CustomAttributeNamedArgument arg)
        {
            return new CustomAttributeSymbolNamedArgument(arg.IsField, ResolveMemberSymbol(arg.MemberInfo), arg.MemberName, ResolveCustomAttributeTypedArgument(arg.TypedValue));
        }

    }

}