using System;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegTestResultMessage
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public JTRegTestResultMessage(JTRegTestResultMessageCategory category, string message)
        {
            Category = category;
            Text = message;
        }

        public JTRegTestResultMessageCategory Category { get; set; }

        public string Text { get; set; }

    }

}
