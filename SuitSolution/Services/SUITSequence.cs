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
        public void InitializeRandomData()
        {
            Random random = new Random();

            int itemCount = random.Next(1, 10);

            Sequence.Clear(); 

            for (int i = 0; i < itemCount; i++)
            {
                SUITComponentId componentId = new SUITComponentId();
                componentId.InitializeRandomData();
                
             
                Sequence.Add(componentId);
            }
        }

        private string GenerateRandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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

            if (suitList == null)
            {
                throw new ArgumentNullException(nameof(suitList));
            }

            foreach (var suitObject in suitList)
            {
                if (suitObject is CBORObject cborObject && cborObject.Type == CBORType.Map)
                {
                    var id = cborObject["Id"].AsString();
                    var classId = cborObject["class-id"].AsString();
                    var vendorId = cborObject["vendor-id"].AsString();
                    var imageDigest = cborObject["image-digest"].AsString();

                    var componentId = new SUITComponentId
                    {
                        Id = id,
                        ClassId = classId,
                        VendorId = vendorId,
                        ImageDigest = imageDigest
                    };

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
    
    
    
    public class SUITSequenceBuilder
    {
        private SUITSequence sequence;

        public SUITSequenceBuilder()
        {
            sequence = new SUITSequence();
        }

        public SUITSequenceBuilder AddComponentId(SUITComponentId componentId)
        {
            sequence.Sequence.Add(componentId);
            return this;
        }

        public SUITSequenceBuilder InitializeRandomData()
        {
            sequence.InitializeRandomData();
            return this;
        }

        public SUITSequence Build()
        {
            return sequence;
        }

        public string ToJson()
        {
            return sequence.ToJson();
        }

        public static SUITSequenceBuilder FromJson(string json)
        {
            return new SUITSequenceBuilder().SetSequence(SUITSequence.FromJson(json));
        }

        public SUITSequenceBuilder SetSequence(SUITSequence existingSequence)
        {
            sequence = existingSequence;
            return this;
        }
    }
}
