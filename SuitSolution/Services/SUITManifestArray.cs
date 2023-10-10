using System;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Interfaces;

public class SUITManifestArray<T> where T : new()
{
    public readonly string one_indent = "    ";

    public List<T> items;

    public SUITManifestArray()
    {
        items = new List<T>();
    }

    public bool Equals(SUITManifestArray<T> rhs)
    {
        if (!GetType().Equals(rhs.GetType()))
            return false;

        if (items.Count != rhs.items.Count)
            return false;

        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].Equals(rhs.items[i]))
                return false;
        }

        return true;
    }

    public SUITManifestArray<T> FromJson(List<object> data)
    {
        items = new List<T>();

        foreach (var d in data)
        {
            var dict = d as Dictionary<string, object>;
            if (dict != null)
            {
                var item = new T();
                (item as ISUITConvertible<T>)?.FromJson(dict);
                items.Add(item);
            }
        }

        return this;
    }

    public List<object> ToJson()
    {
        var jsonList = new List<object>();

        foreach (var item in items)
        {
            var suitConvertible = item as ISUITConvertible<T>;
            if (suitConvertible != null)
            {
                jsonList.Add(suitConvertible.ToJson());
            }
        }

        return jsonList;
    }

    public SUITManifestArray<T> FromSUIT(List<object> data)
    {
        items = new List<T>();

        foreach (var d in data)
        {
            var dict = d as Dictionary<string, object>;
            if (dict != null)
            {
                var item = new T();
                (item as ISUITConvertible<T>)?.FromSUIT(dict);
                items.Add(item);
            }
        }

        return this;
    }

    public List<Dictionary<string, object>> ToSuit()
    {
        var suitList = new List<Dictionary<string, object>>();

        foreach (var item in items)
        {
            var suitConvertible = item as ISUITConvertible<T>;
            if (suitConvertible != null)
            {
                suitList.Add(suitConvertible.ToSUIT());
            }
        }

        return suitList;
    }


    public void append(T element)
    {
        items.Add(element);
    }

    public string to_debug(string indent)
    {
        var newIndent = indent + one_indent;
        var debugList = items.Select(item => $"{newIndent}{(item as ISUITConvertible<T>)?.ToDebug(newIndent)}").ToList();
        var s = $"[\n{string.Join(",\n", debugList)}\n{indent}]";
        return s;
    }
}
