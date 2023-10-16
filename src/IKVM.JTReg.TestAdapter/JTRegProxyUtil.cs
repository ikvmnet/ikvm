using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    static class JTRegProxyUtil
    {

        public static IEnumerable<JTRegTestCase> Convert(IEnumerable<TestCase> tests) => tests.Select(Convert);

        public static JTRegTestCase Convert(TestCase testCase)
        {
            var t = new JTRegTestCase(testCase.FullyQualifiedName, testCase.ExecutorUri, testCase.Source);
            t.TestSuiteRoot = (string)testCase.GetPropertyValue(JTRegTestProperties.TestSuiteRootProperty);
            t.TestSuiteName = (string)testCase.GetPropertyValue(JTRegTestProperties.TestSuiteNameProperty);
            t.TestPathName = (string)testCase.GetPropertyValue(JTRegTestProperties.TestPathNameProperty);
            t.TestId = (string)testCase.GetPropertyValue(JTRegTestProperties.TestIdProperty);
            t.TestTitle = (string)testCase.GetPropertyValue(JTRegTestProperties.TestTitleProperty);
            t.TestAuthor = (string)testCase.GetPropertyValue(JTRegTestProperties.TestAuthorProperty);
            t.TestCategory = (string[])testCase.GetPropertyValue(JTRegTestProperties.TestCategoryProperty);
            t.TestPartition = testCase.GetPropertyValue(JTRegTestProperties.TestPartitionProperty, 0);

            foreach (var trait in testCase.Traits)
                t.Traits.Add(trait.Name, trait.Value);

            return t;
        }

        /// <summary>
        /// Converts a <see cref="JTRegTestCase"/> to a <see cref="TestCase"/>.
        /// </summary>
        /// <param name="testCase"></param>
        /// <returns></returns>
        public static TestCase Convert(JTRegTestCase testCase)
        {
            var t = new TestCase(testCase.FullyQualifiedName, new Uri(JTRegTestManager.URI), testCase.Source) { CodeFilePath = testCase.CodeFilePath };
            t.SetPropertyValue(JTRegTestProperties.TestSuiteRootProperty, testCase.TestSuiteRoot);
            t.SetPropertyValue(JTRegTestProperties.TestSuiteNameProperty, testCase.TestSuiteName);
            t.SetPropertyValue(JTRegTestProperties.TestPathNameProperty, testCase.TestPathName);
            t.SetPropertyValue(JTRegTestProperties.TestIdProperty, testCase.TestId);
            t.SetPropertyValue(JTRegTestProperties.TestTitleProperty, testCase.TestTitle);
            t.SetPropertyValue(JTRegTestProperties.TestAuthorProperty, testCase.TestAuthor);
            t.SetPropertyValue(JTRegTestProperties.TestCategoryProperty, testCase.TestCategory);
            t.SetPropertyValue(JTRegTestProperties.TestPartitionProperty, testCase.TestPartition);

            foreach (var kvp in testCase.Traits)
                t.Traits.Add(kvp.Key, kvp.Value);

            return t;
        }

        public static TestMessageLevel Convert(JTRegTestMessageLevel messageLevel) => messageLevel switch
        {
            JTRegTestMessageLevel.Informational => TestMessageLevel.Informational,
            JTRegTestMessageLevel.Warning => TestMessageLevel.Warning,
            JTRegTestMessageLevel.Error => TestMessageLevel.Error,
            _ => throw new NotImplementedException(),
        };

        public static TestOutcome Convert(JTRegTestOutcome outcome) => outcome switch
        {
            JTRegTestOutcome.None => TestOutcome.None,
            JTRegTestOutcome.Passed => TestOutcome.Passed,
            JTRegTestOutcome.Failed => TestOutcome.Failed,
            JTRegTestOutcome.Skipped => TestOutcome.Skipped,
            _ => throw new NotImplementedException(),
        };

        public static TestResult Convert(JTRegTestResult rslt)
        {
            var r = new TestResult(Convert(rslt.TestCase))
            {
                DisplayName = rslt.DisplayName,
                ComputerName = rslt.ComputerName,
                Duration = rslt.Duration,
                StartTime = rslt.StartTime,
                EndTime = rslt.EndTime,
                Outcome = Convert(rslt.Outcome),
                ErrorMessage = rslt.ErrorMessage,
            };

            foreach (var message in rslt.Messages)
                r.Messages.Add(new TestResultMessage(Convert(message.Category), message.Text));

            var s = new AttachmentSet(new Uri(JTRegTestManager.URI), "IkvmJtRegTestAdapter");
            r.Attachments.Add(s);
            foreach (var attachment in rslt.Attachments)
                s.Attachments.Add(UriDataAttachment.CreateFrom(attachment.Path, attachment.Name));

            return r;
        }

        public static string Convert(JTRegTestResultMessageCategory category) => category switch
        {
            JTRegTestResultMessageCategory.StandardOut => TestResultMessage.StandardOutCategory,
            JTRegTestResultMessageCategory.StandardError => TestResultMessage.StandardErrorCategory,
            JTRegTestResultMessageCategory.AdditionalInfo => TestResultMessage.AdditionalInfoCategory,
            _ => throw new NotImplementedException(),
        };

    }

}
