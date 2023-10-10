using System;
using System.IO;
using System.Text;
using PeterO.Cbor;
using Xunit;

public class SUITEnvelopeTests
{
    [Fact]
    public void TestToSUITAndSaveToFile()
    {
        // Create a sample SUITEnvelope instance
        SUITEnvelope envelope = new SUITEnvelope();
        envelope = SUITEnvelopeSeeder.GenerateSUITEnvelope();

        // Define a temporary file path
        string filePath = "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/suit/suit-manifest-generator/TestCase/object.cbor"; // Change the file extension to .cbor

        try
        {
            // Call the ToSUIT method to generate CBOR data as a CBORObject
            Dictionary<string, object> cborObject = envelope.ToSUIT();
            CBORObject cbordata = CBORObject.FromObject(cborObject);
            // Save the CBOR data to a file with .cbor extension
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                byte[] cborBytes = cbordata.EncodeToBytes();
                fs.Write(cborBytes, 0, cborBytes.Length);
            }

            // Ensure that the file exists
            Assert.True(File.Exists(filePath));

            // You can add additional assertions here to validate the saved CBOR data if needed
        }
        catch (Exception ex)
        {
            // Handle the exception, or log the error message for debugging
            Console.WriteLine("Error creating the file: " + ex.Message);
        }
    }
}