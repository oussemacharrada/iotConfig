using System;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITSequence : ISUITConvertible<SUITSequence>
    {
        public List<SUITCommand> Items { get; set; }

        public SUITSequence()
        {
            Items = new List<SUITCommand>();
        }

        public SUITSequence FromSUIT(Dictionary<object, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (!suitDict.TryGetValue("items", out var itemsObj) || !(itemsObj is List<object> suitList))
            {
                throw new ArgumentException("Invalid or missing 'items' in the SUIT dictionary.");
            }

            FromSUIT(suitList);
            return this;
        }

        public SUITSequence FromJson(Dictionary<string, object> jsonData)
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
                var cmd = new SUITCommand().FromJson(itemData);
                Items.Add(cmd);
            }

            return this;
        }

        public dynamic ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var item in Items)
            {
                var cmdItem = item as SUITCommandContainer.SUITCmd;
                if (cmdItem != null)
                {
                    suitList.AddRange(cmdItem.ToSUIT(cmdItem.SuitKey)); // Use the SuitKey property here
                }
            }
            return new Dictionary<string, object>
            {
                { "items", suitList }
            };
        }


        public dynamic ToJson()
        {
            var jsonList = new List<object>();
            foreach (var item in Items)
            {
                if (item != null)
                {
                    jsonList.Add(item.ToJson());
                }
                else
                {
                 throw new (" i told you  item is null exception ") ;
                }
            }

            return new Dictionary<string, object>
            {
                { "items", jsonList }
            };
        }


        public string ToDebug(string indent)
        {
            return string.Join(Environment.NewLine, Items.Select(item => item.ToJson().ToString()));
        }

        private void FromSUIT(List<object> suitList)
        {
            if (suitList == null)
            {
                throw new ArgumentNullException(nameof(suitList));
            }

            Items.Clear();
            for (int i = 0; i < suitList.Count; i += 2)
            {
                if (suitList[i] is string suitKey && suitList[i + 1] is Dictionary<string, object> commandData)
                {
                    var cmd = new SUITCommand().FromSUIT(new List<object> { suitKey, commandData });
                    Items.Add(cmd);
                }
                else
                {
                    throw new ArgumentException("Invalid object type within the list.");
                }
            }
        }
    }
}
