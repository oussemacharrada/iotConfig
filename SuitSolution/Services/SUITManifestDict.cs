using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterO.Cbor;
using System.Text.Json;
using System.Text.Json.Serialization;
using SuitSolution.Interfaces;
using SuitSolution.Services;
using SuitSolution.Services.SuitSolution.Services;

public class SUITManifestDict : ISUITConvertible
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("sequence")]
    public int Sequence { get; set; }

    [JsonPropertyName("dependencies")]
    public List<SUITDependency> Dependencies { get; set; }

    [JsonPropertyName("dependenciesWrap")]
    public List<SUITBWrapField<COSEList>> DependenciesWrap { get; set; }

    [JsonPropertyName("componentsWrap")]
    public List<SUITBWrapField<COSEList>> ComponentsWrap { get; set; }

    [JsonPropertyName("manifestsWrap")]
    public List<SUITBWrapField<SUITBWrapField<CoseTaggedAuth>>> ManifestsWrap { get; set; }

    public SUITManifestDict()
    {
        Dependencies = new List<SUITDependency>();
        DependenciesWrap = new List<SUITBWrapField<COSEList>>();
        ComponentsWrap = new List<SUITBWrapField<COSEList>>();
        ManifestsWrap = new List<SUITBWrapField<SUITBWrapField<CoseTaggedAuth>>>();
    }

    public byte[] EncodeToBytes()
    {
        string json = JsonSerializer.Serialize(this);

        return Encoding.UTF8.GetBytes(json);
    }

    public void DecodeFromBytes(byte[] bytes)
    {
        string json = Encoding.UTF8.GetString(bytes);

        SUITManifestDict decodedObject = JsonSerializer.Deserialize<SUITManifestDict>(json);

        Version = decodedObject.Version;
        Sequence = decodedObject.Sequence;
        Dependencies = decodedObject.Dependencies;
        DependenciesWrap = decodedObject.DependenciesWrap;
        ComponentsWrap = decodedObject.ComponentsWrap;
        ManifestsWrap = decodedObject.ManifestsWrap;
    }

    public void AddRange(IEnumerable<byte[]> items)
    {
        foreach (var item in items)
        {
            var depWrap = new SUITBWrapField<COSEList>();
            depWrap.DecodeFromBytes(item);

            DependenciesWrap.Add(depWrap);
        }
    }

    public List<object> ToSUIT()
    {
        List<object> suitList = new List<object>
        {
            Version,
            Sequence,
            Dependencies,
            DependenciesWrap.Select(dep => dep.EncodeToBytes()).ToList(),
            ComponentsWrap.Select(comp => comp.EncodeToBytes()).ToList(),
            ManifestsWrap.Select(manifest => manifest.EncodeToBytes()).ToList()
        };

        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count < 6)
        {
            throw new ArgumentException("Invalid SUIT List");
        }

        Version = Convert.ToInt32(suitList[0]);
        Sequence = Convert.ToInt32(suitList[1]);
        Dependencies = (List<SUITDependency>)suitList[2];
        
        DependenciesWrap = ((List<byte[]>)suitList[3])
            .Select(bytes =>
            {
                var depWrap = new SUITBWrapField<COSEList>();
                depWrap.DecodeFromBytes(bytes);
                return depWrap;
            }).ToList();

        ComponentsWrap = ((List<byte[]>)suitList[4])
            .Select(bytes =>
            {
                var compWrap = new SUITBWrapField<COSEList>();
                compWrap.DecodeFromBytes(bytes);
                return compWrap;
            }).ToList();

        ManifestsWrap = ((List<byte[]>)suitList[5])
            .Select(bytes =>
            {
                var manifestWrap = new SUITBWrapField<SUITBWrapField<CoseTaggedAuth>>();
                manifestWrap.DecodeFromBytes(bytes);
                return manifestWrap;
            }).ToList();
    }
}
