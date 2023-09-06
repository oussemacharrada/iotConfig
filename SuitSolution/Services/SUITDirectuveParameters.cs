using System;
using PeterO.Cbor;
namespace SuitSolution.Services
{
public class SUITDirectiveParameters
{
    public int Weight { get; set; }
    public SUITParameters Parameters { get; set; }

    public SUITDirectiveParameters()
    {
        Weight = 0;
        Parameters = new SUITParameters();
    }

    public CBORObject ToCBORObject()
    {
        var cbor = CBORObject.NewArray();

        cbor.Add(Weight);
        cbor.Add(Parameters.ToCBORObject());

        return cbor;
    }

    public void FromCBORObject(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Array || cbor.Count != 2)
        {
            throw new FormatException("Invalid CBOR data for SUITDirectiveParameters");
        }

        Weight = cbor[0].AsInt32();
        Parameters.FromCBORObject(cbor[1]);
    }
} 
}