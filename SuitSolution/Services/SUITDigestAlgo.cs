using System;
using System.Collections.Generic;
using System.Text.Json;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITDigestAlgo : SUITKeyMap<int>, ISUITConvertible<SUITDigestAlgo>
{
    public SUITDigestAlgo() : base(MkKeyMaps())
    {
    }

    private static Dictionary<object, int> MkKeyMaps()
    {
        return new Dictionary<object, int>
        {
            { "sha224", 1 },
            { "sha256", 2 },
            { "sha384", 3 },
            { "sha512", 4 }
        };
    }

    public new SUITDigestAlgo FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null)
        {
            throw new ArgumentNullException(nameof(suitDict), "Invalid SUIT data for SUITDigestAlgo.");
        }

        KeyMap.Clear();
        foreach (var pair in suitDict)
        {
            if (pair.Value is int value)
            {
                KeyMap[pair.Key] = value;
            }
        }

        return this;
    }

    public new SUITDigestAlgo FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData), "Invalid JSON data for SUITDigestAlgo.");
        }

        KeyMap.Clear();
        foreach (var pair in jsonData)
        {
            if (pair.Value is int value)
            {
                KeyMap[pair.Key] = value;
            }
        }

        return this;
    }

    public new dynamic ToSUIT()
    {
        var suitDict = new Dictionary<object, object>();
        foreach (var pair in KeyMap)
        {
            suitDict[pair.Key] = pair.Value;
        }
        return suitDict;
    }

    public dynamic ToJson()
    {
        // Convert the keymap to JSON format
        return JsonSerializer.Serialize(KeyMap, new JsonSerializerOptions { WriteIndented = true });
    }

    public string ToDebug(string indent)
    {
        // Convert the keymap to a debug string format
        var debugList = KeyMap.Select(kvp => $"{indent}{kvp.Key}: {kvp.Value}").ToList();
        return $"[\n{string.Join(",\n", debugList)}\n{indent}]";
    }
}
