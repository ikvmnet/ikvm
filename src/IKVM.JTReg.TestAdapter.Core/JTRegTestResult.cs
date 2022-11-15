using System;
using System.Collections.Generic;

using com.sun.tools.javac.util;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegTestResult
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="testCase"></param>
        public JTRegTestResult(JTRegTestCase testCase)
        {
            TestCase = testCase;
        }

        public JTRegTestCase TestCase { get; set; }

        public string DisplayName { get; set; }

        public string ComputerName { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public JTRegTestOutcome Outcome { get; set; }

        public string ErrorMessage { get; set; }

        public List<JTRegTestResultMessage> Messages { get; set; } = new();

        public List<JTRegAttachment> Attachments { get; set; } = new();

    }

}
