using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITCommon : ISUITConvertible
    {
        public int Sequence { get; set; }
        public List<int> Install { get; set; }
        public List<int> Validate { get; set; }

        public CBORObject ToCbor()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("sequence", Sequence);

            if (Install != null)
            {
                var installArray = CBORObject.NewArray();
                foreach (var item in Install)
                {
                    installArray.Add(item);
                }
                cborObject.Add("install", installArray);
            }

            if (Validate != null)
            {
                var validateArray = CBORObject.NewArray();
                foreach (var item in Validate)
                {
                    validateArray.Add(item);
                }
                cborObject.Add("validate", validateArray);
            }

            return cborObject;
        }
        public List<object> ToSUIT()
        {
            var suitList = new List<object>
            {
                Sequence
            };

            if (Install != null)
            {
                var installList = Install.Cast<object>().ToList();
                suitList.Add(installList);
            }
            else
            {
                suitList.Add(null);
            }

            if (Validate != null)
            {
                var validateList = Validate.Cast<object>().ToList();
                suitList.Add(validateList);
            }
            else
            {
                suitList.Add(null);
            }

            return suitList;
        }

        public void FromSUIT(List<Object> suitList)
        {
            if (suitList == null || suitList.Count < 1)
            {
                throw new ArgumentException("Invalid SUIT list.");
            }

            Sequence = (int)suitList[0];

            if (suitList.Count > 1 && suitList[1] != null)
            {
                Install = ((List<object>)suitList[1]).Cast<int>().ToList();
            }
            else
            {
                Install = null;
            }

            if (suitList.Count > 2 && suitList[2] != null)
            {
                Validate = ((List<object>)suitList[2]).Cast<int>().ToList();
            }
            else
            {
                Validate = null;
            }
        }


        public void FromCBOR(CBORObject cborObject)
        {
            if (cborObject == null || cborObject.Type != CBORType.Map)
            {
                throw new ArgumentException("Invalid CBOR object or type.");
            }

            Sequence = cborObject["sequence"].AsInt32();

            if (cborObject.ContainsKey("install"))
            {
                Install = new List<int>();
                foreach (var item in cborObject["install"].Values)
                {
                    Install.Add(item.AsInt32());
                }
            }
            else
            {
                Install = null;
            }

            if (cborObject.ContainsKey("validate"))
            {
                Validate = new List<int>();
                foreach (var item in cborObject["validate"].Values)
                {
                    Validate.Add(item.AsInt32());
                }
            }
            else
            {
                Validate = null;
            }
        }
        public Dictionary<object, object> ToSUITDict()
        {
            var suitDict = new Dictionary<object, object>
            {
                { "sequence", Sequence }
            };

            if (Install != null)
            {
                suitDict.Add("install", Install);
            }
            else
            {
                suitDict.Add("install", null);
            }

            if (Validate != null)
            {
                suitDict.Add("validate", Validate);
            }
            else
            {
                suitDict.Add("validate", null);
            }

            return suitDict;
        }
        public void InitializeRandomData()
        {
            Random random = new Random();

            Sequence = random.Next(1, 101);

            int installCount = random.Next(0, 5); 
            Install = new List<int>();
            for (int i = 0; i < installCount; i++)
            {
                Install.Add(random.Next(1, 101)); 
            }

            int validateCount = random.Next(0, 5); 
            Validate = new List<int>();
            for (int i = 0; i < validateCount; i++)
            {
                Validate.Add(random.Next(1, 101));
            }
        }

        public void FromSUITDict(Dictionary<object, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (suitDict.TryGetValue("sequence", out var sequenceValue) && sequenceValue is int sequence)
            {
                Sequence = sequence;
            }

            if (suitDict.TryGetValue("install", out var installValue) && installValue is List<object> installList)
            {
                Install = installList.OfType<int>().ToList();
            }
            else
            {
                Install = null;
            }

            if (suitDict.TryGetValue("validate", out var validateValue) && validateValue is List<object> validateList)
            {
                Validate = validateList.OfType<int>().ToList();
            }
            else
            {
                Validate = null;
            }
        }
    }
}
