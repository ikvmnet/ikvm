using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Formats the IL of a compiled method as text that does not depend on metadata tokens, so that it can be compared
    /// across runs.
    /// </summary>
    static class ILFormatter
    {

        static readonly Dictionary<short, OpCode> opcodes = typeof(OpCodes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(i => (OpCode)i.GetValue(null))
            .ToDictionary(i => i.Value);

        /// <summary>
        /// Formats the IL of the given method.
        /// </summary>
        /// <param name="method">The method to format.</param>
        /// <param name="thisType">References to this type are written as '$this'.</param>
        /// <param name="getLineNumber">Returns the source line for an IL offset, or a negative number.</param>
        public static string Format(MethodBase method, Type thisType, Func<int, int> getLineNumber)
        {
            var body = method.GetMethodBody() ?? throw new InvalidOperationException($"Method {method} has no body.");
            var il = body.GetILAsByteArray();
            var typeArgs = method.DeclaringType != null && method.DeclaringType.IsGenericType ? method.DeclaringType.GetGenericArguments() : null;
            var methodArgs = method.IsGenericMethod ? method.GetGenericArguments() : null;

            var sb = new StringBuilder();
            sb.Append(".method ").Append(FormatMethod(method, thisType)).Append('\n');
            sb.Append(".maxstack ").Append(body.MaxStackSize).Append('\n');
            sb.Append(".initlocals ").Append(body.InitLocals ? "true" : "false").Append('\n');

            foreach (var local in body.LocalVariables)
                sb.Append(".local [").Append(local.LocalIndex).Append("] ").Append(FormatType(local.LocalType, thisType)).Append(local.IsPinned ? " pinned" : "").Append('\n');

            foreach (var clause in body.ExceptionHandlingClauses)
            {
                sb.Append(".try IL_").Append(clause.TryOffset.ToString("x4")).Append(" to IL_").Append((clause.TryOffset + clause.TryLength).ToString("x4"));
                switch (clause.Flags)
                {
                    case ExceptionHandlingClauseOptions.Clause:
                        sb.Append(" catch ").Append(FormatType(clause.CatchType, thisType));
                        break;
                    case ExceptionHandlingClauseOptions.Filter:
                        sb.Append(" filter IL_").Append(clause.FilterOffset.ToString("x4"));
                        break;
                    case ExceptionHandlingClauseOptions.Finally:
                        sb.Append(" finally");
                        break;
                    case ExceptionHandlingClauseOptions.Fault:
                        sb.Append(" fault");
                        break;
                }

                sb.Append(" handler IL_").Append(clause.HandlerOffset.ToString("x4")).Append(" to IL_").Append((clause.HandlerOffset + clause.HandlerLength).ToString("x4")).Append('\n');
            }

            var line = -1;
            var pos = 0;
            while (pos < il.Length)
            {
                var offset = pos;

                var l = getLineNumber(offset);
                if (l >= 0 && l != line)
                {
                    line = l;
                    sb.Append("// line ").Append(line).Append('\n');
                }

                var b = il[pos++];
                var opcode = b == 0xFE ? opcodes[unchecked((short)(0xFE00 | il[pos++]))] : opcodes[b];
                sb.Append("IL_").Append(offset.ToString("x4")).Append(": ").Append(opcode.Name);

                switch (opcode.OperandType)
                {
                    case OperandType.InlineNone:
                        break;
                    case OperandType.ShortInlineBrTarget:
                        {
                            var rel = (sbyte)il[pos];
                            pos += 1;
                            sb.Append(" IL_").Append((pos + rel).ToString("x4"));
                            break;
                        }
                    case OperandType.InlineBrTarget:
                        {
                            var rel = BitConverter.ToInt32(il, pos);
                            pos += 4;
                            sb.Append(" IL_").Append((pos + rel).ToString("x4"));
                            break;
                        }
                    case OperandType.ShortInlineI:
                        sb.Append(' ').Append(opcode == OpCodes.Ldc_I4_S ? ((sbyte)il[pos]).ToString(CultureInfo.InvariantCulture) : il[pos].ToString(CultureInfo.InvariantCulture));
                        pos += 1;
                        break;
                    case OperandType.ShortInlineVar:
                        sb.Append(' ').Append(il[pos].ToString(CultureInfo.InvariantCulture));
                        pos += 1;
                        break;
                    case OperandType.InlineVar:
                        sb.Append(' ').Append(BitConverter.ToUInt16(il, pos).ToString(CultureInfo.InvariantCulture));
                        pos += 2;
                        break;
                    case OperandType.InlineI:
                        sb.Append(' ').Append(BitConverter.ToInt32(il, pos).ToString(CultureInfo.InvariantCulture));
                        pos += 4;
                        break;
                    case OperandType.InlineI8:
                        sb.Append(' ').Append(BitConverter.ToInt64(il, pos).ToString(CultureInfo.InvariantCulture));
                        pos += 8;
                        break;
                    case OperandType.ShortInlineR:
                        sb.Append(' ').Append(BitConverter.ToSingle(il, pos).ToString("R", CultureInfo.InvariantCulture));
                        pos += 4;
                        break;
                    case OperandType.InlineR:
                        sb.Append(' ').Append(BitConverter.ToDouble(il, pos).ToString("R", CultureInfo.InvariantCulture));
                        pos += 8;
                        break;
                    case OperandType.InlineSwitch:
                        {
                            var count = BitConverter.ToInt32(il, pos);
                            pos += 4;
                            var end = pos + count * 4;
                            sb.Append(" (");
                            for (int i = 0; i < count; i++)
                            {
                                if (i > 0)
                                    sb.Append(", ");

                                sb.Append("IL_").Append((end + BitConverter.ToInt32(il, pos + i * 4)).ToString("x4"));
                            }

                            sb.Append(')');
                            pos = end;
                            break;
                        }
                    case OperandType.InlineString:
                        sb.Append(" \"").Append(Escape(method.Module.ResolveString(BitConverter.ToInt32(il, pos)))).Append('"');
                        pos += 4;
                        break;
                    case OperandType.InlineSig:
                        sb.Append(" <signature>");
                        pos += 4;
                        break;
                    case OperandType.InlineMethod:
                    case OperandType.InlineField:
                    case OperandType.InlineType:
                    case OperandType.InlineTok:
                        sb.Append(' ').Append(FormatMember(method.Module.ResolveMember(BitConverter.ToInt32(il, pos), typeArgs, methodArgs), thisType));
                        pos += 4;
                        break;
                    default:
                        throw new NotSupportedException(opcode.OperandType.ToString());
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        static string FormatMember(MemberInfo member, Type thisType)
        {
            return member switch
            {
                Type t => FormatType(t, thisType),
                FieldInfo f => FormatType(f.FieldType, thisType) + " " + FormatType(f.DeclaringType, thisType) + "::" + f.Name,
                MethodBase m => FormatMethod(m, thisType),
                _ => member.ToString(),
            };
        }

        static string FormatMethod(MethodBase method, Type thisType)
        {
            var sb = new StringBuilder();
            if (method.IsStatic == false)
                sb.Append("instance ");

            sb.Append(method is MethodInfo mi ? FormatType(mi.ReturnType, thisType) : "void").Append(' ');
            sb.Append(FormatType(method.DeclaringType, thisType)).Append("::").Append(method.Name);

            if (method.IsGenericMethod)
                sb.Append('<').Append(string.Join(", ", method.GetGenericArguments().Select(i => FormatType(i, thisType)))).Append('>');

            sb.Append('(').Append(string.Join(", ", method.GetParameters().Select(i => FormatType(i.ParameterType, thisType)))).Append(')');
            return sb.ToString();
        }

        static string FormatType(Type type, Type thisType)
        {
            if (type == null)
                return "<null>";

            if (type == thisType)
                return "$this";

            if (type.IsByRef)
                return FormatType(type.GetElementType(), thisType) + "&";

            if (type.IsPointer)
                return FormatType(type.GetElementType(), thisType) + "*";

            if (type.IsArray)
                return FormatType(type.GetElementType(), thisType) + "[" + new string(',', type.GetArrayRank() - 1) + "]";

            if (type.IsGenericParameter)
                return (type.DeclaringMethod != null ? "!!" : "!") + type.Name;

            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
                return FormatType(type.GetGenericTypeDefinition(), thisType) + "<" + string.Join(", ", type.GetGenericArguments().Select(i => FormatType(i, thisType))) + ">";

            if (type.IsNested)
                return FormatType(type.DeclaringType, thisType) + "/" + type.Name;

            return type.FullName ?? type.Name;
        }

        static string Escape(string value)
        {
            var sb = new StringBuilder();
            foreach (var c in value)
            {
                if (c == '"' || c == '\\')
                    sb.Append('\\').Append(c);
                else if (c < 0x20 || c > 0x7e)
                    sb.Append("\\u").Append(((int)c).ToString("x4"));
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

    }

}
