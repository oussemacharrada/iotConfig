using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services;

public class ManifestCompiler
{   
    private readonly Dictionary<string, object> _m;
            
    public ManifestCompiler( Dictionary<string, object> m)
    {
        this._m = m;
    }
    private Dictionary<string, SUITComponentId> GetComponentIds()
    {
        var ids = new Dictionary<string, SUITComponentId>();
        foreach (var c in (List<Dictionary<string, object>>)_m["components"])
        {
            foreach (var f in new[] { "install-id", "download-id", "load-id" })
            {
                if (c.TryGetValue(f, out var value))
                {
                    var idData = new Dictionary<string, object>
                    {
                        { "component_id", value }
                    };
                    var id = new SUITComponentId().FromJson(idData);
                    ids[id.ToString()] = id;
                }
            }
        }
        return ids;
    }
    private Dictionary<SUITComponentId, List<Dictionary<string, object>>> GetCidData(Dictionary<string, SUITComponentId> ids)
    {
        var cidData = new Dictionary<SUITComponentId, List<Dictionary<string, object>>>();
        foreach (var c in (List<Dictionary<string, object>>)_m["components"])
        {
            if (!c.ContainsKey("install-id"))
            {
                throw new Exception("install-id required for all components");
            }

            var idData = new Dictionary<string, object>
            {
                { "component_id", c["install-id"] }
            };
            var cid = new SUITComponentId().FromJson(idData);

            if (!cidData.ContainsKey(cid))
            {
                cidData[cid] = new List<Dictionary<string, object>>();
            }
                
            cidData[cid].Add(c);
        }
        return cidData;
    }
    public (byte[] Digest, long ImageSize) HashFile(string filePath, HashAlgorithmName algorithmName)
    {
        long imageSize = 0;
        using (var algorithm = HashAlgorithm.Create(algorithmName.Name))
        using (FileStream stream = File.OpenRead(filePath))
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                imageSize += bytesRead;
                algorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
            }
            algorithm?.TransformFinalBlock(buffer, 0, 0);
            byte[] hash = algorithm.Hash;
            return (hash, imageSize);
        }
    }
    public static SUITSequence MakeSequence(
        SUITComponentId cid,
        List<object> choices, // Remains List<object>
        SUITSequence seq,
        Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>> paramsFuncs,
        Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> cmdsFuncs,
        string pcidKey = null,
        string paramDirective = "directive-set-parameters")
    {
        var flatChoices = new List<Dictionary<string, object>>();

        foreach (var choice in choices)
        {
            if (choice is List<Dictionary<string, object>> list)
            {
                flatChoices.AddRange(list);
            }
            else if (choice is Dictionary<string, object> dict)
            {
                flatChoices.Add(dict);
            }
        }
        SUITComponentId pcid = GetParentComponentId(cid, flatChoices, pcidKey);
        var (eqCmds, neqCmds) = CheckEq(cmdsFuncs, flatChoices, pcid);
        var (eqParams, neqParams) = CheckEq(paramsFuncs, flatChoices, pcid);

        // Add common commands and parameters
          AddCommonParameters(seq, eqParams, pcid, flatChoices, paramDirective); 
        AddEqualCommands(seq, eqCmds, pcid, flatChoices);

        // Add unique commands and parameters for each choice
          AddTryEachCommands(seq, neqParams, neqCmds, pcid, flatChoices, paramDirective);

        return seq;
    }


private static SUITComponentId GetParentComponentId(SUITComponentId cid, List<Dictionary<string, object>> choices, string pcidKey)
{
    return pcidKey == null ? cid : new SUITComponentId().FromJson(choices[0][pcidKey] as Dictionary<string, object>);
}

private static void AddCommonParameters(
    SUITSequence seq, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>> eqParams, 
    SUITComponentId pcid, 
    List<Dictionary<string, object>> choices, 
    string paramDirective)
{
    var commonParams = new Dictionary<string, object>();

    foreach (var paramFunc in eqParams)
    {
        var (key, value) = paramFunc.Value(pcid, choices[0]);

        // Check if the parameter is relevant for the current command
        if (choices[0].ContainsKey(key))
        {
            commonParams[key] = value;
        }
    }

    if (commonParams.Any())
    {
        seq.Items.Add(MkCommand(pcid, paramDirective, commonParams));
    }
}


