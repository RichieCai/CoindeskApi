using CoindeskApi.Models.Data;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MyCommon.Encryption
{
    public class AesEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        public AesEncryptionService(IOptions<EncryptionSettings> options)
        {
            var settings = options.Value;
            if (string.IsNullOrEmpty(settings.SymmetricKey) || string.IsNullOrEmpty(settings.SymmetricIv))
            {
                throw new ArgumentNullException(nameof(settings.SymmetricKey), "Symmetric key is missing in configuration.");
            }

            _key = Encoding.UTF8.GetBytes(settings.SymmetricKey.PadRight(32).Substring(0, 32));
            _iv = Encoding.UTF8.GetBytes(settings.SymmetricIv.PadRight(16).Substring(0, 16));

            Console.WriteLine($"SymmetricKey: {_key.Length} bytes, IV: {_iv.Length} bytes");
        }
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using (var ms = new MemoryStream())
            {
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
