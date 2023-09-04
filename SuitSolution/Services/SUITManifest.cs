using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITManifest class
    public class SUITManifest
    {
        // Properties for fields
        [JsonPropertyName("manifest-version")]
        public int ManifestVersion { get; set; }

        [JsonPropertyName("manifest-sequence-number")]
        public int ManifestSequenceNumber { get; set; }

        [JsonPropertyName("common")]
        public SUITCommon Common { get; set; }

        [JsonPropertyName("components")]
        public List<SUITComponents> Components { get; set; }

        [JsonPropertyName("dependencies")]
        public List<SUITDependencies> Dependencies { get; set; }

        // Constructor
        public SUITManifest()
        {
            Components = new List<SUITComponents>();
            Dependencies = new List<SUITDependencies>();
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

        public static SUITManifest FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITManifest>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("manifest-version", ManifestVersion);
            cborObject.Add("manifest-sequence-number", ManifestSequenceNumber);
            cborObject.Add("common", Common.ToSUIT());

            if (Components != null)
            {
                var componentsArray = CBORObject.NewArray();
                foreach (var component in Components)
                {
                    componentsArray.Add(component.ToSUIT());
                }
                cborObject.Add("components", componentsArray);
            }

            if (Dependencies != null)
            {
                var dependenciesArray = CBORObject.NewArray();
                foreach (var dependency in Dependencies)
                {
                    dependenciesArray.Add(dependency.ToSUIT());
                }
                cborObject.Add("dependencies", dependenciesArray);
            }

            return cborObject;
        }

        public static SUITManifest FromSUIT(CBORObject cborObject)
        {
            return new SUITManifest
            {
                ManifestVersion = cborObject["manifest-version"].AsInt32(),
                ManifestSequenceNumber = cborObject["manifest-sequence-number"].AsInt32(),
                Common = SUITCommon.FromSUIT(cborObject["common"]),
                Components = cborObject["components"].Values.Select(component => SUITComponents.FromSUIT(component)).ToList(),
                Dependencies = cborObject["dependencies"].Values.Select(dependency => SUITDependencies.FromSUIT(dependency)).ToList()
            };
        }
    }

    // Other classes (SUITCommon, SUITComponents, SUITDependencies) are similar in structure and implementation.
    // You can follow a similar pattern to the one shown above for their implementation.
}
