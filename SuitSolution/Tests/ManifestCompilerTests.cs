/*using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using YourNamespace; 

public class ManifestCompilerTests : IDisposable
{
    private readonly string tempInputFilePath;
    private readonly string tempOutputFilePath;
    private Options options; // Move the Options variable to a higher scope

    public ManifestCompilerTests()
    {
        // Create temporary input and output files for testing
        tempInputFilePath = Path.GetTempFileName();
        tempOutputFilePath = Path.GetTempFileName();
    }

    public void Dispose()
    {
        // Clean up temporary files after testing
        File.Delete(tempInputFilePath);
        File.Delete(tempOutputFilePath);
    }

    [Fact]
    public void TestManifestCompilation()
    {
        try
        {
            // Create a sample JSON input
            string jsonInput = @"
                {
                    ""manifest-version"": ""1"",
                    ""manifest-sequence-number"": ""123"",
                    ""severable"": false,
                    ""components"": [
                        {
                            ""install-id"": ""component-1"",
                            ""vendor-id"": ""vendor-1"",
                            ""class-id"": ""class-1"",
                             ""file"", ""C:\\Users\\ousama.charada\\OneDrive - ML!PA Consulting GmbH\\Desktop\\suit\\suit-manifest-generator\\TestCase\\firmware\\d.txt"" }
                        }
                     
                    ]
                }";

            // Write the sample JSON input to the temporary input file
            File.WriteAllText(tempInputFilePath, jsonInput);

            // Initialize options for testing
            options = new Options // Assign options in the higher scope
            {
                InputFile = File.OpenRead(tempInputFilePath),
                OutputFile = File.OpenWrite(tempOutputFilePath),
                Format = "suit", // Set your desired format
                Severable = false, // Set your desired value
                Components = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "install-id", new List<string> { "00" } },
                        { "vendor-id", "1b8ad47db3cc7eb2edb400c04fd430c8" },
                        { "class-id", "44e8bc11daa14ad1ddb400c04fd430c8" },
                        { "file", "C:\\Users\\ousama.charada\\OneDrive - ML!PA Consulting GmbH\\Desktop\\suit\\suit-manifest-generator\\TestCase\\firmware\\d.txt"},
                        { "uri", "coap://[fdea:dbee:f::1]/0" },
                        { "bootable", true },
                        { "offset", 16384 }
                    }
                }

            };
            var jsonData = new Dictionary<string, object>
            {
                { "manifest-version", "1.0" }, // Add the manifest version here
                { "manifest-sequence-number", "1.0" }, // Add the manifest version here

                {
                    "components", new List<Dictionary<string, object>>
                    {
                        // Define your components here following the JSON structure
                        new Dictionary<string, object>
                        {
                            { "install-id", new List<string> { "00" } },
                            { "vendor-id", "1b8ad47db3cc7eb2edb400c04fd430c8" },
                            { "class-id", "44e8bc11daa14ad1ddb400c04fd430c8" },
                            { "file", "C:\\Users\\ousama.charada\\OneDrive - ML!PA Consulting GmbH\\Desktop\\suit\\suit-manifest-generator\\TestCase\\firmware\\d.txt" },
                            { "uri", "coap://[fdea:dbee:f::1]/0" },
                            { "bootable", true },
                            { "offset", 16384 }
                            // Add other component properties if needed
                        },
                        // Add more components if needed
                    }
                }
            };

            // Initialize the ManifestCompiler
            var manifestCompiler = new ManifestCompiler();

            // Compile the manifest
            var nm = manifestCompiler.CompileManifest(options, jsonData);

            // Assert your expectations on the nm object or other outcomes

            // Example: Assert that nm is not null
            Assert.NotNull(nm);
        }
        finally
        {
            if (options != null)
            {
                // Close and delete temporary files
                options.InputFile.Close();
                options.OutputFile.Close();
            }
            File.Delete(tempInputFilePath);
            File.Delete(tempOutputFilePath);
        }
    }
}
*/