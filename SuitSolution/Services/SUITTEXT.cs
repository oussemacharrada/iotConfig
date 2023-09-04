using System.Collections.Generic;
using System.Text.Json.Serialization;
using PeterO.Cbor;
namespace SuitSolution.Services;

public class SUITText
{
    [JsonPropertyName("languages")]
    public List<string> Languages { get; set; }

    [JsonPropertyName("texts")]
    public List<string> Texts { get; set; }

    public SUITText()
    {
        Languages = new List<string>();
        Texts = new List<string>();
    }

    public CBORObject ToSUIT()
    {
        CBORObject cborObject = CBORObject.NewMap();
        cborObject.Add("languages", CBORObject.NewArray().Add(Languages.ToArray()));
        cborObject.Add("texts", CBORObject.NewArray().Add(Texts.ToArray()));
        return cborObject;
    }

    public static SUITText FromSUIT(CBORObject cborObject)
    {
        return new SUITText
        {
            Languages = new List<string>(cborObject["languages"].Values.Select(l => l.AsString())),
            Texts = new List<string>(cborObject["texts"].Values.Select(t => t.AsString()))
        };
    }
}
