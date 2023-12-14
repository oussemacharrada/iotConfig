using System;
using SuitSolution.Interfaces;
using System;
using SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITRaw 
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

        public string ToDebug(string indent)
        {
            return v?.ToString() ?? string.Empty;
        }
    }
}


    public class SUITNil 
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

        public string ToDebug(string indent)
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

    public new  SUITTStr FromJson(object data)
    {
        v = data.ToString();
        return this;
    }

    public new SUITTStr FromSUIT(object data)
    {
        v = data.ToString();
        return this;
    }

    public string ToDebug(string indent)
    {
        return "'" + v.ToString() + "'";
    }
}
