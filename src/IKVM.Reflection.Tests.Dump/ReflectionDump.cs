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
    /// Selects what a rendering covers.
    /// </summary>
#if IKVM_REFLECTION
    enum DumpScope
#else
    public enum DumpScope
#endif
    {

        /// <summary>
        /// Everything the renderer covers.
        /// </summary>
        Full,

        /// <summary>
        /// Only what the IKVM.Reflection builders can emit, since IKVM never emits the rest: no events, parameter
        /// default values, custom attributes on generic parameters, interface constraints or function pointers.
        /// </summary>
        Emittable,

    }

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

        /// <summary>
        /// Binds every member declared by a type itself, whatever its visibility and whether it is static.
        /// </summary>
        const BindingFlags DeclaredOnly = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Binds the public members a type declares or inherits, including inherited static members.
        /// </summary>
        const BindingFlags PublicFlattened = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// The full names of the pseudo custom attributes, which are left out of the rendering; the metadata they stand for
        /// is rendered through the dedicated properties instead.
        /// </summary>
        static readonly HashSet<string> s_pseudoAttributes =
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
        /// <param name="assembly">The assembly to render.</param>
        /// <param name="scope">What the rendering covers.</param>
        /// <returns>The rendering, with <c>\n</c> line endings.</returns>
        public static string Dump(Assembly assembly, DumpScope scope = DumpScope.Full)
        {
            var w = new Writer(scope);
            w.Line("assembly " + assembly.GetName().Name + " " + assembly.GetName().Version);
            using (w.Indent())
            {
                CustomAttributes(w, () => assembly.GetCustomAttributesData());
                w.Line("module " + assembly.ManifestModule.Name);
                using (w.Indent())
                    CustomAttributes(w, () => assembly.ManifestModule.GetCustomAttributesData());
            }

            foreach (var type in assembly.GetTypes().OrderBy(TypeName, StringComparer.Ordinal))
                Guard(w, () => TypeDefinition(w, type));

            return w.ToString();
        }

        /// <summary>
        /// Renders a type defined in the dumped assembly: its attributes, hierarchy, generic parameters, custom attributes
        /// and declared members, and the members the public flattened lookup finds in this assembly.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="type">The type to render.</param>
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
                Guard(w, () => Field(w, i));

            foreach (var i in type.GetConstructors(DeclaredOnly).Select(i => (Key: MethodKey(i), Value: i)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Guard(w, () => Method(w, i.Value));

            foreach (var i in type.GetMethods(DeclaredOnly).Select(i => (Key: MethodKey(i), Value: i)).Where(i => w.Includes(i.Key)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Guard(w, () => Method(w, i.Value));

            foreach (var i in type.GetProperties(DeclaredOnly).Select(i => (Key: i.Name + "(" + string.Join(",", i.GetIndexParameters().Select(p => TypeName(p.ParameterType))) + ")", Value: i)).OrderBy(i => i.Key, StringComparer.Ordinal))
                Guard(w, () => Property(w, i.Value));

            if (w.Scope == DumpScope.Full)
                foreach (var i in type.GetEvents(DeclaredOnly).OrderBy(i => i.Name, StringComparer.Ordinal))
                    Guard(w, () => Event(w, i));

            // what the public flattened lookup sees, limited to members declared in this assembly
            foreach (var i in type.GetMembers(PublicFlattened).Where(i => i.DeclaringType != null && i.DeclaringType.Assembly == type.Assembly && (w.Scope == DumpScope.Full || i is not EventInfo)).Select(i => i.MemberType + " " + TypeName(i.DeclaringType) + "::" + MemberKey(i)).Where(w.Includes).OrderBy(i => i, StringComparer.Ordinal))
                w.Line("visible " + i);
        }

        /// <summary>
        /// Renders the generic parameters among the given generic arguments, with their constraints and custom attributes.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="parameters">The generic arguments of a type or method; those that are not generic parameters are skipped.</param>
        static void GenericParameters(Writer w, Type[] parameters)
        {
            foreach (var p in parameters)
            {
                if (p.IsGenericParameter == false)
                    continue;

                w.Line("genericparameter " + p.GenericParameterPosition + " " + p.Name + " " + Hex(p.GenericParameterAttributes));
                using var _ = w.Indent();
                foreach (var c in p.GetGenericParameterConstraints().Where(i => w.Scope == DumpScope.Full || i.IsInterface == false).Select(TypeName).OrderBy(i => i, StringComparer.Ordinal))
                    w.Line("constraint " + c);
                if (w.Scope == DumpScope.Full)
                    CustomAttributes(w, () => p.GetCustomAttributesData());
            }
        }

        /// <summary>
        /// Renders a field.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="field">The field to render.</param>
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

        /// <summary>
        /// Renders a method or constructor, with its return parameter, base definition, generic parameters and parameters.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="method">The method or constructor to render.</param>
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
                    Guard(w, () =>
                    {
                        var b = m.GetBaseDefinition();
                        w.Line("basedefinition " + TypeName(b.DeclaringType) + "::" + MethodKey(b));
                    });
                }

                GenericParameters(w, m.GetGenericArguments());
            }

            foreach (var p in method.GetParameters())
                Parameter(w, "parameter " + p.Position, p);

            CustomAttributes(w, () => method.GetCustomAttributesData());
        }

        /// <summary>
        /// Renders a parameter or return parameter.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="label">The label the line starts with, such as <c>return</c> or <c>parameter 0</c>.</param>
        /// <param name="parameter">The parameter to render.</param>
        static void Parameter(Writer w, string label, ParameterInfo parameter)
        {
            w.Line(label + (parameter.Position >= 0 ? " " + (parameter.Name ?? "<null>") : "") + " : " + TypeName(parameter.ParameterType));
            using var _ = w.Indent();
            w.Line("attributes " + Hex(w.Scope == DumpScope.Full ? parameter.Attributes : parameter.Attributes & ~ParameterAttributes.HasDefault));
            Modifiers(w, parameter.GetRequiredCustomModifiers(), parameter.GetOptionalCustomModifiers());
            // older runtimes report a default for parameters without a metadata row; IKVM does not rely on that
            if (w.Scope == DumpScope.Full && parameter.Position >= 0 && (parameter.MetadataToken & 0xFFFFFF) != 0 && parameter.HasDefaultValue)
                w.Line("default " + Value(parameter.RawDefaultValue));
            CustomAttributes(w, () => parameter.GetCustomAttributesData());
        }

        /// <summary>
        /// Renders a property, with its index parameters and accessors.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="property">The property to render.</param>
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

        /// <summary>
        /// Renders an event, with its accessors.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="event">The event to render.</param>
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

        /// <summary>
        /// Renders required and optional custom modifiers, in declaration order.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="required">The required custom modifiers.</param>
        /// <param name="optional">The optional custom modifiers.</param>
        static void Modifiers(Writer w, Type[] required, Type[] optional)
        {
            foreach (var m in required)
                w.Line("modreq " + TypeName(m));
            foreach (var m in optional)
                w.Line("modopt " + TypeName(m));
        }

        /// <summary>
        /// Renders custom attributes other than pseudo custom attributes, sorted, so the order the implementations return
        /// them in does not matter.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="getAttributes">Gets the custom attributes; an exception it throws is rendered as an error line.</param>
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

        /// <summary>
        /// Renders a custom attribute as one line, with its constructor arguments and its named arguments sorted by name.
        /// </summary>
        /// <param name="a">The custom attribute to render.</param>
        /// <returns>The line, or <see langword="null"/> for a pseudo custom attribute.</returns>
        static string Attribute(CustomAttributeData a)
        {
            var type = a.Constructor.DeclaringType;
            if (s_pseudoAttributes.Contains(type.FullName))
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
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="w">The writer to render to.</param>
        /// <param name="func">The function to run.</param>
        /// <returns>The result of <paramref name="func"/>, or <see langword="null"/> if it threw.</returns>
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

        /// <summary>
        /// Runs the given action, rendering an exception as an error line so one failure does not hide the rest. Only the
        /// exception type is rendered, since messages differ between implementations.
        /// </summary>
        /// <param name="w">The writer to render to.</param>
        /// <param name="action">The action to run.</param>
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

        /// <summary>
        /// Renders a custom attribute argument as its type followed by its value, with array elements rendered in turn.
        /// </summary>
        /// <param name="argument">The argument to render.</param>
        /// <returns>The rendered argument.</returns>
        static string TypedValue(CustomAttributeTypedArgument argument)
        {
            var type = TypeName(argument.ArgumentType);
            if (argument.Value is IEnumerable<CustomAttributeTypedArgument> array)
                return type + " [" + string.Join(", ", array.Select(TypedValue)) + "]";

            return type + " " + Value(argument.Value);
        }

        /// <summary>
        /// Renders a constant or custom attribute value culture-invariantly. Floating point values are rendered as their
        /// bits, so they compare exactly.
        /// </summary>
        /// <param name="value">The value to render.</param>
        /// <returns>The rendered value.</returns>
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

        /// <summary>
        /// Renders the name and signature of a member, which identifies it among the members of its type.
        /// </summary>
        /// <param name="member">The member to render.</param>
        /// <returns>The rendered member.</returns>
        static string MemberKey(MemberInfo member) => member switch
        {
            MethodBase m => MethodKey(m),
            PropertyInfo p => p.Name + " : " + TypeName(p.PropertyType),
            FieldInfo f => f.Name + " : " + TypeName(f.FieldType),
            EventInfo e => e.Name + " : " + TypeName(e.EventHandlerType),
            Type t => TypeName(t),
            _ => member.Name,
        };

        /// <summary>
        /// Renders the name, generic parameters, parameter types and return type of a method, which identifies it among the
        /// methods of its type and orders the rendering.
        /// </summary>
        /// <param name="method">The method or constructor to render.</param>
        /// <returns>The rendered signature.</returns>
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
        /// <param name="type">The type to render, or <see langword="null"/>.</param>
        /// <returns>The rendered name, or <c>&lt;null&gt;</c> if <paramref name="type"/> is <see langword="null"/>.</returns>
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

        /// <summary>
        /// Renders the raw value of an enum as hexadecimal, so flags that one implementation does not name still compare.
        /// </summary>
        /// <param name="value">The enum value to render.</param>
        /// <returns>The value as <c>0x</c> followed by hexadecimal digits.</returns>
        static string Hex(Enum value)
        {
            // mask to the width of the enum, since the IKVM.Reflection and System.Reflection enums are not all the same width
            var bits = System.Runtime.InteropServices.Marshal.SizeOf(Enum.GetUnderlyingType(value.GetType())) * 8;
            var raw = unchecked((ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture));
            return "0x" + (bits == 64 ? raw : raw & ((1UL << bits) - 1)).ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Renders a function pointer type, or returns <see langword="null"/> if the type is not one. Calling conventions
        /// beyond managed and unmanaged are not rendered, because runtime reflection only reports them on modified types.
        /// Runtimes before .NET 8 cannot report function pointer types, so there nothing is rendered.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <returns>The rendered function pointer type, or <see langword="null"/>.</returns>
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

        /// <summary>
        /// Renders a flag word, preceded by a space, when the flag is set.
        /// </summary>
        /// <param name="value">Whether the flag is set.</param>
        /// <param name="name">The word to render.</param>
        /// <returns>The word preceded by a space, or an empty string.</returns>
        static string Flag(bool value, string name) => value ? " " + name : "";

        /// <summary>
        /// Accumulates the rendering as lines indented by two spaces per level.
        /// </summary>
        /// <param name="scope">What the rendering covers.</param>
        sealed class Writer(DumpScope scope)
        {

            readonly StringBuilder _builder = new();
            int _depth;

            /// <summary>
            /// Gets what the rendering covers.
            /// </summary>
            public DumpScope Scope => scope;

            /// <summary>
            /// Gets whether a member with the given rendered signature is in scope.
            /// </summary>
            /// <param name="signature">The rendered signature of the member.</param>
            /// <returns><see langword="true"/> unless the scope is <see cref="DumpScope.Emittable"/> and the signature uses a function pointer.</returns>
            public bool Includes(string signature) => scope == DumpScope.Full || signature.Contains("fnptr") == false;

            /// <summary>
            /// Appends a line at the current indentation.
            /// </summary>
            /// <param name="text">The text of the line.</param>
            public void Line(string text) => _builder.Append(' ', _depth * 2).Append(text).Append('\n');

            /// <summary>
            /// Indents the lines that follow by one more level, until the returned object is disposed.
            /// </summary>
            /// <returns>An object that restores the previous indentation when disposed.</returns>
            public IDisposable Indent()
            {
                _depth++;
                return new Dedent(this);
            }

            /// <summary>
            /// Gets the rendering so far.
            /// </summary>
            /// <returns>The lines appended so far.</returns>
            public override string ToString() => _builder.ToString();

            /// <summary>
            /// Restores the indentation of a <see cref="Writer"/> when disposed.
            /// </summary>
            /// <param name="writer">The writer whose indentation to restore.</param>
            sealed class Dedent(Writer writer) : IDisposable
            {

                /// <summary>
                /// Reduces the indentation by one level.
                /// </summary>
                public void Dispose() => writer._depth--;

            }

        }

    }

}
