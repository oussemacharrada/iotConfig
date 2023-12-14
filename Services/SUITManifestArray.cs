using System;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Interfaces;

public class SUITManifestArray<T> where T : ISUITConvertible<T>, new()
{
    private const string OneIndent = "    ";
    public List<T> Items { get; set; }

    public SUITManifestArray()
    {
        Items = new List<T>();
    }

    public bool Equals(SUITManifestArray<T> rhs)
    {
        return GetType().Equals(rhs.GetType()) &&
               Items.Count == rhs.Items.Count &&
               !Items.Where((t, i) => !t.Equals(rhs.Items[i])).Any();
    }

    public SUITManifestArray<T> FromJson(List<object> data)
    {
        Items = data.Select(d => 
        {
            var item = new T();
            item.FromJson((Dictionary<string, object>)d);
            return item;
        }).ToList();
    
        return this;
    }

    public dynamic ToJson()
    {
        return Items.Select(item => item.ToJson()).ToList();
    }

    public SUITManifestArray<T> FromSUIT(List<object> data)
    {
        TreeBranch.Append(typeof(SUITManifestArray<T>).FullName);
        Items = data.Select(d => 
        {
            TreeBranch.Append(Items.Count.ToString());
            var item = new T();
            item.FromSUIT((Dictionary<object, object>)d);
            TreeBranch.Pop();
            return item;
        }).ToList();
        TreeBranch.Pop();

        return this;
    }

    public List<object> ToSuit()
    {
        return Items.Select(item => item.ToSUIT()).ToList();
    }

    public void Append(T element)
    {
        Items.Add(element);
    }

    public string ToDebug(string indent)
    {
        var newIndent = indent + OneIndent;
        var debugList = Items.Select(item => $"{newIndent}{item.ToDebug(newIndent)}").ToList();
        return $"[\n{string.Join(",\n", debugList)}\n{indent}]";
    }
}