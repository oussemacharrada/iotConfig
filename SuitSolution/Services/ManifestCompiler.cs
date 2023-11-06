using System.Security.Cryptography;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services;

public class ManifestCompiler
{   
    private readonly Dictionary<string, object> _options;
    private readonly Dictionary<string, object> _m;
            
    public ManifestCompiler(Dictionary<string, object> options, Dictionary<string, object> m)
    {
        this._options = options;
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
        List<Dictionary<string, object>> choices,
        SUITSequence seq,
        Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>> paramsFuncs,
        Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>> cmdsFuncs,
        string pcidKey = null,
        string paramDirective = "directive-set-parameters")
    {
        var (eqCmds, neqCmds) = CheckEq(cmdsFuncs, choices);
        var (eqParams, neqParams) = CheckEq(paramsFuncs, choices);

        SUITComponentId pcid = pcidKey == null ? cid : new SUITComponentId().FromJson(choices[0][pcidKey] as Dictionary<string, object>);

        var commonParams = new Dictionary<string, object>();
        foreach (var paramFunc in eqParams)
        {
            var (key, value) = paramFunc.Value(pcid, choices[0]);
            commonParams[key] = value;
        }

        if (commonParams.Any())
        {
            seq.Items.Add(MkCommand(pcid, paramDirective, commonParams));
        }

        var tryEachCmd = new SUITTryEach();
        foreach (var choice in choices)
        {
            var tecSeq = new SUITSequence();

            var tecParams = new Dictionary<string, object>();
            foreach (var paramFunc in neqParams)
            {
                var (key, value) = paramFunc.Value(pcid, choice);
                tecParams[key] = value;
            }

            var tecCmds = neqCmds.Select(cmdFunc => cmdFunc.Value(pcid, choice)).ToList();

            if (tecParams.Any())
            {
                tecSeq.Items.Add(MkCommand(pcid, paramDirective, tecParams));
            }

            tecSeq.Items.AddRange(tecCmds);

            if (tecSeq.Items.Any())
            {
                tryEachCmd.Items.Add(tecSeq);
            }
        }

        if (tryEachCmd.Items.Any())
        {
            seq.Items.Add(MkCommand(cid, "directive-try-each", tryEachCmd));
        }

        foreach (var cmdFunc in eqCmds)
        {
            seq.Items.Add(cmdFunc.Value(pcid, choices[0]));
        }

        return seq;
    }
    private static (Dictionary<string, T>, Dictionary<string, T>) CheckEq<T>(Dictionary<string, T> ids, List<Dictionary<string, object>> choices)
    {
        var eq = new Dictionary<string, T>();
        var neq = new Dictionary<string, T>();

        foreach (var key in ids.Keys)
        {
            var values = choices.Where(c => c.ContainsKey(key)).Select(c => c[key]).ToList();

            if (values.Count > 0)
            {
                if (values.Distinct().Count() == 1)
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

    private static SUITCommand MkCommand(SUITComponentId cid, string name, object arg)
    {
        object jarg;

        if (arg is ISUITConvertible jsonConvertible)
        {
            jarg = jsonConvertible.ToJson();
        }
        else
        {
            jarg = arg;
        }

        return new SUITCommand().FromJson(new Dictionary<string, object>
        {
            { "component-id", cid.ToJson() },
            { "command-id", name },
            { "command-arg", jarg }
        });
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

        foreach (var entry in cidData)
        {
            var cid = entry.Key;
            var choices = entry.Value;

            foreach (var choice in choices)
            {
                object GetValue(string key)
                {
                    if (choice.ContainsKey(key))
                    {
                        return choice[key];
                    }
                    return null;
                }

                var installDigest = GetValue("install-digest") as Dictionary<string, object>;
                if (installDigest != null)
                {
                    commonSeq.Items.Add(MkCommand(cid, "condition-image-match", installDigest));
                }

                var vendorId = GetValue("vendor-id");
                if (vendorId != null)
                {
                    var vendorIdArg = new Dictionary<string, object> { { "vendor-id", vendorId } };
                    commonSeq.Items.Add(MkCommand(cid, "condition-vendor-identifier", vendorIdArg));
                }

                var classId = GetValue("class-id");
                if (classId != null)
                {
                    var classIdArg = new Dictionary<string, object> { { "class-id", classId } };
                    commonSeq.Items.Add(MkCommand(cid, "condition-class-identifier", classIdArg));
                }

                var offsetValue = GetValue("offset");
                if (offsetValue != null)
                {
                    var offsetArg = new Dictionary<string, object> { { "offset", offsetValue } };
                    commonSeq.Items.Add(MkCommand(cid, "condition-component-offset", offsetArg));
                }
            }
        }

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

            installSeq = MakeSequence(cid, installableChoices, installSeq, installParams, installCmds);
            
            if (DepRequiredSequences.TryGetValue("install", out var sequence))
            {
                foreach (var cmd in sequence)
                {
                    installSeq.Items.Add(cmd);
                }
            }
            installSeq.Items.Add(MkCommand(cid, "directive-fetch", null));
            installSeq.Items.Add(MkCommand(cid, "condition-image-match", null));
        }
        else if (choices.Any(choice => choice.ContainsKey("uri")))
        {
            throw new NotImplementedException("Logic for choices containing 'uri' is not yet implemented.");
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

        if (choices.Any(choice => choice.ContainsKey("uri")))
        {
            var fetchParams = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>
            {
                { "uri", (cid, data) => new KeyValuePair<string, object>("uri", data["uri"]) },
                { "download-digest", (cid, data) => new KeyValuePair<string, object>("image-digest", data.TryGetValue("download-digest", out var digest) ? digest : data["install-digest"]) },
                { "offset", (cid, data) => new KeyValuePair<string, object>("offset", data["offset"]) }
            };

            if (choices.Any(choice => choice.ContainsKey("compression-info") && (!choice.TryGetValue("decompress-on-load", out var decompress) || !(bool)decompress)))
            {
                fetchParams["compression-info"] = (cid, data) => new KeyValuePair<string, object>("compression-info", data["compression-info"]);
            }

            var fetchCmds = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>
            {
                { "condition-component-offset", (cid, data) => MkCommand(cid, "condition-component-offset", new Dictionary<string, object> { { "offset", data["offset"] } }) }
            };

            fetchSeq = MakeSequence(cid, choices, fetchSeq, fetchParams, fetchCmds);

            if (choices.First().TryGetValue("download-id", out var downloadIdValue))
            {
                var did = new SUITComponentId().FromJson(downloadIdValue as Dictionary<string, object>);
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

    /* public SUITSequence ConstructRunSequence()
     {
         var cidData = GetCidData(GetComponentIds());
         var runSeq = new SUITSequence();
 
         var runParamsFuncs = new Dictionary<string, Func<string, Dictionary<string, object>, KeyValuePair<string, object>>>
         {
             { "component-index", (cid, choice) => 
                 choice.TryGetValue("component-index", out var componentIndex) 
                     ? new KeyValuePair<string, object>("index", componentIndex) 
                     : new KeyValuePair<string, object>("index", "Not Found") }
         };
 
         var runCmdsFuncs = new Dictionary<string, Func<string, Dictionary<string, object>, SUITCommandContainer.SUITCmd>>
         {
             { "run", (cid, choice) => 
                 choice.TryGetValue("run", out var runValue) 
                     ? MkCommand(new SUITComponentId(cid), "directive-run", new Dictionary<string, object> { { "run", runValue } }) 
                     : null },
             { "wait", (cid, choice) => 
                 choice.TryGetValue("wait", out var waitValue) 
                     ? MkCommand(new SUITComponentId(cid), "directive-wait", new Dictionary<string, object> { { "wait", waitValue } }) 
                     : null }
         };
 
         foreach (var entry in cidData)
         {
             var cid = entry.Key;
             var choices = entry.Value;
             runSeq = MakeSequence(cid, choices, runSeq, runParamsFuncs, runCmdsFuncs);
         }
 
         runSeq.Items = runSeq.Items.Where(item => item != null).ToList();
 
         return runSeq;
     }*/

public SUITSequence ConstructValidateSequence()
{
    var cidData = GetCidData(GetComponentIds());
    var validateSeq = new SUITSequence();

    foreach (var entry in cidData)
    {
        var cid = entry.Key;
        var choices = entry.Value;

        // Define parameter functions if any parameters are common across choices
        var paramsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>();

        // Define command functions based on the choices
        var cmdsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>();

        // Add command functions for "install-digest", "vendor-id", and "class-id" if they exist in choices
        if (choices.Any(choice => choice.ContainsKey("install-digest")))
        {
            cmdsFuncs["install-digest"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-image-match", new Dictionary<string, object> { { "digest", data["install-digest"] } });
            };
        }

        if (choices.Any(choice => choice.ContainsKey("vendor-id")))
        {
            cmdsFuncs["vendor-id"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-vendor-identifier", new Dictionary<string, object> { { "vendor-id", data["vendor-id"] } });
            };
        }

        if (choices.Any(choice => choice.ContainsKey("class-id")))
        {
            cmdsFuncs["class-id"] = (componentId, data) =>
            {
                return MkCommand(componentId, "condition-class-identifier", new Dictionary<string, object> { { "class-id", data["class-id"] } });
            };
        }

        // Construct the sequence for this component ID
        validateSeq = MakeSequence(cid, choices, validateSeq, paramsFuncs, cmdsFuncs);
    }

    // Add any dependent required sequences for "validate"
    if (DepRequiredSequences.TryGetValue("validate", out var sequence))
    {
        foreach (var cmd in sequence)
        {
            validateSeq.Items.Add(cmd);
        }
    }

    // Add a final "condition-image-match" command for the last component ID
    // Assuming cidData has at least one entry and GetComponentIds returns the component IDs in the same order
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
        var choices = entry.Value;

        // Define parameter functions based on the choices
        var paramsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, KeyValuePair<string, object>>>();

        // Define command functions based on the choices
        var cmdsFuncs = new Dictionary<string, Func<SUITComponentId, Dictionary<string, object>, SUITCommand>>();

        // Add parameter and command functions for "load-id", "load-digest", "load-size", and "compression-info" if they exist in choices
        if (choices.Any(choice => choice.ContainsKey("load-id")))
        {
            paramsFuncs["load-id"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("source-component", data["load-id"]);
            };
        }
        if (choices.Any(choice => choice.ContainsKey("load-digest")))
        {
            paramsFuncs["load-digest"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("image-digest", data.ContainsKey("load-digest") ? data["load-digest"] : data["install-digest"]);
            };
        }
        if (choices.Any(choice => choice.ContainsKey("load-size")))
        {
            paramsFuncs["load-size"] = (componentId, data) =>
            {
                return new KeyValuePair<string, object>("image-size", data.ContainsKey("load-size") ? data["load-size"] : data["install-size"]);
            };
        }
        if (choices.Any(choice => choice.ContainsKey("compression-info") && choice.ContainsKey("decompress-on-load") && (bool)choice["decompress-on-load"]))
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
        var optionsComponents = _options.ContainsKey("components") ? _options["components"] as List<Dictionary<string, object>> : null;
        if (optionsComponents == null)
        {
            optionsComponents = new List<Dictionary<string, object>>();
        }
        _m["components"] = mComponents.Concat(optionsComponents).ToList();

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

        var commonSeq = ConstructCommonSequence(cidData);
        var depSeq = ConstructDependenciesSequence(cidData);
        var instSeq = ConstructCommonSequence(cidData);
        var fetchSeq = ConstructFetchSequence(cidData);
        var validateseq = ConstructCommonSequence(cidData);
        var loadseq = ConstructLoadSequence();
        var runseq = fetchSeq;
        var installreset = new SUITSequenceComponentReset(instSeq);

        var install = new SUITSeverableField<SUITSequenceComponentReset>
        {
            v = installreset
        };

        var validatereset = new SUITSequenceComponentReset(validateseq);

        var validate = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = validatereset
        };

        var loadreset = new SUITSequenceComponentReset(loadseq);

        var load = new SUITBWrapField<SUITSequenceComponentReset>
        {
            v = loadreset
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
        var auth = COSEListDataSeeder.GenerateCOSEList(1);
        var newauth = new SUITBWrapField<COSEList>
        {
            v = auth
        };
        var newtext = SUITTextDataSeeder.SeedSUITText();
        var textbwrap = new SUITSeverableField<SUITText>
        {
            v = newtext
        };
        var commonBWrap = new SUITBWrapField<SUITCommon>
        {
            v = new SUITCommon(commonSeq)
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
                    deres = deres,
                    fetch = fetech,
                    install = install,
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