private static bool IsParamRelevantForCommand(string paramKey, 
    Dictionary<string, List<string>> commandParamMapping, 
    string commandKey)
{
    if (commandParamMapping.TryGetValue(commandKey, out var relevantParams))
    {
        return relevantParams.Contains(paramKey);
    }
    return false;
}
private static void AddTryEachCommands(SUITSequence seq, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>> neqParams, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> neqCmds, 
    SUITComponentId pcid, 
    List<Dictionary<string, object>> choices, 
    string paramDirective)
{
    var tryEachCmd = new SUITTryEach();
    SUITSequence reportingPolicySeq = null;

    foreach (var choice in choices)
    {
        var tecSeq = new SUITSequence();
        AddNonEqualParamsAndCommands(tecSeq, neqParams, neqCmds, pcid, choice, paramDirective, ref reportingPolicySeq);

        if (tecSeq.Items.Any())
        {
            tryEachCmd.Items.Add(tecSeq);
        }
    }

    // Add reporting policy commands at the end if they exist
    if (reportingPolicySeq != null && reportingPolicySeq.Items.Any())
    {
        tryEachCmd.Items.Add(reportingPolicySeq);
    }

    if (tryEachCmd.Items.Any())
    {
        seq.Items.Add(MkCommand(pcid, "directive-try-each", tryEachCmd));
    }
}



private static void AddNonEqualParamsAndCommands(SUITSequence tecSeq, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>> neqParams, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> neqCmds, 
    SUITComponentId pcid, 
    Dictionary<string, object> choice, 
    string paramDirective,
    ref SUITSequence reportingPolicySeq)
{
    // Process non-equal parameters
    foreach (var paramFunc in neqParams)
    {
        var (key, value) = paramFunc.Value(pcid, choice);
        if (choice.ContainsKey(key))
        {
            var singleParam = new Dictionary<string, object> { { key, value } };
            tecSeq.Items.Add(MkCommand(pcid, paramDirective, singleParam));
        }
    }

    // Process non-equal commands excluding reporting policy commands
    foreach (var cmdFunc in neqCmds)
    {
        if (!IsReportingPolicyCommand(cmdFunc.Key))
        {
            var cmd = cmdFunc.Value(pcid, choice);
            tecSeq.Items.Add(cmd);
        }
    }

    // Add reporting policy commands at the end
    if (reportingPolicySeq == null)
    {
        reportingPolicySeq = new SUITSequence();
        AddReportingPolicyCommands(reportingPolicySeq, neqCmds, pcid, choice);
    }}

private static bool IsReportingPolicyCommand(string commandKey)
{
    return commandKey == "condition-component-offset" || 
           commandKey == "condition-vendor-identifier" || 
           commandKey == "condition-class-identifier";
}

private static void AddReportingPolicyCommands(SUITSequence tecSeq, 
    Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> neqCmds, 
    SUITComponentId pcid, 
    Dictionary<string, object> choice)
{
    var reportingPolicyKeys = new List<string> { "condition-component-offset", "condition-vendor-identifier", "condition-class-identifier" };
    foreach (var key in reportingPolicyKeys)
    {
        if (neqCmds.TryGetValue(key, out var cmdFunc))
        {
            var cmd = cmdFunc(pcid, choice);
            tecSeq.Items.Add(cmd);
        }
    }
}



