using System;
using System.Collections.Generic;
using System.Text;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITComponentId : SUITManifestArray<SUITComponentId>, ISUITConvertible<SUITComponentId>
{
    private readonly List<SUITBytes> componentIds = new List<SUITBytes>();

    public SUITComponentId()
    {
    }

    public SUITComponentId(List<SUITBytes> suitBytesList)
    {
        this.componentIds = suitBytesList;
    }

    public Dictionary<string, object> ToSUIT()
    {
        return new Dictionary<string, object>
        {
            { "component_id", componentIds.ConvertAll(item => item.v) }
        };
    }

    public SUITComponentId FromSUIT(List<object> cborObject)
    {
        componentIds.Clear();

        foreach (var item in cborObject)
        {
            if (item is Dictionary<string, object> suitDict && suitDict.ContainsKey("hex") && suitDict["hex"] is byte[] byteArray)
            {
                componentIds.Add(new SUITBytes { v = byteArray });
            }
            else
            {
                throw new ArgumentException("Invalid object type within the list.");
            }
        }

        return this;
    }

    public Dictionary<string, object> ToJson()
    {
        var jsonList = componentIds.ConvertAll(item => Encoding.UTF8.GetString(item.v));
        return new Dictionary<string, object>
        {
            { "component_id", jsonList }
        };
    }

    public SUITComponentId FromSUIT(Dictionary<string, object> suitDict)
    {
        throw new NotImplementedException();
    }

    public SUITComponentId FromJson(Dictionary<string, object> jsonData)
    {
        componentIds.Clear();

        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData));
        }

        if (jsonData.TryGetValue("component_id", out var componentIdValue))
        {
            if (componentIdValue is int singleIntValue)
            {
                // Handle the case where "component_id" is a single integer
                componentIds.Add(new SUITBytes { v = BitConverter.GetBytes(singleIntValue) });
            }
            else if (componentIdValue is List<object> jsonList)
            {
                // Handle the case where "component_id" is a list of integers
                foreach (var item in jsonList)
                {
                    if (item is int intValue)
                    {
                        componentIds.Add(new SUITBytes { v = BitConverter.GetBytes(intValue) });
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type within the JSON list.");
                    }
                }
            }   
            else
            {
                throw new ArgumentException("Invalid value type for 'component_id' in JSON.");
            }
        }
        else
        {
            throw new ArgumentException("Missing 'component_id' in JSON.");
        }

        return this;
    }

    public string ToDebug(string indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{indent}Component ID:");
        foreach (var componentId in componentIds)
        {
            sb.AppendLine($"{indent}  {Encoding.UTF8.GetString(componentId.v)}");
        }
        return sb.ToString();
    }
}
