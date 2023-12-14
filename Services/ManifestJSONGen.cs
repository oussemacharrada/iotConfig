using SuitSolution.Services.Utility;

namespace SuitSolution.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;



public class ManifestJSONGen
{
    
  static async Task Func(string[] args)
        {
            var parsedArgs = new ParsedArguments
            {
                UrlRoot = "coap://[fdea:dbee:f::1]",
                Seqnr = 1,
                Output = "out.json",
                UuidVendor = "mlpa.com",
                UuidClass = "board",
                SlotFiles = new List<string>
                {
                    "firmware/0:0x4000",
                    "firmware/1:0x82000"
                }
            };

            await GenerateManifest(parsedArgs);
        }

  public static async Task<Dictionary<string, object>> GenerateManifest(ParsedArguments parsedArgs)
  {
      var uuidVendor = CreateUuidNamespaceDns(parsedArgs.UuidVendor);
      var uuidClass = CreateUuidNamespaceDns(uuidVendor, parsedArgs.UuidClass);

      var template = new Dictionary<string, object>
      {
          { "manifest-version", 1 },
          { "manifest-sequence-number", parsedArgs.Seqnr },
          { "components", new List<Dictionary<string, object>>() }
      };

      var images = new List<(string Filename, int Offset, List<string> CompName)>();

      foreach (var filenameOffset in parsedArgs.SlotFiles)
      {
          var compName = new List<string> { "00" };
          var split = filenameOffset.Split(':');
          if (split.Length >= 2)
          {
              var (filename, offset) = (split[0]+":"+split[1], StrToInt(split[2]));
              if (split.Length == 3)
              {
                  compName = new List<string> { split[2] };
              }
              images.Add((filename, offset, compName));
          }
      }

      foreach (var (slot, image) in images.Select((value, i) => (i, value)))
      {
          var (filename, offset, compName) = image;
          var uri = UriUtility.GetUriWithFileName(parsedArgs.UrlRoot, filename);

          var component = new Dictionary<string, object>
          {
              { "install-id", compName },
              { "vendor-id", uuidVendor.ToString("N") },
              { "class-id", uuidClass.ToString("N") },
              { "file", filename },
              { "uri", uri },
              { "bootable", true }
          };

          if (offset != 0)
          {
              component.Add("offset", offset);
          }

          ((List<Dictionary<string, object>>)template["components"]).Add(component);
      }

      return template;
  }
        static int StrToInt(string x)
        {
            if (x.StartsWith("0x"))
            {
                if (int.TryParse(x.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int result))
                {
                    return result;
                }
                else
                {
                    throw new ArgumentException("Hexadecimal value is not in the correct format.", nameof(x));
                }
            }
            else
            {
                if (int.TryParse(x, out int result))
                {
                    return result;
                }
                else
                {
                    throw new ArgumentException("Integer value is not in the correct format.", nameof(x));
                }
            }
        }

        static Guid CreateUuidNamespaceDns(string name)
        {
            var generator = new UniqueIdGenerator();
            return generator.Generate(name);
        }

        static Guid CreateUuidNamespaceDns(Guid namespaceId, string name)
        {
            var generator = new UniqueIdGenerator();
            return generator.Generate($"{namespaceId}{name}");
        }
    }

public class ParsedArguments
  {
      public string UrlRoot { get; set; }
      public int Seqnr { get; set; }
      public string Output { get; set; }
      public string UuidVendor { get; set; }
      public string UuidClass { get; set; }
      public List<string> SlotFiles { get; set; }
  }

  class ManifestTemplate
  {
      public int ManifestVersion { get; set; }
      public int ManifestSequenceNumber { get; set; }
      public List<Component> Components { get; set; }
  }

  class Component
  {
      [JsonPropertyName("install-id")]  
      public List<string> InstallID { get; set; }
      public string VendorId { get; set; }
      public string ClassId { get; set; }
      public string File { get; set; }
      public string Uri { get; set; }
      [JsonPropertyName("bootable")]
      public bool Bootable { get; set; }
      public int? Offset { get; set; }
  }
  static class UriUtility
  {
      public static string GetUriWithFileName(string baseUrl, string filePath)
      {
          return $"{baseUrl.TrimEnd('/')}/{Path.GetFileName(filePath)}";
      }
  }
    static class GuidUtility
    {
        public static Guid Create(Guid namespaceId, string name)
        {
            byte[] namespaceBytes = namespaceId.ToByteArray();
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);

            if (nameBytes.Length < 16)
            {
                byte[] paddedNameBytes = new byte[16];
                Array.Copy(nameBytes, paddedNameBytes, nameBytes.Length);
                nameBytes = paddedNameBytes;
            }

            for (int i = 0; i < 16; i++)
            {
                namespaceBytes[i] = (byte)(namespaceBytes[i] ^ nameBytes[i]);
            }

            return new Guid(namespaceBytes);
        }

        public static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
    }
