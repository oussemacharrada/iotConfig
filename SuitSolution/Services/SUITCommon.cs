using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using SuitSolution.Interfaces;
using PeterO.Cbor;
namespace SuitSolution.Services
{
    public class SUITCommon : SUITManifestDict, ISUITConvertible<SUITCommon>
    {
        public SUITBWrapField<SUITDependencies> dependencies { get; set; } = new SUITBWrapField<SUITDependencies>();
        public SUITComponents components { get; set; }= new SUITComponents();
        public SUITBWrapField<SUITSequenceComponentReset> commonSequence { get; set; }= new SUITBWrapField<SUITSequenceComponentReset>();

        public SUITCommon()
        {
            fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
                ("dependencies", "1", () => dependencies),
                ("components", "2", () => components),
                ("common-sequence", "4", () => commonSequence)
            ));
        }
        
        private List<object> DeserializeComponents(byte[] data)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(data);
                var componentsList = JsonConvert.DeserializeObject<List<object>>(jsonString);
                return componentsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing components: {ex.Message}");
                return null;
            }
        }
      
        public SUITCommon FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (suitDict.TryGetValue("dependencies", out var dependenciesValue) && dependenciesValue is List<object> dependenciesList)
            {
                var dependenciesLists = SerializationHelper.SerializeToBytes(dependenciesList);
                dependencies.FromSUIT(dependenciesLists);
            }
            else
            {
                throw new ArgumentException("Invalid SUIT data for SUITCommon: missing or invalid 'dependencies' field.");
            }

            if (suitDict.TryGetValue("components", out var componentsValue) && componentsValue is List<object> componentsList)
            {
                components.FromSUIT(componentsList);
            }
            else
            {
                throw new ArgumentException("Invalid SUIT data for SUITCommon: missing or invalid 'components' field.");
            }



            if (suitDict.TryGetValue("common-sequence", out var commonSeqValue) && commonSeqValue is List<object> commonSeqList)
            {
                var commonSeqLists = SerializationHelper.SerializeToBytes(commonSeqList);
                commonSequence.FromSUIT(commonSeqLists); 
            }
            else
            {
                throw new ArgumentException("Invalid SUIT data for SUITCommon: missing or invalid 'common-sequence' field.");
            }

            return this;
        }


        public SUITCommon FromJson(Dictionary<string, object> jsonData)
        {
            throw new NotImplementedException();
        }
     
     


      

        public Dictionary<string, object> ToJson()
        {
            var commonDict = new Dictionary<string, object>();
            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.json_key;
                var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
                if (fieldValue != null)
                {
                    var suitKey = fieldInfo.suit_key;
                    var suitValue = (fieldValue as dynamic).ToJson();
                    commonDict[suitKey] = suitValue;
                }
            }
            return new Dictionary<string, object>
            {
                { "common", commonDict }
            };
        }
    }
}
