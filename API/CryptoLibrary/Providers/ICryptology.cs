

namespace API.CryptologyLibrary.Providers
{

    public interface ICryptology
    {
        string Decrypt(string textToDecrypt);
        string Encrypt(string textToEncrypt);
    }
}
