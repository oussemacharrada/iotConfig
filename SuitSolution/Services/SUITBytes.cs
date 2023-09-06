using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SuitSolution.Services
{
    public class SUITBytes
    {
        [JsonPropertyName("bytes")]
        public byte[] Bytes { get; set; }

        public SUITBytes()
        {
            Bytes = new byte[0]; 
        }

        public SUITBytes(byte[] bytes)
        {
            Bytes = bytes;
        }


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
        public CBORObject ToSUIT()
        {
            return CBORObject.FromObject(Bytes);
        }

        public static SUITBytes FromSUIT(CBORObject suitBytes)
        {

            byte[] bytes = suitBytes.EncodeToBytes();
            return new SUITBytes(bytes);
        }
    }
}