using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SuitSolution.Services
{
    // SUITBytes class
    public class SUITBytes
    {
        [JsonPropertyName("bytes")]
        public byte[] Bytes { get; set; }

        // Default constructor
        public SUITBytes()
        {
            Bytes = new byte[0]; // Initialize with an empty byte array
        }

        // Constructor
        public SUITBytes(byte[] bytes)
        {
            Bytes = bytes;
        }


        // Serialization methods
      /*  public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITBytes FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITBytes>(json);
        }
*/
        // Method for converting to SUIT format
        public CBORObject ToSUIT()
        {
            return CBORObject.FromObject(Bytes);
        }

        // Method for converting from SUIT format
        public static SUITBytes FromSUIT(CBORObject suitBytes)
        {

            byte[] bytes = suitBytes.EncodeToBytes();
            return new SUITBytes(bytes);
        }
    }
}