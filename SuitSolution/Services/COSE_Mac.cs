
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuitSolution.Services
{  
    // COSE_Mac class
    public class COSE_Mac
    {
        [JsonPropertyName("protected")]
        public byte[] Protected { get; set; }

        [JsonPropertyName("unprotected")]
        public COSEHeaderMap Unprotected { get; set; }

        [JsonPropertyName("payload")]
        public SUITDigest Payload { get; set; }

        [JsonPropertyName("tag")]
        public SUITBytes Tag { get; set; }

        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static COSE_Mac FromJson(string json)
        {
            return JsonSerializer.Deserialize<COSE_Mac>(json);
        }

        // Method for converting to SUIT format
    
        // Method for converting from SUIT format
    }
}
