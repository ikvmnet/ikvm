using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using IKVM.Runtime;

namespace IKVM.Tools.Importer.MapXml
{

    /// <summary>
    /// Base class for MapXML instruction instances.
    /// </summary>
    public abstract class Instruction : MapXmlElement
    {

        static readonly Dictionary<XName, MethodInfo> readMethodsByName;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Instruction()
        {
            readMethodsByName = typeof(Instruction).Assembly.GetTypes()
                .Where(i => i.IsSubclassOf(typeof(Instruction)))
                .Where(i => i.GetCustomAttribute<InstructionAttribute>() is InstructionAttribute ia)
                .Select(i => new { ElementName = MapXmlSerializer.NS + i.GetCustomAttribute<InstructionAttribute>().ElementName, Type = i })
                .Select(i => new { i.ElementName, Method = i.Type.GetMethod("Read", BindingFlags.Public | BindingFlags.Static) })
                .ToDictionary(i => i.ElementName, i => i.Method);
        }

        /// <summary>
        /// Reads the XML element into a new <see cref="InstructionList"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Instruction Read(XElement element)
        {
            if (readMethodsByName.TryGetValue(element.Name, out var m) == false)
                throw new MapXmlException($"Could not find instruction type for '{element.Name}'.");

            return (Instruction)m.Invoke(null, new[] { element });
        }

        /// <summary>
        /// Emits the instruction to the code generator.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ilgen"></param>
        internal abstract void Generate(CodeGenContext context, CodeEmitter ilgen);

    }

}
