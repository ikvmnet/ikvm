using System;
using System.Collections.Generic;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegAttachmentSet
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="displayName"></param>
        public JTRegAttachmentSet(Uri uri, string displayName)
        {
            Uri = uri;
            DisplayName = displayName;
        }

        public Uri Uri { get; set; }

        public string DisplayName { get; set; }

        public List<JTRegUriDataAttachment> Attachments { get; set; } = new();

    }

}