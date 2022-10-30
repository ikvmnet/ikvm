using System;
using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegTestCase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fullyQualifiedName"></param>
        /// <param name="uri"></param>
        /// <param name="source"></param>
        public JTRegTestCase(string fullyQualifiedName, Uri uri, string source)
        {
            FullyQualifiedName = fullyQualifiedName;
            Uri = uri;
            Source = source;
        }

        public string FullyQualifiedName { get; }

        public Uri Uri { get; }

        public string Source { get; }

        public string CodeFilePath { get; set; }

        public string TestSuiteRoot { get; set; }

        public string TestSuiteName { get; set; }

        public string TestPathName { get; set; }

        public string TestId { get; set; }

        public string TestTitle { get; set; }

        public string TestAuthor { get; set; }

        public string[] TestCategory { get; set; }

        public int TestPartition { get; set; }

        public Dictionary<string, string> Traits { get; set; } = new();

    }

}
