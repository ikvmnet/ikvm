using System.Collections.Generic;

using Buildalyzer;

namespace IKVM.MSBuild.Tests.Util
{

    public static class BuildalyzerExtensions
    {

        public static void SetGlobalProperties(this IProjectAnalyzer self, IDictionary<string, string> properties)
        {
            foreach (var kvp in properties)
                self.SetGlobalProperty(kvp.Key, kvp.Value);
        }

    }

}
