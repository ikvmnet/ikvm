using System;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

class MapGen
{
	static void Main(string[] args)
	{
		MapXml.Root map;
		using(Stream s = File.Open("\\ikvm\\ik.vm.net\\map.xml", FileMode.Open))
		{
			System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MapXml.Root));
			map = (MapXml.Root)ser.Deserialize(s);
		}
		Console.WriteLine("class MapXmlGenerator");
		Console.WriteLine("{");
		Console.WriteLine("internal static MapXml.Root Generate()");
		Console.WriteLine("{");
		Console.WriteLine("return {0};", DoInstance(map));
		Console.WriteLine("}");
		Console.WriteLine("}");
	}

	static int objcount;

	static string DoInstance(object o)
	{
		string name = "obj" + (objcount++);
		if(o == null)
		{
			return "null";
		}
		else if(o is Array)
		{
			Array a = (Array)o;
			Console.WriteLine("{0}[] {1} = new {0}[{2}];", o.GetType().GetElementType().FullName, name, a.Length);
			for(int i = 0; i < a.Length; i++)
			{
				Console.WriteLine("{0}[{1}] = {2};", name, i, DoInstance(a.GetValue(i)));
			}
		}
		else if(o is string)
		{
			return "\"" + o + "\"";
		}
		else if(o.GetType().IsPrimitive)
		{
			return o.ToString();
		}
		else if(o.GetType().IsEnum)
		{
			return "(" + o.GetType().FullName + ")" + (int)o;
		}
		else
		{
			Console.WriteLine("{0} {1} = new {0}();", o.GetType().FullName, name, o.GetType().FullName);
			FieldInfo[] fields = o.GetType().GetFields();
			for(int i = 0; i < fields.Length; i++)
			{
				object val = fields[i].GetValue(o);
				if(val != null)
				{
					string fieldName = fields[i].Name;
					if(fieldName == "override")
					{
						fieldName = "@" + fieldName;
					}
					Console.WriteLine("{0}.{1} = {2};", name, fieldName, DoInstance(val));
				}
			}
		}
		return name;
	}
}
