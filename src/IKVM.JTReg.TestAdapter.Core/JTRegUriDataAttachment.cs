using System;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegUriDataAttachment
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        public JTRegUriDataAttachment(Uri uri, string name)
        {
            Uri = uri;
            Description = name;
        }

        public Uri Uri { get; set; }

        public string Description { get; set; }

    }

}