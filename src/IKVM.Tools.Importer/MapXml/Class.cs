using System.Linq;
using System.Xml.Linq;

namespace IKVM.Tools.Importer.MapXml
{

    public class Class : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Class"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Class Read(XElement element)
        {
            var clazz = new Class();
            Load(clazz, element);
            return clazz;
        }

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="element"></param>
        public static void Load(Class clazz, XElement element)
        {
            Load((MapXmlElement)clazz, element);
            clazz.Name = (string)element.Attribute("name");
            clazz.Shadows = (string)element.Attribute("shadows");
            clazz.Modifiers = MapXmlSerializer.ReadMapModifiers((string)element.Attribute("modifiers"));
            clazz.Scope = MapXmlSerializer.ReadScope((string)element.Attribute("scope"));
            clazz.Constructors = element.Elements(MapXmlSerializer.NS + "constructor").Select(Constructor.Read).ToArray();
            clazz.Methods = element.Elements(MapXmlSerializer.NS + "method").Select(Method.Read).ToArray();
            clazz.Fields = element.Elements(MapXmlSerializer.NS + "field").Select(Field.Read).ToArray();
            clazz.Properties = element.Elements(MapXmlSerializer.NS + "property").Select(Property.Read).ToArray();
            clazz.Interfaces = element.Elements(MapXmlSerializer.NS + "implements").Select(Interface.Read).ToArray();
            clazz.Clinit = element.Elements(MapXmlSerializer.NS + "clinit").Select(ClassInitializer.Read).FirstOrDefault();
            clazz.Attributes = element.Elements(MapXmlSerializer.NS + "attribute").Select(Attribute.Read).ToArray();
        }

        public string Name { get; set; }

        public string Shadows { get; set; }

        public MapModifiers Modifiers { get; set; }

        public Scope Scope { get; set; }

        public Constructor[] Constructors { get; set; }

        public Method[] Methods { get; set; }

        public Field[] Fields { get; set; }

        public Property[] Properties { get; set; }

        public Interface[] Interfaces { get; set; }

        public ClassInitializer Clinit { get; set; }

        public Attribute[] Attributes { get; set; }

    }

}
