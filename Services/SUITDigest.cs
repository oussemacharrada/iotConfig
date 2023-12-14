using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITDigest : SuitManifestNamedList, ISUITConvertible<SUITDigest>
    {
        public SUITDigestAlgo AlgorithmId { get; set; }
        public SUITBytes DigestBytes { get; set; }

        public SUITDigest()
        {
            fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
                ("algorithm-id", 0, () => AlgorithmId,"AlgorithmId"),
                ("digest-bytes", 1, () =>DigestBytes,"DigestBytes")
            ));
        }

        public new dynamic ToSUIT()
        {
            var suitDict = new Dictionary<int, object>();

            if (AlgorithmId != null) // Assuming IsEmpty() is a method to check if the object is in its default state
                suitDict.Add(0, AlgorithmId.ToSUIT());

            if (DigestBytes != null)
                suitDict.Add(    1, DigestBytes.ToSUIT());
            return suitDict;
        }



        public SUITDigest FromSUIT(Dictionary<object, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.JsonKey;
                if (suitDict.TryGetValue(fieldInfo.SuitKey, out var suitValue))
                {
                    var property = GetType().GetProperty(fieldName);
                    if (property != null && suitValue != null)
                    {
                        var fieldInstance = (ISUITConvertible<object>)fieldInfo.ObjectFactory.Invoke();
                        fieldInstance.FromSUIT(suitValue as Dictionary<object, object>);
                        property.SetValue(this, fieldInstance);
                    }
                }
            }

            return this;
        }

        public SUITDigest FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData));
            }

            // Handle 'algorithm-id'
            if (jsonData.TryGetValue("algorithm-id", out var algorithmIdValue) && algorithmIdValue is string algorithmIdString)
            {
                // Assuming SUITDigestAlgo can be constructed from a string
                SUITDigestAlgo algo = new SUITDigestAlgo();
                algo.SetAlgorithmValue(algo.KeyMap[algorithmIdString]);
                this.AlgorithmId = algo;
            }

            // Handle 'digest-bytes'
            if (jsonData.TryGetValue("digest-bytes", out var digestBytesValue) && digestBytesValue is byte[] digestBytesArray)
            {
                // Directly use the byte array
                SUITBytes bytes = new SUITBytes(digestBytesArray);
                this.DigestBytes = bytes;
            }

            return this;
        }


        public dynamic ToJson()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.JsonKey;
                var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
                if (fieldValue != null)
                {
                    hash = hash * 23 + fieldValue.GetHashCode();
                }
            }
            return hash;
        }
    }
}
