using System;
using System.Collections.Generic;
using System.Text.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITKeyMap<T>
    {
        public readonly Dictionary<object, T> KeyMap;
        private readonly Dictionary<T, object> RKeyMap;
        private T v;

        public SUITKeyMap(Dictionary<object, T> keyMap)
        {
            KeyMap = keyMap;
            RKeyMap = new Dictionary<T, object>();
            foreach (var entry in KeyMap)
            {
                RKeyMap[entry.Value] = entry.Key;
            }
        }

        protected SUITKeyMap()
        {
        }

        public string ToJson()
        {
            if (RKeyMap.TryGetValue(v, out object key))
            {
                return JsonSerializer.Serialize(key, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new ArgumentException("Unknown value.");
            }
        }

        public SUITKeyMap<T> FromJson(string jsonData)
        {
            var deserializedKey = JsonSerializer.Deserialize<string>(jsonData);
            if (KeyMap.TryGetValue(deserializedKey, out T value))
            {
                v = value;
                return this;
            }
            else
            {
                throw new ArgumentException("Unknown key.");
            }
        }

        public T ToSUIT()
        {
            return v;
        }

        public SUITKeyMap<T> FromSUIT(string d)
        {
            TreeBranch.Append(typeof(SUITKeyMap<T>).FullName);
           if (KeyMap.TryGetValue(d, out T value))
            {
                v = value;
                TreeBranch.Pop();
                return this;
            }
            else
            {
                TreeBranch.Pop();
                throw new ArgumentException("Unknown key.");
            }
        }

        public string ToDebug()
        {
            return $"{v} / {ToJson()} /";
        }
    }
}
