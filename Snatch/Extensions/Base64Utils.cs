using System;
using System.Linq;

namespace Snatch
{
    class Base64Utils
    {
        #region Judge if a string is a base64 string

        /// <summary>
        /// Judge if it is a base64 string
        /// </summary>
        /// <param name="base64Str">The string to be judged</param>
        /// <returns></returns>
        public static bool IsBase64(string base64Str)
        {
            byte[] bytes = null;
            return IsBase64(base64Str, out bytes);
        }

        /// <summary>
        /// Judge if it is a base64 string
        /// </summary>
        /// <param name="base64Str">Base64 string</param>
        /// <param name="bytes">String byte arrays</param>
        /// <returns></returns>
        private static bool IsBase64(string base64Str, out byte[] bytes)
        {
            //string strRegex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
            bytes = null;
            if (string.IsNullOrEmpty(base64Str))
                return false;
            else
            {
                if (base64Str.Contains(","))
                    base64Str = base64Str.Split(',')[1];
                if (base64Str.Length % 4 != 0)
                    return false;
                if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                    return false;
            }

            try
            {
                bytes = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static char[] base64CodeArray = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', '='
        };

        #endregion Judge if a string is a base64 string

        /// <summary>
        /// Get file suffix (image format) based on base64 string
        /// </summary>
        /// <param name="base64Str">Base64 string</param>
        /// <returns></returns>
        public static string GetSuffixFromBase64Str(string base64Str)
        {
            string suffix = string.Empty;
            string prefix = "data:image/";
            if (base64Str.StartsWith(prefix) && base64Str.Contains(";") && base64Str.Contains(","))
            {
                base64Str = base64Str.Split(';')[0];
                suffix = base64Str.Substring(prefix.Length);
            }

            return suffix;
        }
    }
}