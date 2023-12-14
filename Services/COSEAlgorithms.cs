using SuitSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class COSEAlgorithms : SUITKeyMap<object>
{
    private static readonly Dictionary<string, int> _algorithms = new Dictionary<string, int>
    {
        { "ES256", -7 },
        { "ES384", -35 },
        { "ES512", -36 },
        { "EdDSA", -8 }, 
        { "HSS-LMS", -46 },
    };

    private int _currentAlgorithmValue;

    public COSEAlgorithms() : base(_algorithms.ToDictionary(kvp => (object)kvp.Key, kvp => (object)kvp.Value))
    {
        _currentAlgorithmValue = -8;
        currentValue = -8; // Default to EdDSA
    }


    public COSEAlgorithms(int algorithmValue) : this()
    {
        if (_algorithms.ContainsValue(algorithmValue))
        {
            _currentAlgorithmValue = algorithmValue;
            currentValue = algorithmValue;
        }
        else
        {
            throw new KeyNotFoundException($"No algorithm found for value {algorithmValue}.");
        }
    }

    public static COSEAlgorithms CreateDefault()
    {
        return new COSEAlgorithms();
    }

    public static bool TryGetAlgorithmValue(string name, out int value)
    {
        return _algorithms.TryGetValue(name, out value);
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
    public new dynamic ToSUIT()
    {
        // Return the current algorithm value for SUIT representation
        return -8;
    }

    public void SetAlgorithmValue(string algorithmName)
    {
        if (_algorithms.TryGetValue(algorithmName, out int value))
        {
            _currentAlgorithmValue = value;
        }
        else
        {
            throw new KeyNotFoundException($"Algorithm '{algorithmName}' not found.");
        }
    }

    public int GetCurrentAlgorithmValue()
    {
        return _currentAlgorithmValue;
    }
}