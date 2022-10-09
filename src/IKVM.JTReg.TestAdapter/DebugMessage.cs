using System.Text.Json.Serialization;

namespace IKVM.JTReg.TestAdapter
{

    class DebugMessage
    {

        [JsonPropertyName("processId")]
        public int ProcessId { get; set; }

    }

}
