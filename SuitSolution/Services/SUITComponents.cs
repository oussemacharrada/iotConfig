using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITComponents
    {
        [JsonPropertyName("install-id")]
        public SUITComponentId InstallId { get; set; }

        [JsonPropertyName("download-id")]
        public SUITComponentId DownloadId { get; set; }

        [JsonPropertyName("load-id")]
        public SUITComponentId LoadId { get; set; }

        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("install-digest")]
        public SUITDigest InstallDigest { get; set; }

        [JsonPropertyName("install-size")]
        public long InstallSize { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("load-components")]
        public List<SUITComponentId> LoadComponents { get; set; }

        [JsonPropertyName("install-components")]
        public List<SUITComponentId> InstallComponents { get; set; }

        public SUITComponents()
        {
            LoadComponents = new List<SUITComponentId>();
            InstallComponents = new List<SUITComponentId>();
        }
        public void InitializeRandomData()
        {
            Random random = new Random();

            int loadCount = random.Next(0, 6); 
            LoadComponents = new List<SUITComponentId>();
            for (int i = 0; i < loadCount; i++)
            {
                var component = new SUITComponentId();
                component.InitializeRandomData();
                LoadComponents.Add(component);
            }

            int installCount = random.Next(0, 6);
            InstallComponents = new List<SUITComponentId>();
            for (int i = 0; i < installCount; i++)
            {
                var component = new SUITComponentId();
                component.InitializeRandomData();
                InstallComponents.Add(component);
            }

            InstallId = new SUITComponentId();
            InstallId.InitializeRandomData();
            DownloadId = new SUITComponentId();
            DownloadId.InitializeRandomData();
            LoadId = new SUITComponentId();
            LoadId.InitializeRandomData();
            File = Guid.NewGuid().ToString(); 
            InstallDigest = new SUITDigest();
            InstallDigest.InitializeRandomData();
            InstallSize = random.Next(100, 1000);
            Uri = $"http://example.com/{Guid.NewGuid()}"; 
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITComponents FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITComponents>(json);
        }

        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();

            if (LoadComponents.Any())
            {
                cborObject.Add("load-components", CBORObject.NewArray().Add(LoadComponents.Select(comp => comp.ToSUIT()).ToArray()));
            }

            if (InstallComponents.Any())
            {
                cborObject.Add("install-components", CBORObject.NewArray().Add(InstallComponents.Select(comp => comp.ToSUIT()).ToArray()));
            }

            return cborObject;
        }

        public SUITComponents FromSUIT(CBORObject cborObject)
        {
            return new SUITComponents
            {
                LoadComponents = cborObject["load-components"]?.Values.Select(comp => SUITComponentId.FromSUIT(comp)).ToList() ?? new List<SUITComponentId>(),
                InstallComponents = cborObject["install-components"]?.Values.Select(comp => SUITComponentId.FromSUIT(comp)).ToList() ?? new List<SUITComponentId>()
            };
        }
    }
}
