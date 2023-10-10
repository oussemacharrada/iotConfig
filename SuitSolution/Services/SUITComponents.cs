using System.Collections.Generic;

public class SUITComponents : SUITManifestArray<SUITComponentId>
{
    public SUITComponents()
    {
    }

    // Implement the ToSUIT method with a return type of Dictionary<string, object>
    public Dictionary<string, object> ToSUIT()
    {
        // Create a dictionary to hold the SUIT data
        var suitData = new Dictionary<string, object>();

        // Convert the internal data of SUITComponents to the desired SUIT format
        var suitList = new List<object>();

        // Iterate over your SUIT data and add elements to the suitList
        foreach (var item in items)
        {
            // Convert each item to its SUIT representation and add it to the list
            // You may need to define a ToSUIT method for SUITComponentId if it's a custom type
            suitList.Add(item.ToSUIT());
        }

        // Add the suitList to the dictionary under the desired key
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
        SUITCommonInfo.ComponentIds = items;
    }
}