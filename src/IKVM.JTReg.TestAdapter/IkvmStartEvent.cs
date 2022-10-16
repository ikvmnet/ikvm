using System.Text.Json.Serialization;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Data sent as part of the debug ping protocol.
    /// </summary>
    class IkvmStartEvent
    {

        [JsonPropertyName("processId")]
        public int ProcessId { get; set; }

    }

}
