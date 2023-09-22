using System;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITDigest
    {
        [JsonPropertyName("algorithm-id")]
        public string AlgorithmId { get; set; }

        [JsonPropertyName("digest-bytes")]
        public byte[] DigestBytes { get; set; }

        public void InitializeRandomData()
        {
            Random random = new Random();

            AlgorithmId = Guid.NewGuid().ToString();

            var length = random.Next(16, 32); 
            var bytes = new byte[length];
            random.NextBytes(bytes);

            DigestBytes = bytes;
        }

        public CBORObject ToSUIT()
        {
            var cborMap = CBORObject.NewMap();
            cborMap.Add("algorithm-id", AlgorithmId);
            cborMap.Add("digest-bytes", CBORObject.FromObject(DigestBytes));

            return cborMap;
        }

        public static SUITDigest FromSUIT(CBORObject cbor)
        {
            if (cbor.Type != CBORType.Map)
                throw new Exception("Invalid CBOR format for SUITDigest");

            var suitDigest = new SUITDigest();

            if (cbor.ContainsKey("algorithm-id"))
            {
                suitDigest.AlgorithmId = cbor["algorithm-id"].AsString();
            }

            if (cbor.ContainsKey("digest-bytes"))
            {
                suitDigest.DigestBytes = cbor["digest-bytes"].GetByteString();
            }

            return suitDigest;
        }
    }
}