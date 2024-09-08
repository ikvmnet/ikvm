using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IKVM.Tools.Runner.Diagnostics
{

    /// <summary>
    /// Describes an event emitted from a tool.
    /// </summary>
    public readonly record struct IkvmToolDiagnosticEvent(int Id, IkvmToolDiagnosticEventLevel Level, string Message, object?[] Args, IkvmToolDiagnosticEventLocation Location = default)
    {

        /// <summary>
        /// Reads a <see cref="IkvmToolDiagnosticEvent"/> from the 'json' formatter log output.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public static IkvmToolDiagnosticEvent ReadJson(ref Utf8JsonReader reader)
        {
            IkvmToolDiagnosticEventLevel? level = null;
            int? id = null;
            string? message = null;
            List<object?>? args = null;

            string? path = null;
            int startLine = 0;
            int startColumn = 0;
            int endLine = 0;
            int endColumn = 0;

            // advance to first actual JSON node
            while (reader.TokenType == JsonTokenType.None && reader.Read())
                continue;


            // first JSON node should be an object
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Could not locate JSON object.");

            // read until end of object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (id == null && reader.ValueTextEquals("id"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.Number)
                            throw new JsonException();

                        id = reader.GetInt32();
                        continue;
                    }

                    if (level == null && reader.ValueTextEquals("level"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        level = ParseLevel(ref reader);
                        continue;
                    }

                    if (message == null && reader.ValueTextEquals("message"))
                    {
                        reader.Skip();
                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        message = reader.GetString();
                        continue;
                    }

                    if (args == null && reader.ValueTextEquals("args"))
                    {
                        args ??= [];

                        if (reader.Read() == false || reader.TokenType != JsonTokenType.StartArray)
                            throw new JsonException("Expected array for 'args'.");

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                                break;

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

                    if (reader.ValueTextEquals("location"))
                    {
                        if (reader.Read() == false || reader.TokenType != JsonTokenType.StartObject)
                            throw new JsonException("Expected JSON object for 'location'.");

                        // read until end of object
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject)
                                break;

                            if (reader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (path == null && reader.ValueTextEquals("path"))
                                {
                                    reader.Skip();
                                    if (reader.TokenType != JsonTokenType.String)
                                        throw new JsonException();

                                    path = reader.GetString();
                                    continue;
                                }

                                if (reader.ValueTextEquals("position"))
                                {
                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.StartArray)
                                        throw new JsonException("Expected array for 'location/position' property.");

                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.Number || reader.TryGetInt32(out startLine) == false)
                                        throw new JsonException("Could not read start line.");
                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.Number || reader.TryGetInt32(out startColumn) == false)
                                        throw new JsonException("Could not read start column.");
                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.Number || reader.TryGetInt32(out endLine) == false)
                                        throw new JsonException("Could not read end line.");
                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.Number || reader.TryGetInt32(out endColumn) == false)
                                        throw new JsonException("Could not read end column.");

                                    if (reader.Read() == false || reader.TokenType != JsonTokenType.EndArray)
                                        throw new JsonException("Expected end of array.");

                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            if (id == null)
                throw new JsonException("Missing 'id' property.");
            if (level == null)
                throw new JsonException("Missing 'level' property.");
            if (message == null)
                throw new JsonException("Missing 'message' property.");

            return new IkvmToolDiagnosticEvent(id.Value, level.Value, message, args?.ToArray() ?? [], new IkvmToolDiagnosticEventLocation(path, startLine, startColumn, endLine, endColumn));
        }

        /// <summary>
        /// Parses the level into an enumeration.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        internal static IkvmToolDiagnosticEventLevel ParseLevel(ref Utf8JsonReader reader)
        {
            if (reader.ValueTextEquals("trace"))
                return IkvmToolDiagnosticEventLevel.Trace;
            if (reader.ValueTextEquals("info"))
                return IkvmToolDiagnosticEventLevel.Info;
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
        internal static object ParseNumber(ref Utf8JsonReader reader)
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
        /// Gets the ID of the event.
        /// </summary>
        public int Id { get; } = Id;

        /// <summary>
        /// Gets the level of the event.
        /// </summary>
        public IkvmToolDiagnosticEventLevel Level { get; } = Level;

        /// <summary>
        /// Message format string.
        /// </summary>
        public string Message { get; } = Message;

        /// <summary>
        /// Objects to include with format string.
        /// </summary>
        public object?[] Args { get; } = Args;

        /// <summary>
        /// Location of the event.
        /// </summary>
        public IkvmToolDiagnosticEventLocation Location { get; } = Location;

    }

}
