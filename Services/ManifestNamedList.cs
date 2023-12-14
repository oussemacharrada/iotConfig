namespace SuitSolution.Services;

public class SuitManifestNamedList : SUITManifestDict
{public new dynamic ToSUIT()
    {
        var maxIndex = fields.Values
            .Select(f =>
            {
                var result = f.SuitKey;
                return result;
            })
            .Max();
        var cborList = new List<object>(new object[maxIndex + 1]);

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
            if (fieldValue != null)
            {
                Console.WriteLine($"Converting property '{fieldName}' to CBOR.");
                var suitValue = (fieldValue as dynamic).ToSUIT();

                int suitKeyIndex = fieldInfo.SuitKey;
                cborList[suitKeyIndex] = suitValue;
            }
            else
            {
                Console.WriteLine($"Field '{fieldName}' is null.");
            }
        }
        return cborList;
    }


    public new void FromSUIT(List<object> suitList)
    {
        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var suitKeyIndex = fieldInfo.SuitKey;

            if (suitKeyIndex < suitList.Count)
            {
                var suitValue = suitList[suitKeyIndex];
                if (suitValue != null)
                {
                    GetType().GetProperty(fieldName)?.SetValue(this, suitValue);
                }
            }
        }
    }

    public string ToDebug(string indent)
    {
        var newIndent = indent + one_indent;
        var items = new List<string>();

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);
            if (fieldValue != null)
            {
                items.Add($"/ {fieldInfo.JsonKey} / {(fieldValue as dynamic).ToDebug(newIndent)}");
            }
        }

        return $"[\n{newIndent}{string.Join($",\n{newIndent}", items)}\n{indent}]";
    }
}