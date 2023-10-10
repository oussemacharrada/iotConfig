using System.Text;
using PeterO.Cbor;

using Bogus;
using SuitSolution.Services;
public class SUITComponentIdSeeder
{
    public static SUITComponentId GenerateRandomSUITComponentId()
    {
        var faker = new Faker();

        var componentIds = new List<SUITBytes>();

        for (var i = 0; i < faker.Random.Number(1, 5); i++)
        {
            var componentIdBytes = Encoding.UTF8.GetBytes(faker.Random.AlphaNumeric(10)); 
            componentIds.Add(new SUITBytes(componentIdBytes));
        }

        return new SUITComponentId(componentIds);
    }

    public static Dictionary<string,object> GenerateRandomSUITComponentIdJson()
    {
        var suitComponentId = GenerateRandomSUITComponentId();
        var jsonRepresentation = suitComponentId.ToJson();
        return jsonRepresentation;
    }
}
public class SUITDigestAlgoDataSeeder
{
    public static Faker<SUITDigestAlgo> GenerateSUITDigestAlgo()
    {
        var id = new Random().Next(1, 100);
        var algorithm = "sha256"; 

        return new Faker<SUITDigestAlgo>()
            .RuleFor(d => id, id)
            .RuleFor(d =>algorithm , algorithm);
    }
}
public class SUITDigestDataSeeder
{
    public static Faker<SUITDigest> GenerateSUITDigest()
    {
        var algo = SUITDigestAlgoDataSeeder.GenerateSUITDigestAlgo().Generate();
        var digest = Guid.NewGuid().ToByteArray(); 
        return new Faker<SUITDigest>()
            .RuleFor(d => algo, algo)
            .RuleFor(d => digest, digest);
    }
}
public class SUITReportingPolicyDataSeeder
{
    public static Faker<SUITReportingPolicy> GenerateSUITReportingPolicy()
    {
        var policy = new Randomizer().Int(1, 100); // Adjust the range as needed.

        return new Faker<SUITReportingPolicy>()
            .CustomInstantiator(f => new SUITReportingPolicy(policy));
    }
}
public class COSESign1DataSeeder
{
    public static Faker<COSESign1> GenerateCOSESign1()
    {
        return new Faker<COSESign1>()
            .CustomInstantiator(f => new COSESign1());
    }
}
public class COSEAlgorithmsDataSeeder
{
    public static Faker<COSEAlgorithms> GenerateCOSEAlgorithms()
    {
        return new Faker<COSEAlgorithms>()
            .CustomInstantiator(f => COSEAlgorithms.CreateDefault());
    }
}
public class COSEHeaderMapDataSeeder
{
    public static Faker<COSEHeaderMap> GenerateCOSEHeaderMap()
    {
        return new Faker<COSEHeaderMap>()
            .RuleFor(p => p.Alg, f => COSEAlgorithmsDataSeeder.GenerateCOSEAlgorithms().Generate())
            .RuleFor(p => p.Kid, f => new SUITBytes(Encoding.UTF8.GetBytes(f.Random.AlphaNumeric(16))));
    }
}
public class COSE_MacDataSeeder
{
    public static Faker<COSE_Mac> GenerateCOSE_Mac()
    {
        return new Faker<COSE_Mac>()
            .RuleFor(p => p.Protected, f => f.Random.Bytes(16))
            .RuleFor(p => p.Unprotected, f => COSEHeaderMapDataSeeder.GenerateCOSEHeaderMap().Generate())
            .RuleFor(p => p.Payload, f => SUITDigestDataSeeder.GenerateSUITDigest().Generate())
            .RuleFor(p => p.Tag, f => new SUITBytes(Encoding.UTF8.GetBytes(f.Random.AlphaNumeric(16))));
    }
}
public class COSE_Mac0DataSeeder
{
    public static Faker<COSE_Mac0> GenerateCOSE_Mac0()
    {
        return new Faker<COSE_Mac0>()
            .RuleFor(p => p.Protected, f => f.Random.Bytes(16))
            .RuleFor(p => p.Payload, f => SUITDigestDataSeeder.GenerateSUITDigest().Generate())
            .RuleFor(p => p.Tag, f =>new SUITBytes(Encoding.UTF8.GetBytes(f.Random.AlphaNumeric(16))));
    }
}
public class COSESignDataSeeder
{
    public static Faker<COSESign> GenerateCOSESign()
    {
        return new Faker<COSESign>()
            .RuleFor(p => p.Protected, f => CBORObject.NewMap())
            .RuleFor(p => p.Unprotected, f => CBORObject.NewMap())
            .RuleFor(p => p.Payload, f => CBORObject.Null)
            .RuleFor(p => p.Signature, f => CBORObject.Null);
    }
}
public class COSETaggedAuthDataSeeder
{
    public static Faker<COSETaggedAuth> GenerateCOSETaggedAuth()
    {
        return new Faker<COSETaggedAuth>()
            .RuleFor(p => p.CoseSign, f => COSESignDataSeeder.GenerateCOSESign().Generate())
            .RuleFor(p => p.CoseSign1, f => COSESign1DataSeeder.GenerateCOSESign1().Generate())
            .RuleFor(p => p.CoseMac, f => COSE_MacDataSeeder.GenerateCOSE_Mac().Generate())
            .RuleFor(p => p.CoseMac0, f => COSE_Mac0DataSeeder.GenerateCOSE_Mac0().Generate());
    }
}

