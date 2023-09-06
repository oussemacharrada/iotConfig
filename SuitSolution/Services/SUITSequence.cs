using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITSequence : ISUITConvertible
    {
        [JsonPropertyName("seq")]
        public List<SUITComponentId> Sequence { get; set; }

        public SUITSequence()
        {
            Sequence = new List<SUITComponentId>();
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITSequence FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITSequence>(json);
        }
        public List<object> ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var componentId in Sequence)
            {
                suitList.Add(componentId.ToSUIT());
            }

            return suitList;
        }

        public CBORObject ToCBOR()
        {
            var cborArray = CBORObject.NewArray();
            foreach (var componentId in Sequence)
            {
                cborArray.Add(componentId.ToSUIT()); 
            }

            return cborArray;
        }

        public void FromSUIT(List<Object> suitList)
        {
            Sequence.Clear();
            foreach (var suitObject in suitList)
            {
                if (suitObject is SUITComponentId componentId)
                {
                    Sequence.Add(componentId);
                }
                else
                {
                    throw new ArgumentException("Invalid object type in the list.");
                }
            }
        }

        public void FromCBOR(CBORObject cborArray)
        {
            Sequence.Clear();
            foreach (var item in cborArray.Values)
            {
                if (item.Type == CBORType.Array)
                {
                    var componentId = SUITComponentId.FromSUIT(item);
                    Sequence.Add(componentId);
                }
                else
                {
                    throw new ArgumentException("Invalid CBOR type in the array.");
                }
            }
        }
    }
}
