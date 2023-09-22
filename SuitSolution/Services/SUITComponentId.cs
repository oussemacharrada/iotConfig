using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITComponentId
    {
        
        [JsonPropertyName("vendor-id")]
        public string VendorId { get; set; }

        [JsonPropertyName("class-id")]
        public string ClassId { get; set; }

        [JsonPropertyName("image-digest")]
        public string ImageDigest { get; set; }
        public string Id { get; set; } 
        public SUITComponentId()
        {
        }
        public void InitializeRandomData()
        {
            Random random = new Random();

            VendorId = Guid.NewGuid().ToString();
            ClassId = Guid.NewGuid().ToString();
            ImageDigest = Guid.NewGuid().ToString(); 
            Id = Guid.NewGuid().ToString(); 
        }

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

        public CBORObject ToSUIT()
        {
            var cborMap = CBORObject.NewMap();
            cborMap.Add("vendor-id", VendorId);
            cborMap.Add("class-id", ClassId);
            cborMap.Add("image-digest", ImageDigest);
            cborMap.Add("Id", Id); 

            return cborMap;
        }



        public static SUITComponentId FromSUIT(CBORObject cborObject)
        {
            if (cborObject == null || cborObject.Type != CBORType.Map)
            {
                throw new ArgumentException("Invalid CBOR object or type.");
            }

            var componentId = new SUITComponentId();
            foreach (var key in cborObject.Keys)
            {
                switch (key.AsString())
                {
                    case "vendor-id":
                        componentId.VendorId = cborObject[key].AsString();
                        break;
                    case "class-id":
                        componentId.ClassId = cborObject[key].AsString();
                        break;
                    case "image-digest":
                        componentId.ImageDigest = cborObject[key].AsString();
                        break;
                    case "Id":
                        componentId.Id = cborObject[key].AsString();
                        break;
                    default:
                        break;
                }
            }

            return componentId;
        }

    }
}

    public class SUITComponentText
    {
        [JsonPropertyName("vendor-name")] public string? VendorName { get; set; }

        [JsonPropertyName("model-name")] public string? ModelName { get; set; }

        [JsonPropertyName("vendor-domain")] public string? VendorDomain { get; set; }

        [JsonPropertyName("json-source")] public string? JsonSource { get; set; }

        [JsonPropertyName("component-description")]
        public string? ComponentDescription { get; set; }

        [JsonPropertyName("version")] public string? Version { get; set; }

        [JsonPropertyName("required-version")] public string? RequiredVersion { get; set; }

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

        public Dictionary<object, object?> ToSUIT()
        {
            return new Dictionary<object, object?>
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

        public void FromSUIT(Dictionary<string, object> suitDict)
        {
            VendorName = GetStringFromSUITDict(suitDict, "vendor-name");
            ModelName = GetStringFromSUITDict(suitDict, "model-name");
            VendorDomain = GetStringFromSUITDict(suitDict, "vendor-domain");
            JsonSource = GetStringFromSUITDict(suitDict, "json-source");
            ComponentDescription = GetStringFromSUITDict(suitDict, "component-description");
            Version = GetStringFromSUITDict(suitDict, "version");
            RequiredVersion = GetStringFromSUITDict(suitDict, "required-version");
        }

        private string GetStringFromSUITDict(Dictionary<string, object> suitDict, string key)
        {
            if (suitDict.TryGetValue(key, out var value) && value is CBORObject cborString)
            {
                return cborString.AsString();
            }
            return null;
        }
    }

/*
public class Program
{
    public static void Main(string[] args)
    {
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

        var suitText = new SUITText
        {
            ManifestDescription = "Manifest description",
           
*/