using System;
using System.Text.Json;
using PeterO.Cbor;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class COSETagChoice
{
    public int Tag { get; set; }
    public CBORObject Value { get; set; }

    // Constructor
    public COSETagChoice()
    {
        Tag = 0;
        Value = CBORObject.Null;
    }

    // Serialization methods
    /*public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }

    public static COSETagChoice? FromJson(string json)
    {
        return JsonSerializer.Deserialize<COSETagChoice>(json);
    }
*/
    // Method for converting to SUIT format
    // Method for converting to SUIT format
    public CBORObject ToSUIT()
    {
        CBORObject cborMap = CBORObject.NewMap();
        cborMap.Add(Tag, Value);
        return cborMap;
    }

// Method for converting from SUIT format
    public static COSETagChoice FromSUIT(byte[] suitBytes)
    {
        CBORObject cborMap = CBORObject.DecodeFromBytes(suitBytes);
        if (cborMap.Type == CBORType.Map && cborMap.Count > 0)
        {
            CBORObject tag = cborMap.Keys.FirstOrDefault();
            CBORObject value = cborMap[tag];

            var coseTagChoice = new COSETagChoice
            {
                Tag = tag.AsInt32(),
                Value = value
            };

            return coseTagChoice;
        }
        else
        {
            throw new ArgumentException("Invalid SUIT CBOR map for COSETagChoice");
        }
    }

}