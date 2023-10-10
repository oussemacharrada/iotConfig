using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITDigestAlgo : SUITKeyMap<int>, ISUITConvertible<SUITDigestAlgo>
{
    public SUITDigestAlgo() : base(MkKeyMaps())
    {
    }

    private static Dictionary<string, int> MkKeyMaps()
    {
        return new Dictionary<string, int>
        {
            { "sha224", 1 },
            { "sha256", 2 },
            { "sha384", 3 },
            { "sha512", 4 }
        };
    }

    public new SUITDigestAlgo FromSUIT(Dictionary<string, object> suitDict)
    {
        if (suitDict == null)
        {
            throw new ArgumentNullException(nameof(suitDict), "Invalid SUIT data for SUITDigestAlgo.");
        }

        keymap.Clear();
        foreach (var pair in suitDict)
        {
            if (pair.Value is int value)
            {
                keymap[pair.Key] = value;
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

        keymap.Clear();
        foreach (var pair in jsonData)
        {
            if (pair.Value is int value)
            {
                keymap[pair.Key] = value;
            }
        }

        return this;
    }

    public new Dictionary<string, object> ToSUIT()
    {
        var suitDict = new Dictionary<string, object>();
        foreach (var pair in keymap)
        {
            suitDict[pair.Key] = pair.Value;
        }
        return suitDict;
    }

    public Dictionary<string, object> ToJson()
    {
        throw new NotImplementedException();
    }

    public string ToDebug(string indent)
    {
        throw new NotImplementedException();
    }
}