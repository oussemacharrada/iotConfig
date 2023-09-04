using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Formats.Cbor;
using Dahomey.Cbor;
using PeterO.Cbor;

namespace SuitTool
{
    class Program
    {
        static int Main(string[] args)
        {

            string filePath =
                "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/suit/suit-manifest-generator/TestCase/hwr-0_fwt-13_fwr-1unsigned.mani"; // Replace with your hardcoded file path

            try
            {
                // Read all the bytes from a file and decode the CBOR object
                // from it.  However, there are two disadvantages to this approach:
                // 1.  The byte array might be very huge, so a lot of memory to store
                // the array may be needed.
                // 2.  The decoding will succeed only if the entire array,
                // not just the start of the array, consists of a CBOR object.
                var cbor = CBORObject.DecodeFromBytes(
                    File.ReadAllBytes(filePath), CBOREncodeOptions.Default);
                byte[] data = new byte[] { 0xD8, 0x21, 0x78, 0x1E, 0x61, 0x48, 0x52, 0x30, 0x63, 0x44, 0x6F, 0x76, 0x4C, 0x33, 0x64, 0x33, 0x64, 0x79, 0x35, 0x6C, 0x65, 0x47, 0x46, 0x74, 0x63, 0x47, 0x78, 0x6C, 0x4C, 0x6D, 0x4E, 0x76, 0x62, 0x51 };
                CBORDecoder decoder = new CBORDecoder(data);
                var f = decoder.ReadItem();
                Assert.IsInstanceOf<Uri>(f);
                    return 0;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
            catch (CBORException ex)
            {
                Console.WriteLine($"CBOR parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            return 1;
        }

        static bool UseJsonOutput(string[] args)
        {
            return args.Length >= 2 && args[1] == "--json";
        }



    }



    static class TextWrapper
    {
        public static IEnumerable<string> Wrap(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                yield return string.Empty;
                yield break;
            }

            string[] words = text.Split(' ');

            StringBuilder line = new StringBuilder();
            foreach (string word in words)
            {
                if (line.Length + word.Length <= maxLength)
                {
                    line.Append(word).Append(' ');
                }
                else
                {
                    yield return line.ToString().TrimEnd();
                    line.Clear().Append(word).Append(' ');
                }
            }

            if (line.Length > 0)
            {
                yield return line.ToString().TrimEnd();
            }
        }
    }
}

