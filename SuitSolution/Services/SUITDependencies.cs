using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITDependencies class
    public class SUITDependencies
    {
        // Property for dependency list
        [JsonPropertyName("dependencies")]
        public List<SUITDependency> Dependencies { get; set; }

        // Constructor
        public SUITDependencies()
        {
            Dependencies = new List<SUITDependency>();
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

        public static SUITDependencies FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITDependencies>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborArray = CBORObject.NewArray();
            foreach (var dependency in Dependencies)
            {
                cborArray.Add(dependency.ToSUIT());
            }
            return cborArray;
        }

        public static SUITDependencies FromSUIT(CBORObject cborArray)
        {
            var dependencies = new SUITDependencies();
            foreach (var item in cborArray.Values)
            {
                dependencies.Dependencies.Add(SUITDependency.FromSUIT(item));
            }
            return dependencies;
        }
    }
}