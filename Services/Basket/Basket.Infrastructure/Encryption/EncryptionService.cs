namespace Basket.Infrastructure.Encryption;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] key = System.Text.Encoding.UTF8.GetBytes("ASD-ASDF-GAALLL@");
    private readonly byte[] iv = new byte[16];

    public string Encrypt(string plainText)
    {
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream();
        using (var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
        using (var sw = new System.IO.StreamWriter(cs))
        {
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();
        }
        return Convert.ToBase64String(ms.ToArray());
    }


    public string Decrypt(string cipherText)
    {
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);
        using var sr = new System.IO.StreamReader(cs);
        return sr.ReadToEnd();
    }
}
