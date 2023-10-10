using System;
using System.Collections.Generic;
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

        public void FromSUIT(List<object> suitList)
        {
            if (suitList == null)
            {
                throw new ArgumentNullException(nameof(suitList));
            }

            Items = new List<SUITCommand>();
            foreach (var item in suitList)
            {
                if (item is Dictionary<string, object> commandData)
                {
                    var cmd = new SUITCommand();
                    cmd.FromSUIT(commandData);
                    Items.Add(cmd);
                }
                else
                {
                    throw new ArgumentException("Invalid object type within the list.");
                }
            }
        }

        public SUITSequence FromSUIT(Dictionary<string, object> suitDict)
        {
            throw new NotImplementedException();
        }

        public SUITSequence FromJson(Dictionary<string, object> jsonData)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> ToSUIT()
        {
            var suitList = new List<object>();
            foreach (var item in Items)
            {
                suitList.Add(item.ToSUIT());
            }

            // Create a dictionary to represent the SUITSequence
            var suitDict = new Dictionary<string, object>
            {
                { "items", suitList }
            };

            return suitDict;
        }

        public Dictionary<string, object> ToJson()
        {
            var jsonList = new List<object>();
            foreach (var item in Items)
            {
                jsonList.Add(item.ToJson());
            }

            // Create a dictionary to represent the JSON output
            var jsonDict = new Dictionary<string, object>
            {
                { "items", jsonList }
            };

            return jsonDict;
        }

        public string ToDebug(string indent)
        {
            throw new NotImplementedException();
        }
    }
}
