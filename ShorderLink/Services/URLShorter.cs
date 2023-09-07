using System.Text;
using XSystem.Security.Cryptography;

namespace ShorderLink.Services
{
    public class URLShorter
    {
        public static string HashString(string text)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);

            char[] hash2 = new char[16];

            for (int i = 0; i < hash2.Length; i++)
            {
                hash2[i] = chars[hash[i] % chars.Length];
            }

            return new string(hash2);
        }
    }
}
