public class SUITManifestNamedList : SUITManifestDict
{
    public SUITManifestNamedList() : base()
    {
    }

    public new SUITManifestNamedList FromSUIT(Dictionary<string, object> data)
    {
        foreach (var kvp in fields)
        {
            var fieldName = kvp.Key;
            var fieldInfo = kvp.Value;

            if (data.ContainsKey(fieldInfo.suit_key))
            {
                var fieldValue = data[fieldInfo.suit_key];
                if (fieldValue != null)
                {
                    var fieldValueObject = (fieldInfo.ObjectFactory as dynamic).FromSUIT(fieldValue);
                    GetType().GetProperty(fieldName)?.SetValue(this, fieldValueObject);
                }
            }
        }

        return this;
    }

    public new List<object> ToSUIT()
    {
        int maxSuitKey = fields.Values.Max(fieldInfo => int.Parse(fieldInfo.suit_key));

        var suitList = new List<object>(maxSuitKey + 1);

        foreach (var kvp in fields)
        {
            var fieldInfo = kvp.Value;
            var fieldName = kvp.Key;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                while (suitList.Count <= int.Parse(fieldInfo.suit_key)) 
                {
                    suitList.Add(null);
                }

                suitList[int.Parse(fieldInfo.suit_key)] = (fieldValue as dynamic).ToSUIT();
            }
        }

        return suitList;
    }


    public new string ToDebug(string indent)
    {
        var newIndent = indent + one_indent;
        var items = new List<string>();

        foreach (var kvp in fields)
        {
            var fieldName = kvp.Key;
            var fieldInfo = kvp.Value;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                items.Add($"/ {fieldInfo.json_key} / {(fieldValue as dynamic).ToDebug(newIndent)}");
            }
        }

        return $"[\n{newIndent}{string.Join(",\n" + newIndent, items)}\n{indent}]";
    }
}
