using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITCommon class
    public class SUITCommon
    {
        // Properties for fields
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("install")]
        public List<int> Install { get; set; }

        [JsonPropertyName("validate")]
        public List<int> Validate { get; set; }

        // Constructor
        public SUITCommon()
        {
            Install = new List<int>();
            Validate = new List<int>();
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

        public static SUITCommon FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITCommon>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("sequence", Sequence);

            if (Install != null)
            {
                var installArray = CBORObject.NewArray();
                foreach (var item in Install)
                {
                    installArray.Add(item);
                }
                cborObject.Add("install", installArray);
            }

            if (Validate != null)
            {
                var validateArray = CBORObject.NewArray();
                foreach (var item in Validate)
                {
                    validateArray.Add(item);
                }
                cborObject.Add("validate", validateArray);
            }

            return cborObject;
        }

        public static SUITCommon FromSUIT(CBORObject cborObject)
        {
            return new SUITCommon
            {
                Sequence = cborObject["sequence"].AsInt32(),
                Install = cborObject["install"].Values.Select(item => item.AsInt32()).ToList(),
                Validate = cborObject["validate"].Values.Select(item => item.AsInt32()).ToList()
            };
        }
    }
}