public class COSEListDataSeeder
{
    public static COSEList GenerateCOSEList(int itemCount)
    {
        return new Faker<COSEList>()
            .RuleFor(p => p.items, f => COSETaggedAuthDataSeeder.GenerateCOSETaggedAuth().Generate(itemCount));
    }
}

public class SUITBytesDataSeeder
{
    public static SUITBytes GenerateSUITBytes()
    {
        return new Faker<SUITBytes>()
            .RuleFor(p => p.v, f => GenerateRandomByteArray());
    }

    private static byte[] GenerateRandomByteArray()
    {
        var random = new Random();
        var length = random.Next(1, 256); 
        var byteArray = new byte[length];
        random.NextBytes(byteArray);
        return byteArray;
    }
}

public class SUITCmdDataSeeder
{
    public static List<SUITCommandContainer.SUITCmd> SeedSUITCmds(int numberOfCmds)
    {
        var suitCmds = new List<SUITCommandContainer.SUITCmd>();

        var cmdFaker = new Faker<SUITCommandContainer.SUITCmd>()
            .RuleFor(cmd => cmd.json_key, f => f.Commerce.ProductName())
            .RuleFor(cmd => cmd.suit_key, f => f.Random.Number(1, 10).ToString())
            .RuleFor(cmd => cmd.dep_params, f => f.Make(3, () => f.Commerce.ProductName()));

        for (int i = 0; i < numberOfCmds; i++)
        {
            var cmd = cmdFaker.Generate();
            suitCmds.Add(cmd);
        }

        return suitCmds;
    }
}

public static class SUITCommandContainerDataSeeder
{
    public static List<SUITCommandContainer> SeedSUITCommandContainers(int numberOfContainers)
    {
        var containers = new List<SUITCommandContainer>();

        var containerFaker = new Faker<SUITCommandContainer>()
            .RuleFor(container => container.json_key, f => f.Lorem.Word())
            .RuleFor(container => container.suit_key, f => f.Random.Number(1, 10).ToString())
            .RuleFor(container => container.dep_params, f => f.Make(3, () => f.Lorem.Word()))
            .RuleFor(container => container.argtype, typeof(SUITCommandContainer.SUITCmd)); // Specify the argument type's fully qualified name

        for (int i = 0; i < numberOfContainers; i++)
        {
            var container = containerFaker.Generate();
            containers.Add(container);
        }

        return containers;
    }
}


