using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
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
                ("algorithm-id", 0, () => AlgorithmId ??= new SUITDigestAlgo(),"AlgorithmId"),
                ("digest-bytes", 1, () => DigestBytes ??= new SUITBytes(),"DigestBytes")
            ));
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

            foreach (var fieldInfo in fields.Values)
            {
                var fieldName = fieldInfo.JsonKey;
                if (jsonData.TryGetValue(fieldName, out var jsonValue))
                {
                    var property = GetType().GetProperty(fieldName);
                    if (property != null && jsonValue != null)
                    {
                        var fieldInstance = (ISUITConvertible<object>)fieldInfo.ObjectFactory.Invoke();
                        fieldInstance.FromJson(jsonValue as Dictionary<string, object>);
                        property.SetValue(this, fieldInstance);
                    }
                }
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
