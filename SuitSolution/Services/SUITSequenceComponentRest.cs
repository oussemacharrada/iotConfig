using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITSequenceComponentReset : SUITSequence, ISUITConvertible<SUITSequenceComponentReset>
{
    public SUITSequenceComponentReset()
    {
    }

    public new SUITSequenceComponentReset FromSUIT(List<object> suitList)
    {
        if (suitList == null)
        {
            throw new ArgumentNullException(nameof(suitList));
        }

        base.FromSUIT(suitList);

        return this;
    }

    public SUITSequenceComponentReset FromSUIT(Dictionary<string, object> suitDict)
    {
        throw new NotImplementedException();
    }

    public new SUITSequenceComponentReset FromJson(Dictionary<string, object> jsonData)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, object> ToSUIT()
    {
        // Set suitCommonInfo.current_index to null before calling base class's ToSUIT
        SUITCommonInfo.CurrentIndex = null;
        var suitData = base.ToSUIT();

        // Convert suitData to a dictionary if needed
        var suitDictionary = new Dictionary<string, object>();
        // You should populate suitDictionary with suitData according to your requirements

        return suitDictionary;
    }

    public string ToDebug(string indent)
    {
        throw new NotImplementedException();
    }
}