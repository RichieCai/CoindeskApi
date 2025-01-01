namespace CoindeskApi.Encryption
{
    public interface IAesEncryptionService
    {
        string Encrypt(string plaintext);
        string Decrypt(string ciphertext);
    }
}
