using System;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class StringUtils
    {
        private static string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GetRandomUserName(int length = 7)
        {
            string tempUsername = String.Empty;
            for (int i = 0; i < length; i++)
            {
                tempUsername += chars[Random.Range(0, chars.Length - 1)];
            }

            return tempUsername;
        }
    }
}