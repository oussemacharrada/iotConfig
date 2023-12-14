using System;
using PeterO.Cbor;
namespace SuitSolution.Services;

public class SUITDirective
{
    public int DirectiveId { get; set; }
    public SUITParameters Parameters { get; set; }
    public SUITTryEach TryEach { get; set; }
    public SUITCommonInfo Common { get; set; }

    public SUITDirective()
    {
        DirectiveId = 0;
        Parameters = new SUITParameters();
        TryEach = new SUITTryEach();
        Common = new SUITCommonInfo();
    }

 
}
