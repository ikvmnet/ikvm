using System.Collections.Generic;

namespace IKVM.Tools.Importer
{

    struct ImportArgLevel
    {

        public int Depth;
        public List<string> Args;
        public List<ImportArgLevel> Nested;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="depth"></param>
        public ImportArgLevel(int depth)
        {
            Depth = depth;
            Args = new List<string>();
            Nested = new List<ImportArgLevel>();
        }

    }

}
