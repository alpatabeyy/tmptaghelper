using System.Collections.Generic;
using System.Text;

namespace TMPTagHelper.Scripts.Runtime
{
    public static class TextMeshProTagHelper
    {
        private static readonly List<StringBuilder> StringBuilders = new List<StringBuilder>();
        
        public static TextMeshProTagBuilder CreateTagBuilder(this string text)
        {
            if (StringBuilders.Count <= 0)
            {
                return GenerateNewBuilder(text);
            }

            foreach (StringBuilder stringBuilder in StringBuilders)
            {
                return stringBuilder.Length <= 0
                    ? new TextMeshProTagBuilder(stringBuilder.Append(text))
                    : GenerateNewBuilder(text);
            }

            return GenerateNewBuilder(text);
        }

        private static TextMeshProTagBuilder GenerateNewBuilder(string text)
        {
            StringBuilder generatedStringBuilder = new StringBuilder(text);
            StringBuilders.Add(generatedStringBuilder);
            return new TextMeshProTagBuilder(generatedStringBuilder);
        }
    }
}