namespace API.CryptologyLibrary.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal class AESEncryptor
    {
        private AESBits fEncryptionBits;
        private string fPassword;
        private byte[] fSalt;

        public AESEncryptor(string password, AESBits encryptionBits)
        {
            this.fSalt = new byte[] { 0, 1, 2, 0x1c, 0x1d, 30, 3, 4, 5, 15, 0x20, 0x21, 0xad, 0xaf, 0xa4 };
            this.fPassword = password;
            this.fEncryptionBits = encryptionBits;
        }

        public AESEncryptor(string password, AESBits encryptionBits, byte[] salt)
        {
            this.fSalt = new byte[] { 0, 1, 2, 0x1c, 0x1d, 30, 3, 4, 5, 15, 0x20, 0x21, 0xad, 0xaf, 0xa4 };
            this.fPassword = password;
            this.fEncryptionBits = encryptionBits;
            this.fSalt = salt;
        }

        public string Decrypt(string data)
        {
            byte[] buffer = Convert.FromBase64String(data);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(this.fPassword, this.fSalt);
            switch (this.fEncryptionBits)
            {
                case AESBits.BITS128:
                    return Encoding.Unicode.GetString(this.iDecrypt(buffer, bytes.GetBytes(0x10), bytes.GetBytes(0x10)));

                case AESBits.BITS192:
                    return Encoding.Unicode.GetString(this.iDecrypt(buffer, bytes.GetBytes(0x18), bytes.GetBytes(0x10)));

                case AESBits.BITS256:
                    return Encoding.Unicode.GetString(this.iDecrypt(buffer, bytes.GetBytes(0x20), bytes.GetBytes(0x10)));
            }
            return null;
        }

        public byte[] Decrypt(byte[] data)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(this.fPassword, this.fSalt);
            switch (this.fEncryptionBits)
            {
                case AESBits.BITS128:
                    return this.iDecrypt(data, bytes.GetBytes(0x10), bytes.GetBytes(0x10));

                case AESBits.BITS192:
                    return this.iDecrypt(data, bytes.GetBytes(0x18), bytes.GetBytes(0x10));

                case AESBits.BITS256:
                    return this.iDecrypt(data, bytes.GetBytes(0x20), bytes.GetBytes(0x10));
            }
            return null;
        }

        public string Encrypt(string data)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(data);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(this.fPassword, this.fSalt);
            switch (this.fEncryptionBits)
            {
                case AESBits.BITS128:
                    return Convert.ToBase64String(this.iEncrypt(buffer, bytes.GetBytes(0x10), bytes.GetBytes(0x10)));

                case AESBits.BITS192:
                    return Convert.ToBase64String(this.iEncrypt(buffer, bytes.GetBytes(0x18), bytes.GetBytes(0x10)));

                case AESBits.BITS256:
                    return Convert.ToBase64String(this.iEncrypt(buffer, bytes.GetBytes(0x20), bytes.GetBytes(0x10)));
            }
            return null;
        }

        public byte[] Encrypt(byte[] data)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(this.fPassword, this.fSalt);
            switch (this.fEncryptionBits)
            {
                case AESBits.BITS128:
                    return this.iEncrypt(data, bytes.GetBytes(0x10), bytes.GetBytes(0x10));

                case AESBits.BITS192:
                    return this.iEncrypt(data, bytes.GetBytes(0x18), bytes.GetBytes(0x10));

                case AESBits.BITS256:
                    return this.iEncrypt(data, bytes.GetBytes(0x20), bytes.GetBytes(0x10));
            }
            return null;
        }

        private byte[] iDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream stream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = key;
            rijndael.IV = iv;
            CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(data, 0, data.Length);
            stream2.Close();
            return stream.ToArray();
        }

        private byte[] iEncrypt(byte[] data, byte[] key, byte[] iV)
        {
            MemoryStream stream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = key;
            rijndael.IV = iV;
            CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(data, 0, data.Length);
            stream2.Close();
            return stream.ToArray();
        }

        public AESBits EncryptionBits
        {
            get
            {
                return this.fEncryptionBits;
            }
            set
            {
                this.fEncryptionBits = value;
            }
        }

        public string Password
        {
            get
            {
                return this.fPassword;
            }
            set
            {
                this.fPassword = value;
            }
        }

        public byte[] Salt
        {
            get
            {
                return this.fSalt;
            }
            set
            {
                this.fSalt = value;
            }
        }
    }
}
