using System.Text.RegularExpressions;

namespace BluePrism.TechTest.Library.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidWord(this string word, int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Supplied length must be greater than 0");
            
            if (string.IsNullOrWhiteSpace(word)) return false;

            return Regex.IsMatch(word, $@"^\b[A-Za-z]{{1}}[a-z]{{{length - 1}}}\b");
        }
    }
}
