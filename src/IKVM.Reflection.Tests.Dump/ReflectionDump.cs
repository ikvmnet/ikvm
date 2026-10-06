#if IKVM_REFLECTION
namespace IKVM.Reflection.Tests.Dump.Ikvm
#else
namespace IKVM.Reflection.Tests.Dump.Runtime
#endif
{

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

#if IKVM_REFLECTION
    using IKVM.Reflection;

    using Type = IKVM.Reflection.Type;
#else
    using System.Reflection;

    using Type = System.Type;
#endif

    /// <summary>
    /// Renders the reflection view of an assembly as deterministic text. This source is compiled twice: once against
    /// System.Reflection, to record what the real runtime reports, and once against IKVM.Reflection, so the two can be
    /// compared line for line.
    /// </summary>
    /// <remarks>
    /// Only members declared in the dumped assembly are rendered. Framework types are named but not expanded, because the
    /// runtime sees implementation assemblies where IKVM.Reflection sees reference assemblies. Type names are rendered
    /// without assembly identity for the same reason. Pseudo custom attributes are excluded; the metadata they stand for
    /// is rendered through the dedicated properties instead.
    /// </remarks>
#if IKVM_REFLECTION
    static class ReflectionDump
#else
    public static class ReflectionDump
#endif
    {

        const BindingFlags DeclaredOnly = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        const BindingFlags PublicFlattened = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        static readonly HashSet<string> PseudoAttributes =
        [
            "System.SerializableAttribute",
            "System.NonSerializedAttribute",
            "System.Runtime.InteropServices.ComImportAttribute",
            "System.Runtime.InteropServices.DllImportAttribute",
            "System.Runtime.InteropServices.FieldOffsetAttribute",
            "System.Runtime.InteropServices.InAttribute",
            "System.Runtime.InteropServices.MarshalAsAttribute",
            "System.Runtime.InteropServices.OptionalAttribute",
            "System.Runtime.InteropServices.OutAttribute",
            "System.Runtime.InteropServices.PreserveSigAttribute",
            "System.Runtime.InteropServices.StructLayoutAttribute",
            "System.Runtime.CompilerServices.MethodImplAttribute",
            "System.Runtime.CompilerServices.SpecialNameAttribute",
            "System.Security.SuppressUnmanagedCodeSecurityAttribute",
        ];

        /// <summary>
        /// Renders the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string Dump(Assembly assembly)
        {
            var w = new Writer();
            w.Line("assembly " + assembly.GetName().Name + " " + assembly.GetName().Version);
            using (w.Indent())
            {
                CustomAttributes(w, () => assembly.GetCustomAttributesData());
                w.Line("module " + assembly.ManifestModule.Name);
                using (w.Indent())
                    CustomAttributes(w, () => assembly.ManifestModule.GetCustomAttributesData());
            }

            foreach (var type in assembly.GetTypes().OrderBy(TypeName, StringComparer.Ordinal))
                TypeDefinition(w, type);

            return w.ToString();
        }

        static void TypeDefinition(Writer w, Type type)
        {
            w.Line("type " + TypeName(type));
            using var _ = w.Indent();

            w.Line("attributes " + Hex(type.Attributes));
            w.Line("kind" + Flag(type.IsClass, "class") + Flag(type.IsInterface, "interface") + Flag(type.IsValueType, "valuetype") + Flag(type.IsEnum, "enum") + Flag(type.IsAbstract, "abstract") + Flag(type.IsSealed, "sealed") + Flag(type.IsNested, "nested") + Flag(type.IsGenericTypeDefinition, "generic") + Flag(type.IsSerializable, "serializable"));
            w.Line("name " + type.Name + " namespace " + (type.Namespace ?? "<null>") + " fullname " + type.FullName);
            if (type.DeclaringType != null)
                w.Line("declaring " + TypeName(type.DeclaringType));
            if (type.BaseType != null)
                w.Line("base " + TypeName(type.BaseType));
            if (type.IsEnum)
                w.Line("underlying " + TypeName(type.GetEnumUnderlyingType()));

            foreach (var i in type.GetInterfaces().Select(TypeName).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("interface " + i);

            GenericParameters(w, type.GetGenericArguments());
            CustomAttributes(w, () => type.GetCustomAttributesData());

            foreach (var i in type.GetNestedTypes(DeclaredOnly).Select(TypeName).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("nested " + i);

            foreach (var i in type.GetFields(DeclaredOnly).OrderBy(i => i.Name, StringComparer.Ordinal))
                Field(w, i);

            foreach (var i in type.GetConstructors(DeclaredOnly).Select(i => (Key: MethodKey(i), Value: i)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Method(w, i.Value);

            foreach (var i in type.GetMethods(DeclaredOnly).Select(i => (Key: MethodKey(i), Value: i)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Method(w, i.Value);

            foreach (var i in type.GetProperties(DeclaredOnly).Select(i => (Key: i.Name + "(" + string.Join(",", i.GetIndexParameters().Select(p => TypeName(p.ParameterType))) + ")", Value: i)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Property(w, i.Value);

            foreach (var i in type.GetEvents(DeclaredOnly).OrderBy(i => i.Name, StringComparer.Ordinal))
                Event(w, i);

            // what the public flattened lookup sees, limited to members declared in this assembly
            foreach (var i in type.GetMembers(PublicFlattened).Where(i => i.DeclaringType != null && i.DeclaringType.Assembly == type.Assembly).Select(i => i.MemberType + " " + TypeName(i.DeclaringType) + "::" + MemberKey(i)).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("visible " + i);
        }

        static void GenericParameters(Writer w, Type[] parameters)
        {
            foreach (var p in parameters)
            {
                if (p.IsGenericParameter == false)
                    continue;

                w.Line("genericparameter " + p.GenericParameterPosition + " " + p.Name + " " + Hex(p.GenericParameterAttributes));
                using var _ = w.Indent();
                foreach (var c in p.GetGenericParameterConstraints().Select(TypeName).OrderBy(i => i, StringComparer.Ordinal))
                    w.Line("constraint " + c);
                CustomAttributes(w, () => p.GetCustomAttributesData());
            }
        }

        static void Field(Writer w, FieldInfo field)
        {
            w.Line("field " + field.Name + " : " + TypeName(field.FieldType));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(field.Attributes));
            Modifiers(w, field.GetRequiredCustomModifiers(), field.GetOptionalCustomModifiers());
            if (field.IsLiteral)
                w.Line("constant " + Value(field.GetRawConstantValue()));
            CustomAttributes(w, () => field.GetCustomAttributesData());
        }

        static void Method(Writer w, MethodBase method)
        {
            w.Line("method " + MethodKey(method));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(method.Attributes));
            w.Line("implementation " + Hex(method.GetMethodImplementationFlags()));
            w.Line("callingconvention " + Hex(method.CallingConvention));

            if (method is MethodInfo m)
            {
                Parameter(w, "return", m.ReturnParameter);
                if (m.IsVirtual)
                {
                    var b = m.GetBaseDefinition();
                    w.Line("basedefinition " + TypeName(b.DeclaringType) + "::" + MethodKey(b));
                }

                GenericParameters(w, m.GetGenericArguments());
            }

            foreach (var p in method.GetParameters())
                Parameter(w, "parameter " + p.Position, p);

            CustomAttributes(w, () => method.GetCustomAttributesData());
        }

        static void Parameter(Writer w, string label, ParameterInfo parameter)
        {
            w.Line(label + (parameter.Position >= 0 ? " " + (parameter.Name ?? "<null>") : "") + " : " + TypeName(parameter.ParameterType));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(parameter.Attributes));
            Modifiers(w, parameter.GetRequiredCustomModifiers(), parameter.GetOptionalCustomModifiers());
            // older runtimes report a default for parameters without a metadata row; IKVM does not rely on that
            if (parameter.Position >= 0 && (parameter.MetadataToken & 0xFFFFFF) != 0 && parameter.HasDefaultValue)
                w.Line("default " + Value(parameter.RawDefaultValue));
            CustomAttributes(w, () => parameter.GetCustomAttributesData());
        }

        static void Property(Writer w, PropertyInfo property)
        {
            w.Line("property " + property.Name + " : " + TypeName(property.PropertyType));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(property.Attributes));
            foreach (var p in property.GetIndexParameters())
                w.Line("index " + p.Position + " " + p.Name + " : " + TypeName(p.ParameterType));
            Modifiers(w, property.GetRequiredCustomModifiers(), property.GetOptionalCustomModifiers());
            if (property.GetGetMethod(true) is { } get)
                w.Line("get " + MethodKey(get));
            if (property.GetSetMethod(true) is { } set)
                w.Line("set " + MethodKey(set));
            foreach (var a in property.GetAccessors(true).Select(MethodKey).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("accessor " + a);
            CustomAttributes(w, () => property.GetCustomAttributesData());
        }

        static void Event(Writer w, EventInfo @event)
        {
            w.Line("event " + @event.Name + " : " + TypeName(@event.EventHandlerType));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(@event.Attributes));
            if (@event.GetAddMethod(true) is { } add)
                w.Line("add " + MethodKey(add));
            if (@event.GetRemoveMethod(true) is { } remove)
                w.Line("remove " + MethodKey(remove));
            if (@event.GetRaiseMethod(true) is { } raise)
                w.Line("raise " + MethodKey(raise));
            foreach (var o in @event.GetOtherMethods(true).Select(MethodKey).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("other " + o);
            CustomAttributes(w, () => @event.GetCustomAttributesData());
        }

        static void Modifiers(Writer w, Type[] required, Type[] optional)
        {
            foreach (var m in required)
                w.Line("modreq " + TypeName(m));
            foreach (var m in optional)
                w.Line("modopt " + TypeName(m));
        }

        static void CustomAttributes(Writer w, Func<IList<CustomAttributeData>> getAttributes)
        {
            var lines = new List<string>();
            if (Guard(w, getAttributes) is not { } attributes)
                return;

            foreach (var a in attributes)
            {
                if (Guard(w, () => Attribute(a)) is { } line)
                    lines.Add(line);
            }

            lines.Sort(StringComparer.Ordinal);
            foreach (var l in lines)
                w.Line(l);
        }

        static string Attribute(CustomAttributeData a)
        {
            var type = a.Constructor.DeclaringType;
            if (PseudoAttributes.Contains(type.FullName))
                return null;

            var b = new StringBuilder();
            b.Append("customattribute ").Append(TypeName(type)).Append('(');
            b.Append(string.Join(", ", a.ConstructorArguments.Select(TypedValue)));
            b.Append(')');
            foreach (var n in a.NamedArguments.OrderBy(n => n.MemberName, StringComparer.Ordinal))
                b.Append(n.IsField ? " field " : " property ").Append(n.MemberName).Append(" = ").Append(TypedValue(n.TypedValue));

            return b.ToString();
        }

        /// <summary>
        /// Runs the given function, rendering an exception as an error line so one failure does not hide the rest.
        /// Only the exception type is rendered, since messages differ between implementations.
        /// </summary>
        static T Guard<T>(Writer w, Func<T> func) where T : class
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                w.Line("error " + e.GetType().FullName);
                return null;
            }
        }

        static void Guard(Writer w, Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                w.Line("error " + e.GetType().FullName);
            }
        }

        static string TypedValue(CustomAttributeTypedArgument argument)
        {
            var type = TypeName(argument.ArgumentType);
            if (argument.Value is IEnumerable<CustomAttributeTypedArgument> array)
                return type + " [" + string.Join(", ", array.Select(TypedValue)) + "]";

            return type + " " + Value(argument.Value);
        }

        static string Value(object value) => value switch
        {
            null => "null",
            string s => "\"" + s + "\"",
            char c => "'" + c + "' (" + ((int)c).ToString(CultureInfo.InvariantCulture) + ")",
            bool b => b ? "true" : "false",
            float f => "float 0x" + BitConverter.ToInt32(BitConverter.GetBytes(f), 0).ToString("X8", CultureInfo.InvariantCulture),
            double d => "double 0x" + BitConverter.DoubleToInt64Bits(d).ToString("X16", CultureInfo.InvariantCulture),
            Type t => "typeof(" + TypeName(t) + ")",
            IFormattable f => f.GetType().Name + " " + f.ToString(null, CultureInfo.InvariantCulture),
            _ => value.GetType().Name + " " + value,
        };

        static string MemberKey(MemberInfo member) => member switch
        {
            MethodBase m => MethodKey(m),
            PropertyInfo p => p.Name + " : " + TypeName(p.PropertyType),
            FieldInfo f => f.Name + " : " + TypeName(f.FieldType),
            EventInfo e => e.Name + " : " + TypeName(e.EventHandlerType),
            Type t => TypeName(t),
            _ => member.Name,
        };

        static string MethodKey(MethodBase method)
        {
            var b = new StringBuilder();
            b.Append(method.Name);
            if (method.IsGenericMethodDefinition)
                b.Append('<').Append(string.Join(",", method.GetGenericArguments().Select(i => i.Name))).Append('>');
            b.Append('(');
            b.Append(string.Join(",", method.GetParameters().Select(i => TypeName(i.ParameterType))));
            if ((method.CallingConvention & CallingConventions.VarArgs) != 0)
                b.Append(method.GetParameters().Length > 0 ? ",..." : "...");
            b.Append(')');
            if (method is MethodInfo m)
                b.Append(" : ").Append(TypeName(m.ReturnType));
            return b.ToString();
        }

        /// <summary>
        /// Renders a type name without assembly identity.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string TypeName(Type type)
        {
            if (type == null)
                return "<null>";
            if (FunctionPointer(type) is { } fnptr)
                return fnptr;
            if (type.IsGenericParameter)
                return (type.DeclaringMethod != null ? "!!" : "!") + type.GenericParameterPosition + ":" + type.Name;
            if (type.IsByRef)
                return TypeName(type.GetElementType()) + "&";
            if (type.IsPointer)
                return TypeName(type.GetElementType()) + "*";
            if (type.IsArray)
                return TypeName(type.GetElementType()) + (type.GetArrayRank() == 1 && type.Name.EndsWith("[]", StringComparison.Ordinal) ? "[]" : "[" + new string(',', type.GetArrayRank() - 1) + "]");
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
                return TypeName(type.GetGenericTypeDefinition()) + "<" + string.Join(",", type.GetGenericArguments().Select(TypeName)) + ">";
            if (type.DeclaringType != null)
                return TypeName(type.DeclaringType) + "/" + type.Name;

            return type.Namespace != null ? type.Namespace + "." + type.Name : type.Name;
        }

        static string Hex(Enum value)
        {
            // mask to the width of the enum, since the IKVM.Reflection and System.Reflection enums are not all the same width
            var bits = System.Runtime.InteropServices.Marshal.SizeOf(Enum.GetUnderlyingType(value.GetType())) * 8;
            var raw = unchecked((ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture));
            return "0x" + (bits == 64 ? raw : raw & ((1UL << bits) - 1)).ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Renders a function pointer type, or returns <c>null</c> if the type is not one. Calling conventions beyond
        /// managed and unmanaged are not rendered, because runtime reflection only reports them on modified types.
        /// </summary>
        static string FunctionPointer(Type type)
        {
#if IKVM_REFLECTION
            if (type.IsFunctionPointer == false)
                return null;

            var sig = type.__MethodSignature;
            return (sig.IsUnmanaged ? "fnptr unmanaged(" : "fnptr(") + string.Join(",", sig.ParameterTypes.Select(TypeName)) + ") : " + TypeName(sig.ReturnType);
#elif NET8_0_OR_GREATER
            if (type.IsFunctionPointer == false)
                return null;

            return (type.IsUnmanagedFunctionPointer ? "fnptr unmanaged(" : "fnptr(") + string.Join(",", type.GetFunctionPointerParameterTypes().Select(TypeName)) + ") : " + TypeName(type.GetFunctionPointerReturnType());
#else
            return null;
#endif
        }

        static string Flag(bool value, string name) => value ? " " + name : "";

        sealed class Writer
        {

            readonly StringBuilder builder = new();
            int depth;

            public void Line(string text) => builder.Append(' ', depth * 2).Append(text).Append('\n');

            public IDisposable Indent()
            {
                depth++;
                return new Dedent(this);
            }

            public override string ToString() => builder.ToString();

            sealed class Dedent(Writer writer) : IDisposable
            {

                public void Dispose() => writer.depth--;

            }

        }

    }

}
