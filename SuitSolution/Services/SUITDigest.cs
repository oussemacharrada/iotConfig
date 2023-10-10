using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITDigest : SUITManifestNamedList, ISUITConvertible<SUITDigest>
    {
        public SUITDigest()
        {
            fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
                ("algo", "algorithm-id", () => algoField),
                ("digest", "digest-bytes", () => digestField)
            ));
        }

        public SUITBWrapField<SUITDigestAlgo> algoField { get; set; } = new SUITBWrapField<SUITDigestAlgo>();
        public SUITBytes digestField { get; set; } = new SUITBytes();

        private static Dictionary<string, ManifestKey> MkFields(params (string, string, Func<object>)[] fieldTuples)
        {
            var fieldDict = new Dictionary<string, ManifestKey>();

            for (int i = 0; i < fieldTuples.Length; i++)
            {
                var (fieldName, suitKey, objFunc) = fieldTuples[i];
                fieldDict[fieldName] = new ManifestKey(fieldName, suitKey, objFunc);
            }

            return fieldDict;
        }

        public SUITDigest FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (suitDict.TryGetValue("algo", out var algoValue) && algoValue is Dictionary<string, object> algoDict)
            {
                var algo = new SUITDigestAlgo();
                algo.FromSUIT(algoDict);
                algoField = new SUITBWrapField<SUITDigestAlgo> { v = algo };
            }

            if (suitDict.TryGetValue("digest", out var digestValue) && digestValue is byte[] digestBytes)
            {
                digestField = new SUITBytes { v = digestBytes };
            }

            return this;
        }

        public SUITDigest FromJson(Dictionary<string, object> jsonData)
        {
            throw new NotImplementedException();
        }
        public List<Dictionary<string, object>> ToSUIT()
        {
            var suitList = new List<Dictionary<string, object>>();
            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.json_key;
                var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

                if (fieldValue != null)
                {
                    var suitKey = fieldInfo.suit_key;
                    var suitValue = (fieldValue as dynamic).ToSUIT();
                    var suitDict = new Dictionary<string, object>
                    {
                        { suitKey, suitValue }
                    };
                    suitList.Add(suitDict);
                }
            }
            return suitList;
        }




        public Dictionary<string, object> ToJson()
        {
            var jsonDict = new Dictionary<string, object>();

            if (algoField != null && algoField.v != null)
            {
                jsonDict["algo"] = algoField.v.ToJson();
            }

            if (digestField != null && digestField.v != null)
            {
                jsonDict["digest"] = digestField.v;
            }

            return jsonDict;
        }
        public override int GetHashCode()
        {
            // Create a list of hash codes for the fields that exist
            var hashCodes = new List<int>();
            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.json_key;
                if (GetType().GetProperty(fieldName) != null)
                {
                    var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
                    if (fieldValue != null)
                    {
                        hashCodes.Add(fieldValue.GetHashCode());
                    }
                }
            }

            // Calculate the hash code based on the hash codes of the fields
            int hashCode = 17;
            foreach (var code in hashCodes)
            {
                hashCode = hashCode * 31 + code;
            }

            return hashCode;
        }

        public void FromSUIT(List<object> suitList)
        {
            foreach (var suitDict in suitList)
            {
                if (suitDict is Dictionary<string, object> dict)
                {
                    foreach (var fieldInfo in fields.Values)
                    {
                        var fieldName = fieldInfo.json_key;
                        var suitKey = fieldInfo.suit_key;

                        if (dict.ContainsKey(suitKey))
                        {
                            var suitValue = dict[suitKey];

                            if (fieldName == "algo")
                            {
                                if (suitValue is Dictionary<string, object> algoDict)
                                {
                                    var algo = new SUITDigestAlgo();
                                    algo.FromSUIT(algoDict);
                                    algoField = new SUITBWrapField<SUITDigestAlgo> { v = algo };
                                }
                            }
                            else if (fieldName == "digest")
                            {
                                if (suitValue is byte[] digestBytes)
                                {
                                    digestField = new SUITBytes { v = digestBytes };
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
