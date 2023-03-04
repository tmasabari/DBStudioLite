using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class DataSecure
{
    public static string ReadFile(string sFile)
    {
        string sData = "";
        try
        {
            using (StreamReader objfile = new StreamReader(sFile))
            {
                sData = objfile.ReadToEnd();
                sData = DataSecure.DecryptString(sData);
            }
        }
        catch
        {
        }
        return sData;
    }
    public static void WriteFile(string sFile, string data)
    {
        using (StreamWriter objfile = new StreamWriter(sFile))
        {
            objfile.Write(DataSecure.EncryptString(data));
        }

    }
    private static string key = ConfigurationManager.AppSettings.Get("Encryptionkey");
    public static string EncryptString(string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }
    public static string DecryptString(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

}

////Reader

#region Security

//rc4encrypt rc4 = new rc4encrypt();
//rc4.Password = "seigolonhcet medloh cigam";
//rc4.PlainText = sData;
//rc4.EnDeCrypt();

//string rc4Decrypted = "";
//string strrc4 = System.Text.Encoding.Default.GetString(byteCon);
//rc4encrypt rc4 = new rc4encrypt();
//rc4.Password = "seigolonhcet medloh cigam";
//rc4.PlainText = strrc4;
//rc4Decrypted = rc4.EnDeCrypt();
#endregion