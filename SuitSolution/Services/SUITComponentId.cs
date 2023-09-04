using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITComponentId class
    public class SUITComponentId
    {
        // Properties for fields
        [JsonPropertyName("vendor-id")]
        public string VendorId { get; set; }

        [JsonPropertyName("class-id")]
        public string ClassId { get; set; }

        [JsonPropertyName("image-digest")]
        public string ImageDigest { get; set; }

        // Constructor
        public SUITComponentId()
        {
            // Initialize properties if needed
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

        public static SUITComponentId FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITComponentId>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("vendor-id", VendorId);
            cborObject.Add("class-id", ClassId);
            cborObject.Add("image-digest", ImageDigest);
            return cborObject;
        }

        public static SUITComponentId FromSUIT(CBORObject cborObject)
        {
            return new SUITComponentId
            {
                VendorId = cborObject["vendor-id"].AsString(),
                ClassId = cborObject["class-id"].AsString(),
                ImageDigest = cborObject["image-digest"].AsString()
            };
        }
    }
}

    // SUITComponentText class
    public class SUITComponentText
    {
        // Properties for component text
        [JsonPropertyName("vendor-name")] public string VendorName { get; set; }

        [JsonPropertyName("model-name")] public string ModelName { get; set; }

        [JsonPropertyName("vendor-domain")] public string VendorDomain { get; set; }

        [JsonPropertyName("json-source")] public string JsonSource { get; set; }

        [JsonPropertyName("component-description")]
        public string ComponentDescription { get; set; }

        [JsonPropertyName("version")] public string Version { get; set; }

        [JsonPropertyName("required-version")] public string RequiredVersion { get; set; }

        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITComponentText FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITComponentText>(json);
        }

        // Method for converting to SUIT format
        public Dictionary<object, object> ToSUIT()
        {
            return new Dictionary<object, object>
            {
                { "vendor-name", VendorName },
                { "model-name", ModelName },
                { "vendor-domain", VendorDomain },
                { "json-source", JsonSource },
                { "component-description", ComponentDescription },
                { "version", Version },
                { "required-version", RequiredVersion }
            };
        }

        // Method for converting from SUIT format
        public void FromSUIT(Dictionary<object, object> suitDict)
        {
            VendorName = suitDict.TryGetValue("vendor-name", out var vendorName) ? (string)vendorName : null;
            ModelName = suitDict.TryGetValue("model-name", out var modelName) ? (string)modelName : null;
            VendorDomain = suitDict.TryGetValue("vendor-domain", out var vendorDomain) ? (string)vendorDomain : null;
            JsonSource = suitDict.TryGetValue("json-source", out var jsonSource) ? (string)jsonSource : null;
            ComponentDescription = suitDict.TryGetValue("component-description", out var compDesc)
                ? (string)compDesc
                : null;
            Version = suitDict.TryGetValue("version", out var version) ? (string)version : null;
            RequiredVersion = suitDict.TryGetValue("required-version", out var reqVersion) ? (string)reqVersion : null;
        }
    }

/*Main class for testing
public class Program
{
    public static void Main(string[] args)
    {
        // Create an instance of SUITComponentText
        var componentText = new SUITComponentText
        {
            VendorName = "Vendor",
            ModelName = "Model",
            VendorDomain = "example.com",
            JsonSource = "{\"key\": \"value\"}",
            ComponentDescription = "Component description",
            Version = "1.0",
            RequiredVersion = "1.0"
        };

        // Create an instance of SUITText
        var suitText = new SUITText
        {
            ManifestDescription = "Manifest description",
           
*/