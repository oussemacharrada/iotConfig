using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace SuitSolution.Services
{
    public class COSE_Mac0
    {
        [JsonPropertyName("protected")]
        public byte[] Protected { get; set; }

        [JsonPropertyName("unprotected")]

   
        public SUITDigest Payload { get; set; }

        [JsonPropertyName("tag")]
        public SUITBytes Tag { get; set; }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static COSE_Mac0 FromJson(string json)
        {
            return JsonSerializer.Deserialize<COSE_Mac0>(json);
        }

       

       
    }
}