namespace SuitSolution.Services
{
    public static class SUITCommandDataSeeder
    {
        public static List<SUITCommand> SeedSUITCommands(int numberOfCommands)
        {
            var commands = new List<SUITCommand>();

            var commandFaker = new Faker<SUITCommand>()
                .RuleFor(command => command.commands, f => SUITCommandContainerDataSeeder.SeedSUITCommandContainers(numberOfCommands))
                .RuleFor(command => command.ToSUIT(), f => new Dictionary<string, object>
                {
                    { "command-id", f.Lorem.Word() },
                    { "command-arg", f.Lorem.Word() }
                })
                .RuleFor(command => command.ToJson(), f => new Dictionary<string, object>
                {
                    { "command-id", f.Lorem.Word() },
                    { "command-arg", f.Lorem.Word() }
                })
                .RuleFor(command => command.ToDebug("dummy_indent"), f => $"Debug info for SUITCommand with indent dummy_indent");

            for (int i = 0; i < numberOfCommands; i++)
            {
                var command = commandFaker.Generate();
                commands.Add(command);
            }

            return commands;
        }
    }
}

public static class SUITSequenceComponentResetDataSeeder
{

    public static SUITSequenceComponentReset SeedSUITSequenceComponentReset()
    {
        var reset = new SUITSequenceComponentReset();

        // Add any initial data to the reset instance if needed
        // reset.SomeProperty = someValue;

        return reset;
    }

}

public  class SUITComponentsDataSeeder
{
    public static SUITComponents SeedSUITComponents(int numberOfComponents)
    {
        var components = new SUITComponents();

        var componentList = new List<object>();

        for (int i = 0; i < numberOfComponents; i++)
        {
            var componentData = new Dictionary<string, object>
            {
                { "component_id", i + 10 },
            };

            componentList.Add(componentData);
        }

        components.FromJson(componentList);

        return components;
    }

}   

public static class SUITDependenciesDataSeeder
{
    public static SUITDependencies SeedSUITDependencies(int numberOfDependencies)
    {
        var dependencies = new SUITDependencies();

        var dependencyList = new List<object>();

        for (int i = 0; i < numberOfDependencies; i++)
        {
            var dependencyData = new Dictionary<string, object>
            {
                { "dependencyKey", $"DependencyKey{i + 1}" },
                { "dependencyValue", $"DependencyValue{i + 1}" }
            };

            dependencyList.Add(dependencyData);
        }

        dependencies.FromJson(new Dictionary<string, object>
        {
            { "dependencies", dependencyList }
        });

        return dependencies;
    }
}
public static class SUITDependencyDataSeeder
{
    public static SUITDependency SeedSUITDependency()
    {
        var dependency = new SUITDependency
        {
            DependencyDigest = SUITDigestDataSeeder.GenerateSUITDigest(),
            DependencyPrefix = SUITComponentIdSeeder.GenerateRandomSUITComponentId()
        };

        return dependency;
    }
}

public static class SUITCommonDataSeeder
{
    public static SUITCommon SeedSUITCommon()
    {
        var common = new SUITCommon();

        common.dependencies.v = SUITDependenciesDataSeeder.SeedSUITDependencies(2);

        common.components = SUITComponentsDataSeeder.SeedSUITComponents(2);

        common.commonSequence.v = SUITSequenceComponentResetDataSeeder.SeedSUITSequenceComponentReset();

        return common;
    }
}

public static class SUITCommonInfoDataSeeder
{
    public static void SeedSUITCommonInfo()
    {
        SUITCommonInfo.ComponentIds = SeedComponentIds();
        SUITCommonInfo.Dependencies = SeedDependencies();
    }

    private static List<SUITComponentId> SeedComponentIds()
    {
      
        var componentIds = new List<SUITComponentId>
        {
            SUITComponentIdSeeder.GenerateRandomSUITComponentId(),
            SUITComponentIdSeeder.GenerateRandomSUITComponentId(),
            SUITComponentIdSeeder.GenerateRandomSUITComponentId(),
            // Add more component ids as needed
        };

        return componentIds;
    }

