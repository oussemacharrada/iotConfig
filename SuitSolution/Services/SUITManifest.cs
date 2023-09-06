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
         
       public void FromSUIT(List<Object> suitList)
       {
           if (suitList == null || suitList.Count < 5)
           {
               throw new ArgumentException("Invalid SUIT List");
           }

           ManifestVersion = Convert.ToInt32(suitList[0]);
           ManifestSequenceNumber = Convert.ToInt32(suitList[1]);
           Common = new SUITCommon();

           if (suitList[2] is List<object> commonList)
           {
               Common.FromSUIT(commonList);
           }
           else
           {
            
               throw new ArgumentException("Invalid format for 'common' in SUIT List.");
           }

           Components = ((List<object>)suitList[3]).Select(component =>
               new SUITComponents().FromSUIT(CBORObject.FromObject(component))).ToList();

           Dependencies = ((List<object>)suitList[4]).Select(dependency =>
               new SUITDependencies().FromSUIT(CBORObject.FromObject(dependency))).ToList();
       }


         

    }
}