private static void AddEqualCommands(SUITSequence seq, Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> eqCmds, SUITComponentId pcid, List<Dictionary<string, object>> choices)
{
    foreach (var cmdFunc in eqCmds)
    {
        seq.Items.Add(cmdFunc.Value(pcid, choices[0]));
    }
}


  
    private static SUITCommand MkCommand(SUITComponentId cid, string name, object arg)
    {
        object jarg;

        if (arg is ISUITConvertible jsonConvertible)
        {
            jarg = jsonConvertible.ToJson();
        }
        else if (arg != null)
        {
            jarg = arg;
        }
        else
        {
            jarg = new Dictionary<string, object>(); // Create an empty dictionary if arg is null
        }
      
        var cidjson = cid.ToJson();
        var SuitCommand =new  SUITCommand().FromJson(new Dictionary<string, object>
      {
          { "component-id", cid.ToJson() },
          { "command-id", name },
          { "command-arg", jarg }
      });
      return SuitCommand;
    }
    private static (Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, T>>, 
        Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, T>>) 
        CheckEq<T>(Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, T>> ids, 
            List<Dictionary<string, object>> choices,
            SUITComponentId pcid)
    {
        var eq = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, T>>();
        var neq = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, T>>();

        // If there's only one choice, treat all keys as not equal
        if (choices.Count == 1)
        {
            foreach (var key in ids.Keys)
            {
                neq[key] = ids[key];
            }
        }
        else
        {
            foreach (var key in ids.Keys)
            {
                var values = choices.Select(choice => ids[key](pcid, choice)).ToList();

                if (values.All(v => EqualityComparer<T>.Default.Equals(v, values[0])))
                {
                    eq[key] = ids[key];
                }
                else
                {
                    neq[key] = ids[key];
                }
            }
        }

        return (eq, neq);
    }







    private string RunnableId(Dictionary<string, object> component)
    {
        if (component.ContainsKey("bootable") && (bool)component["bootable"])
        {
            return component["install-id"].ToString();
        }
        return null;
    }
    private SUITSequence ConstructCommonSequence(Dictionary<SUITComponentId, List<Dictionary<string, object>>> cidData)
    {
         var commonSeq = new SUITSequence();

         var commonCmds = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
         {
             { "condition-component-offset", (cid, data) => MkCommand(cid, "condition-component-offset", data.ContainsKey("offset") ? data["offset"] : null) },
             { "condition-vendor-identifier", (cid, data) => MkCommand(cid, "condition-vendor-identifier", data.ContainsKey("vendor-id") ? new Dictionary<string, object> { { "vendor-id", data["vendor-id"] } } : null) },
             { "condition-class-identifier", (cid, data) => MkCommand(cid, "condition-class-identifier", data.ContainsKey("class-id") ? new Dictionary<string, object> { { "class-id", data["class-id"] } } : null) }
         };


            var commonParams = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
            {
                { "install-digest", (cid, data) => new KeyValuePair<string, object>("install-digest", data["install-digest"]) },
                { "install-size", (cid, data) => new KeyValuePair<string, object>("install-size", data["install-size"]) },
                { "vendor-id", (cid, data) => new KeyValuePair<string, object>("vendor-id", data["vendor-id"]) },
                { "class-id", (cid, data) => new KeyValuePair<string, object>("class-id", data["class-id"]) },
                { "offset", (cid, data) => new KeyValuePair<string, object>("offset", data["offset"]) }
            };
            var choices = new List<object>();
            var cid = new SUITComponentId();
            foreach (var entry in cidData)
            {
                 cid = entry.Key;
                choices.Add(entry.Value); 

            }

            commonSeq = MakeSequence(cid, choices, commonSeq, commonParams, commonCmds,null,
                "directive-override-parameters");


            return commonSeq;
        
    }

    private SUITSequence ConstructDependenciesSequence(Dictionary<SUITComponentId, List<Dictionary<string, object>>> cidData)
    {
        var dependenciesSeq = new SUITSequence();

        foreach (var entry in cidData)
        {
            var cid = entry.Key;
            var choices = entry.Value;

            foreach (var choice in choices)
            {
                if (choice.ContainsKey("dependency-index"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "directive-set-dependency-index", new Dictionary<string, object> { { "index", choice["dependency-index"] } }));
                }

                if (choice.ContainsKey("process-dependency"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "directive-process-dependency", new Dictionary<string, object> { { "process", choice["process-dependency"] } }));
                }

                if (choice.ContainsKey("dependency-digest"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "condition-dependency-digest", new Dictionary<string, object> { { "digest", choice["dependency-digest"] } }));
                }

                if (choice.ContainsKey("dependency-size"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "condition-dependency-size", new Dictionary<string, object> { { "size", choice["dependency-size"] } }));
                }

                if (choice.ContainsKey("dependency-vendor-id"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "condition-dependency-vendor-id", new Dictionary<string, object> { { "vendor-id", choice["dependency-vendor-id"] } }));
                }

                if (choice.ContainsKey("dependency-class-id"))
                {
                    dependenciesSeq.Items.Add(MkCommand(cid, "condition-dependency-class-id", new Dictionary<string, object> { { "class-id", choice["dependency-class-id"] } }));
                }

            }
        }

        return dependenciesSeq;
    }
    private SUITComponentId GetSuitComponentIdForDigest(SUITDigest digest)
    {
        var digestSuitBytes = digest.DigestBytes; 

        return new SUITComponentId(new List<SUITBytes> { digestSuitBytes });
    }
    private SUITSequence ConstructDependencySequence(Dictionary<string, object> manifestData)
    {
        var depSeq = new SUITSequence();
        var dependencies = new SUITDependencies();
        var depRequiredSequences = new Dictionary<string, List<SUITCommand>>
        {
            { "deres", new List<SUITCommand>() },
            { "fetch", new List<SUITCommand>() },
            { "install", new List<SUITCommand>() },
            { "validate", new List<SUITCommand>() },
            { "run", new List<SUITCommand>() },
            { "load", new List<SUITCommand>() }
        };
        if (manifestData.ContainsKey("dependencies"))
        {
            foreach (var dep in manifestData["dependencies"] as List<Dictionary<string, object>>)
            {
                CBORObject manifest = null;
                var sha256 = SHA256.Create();
                using (var depFd = new FileStream(dep["file"].ToString(), FileMode.Open))
                {
                    byte[] depBytes = File.ReadAllBytes(dep["file"].ToString());
                    var depEnvelope = CBORObject.DecodeFromBytes(depBytes);
                    var manifestCbor = depEnvelope[SUITEnvelope.fields["manifest"].SuitKey];
                    byte[] manifestBytes = manifestCbor.GetByteString();
                    byte[] hash = sha256.ComputeHash(manifestBytes);
            
                    manifest = CBORObject.DecodeFromBytes(manifestBytes);
                }
                var digestData = new Dictionary<string, object>
                {
                    { "algo", "sha256" },
                    { "digest", BitConverter.ToString(sha256.Hash).Replace("-", "").ToLower() }
                };

                var digest = new SUITDigest().FromJson(digestData);

                dependencies.Add(new SUITDependency
                {
                    DependencyDigest = digest
                });

                SUITComponentId cid = GetSuitComponentIdForDigest(digest);

                if (dep.ContainsKey("uri"))
                {
                    depSeq.Items.Add(MkCommand(cid, "directive-set-parameters", new Dictionary<string, object> { { "uri", dep["uri"] } }));
                    depSeq.Items.Add(MkCommand(cid, "directive-fetch", null));
                    depSeq.Items.Add(MkCommand(cid, "condition-image-match", null));
                }

                foreach (var key in depRequiredSequences.Keys)
                {
                    if (manifest.ContainsKey(SUITManifest.fields[key].SuitKey))
                    {
                        depRequiredSequences[key].Add(MkCommand(cid, "directive-process-dependency", null));
                    }
                }
            }
        }
        return depSeq;
    }
    private Dictionary<string, List<SUITCommand>> DepRequiredSequences = new Dictionary<string, List<SUITCommand>>();
    
