using System;

public class DataSecure
{

    public  string  GetEncryptedData(string sData)
    {
        byte[] byteCon = new byte[sData.Length];
        byteCon = System.Text.Encoding.Default.GetBytes(sData);
        return Convert.ToBase64String(byteCon);
    }

    public string GetDecryptedData(string sData)
    {   string sConverted ="";
        try {
            byte[] byteCon = Convert.FromBase64String(sData);
            sConverted =System.Text.Encoding.Default.GetString(byteCon);
        }
        catch { sConverted = sData; }  // never mind invalid sData. return original value
        return sConverted;
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