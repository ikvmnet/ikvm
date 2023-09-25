using System.Xml.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    public static class JTRegTestOptionsExtensions
    {

        public static JTRegTestOptions ToJTRegOptions(this IRunSettings self)
        {
            var x = self?.SettingsXml != null ? XDocument.Parse(self.SettingsXml)?.Root?.Element("JTRegConfiguration") : null;
            var o = new JTRegTestOptions();
            o.PartitionCount = (int?)x?.Element("PartitionCount") ?? o.PartitionCount;
            o.TimeoutFactor = (float?)x?.Element("TimeoutFactor") ?? o.TimeoutFactor;
            return o;
        }

    }

}
