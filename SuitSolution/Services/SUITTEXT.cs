using System.Collections.Generic;
using System.Text.Json;
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
        public void InitializeRandomData()
        {
            Languages = new List<string>
            {
                "en", "fr", "es" 
            };

            Texts = new List<string>
            {
                "Sample text 1", "Sample text 2", "Sample text 3" 
            };
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

            if (suitList[0] is CBORObject languagesObject)
            {
                Languages = new List<string> { languagesObject.ToJSONString() }; 
            }
            else
            {
                throw new ArgumentException("Invalid format for 'languages' in SUITText.");
            }

            if (suitList[1] is CBORObject textsObject)
            {
                Texts = new List<string> { textsObject.ToJSONString() };
            }
            else
            {
                throw new ArgumentException("Invalid format for 'texts' in SUITText.");
            }
        }


    }
    public class SUITTextBuilder
    {
        private SUITText suitText;

        public SUITTextBuilder()
        {
            suitText = new SUITText();
        }

        public SUITTextBuilder SetLanguages(List<string> languages)
        {
            suitText.Languages = languages ?? throw new ArgumentNullException(nameof(languages));
            return this;
        }

        public SUITTextBuilder SetTexts(List<string> texts)
        {
            suitText.Texts = texts ?? throw new ArgumentNullException(nameof(texts));
            return this;
        }

        public SUITTextBuilder InitializeRandomData()
        {
            suitText.InitializeRandomData();
            return this;
        }

        public SUITText Build()
        {
            return suitText;
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(suitText, options);
        }

        public static SUITTextBuilder FromJson(string json)
        {
            return new SUITTextBuilder().SetSUITText(JsonSerializer.Deserialize<SUITText>(json));
        }

        public SUITTextBuilder SetSUITText(SUITText existingSUITText)
        {
            suitText = existingSUITText ?? throw new ArgumentNullException(nameof(existingSUITText));
            return this;
        }
    }
}