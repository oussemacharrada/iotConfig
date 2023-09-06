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
        [JsonPropertyName("load-components")]
        public List<SUITComponentId> LoadComponents { get; set; }

        [JsonPropertyName("install-components")]
        public List<SUITComponentId> InstallComponents { get; set; }

        public SUITComponents()
        {
            LoadComponents = new List<SUITComponentId>();
            InstallComponents = new List<SUITComponentId>();
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
