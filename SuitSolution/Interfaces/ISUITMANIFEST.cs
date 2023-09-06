using SuitSolution.Services;

namespace SuitSolution.Interfaces;

public interface ISUITSequenceItem
{
    byte[] ToSUIT();
    void FromSUIT(byte[] data);
    string ToDebug(int indent);
}

public interface ISUITSequence : ISUITSequenceItem
{
    List<ISUITSequenceItem> Items { get; set; }
}



public interface ISUITComponentText : ISUITSequenceItem
{
    string VendorName { get; set; }
    string ModelName { get; set; }
    string VendorDomain { get; set; }
    string ModelInfo { get; set; }
    string ComponentDescription { get; set; }
    string Version { get; set; }
    string RequiredVersion { get; set; }
}
public interface ISUITSequenceComponentReset : ISUITSequenceItem
{
}


public interface ISUITCommonInfo
{
    ISUITVersion Version { get; set; }
    ISUITComponentIdentifier SequenceNumber { get; set; }
    ISUITUUID UEID { get; set; }
    ISUITComponentIdentifier UEIDComponent { get; set; }
    ISUITUUID ManifestUUID { get; set; }
    ISUITPayloadInfo PayloadInfo { get; set; }
    ISUITDependency DependencyInfo { get; set; }
    List<ISUITComponentIdentifier> InstallList { get; set; }
    List<ISUITComponentIdentifier> UninstallList { get; set; }
    ISUITSysinfo SysInfo { get; set; }

    byte[] ToSUIT();
}
public interface ISUITVersion
{
    int Major { get; set; }
    int Minor { get; set; }
    int Version { get; set; }

    byte[] ToSUIT();
}

public interface ISUITComponentIdentifier
{
    int Index { get; set; }
    ISUITUUID UUID { get; set; }

    byte[] ToSUIT();
}
public interface ISUITPayloadInfo
{
    ISUITUUID UUID { get; set; }
    ulong Size { get; set; }
    string Uri { get; set; }

    byte[] ToSUIT();
}
public interface ISUITDependency
{
    ISUITUUID SourceComponentId { get; set; }
    ISUITUUID TargetComponentId { get; set; }
    ISUITUUID Digest { get; set; }
    ISUITUUID Prefix { get; set; }
    ISUITInt Version { get; set; }

    byte[] ToSUIT();
}
public interface ISUITSysinfo
{
    string Manufacturer { get; set; }
    string ModelNumber { get; set; }
    string SerialNumber { get; set; }
    string DeviceClass { get; set; }
    string VendorIdentifier { get; set; }

    byte[] ToSUIT();
}
public interface ISUITInt
{
    long Value { get; set; }

    byte[] ToSUIT();
}
