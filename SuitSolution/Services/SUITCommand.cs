using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // SUITCommand class
    public class SUITCommand
    {
        // Properties for fields
        [JsonPropertyName("sequence-number")]
        public int SequenceNumber { get; set; }

        [JsonPropertyName("command")]
        public int Command { get; set; }

        [JsonPropertyName("args")]
        public List<CBORObject> Args { get; set; }

        // Constructor
        public SUITCommand()
        {
            Args = new List<CBORObject>();
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

        public static SUITCommand FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITCommand>(json);
        }

        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("sequence-number", SequenceNumber);
            cborObject.Add("command", Command);

            if (Args != null)
            {
                var argsArray = CBORObject.NewArray();
                foreach (var arg in Args)
                {
                    argsArray.Add(arg);
                }
                cborObject.Add("args", argsArray);
            }

            return cborObject;
        }

        public static SUITCommand FromSUIT(CBORObject cborObject)
        {
            return new SUITCommand
            {
                SequenceNumber = cborObject["sequence-number"].AsInt32(),
                Command = cborObject["command"].AsInt32(),
                Args = cborObject["args"].Values.ToList()
            };
        }
    }
}