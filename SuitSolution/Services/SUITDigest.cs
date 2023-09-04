using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services;

public class SUITDigest
{
    [JsonPropertyName("algorithm-id")]
    public string AlgorithmId { get; set; }

    [JsonPropertyName("digest-bytes")]
    public byte[] DigestBytes { get; set; }

    // Implement methods for serialization and deserialization here

    public CBORObject ToSUIT()
    {
        // Create a CBOR map representing the SUITDigest object
        var cborMap = CBORObject.NewMap();
        cborMap.Add("digest-bytes", CBORObject.FromObject(DigestBytes));

        return cborMap;
    }

    public SUITDigest FromSUIT(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Map)
            throw new Exception("Invalid CBOR format for SUITDigest");

        var suitDigest = new SUITDigest();

        if (cbor.ContainsKey("digest-bytes"))
        {
            suitDigest.DigestBytes = cbor["digest-bytes"].GetByteString();
        }

        return suitDigest;
    }
}