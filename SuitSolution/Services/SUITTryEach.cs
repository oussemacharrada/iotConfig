using System;
using System.Collections.Generic;
using PeterO.Cbor;
namespace SuitSolution.Services
{
public class SUITTryEach
{
    public SUITDirective Directive { get; set; }
    public List<SUITDirectiveParameters> DirectiveParameters { get; set; }

    public SUITTryEach()
    {
        Directive = new SUITDirective();
        DirectiveParameters = new List<SUITDirectiveParameters>();
    }

    public CBORObject ToCBORObject()
    {
        var cbor = CBORObject.NewArray();

        cbor.Add(Directive.ToCBORObject());

        if (DirectiveParameters.Count > 0)
        {
            var parametersArray = CBORObject.NewArray();
            foreach (var parameter in DirectiveParameters)
            {
                parametersArray.Add(parameter.ToCBORObject());
            }
            cbor.Add(parametersArray);
        }

        return cbor;
    }

    public void FromCBORObject(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Array || cbor.Count < 1)
        {
            throw new FormatException("Invalid CBOR data for SUITTryEach");
        }

        Directive.FromCBORObject(cbor[0]);

        if (cbor.Count > 1)
        {
            var parametersArray = cbor[1];
            if (parametersArray.Type != CBORType.Array)
            {
                throw new FormatException("Invalid CBOR data for SUITTryEach parameters");
            }

            foreach (var parameter in parametersArray.Values)
            {
                var param = new SUITDirectiveParameters();
                param.FromCBORObject(parameter);
                DirectiveParameters.Add(param);
            }
        }
    }
}
}