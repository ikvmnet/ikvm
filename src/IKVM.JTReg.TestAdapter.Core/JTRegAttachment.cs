using System;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegAttachment
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public JTRegAttachment(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public string Path { get; set; }

        public string Name { get; set; }

    }

}