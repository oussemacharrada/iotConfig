using System.Collections.Generic;
using System.Text;
using SuitSolution.Services;

public class SUITComponents : SUITManifestArray<SUITComponentId>
{
    public SUITComponents() { }

    public dynamic ToSUIT()
    {
        var suitData = base.ToSuit();
        return suitData;
    }
  public void AddComponent(dynamic componentId)
    {
        // Check if componentId is a SUITComponentId
        if (componentId is SUITComponentId suitComponentId)
        {
            foreach (var bytes in suitComponentId.componentIds)
            {
                var newComponentId = new SUITComponentId();
                newComponentId.componentIds.Add(bytes);
                Items.Add(newComponentId);
            }
        }
        // Check if componentId is a Dictionary<string, object>
        else if (componentId is Dictionary<string, object> dict && dict.ContainsKey("componentIds"))
        {
            var componentIdList = dict["componentIds"] as List<object>;
            if (componentIdList != null)
            {
                foreach (var item in componentIdList)
                {
                    if (item is Dictionary<string, object> itemDict && itemDict.ContainsKey("hex"))
                    {
                        var hexValue = itemDict["hex"] as string;
                        if (!string.IsNullOrEmpty(hexValue))
                        {
                            var bytes = ConvertHexStringToByteArray(hexValue);
                            var newComponentId = new SUITComponentId();
                            newComponentId.componentIds.Add(new SUITBytes { v = bytes });
                            Items.Add(newComponentId);
                        }
                    }
                }
            }
        }
        else
        {
            throw new ArgumentException("Unsupported type for componentId.");
        }
    }

    private byte[] ConvertHexStringToByteArray(string hexString)
    {
        if (hexString.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even length");

        byte[] bytes = new byte[hexString.Length / 2];
        for (int i = 0; i < hexString.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return bytes;
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