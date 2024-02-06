using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.CryptologyLibrary.Services
{
    using API.CryptologyLibrary.Providers;
    using System;

    public class CryptologyServiceLocator
    {
        public static ICryptology CryptologyProvider(string PasswordToEncrypt)
        {
            var s = "";
            return new AESProvider(PasswordToEncrypt);
        }
    }
}
