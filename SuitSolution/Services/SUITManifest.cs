using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITManifest : ISUITConvertible
    {
        [JsonPropertyName("manifest-version")]
        public int ManifestVersion { get; set; }

        [JsonPropertyName("manifest-sequence-number")]
        public int ManifestSequenceNumber { get; set; }

        [JsonPropertyName("common")]
        public SUITCommon Common { get; set; }

        [JsonPropertyName("components")]
        public List<SUITComponents> Components { get; set; }

        [JsonPropertyName("dependencies")]
        public List<SUITDependencies> Dependencies { get; set; }

        public SUITManifest()
        {
            Common = new SUITCommon();
            Components = new List<SUITComponents>();
            Dependencies = new List<SUITDependencies>();
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITManifest FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITManifest>(json);
        }

        public CBORObject ToCBOR()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("manifest-version", ManifestVersion);
            cborObject.Add("manifest-sequence-number", ManifestSequenceNumber);
            cborObject.Add("common", Common.ToSUIT());

            if (Components != null)
            {
                var componentsArray = CBORObject.NewArray();
                foreach (var component in Components)
                {
                    componentsArray.Add(component.ToSUIT());
                }
                cborObject.Add("components", componentsArray);
            }

            if (Dependencies != null)
            {
                var dependenciesArray = CBORObject.NewArray();
                foreach (var dependency in Dependencies)
                {
                    dependenciesArray.Add(dependency.ToSUIT());
                }
                cborObject.Add("dependencies", dependenciesArray);
            }

            return cborObject;
        }

        public void FromCBOR(CBORObject cborObject)
        {
            if (cborObject == null || cborObject.Type != CBORType.Map)
            {
                throw new ArgumentException("Invalid CBOR object or type.");
            }

            ManifestVersion = cborObject["manifest-version"].AsInt32();
            ManifestSequenceNumber = cborObject["manifest-sequence-number"].AsInt32();
            Common = new SUITCommon();
            Common.FromCBOR(cborObject["common"]);

            if (cborObject.ContainsKey("components"))
            {
                var componentsList = cborObject["components"].Values;
                Components = componentsList.Select(item => new SUITComponents().FromSUIT(item)).ToList();
            }
            else
            {
                Components = new List<SUITComponents>();
            }

            if (cborObject.ContainsKey("dependencies"))
            {
                var dependenciesList = cborObject["dependencies"].Values;
                Dependencies = dependenciesList.Select(item => new SUITDependencies().FromSUIT(item)).ToList();
            }
            else
            {
                Dependencies = new List<SUITDependencies>();
            }
        }

        public List<object> ToSUIT()
        {
            var commonDict = Common.ToSUITDict();
            var componentsList = Components.Select(component => component.ToSUIT()).ToList();
            var dependenciesList = Dependencies.Select(dependency => dependency.ToSUIT()).ToList();

            return new List<object>
            {
                ManifestVersion,
                ManifestSequenceNumber,
                commonDict,
                componentsList,
                dependenciesList
            };
        }

       

       /* public SUITManifest FromSUIT(CBORObject cborObject)
        {
            var suitList = cborObject.ToObject<List<object>>();
            var manifest = new SUITManifest();

            foreach (var item in suitList)
            {
                if (item is Dictionary<object, object> dictionary)
                {
                    if (dictionary.ContainsKey("manifest-version"))
                    {
                        manifest.ManifestVersion = Convert.ToInt32(dictionary["manifest-version"]);
                    }
                    else if (dictionary.ContainsKey("manifest-sequence-number"))
                    {
                        manifest.ManifestSequenceNumber = Convert.ToInt32(dictionary["manifest-sequence-number"]);
                    }
                    else if (dictionary.ContainsKey("common"))
                    {
                        var common = new SUITCommon();
                        common.FromSUITDict(dictionary["common"] as Dictionary<object, object>);
                        manifest.Common = common;
                    }
                    else if (dictionary.ContainsKey("components"))
                    {
                        manifest.Components = ((List<object>)dictionary["components"]).Select(component =>
                            new SUITComponents().FromSUIT(CBORObject.FromObject(component))).ToList();
                    }
                    else if (dictionary.ContainsKey("dependencies"))
                    {
                        manifest.Dependencies = ((List<object>)dictionary["dependencies"]).Select(dependency =>
                            new SUITDependencies().FromSUIT(CBORObject.FromObject(dependency))).ToList();
                    }
                }
            }

            return manifest;
        }
*/
       public void InitializeRandomData()
       {
           Random random = new Random();

          
           ManifestVersion = random.Next(1, 10); 
           ManifestSequenceNumber = random.Next(1000, 9999); 

      
           Common = new SUITCommon();
           Common.InitializeRandomData();

      
           Components.Clear();
           int componentCount = random.Next(1, 5);
           for (int i = 0; i < componentCount; i++)
           {
               SUITComponents component = new SUITComponents();
               component.InitializeRandomData();
               Console.WriteLine(Components);
               Components.Add(component);
           }

           
           Dependencies.Clear(); 
           int dependencyCount = random.Next(1, 5); 
           for (int i = 0; i < dependencyCount; i++)
           {
               SUITDependencies dependency = new SUITDependencies();
               dependency.InitializeRandomData(2);
               Console.WriteLine(dependency);

               Dependencies.Add(dependency);
           }
       }

         
       public void FromSUIT(List<object> suitList)
       {
           if (suitList == null || suitList.Count == 0)
           {
               throw new ArgumentException("Invalid input data for SUITManifest.");
           }

           if (suitList[0] is CBORObject cborManifestVersion && cborManifestVersion.Type == CBORType.Integer)
           {
               ManifestVersion = (int)cborManifestVersion.AsInt32();
           }
           else
           {
               throw new ArgumentException("Invalid format for manifest version in SUITManifest.");
           }

           if (suitList[1] is CBORObject cborManifestSequenceNumber && cborManifestSequenceNumber.Type == CBORType.Integer)
           {
               ManifestSequenceNumber = (int)cborManifestSequenceNumber.AsInt32();
           }
           else
           {
               throw new ArgumentException("Invalid format for manifest sequence number in SUITManifest.");
           }

           if (suitList[2] is CBORObject cborCommon && cborCommon.Type == CBORType.Map)
           {
               var commonDict = cborCommon.ConvertToDictionary();
               var common = new SUITCommon();
               common.FromSUITDict(commonDict);
               Common = common;
           }
           else
           {
               throw new ArgumentException("Invalid format for common data in SUITManifest.");
           }

           if (suitList.Count > 3 && suitList[3] is List<object> componentsList)
           {
               Components = componentsList
                   .Select(item => new SUITComponents().FromSUIT(CBORObject.FromObject(item as Dictionary<object, object>)))
                   .ToList();
           }

           if (suitList.Count > 4 && suitList[4] is List<object> dependenciesList)
           {
               Dependencies = dependenciesList
                   .Select(item => new SUITDependencies().FromSUIT(CBORObject.FromObject(item as Dictionary<object, object>)))
                   .ToList();
           }
       }




         

    }
 

        public class SUITManifestBuilder
        {
            private SUITManifest manifest;

            public SUITManifestBuilder()
            {
                manifest = new SUITManifest();
            }

            public SUITManifestBuilder SetManifestVersion(int version)
            {
                manifest.ManifestVersion = version;
                return this;
            }

            public SUITManifestBuilder SetManifestSequenceNumber(int sequenceNumber)
            {
                manifest.ManifestSequenceNumber = sequenceNumber;
                return this;
            }

            public SUITManifestBuilder SetCommon(SUITCommon common)
            {
                manifest.Common = common;
                return this;
            }

            public SUITManifestBuilder AddComponent(SUITComponents component)
            {
                if (manifest.Components == null)
                {
                    manifest.Components = new List<SUITComponents>();
                }
                manifest.Components.Add(component);
                return this;
            }

            public SUITManifestBuilder AddDependency(SUITDependencies dependency)
            {
                if (manifest.Dependencies == null)
                {
                    manifest.Dependencies = new List<SUITDependencies>();
                }
                manifest.Dependencies.Add(dependency);
                return this;
            }

            public SUITManifest Build()
            {
                return manifest;
            }

            public string ToJson()
            {
                return manifest.ToJson();
            }

            public static SUITManifestBuilder FromJson(string json)
            {
                return new SUITManifestBuilder().SetManifest(SUITManifest.FromJson(json));
            }

            public SUITManifestBuilder SetManifest(SUITManifest existingManifest)
            {
                manifest = existingManifest;
                return this;
            }
        }
   

}
