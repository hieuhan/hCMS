using System;
using System.Text;
using System.Security.Cryptography;

namespace LibUtils
{
    public class md5
    {
        public static string getMd5(string pass)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            byte[] array = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(pass));
            return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(pass)));
        }

        public string CalculateMD5Hash(string input)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string MD5Hash(string text)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            mD.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] hash = mD.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static string MD5HashHas(string text)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            mD.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] hash = mD.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
