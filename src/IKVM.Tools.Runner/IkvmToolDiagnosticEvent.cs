using System;
using System.Collections.Generic;
using System.Text.Json;

namespace IKVM.Tools.Runner
{

    /// <summary>
    /// Describes an event emitted from a tool.
    /// </summary>
    public class IkvmToolDiagnosticEvent
    {

        /// <summary>
        /// Reads a <see cref="IkvmToolDiagnosticEvent"/> from the 'json' formatter log output.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public static IkvmToolDiagnosticEvent ReadJson(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            IkvmToolDiagnosticEventLevel? level = null;
            int? id = null;
            string? name = null;
            string? message = null;
            List<object?>? args = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals("level"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        level = ParseLevel(ref reader);
                        continue;
                    }

                    if (reader.ValueTextEquals("id"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.Number)
                            throw new JsonException();

                        id = reader.GetInt32();
                        continue;
                    }

                    if (reader.ValueTextEquals("name"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        name = reader.GetString();
                        continue;
                    }

                    if (reader.ValueTextEquals("message"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        message = reader.GetString();
                        continue;
                    }

                    if (reader.ValueTextEquals("args"))
                    {
                        if (reader.Read() == false)
                            throw new JsonException();
                        if (reader.TokenType != JsonTokenType.StartArray)
                            throw new JsonException();

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                                break;

                            args ??= [];

                            switch (reader.TokenType)
                            {
                                case JsonTokenType.String:
                                    args.Add(reader.GetString());
                                    break;
                                case JsonTokenType.Number:
                                    args.Add(ParseNumber(ref reader));
                                    break;
                                case JsonTokenType.True:
                                    args.Add(true);
                                    break;
                                case JsonTokenType.False:
                                    args.Add(false);
                                    break;
                                case JsonTokenType.Null:
                                    args.Add(null);
                                    break;
                            }
                        }

                        continue;
                    }
                }
            }

            if (level == null || id == null || message == null)
                throw new JsonException();

            return new IkvmToolDiagnosticEvent(level.Value, id.Value, message, args?.ToArray() ?? []);
        }

        /// <summary>
        /// Parses the level into an enumeration.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        static IkvmToolDiagnosticEventLevel ParseLevel(ref Utf8JsonReader reader)
        {
            if (reader.ValueTextEquals("trace"))
                return IkvmToolDiagnosticEventLevel.Trace;
            if (reader.ValueTextEquals("information"))
                return IkvmToolDiagnosticEventLevel.Information;
            if (reader.ValueTextEquals("warning"))
                return IkvmToolDiagnosticEventLevel.Warning;
            if (reader.ValueTextEquals("error"))
                return IkvmToolDiagnosticEventLevel.Error;
            if (reader.ValueTextEquals("fatal"))
                return IkvmToolDiagnosticEventLevel.Fatal;

            throw new JsonException();
        }

        /// <summary>
        /// Attempts to read the number value.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static object ParseNumber(ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt16(out var _short))
                return _short;
            if (reader.TryGetInt32(out var _int))
                return _int;
            if (reader.TryGetInt64(out var _long))
                return _int;
            if (reader.TryGetSingle(out var _float))
                return _float;
            if (reader.TryGetDouble(out var _double))
                return _double;

            throw new JsonException();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public IkvmToolDiagnosticEvent(IkvmToolDiagnosticEventLevel level, int id, string message, object?[] args)
        {
            Level = level;
            Id = id;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }

        /// <summary>
        /// Gets the level of the event.
        /// </summary>
        public IkvmToolDiagnosticEventLevel Level { get; }

        /// <summary>
        /// Gets the ID of the event.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Message format string.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Objects to include with format string.
        /// </summary>
        public object?[] Args { get; }

    }

}
