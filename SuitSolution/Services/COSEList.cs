using SuitSolution.Interfaces;

public class COSEList : SUITManifestArray<COSETaggedAuth>, ISUITConvertible<COSEList>
{
    public SUITBWrapField<COSETaggedAuth> Field { get; set; }

    public COSEList()
    {
        Field = new SUITBWrapField<COSETaggedAuth>
        {
            v = new COSETaggedAuth()
        };
    }


    public dynamic ToSUIT()
    {
        return base.ToSuit();
    }

    public Dictionary<string, object> ToJson()
    {
        return new Dictionary<string, object>
        {
            { "items", Items.Select(item => item.ToJson()).ToList() }
        };
    }

    public string ToDebug(string indent)
    {
        return base.ToDebug(indent);
    }

    public COSEList FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null || !suitDict.ContainsKey("items"))
        {
            throw new ArgumentException("Invalid SUIT data.");
        }

        var itemsList = (List<object>)suitDict["items"];
        Items = itemsList.Select(item => 
        {
            var coseTaggedAuth = new COSETaggedAuth();
            coseTaggedAuth.FromSUIT((Dictionary<object, object>)item);
            return coseTaggedAuth;
        }).ToList();

        return this;
    }

    public COSEList FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null || !jsonData.ContainsKey("items"))
        {
            throw new ArgumentException("Invalid JSON data.");
        }

        var itemsList = (List<object>)jsonData["items"];
        Items = itemsList.Select(item => 
        {
            var coseTaggedAuth = new COSETaggedAuth();
            coseTaggedAuth.FromJson((Dictionary<string, object>)item);
            return coseTaggedAuth;
        }).ToList();

        return this;
    }
}