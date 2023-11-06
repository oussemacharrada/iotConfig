using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;
using SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITTryEach : SUITManifestArray<SUITTryEach>, ISUITConvertible<SUITTryEach>
    {
        public List<SUITSequence> Items { get; set; } = new List<SUITSequence>();

        public SUITTryEach()
        {
        }

        public SUITTryEach FromSUIT(Dictionary<object, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (!suitDict.TryGetValue("items", out var itemsObj) || !(itemsObj is List<object> suitList))
            {
                throw new ArgumentException("Invalid or missing 'items' in the SUIT dictionary.");
            }

            Items.Clear();
            foreach (var item in suitList)
            {
                Items.Add(new SUITSequence().FromSUIT(item as Dictionary<object, object>));
            }

            return this;
        }

        public SUITTryEach FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData));
            }

            if (!jsonData.TryGetValue("items", out var itemsObj) || !(itemsObj is List<Dictionary<string, object>> jsonList))
            {
                throw new ArgumentException("Invalid or missing 'items' in the JSON data.");
            }

            Items.Clear();
            foreach (var itemData in jsonList)
            {
                Items.Add(new SUITSequence().FromJson(itemData));
            }

            return this;
        }

        public dynamic ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var item in Items)
            {
                suitList.Add(item.ToSUIT());
            }
            return new Dictionary<string, object>
            {
                { "items", suitList }
            };
        }

        public Dictionary<string, object> ToJson()
        {
            var jsonList = new List<object>();
            foreach (var item in Items)
            {
                jsonList.Add(item.ToJson());
            }

            return new Dictionary<string, object>
            {
                { "items", jsonList }
            };
        }

        public string ToDebug(string indent)
        {
            return string.Join(Environment.NewLine, Items.Select(item => item.ToDebug(indent)));
        }
    }
}

