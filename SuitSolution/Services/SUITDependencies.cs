using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    
    public class SUITDependencies
    {
        [JsonPropertyName("dependencies")]
        public List<SUITDependency> Dependencies { get; set; }

        public SUITDependencies()
        {
            Dependencies = new List<SUITDependency>();
        }

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

        public CBORObject ToSUIT()
        {
            var cborArray = CBORObject.NewArray();
            foreach (var dependency in Dependencies)
            {
                cborArray.Add(dependency.ToSUIT());
            }
            return cborArray;
        }

        public SUITDependencies FromSUIT(CBORObject cborArray)
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