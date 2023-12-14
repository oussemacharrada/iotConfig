using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITComponentId : SUITManifestArray<SUITComponentId>, ISUITConvertible<SUITComponentId>
    {
        public List<SUITBytes> componentIds = new List<SUITBytes>();

        public SUITComponentId() { }

        public SUITComponentId(List<SUITBytes> suitBytesList)
        {
            this.componentIds = suitBytesList;
        }

        public SUITComponentId(string suitBytesList)
        {
            List<SUITBytes> result = suitBytesList.Select(item => new SUITBytes(ObjectToByteArray(item))).ToList();

            this.componentIds = result;
        }

        public dynamic ToSUIT()
        {
            return componentIds.Select(item => item.v).ToList();
        }

        public string ToDebug(string indent)
        {
            var newIndent = indent + "    ";
            return "[" + string.Join("", componentIds.Select(item => item.ToDebug(newIndent))) + "]";
        }
        public dynamic ToJson()
        {
            var jsonComponentIds = componentIds.Select(bytes => bytes.ToJson()).ToList();
            return new Dictionary<string, object>
            {
                { "componentIds", jsonComponentIds }
            };
        }
      
        public SUITComponentId FromSUIT(Dictionary<object, object> suitDict)
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
                if (componentIdValue is string strValue)
                {
                    componentIds.Add(new SUITBytes { v = Encoding.UTF8.GetBytes(strValue) });
                }
                else if (componentIdValue is int intValue)
                {
                    componentIds.Add(new SUITBytes { v = BitConverter.GetBytes(intValue) });
                }
                else if (componentIdValue is List<string> stringList)
                {
                    foreach (var item in stringList)
                    {
                        componentIds.Add(new SUITBytes { v = Encoding.UTF8.GetBytes(item) });
                    }
                }
                else if (componentIdValue is List<object> jsonList)
                {
                    foreach (var item in jsonList)
                    {
                        if (item is string listItemStrValue)
                        {
                            componentIds.Add(new SUITBytes { v = Encoding.UTF8.GetBytes(listItemStrValue) });
                        }
                        else if (item is int listItemIntValue)
                        {
                            componentIds.Add(new SUITBytes { v = BitConverter.GetBytes(listItemIntValue) });
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid value type within the JSON list for key 'component_id'.");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid value type for key 'component_id' in JSON.");
                }
            }
            else
            {
                throw new ArgumentException("Missing 'component_id' in JSON.");
            }

            return this;
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            string json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }
        public override bool Equals(object obj)
        {
            if (obj is SUITComponentId other)
            {
                return this.componentIds.SequenceEqual(other.componentIds);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var item in componentIds)
            {
                hash = hash * 23 + item.GetHashCode();
            }
            return hash;
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
    }
}
