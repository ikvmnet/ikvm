using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.CoreLib.Reflection
{

    static class ReflectionExtensions
    {

        static readonly ParameterExpression _assemblyBuilderRuntimeAssemblyParameter = Expression.Parameter(typeof(AssemblyBuilder), "p");
        static readonly ParameterExpression _propertyBuilderParameter = Expression.Parameter(typeof(PropertyBuilder), "p");
        static readonly ParameterExpression _eventBuilderParameter = Expression.Parameter(typeof(EventBuilder), "p");
        static readonly ParameterExpression _parameterBuilderParameter = Expression.Parameter(typeof(ParameterBuilder), "p");

#if NET

#if NET8_0_OR_GREATER

        static readonly Type _assemblyBuilderType = typeof(AssemblyBuilder).Assembly.GetType("System.Reflection.Emit.RuntimeAssemblyBuilder", true)!;
        static readonly Type _propertyBuilderType = typeof(PropertyBuilder).Assembly.GetType("System.Reflection.Emit.RuntimePropertyBuilder", true)!;
        static readonly Type _eventBuilderType = typeof(EventBuilder).Assembly.GetType("System.Reflection.Emit.RuntimeEventBuilder", true)!;
        static readonly Type _parameterBuilderType = typeof(ParameterBuilder).Assembly.GetType("System.Reflection.Emit.RuntimeParameterBuilder", true)!;

#else

        static readonly Type _assemblyBuilderType = typeof(AssemblyBuilder).Assembly.GetType("System.Reflection.Emit.AssemblyBuilder", true)!;
        static readonly Type _propertyBuilderType = typeof(PropertyBuilder).Assembly.GetType("System.Reflection.Emit.PropertyBuilder", true)!;
        static readonly Type _eventBuilderType = typeof(EventBuilder).Assembly.GetType("System.Reflection.Emit.EventBuilder", true)!;
        static readonly Type _parameterBuilderType = typeof(ParameterBuilder).Assembly.GetType("System.Reflection.Emit.ParameterBuilder", true)!;

#endif

        static readonly Func<AssemblyBuilder, Assembly> _getAssemblyBuilderRuntimeAssemblyFunc = Expression.Lambda<Func<AssemblyBuilder, Assembly>>(
                Expression.Property(
                    Expression.ConvertChecked(_assemblyBuilderRuntimeAssemblyParameter, _assemblyBuilderType),
                    _assemblyBuilderType.GetProperty("InternalAssembly", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _assemblyBuilderRuntimeAssemblyParameter)
            .Compile();

        static readonly Func<PropertyBuilder, int> _getPropertyMetadataTokenFunc = Expression.Lambda<Func<PropertyBuilder, int>>(
                Expression.Field(
                    Expression.ConvertChecked(_propertyBuilderParameter, _propertyBuilderType),
                    _propertyBuilderType.GetField("m_tkProperty", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _propertyBuilderParameter)
            .Compile();

        static readonly Func<EventBuilder, int> _getEventMetadataTokenFunc = Expression.Lambda<Func<EventBuilder, int>>(
                Expression.Field(
                    Expression.ConvertChecked(_eventBuilderParameter, _eventBuilderType),
                    _eventBuilderType.GetField("m_evToken", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _eventBuilderParameter)
            .Compile();

        static readonly Func<ParameterBuilder, MethodBuilder> _getParameterMethodBuilderFunc = Expression.Lambda<Func<ParameterBuilder, MethodBuilder>>(
                Expression.Field(
                    Expression.ConvertChecked(_parameterBuilderParameter, _parameterBuilderType),
                    _parameterBuilderType.GetField("_methodBuilder", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _parameterBuilderParameter)
            .Compile();

        static readonly Func<ParameterBuilder, int> _getParameterMetadataTokenFunc = Expression.Lambda<Func<ParameterBuilder, int>>(
                Expression.Field(
                    Expression.ConvertChecked(_parameterBuilderParameter, _parameterBuilderType),
                    _parameterBuilderType.GetField("_token", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _parameterBuilderParameter)
            .Compile();

#else

        static readonly Type _assemblyBuilderType = typeof(AssemblyBuilder).Assembly.GetType("System.Reflection.Emit.AssemblyBuilder", true)!;
        static readonly Type _eventBuilderType = typeof(EventBuilder).Assembly.GetType("System.Reflection.Emit.EventBuilder", true)!;
        static readonly Type _parameterBuilderType = typeof(ParameterBuilder).Assembly.GetType("System.Reflection.Emit.ParameterBuilder", true)!;

        static readonly Func<AssemblyBuilder, Assembly> _getAssemblyBuilderRuntimeAssemblyFunc = Expression.Lambda<Func<AssemblyBuilder, Assembly>>(
                Expression.Call(
                    Expression.ConvertChecked(_assemblyBuilderRuntimeAssemblyParameter, _assemblyBuilderType),
                    _assemblyBuilderType.GetMethod("GetNativeHandle", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _assemblyBuilderRuntimeAssemblyParameter)
            .Compile();

        static readonly Func<ParameterBuilder, MethodBuilder> _getParameterMethodBuilderFunc = Expression.Lambda<Func<ParameterBuilder, MethodBuilder>>(
                Expression.Field(
                    Expression.ConvertChecked(_parameterBuilderParameter, _parameterBuilderType),
                    _parameterBuilderType.GetField("m_methodBuilder", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _parameterBuilderParameter)
            .Compile();

#endif

        static readonly Func<EventBuilder, ModuleBuilder> _getEventModuleBuilderFunc = Expression.Lambda<Func<EventBuilder, ModuleBuilder>>(
                Expression.Field(
                    Expression.ConvertChecked(_eventBuilderParameter, _eventBuilderType),
                    _eventBuilderType.GetField("m_module", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _eventBuilderParameter)
            .Compile();

        static readonly Func<EventBuilder, TypeBuilder> _getEventTypeBuilderFunc = Expression.Lambda<Func<EventBuilder, TypeBuilder>>(
                Expression.Field(
                    Expression.ConvertChecked(_eventBuilderParameter, _eventBuilderType),
                    _eventBuilderType.GetField("m_type", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _eventBuilderParameter)
            .Compile();

        static readonly Func<EventBuilder, string> _getEventNameFunc = Expression.Lambda<Func<EventBuilder, string>>(
                Expression.Field(
                    Expression.ConvertChecked(_eventBuilderParameter, _eventBuilderType),
                    _eventBuilderType.GetField("m_name", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                _eventBuilderParameter)
            .Compile();

        static readonly Func<EventBuilder, EventAttributes> _getEventAttributesFunc = Expression.Lambda<Func<EventBuilder, EventAttributes>>(
                Expression.ConvertChecked(
                    Expression.Field(
                        Expression.ConvertChecked(_eventBuilderParameter, _eventBuilderType),
                        _eventBuilderType.GetField("m_attributes", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException()),
                    typeof(EventAttributes)),
                _eventBuilderParameter)
            .Compile();

        /// <summary>
        /// Gets the metadata token for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this Module module)
        {
            var t = module.MetadataToken;
            if (t == 0)
                throw new InvalidOperationException();

            return t;
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this Module module)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.EntityHandle(module.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this MemberInfo member)
        {
            return member switch
            {
                Type t => t.GetMetadataTokenSafe(),
                MethodBase m => m.GetMetadataTokenSafe(),
                FieldInfo f => f.GetMetadataTokenSafe(),
                PropertyInfo p => p.GetMetadataTokenSafe(),
                EventInfo e => e.GetMetadataTokenSafe(),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this Type type)
        {
#if NETFRAMEWORK
            if (type is TypeBuilder b)
            {
                var t = b.TypeToken.Token;
                if (t == 0)
                    throw new InvalidOperationException();

                return t;
            }
#endif

            return type.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this Type type)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.TypeDefinitionHandle(type.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this FieldInfo field)
        {
#if NETFRAMEWORK
            if (field is FieldBuilder b)
            {
                var t = b.GetToken().Token;
                if (t == 0)
                    throw new InvalidOperationException();

                return t;
            }
#endif

            return field.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this FieldInfo field)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.FieldDefinitionHandle(field.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetMetadataTokenSafe(this MethodBase method)
        {
            return method switch
            {
                ConstructorInfo c => c.GetMetadataTokenSafe(),
                MethodInfo m => m.GetMetadataTokenSafe(),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetMetadataTokenRowNumberSafe(this MethodBase method)
        {
            return method switch
            {
                ConstructorInfo c => c.GetMetadataTokenRowNumberSafe(),
                MethodInfo m => m.GetMetadataTokenRowNumberSafe(),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this ConstructorInfo ctor)
        {
#if NETFRAMEWORK
            if (ctor is ConstructorBuilder b)
            {
                var t = b.GetToken().Token;
                if (t == 0)
                    throw new InvalidOperationException();

                return t;
            }
#endif

            return ctor.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this ConstructorInfo ctor)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(ctor.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static int GetMetadataTokenSafe(this MethodInfo method)
        {
#if NETFRAMEWORK
            if (method is MethodBuilder b)
            {
                var t = b.GetToken().Token;
                if (t == 0)
                    throw new InvalidOperationException();

                return t;
            }
#endif

            return method.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this MethodInfo ctor)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(ctor.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetMetadataTokenSafe(this PropertyInfo property)
        {
            if (property is PropertyBuilder b)
            {
#if NETFRAMEWORK
                var t = b.PropertyToken.Token;
#else
                var t = _getPropertyMetadataTokenFunc(b);
#endif
                if (t == 0)
                    throw new InvalidOperationException();

                return t;
            }

            return property.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this PropertyInfo property)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.PropertyDefinitionHandle(property.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetMetadataTokenSafe(this EventInfo @event)
        {
            return @event.GetMetadataToken();
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this EventInfo @event)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static int GetMetadataToken(this EventBuilder @event)
        {
#if NETFRAMEWORK
            return @event.GetEventToken().Token;
#else
            return _getEventMetadataTokenFunc(@event);
#endif
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static int GetMetadataTokenSafe(this EventBuilder @event)
        {
            var t = @event.GetMetadataToken();
            if (t == 0)
                throw new InvalidOperationException();

            return t;
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this EventBuilder @event)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetMetadataTokenSafe(this ParameterInfo parameter)
        {
            var t = parameter.MetadataToken;
            if (t == 0)
                throw new InvalidOperationException();

            return t;
        }

        /// <summary>
        /// Gets the metadata row number for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static int GetMetadataTokenRowNumberSafe(this ParameterInfo parameter)
        {
            return MetadataTokens.GetRowNumber(MetadataTokens.ParameterHandle(parameter.GetMetadataTokenSafe()));
        }

        /// <summary>
        /// Gets the metadata token for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static int GetMetadataToken(this ParameterBuilder parameter)
        {
#if NETFRAMEWORK
            return parameter.GetToken().Token;
#else
            return _getParameterMetadataTokenFunc(parameter);
#endif
        }

        /// <summary>
        /// Gets the <see cref="ModuleBuilder"/> associated with a <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Assembly GetRuntimeAssembly(this AssemblyBuilder assembly)
        {
            return _getAssemblyBuilderRuntimeAssemblyFunc(assembly);
        }

        /// <summary>
        /// Gets the <see cref="ModuleBuilder"/> associated with a <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ModuleBuilder GetModuleBuilder(this ParameterBuilder parameter)
        {
            return (ModuleBuilder)_getParameterMethodBuilderFunc(parameter).Module;
        }

        /// <summary>
        /// Gets the <see cref="TypeBuilder"/> associated with a <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static TypeBuilder GetTypeBuilder(this ParameterBuilder parameter)
        {
            return (TypeBuilder?)_getParameterMethodBuilderFunc(parameter).DeclaringType ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the <see cref="ModuleBuilder"/> associated with a <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static ModuleBuilder GetModuleBuilder(this FieldBuilder field)
        {
            return (ModuleBuilder)field.Module;
        }

        /// <summary>
        /// Gets the <see cref="TypeBuilder"/> associated with a <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static TypeBuilder GetTypeBuilder(this FieldBuilder field)
        {
            return (TypeBuilder?)field.DeclaringType ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the <see cref="ModuleBuilder"/> associated with a <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static ModuleBuilder GetModuleBuilder(this PropertyBuilder property)
        {
            return (ModuleBuilder)property.Module;
        }

        /// <summary>
        /// Gets the <see cref="TypeBuilder"/> associated with a <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static TypeBuilder GetTypeBuilder(this PropertyBuilder property)
        {
            return (TypeBuilder)property.DeclaringType!;
        }

        /// <summary>
        /// Gets the <see cref="ModuleBuilder"/> associated with a <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static ModuleBuilder GetModuleBuilder(this EventBuilder @event)
        {
            return _getEventModuleBuilderFunc(@event);
        }

        /// <summary>
        /// Gets the <see cref="TypeBuilder"/> associated with a <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static TypeBuilder GetTypeBuilder(this EventBuilder @event)
        {
            return _getEventTypeBuilderFunc(@event);
        }

        /// <summary>
        /// Gets the name associated with a <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static string GetEventName(this EventBuilder @event)
        {
            return _getEventNameFunc(@event);
        }

        /// <summary>
        /// Gets the attributes associated with a <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static EventAttributes GetEventAttributes(this EventBuilder @event)
        {
            return _getEventAttributesFunc(@event);
        }

        /// <summary>
        /// Gets the <see cref="MethodBuilder"/> associated with a <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static MethodBuilder GetMethodBuilder(this ParameterBuilder parameter)
        {
            return _getParameterMethodBuilderFunc(parameter);
        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Type"/> is a SZArray.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSZArray(this Type type)
        {
#if NET
            return type.IsSZArray;
#else
            return type.IsArray && type.Name.EndsWith("[]");
#endif
        }

        /// <summary>
        /// Implements GetcustomAttributeData with support for examining inheritence.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetCustomAttributesData(this Type type, bool inherit)
        {
            foreach (var i in type.GetCustomAttributesData())
                yield return i;

            if (inherit)
                for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
                    foreach (var cad in baseType.GetCustomAttributesData())
                        if (cad.AttributeType.GetCustomAttribute<AttributeUsageAttribute>()?.Inherited ?? false)
                            yield return cad;
        }

        /// <summary>
        /// Implements GetcustomAttributeData with support for examining inheritence.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetInheritedCustomAttributesData(this Type type)
        {
            for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
                foreach (var cad in baseType.GetCustomAttributesData())
                    if (cad.AttributeType.GetCustomAttribute<AttributeUsageAttribute>()?.Inherited ?? false)
                        yield return cad;
        }

        /// <summary>
        /// Implements GetcustomAttributeData with support for examining inheritence.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetCustomAttributesData(this MethodInfo method, bool inherit)
        {
            foreach (var i in method.GetCustomAttributesData())
                yield return i;

            if (inherit)
                for (var baseMethod = method.GetBaseDefinition(); baseMethod != null; baseMethod = baseMethod.GetBaseDefinition())
                    foreach (var cad in baseMethod.GetCustomAttributesData())
                        if (cad.AttributeType.GetCustomAttribute<AttributeUsageAttribute>()?.Inherited ?? false)
                            yield return cad;
        }

        /// <summary>
        /// Implements GetcustomAttributeData with support for examining inheritence.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetInheritedCustomAttributesData(this MethodInfo method)
        {
            for (var baseMethod = method.GetBaseDefinition(); baseMethod != null; baseMethod = baseMethod.GetBaseDefinition())
                foreach (var cad in baseMethod.GetCustomAttributesData())
                    if (cad.AttributeType.GetCustomAttribute<AttributeUsageAttribute>()?.Inherited ?? false)
                        yield return cad;
        }

        /// <summary>
        /// Implements GetcustomAttributeData with support for examining inheritence.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetCustomAttributesData(this MemberInfo member, bool inherit)
        {
            if (member is Type type)
                return GetCustomAttributesData(type, inherit);

            if (member is MethodInfo method)
                return GetCustomAttributesData(method, inherit);

            return member.GetCustomAttributesData();
        }

    }

}