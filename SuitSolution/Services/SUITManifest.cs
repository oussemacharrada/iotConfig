using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SuitSolution.Interfaces;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Services;

public class SUITManifest : SUITManifestDict, ISUITConvertible<SUITManifest>
{
    public SUITManifest()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("manifest-version", 1, () => version,"version"),
            ("manifest-sequence-number", 2, () => sequence,"sequence"),
            ("common", 3, () => common,"common"),
                ("reference-uri", 4, () => refuri,"refuri"),
                ("dependency-resolution", 7, () => deres,"deres"),
                ("payload-fetch", 8, () => fetch,"fetch"),
                ("install", 9, () => install,"install"),
                ("validate", 10  , () => validate,"validate"),
                ("load", 11, () => load,"load"),
                ("run", 12, () => run,"run"),
                ("text", 13, () => text,"text")
            ));

   
    }

    public SUITPosInt version { get; set; }= new SUITPosInt();
    public SUITPosInt sequence { get; set; }= new SUITPosInt();
    public SUITBWrapField<SUITCommon> common { get; set; }= new SUITBWrapField<SUITCommon>();
    public SUITTStr refuri { get; set; } = new SUITTStr();
    public SUITSeverableField<SUITSequenceComponentReset> deres { get; set; }= new SUITSeverableField<SUITSequenceComponentReset>();
    public SUITSeverableField<SUITSequenceComponentReset> fetch { get; set; }= new SUITSeverableField<SUITSequenceComponentReset>();
    public SUITSeverableField<SUITSequenceComponentReset> install { get; set; }= new SUITSeverableField<SUITSequenceComponentReset>();
    public SUITBWrapField<SUITSequenceComponentReset> validate { get; set; }= new SUITBWrapField<SUITSequenceComponentReset>();
    public SUITBWrapField<SUITSequenceComponentReset> load { get; set; }= new SUITBWrapField<SUITSequenceComponentReset>();
    public SUITBWrapField<SUITSequenceComponentReset> run { get; set; }= new SUITBWrapField<SUITSequenceComponentReset>();
    public SUITSeverableField<SUITText> text { get; set; }= new SUITSeverableField<SUITText>();
    public SUITBytes coswid { get; set; } = new SUITBytes();

    public new SUITManifest FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null)
        {
            throw new ArgumentNullException(nameof(suitDict));
        }

        base.FromSUIT(suitDict);

        if (suitDict.TryGetValue("version", out var versionValue) && versionValue is int versionInt)
        {
            version = new SUITPosInt { v = versionInt };
        }

        if (suitDict.TryGetValue("sequence", out var sequenceValue) && sequenceValue is int sequenceInt)
        {
            sequence = new SUITPosInt { v = sequenceInt };
        }
    
        if (suitDict.TryGetValue("common", out var commonValue) && commonValue is Dictionary<string, object> commonDict)
        {
            var commonValues = SerializationHelper.SerializeToBytes(commonDict); 
            common.FromSUIT(commonValues);
        }

        if (suitDict.TryGetValue("refuri", out var refuriValue) && refuriValue is string refuriStr)
        {
            refuri = new SUITTStr { v = refuriStr };
        }

        if (suitDict.TryGetValue("deres", out var deresValue) && deresValue is List<object> deresList)
        {
            deres.FromSUIT(deresList);
        }

        if (suitDict.TryGetValue("fetch", out var fetchValue) && fetchValue is List<object> fetchList)
        {
            fetch.FromSUIT(fetchList);
        }

        if (suitDict.TryGetValue("install", out var installValue) && installValue is List<object> installList)
        {
            install.FromSUIT(installList);
        }

        if (suitDict.TryGetValue("validate", out var validateValue) && validateValue is Dictionary<string, object> validateDict)
        {
            var validateDicts = SerializationHelper.SerializeToBytes(validateDict); 

            validate.FromSUIT(validateDicts);
        }

        if (suitDict.TryGetValue("load", out var loadValue) && loadValue is Dictionary<string, object> loadDict)
        {
            var loadDicts = SerializationHelper.SerializeToBytes(loadDict); 

            load.FromSUIT(loadDicts);
        }

        if (suitDict.TryGetValue("run", out var runValue) && runValue is Dictionary<string, object> runDict)
        {
            var commonValues = SerializationHelper.SerializeToBytes(runDict); 

            run.FromSUIT(commonValues);
        }

        if (suitDict.TryGetValue("text", out var textValue) && textValue is List<object> textList)
        {
            text.FromSUIT(textList);
        }

        if (suitDict.TryGetValue("coswid", out var coswidValue) && coswidValue is string coswidStr)
        {
            coswid = new SUITBytes { v = Convert.FromBase64String(coswidStr) };
        }

        return this;
    }

    public new SUITManifest FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData));
        }

        if (jsonData.TryGetValue("version", out var versionValue) && versionValue is int versionInt)
        {
            version = new SUITPosInt { v = versionInt };
        }

        if (jsonData.TryGetValue("sequence", out var sequenceValue) && sequenceValue is int sequenceInt)
        {
            sequence = new SUITPosInt { v = sequenceInt };
        }

        if (jsonData.TryGetValue("common", out var commonValue) && commonValue is Dictionary<string, object> commonDict)
        {
            common = new SUITBWrapField<SUITCommon>();
            common.FromJson(JsonConvert.SerializeObject(commonDict));
        }


        if (jsonData.TryGetValue("refuri", out var refuriValue) && refuriValue is string refuriStr)
        {
            refuri = new SUITTStr { v = refuriStr };
        }

        if (jsonData.TryGetValue("deres", out var deresValue) && deresValue is List<object> deresList)
        {
            deres = new SUITSeverableField<SUITSequenceComponentReset>();
            deres.FromJson(JsonConvert.SerializeObject(deresList));
        }


        if (jsonData.TryGetValue("fetch", out var fetchValue) && fetchValue is List<object> fetchList)
        {
            fetch = new SUITSeverableField<SUITSequenceComponentReset>();
            fetch.FromJson(JsonConvert.SerializeObject(fetchList));        }

        if (jsonData.TryGetValue("install", out var installValue) && installValue is List<object> installList)
        {
            install = new SUITSeverableField<SUITSequenceComponentReset>();
            install.FromJson(JsonConvert.SerializeObject(installList));          }

        if (jsonData.TryGetValue("validate", out var validateValue) && validateValue is Dictionary<string, object> validateDict)
        {
            validate = new SUITBWrapField<SUITSequenceComponentReset>();
            validate.FromJson(JsonConvert.SerializeObject(validateDict));
            
        }

        if (jsonData.TryGetValue("load", out var loadValue) && loadValue is Dictionary<string, object> loadDict)
        {
            load = new SUITBWrapField<SUITSequenceComponentReset>();
            load.FromJson(JsonConvert.SerializeObject(loadDict));
        }

        if (jsonData.TryGetValue("run", out var runValue) && runValue is Dictionary<string, object> runDict)
        {
            run = new SUITBWrapField<SUITSequenceComponentReset>();
            run.FromJson(JsonConvert.SerializeObject(runDict));          }

        if (jsonData.TryGetValue("text", out var textValue) && textValue is List<object> textList)
        {
            text = new SUITSeverableField<SUITText>();
            text.FromJson(JsonConvert.SerializeObject(textList));          }

        if (jsonData.TryGetValue("coswid", out var coswidValue) && coswidValue is string coswidStr)
        {
            coswid = new SUITBytes { v = Convert.FromBase64String(coswidStr) };
        }

        return this;
    }

    public dynamic ToSuit()
    {
        var dic = base.ToSUIT();
        dic.Add(3, common.ToSUIT());
        return dic;
    }
    public new dynamic ToJson()
    {
        var jsonData = base.ToJson();
        

        jsonData["version"] = version.ToJson();

        jsonData["sequence"] = sequence.ToJson();

        jsonData["common"] = common.ToJson();

        jsonData["refuri"] = refuri.ToJson();

        jsonData["deres"] = deres.ToJson();

        jsonData["fetch"] = fetch.ToJson();

        jsonData["install"] = install.ToJson();

        jsonData["validate"] = validate.ToJson();

        jsonData["load"] = load.ToJson();

        jsonData["run"] = run.ToJson();

        jsonData["text"] = text.ToJson();

        jsonData["coswid"] = Convert.ToBase64String(coswid.v);

        return jsonData;
    }
}
