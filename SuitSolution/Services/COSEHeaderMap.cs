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
            ("alg", "alg", () => Alg),
            ("kid", "kid", () => Kid)
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

    public COSEHeaderMap FromSUIT(Dictionary<string, object> suitDict)
    {
        if (suitDict == null || suitDict.Count == 0)
        {
            throw new ArgumentException("Invalid SUIT data.");
        }

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var suitKey = fieldInfo.suit_key;

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

    public Dictionary<string, object> ToJson()
    {
        var jsonDict = new Dictionary<string, object>();
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                jsonDict.Add(fieldInfo.json_key, ((ISUITConvertible)fieldValue).ToJson());
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
            var fieldName = fieldInfo.json_key;

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

    public List<object> ToSUIT()
    {
        var cborMap = CBORObject.NewMap();
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                var suitKey = fieldInfo.suit_key;
                var suitValue = (fieldValue as ISUITConvertible<COSEHeaderMap>)?.ToSUIT();

                if (suitValue != null)
                {
                    cborMap.Add(suitKey, CBORObject.DecodeFromBytes(SerializeToJsonBytes(suitValue)));
                }
            }
        }

        return new List<object> { cborMap };
    }
}
