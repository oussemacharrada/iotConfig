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
    public SUITCommon(SUITSequence sequence)
    {
        var common = new SUITSequenceComponentReset(sequence);
        
        this.commonSequence.v = common; 
    }
    public SUITCommon()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
          ("dependencies", 1, () => dependencies,"dependencies"),
         ("components", 2, () => components,"components"),
            ("common_sequence", 4, () => commonSequence,"commonSequence")
        ));
    }

    public SUITCommon(List<SUITComponentId> componentIds, SUITSequence commonSeq)
    {
        // Initialize the 'components' property with the provided component IDs
        this.components = new SUITComponents();
        foreach (var componentId in componentIds)
        {
            this.components.AddComponent(componentId);
        }

        // Initialize the 'commonSequence' property with the provided SUITSequence
        // Wrapped in 'SUITBWrapField' as per your existing structure
        this.commonSequence = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = new SUITSequenceComponentReset(commonSeq)
        };

        // Initialize 'fields' dictionary
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("dependencies", 1, () => dependencies, "dependencies"),
            ("components", 2, () => components, "components"),
            ("common_sequence", 4, () => commonSequence, "commonSequence")
        ));
    }


    public dynamic ToSUIT()
    {
        var dic = base.ToSUIT();
        dic.Add(2, components.ToSUIT());

        dic.Add(4,commonSequence.ToSUIT());
                return dic;
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
          
            public SUITCommon FromSUIT(Dictionary<object, object> suitDict)
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
                if (jsonData == null)
                {
                    throw new ArgumentNullException(nameof(jsonData));
                }

               

                if (jsonData.TryGetValue("components", out var componentsJson) && componentsJson is List<object> componentsList)
                {
                    this.components = new SUITComponents().FromJson(componentsList);
                }

                if (jsonData.TryGetValue("common-sequence", out var commonSeqJson) && commonSeqJson is List<object> commonSeqList && commonSeqList.Count > 0)
                {
                    if (commonSeqList[0] is Dictionary<string, object> commonSeqDict)
                    {
                        this.commonSequence = new SUITBWrapField<SUITSequenceComponentReset>
                        {
                            v = new SUITSequenceComponentReset().FromJson(commonSeqDict)
                        };
                    }
                }


                return this;
            }


            


            public dynamic ToJson()
            {
                var commonDict = new Dictionary<int, object>();
                foreach (var fieldInfo in fields.Values)
                {
                    var fieldName = fieldInfo.JsonKey;
                    var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
                    if (fieldValue != null)
                    {
                        var suitKey = fieldInfo.SuitKey;
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
