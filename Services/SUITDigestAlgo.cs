using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITDigestAlgo : SUITKeyMap<int>
{
    private int _currentAlgorithmValue;

    public SUITDigestAlgo() : base(MkKeyMaps())
    {
        _currentAlgorithmValue = 2; // Default value indicating SHA256
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

    public void SetAlgorithmValue(int algorithmValue)
    {
        if (reverseKeyMap.ContainsKey(algorithmValue))
        {
            _currentAlgorithmValue = algorithmValue;
        }
        else
        {
            throw new KeyNotFoundException($"Algorithm with value '{algorithmValue}' not found.");
        }
    }

    public HashAlgorithm CreateHashAlgorithm()
    {
        return _currentAlgorithmValue switch
        {
            1 => SHA256.Create(), // Assuming SHA224 is not commonly used and mapping it to SHA256
            2 => SHA256.Create(),
            3 => SHA384.Create(),
            4 => SHA512.Create(),
            _ => throw new InvalidOperationException($"Unsupported digest algorithm with value {_currentAlgorithmValue}."),
        };
    }

    public new dynamic ToSUIT()
    {
        // Return the current algorithm value for SUIT representation
        return _currentAlgorithmValue;
    }

    public new SUITDigestAlgo FromSUIT(object key)
    {
        if (key is int intValue)
        {
            SetAlgorithmValue(intValue);
        }
        else
        {
            throw new ArgumentException("Invalid key type for SUITDigestAlgo.");
        }
        return this;
    }

    public new string ToJson()
    {
        // Serialize the current algorithm value to JSON
        return JsonSerializer.Serialize(_currentAlgorithmValue, new JsonSerializerOptions { WriteIndented = true });
    }

    public new SUITDigestAlgo FromJson(string jsonData)
    {
        if (int.TryParse(jsonData, out int intValue))
        {
            SetAlgorithmValue(intValue);
        }
        else
        {
            throw new ArgumentException("Invalid JSON data for SUITDigestAlgo.");
        }
        return this;
    }

    public string ToDebug(string indent)
    {
        var debugList = KeyMap.Select(kvp => $"{indent}{kvp.Key}: {kvp.Value}").ToList();
        return $"[\n{string.Join(",\n", debugList)}\n{indent}]";
    }
}
