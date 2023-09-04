using System;
using System.Text.Json;
using PeterO.Cbor;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class COSESign
{
    public CBORObject Protected { get; set; }
    public CBORObject Unprotected { get; set; }
    public CBORObject Payload { get; set; }
    public CBORObject Signature { get; set; }

    // Constructor
    public COSESign()
    {
        Protected = CBORObject.NewMap();
        Unprotected = CBORObject.NewMap();
        Payload = CBORObject.Null;
        Signature = CBORObject.Null;
    }

    // Serialization methods
    /* public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }

    public static COSESign? FromJson(string json)
    {
        return JsonSerializer.Deserialize<COSESign>(json);
    }
*/
    // Method for converting to SUIT format
    public CBORObject ToSUIT()
    {
        CBORObject cborMap = CBORObject.NewMap();

        cborMap.Add(1, Protected);
        cborMap.Add(2, Unprotected);
        cborMap.Add(3, Payload);
        cborMap.Add(4, Signature);

        return cborMap;
    }

    // Method for converting from SUIT format
    public static COSESign FromSUIT(byte[] suitBytes)
    {
        CBORObject cborMap = CBORObject.DecodeFromBytes(suitBytes);
        var coseSign = new COSESign
        {
            Protected = cborMap[1],
            Unprotected = cborMap[2],
            Payload = cborMap[3],
            Signature = cborMap[4]
        };

        return coseSign;
    }
}