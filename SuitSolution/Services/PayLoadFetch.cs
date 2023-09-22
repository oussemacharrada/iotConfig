using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

public class PayloadFetch : ISUITConvertible
{
    public List<object> Operations { get; set; }
    public int InitialRetryDelay { get; set; }

    public PayloadFetch()
    {
        Operations = new List<object>();
        InitialRetryDelay = 0;
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            Operations,
            InitialRetryDelay
        };
        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count != 2)
        {
            throw new Exception("Invalid PayloadFetch format");
        }

        if (suitList[0] is CBORObject cborArray && cborArray.Type == CBORType.Array)
        {
            Operations = new List<object>();
            foreach (CBORObject item in cborArray.Values)
            {
            
                    Dictionary<string, CBORObject> dictionary = item.ToObject<Dictionary<string, CBORObject>>();

                    Operations.Add(dictionary);
                
            }
        }
        else
        {
            throw new Exception("Invalid PayloadFetch format");
        }

        if (suitList[1] is CBORObject cborInt)
        {
            InitialRetryDelay = cborInt.AsInt32();
        }
        else
        {
            throw new Exception($"Invalid PayloadFetch format. suitList[1]: {suitList[1]}");
        }
    }


    public void InitializeRandomData()
    {
        Operations = new List<object>
        {
            new DirectiveFetch(), 
            new DirectiveFetch()
        };
        InitialRetryDelay = new Random().Next(0, 1000); 
    }
}