using System;
using PeterO.Cbor;
namespace SuitSolution.Services;

public class SUITDirective
{
    public int DirectiveId { get; set; }
    public SUITParameters Parameters { get; set; }
    public SUITTryEach TryEach { get; set; }
    public SUITCommonInfo Common { get; set; }

    public SUITDirective()
    {
        DirectiveId = 0;
        Parameters = new SUITParameters();
        TryEach = new SUITTryEach();
        Common = new SUITCommonInfo();
    }

    public CBORObject ToCBORObject()
    {
        var cbor = CBORObject.NewMap();

        cbor.Add("directive-id", DirectiveId);
        cbor.Add("parameters", Parameters.ToCBORObject());
        cbor.Add("try-each", TryEach.ToCBORObject());
        cbor.Add("common", Common.ToCBORObject());

        return cbor;
    }

    public void FromCBORObject(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Map)
        {
            throw new FormatException("Invalid CBOR data for SUITDirective");
        }

        DirectiveId = cbor["directive-id"].AsInt32();
        Parameters.FromCBORObject(cbor["parameters"]);
        TryEach.FromCBORObject(cbor["try-each"]);
        Common.FromCBORObject(cbor["common"]);
    }
}
