using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITDependencies : List<SUITDependency>, ISUITConvertible<SUITDependencies>
    {
        public new SUITDependencies FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict), "Invalid SUIT data for SUITDependencies.");
            }

            // Clear the existing list before adding new dependencies
            this.Clear();

            if (suitDict.ContainsKey("dependencies") && suitDict["dependencies"] is List<object> dependencyList)
            {
                foreach (var dependencyData in dependencyList)
                {
                    if (dependencyData is Dictionary<string, object> dependencyDict)
                    {
                        var dependency = new SUITDependency();
                        dependency.FromSUIT(dependencyDict);
                        this.Add(dependency);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid object type within the 'dependencies' list.");
                    }
                }
            }

            return this;
        }

        public new SUITDependencies FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData), "Invalid JSON data for SUITDependencies.");
            }

            // Clear the existing list before adding new dependencies
            this.Clear();

            if (jsonData.ContainsKey("dependencies") && jsonData["dependencies"] is List<object> dependencyList)
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

            return this;
        }

        public Dictionary<string, object> ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var dependency in this)
            {
                suitList.Add(dependency.ToSUIT());
            }

            return new Dictionary<string, object>
            {
                { "dependencies", suitList }
            };
        }

        public Dictionary<string, object> ToJson()
        {
            var jsonList = new List<object>();
            foreach (var dependency in this)
            {
                jsonList.Add(dependency.ToJson());
            }

            return new Dictionary<string, object>
            {
                { "dependencies", jsonList }
            };
        }

        public string ToDebug(string indent)
        {
            throw new NotImplementedException();
        }
    }
}