    private static List<SUITDependency> SeedDependencies()
    {
        var dependencies = new List<SUITDependency>
        {
            SUITDependencyDataSeeder.SeedSUITDependency(),
            SUITDependencyDataSeeder.SeedSUITDependency(),
            // Add more dependencies as needed
        };

        return dependencies;
    }
}
public static class SUITComponentIndexDataSeeder
{
    public static List<SUITComponentIndex> SeedSUITComponentIndexes(int numberOfIndexes)
    {
        var indexes = new List<SUITComponentIndex>();

        for (int i = 0; i < numberOfIndexes; i++)
        {
            // Generate a random index within the range of available ComponentIds
            int randomIndex = new Random().Next(0, SUITCommonInfo.ComponentIds.Count);

            var componentIndex = new SUITComponentIndex();
            indexes.Add(componentIndex.FromSUIT(randomIndex));
        }

        return indexes;
    }
}
public static class SUITRawDataSeeder
{
    public static List<SUITRaw> SeedSUITRawObjects(int numberOfObjects)
    {
        var rawObjects = new List<SUITRaw>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate some sample data (you can customize this part)
            var data = $"SampleData_{i}";

            var suitRaw = new SUITRaw(data);
            rawObjects.Add(suitRaw);
        }

        return rawObjects;
    }
}

public static class SUITNilDataSeeder
{
    public static List<SUITNil> SeedSUITNilObjects(int numberOfObjects)
    {
        var nilObjects = new List<SUITNil>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            var suitNil = new SUITNil();
            nilObjects.Add(suitNil);
        }

        return nilObjects;
    }
}
public static class SUITTStrDataSeeder
{
    public static List<SUITTStr> SeedSUITTStrObjects(int numberOfObjects)
    {
        var tstrObjects = new List<SUITTStr>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate some sample data (you can customize this part)
            var data = $"SampleData_{i}";

            var suitTStr = new SUITTStr(data);
            tstrObjects.Add(suitTStr);
        }

        return tstrObjects;
    }
}
public static class SUITComponentTextDataSeeder
{
    public static List<SUITComponentText> SeedSUITComponentTextObjects(int numberOfObjects)
    {
        var componentTextObjects = new List<SUITComponentText>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            var suitComponentText = new SUITComponentText
            {
                VendorName = new SUITTStr($"Vendor_{i}"),
                ModelName = new SUITTStr($"Model_{i}"),
                VendorDomain = new SUITTStr($"Domain_{i}"),
                ModelInfo = new SUITTStr($"ModelInfo_{i}"),
                ComponentDescription = new SUITTStr($"Description_{i}"),
                Version = new SUITTStr($"Version_{i}"),
                RequiredVersion = new SUITTStr($"RequiredVersion_{i}")
            };

            componentTextObjects.Add(suitComponentText);
        }

