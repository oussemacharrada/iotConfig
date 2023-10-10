using System;
using System.Collections.Generic;
using System.Reflection;

public class SUITComponentText : SUITManifestDict
{
    public SUITComponentText()
    {
        InitializeFields();
    }

    private void InitializeFields()
    {
        // Initialize fields here.
        VendorName = new SUITTStr();
        ModelName = new SUITTStr();
        VendorDomain = new SUITTStr();
        ModelInfo = new SUITTStr();
        ComponentDescription = new SUITTStr();
        Version = new SUITTStr();
        RequiredVersion = new SUITTStr();
    }

    public SUITTStr VendorName { get; set; }
    public SUITTStr ModelName { get; set; }
    public SUITTStr VendorDomain { get; set; }
    public SUITTStr ModelInfo { get; set; }
    public SUITTStr ComponentDescription { get; set; }
    public SUITTStr Version { get; set; }
    public SUITTStr RequiredVersion { get; set; }

    public Dictionary<string, object> ToSUIT()
    {
        var suitDict = new Dictionary<string, object>();

        suitDict["vendor-name"] = VendorName.ToSUIT();
        suitDict["model-name"] = ModelName.ToSUIT();
        suitDict["vendor-domain"] = VendorDomain.ToSUIT();
        suitDict["json-source"] = ModelInfo.ToSUIT();
        suitDict["component-description"] = ComponentDescription.ToSUIT();
        suitDict["version"] = Version.ToSUIT();
        suitDict["required-version"] = RequiredVersion.ToSUIT();

        return suitDict;
    }
    public static Dictionary<string, object> ToSUITDictionary(Dictionary<SUITComponentId, SUITComponentText> inputDictionary)
    {
        var suitDict = new Dictionary<string, object>();

        foreach (var kvp in inputDictionary)
        {
            var suitKey = kvp.Key.ToSUIT(); // Assuming SUITComponentId has a ToSUIT method
            var suitValue = kvp.Value.ToSUIT(); // Assuming SUITComponentText has a ToSUIT method

            suitDict[suitKey.ToString()] = suitValue;
        }

        return suitDict;
    }
}


public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}