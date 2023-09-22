using System;
using System.Collections.Generic;
using SuitSolution.Services.SuitSolution.Services;

public class DirectiveTryEach : ISUITConvertible
{
    
    public List<DirectiveOverrideParameters> Overrides { get; set; }
    public int ConditionComponentOffset { get; set; }
    public DirectiveTryEach()
    {
        Overrides = new List<DirectiveOverrideParameters>();
        ConditionComponentOffset = 0;
    }

    public void InitializeRandomData()
    {
        Overrides = new List<DirectiveOverrideParameters>
        {
            new DirectiveOverrideParameters(), 
            new DirectiveOverrideParameters()
        };
        ConditionComponentOffset = 5; 
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            Overrides,
            ConditionComponentOffset
        };
        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count < 2)
        {
            throw new ArgumentException("Invalid SUIT list.");
        }

        if (suitList[0] is List<DirectiveOverrideParameters> overrides)
        {
            Overrides = overrides;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'Overrides' in DirectiveTryEach.");
        }

        if (suitList[1] is int componentOffset)
        {
            ConditionComponentOffset = componentOffset;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'ConditionComponentOffset' in DirectiveTryEach.");
        }
    }
}

public partial class DirectiveFetch : ISUITConvertible
{
    public string Uri { get; set; }
    public int CompressionAlgorithm { get; set; }

    public DirectiveFetch()
    {
        Uri = "";
        CompressionAlgorithm = 0;
    }

    public void InitializeRandomData()
    {
        Uri = "http://example.com"; 
        CompressionAlgorithm = new Random().Next(0, 3); 
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            Uri,
            CompressionAlgorithm
        };
        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count < 2)
        {
            throw new ArgumentException("Invalid SUIT list.");
        }

        if (suitList[0] is string uri)
        {
            Uri = uri;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'Uri' in DirectiveFetch.");
        }

        if (suitList[1] is int compressionAlgorithm)
        {
            CompressionAlgorithm = compressionAlgorithm;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'CompressionAlgorithm' in DirectiveFetch.");
        }
    }
}


public class DirectiveSetParameters : ISUITConvertible
{
    public int Offset { get; set; }
    public string Uri { get; set; }

    public DirectiveSetParameters()
    {
        Offset = 0;
        Uri = "";
    }

    public void InitializeRandomData()
    {
        Offset = new Random().Next(0, 10000);

        Uri = "http://example.com"; 
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            Offset,
            Uri
        };
        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count < 2)
        {
            throw new ArgumentException("Invalid SUIT list.");
        }

        if (suitList[0] is int offset)
        {
            Offset = offset;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'Offset' in DirectiveSetParameters.");
        }

        if (suitList[1] is string uri)
        {
            Uri = uri;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'Uri' in DirectiveSetParameters.");
        }
    }
}

public class DirectiveOverrideParameters : ISUITConvertible
{
    public Guid VendorId { get; set; }
    public Guid ClassId { get; set; }
    public int ImageSize { get; set; }

    public DirectiveOverrideParameters()
    {
        VendorId = Guid.Empty;
        ClassId = Guid.Empty;
        ImageSize = 0;
    }

    public void InitializeRandomData()
    {
        VendorId = Guid.NewGuid();
        ClassId = Guid.NewGuid();
        ImageSize = new Random().Next(100000, 1000000); 
    }

    public List<object> ToSUIT()
    {
        var suitList = new List<object>
        {
            VendorId.ToString(),
            ClassId.ToString(),
            ImageSize
        };
        return suitList;
    }

    public void FromSUIT(List<object> suitList)
    {
        if (suitList == null || suitList.Count < 3)
        {
            throw new ArgumentException("Invalid SUIT list.");
        }

        if (suitList[0] is string vendorIdString && Guid.TryParse(vendorIdString, out Guid vendorId))
        {
            VendorId = vendorId;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'VendorId' in DirectiveOverrideParameters.");
        }

        if (suitList[1] is string classIdString && Guid.TryParse(classIdString, out Guid classId))
        {
            ClassId = classId;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'ClassId' in DirectiveOverrideParameters.");
        }

        if (suitList[2] is int imageSize)
        {
            ImageSize = imageSize;
        }
        else
        {
            throw new ArgumentException("Invalid format for 'ImageSize' in DirectiveOverrideParameters.");
        }
    }
}
