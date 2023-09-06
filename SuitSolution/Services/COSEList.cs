using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class COSEList : ISUITConvertible
    {
        [JsonIgnore]
        public CBORObject[] ListData { get; set; }

        public COSEList()
        { 
            ListData = Array.Empty<CBORObject>();
        }

        public string ToJson()
        {
            var jsonList = new List<string>();

            foreach (var cborObject in ListData)
            {
                jsonList.Add(cborObject.ToJSONString());
            }

            return JsonSerializer.Serialize(jsonList);
        }


        public COSEList FromJson(string json)
        {
            List<CBORObject> listData = CBORObject.FromJSONString(json).Values.ToList();
            ListData = listData.ToArray();
            return this;
        }







        public List<object> ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var data in ListData)
            {
                if (data.Type == CBORType.TextString)
                {
                    suitList.Add(data.AsString());
                }
                else if (data.Type == CBORType.Integer) 
                {
                    suitList.Add((int)data.AsInt64()); 
                }
                else if (data.Type == CBORType.Boolean)
                {
                    suitList.Add(data.AsBoolean());
                }
                else if (data.IsNull)
                {
                    suitList.Add(null);
                }
            }
            return suitList;
        }

      
        public void FromSUIT(List<object> suitList)
        {
            var cborList = new List<CBORObject>();
            foreach (var item in suitList)
            {
                if (item is string str)
                {
                    cborList.Add(CBORObject.FromObject(str));
                }
                else if (item is int intValue)
                {
                    cborList.Add(CBORObject.FromObject(intValue));
                }
                else if (item is double doubleValue)
                {
                    cborList.Add(CBORObject.FromObject(doubleValue));
                }
                else if (item is bool boolValue)
                {
                    cborList.Add(CBORObject.FromObject(boolValue));
                }
                else if (item == null)
                {
                    cborList.Add(CBORObject.Null);
                }
            }
            ListData = cborList.ToArray();
        }
    }
}
