using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Base adapter class.
    /// </summary>
    public abstract class IkvmJTRegTestAdapter
    {

        internal const string URI = "executor://ikvmjtregtestadapter/v1";

        internal const string BASEDIR_PREFIX = "ikvm-jtreg-";
        internal const string TEST_ROOT_FILE_NAME = "TEST.ROOT";
        internal const string TEST_PROBLEM_LIST_FILE_NAME = "ProblemList.txt";
        internal const string TEST_EXCLUDE_LIST_FILE_NAME = "ExcludeList.txt";
        internal const string TEST_INCLUDE_LIST_FILE_NAME = "IncludeList.txt";
        internal const string DEFAULT_WORK_DIR_NAME = "work";
        internal const string DEFAULT_REPORT_DIR_NAME = "report";
        internal const string DEFAULT_PARAM_TAG = "regtest";
        internal const string ENV_PREFIX = "JTREG_";

        protected static readonly MD5 MD5 = MD5.Create();

        /// <summary>
        /// Creates a short hash of the given string to uniquely identify it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static string GetSourceHash(string source)
        {
            var b = MD5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var s = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
                s.Append(b[i].ToString("x2"));
            return s.ToString();
        }

        protected readonly Dictionary<string, TestProperty> properties = new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected IkvmJTRegTestAdapter()
        {
            properties.Add(IkvmJTRegTestProperties.TestSuiteRootProperty.Label, IkvmJTRegTestProperties.TestSuiteRootProperty);
            properties.Add(IkvmJTRegTestProperties.TestSuiteNameProperty.Label, IkvmJTRegTestProperties.TestSuiteNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestPathNameProperty.Label, IkvmJTRegTestProperties.TestPathNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestIdProperty.Label, IkvmJTRegTestProperties.TestIdProperty);
            properties.Add(IkvmJTRegTestProperties.TestNameProperty.Label, IkvmJTRegTestProperties.TestNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestTitleProperty.Label, IkvmJTRegTestProperties.TestTitleProperty);
            properties.Add(IkvmJTRegTestProperties.TestAuthorProperty.Label, IkvmJTRegTestProperties.TestAuthorProperty);
            properties.Add(IkvmJTRegTestProperties.TestPartitionProperty.Label, IkvmJTRegTestProperties.TestPartitionProperty);
            properties.Add(IkvmJTRegTestProperties.TestCategoryProperty.Label, IkvmJTRegTestProperties.TestCategoryProperty);
        }

        /// <summary>
        /// Creates a new TestManager.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="baseDir"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        protected dynamic CreateTestManager(IMessageLogger logger, string baseDir, java.io.PrintWriter output)
        {
            if (string.IsNullOrEmpty(baseDir))
                throw new ArgumentException($"'{nameof(baseDir)}' cannot be null or empty.", nameof(baseDir));
            if (output is null)
                throw new ArgumentNullException(nameof(output));

            var errorHandler = ErrorHandlerInterceptor.Create(new ErrorHandlerImplementation(logger));
            var testManager = JTRegTypes.TestManager.New(output, new java.io.File(Environment.CurrentDirectory), errorHandler);

            var workDirectory = Path.Combine(baseDir, DEFAULT_WORK_DIR_NAME);
            logger.SendMessage(TestMessageLevel.Informational, $"JTReg: Using work directory: '{workDirectory}'.");
            Directory.CreateDirectory(workDirectory);
            testManager.setWorkDirectory(new java.io.File(workDirectory));

            var reportDirectory = Path.Combine(baseDir, DEFAULT_REPORT_DIR_NAME);
            logger.SendMessage(TestMessageLevel.Informational, $"JTReg: Using report directory: '{reportDirectory}'.");
            Directory.CreateDirectory(reportDirectory);
            testManager.setReportDirectory(new java.io.File(reportDirectory));
            return testManager;
        }

    }

}
