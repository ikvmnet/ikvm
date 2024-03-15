using System.Linq;
using System.Xml.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    public static class JTRegTestOptionsExtensions
    {

        const string JTRegConfigurationElementName = "JTRegConfiguration";
        const string PartitionCountElementName = "PartitionCount";
        const string TimeoutFactorElementName = "TimeoutFactor";
        const string ExcludeListFilesElementName = "ExcludeListFile";
        const string IncludeListFilesElementName = "IncludeListFile";

        /// <summary>
        /// Returns the <see cref="JTRegTestOptions"/> loaded from the specified <see cref="IRunSettings"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static JTRegTestOptions ToJTRegOptions(this IRunSettings self)
        {
            var x = self?.SettingsXml != null ? XDocument.Parse(self.SettingsXml)?.Root?.Element(JTRegConfigurationElementName) : null;
            var o = new JTRegTestOptions();
            if (x != null)
            {
                if ((int?)x.Element(PartitionCountElementName) is int partitionCount)
                    o.PartitionCount = partitionCount;

                if ((float?)x.Element(TimeoutFactorElementName) is float timeoutFactor)
                    o.TimeoutFactor = timeoutFactor;

                var excludeListFilesElements = x.Elements(ExcludeListFilesElementName);
                if (excludeListFilesElements != null && excludeListFilesElements.Any())
                {
                    o.ExcludeListFiles.Clear();
                    o.ExcludeListFiles.AddRange(excludeListFilesElements.Select(i => i.Value));
                }

                var includeListFilesElements = x.Elements(IncludeListFilesElementName);
                if (includeListFilesElements != null && includeListFilesElements.Any())
                {
                    o.IncludeListFiles.Clear();
                    o.IncludeListFiles.AddRange(includeListFilesElements.Select(i => i.Value));
                }
            }

            return o;
        }

    }

}
