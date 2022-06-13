using System;
using System.IO;
using System.Text;

namespace QMA.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to a (memory) stream using UTF8 encoding
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns></returns>
        public static Stream ToStream(this string value) => ToStream(value, Encoding.UTF8);

        /// <summary>
        /// Converts a string to a (memory) stream
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="encoding">The encoding to use for the conversion</param>
        /// <returns></returns>
        public static Stream ToStream(this string value, Encoding encoding)
            => new MemoryStream(encoding.GetBytes(value ?? string.Empty));
    }
}