private SUITSequence ConstructInstallSequence(Dictionary<SUITComponentId, List<Dictionary<string, object>>> cidData)
{
    var installSeq = new SUITSequence();

    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value;

        var installableChoices = choices.Where(choice => choice.ContainsKey("uri") && (choice.ContainsKey("install-on-download") ? (bool)choice["install-on-download"] : true)).ToList();

        if (installableChoices.Any())
        {
            var installParams = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
            {
                { "uri", (cid, data) => new KeyValuePair<string, object>("uri", data["uri"]) },
                { "offset", (cid, data) => new KeyValuePair<string, object>("offset", data["offset"]) }
            };

            if (installableChoices.Any(choice => choice.ContainsKey("compression-info") && (!choice.ContainsKey("decompress-on-load") || !(bool)choice["decompress-on-load"])))
            {
                installParams["compression-info"] = (cid, data) => new KeyValuePair<string, object>("compression-info", data["compression-info"]);
            }

            var installCmds = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
            {
                { "condition-component-offset", (cid, data) => MkCommand(cid, "condition-component-offset", data["offset"]) }
            };

            // Add dependency required sequences if any
            if (DepRequiredSequences.TryGetValue("install", out var depSequence))
            {
                foreach (var cmd in depSequence)
                {
                    installSeq.Items.Add(cmd);
                }
            }

            // Construct the sequence with parameters and commands
            installSeq = MakeSequence(cid, installableChoices.Cast<object>().ToList(), installSeq, installParams, installCmds);
        
            // Add fetch and image match commands
            installSeq.Items.Add(MkCommand(cid, "directive-fetch", null));
            installSeq.Items.Add(MkCommand(cid, "condition-image-match", null));
        }
        else if (choices.Any(choice => choice.ContainsKey("uri")))
        {
            // Handle cases where 'uri' is present but not installable
            var fetchChoices = choices.Where(choice => choice.ContainsKey("uri")).ToList();
            var fetchParams = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
            {
                { "uri", (cid, data) => new KeyValuePair<string, object>("uri", data["uri"]) }
            };

            var fetchCmds = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
            {
                { "directive-fetch", (cid, data) => MkCommand(cid, "directive-fetch", null) },
                { "condition-image-match", (cid, data) => MkCommand(cid, "condition-image-match", null) }
            };

            // Construct the sequence for fetching
            installSeq = MakeSequence(cid, fetchChoices.Cast<object>().ToList(), installSeq, fetchParams, fetchCmds);
        }
    }

    return installSeq;
}



