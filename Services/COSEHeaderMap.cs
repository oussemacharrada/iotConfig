using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using SuitSolution.Services;
using PeterO.Cbor;
using SuitSolution.Interfaces;

public static class DebugUtils
{
    public static string DictionaryToDebugString(Dictionary<string, string> dict, string indent)
    {
        var sb = new StringBuilder();
        foreach (var kvp in dict)
        {
            sb.AppendLine($"{indent}{kvp.Key}: {kvp.Value}");
        }
        return sb.ToString();
    }
}

public class COSEHeaderMap : SUITManifestDict, ISUITConvertible<COSEHeaderMap>
{
    public COSEAlgorithms Alg { get; set; }
    public SUITBytes Kid { get; set; }

    public COSEHeaderMap()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("alg", 1, () => Alg,"Alg"),
            ("kid", 4, () => Kid,"Kid")
        ));
    }

    public static byte[] SerializeToJsonBytes(object obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        return Encoding.UTF8.GetBytes(json);
    }

    public static T DeserializeFromJsonBytes<T>(byte[] bytes)
    {
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public COSEHeaderMap FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null || suitDict.Count == 0)
        {
            throw new ArgumentException("Invalid SUIT data.");
        }

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var suitKey = fieldInfo.SuitKey;

            if (suitDict.ContainsKey(suitKey))
            {
                var fieldValue = suitDict[suitKey];
                var property = GetType().GetProperty(fieldName);
                if (property != null)
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    if (value is ISUITConvertible convertible)
                    {
                        convertible.FromSUIT((Dictionary<string, object>)fieldValue);
                        property.SetValue(this, value);
                    }
                }
            }
        }

        return this;
    }

    public dynamic ToJson()
    {
        var jsonDict = new Dictionary<string, object>();
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                jsonDict.Add(fieldInfo.JsonKey, ((ISUITConvertible)fieldValue).ToJson());
            }
        }

        return jsonDict;
    }

    public COSEHeaderMap FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null || jsonData.Count == 0)
        {
            throw new ArgumentException("Invalid JSON data.");
        }

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;

            if (jsonData.ContainsKey(fieldName))
            {
                var fieldValue = jsonData[fieldName];
                var property = GetType().GetProperty(fieldName);
                if (property != null)
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    if (value is ISUITConvertible convertible)
                    {
                        convertible.FromJson((Dictionary<string, object>)fieldValue);
                        property.SetValue(this, value);
                    }
                }
            }
        }

        return this;
    }

    public dynamic ToSUIT()
    {
        var cborMap = CBORObject.NewMap();

        if (Alg != null )
        {
            cborMap.Add(1, Alg.ToSUIT()); 
        }

        if (Kid != null)
        {
            cborMap.Add(4, Kid.ToSUIT()); 
        }

        return cborMap;
    }
}
