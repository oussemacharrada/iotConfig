using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITDependency
    {
        [JsonPropertyName("digest")]
        public string Digest { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }

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