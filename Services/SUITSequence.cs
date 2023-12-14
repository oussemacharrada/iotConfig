using System;
using System.Collections.Generic;
using System.Linq;
using PeterO.Cbor;
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
            if (suitDict == null) throw new ArgumentNullException(nameof(suitDict));

            if (!suitDict.TryGetValue("items", out var itemsObj) || !(itemsObj is List<object> suitList))
                throw new ArgumentException("Invalid or missing 'items' in the SUIT dictionary.");

            return FromSUIT(suitList);
        }

        public SUITSequence FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null) throw new ArgumentNullException(nameof(jsonData));

            if (!jsonData.TryGetValue("items", out var itemsObj) || !(itemsObj is List<Dictionary<string, object>> jsonList))
                throw new ArgumentException("Invalid or missing 'items' in the JSON data.");

            Items = jsonList.Select(itemData => new SUITCommand().FromJson(itemData)).ToList();
            return this;
        }

        public int GetComponentIndex(object componentId)
        {
            if (componentId == null)
            {
                Console.WriteLine("componentId parameter is null.");
                return -1;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is SUITCommandContainer.SUITCmd cmdItem && cmdItem.cid != null && cmdItem.cid.Equals(componentId))
                {
                    return i;
                }
            }

            return -1;
        }

        public dynamic ToSUIT()
        {
            var suitList = new List<object>();
            int? currentIndex = null;

            foreach (var item in Items)
            {
                if (!(item is SUITCommandContainer.SUITCmd cmdItem))
                    throw new InvalidOperationException("Invalid command item in the sequence.");

                // Handle index setting commands
                currentIndex = HandleIndexSettingCommands(cmdItem, currentIndex, suitList);

                // Add command to the list
                var cmdItemSUIT = cmdItem.ToSUIT();
                if (cmdItemSUIT != null && cmdItemSUIT.Length > 0)
                {
                    suitList.AddRange(cmdItemSUIT);
                }
            }

            return suitList.ToArray();
        }

        private int? HandleIndexSettingCommands(SUITCommandContainer.SUITCmd cmdItem, int? currentIndex, List<object> suitList)
        {
            if (cmdItem.JsonKey == "directive-set-component-index")
            {
                return GetComponentIndex(cmdItem.argInstance);
            }
            else if (cmdItem.JsonKey == "directive-set-dependency-index")
            {
                return SUITCommonInfo.GetDependencyIndex(cmdItem.argInstance);
            }
            else
            {
                int cidx = GetComponentIndex(cmdItem.cid);
                if (cidx != currentIndex)
                {
                    string op = cidx is DependencyIndex ? "directive-set-dependency-index" : "directive-set-component-index";
                    currentIndex = cidx;

                    // Create a new command for changing the index
                    var indexCommand = new SUITCommand().FromJson(new Dictionary<string, object>
                    {
                        { "command-id", op },
                        { "command-arg", cidx }
                    });

                    suitList.AddRange(indexCommand.ToSUIT(cmdItem.SuitKey));
                }
            }

            return currentIndex;
        }

        public dynamic ToJson()
        {
            var jsonList = Items.Select(item => item?.ToJson() ?? throw new InvalidOperationException("Item is null.")).ToList();
            return new Dictionary<string, object> { { "items", jsonList } };
        }

        public string ToDebug(string indent)
        {
            return string.Join(Environment.NewLine, Items.Select(item => item.ToJson().ToString()));
        }

        public SUITSequence FromSUIT(List<object> suitList)
        {
            if (suitList == null) throw new ArgumentNullException(nameof(suitList));

            Items = suitList.Select((suitData, i) =>
            {
                if (!(suitData is List<object> suitDataList))
                    throw new ArgumentException($"Invalid SUIT data at index {i}.");

                return new SUITCommand().FromSUIT(suitDataList);
            }).ToList();

            return this;
        }
    }
}
