using System;
using System.Collections.Generic;
using SuitSolution.Services;

public class SUITCompressionInfo : SUITKeyMap<int>
{
    public int CompressionMethod { get; private set; }

    public SUITCompressionInfo() : base(MkCompressionMethods())
    {
    }

    private static Dictionary<object, int> MkCompressionMethods()
    {
        return new Dictionary<object, int>
        {
            { "gzip", 1 },
            { "bzip2", 2 },
            { "deflate", 3 },
            { "lz4", 4 },
            { "lzma", 7 }
        };
    }
  
    public void AddCompressionMethod(string methodName, int methodValue)
    {
        KeyMap[methodName] = methodValue;
    }

    public bool TryGetCompressionMethod(string methodName, out int methodValue)
    {
        return KeyMap.TryGetValue(methodName, out methodValue);
    }

    public SUITCompressionInfo FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
            throw new ArgumentNullException(nameof(jsonData));

        foreach (var kvp in jsonData)
        {
            if (KeyMap.TryGetValue(kvp.Key, out int methodValue))
            {
                CompressionMethod = methodValue;
                return this;
            }
        }

        throw new ArgumentException("No matching compression method found in JSON data.");
    }
}