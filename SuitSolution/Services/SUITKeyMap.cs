using System;
using System.Collections.Generic;
using System.Text.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITKeyMap<T>
    {
        public readonly Dictionary<string, T> keymap;
        private readonly Dictionary<T, string> rkeymap;
        private T v;

        public SUITKeyMap(Dictionary<string, T> keymap)
        {
            this.keymap = keymap;
            rkeymap = new Dictionary<T, string>();
            foreach (var entry in keymap)
            {
                rkeymap[entry.Value] = entry.Key;
            }
        }

        protected SUITKeyMap()
        {
        }

        public string ToJson()
        {
            if (rkeymap.TryGetValue(v, out string key))
            {
                return JsonSerializer.Serialize(key, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new ArgumentException("Unknown value.");
            }
        }

        public SUITKeyMap<T> FromSUIT(string d)
        {
            if (keymap.TryGetValue(d, out T value))
            {
                v = value;
            }
            else
            {
                throw new ArgumentException("Unknown key.");
            }

            return this;
        }

        public T ToSUIT()
        {
            return v;
        }

        public string ToDebug()
        {
            var s = v.ToString() + " / " + ToJson() + " /";
            return s;
        }
    }
}