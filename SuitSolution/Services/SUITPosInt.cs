using System;
using PeterO.Cbor;
using SuitSolution.Interfaces;

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
    }


    public ISUITObject FromJson(string json)
    {
        if (int.TryParse(json, out int _v) && _v >= 0)
        {
            this.v = _v;
            return this;
        }
        else
        {
            throw new Exception("Positive Integers must be >= 0");
        }
    }

    public object FromSUIT(object cborObject)
    {
        int _v = ((CBORObject)cborObject).AsInt32();
        if (_v >= 0)
        {
            this.v = _v;
            return this;
        }
        else
        {
            throw new Exception("Positive Integers must be >= 0");
        }
    }


    

}