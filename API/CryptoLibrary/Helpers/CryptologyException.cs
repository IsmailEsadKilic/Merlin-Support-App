namespace API.CryptologyLibrary.Helpers
{
    using System;

    public class CryptologyException : ApplicationException
    {
        public CryptologyException()
        {
        }

        public CryptologyException(string message) : base(message)
        {
        }
    }
}
