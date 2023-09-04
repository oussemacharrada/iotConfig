using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITSequence class
    public class SUITSequence
    {
        // Properties for sequence fields
        [JsonPropertyName("seq")]
        public List<SUITComponentId> Sequence { get; set; }

        // Constructor
        public SUITSequence()
        {
            Sequence = new List<SUITComponentId>();
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

        public static SUITSequence FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITSequence>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborArray = CBORObject.NewArray();
            foreach (var componentId in Sequence)
            {
                cborArray.Add(componentId.ToSUIT());
            }
            return cborArray;
        }

        public static SUITSequence FromSUIT(CBORObject cborArray)
        {
            var sequence = new SUITSequence();
            foreach (var item in cborArray.Values)
            {
                sequence.Sequence.Add(SUITComponentId.FromSUIT(item));
            }
            return sequence;
        }
    }
}