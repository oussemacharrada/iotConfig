using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    public static byte[] DecryptPrivateKey(byte[] encryptedKeyData, string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        const int iterations = 10000; // Adjust the number of iterations based on your security requirements
        using (var deriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt: null, iterations))
        {
            byte[] key = deriveBytes.GetBytes(32); // 256 bits
            byte[] iv = deriveBytes.GetBytes(16); // 128 bits

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msDecrypt = new MemoryStream(encryptedKeyData))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream and return them
                                return Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }
    }

    public static void CopyFirmwareFiles(string sourceDirectory, string destinationDirectory)
    {
        // Ensure the destination directory exists
        Directory.CreateDirectory(destinationDirectory);

        // Get a list of firmware files in the source directory
        string[] firmwareFiles = Directory.GetFiles(sourceDirectory, "hwr-0_fwt-13_fwr-1_fws-*.bin");

        // Copy each firmware file to the destination directory with names "0" and "1"
        for (int i = 0; i < firmwareFiles.Length; i++)
        {
            string destinationFileName = Path.Combine(destinationDirectory, i.ToString());

            // Delete the file if it already exists
            if (File.Exists(destinationFileName))
            {
                File.Delete(destinationFileName);
            }

            // Copy the firmware file to the destination directory
            File.Copy(firmwareFiles[i], destinationFileName);
        }
    }

}
