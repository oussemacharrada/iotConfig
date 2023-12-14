using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITInt : ISUITObject
{
    public int V { get; set; }
    public ISUITObject FromJson(string v)
    {
        if (!int.TryParse(v, out int result))
        {
            // Handle the error, for example, by logging it and setting a default value
            V = 123; // or any default value you deem appropriate
        }
        else
        {
            V = result;
        }
        return this;
    }

    public string ToJson()
    {
        return V.ToString();
    }

    public ISUITObject FromSUIT(CBORObject cborObject)
    {
        TreeBranch.Append("SUITInt");
        
        if (cborObject != null && cborObject.Type == CBORType.Integer)
        {
            V = cborObject.AsInt32();
        }

        TreeBranch.Pop();
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