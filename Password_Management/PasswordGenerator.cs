using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Generate_Password
{
    public class PasswordGenerator
    {
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NumericChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_+=";


        public static string GeneratePassword(int length, bool isUppercaseChars, bool includeNumbers, bool includeSpecialChars, int method_type = 0)
        {
            string chars = LowercaseChars + (isUppercaseChars ? UppercaseChars : "");
            if (includeNumbers)
                chars += NumericChars;
            if (includeSpecialChars)
                chars += SpecialChars;

            char[] result = new char[length];

            switch (method_type)
            {
                case 0:
                    byte[] data = new byte[length];
                    using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                    {
                        crypto.GetBytes(data);
                    }
                    for (int i = 0; i < length; i++)
                    {
                        result[i] = chars[data[i] % chars.Length];
                    }
                    break;

                case 1:
                    Random random = new Random();
                    for (int i = 0; i < length; i++)
                    {
                        result[i] = chars[random.Next(chars.Length)];
                    }
                    break;
            }

            return new string(result);
        }
    }
}