private SUITSequence ConstructFetchSequence(Dictionary<SUITComponentId, List<Dictionary<string, object>>> cidData)
{
    var fetchSeq = new SUITSequence();

    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value;

        // Process choices that contain a URI
        if (choices.Any(choice => choice.ContainsKey("uri")))
        {
            // Define fetch parameters
            var fetchParams = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
            {
                { "uri", (c, data) => new KeyValuePair<string, object>("uri", data["uri"]) },
                { "download-digest", (c, data) => new KeyValuePair<string, object>("image-digest", data.TryGetValue("download-digest", out var digest) ? digest : data["install-digest"]) },
                { "offset", (c, data) => new KeyValuePair<string, object>("offset", data["offset"]) }
            };

            // Add compression-info if needed
            if (choices.Any(choice => choice.ContainsKey("compression-info") && (!choice.TryGetValue("decompress-on-load", out var decompress) || !(bool)decompress)))
            {
                fetchParams["compression-info"] = (c, data) => new KeyValuePair<string, object>("compression-info", data["compression-info"]);
            }

            // Define fetch commands
            var fetchCmds = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
            {
                { "condition-component-offset", (c, data) => MkCommand(c, "condition-component-offset", new Dictionary<string, object> { { "offset", data["offset"] } }) }
            };

            
            // Process download-id if present
            if (choices.First().TryGetValue("download-id", out var downloadIdValue) && downloadIdValue is Dictionary<string, object> downloadIdDict)
            {
                var did = new SUITComponentId().FromJson(downloadIdDict);
                if (DepRequiredSequences.TryGetValue("fetch", out var sequence))
                {
                    foreach (var cmd in sequence)
                    {
                        fetchSeq.Items.Add(cmd);
                    }
                }
                fetchSeq.Items.Add(MkCommand(did, "directive-fetch", null));
                fetchSeq.Items.Add(MkCommand(did, "condition-image-match", null));
            }
        }
    }

    return fetchSeq;
}
public SUITSequence ConstructRunSequence()
{
    // Retrieve component data which is a dictionary of component IDs and their choices
    var cidData = GetCidData(GetComponentIds());

    // Initialize the sequence for the 'run' commands
    var runSeq = new SUITSequence();

    // Define parameter functions for the 'run' sequence
    var runParamsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
    {
        { "component-index", (cid, choice) => 
            choice.TryGetValue("component-index", out var componentIndex) 
                ? new KeyValuePair<string, object>("index", componentIndex) 
                : throw new KeyNotFoundException("The 'component-index' key is not found in the choice dictionary.") }
    };

    // Define command functions for the 'run' sequence
    var runCmdsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
    {
        { "run", (cid, choice) => 
            choice.TryGetValue("run", out var runValue) 
                ? MkCommand(cid, "directive-run", new Dictionary<string, object> { { "run", runValue } }) 
                : throw new KeyNotFoundException("The 'run' key is not found in the choice dictionary.") },
        { "wait", (cid, choice) => 
            choice.TryGetValue("wait", out var waitValue) 
                ? MkCommand(cid, "directive-wait", new Dictionary<string, object> { { "wait", waitValue } }) 
                : throw new KeyNotFoundException("The 'wait' key is not found in the choice dictionary.") }
    };

    // Iterate over each component ID and its associated choices to build the sequence
    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value;

        // Assuming MakeSequence is a method that processes the choices and adds appropriate commands to the sequence
        // runSeq = MakeSequence(cid, choices, runSeq, runParamsFuncs, runCmdsFuncs);
    }

    // Filter out any null items that may have been added to the sequence
    runSeq.Items = runSeq.Items.Where(item => item != null).ToList();

    return runSeq;
}

