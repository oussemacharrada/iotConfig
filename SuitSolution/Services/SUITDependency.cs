using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITDependency class
    public class SUITDependency
    {
        // Properties for dependency fields
        [JsonPropertyName("digest")]
        public string Digest { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }

        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITDependency FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITDependency>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("digest", Digest);
            cborObject.Add("prefix", Prefix);
            return cborObject;
        }

        public static SUITDependency FromSUIT(CBORObject cborObject)
        {
            return new SUITDependency
            {
                Digest = cborObject["digest"].AsString(),
                Prefix = cborObject["prefix"].AsString()
            };
        }
    }
}