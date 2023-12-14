using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SuitSolution.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the full path to the source directory:");
        string sourceDirectory = Console.ReadLine();

        const string SUIT_COAP_SERVER = "[fdea:dbee:f::1]";
        const string APP_VER = "1";
        const string MLPA_PUBLISH_DIR = "firmware";
        string destinationDirectory = Path.Combine(sourceDirectory, MLPA_PUBLISH_DIR);

        // Copy firmware files to MLPA_PUBLISH_DIR
        CryptoHelper.CopyFirmwareFiles(sourceDirectory, destinationDirectory);

        // Replace the data with the specified values
        string urlRoot = $"coap://{SUIT_COAP_SERVER}/fw/{APP_VER}/d";
        int seqnr = 12345; // Replace with the actual sequence number
        string SUIT_VENDOR = "ml-pa.com";
        string SUIT_CLASS = "lhb_mic40";
        List<string> slotFiles = new List<string>
        {
            Path.Combine(destinationDirectory, "0:0x4000"),
            Path.Combine(destinationDirectory, "1:0x82000")
        };
        string keyPath = "decrypted_key.pem";
        string encryptionpass = "StrongPasswordsAreHardToFind#";

        // Create an instance of SuitManifestProcessor
        var processor = new SuitManifestProcessor(sourceDirectory,urlRoot, seqnr, SUIT_VENDOR, SUIT_CLASS, slotFiles, keyPath, encryptionpass);

        // Run the SuitManifestProcessor
        processor.ProcessSUITFileSigning();

        Console.WriteLine("Processing completed!");
    }
}