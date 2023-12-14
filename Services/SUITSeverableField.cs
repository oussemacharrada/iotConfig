using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITSeverableField<T> where T : ISUITConvertible<T>, new()
{
    public T v { get; set; }

    public SUITSeverableField(T value)
    {
        this.v = value;
    }

    public SUITSeverableField()
    {
    }

    public SUITSeverableField<T> FromSUIT(object data)
    {
        if (data is Dictionary<object, object> suitDict)
        {
            var instance = new T();
            instance.FromSUIT(suitDict); // Assuming FromJson can handle the dictionary directly
            v = instance;
        }
        else if (data is List<object> suitList)
        {
            // Process the list and convert it to a dictionary if needed
            var suitDictFromList = ConvertListToDictionary(suitList);
            var instance = new T();
            instance.FromSUIT(suitDictFromList);
            v = instance;
        }
        else
        {
            throw new ArgumentException("Invalid data type");
        }
        return this;
    }

    private Dictionary<object, object> ConvertListToDictionary(List<object> suitList)
    {
        if (suitList.Count % 2 != 0)
        {
            throw new ArgumentException("The list should have an even number of elements.");
        }

        var suitDict = new Dictionary<object, object>();
        for (int i = 0; i < suitList.Count; i += 2)
        {
            if (suitList[i] is string key)
            {
                suitDict[key] = suitList[i + 1];
            }
            else
            {
                throw new ArgumentException("Expected a string key at even indices of the list.");
            }
        }

        return suitDict;
    }


    public SUITSeverableField<T> FromJson(string data)
    {
        v = JsonConvert.DeserializeObject<T>(data);
        return this;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(v);
    }

    public byte[] ToSUIT()
    {
        var cborObject = v.ToSUIT();
        var cbor = CBORObject.FromObject(cborObject);
        byte[] cborBytes = cbor.EncodeToBytes();
        TreeBranch.Pop();
        return cborBytes;
    }


}