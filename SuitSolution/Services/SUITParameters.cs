using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITParameters : SUITManifestDict,ISUITConvertible<SUITParameters>
{
    public SUITUUID VendorId { get; set; }
    public SUITUUID ClassId { get; set; }
    public SUITBWrapField<SUITDigest> Digest { get; set; }
    public SUITPosInt Size { get; set; }
    public SUITTStr Uri { get; set; }
    public SUITComponentIndex Src { get; set; }
    public SUITCompressionInfo Compress { get; set; }
    public SUITPosInt Offset { get; set; }

    public SUITParameters()
    {
        VendorId = new SUITUUID();
        ClassId = new SUITUUID();
        Digest = new SUITBWrapField<SUITDigest>();
        Size = new SUITPosInt();
        Uri = new SUITTStr();
        Src = new SUITComponentIndex();
        Compress = new SUITCompressionInfo();
        Offset = new SUITPosInt();
    }

     public SUITParameters FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null)
            throw new ArgumentNullException(nameof(suitDict));


        return this;
    }

    public SUITParameters FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
            throw new ArgumentNullException(nameof(jsonData));

        // Assuming each property has a corresponding key in the jsonData

        return this;
    }

   

    public dynamic ToJson()
    {
        var jsonDict = new Dictionary<string, object>
        {
            ["vendor-id"] = VendorId.ToJson(),
            ["class-id"] = ClassId.ToJson(),
            ["digest"] = Digest.ToJson(),
            ["size"] = Size.ToJson(),
            ["uri"] = Uri.ToJson(),
            ["src"] = Src.ToJson(),
            ["compress"] = Compress.ToJson(),
            ["offset"] = Offset.ToJson()
        };

        return jsonDict;
    }

    public new Dictionary<string, object> ToSUIT()
    {
        var suitDict = base.ToSUIT();
        suitDict["vendor-id"] = VendorId.ToSUIT();
        suitDict["class-id"] = ClassId.ToSUIT();
        suitDict["digest"] = Digest.ToSUIT();
        suitDict["size"] = Size.ToSUIT();
        suitDict["uri"] = Uri.ToSUIT();
        suitDict["src"] = Src.ToSUIT();
        suitDict["compress"] = Compress.ToSUIT();
        suitDict["offset"] = Offset.ToSUIT();
        return suitDict;
    }

   

    public void FromSUIT(object suitData)
        {
            if (suitData is List<object> suitList)
            {
                foreach (var fieldInfo in fields.Values)
                {
                    var fieldName = fieldInfo.JsonKey;
                    var suitKey = fieldInfo.SuitKey;

                    if (suitList.Count > 0 && suitList[0] is Dictionary<object, object> suitDict && suitDict.ContainsKey(suitKey))
                    {
                        var suitValue = suitDict[suitKey];
                        if (suitValue != null)
                        {
                            var property = GetType().GetProperty(fieldName);
                            if (property != null)
                            {
                                property.SetValue(this, suitValue);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Property {fieldName} not found in {GetType().Name}");
                            }
                        }
                    }
                }
            }
            else if (suitData is Dictionary<object, object> suitDict)
            {
                foreach (var fieldInfo in fields.Values)
                {
                    var fieldName = fieldInfo.JsonKey;
                    var suitKey = fieldInfo.SuitKey;

                    if (suitDict.ContainsKey(suitKey))
                    {
                        var suitValue = suitDict[suitKey];
                        if (suitValue != null)
                        {
                            var property = GetType().GetProperty(fieldName);
                            if (property != null)
                            {
                                property.SetValue(this, suitValue);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Property {fieldName} not found in {GetType().Name}");
                            }
                        }
                    }
                }
            }
        }



      
    }