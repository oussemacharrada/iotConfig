using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITPosInt : SUITInt
{
    public int v;

    public SUITPosInt()
    {
        this.v = 0; 
    }

    public SUITPosInt(int cidx)
    {
        if (cidx < 0)
        {
            throw new ArgumentException("Positive Integers must be >= 0", nameof(cidx));
        }

        this.v = cidx;
        this.V = cidx;
    }

    public new ISUITObject FromJson(string json)
    {
        TreeBranch.Append("SUITPosInt");

        if (int.TryParse(json, out int _v) && _v >= 0)
        {
            this.v = _v;
            TreeBranch.Pop();
            return this;
        }
        else
        {
            TreeBranch.Pop();
            throw new Exception("Positive Integers must be >= 0");
        }
    }

    public new object FromSUIT(object cborObject)
    {
        TreeBranch.Append("SUITPosInt");

        int _v = ((CBORObject)cborObject).AsInt32();
        if (_v >= 0)
        {
            this.v = _v;
            TreeBranch.Pop();
            return this;
        }
        else
        {
            TreeBranch.Pop();
            throw new Exception("Positive Integers must be >= 0");
        }
    }
}