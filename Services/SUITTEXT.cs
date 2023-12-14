using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITText : SUITManifestDict, ISUITConvertible<SUITText>
{
    public SUITText()
    {
        mdesc = new SUITTStr();
        udesc = new SUITTStr();
        json = new SUITTStr();
        yaml = new SUITTStr();

        components = new Dictionary<SUITComponentId, SUITComponentText>();
    }

    public SUITTStr mdesc { get; set; }
    public SUITTStr udesc { get; set; }
    public SUITTStr json { get; set; }
    public SUITTStr yaml { get; set; }

    public byte[] ConvertSUITTextToByteArray()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        return jsonBytes;
    }

    public Dictionary<SUITComponentId, SUITComponentText> components { get; set; }

    public SUITText FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null)
        {
            throw new ArgumentNullException(nameof(suitDict));
        }

        // Process main fields
        if (suitDict.TryGetValue("mdesc", out var mdescValue) && mdescValue is string mdescStr)
        {
            mdesc = new SUITTStr { v = mdescStr };
        }

        if (suitDict.TryGetValue("udesc", out var udescValue) && udescValue is string udescStr)
        {
            udesc = new SUITTStr { v = udescStr };
        }

        if (suitDict.TryGetValue("json", out var jsonValue) && jsonValue is string jsonStr)
        {
            json = new SUITTStr { v = jsonStr };
        }

        if (suitDict.TryGetValue("yaml", out var yamlValue) && yamlValue is string yamlStr)
        {
            yaml = new SUITTStr { v = yamlStr };
        }

        if (suitDict.TryGetValue("components", out var componentsValue) && componentsValue is List<object> componentsList)
        {
            components = new Dictionary<SUITComponentId, SUITComponentText>();
            foreach (var componentObj in componentsList)
            {
                if (componentObj is Dictionary<object, object> componentDict)
                {
                    if (componentDict.TryGetValue("component_id", out var componentIdObj) && componentIdObj is Dictionary<object, object> componentIdDict)
                    {
                        var componentId = new SUITComponentId();
                        componentId.FromSUIT(componentIdDict);

                        if (componentDict.TryGetValue("component_text", out var componentTextObj) && componentTextObj is Dictionary<object, object> componentTextDict)
                        {
                            var componentText = new SUITComponentText();
                            componentText.FromSUIT(componentTextDict);
                            components.Add(componentId, componentText);
                        }
                    }
                }
            }
        }

        base.FromSUIT(suitDict);

        return this;
    }

    public SUITText FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData));
        }

        if (jsonData.TryGetValue("mdesc", out var mdescValue) && mdescValue is string mdescStr)
        {
            mdesc = new SUITTStr { v = mdescStr };
        }

        if (jsonData.TryGetValue("udesc", out var udescValue) && udescValue is string udescStr)
        {
            udesc = new SUITTStr { v = udescStr };
        }

        if (jsonData.TryGetValue("json", out var jsonValue) && jsonValue is string jsonStr)
        {
            json = new SUITTStr { v = jsonStr };
        }

        if (jsonData.TryGetValue("yaml", out var yamlValue) && yamlValue is string yamlStr)
        {
            yaml = new SUITTStr { v = yamlStr };
        }

        if (jsonData.TryGetValue("components", out var componentsValue) && componentsValue is List<object> componentsList)
        {
            components = new Dictionary<SUITComponentId, SUITComponentText>();
            foreach (var componentObj in componentsList)
            {
                if (componentObj is Dictionary<string, object> componentDict)
                {
                    if (componentDict.TryGetValue("component_id", out var componentIdObj) && componentIdObj is Dictionary<object, object> componentIdDict)
                    {
                        var componentId = new SUITComponentId();
                        componentId.FromSUIT(componentIdDict);

                        if (componentDict.TryGetValue("component_text", out var componentTextObj) && componentTextObj is Dictionary<object, object> componentTextDict)
                        {
                            var componentText = new SUITComponentText();
                            componentText.FromSUIT(componentTextDict);
                            components.Add(componentId, componentText);
                        }
                    }
                }
            }
        }

        return this;
    }

    public new List<object> ToSUIT()
    {
        var suitList = new List<object>();

        foreach (var kvp in components)
        {
            var keyList = kvp.Key.ToSUIT().Select(new Func<object, object>(k => k.ToString())).ToList();
            string selectedKey = null;

            foreach (var key in keyList)
            {
                if (int.TryParse(key, out int index) && index >= 0 && index < keyList.Count)
                {
                    selectedKey = keyList[index];
                    break;
                }
            }

            if (selectedKey != null)
            {
                var componentDict = new Dictionary<string, object>
                {
                    { selectedKey, kvp.Value.ToSUIT() }
                };
                suitList.Add(componentDict);
            }
        }

        var baseProperties = base.ToSUIT();
        if (baseProperties.Count > 0)
        {
            var baseDict = new Dictionary<string, object>();
            foreach (var prop in baseProperties)
            {
                if (prop is KeyValuePair<string, object> kvp)
                {
                    baseDict.Add(kvp.Key, kvp.Value);
                }
            }
            suitList.Add(baseDict);
        }

        return suitList;
    }

    public dynamic ToJson()
    {
        throw new NotImplementedException();
    }

    public new dynamic sToJson()
    {
        var jsonData = new Dictionary<string, object>();

        if (mdesc != null)
        {
            jsonData["mdesc"] = mdesc.v;
        }

        if (udesc != null)
        {
            jsonData["udesc"] = udesc.v;
        }

        if (json != null)
        {
            jsonData["json"] = json.v;
        }

        if (yaml != null)
        {
            jsonData["yaml"] = yaml.v;
        }

        if (components != null)
        {
            var componentsList = new List<object>();
            foreach (var kvp in components)
            {
                var componentDict = new Dictionary<string, object>();
                componentDict["component_id"] = kvp.Key.ToSUIT();
                componentDict["component_text"] = kvp.Value.ToSUIT();
                componentsList.Add(componentDict);
            }
            jsonData["components"] = componentsList;
        }

        return jsonData;
    }

    public new string ToDebug(string indent)
    {
        throw new NotImplementedException();
    }
}
