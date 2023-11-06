using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using PeterO.Cbor;
using SuitSolution.Services; // Assuming you're using PeterO.Cbor for CBOR serialization

namespace SuitSolution.Tests
{
    public class ManifestCompilerTests
    {
        [Fact]
        public void TestCompileManifestToFile()
        {
            var options = new Dictionary<string, object>
            {
                { "log_level", "info" },
                { "action", "create" },
                { "input_file", "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/hwr-0_fwt-13_fwr-1.json" },
                { "output_file", "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/object.mani" },
                { "format", "suit" },
                { "severable", false },
                { "components", new List<object>() }
            };

            var m = new Dictionary<string, object>
            {
                { "manifest-version", 1 },
                { "manifest-sequence-number", 1 },
                { "components", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "install-id", new List<string> { "00" } },
                            { "vendor-id", "1b8ad47db3cc7eb2edb400c04fd430c8" },
                            { "class-id", "44e8bc11daa14ad1ddb400c04fd430c8" },
                            { "file", "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/firmware/0" },
                            { "uri", "coap://[fdea:dbee:f::1]/0" },
                            { "bootable", true },
                            { "offset", 16384 }
                        },
                        new Dictionary<string, object>
                        {
                            { "install-id", new List<string> { "00" } },
                            { "vendor-id", "1b8ad47db3cc7eb2edb400c04fd430c8" },
                            { "class-id", "44e8bc11daa14ad1ddb400c04fd430c8" },
                            { "file", "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/firmware/1" },
                            { "uri", "coap://[fdea:dbee:f::1]/1" },
                            { "bootable", true },
                            { "offset", 532480 }
                        }
                    }
                }
            };

            var compiler = new ManifestCompiler(options, m);

            var manifest = compiler.CompileManifest();
            Dictionary<int, object> cbor = manifest.ToSUIT();
            CBORObject cborObject = CBORObject.FromObject(cbor);
            byte[] cborData = cborObject.EncodeToBytes();

            var filePath = "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/object.cbor";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(cborData, 0, cborData.Length);
            }
// You can add additional assertions here to validate the saved CBOR data if needed
        }
      
    }

}
   
