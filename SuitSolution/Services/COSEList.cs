using System;
using System.Text.Json;
using PeterO.Cbor;

public class COSEList
{
    public CBORObject[] ListData { get; set; }

    // Constructor
    public COSEList()
    {
        ListData = Array.Empty<CBORObject>();
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

    public static COSEList? FromJson(string json)
    {
        return JsonSerializer.Deserialize<COSEList>(json);
    }

    // Method for converting to SUIT format
    public CBORObject ToSUIT()
    {
        CBORObject cborList = CBORObject.NewArray();
        foreach (var data in ListData)
        {
            cborList.Add(data);
        }
        return cborList;
    }

    // Method for converting from SUIT format
    public static COSEList FromSUIT(byte[] suitBytes)
    {
        CBORObject cborList = CBORObject.DecodeFromBytes(suitBytes);
        var coseList = new COSEList
        {
            ListData = new CBORObject[cborList.Count]
        };

        for (int i = 0; i < cborList.Count; i++)
        {
            coseList.ListData[i] = cborList[i];
        }

        return coseList;
    }
}