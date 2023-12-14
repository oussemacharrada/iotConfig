using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PeterO.Cbor;
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
    
    private Dictionary<string, ManifestKey> MkFields()
    {
        return new Dictionary<string, ManifestKey>
        {
            { "vendor-id", new ManifestKey("vendor-id", 1, () => VendorId, "VendorId") },
            { "class-id", new ManifestKey("class-id", 2, () => ClassId, "ClassId") },
            { "digest", new ManifestKey("image-digest", 3, () => Digest, "Digest") },
            { "size", new ManifestKey("image-size", 14, () => Size, "Size") },
            { "uri", new ManifestKey("uri", 21, () => Uri, "Uri") },
            { "src", new ManifestKey("source-component", 22, () => Src, "Src") },
            { "compress", new ManifestKey("compression-info", 19, () => Compress, "Compress") },
            { "offset", new ManifestKey("offset", 5, () => Offset, "Offset") }
        };
    
    }
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

        if (suitDict.TryGetValue(1, out var vendorIdValue) && vendorIdValue is Dictionary<object, object> vendorIdDict)
        {
            VendorId.FromSUIT(vendorIdDict);
        }

        if (suitDict.TryGetValue(2, out var classIdValue) && classIdValue is Dictionary<object, object> classIdDict)
        {
            ClassId.FromSUIT(classIdDict);
        }

        if (suitDict.TryGetValue(3, out var digestValue) && digestValue is Dictionary<object, object> digestDict)
        {
            Digest.v.FromSUIT(digestDict);
        }

        if (suitDict.TryGetValue(14, out var sizeValue) && sizeValue is Dictionary<object, object> sizeDict)
        {
            Size.FromSUIT(sizeDict);
        }

        if (suitDict.TryGetValue(21, out var uriValue) && uriValue is Dictionary<object, object> uriDict)
        {
            Uri.FromSUIT(uriDict);
        }

        if (suitDict.TryGetValue(22, out var srcValue) && srcValue is Dictionary<object, object> srcDict)
        {
            Src.FromSUIT(srcDict);
        }

        if (suitDict.TryGetValue(19, out var compressValue) && compressValue is Dictionary<object, object> compressDict)
        {
            string compressJson = JsonConvert.SerializeObject(compressDict);

            Compress.FromSUIT(compressJson);
        }

        // Handle Offset
        if (suitDict.TryGetValue(5, out var offsetValue) && offsetValue is Dictionary<object, object> offsetDict)
        {
            Offset.FromSUIT(offsetDict);
        }


        return this;
    }
    private void ResetFields()
    {
        VendorId = null;
        ClassId = null;
        Digest = null; // This will be set later
        Size = null;
        Uri = null;
        Src = null;
        Compress = null;
        Offset = null;
        // Reset any other fields you have
    }
    

    public SUITParameters FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
            throw new ArgumentNullException(nameof(jsonData));
        ResetFields();

        SUITDigest digest = new SUITDigest();
        if (jsonData.TryGetValue("algorithm-id", out var algorithmIdValue) && algorithmIdValue is string algorithmIdString)
        {
            // Assuming SUITDigestAlgo can be constructed from a string
            SUITDigestAlgo algo = new SUITDigestAlgo();
            algo.SetAlgorithmValue(algo.KeyMap[algorithmIdString]);
            digest.AlgorithmId = algo;
        }

        // Handle 'digest-bytes'
        if (jsonData.TryGetValue("digest-bytes", out var digestBytesValue) && digestBytesValue is byte[] digestBytesArray)
        {
            // Directly use the byte array
            SUITBytes bytes = new SUITBytes(digestBytesArray);
            digest.DigestBytes = bytes;
        }

        // Wrap the populated SUITDigest instance in SUITBWrapField
        Digest = new SUITBWrapField<SUITDigest>(digest);

        // Parse and set VendorId
        if (jsonData.TryGetValue("vendor-id", out var vendorIdValue) && vendorIdValue is string vendorIdString)
        {
            VendorId = new SUITUUID().FromJson(vendorIdString);
        }
        // Parse and set ClassId
        if (jsonData.TryGetValue("class-id", out var classIdValue) && classIdValue is  string classIdJson)
        {
            ClassId = new SUITUUID().FromJson(classIdJson);
        }

        // Parse and set Digest
        if (jsonData.TryGetValue("install-digest", out var digestValue) && digestValue is Dictionary<string, object> digestJson)
        {
            Digest = new SUITBWrapField<SUITDigest>(new SUITDigest().FromJson(digestJson));
        }

        // Parse and set Size
        if (jsonData.TryGetValue("install-size", out var sizeValue))
        {
            if (sizeValue is int intValue)
            {
                // Convert intValue to string and pass it to FromJson, then cast the result to SUITPosInt
                Size = new SUITPosInt(intValue) ;
            }else if (sizeValue is long longvalue)
            {
                Size = new SUITPosInt((int)longvalue);
            }
            else if (sizeValue is string sizeString)
            {
                // Pass sizeString to FromJson and cast the result to SUITPosInt
                Size = new SUITPosInt().FromJson(sizeString) as SUITPosInt;
            }
        
        }

        

        // Parse and set Uri
        if (jsonData.TryGetValue("uri", out var uriValue) && uriValue is Dictionary<string, object> uriJson)
        {
            Uri = new SUITTStr().FromJson(uriJson);
        }

        // Parse and set Src
        if (jsonData.TryGetValue("src", out var srcValue) && srcValue is Dictionary<string, object> srcJson)
        {
            Src = new SUITComponentIndex().FromJson(srcJson);
        }

        // Parse and set Compress
        // Parse and set Compress
        if (jsonData.TryGetValue("compress", out var compressValue) && compressValue is Dictionary<string, object> compressJson)
        {
            Compress = new SUITCompressionInfo().FromJson(compressJson);
        }




        // Parse and set Offset
        if (jsonData.TryGetValue("offset", out var offsetValue) && offsetValue is int offsetJson)
        {
            Offset = new SUITPosInt(offsetJson); // Assuming SUITPosInt has a constructor that takes an int
        }


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
            ["offset"] = Offset.ToJson() };

        return jsonDict;
    }

    public new Dictionary<int, object> ToSUIT()
    {
        var suitDict = new Dictionary<int, object>();

        if (VendorId != null) // Assuming IsEmpty() is a method to check if the object is in its default state
            suitDict.Add(1, VendorId.ToSUIT());

        if (ClassId != null)
            suitDict.Add(2, ClassId.ToSUIT());

        if (Digest.v.DigestBytes != null)
            suitDict.Add(3, Digest.ToSUIT());

        if (Size != null) 
            suitDict.Add(14,Size.ToSUIT());

        if (Uri != null )
            suitDict.Add(21, Uri.ToSUIT());

        if (Src != null )
            suitDict.Add(22,Src.ToSUIT());

        if (Compress != null )
            suitDict.Add(19, Compress.ToSUIT());

        if (Offset != null )
            suitDict.Add(5,Offset.ToSUIT());
    
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