public SUITSequence ConstructValidateSequence()
{
    var cidData = GetCidData(GetComponentIds());
    var validateSeq = new SUITSequence();

    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value.Cast<object>().ToList(); // Cast choices to List<object>

        var paramsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>();

        var cmdsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>();

        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("install-digest")))
        {
            cmdsFuncs["install-digest"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-image-match", new Dictionary<string, object> { { "digest", data["install-digest"] } });
            };
        }

        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("vendor-id")))
        {
            cmdsFuncs["vendor-id"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-vendor-identifier", new Dictionary<string, object> { { "vendor-id", data["vendor-id"] } });
            };
        }

        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("class-id")))
        {
            cmdsFuncs["class-id"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-class-identifier", new Dictionary<string, object> { { "class-id", data["class-id"] } });
            };
        }

        validateSeq = MakeSequence(cid, choices, validateSeq, paramsFuncs, cmdsFuncs);
    }

    if (DepRequiredSequences.TryGetValue("validate", out var sequence))
    {
        foreach (var cmd in sequence)
        {
            validateSeq.Items.Add(cmd);
        }
    }

    var lastCid = cidData.Last().Key;
    validateSeq.Items.Add(MkCommand(lastCid, "condition-image-match", null));

    return validateSeq;
}

public SUITSequence ConstructLoadSequence()
{
    var cidData = GetCidData(GetComponentIds());
    var loadSeq = new SUITSequence();

    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value.Cast<object>().ToList(); // Cast choices to List<object>

        var paramsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>();
        var cmdsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>();

        // Define parameter functions based on the choices
        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("load-id")))
        {
            paramsFuncs["load-id"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("source-component", data["load-id"]);
            };
        }
        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("load-digest")))
        {
            paramsFuncs["load-digest"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("image-digest", data.ContainsKey("load-digest") ? data["load-digest"] : data["install-digest"]);
            };
        }
        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("load-size")))
        {
            paramsFuncs["load-size"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("image-size", data.ContainsKey("load-size") ? data["load-size"] : data["install-size"]);
            };
        }
        if (choices.Any(choice => choice is Dictionary<string, object> dict && dict.ContainsKey("compression-info") && dict.ContainsKey("decompress-on-load") && (bool)dict["decompress-on-load"]))
        {
            paramsFuncs["compression-info"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("compression-info", data["compression-info"]);
            };
        }

        // Construct the sequence for this component ID
        loadSeq = MakeSequence(cid, choices, loadSeq, paramsFuncs, cmdsFuncs);
    }

    // Add any dependent required sequences for "load"
    if (DepRequiredSequences.TryGetValue("load", out var sequence))
    {
        foreach (var cmd in sequence)
        {
            loadSeq.Items.Add(cmd);
        }
    }

    return loadSeq;
}


    public SUITEnvelope CompileManifest()
    {
        var mComponents = _m["components"] as List<Dictionary<string, object>> ?? new List<Dictionary<string, object>>();
      

        var ids = GetComponentIds();
        var cidData = GetCidData(ids);
        foreach (var entry in cidData)
        {
            var cid = entry.Key;
            var components = entry.Value;

            foreach (var component in components)
            {
                if (component.ContainsKey("file"))
                {
                    var (digest, imgsize) = HashFile((string)component["file"], new HashAlgorithmName("SHA256"));
                    component["install-digest"] = new Dictionary<string, object>
                    {
                        { "algorithm-id", "sha256" },
                        { "digest-bytes", digest }
                    };
                    component["install-size"] = imgsize;
                }
            }
        }
        bool hasVendorId = ((List<Dictionary<string, object>>)_m["components"]).Any(c => c.ContainsKey("vendor-id"));
        bool hasClassId = ((List<Dictionary<string, object>>)_m["components"]).Any(c => c.ContainsKey("vendor-id") && c.ContainsKey("class-id"));

        if (!hasVendorId)
        {
            throw new Exception("A vendor-id is required for at least one component");
        }

        if (!hasClassId)
        {
            throw new Exception("A class-id is required for at least one component that also has a vendor-id");
        }
        var componentIds = cidData.Keys.ToList();
       
        var commonSeq = ConstructCommonSequence(cidData);
        var depSeq = ConstructDependenciesSequence(cidData);
        var instSeq = ConstructInstallSequence(cidData);
        var fetchSeq = ConstructFetchSequence(cidData);
        var validateseq = ConstructValidateSequence();
        var loadseq = ConstructLoadSequence();
        var runseq = fetchSeq;
        var installreset = new SUITSequenceComponentReset(instSeq);

        var install = new SUITSeverableField<SUITSequenceComponentReset>
        {
            v = new SUITSequenceComponentReset()
        };

        var validatereset = new SUITSequenceComponentReset(validateseq);

        var validate = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = validatereset
        };

        var loadreset = new SUITSequenceComponentReset(loadseq);

        var load = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = new SUITSequenceComponentReset()
        };
        var runreset = new SUITSequenceComponentReset(runseq);

        var run = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = runreset
        };
        var deresreset = new SUITSequenceComponentReset(depSeq);

        var deres = new SUITSeverableField<SUITSequenceComponentReset>
        {
            v = deresreset
        };
        var fetchreset = new SUITSequenceComponentReset(fetchSeq);

        var fetech = new SUITSeverableField<SUITSequenceComponentReset>
        {
            v = fetchreset
        };

        var newtext = new SUITText();
        var textbwrap = new SUITSeverableField<SUITText>
        {
            v = newtext
        };
   

 var commonJsonData = new Dictionary<string, object>
 {
     { "components", componentIds.Select(cid => cid != null ? cid : null).ToList() },
     { "common-sequence", commonSeq.ToJson() }
     // Add "dependencies" if needed
 };


 var common = new SUITCommon(componentIds,commonSeq);


        var commonBWrap = new SUITBWrapField<SUITCommon>
        {
            v = common
        };

        var auth = new COSEList();
 var newauth = new SUITBWrapField<COSEList>
 {
     v = auth
 };
        var wrappedManifest = new SUITEnvelope
        { 
            Auth =newauth ,
            Manifest = new SUITBWrapField<SUITManifest>
            {
                    
                v = new SUITManifest
                {
                    version =  new SUITPosInt(1) ,
                    sequence = new SUITPosInt(1) ,
                    common = commonBWrap,
                    install = install,
                    deres = deres,
                    fetch = fetech,
                 
                    load = load,
                    run = run,
                    validate = validate,
                    text = textbwrap

                }
            }
        };

        return wrappedManifest;
    }
}