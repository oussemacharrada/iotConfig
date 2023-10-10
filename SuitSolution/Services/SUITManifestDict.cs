using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Interfaces;

public class SUITManifestDict
{
    public readonly string one_indent = "    ";

    public class ManifestKey
    {
        public string json_key { get; private set; }
        public string suit_key { get; private set; }
        public Func<object> ObjectFactory { get; private set; }

        public ManifestKey(string jsonKey, string suitKey, Func<object> objectFactory)
        {
            json_key = jsonKey;
            suit_key = suitKey;
            ObjectFactory = objectFactory;
        }
    }

    public  ReadOnlyDictionary<string, ManifestKey> fields;

    public SUITManifestDict(params (string, string, Func<object>)[] fieldTuples)
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(fieldTuples));
    }

    public SUITManifestDict()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields());
    }

    public static Dictionary<string, ManifestKey> MkFields(params (string, string, Func<object>)[] fieldTuples)
    {
        var fieldDict = new Dictionary<string, ManifestKey>();

        for (int i = 0; i < fieldTuples.Length; i++)
        {
            var (fieldName, suitKey, objFunc) = fieldTuples[i];
            fieldDict[fieldName] = new ManifestKey(fieldName, suitKey, objFunc);
        }

        return fieldDict;
    }

    public Dictionary<string, object> ToSUIT()
    {
        var cborMap = new Dictionary<string, object>();
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
            if (fieldValue != null)
            {
                var suitKey = fieldInfo.suit_key;
                Console.WriteLine($"Converting property '{fieldName}' to CBOR.");

                // Check if fieldValue is a SUITBWrapField<T>
                if (fieldValue.GetType().IsGenericType && fieldValue.GetType().GetGenericTypeDefinition() == typeof(SUITBWrapField<>))
                {
                    var wrappedField = fieldValue as dynamic;
                    if (wrappedField.v != null)
                    {
                        var suitValue = wrappedField.v.ToSUIT();
                        cborMap[suitKey] = suitValue;
                    }
                }
                else
                {
                    // Execute ToSUIT directly if not wrapped
                    var suitValue = (fieldValue as dynamic).ToSUIT();
                    cborMap[suitKey] = suitValue;
                }
            }
            else
            {
                Console.WriteLine($"Field '{fieldName}' is null.");
            }
        }
        return cborMap;
    }



 

    public void FromSUIT(Dictionary<string, object> suitDict)
    {
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var suitKey = fieldInfo.suit_key;

            if (suitDict.ContainsKey(suitKey))
            {
                var suitValue = suitDict[suitKey];
                if (suitValue != null)
                {
                    GetType().GetProperty(fieldName)?.SetValue(this, suitValue);
                }
            }
        }
    }

    public string ToDebug(string indent)
    {
        var debugText = "{";
        var newIndent = indent + one_indent;

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                debugText += $"\n{newIndent}/{fieldInfo.json_key} / {fieldInfo.suit_key}:";
                debugText += (fieldValue as dynamic).ToDebug(newIndent) + ",";
            }
        }

        debugText += $"\n{indent}";
        return debugText;
    }
    public Dictionary<string, object> ToJson()
    {
        var jsonData = new Dictionary<string, object>();

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                jsonData[fieldName] = fieldValue;
            }
        }

        return jsonData;
    }

    public void FromJson(Dictionary<string, object> jsonData)
    {
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.json_key;
            if (jsonData.TryGetValue(fieldName, out var fieldValue))
            {
                GetType().GetProperty(fieldName)?.SetValue(this, fieldValue);
            }
        }
    }
}
