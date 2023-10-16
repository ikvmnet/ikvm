using System;
using System.Linq;
using System.Xml.Linq;

using IKVM.Reflection;

namespace IKVM.Tools.Importer.MapXml
{

    /// <summary>
    /// Provides the ability to serialize MapXML.
    /// </summary>
    class MapXmlSerializer
    {

        public static XNamespace NS = "http://ikvm.net/schemas/mapxml";

        /// <summary>
        /// Reads a list of <see cref="MapModifiers"/> values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MapModifiers ReadMapModifiers(string value) => value != null ? value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Aggregate((MapModifiers)0, (i, j) => i | (Enum.TryParse<MapModifiers>(j, true, out var v) ? v : throw new MapXmlException($"Invalid modifiers value '{j}'."))) : 0;

        /// <summary>
        /// Reads a single <see cref="Scope"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Scope ReadScope(string value) => value != null ? (Enum.TryParse<Scope>(value, true, out var v) ? v : throw new MapXmlException($"Invalid scope value '{value}'.")) : 0;

        /// <summary>
        /// Reads a list of <see cref="MapModifiers"/> values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MethodAttributes ReadMethodAttributes(string value) => value != null ? value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Aggregate((MethodAttributes)0, (i, j) => i | (Enum.TryParse<MethodAttributes>(j, true, out var v) ? v : throw new MapXmlException($"Invalid attributes value '{j}'."))) : 0;

        /// <summary>
        /// Deserializes the given XML into a <see cref="Root"/> instance.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public Root Read(XDocument document)
        {
            return Read(document.Root);
        }

        /// <summary>
        /// Deserializes the given XML into a <see cref="Root"/> instance.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public Root Read(XElement element)
        {
            return Root.Read(element);
        }

    }

}