        return componentTextObjects;
    }
}
public static class SUITCompressionInfoDataSeeder
{
    public static SUITCompressionInfo SeedSUITCompressionInfo()
    {
        var compressionInfo = new SUITCompressionInfo();
        compressionInfo.AddCompressionMethod("gzip", 1);
        compressionInfo.AddCompressionMethod("bzip2", 2);
        compressionInfo.AddCompressionMethod("deflate", 3);
        compressionInfo.AddCompressionMethod("lz4", 4);
        compressionInfo.AddCompressionMethod("lzma", 7);

        return compressionInfo;
    }
}
public static class SUITIntDataSeeder
{
    public static List<SUITInt> SeedSUITInts(int numberOfInts)
    {
        var suintInts = new List<SUITInt>();
        var random = new Random();

        for (int i = 0; i < numberOfInts; i++)
        {
            int randomValue = random.Next(int.MinValue, int.MaxValue);
            var suintInt = new SUITInt { V = randomValue };
            suintInts.Add(suintInt);
        }

        return suintInts;
    }
}
public static class SUITPosIntDataSeeder
{
    public static List<SUITPosInt> SeedSUITPosInts(int numberOfPosInts)
    {
        var suintPosInts = new List<SUITPosInt>();
        var random = new Random();

        for (int i = 0; i < numberOfPosInts; i++)
        {
            int randomValue = random.Next(0, int.MaxValue);
            var suintPosInt = new SUITPosInt(randomValue);
            suintPosInts.Add(suintPosInt);
        }

        return suintPosInts;
    }
}






    public static class SUITSequenceDataSeeder
    {
        public static SUITSequence SeedSUITSequence(int numberOfItems, int numberOfCommandsPerItem)
        {
            var sequence = new SUITSequence();

            for (int i = 0; i < numberOfItems; i++)
            {
                var commands = SeedSUITCommands(numberOfCommandsPerItem);

                foreach (var suitCommand in commands)
                {
                    sequence.Items.Add(suitCommand); 
                }
            }

            return sequence;
        }

        public static List<SUITCommand> SeedSUITCommands(int numberOfCommands)
        {
            var commands = new List<SUITCommand>();

            for (int i = 0; i < numberOfCommands; i++)
            {
                var command = new SUITCommand();
                commands.Add(command);
            }

            return commands;
        }
    }


public static class SUITTryEachDataSeeder
{
    public static SUITTryEach SeedSUITTryEach(int numberOfItems)
    {
        var tryEach = new SUITTryEach();
        return tryEach;
    }
}

public class SUITTextDataSeeder
{
    public static SUITText SeedSUITText()
    {
        var suitText = new SUITText
        {
            mdesc = new SUITTStr { v = "Description" },
            udesc = new SUITTStr { v = "User Description" },
            json = new SUITTStr { v = "JSON Data" },
            yaml = new SUITTStr { v = "YAML Data" },
            components = new Dictionary<SUITComponentId, SUITComponentText>()
        };

        foreach (var seedSuitComponentTextObject in SUITComponentTextDataSeeder.SeedSUITComponentTextObjects(1))
        {
            var componentId = SUITComponentIdSeeder.GenerateRandomSUITComponentId();
            suitText.components.Add(componentId, seedSuitComponentTextObject);
        }

        return suitText;
    }
}

public class SUITManifestDataSeeder
{
    public static SUITManifest SeedSUITManifest()
    {

        var   version = new SUITPosInt { v = 1 };
        var   sequence = new SUITPosInt { v = 123 };
        var   common = new SUITBWrapField<SUITCommon>(SUITCommonDataSeeder.SeedSUITCommon());
        var   refuri = new SUITTStr { v = "https://example.com/manifest" };
        var   deres = new SUITSeverableField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var    fetch = new SUITSeverableField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var    install = new SUITSeverableField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var    validate = new SUITBWrapField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var    load = new SUITBWrapField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var   run = new SUITBWrapField<SUITSequenceComponentReset>(SUITSequenceComponentResetDataSeeder
            .SeedSUITSequenceComponentReset());
        var text = new SUITSeverableField<SUITText>(SUITTextDataSeeder.SeedSUITText());
           var coswid = new SUITBytes { v = new byte[] { 0x01, 0x02, 0x03 } };
           var manifest = new SUITManifest();
           manifest.common = common;
           manifest.version = version;

           manifest.sequence = sequence;
           manifest.refuri = refuri;
           manifest.deres = deres;
           manifest.fetch = fetch;
          manifest.install = install;
          manifest.validate = validate;
           manifest.load = load;
           manifest.run = run;
           manifest.text = text;
          manifest.coswid = coswid;


        

        return manifest;
    }
}

public class SUITEnvelopeSeeder
{
    public static SUITEnvelope GenerateSUITEnvelope()
    {
        var auth = COSEListDataSeeder.GenerateCOSEList(1);
        var manifest = SUITManifestDataSeeder.SeedSUITManifest();
        var envolope = new SUITEnvelope();
        
        envolope.Auth.v = auth;
        envolope.Manifest.v = manifest;
        return envolope;
    }
}
