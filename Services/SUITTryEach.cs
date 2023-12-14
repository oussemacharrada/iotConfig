using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITTryEach : SUITManifestArray<SUITTryEach>, ISUITConvertible<SUITTryEach>
    {
        private const string Indentation = "  ";
        public List<SUITSequence> Items { get; set; } = new List<SUITSequence>();

        private void LoadItemsFromDict<T>(IEnumerable<T> itemList, Func<T, SUITSequence> converter)
        {
            Items.Clear();
            foreach (var item in itemList)
            {
                if (item is T itemDict)
                {
                    Items.Add(converter(itemDict));
                }
                else
                {
                    throw new ArgumentException($"Expected item of type {typeof(T)}, but found {item.GetType()}");
                }
            }
        }

        public SUITTryEach FromSUIT(Dictionary<object, object> suitDict)
        {
         return this;
        }

        public SUITTryEach FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
                throw new ArgumentNullException(nameof(jsonData));

            if (!jsonData.TryGetValue("items", out var itemsObj) || !(itemsObj is List<object> jsonList))
                throw new ArgumentException("Invalid or missing 'items' in the JSON data.");

            LoadItemsFromDict(jsonList, itemDict =>
            {
                if (itemDict is Dictionary<string, object> dict)
                    return new SUITSequence().FromJson(dict);
                else
                    throw new ArgumentException("Item format is not valid. Expected a Dictionary<string, object>.");
            });

            return this;
        }


        public dynamic ToSUIT()
        {
            var suitList = CBORObject.NewArray();

            // Add the identifier for SUITTryEach, assuming it's 15

            // Process each SUITSequence in Items
            var sequencesList = new List<object>();
            foreach (var sequence in Items)
            {
                var sequenceData = sequence.ToSUIT(); // Assuming SUITSequence has a ToSUIT method
                sequencesList.AddRange(sequenceData);
            }

            // Add the sequences list after the identifier
            suitList.Add(CBORObject.FromObject(sequencesList).EncodeToBytes());

            return suitList;
        }


        public Dictionary<string, object> ToJson()
        {
            var jsonList = Items.Select(item => item.ToJson()).ToList();
            return new Dictionary<string, object> { { "items", jsonList } };
        }

        public string ToDebug(string indent)
        {
            var debugLines = Items.Select(item => item.ToDebug(indent + Indentation));
            return $"{indent}SUITTryEach:\n{string.Join("\n", debugLines)}";
        }
    }
}
