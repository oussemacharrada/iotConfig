using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITSequenceComponentReset class
    public class SUITSequenceComponentReset
    {
        // Properties for sequence component reset fields
        [JsonPropertyName("component")]
        public SUITComponentId Component { get; set; }

        // Constructor
        public SUITSequenceComponentReset()
        {
            Component = new SUITComponentId();
        }

        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITSequenceComponentReset FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITSequenceComponentReset>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            return Component.ToSUIT();
        }

        public static SUITSequenceComponentReset FromSUIT(CBORObject cborObject)
        {
            var reset = new SUITSequenceComponentReset
            {
                Component = SUITComponentId.FromSUIT(cborObject)
            };
            return reset;
        }
    }
}