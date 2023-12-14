using System;
using System.Collections.Generic;
using System.Linq;

namespace SuitSolution.Services
{
    public class SUITComponentText : SUITManifestDict
    {
        public SUITTStr VendorName { get; set; } = new SUITTStr();
        public SUITTStr ModelName { get; set; } = new SUITTStr();
        public SUITTStr VendorDomain { get; set; } = new SUITTStr();
        public SUITTStr ModelInfo { get; set; } = new SUITTStr();
        public SUITTStr ComponentDescription { get; set; } = new SUITTStr();
        public SUITTStr Version { get; set; } = new SUITTStr();
        public SUITTStr RequiredVersion { get; set; } = new SUITTStr();

        public Dictionary<string, object> ToSUIT()
        {
            return new Dictionary<string, object>
            {
                ["vendor-name"] = VendorName.ToSUIT(),
                ["model-name"] = ModelName.ToSUIT(),
                ["vendor-domain"] = VendorDomain.ToSUIT(),
                ["json-source"] = ModelInfo.ToSUIT(),
                ["component-description"] = ComponentDescription.ToSUIT(),
                ["version"] = Version.ToSUIT(),
                ["required-version"] = RequiredVersion.ToSUIT()
            };
        }

        public static Dictionary<string, object> ToSUITDictionary(Dictionary<SUITComponentId, SUITComponentText> inputDictionary)
        {
            var suitDict = new Dictionary<string, object>();

            foreach (var kvp in inputDictionary)
            {
                var suitKey = kvp.Key.ToSUIT(); // Assuming SUITComponentId has a ToSUIT method
                var suitValue = kvp.Value.ToSUIT(); // Using the ToSUIT method from this class

                suitDict[suitKey.ToString()] = suitValue;
            }

            return suitDict;
        }
    }
}

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}
