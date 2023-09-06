using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITSequenceComponentReset
    {
        [JsonPropertyName("component")]
        public SUITComponentId Component { get; set; }

        public SUITSequenceComponentReset()
        {
            Component = new SUITComponentId();
        }

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