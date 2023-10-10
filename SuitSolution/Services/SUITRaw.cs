using System;
using SuitSolution.Interfaces;


public class SUITRaw : ISUITMember
{
    public object v;

    public SUITRaw() { }

    public SUITRaw(object value)
    {
        v = value;
    }

    public object ToJson()
    {
        return v;
    }

    public void FromJson(object data)
    {
        v = data;
    }

    public object ToSUIT()
    {
        return v;
    }

    public void FromSUIT(object data)
    {
        v = data;
    }

    public string ToDebug(int indent)
    {
        return v.ToString();
    }
}

public class SUITNil : ISUITMember
{
    public object ToJson()
    {
        return null;
    }

    public void FromJson(object data)
    {
        if (data != null)
        {
            throw new Exception("Expected Nil");
        }
    }

    public object ToSUIT()
    {
        return null;
    }

    public void FromSUIT(object data)
    {
        if (data != null)
        {
            throw new Exception("Expected Nil");
        }
    }

    public string ToDebug(int indent)
    {
        return "F6 / nil /";
    }
}

public class SUITTStr : SUITRaw
{
    public SUITTStr()
    {
    }
    public SUITTStr(string data)
    {
        this.v = data;
    }

    public new SUITTStr FromJson(object data)
    {
        v = data.ToString();
        return this;
    }

    public new SUITTStr FromSUIT(object data)
    {
        v = data.ToString();
        return this;
    }

    public string ToDebug(int indent)
    {
        return "'" + v.ToString() + "'";
    }
}
