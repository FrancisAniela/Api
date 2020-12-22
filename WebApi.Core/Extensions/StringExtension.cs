using System;
using System.Text;

namespace WebApi.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string data)
        {
            return string.IsNullOrEmpty(data);
        }

        public static bool IsNullOrWhiteSpace(this string data)
        {
            return string.IsNullOrWhiteSpace(data);
        }

        public static string ToBase64(this string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        public static string FromBase64(this string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        public static string UppercaseFirst(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            char[] a = data.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }


    }
}
