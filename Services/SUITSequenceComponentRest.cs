using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITSequenceComponentReset : SUITSequence, ISUITConvertible<SUITSequenceComponentReset>
{
    
    public SUITSequenceComponentReset()
    {
    }
   

    public SUITSequenceComponentReset(SUITSequence sequence) : base()
    {
        if (sequence == null)
        {
            throw new ArgumentNullException(nameof(sequence));
        }

            this.Items = new List<SUITCommand>(sequence.Items);
    }
    public new SUITSequenceComponentReset FromSUIT(List<object> suitList)
    {
        if (suitList == null)
        {
            throw new ArgumentNullException(nameof(suitList));
        }

        if (suitList.Count % 2 != 0)
        {
            throw new ArgumentException("The suitList should have an even number of elements.");
        }

        var suitDict = new Dictionary<object, object>();
        for (int i = 0; i < suitList.Count; i += 2)
        {
            if (!(suitList[i] is string key))
            {
                throw new ArgumentException($"Expected a string key at index {i} in the suitList.");
            }

            suitDict[key] = suitList[i + 1];
        }

        base.FromSUIT(suitDict);
    
        return this;
    }


    public SUITSequenceComponentReset FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null)
        {
            throw new ArgumentNullException(nameof(suitDict));
        }

        base.FromSUIT(suitDict);
        return this;
    }



    public new SUITSequenceComponentReset FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData));
        }

        base.FromJson(jsonData);
        return this;
    }
    public  dynamic ToSUIT()
    {
        SUITCommonInfo.CurrentIndex = -1;
        var tosuit = base.ToSUIT();
        return tosuit;
    }
    public string ToDebug(string indent)
    {
        return base.ToDebug(indent);
    }
}