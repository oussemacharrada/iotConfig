using PeterO.Cbor;
using SuitSolution.Interfaces;

public class SUITInt : ISUITObject
{ public int V { get; set; }

    public ISUITObject FromJson(string v)
    {
        V = int.Parse(v);
        return this;
    }

    public string ToJson()
    {
        return V.ToString();
    }

    public ISUITObject FromSUIT(CBORObject cborObject)
    {
        if (cborObject != null && cborObject.Type == CBORType.Integer)
        {
            V = cborObject.AsInt32();
        }
        return this;
    }

    public object ToSUIT()
    {
        return V;
    }

    public string ToDebug(string indent)
    {
        return V.ToString();
    }
}