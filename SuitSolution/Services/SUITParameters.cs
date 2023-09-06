using System;
using PeterO.Cbor;
namespace SuitSolution.Services
{
public class SUITParameters
{
    public SUITCommon Common { get; set; }
    public int Seq { get; set; }
    public int Size { get; set; }

    public SUITParameters()
    {
        Common = new SUITCommon();
        Seq = 0;
        Size = 0;
    }

    public CBORObject ToCBORObject()
    {
        var cbor = CBORObject.NewArray();

        cbor.Add(Common.ToSUIT());
        cbor.Add(Seq);
        cbor.Add(Size);

        return cbor;
    }

    public void FromCBORObject(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Array || cbor.Count != 3)
        {
            throw new FormatException("Invalid CBOR data for SUITParameters");
        }



        Common.FromCBOR(cbor[0]);
        Seq = cbor[1].AsInt32();
        Size = cbor[2].AsInt32();
    }
}
}