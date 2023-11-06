using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITManifestDict
{
    public readonly string one_indent = "    ";

    public class ManifestKey
    {
        public string JsonKey { get; private set; }
        public int SuitKey { get; private set; }
        public Func<object> ObjectFactory { get; private set; }
        public string DescriptiveName { get; private set; }

        public ManifestKey(string jsonKey, int suitKey, Func<object> objectFactory, string descriptiveName)
        {
            JsonKey = jsonKey;
            SuitKey = suitKey;
            ObjectFactory = objectFactory;
            DescriptiveName = descriptiveName;
        }
    }

    public static ReadOnlyDictionary<string, ManifestKey> fields;

    public SUITManifestDict(params (string, int, Func<object>, string)[] fieldTuples)
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(fieldTuples));
    }

    public SUITManifestDict()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields());
    }

    public static Dictionary<string, ManifestKey> MkFields(params (string, int, Func<object>, string)[] fieldTuples)
    {
        var fieldDict = new Dictionary<string, ManifestKey>();

        for (int i = 0; i < fieldTuples.Length; i++)
        {
            var (fieldName, suitKey, objFunc, descriptiveName) = fieldTuples[i];
            fieldDict[fieldName] = new ManifestKey(fieldName, suitKey, objFunc, descriptiveName);
        }

        return fieldDict;
    }

    public dynamic ToSUIT()
    {
        var cborMap = new Dictionary<int, object>();
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.DescriptiveName;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                var suitKey = fieldInfo.SuitKey;
                Console.WriteLine($"Converting property '{fieldName}' to CBOR. '{fieldInfo.SuitKey}'");

                
                    // Execute ToSUIT directly if not wrapped
                    var suitValue = (fieldValue as dynamic).ToSUIT();
                    cborMap[suitKey] = suitValue;
                
            }
            else
            {
                Console.WriteLine($"Field '{fieldName}' is null.");
            }
        }
        return cborMap;
    }



 

  
    public void FromSUIT(Dictionary<object, object> suitDict)
    {
        TreeBranch.Append("SUITManifestDict");  // Using string instead of typeof

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var suitKey = fieldInfo.SuitKey;

            TreeBranch.Append(fieldName);  // This is now valid

            if (suitDict.ContainsKey(suitKey))
            {
                var suitValue = suitDict[suitKey];
                if (suitValue != null)
                {
                    var property = GetType().GetProperty(fieldName);
                    if (property != null)
                    {
                        property.SetValue(this, suitValue);
                    }
                }
            }

            TreeBranch.Pop();
        }

        TreeBranch.Pop();
    }


    public string ToDebug(string indent)
    {
        var debugText = "{";
        var newIndent = indent + one_indent;

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                debugText += $"\n{newIndent}/{fieldInfo.JsonKey} / {fieldInfo.SuitKey}:";
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
            var fieldName = fieldInfo.JsonKey;
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
            var fieldName = fieldInfo.JsonKey;
            if (jsonData.TryGetValue(fieldName, out var fieldValue))
            {
                GetType().GetProperty(fieldName)?.SetValue(this, fieldValue);
            }
        }
    }
}
