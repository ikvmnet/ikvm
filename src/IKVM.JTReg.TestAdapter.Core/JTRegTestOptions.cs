using System;

namespace IKVM.JTReg.TestAdapter.Core
{

    [Serializable]
    public class JTRegTestOptions
    {

        /// <summary>
        /// Gets or sets the number of partitions to generate.
        /// </summary>
        public int PartitionCount { get; set; } = 8;

        /// <summary>
        /// Gets or sets the factor by which to multiply various timeouts.
        /// </summary>
        public float TimeoutFactor { get; set; } = 1.0f;

    }

}
