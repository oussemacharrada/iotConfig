using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITDependencies : List<SUITDependency>, ISUITConvertible<SUITDependencies>
    {
        // Convert from SUIT format to object
        public SUITDependencies FromSUIT(Dictionary<object, object> suitDict)
        {
            return FromData(suitDict, true);
        }

        // Convert from JSON format to object
        public SUITDependencies FromJson(Dictionary<string, object> jsonData)
        {
            return FromJsonData(jsonData, false);
        }

        // Convert to SUIT format
        public dynamic ToSUIT()
        {
            return ToData();
        }

        dynamic ISUITConvertible<SUITDependencies>.ToJson()
        {
            return ToJson();
        }

        public string ToDebug(string indent)
        {
            throw new NotImplementedException();
        }

        // Convert to JSON format
        public Dictionary<string, object> ToJson()
        {
            return ToData();
        }

        // Helper method to convert from SUIT or JSON format to object
        private SUITDependencies FromData(Dictionary<object, object> dataDict, bool isSUIT)
        {
            if (dataDict == null)
            {
                throw new ArgumentNullException(nameof(dataDict), "Invalid data for SUITDependencies.");
            }

            this.Clear();

            if (dataDict.ContainsKey("dependencies") && dataDict["dependencies"] is List<object> dependencyList)
            {
                foreach (var dependencyData in dependencyList)
                {
                    if (dependencyData is Dictionary<object, object> dependencyDict)
                    {
                        var dependency = new SUITDependency();
                            dependency.FromSUIT(dependencyDict);
                  
                    }
                    else
                    {
                        throw new ArgumentException("Invalid object type within the 'dependencies' list.");
                    }
                }
            }

            // Update the static Dependencies property of SUITCommonInfo
            SUITCommonInfo.Dependencies = this;

            return this;
        }
        private SUITDependencies FromJsonData(Dictionary<string, object> dataDict, bool isSUIT)
        {
            if (dataDict == null)
            {
                throw new ArgumentNullException(nameof(dataDict), "Invalid data for SUITDependencies.");
            }

            this.Clear();

            if (dataDict.ContainsKey("dependencies") && dataDict["dependencies"] is List<object> dependencyList)
            {
                foreach (var dependencyData in dependencyList)
                {
                    if (dependencyData is Dictionary<string, object> dependencyDict)
                    {
                        var dependency = new SUITDependency();
                       
                            dependency.FromJson(dependencyDict);
                        this.Add(dependency);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid object type within the 'dependencies' list.");
                    }
                }
            }

            // Update the static Dependencies property of SUITCommonInfo
            SUITCommonInfo.Dependencies = this;

            return this;
        }

        // Helper method to convert to SUIT or JSON format
        private Dictionary<string, object> ToData()
        {
            var dataList = new List<object>();
            foreach (var dependency in this)
            {
                dataList.Add(dependency.ToSUIT()); // Convert each dependency to SUIT format
            }

            return new Dictionary<string, object>
            {
                { "dependencies", dataList }
            };
        }
    }
}
