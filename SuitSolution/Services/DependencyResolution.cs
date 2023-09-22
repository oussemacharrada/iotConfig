using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services;

using System.Collections.Generic;

public class DependencyResolution : ISUITConvertible
{
    public List<object> Components { get; set; }
    public string CommonSequence { get; set; }

    public DependencyResolution()
    {
        Components = new List<object>();
        CommonSequence = string.Empty;
    }
    public void InitializeRandomData()
    {
 
        Components = new List<object>
        {
            new List<object> { 1, "https://example.com/dependency1" },
            new List<object> { 2, "https://example.com/dependency2" },
            new List<object> { 3, "https://example.com/dependency3" }
        };

        CommonSequence = "SampleCommonSequence";
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            Components,
            CommonSequence
        };
        return suitList;
    }
    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count != 2)
        {
            throw new Exception("Invalid DependencyResolution format");
        }

            if (suitList[0] is CBORObject cborArray && cborArray.Type == CBORType.Array)
        {
            string serializedData = cborArray.ToJSONString();

            Components.Add(serializedData);
        }
        else
        {
            throw new Exception("Invalid format for 'Components' in DependencyResolution.");
        }

        if (suitList[1] is CBORObject commonSequenceCbor && commonSequenceCbor.Type == CBORType.TextString)
        {
            CommonSequence = commonSequenceCbor.AsString();
        }
        else
        {
            throw new Exception("Invalid format for 'CommonSequence' in DependencyResolution.");
        }
    }



    }


