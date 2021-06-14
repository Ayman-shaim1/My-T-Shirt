using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace My_T_Shirt.Helpers
{
    public class Cryptage
    {
        public static string Salt { get; private set; }

        static string hashKey = "HGFY001937!90";
        public static byte[] GetHashKey()
        {
            // Initialize
            UTF8Encoding encoder = new UTF8Encoding();
            // Get the salt
            string salt = !string.IsNullOrEmpty(Salt) ? Salt : "I am a nice little salt";
            byte[] saltBytes = encoder.GetBytes(salt);
            // Setup the hasher
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(hashKey, saltBytes);
            // Return the key
            return rfc.GetBytes(16);
        }

        public static string Encrypt(string dataToEncrypt)
        {
            // Initialize
            AesManaged encryptor = new AesManaged();
            // Set the key
            encryptor.Key = GetHashKey();
            encryptor.IV = GetHashKey();
            // create a memory stream
            using (MemoryStream encryptionStream = new MemoryStream())
            {
                // Create the crypto stream
                using (CryptoStream encrypt = new CryptoStream(encryptionStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt
                    byte[] utfD1 = UTF8Encoding.UTF8.GetBytes(dataToEncrypt);
                    encrypt.Write(utfD1, 0, utfD1.Length);
                    encrypt.FlushFinalBlock();
                    encrypt.Close();
                    // Return the encrypted data
                    return Convert.ToBase64String(encryptionStream.ToArray());
                }
            }
        }
        public static string Decrypt( string encryptedString)
        {
            // Initialize
            AesManaged decryptor = new AesManaged();
            byte[] encryptedData = Convert.FromBase64String(encryptedString);
            // Set the key
            decryptor.Key = GetHashKey();
            decryptor.IV = GetHashKey();
            // create a memory stream
            using (MemoryStream decryptionStream = new MemoryStream())
            {
                // Create the crypto stream
                using (CryptoStream decrypt = new CryptoStream(decryptionStream, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt
                    decrypt.Write(encryptedData, 0, encryptedData.Length);
                    decrypt.Flush();
                    decrypt.Close();
                    // Return the unencrypted data
                    byte[] decryptedData = decryptionStream.ToArray();
                    return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                }
            }
        }



    }
}