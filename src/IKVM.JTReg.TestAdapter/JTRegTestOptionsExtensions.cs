using System.Xml.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    public static class JTRegTestOptionsExtensions
    {

        const int DEFAULT_PARTITION_COUNT = 8;

        public static JTRegTestOptions ToJTRegOptions(this IRunSettings self)
        {
            var x = XDocument.Parse(self.SettingsXml)?.Root?.Element("JTRegConfiguration");
            var o = new JTRegTestOptions();
            o.PartitionCount = (int?)x?.Element("PartitionCount") ?? DEFAULT_PARTITION_COUNT;
            return o;
        }

    }

}
