using System.Collections.Generic;
using SuitSolution.Services;

public class SUITComponents : SUITManifestArray<SUITComponentId>
{
    public SUITComponents() { }

    public Dictionary<string, object> ToSUIT()
    {
        var suitData = new Dictionary<string, object>();
        var suitList = new List<object>();

        foreach (var item in Items)
        {
            suitList.Add(item.ToSUIT());
        }

        suitData["components"] = suitList;
        return suitData;
    }

    public new SUITComponents FromSUIT(List<object> data)
    {
        base.FromSUIT(data);
        UpdateComponentIds();
        return this;
    }

    public new SUITComponents FromJson(List<object> jsonData)
    {
        base.FromJson(jsonData);
        UpdateComponentIds();
        return this;
    }

    private void UpdateComponentIds()
    {
        SUITCommonInfo.ComponentIds = Items;
    }
}