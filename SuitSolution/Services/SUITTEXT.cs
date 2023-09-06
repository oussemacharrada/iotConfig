using System.Collections.Generic;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITText : ISUITConvertible
    {
        [JsonPropertyName("languages")]
        public List<string> Languages { get; set; }

        [JsonPropertyName("texts")]
        public List<string> Texts { get; set; }

        public SUITText()
        {
            Languages = new List<string>();
            Texts = new List<string>();
        }

        public CBORObject ToCBOR()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("languages", Languages);
            cborObject.Add("texts", Texts);
            return cborObject;
        }

        public void FromCBOR(CBORObject cborObject)
        {
            Languages = cborObject["languages"].Values.Select(l => l.AsString()).ToList();
            Texts = cborObject["texts"].Values.Select(t => t.AsString()).ToList();
        }

        public List<object> ToSUIT()
        {
            var suitList = new List<object>
            {
                Languages,
                Texts
            };

            return suitList;
        }

        public void FromSUIT(List<object> suitList)
        {
            if (suitList == null || suitList.Count < 2)
            {
                throw new ArgumentException("Invalid SUIT list.");
            }

            Languages = (List<string>)suitList[0];
            Texts = (List<string>)suitList[1];
        }
    }
}