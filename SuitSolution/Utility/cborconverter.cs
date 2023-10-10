using PeterO.Cbor;

namespace SuitSolution.Services.Utility;

public class cborconverter
{

public class CborConverter
{
    public CBORObject ConvertDictionaryToCbor(Dictionary<string, object> data)
    {
        CBORObject cborMap = CBORObject.NewMap();

        foreach (var kvp in data)
        {
            string key = kvp.Key;
            object value = kvp.Value;

            CBORObject cborKey = CBORObject.FromObject(key);

            if (value is Dictionary<string, object> nestedDict)
            {
                // Recursively convert nested dictionaries
                CBORObject cborNestedDict = ConvertDictionaryToCbor(nestedDict);
                cborMap.Add(cborKey, cborNestedDict);
            }
            else
            {
                // Convert other data types to CBOR
                CBORObject cborValue = CBORObject.FromObject(value);
                cborMap.Add(cborKey, cborValue);
            }
        }

        return cborMap;
    }
}

    
}