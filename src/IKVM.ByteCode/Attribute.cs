using System;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents the raw unlinked structure of an attribute.
    /// </summary>
    public class Attribute
    {

        public string Name { get; set; }

        public ReadOnlyMemory<byte> Info { get; set; }

    }

}
