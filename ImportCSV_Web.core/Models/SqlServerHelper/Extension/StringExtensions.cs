using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerHelper.Core.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to PascalCase
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns></returns>
        public static String ToPascalCase(this String str)
        {
            // Replace all non-letter and non-digits with an underscore and lowercase the rest.
            String sample = String.Join(String.Empty, str?.Select(c => Char.IsLetterOrDigit(c) ? c.ToString().ToLower() : "_").ToArray());

            // Split the resulting string by underscore
            // Select first character, uppercase it and concatenate with the rest of the string
            var arr = sample?
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => $"{s.Substring(0, 1).ToUpper()}{s.Substring(1)}");

            // Join the resulting collection
            sample = String.Join(String.Empty, arr);

            return sample;
        }

        /// <summary>
        /// To the camel case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static String ToCamelCase(this String str)
        {
            return ToLowerCaseFirst(ToPascalCase(str));
        }

        /// <summary>
        /// To the lower case first.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static String ToLowerCaseFirst(this String s)
        {
            if (String.IsNullOrEmpty(s)) { return String.Empty; }

            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);

            return new String(a);
        }

        /// <summary>
        /// Splitbies the case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static String SplitbyCase(this String str)
        {
            char[] chars = str.ToCharArray();

            List<Char> charList = new List<Char>();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i + 1 >= chars.Length)
                {
                    charList.Add(chars[i]);
                    continue;
                }

                if (char.IsUpper(chars[i]) && char.IsLower(chars[i + 1])) { charList.Add(' '); }

                charList.Add((char.IsLower(chars[i + 1]) ? chars[i] : char.ToLower(chars[i])));
            }

            return new string(charList.ToArray());
        }
    }
}
