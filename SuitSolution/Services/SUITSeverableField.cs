using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITSeverableField<T>
{
    public SUITSeverableField(T v)
    {
        this.v = v;
    }

    public SUITSeverableField()
    {
    }

    public T v { get; set; }

    public SUITSeverableField<T> FromSUIT(object data)
    {
        if (data is Dictionary<string, object> suitDict)
        {
            if (typeof(T) == typeof(SUITDigest))
            {
                var suitDigest = new SUITDigest();
                if (suitDict.TryGetValue("json", out var jsonValue) && jsonValue is string json)
                {
                    var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    suitDigest.FromJson(jsonDict);
                    v = (T)(object)suitDigest;
                }
                else
                {
                    throw new ArgumentException("Invalid 'json' field in SUIT dictionary.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid data type for T");
            }
        }
        else
        {
            throw new ArgumentException("Invalid data type");
        }
        return this;
    }


    public SUITSeverableField<T> FromJson(string data)
    {
        if (typeof(T) == typeof(SUITDigest))
        {
            var suitDigest = JsonConvert.DeserializeObject<SUITDigest>(data);
            v = (T)(object)suitDigest;
        }
        else
        {
            throw new ArgumentException("Invalid JSON data");
        }
        return this;
    }

    public string ToJson()
    {
        if (v is ISUITConvertible<T> convertible)
        {
            return JsonConvert.SerializeObject(convertible);
        }
        else
        {
            throw new InvalidOperationException("Invalid data type for T");
        }
    }

    public Dictionary<string, object> ToSUIT()
    {
        if (v is ISUITConvertible<T> convertible)
        {
            var suitDict = new Dictionary<string, object>
            {
                ["json"] = convertible.ToJson()
            };
            return suitDict;
        }
        else
        {
            throw new InvalidOperationException("Invalid data type for T");
        }
    }
}
