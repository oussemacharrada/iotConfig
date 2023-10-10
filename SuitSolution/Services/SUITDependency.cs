using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITDependency : SUITManifestDict, ISUITConvertible<SUITDependency>
    {
        // Properties to hold DependencyDigest and DependencyPrefix
        public SUITDigest DependencyDigest { get; set; }
        public SUITComponentId DependencyPrefix { get; set; }

        // Default constructor initializes properties
        public SUITDependency()
        {
            DependencyDigest = new SUITDigest();
            DependencyPrefix = new SUITComponentId();
        }

        // Parameterized constructor for initializing properties
        public SUITDependency(SUITDigest digest, SUITComponentId prefix)
        {
            DependencyDigest = digest;
            DependencyPrefix = prefix;
        }

        // Convert from SUIT format to object
        public SUITDependency FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict), "Invalid SUIT data for SUITDependency.");
            }

            // Check and initialize properties based on SUIT dictionary keys
            if (suitDict.ContainsKey("dependency-digest"))
            {
                DependencyDigest = new SUITDigest().FromSUIT(suitDict["dependency-digest"] as Dictionary<string, object>);
            }

            if (suitDict.ContainsKey("dependency-prefix"))
            {
                DependencyPrefix = new SUITComponentId().FromSUIT(suitDict["dependency-prefix"] as Dictionary<string, object>);
            }

            return this;
        }

        // Convert from JSON format to object
        public SUITDependency FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData), "Invalid JSON data for SUITDependency.");
            }

            // Check and initialize properties based on JSON keys
            if (jsonData.ContainsKey("0"))
            {
                DependencyDigest = new SUITDigest().FromJson(jsonData["0"] as Dictionary<string, object>);
            }

            if (jsonData.ContainsKey("1"))
            {
                DependencyPrefix = new SUITComponentId().FromJson(jsonData["1"] as Dictionary<string, object>);
            }

            return this;
        }

        // Convert to SUIT format (list of objects)
        public new List<object> ToSUIT()
        {
            var suitList = new List<object>();

            // Add DependencyDigest and DependencyPrefix to the list
            if (DependencyDigest != null)
            {
                suitList.Add(DependencyDigest.ToSUIT());
            }
            else
            {
                suitList.Add(null);
            }

            if (DependencyPrefix != null)
            {
                suitList.Add(DependencyPrefix.ToSUIT());
            }
            else
            {
                suitList.Add(null);
            }

            return suitList;
        }

        // Convert to JSON format (dictionary)
        public Dictionary<string, object> ToJson()
        {
            var jsonDict = new Dictionary<string, object>();

            // Add DependencyDigest and DependencyPrefix to the dictionary
            if (DependencyDigest != null)
            {
                jsonDict["0"] = DependencyDigest.ToJson();
            }

            if (DependencyPrefix != null)
            {
                jsonDict["1"] = DependencyPrefix.ToJson();
            }

            return jsonDict;
        }
    }
}
