namespace API.CryptologyLibrary.Providers
{
    using API.CryptologyLibrary.Helpers;

    internal class AESProvider : ICryptology
    {
        private const string EXTRAPASSWORD = "MERLİN;P2wtW1gw";
        private string password;

        public AESProvider(string password)
        {
            this.Password = password + "MERLİN;P2wtW1gw";            
        }

        public string Decrypt(string textToDecrypt)
        {
            string str;
            try
            {
                str = new AESEncryptor(this.Password, AESBits.BITS256).Decrypt(textToDecrypt);
            }
            catch
            {
                throw new CryptologyException("Error when decrypting");
            }
            return str;
        }

        public string Encrypt(string textToEncrypt)
        {
            string str;
            try
            {
                str = new AESEncryptor(this.Password, AESBits.BITS256).Encrypt(textToEncrypt);
            }
            catch
            {
                throw new CryptologyException("Error when encrypting");
            }
            return str;
        }

        private string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }
    }
}
